//frmReport.cs
//   Report Viewer Form...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//TODO:   05/28/21    Updated to support screen size & location user preferences;
//   07/27/19    Added logic to adjust form placement to account for preferences from a device with a size larger than the 
//               current device so this form would always displays on the current viewport;
//TODO:   07/21/19    Changed form positioning from being independent to being centered on the parent form;
//   07/26/10    Reorganized registry settings;
//   07/25/10    Resized and defaulted to (0,0) if no saved location information is in registry;
//   11/28/09    Migrated to VB.NET;
//   10/19/02    Added Filter Support;
//   10/14/02    Added Error Handling;
//   10/13/02    Refitted for use in TreasureChest;
//=================================================================================================================================
//Notes:
//=================================================================================================================================
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
using CrystalDecisions.CrystalReports.Engine;
namespace TCBase
{
	public class frmReport : TCBase.frmTCBase
	{
		public frmReport(clsSupport objSupport, clsTCBase objTCBase, Form objParent = null, string Caption = null) : base(objSupport, "frmReport", objTCBase, objParent)
		{
			Closing += frmReport_Closing;
			Activated += frmReport_Activated;
			InitializeComponent();
			if ((Caption != null))
				this.Text = Caption;

			mRegistryKey = string.Format("{0}\\{1} Settings\\Report Settings", mTCBase.RegistryKey, ((mTCBase.ActiveForm == null) ? "frmMain" : mTCBase.ActiveForm.Name));
		}
		#region " Windows Form Designer generated code "

		public frmReport() : base()
		{
			Closing += frmReport_Closing;
			Activated += frmReport_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		internal CrystalDecisions.Windows.Forms.CrystalReportViewer crvMain;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.crvMain = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
			this.SuspendLayout();
			//
			//crvMain
			//
			this.crvMain.ActiveViewIndex = -1;
			this.crvMain.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.crvMain.Location = new System.Drawing.Point(0, 0);
			this.crvMain.Name = "crvMain";
			this.crvMain.ReportSource = null;
			this.crvMain.Size = new System.Drawing.Size(980, 372);
			this.crvMain.TabIndex = 0;
			//
			//frmReport
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(980, 374);
			this.Controls.Add(this.crvMain);
			this.Name = "frmReport";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmReport";
			this.ResumeLayout(false);

		}

		#endregion
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
			base.AdjustFormPlacement(ref iTop, ref iLeft); //Correct for errant form placement...
			this.SetBounds(iLeft, iTop, iWidth, iHeight);
		}
		private void frmReport_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mTCBase.SaveBounds(mRegistryKey, this.Left, this.Top, this.Width, this.Height);
		}
		#endregion
	}
}
