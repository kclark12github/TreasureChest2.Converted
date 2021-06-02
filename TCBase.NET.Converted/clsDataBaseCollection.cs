//clsDataBaseCollection.cs
//   DataBase Collection Object...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/28/09    Rewritten in VB.NET;
//   10/07/02    Added History;
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
	internal class clsDataBaseCollection : IEnumerable
	{
		public clsDataBaseCollection() : base()
		{
			mCol = new Collection();
		}

		#region "Properties"
		private Collection mCol;
		public clsDataBaseInfo this[object Key] {
			get { return (clsDataBaseInfo)mCol[Key]; }
		}
		public int Count {
			get { return mCol.Count; }
		}
		#endregion
		#region "Methods"
		public clsDataBaseInfo Add(string Key, string SQLSource, DataView dv)
		{
			clsDataBaseInfo functionReturnValue = null;
			clsDataBaseInfo objNewMember = new clsDataBaseInfo();

			objNewMember.Key = Key;
			objNewMember.SQLSource = SQLSource;
			objNewMember.DataView = dv;
			mCol.Add(objNewMember, Key);

			functionReturnValue = objNewMember;
			objNewMember = null;
			return functionReturnValue;
		}
		#endregion
		public System.Collections.IEnumerator GetEnumerator()
		{
			return mCol.GetEnumerator();
		}
		public void Clear()
		{
			foreach (clsDataBaseInfo iCol in mCol) {
				mCol.Remove(1);
			}
		}
		public string FindDataView(DataView dv)
		{
			string functionReturnValue = null;
			functionReturnValue = Constants.vbNullString;
			foreach (clsDataBaseInfo iCol in mCol) {
				if (object.ReferenceEquals(iCol.DataView, dv)) {
					functionReturnValue = iCol.Key;
					break;
				}
			}
			return functionReturnValue;
		}
		public void Remove(object Key)
		{
			mCol.Remove((string)Key);
		}
	}
}
