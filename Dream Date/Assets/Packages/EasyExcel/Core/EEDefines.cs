using System;
using UnityEngine;

namespace EasyExcel
{
	/// <summary>
	///     One row in a excel sheet.
	/// </summary>
	[Serializable]
	public abstract class EERowData
	{
		public object GetKeyFieldValue()
		{
			var keyField = EEUtility.GetRowDataKeyField(GetType());
			return keyField == null ? null : keyField.GetValue(this);
		}
	}

	/// <summary>
	///     All RowData in an excel sheet
	/// </summary>
	public abstract class EERowDataCollection : ScriptableObject
	{
		public string ExcelFileName;
		public string ExcelSheetName;
		public string KeyFieldName;
		public abstract void AddData(EERowData data);
		public abstract int GetDataCount();
		public abstract EERowData GetData(int index);
	}
	
	public static class EEConstant
	{
		public const string Version = "3.0";
	}

	/// <summary>
	/// 	Mark which field of class is key
	/// </summary>
	public class EEKeyFieldAttribute : Attribute
	{
		
	}
	
}