using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EasyExcel
{
	/// <summary>
	///     Excel Converter
	/// </summary>
	public static partial class EEConverter
	{
		public static void GenerateScriptableObjects(string xlsxPath, string assetPath)
		{
			try
			{
				xlsxPath = xlsxPath.Replace("\\", "/");
				assetPath = assetPath.Replace("\\", "/");

				if (!Directory.Exists(xlsxPath))
				{
					EditorUtility.DisplayDialog("EasyExcel", "Xls/xlsx path doesn't exist.", "OK");
					return;
				}

				if (!Directory.Exists(assetPath))
				{
					/*var opt = EditorUtility.DisplayDialogComplex("EasyExcel",
						string.Format(
							"EasyExcelSettings AssetPath doesn't exist. Would you like to create it?\n{0}",
							assetPath),
						"Create", "Cancel", "Quit");
					switch (opt)
					{
						case 0:*/
							Directory.CreateDirectory(assetPath);
							/*break;
						case 1:
						case 2:
							return;
					}*/
				}

				xlsxPath = xlsxPath.Replace("\\", "/");
				assetPath = assetPath.Replace("\\", "/");
				if (!assetPath.EndsWith("/"))
					assetPath += "/";
				if (Directory.Exists(assetPath))
					Directory.Delete(assetPath, true);
				Directory.CreateDirectory(assetPath);

				var filePaths = Directory.GetFiles(xlsxPath);
				var count = 0;
				for (var i = 0; i < filePaths.Length; ++i)
				{
					var filePath = filePaths[i].Replace("\\", "/");
					if (!IsExcelFile(filePath)) continue;
					UpdateProgressBar(i, filePaths.Length, "");
					ToScriptableObject(filePath, assetPath);
					count++;
				}

				EELog.Log("Assets are generated successfully.");

				ClearProgressBar();
				AssetDatabase.Refresh();
				EELog.Log(string.Format("Import done. {0} sheets were imported.", count));
			}
			catch (Exception e)
			{
				EELog.LogError(e.ToString());
				ClearProgressBar();
				AssetDatabase.Refresh();
			}
		}
		
		private static void ToScriptableObject(string excelPath, string outputPath)
		{
			try
			{
				var book = EEWorkbook.Load(excelPath);
				if (book == null)
					return;
				foreach (var sheet in book.sheets)
				{
					if (sheet == null)
						continue;
					if (!IsValidSheet(sheet))
						continue;
					//var sheetData = ToSheetData(sheet);
					var sheetData = ToSheetDataRemoveEmptyColumn(sheet);
					ToScriptableObject(excelPath, sheet.name, outputPath, sheetData);
				}
			}
			catch (Exception e)
			{
				EELog.LogError(e.ToString());
				AssetDatabase.Refresh();
			}
		}

		private static void ToScriptableObject(string excelPath, string sheetName, string outputPath, SheetData sheetData)
		{
			try
			{
				var sheetClassName = EESettings.Current.GetSheetClassName(sheetName);
				var asset = ScriptableObject.CreateInstance(sheetClassName);
				var dataCollect = asset as EERowDataCollection;
				if (dataCollect == null)
					return;
				dataCollect.ExcelFileName = Path.GetFileName(excelPath);
				dataCollect.ExcelSheetName = sheetName;
				var className = EESettings.Current.GetRowDataClassName(sheetName, true);
				var dataType = Type.GetType(className);
				if (dataType == null)
				{
					var asmb = Assembly.LoadFrom(Environment.CurrentDirectory + "/Library/ScriptAssemblies/Assembly-CSharp.dll");
					dataType = asmb.GetType(className);
				}
				if (dataType == null)
				{
					EELog.LogError(className + " not exist !");
					return;
				}

				//var dataCtor = dataType.GetConstructor(Type.EmptyTypes);
				var dataCtor = dataType.GetConstructor(new []{typeof(List<List<string>>), typeof(int), typeof(int)});
				if (dataCtor == null)
					return;
				var keySet = new HashSet<object>();
				for (var row = EESettings.Current.DataStartIndex; row < sheetData.RowCount; ++row)
				{
					for (var col = 0; col < sheetData.ColumnCount; ++col)
						sheetData.Set(row, col, sheetData.Get(row, col).Replace("\n", "\\n"));

					var inst = dataCtor.Invoke(new object[]{sheetData.Table, row, 0}) as EERowData;
					if (inst == null)
						continue;
					//inst._init(sheetData.Table, row, 0);
					var key = inst.GetKeyFieldValue();
                    if (key == null)
                    {
						EELog.LogError("The value of key is null in sheet " + sheetName);
						continue;
					}

                    if (string.IsNullOrEmpty(key.ToString()) || key.ToString() == "0") continue;
                   
					if (!keySet.Contains(row)) //这里修改了 key => row
                    {
						dataCollect.AddData(inst);
						keySet.Add(row); //这里修改了 key => row
                    }
					else
						EELog.LogError(string.Format("More than one rows have the same Key [{0}] in Sheet {1} row {2}", key, sheetName, row));
				}

				var keyField = EEUtility.GetRowDataKeyField(dataType);
				if (keyField != null)
					dataCollect.KeyFieldName = keyField.Name;

				var itemPath = outputPath + EESettings.GetAssetFileName(sheetName);
				itemPath = itemPath.Substring(itemPath.IndexOf("Assets", StringComparison.Ordinal));
				AssetDatabase.CreateAsset(asset, itemPath);

				AssetDatabase.Refresh();
			}
			catch (Exception ex)
			{
				EELog.LogError(ex.ToString());
			}
		}

	}
}