﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EasyExcel.
//     Runtime Version: 3.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using EasyExcel;

namespace EasyExcelGenerated
{
	[Serializable]
	public class Localization_TW : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private string _ID;
		public string ID { get { return _ID; } }

		[SerializeField]
		private string _value;
		public string value { get { return _value; } }


		public Localization_TW()
		{
		}

#if UNITY_EDITOR
		public Localization_TW(List<List<string>> sheet, int row, int column)
		{
			_ID = sheet[row][column++] ?? "";
			_value = sheet[row][column++] ?? "";
		}
#endif
	}

	public class Localization_TW_Generated : EERowDataCollection
	{
		[SerializeField]
		private List<Localization_TW> elements = new List<Localization_TW>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as Localization_TW);
		}

		public override int GetDataCount()
		{
			return elements.Count;
		}

		public override EERowData GetData(int index)
		{
			return elements[index];
		}
	}
}
