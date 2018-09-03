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
//clsDataBaseInfo - clsDataBaseInfo.cls
//   DataBase Information (DataView) Object...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/28/09    Rewritten in VB.NET;
//   10/08/02    Added History;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	internal class clsDataBaseInfo
	{
		private string mKey = "";
		private string mSQLSource = "";
		private DataView mDataView = null;
		public string Key {
			get { return mKey; }
			set { mKey = value; }
		}
		public string SQLSource {
			get { return mSQLSource; }
			set { mSQLSource = value; }
		}
		public DataView DataView {
			get { return mDataView; }
			set { mDataView = value; }
		}
	}
}
