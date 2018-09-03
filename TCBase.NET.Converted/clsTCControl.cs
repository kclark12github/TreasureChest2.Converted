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
//clsTCControl,vb
//   Form Control to Database Field Mapping Object...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/25/09    Migrated to VB.NET;
//   10/08/02    Added DataSourceSQL for virtual recordset fields;
//   08/20/02    Added DataType and OriginalValue properties;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	internal class clsTCControl
	{
		private string mCaption = "";
		private System.Windows.Forms.Control mControl = null;
		private string mDataMember = "";
		private DataView mDataSource = null;
		private System.Data.SqlDbType mDataType = SqlDbType.VarChar;
		private int mDataSize;
		private string mFormat = "";
		private System.Windows.Forms.Control mLabelControl = null;
		private object mOriginalValue = null;
		private string mDisplayMember = "";
		private DataView mDisplaySource = null;
		private bool mChanged = false;
		public string Caption {
			get { return mCaption; }
			set { mCaption = value; }
		}
		public bool Changed {
			get { return mChanged; }
			set { mChanged = value; }
		}
		public System.Windows.Forms.Control Control {
			get { return mControl; }
			set { mControl = value; }
		}
		public string DataMember {
			get { return mDataMember; }
			set { mDataMember = value; }
		}
		public int DataSize {
			get { return mDataSize; }
			set { mDataSize = value; }
		}
		public DataView DataSource {
			get { return mDataSource; }
			set { mDataSource = value; }
		}
		public System.Data.SqlDbType DataType {
			get { return mDataType; }
			set { mDataType = value; }
		}
		public string Format {
			get { return mFormat; }
			set { mFormat = value; }
		}
		public System.Windows.Forms.Control LabelControl {
			get { return mLabelControl; }
			set { mLabelControl = value; }
		}
		public object OriginalValue {
			get { return mOriginalValue; }
			set { mOriginalValue = value; }
		}
		public string DisplayMember {
			get { return mDisplayMember; }
			set { mDisplayMember = value; }
		}
		public DataView DisplaySource {
			get { return mDisplaySource; }
			set { mDisplaySource = value; }
		}
		public clsTCControl() : base()
		{
		}
	}
}
