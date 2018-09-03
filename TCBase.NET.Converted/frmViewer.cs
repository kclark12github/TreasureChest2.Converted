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
	public partial class frmViewer
	{
		public frmViewer(clsSupport objSupport, clsTCBase objTCBase, string ReportName, Form objParent = null, string Caption = null) : base(objSupport, "frmViewer", objTCBase, objParent)
		{
			Load += frmViewer_Load;
			Closing += frmReport_Closing;
			Activated += frmReport_Activated;
			InitializeComponent();
			if ((Caption != null))
				this.Text = Caption;
			mReportName = ReportName;
			mRegistryKey = string.Format("{0}\\{1} Settings\\Report Settings", mTCBase.RegistryKey, ((mTCBase.ActiveForm == null) ? "frmMain" : mTCBase.ActiveForm.Name));
		}
		#region "Properties"
		#region "Declarations"
		private bool fActivated = false;
		private string mRegistryKey;
			#endregion
		private string mReportName;
		public string ReportName {
			get { return mReportName; }
		}
		#endregion
		#region "Methods"
		#endregion
		#region "Event Handlers"
		private void frmReport_Activated(object sender, System.EventArgs e)
		{
			if (fActivated)
				return;
			fActivated = true;

			int iLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Left", 0);
			int iTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Top", 0);
			int iWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Width", Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width));
			int iHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Height", Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height));
			this.SetBounds(iLeft, iTop, iWidth, iHeight);
		}
		private void frmReport_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mTCBase.SaveBounds(mRegistryKey, this.Left, this.Top, this.Width, this.Height);
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private void frmViewer_Load(object sender, EventArgs e)
		{
			this.rdlcViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.Normal);
			object reportDataTable = null;
			this.rdlcViewer.LocalReport.ReportEmbeddedResource = mReportName;
			switch (mReportName) {
				case "TCBase.Books.rdlc":
					this.ReportBindingSource.DataMember = "Books";
					reportDataTable = this.ReportDataSet.Books;
					if (!string.IsNullOrEmpty(mTCBase.SQLFilter))
						this.BooksTableAdapter.CommandCollection[0].CommandText += string.Format(" Where {0}", mTCBase.SQLFilter);
					break;
			}
//            this.BooksTableAdapter.Fill((ReportDataSet.BooksDataTable)reportDataTable);
			this.rdlcViewer.RefreshReport();
		}
		#endregion
	}
}
