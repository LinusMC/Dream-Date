using System;
using System.IO;
using UnityEngine;

namespace EasyExcel
{
	[CreateAssetMenu(fileName = "New EasyExcel Settings", menuName = "EasyExcel/Settings", order = 999)]
	public class EESettings : ScriptableObject
	{
		private const string EESettingsSavedFileName = "EasyExcelSettings";
		private const string EESettingsFileExtension = ".asset";
		private const string EESettingsSavedPath = "Assets/Resources/" + EESettingsSavedFileName + EESettingsFileExtension;
		
		[EEComment("This row in a excel sheet is Name (starting from 0)")]
		public int NameRowIndex;
		
		[EEComment("This row in a excel sheet is Type (starting from 0)")]
		public int TypeRowIndex;
		
		[EEComment("This row in a excel sheet is where real data starts (starting from 0)")]
		public int DataStartIndex;
		
		[EEComment("This is where the generated ScriptableObject files will be")]
		public string GeneratedAssetPath;
		
		[EEComment("This is where the generated csharp files will be")]
		public string GeneratedScriptPath;
		
		[EEComment(@"Postfix of generated data table classes")]
		public string SheetDataPostfix;
		
		[EEComment(@"Postfix of generated row data classes")]
		public string RowDataPostfix;

		[EEComment("The namespace of generated classes from excel files")]
		public string NameSpace;

		[NonSerialized] public bool ShowHelp = true;

		public void ResetAll()
		{
			NameRowIndex = 0;
			TypeRowIndex = 1;
			DataStartIndex = 2;
			GeneratedAssetPath = "Assets/Resources/EasyExcelGeneratedAsset/";
			GeneratedScriptPath = "Assets/Scripts/EasyExcelScripts";
			SheetDataPostfix = "_Generated";
			RowDataPostfix = "";
			NameSpace = "EasyExcelGenerated";
		}

		#region Sigleton

		private static EESettings _current;
		
		public static EESettings Current
		{
			get
			{
				if (_current != null) return _current;
				_current = Resources.Load<EESettings>(EESettingsSavedFileName);
				if (_current != null) return _current;
				_current = CreateInstance<EESettings>();
				_current.ResetAll();
#if UNITY_EDITOR
				var resourcesPath = Application.dataPath + "/Resources";
				if (!Directory.Exists(resourcesPath))
				{
					Directory.CreateDirectory(resourcesPath);
					UnityEditor.AssetDatabase.Refresh();
				}

				UnityEditor.AssetDatabase.CreateAsset(_current, EESettingsSavedPath);
#endif
				return _current;
			}
		}

		#endregion

		public string GetRowDataClassName(string sheetName, bool includeNameSpace = false)
		{
			return (includeNameSpace? NameSpace + "." : null) + sheetName + RowDataPostfix;
		}

		public string GetSheetClassName(string sheetName)
		{
			return sheetName + SheetDataPostfix;
		}
		
		public string GetSheetInspectorClassName(string sheetName)
		{
			return sheetName + "Inspector" + SheetDataPostfix;
		}

		public static string GetAssetFileName(string sheetName)
		{
			return sheetName + EESettingsFileExtension;
		}
		
		public string GetCSharpFileName(string sheetName)
		{
			// The file name must not differ from the name of ScriptableObject class
			return GetSheetClassName(sheetName) + ".cs";
		}
		
		public string GetSheetInspectorFileName(string sheetName)
		{
			return GetSheetInspectorClassName(sheetName) + ".cs";
		}
	}
}