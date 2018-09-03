//clsTCControls,vb
//   Form Control Collection...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/25/09    Migrated to VB.NET;
//   10/08/02    Added DataSourceSQL to Add();
//   10/07/02    Copied from clsDatabaseCollection;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Implementation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
namespace TCBase
{
	internal class clsTCControls : System.Collections.IEnumerable
	{
		public clsTCControls() : base()
		{
		}
		private Collection mControls = new Collection();
		public clsTCControl Add(System.Windows.Forms.ComboBox ctl, DataView dataSource, string dataMember, System.Data.SqlDbType dataType, int dataSize, DataView displaySource, string displayMember, string Format = null, string Caption = null, System.Windows.Forms.Label Label = null)
		{
			clsTCControl tcc = new clsTCControl();
			try {
				var _with1 = tcc;
				_with1.Control = ctl;
				_with1.DataSource = dataSource;
				_with1.DataMember = dataMember;
				_with1.DataType = dataType;
				_with1.DataSize = dataSize;
				_with1.DisplayMember = displayMember;
				_with1.DisplaySource = displaySource;
				//Optional...
				_with1.Format = Format;
				_with1.Caption = Caption;
				_with1.LabelControl = Label;
				mControls.Add(tcc, ctl.Name);
				return tcc;
			} catch (Exception ex) {
				throw;
			}
		}
		public clsTCControl Add(System.Windows.Forms.Control ctl, DataView dataSource, string dataMember, System.Data.SqlDbType dataType, int dataSize, string Format = null, string Caption = null, System.Windows.Forms.Label Label = null)
		{
			clsTCControl tcc = new clsTCControl();
			try {
				switch (Information.TypeName(ctl)) {
					case "ComboBox":
						throw new Exception("Use ComboBox-specific form of Add()");
					default:
						var _with2 = tcc;
						_with2.Control = ctl;
						_with2.DataSource = dataSource;
						_with2.DataMember = dataMember;
						_with2.DataType = dataType;
						_with2.DataSize = dataSize;
						//Optional...
						_with2.Format = Format;
						_with2.Caption = Caption;
						_with2.LabelControl = Label;
						mControls.Add(tcc, ctl.Name);
						return tcc;
				}
			} catch (Exception ex) {
				throw;
			}
		}
		public clsTCControl this[object Key] {
			get { return (clsTCControl)mControls[Key]; }
		}
		public int Count {
			get { return mControls.Count; }
		}
		public System.Collections.IEnumerator GetEnumerator()
		{
			return mControls.GetEnumerator();
		}
		public void Clear()
		{
			foreach (clsTCControl iTCControl in mControls) {
				mControls.Remove(1);
			}
		}
		public void ClearChangedFlag()
		{
			foreach (clsTCControl iTCControl in mControls) {
				iTCControl.Changed = false;
			}
		}
		//public void Remove(ref object Key)
		//{
		//	mControls.Remove(Key);
		//}
	}
}
