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
//frmTraceOptions.vb
//   TreasureChest2 Trace Option Entry Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/03/05    Added support for new trcMemory TraceOption;
//   08/24/05    Added support for new TraceOptions:
//                   trcServer = 2 ^ 17
//                   trcFileWatcher = 2 ^ 18
//                   trcMSMQWatcher = 2 ^ 19
//                   trcTaskWatcher = 2 ^ 20
//                   trcTCPIPWatcher = 2 ^ 21
//                   Reorganized screen display;
//   04/20/05    Added trcWinsock support;
//   03/23/05    Added support for trcSupport and cmdBrowse;
//   03/03/05    Upgraded for use as a VB.NET Component;
//   02/24/04    Added chkRPC;
//   11/24/03    Added chkADO, cmdCancel, cmdSelectAll and cmdDeselectAll;
//   10/30/02    Added "On Error Resume Next" to functions without any error handling;
//               Added forced validation of txtTraceFile on cmdOK_Click;
//   11/14/01    Migrated to SIASUTL from FiRRe;
//   09/19/01    Reviewed case-sensitivity issues for SQL Server 'dictionary_iso (51)' character set support;
//   06/12/01    Added chkAudit;
//   06/05/01    Added support for TraceFile;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
using VB = Microsoft.VisualBasic;
namespace TCBase
{

	internal class frmTraceOptions : frmTCBase
	{
		public frmTraceOptions(ref clsSupport objSupport) : base(objSupport, "frmTraceOptions")
		{
			Closing += frmTraceOptions_Closing;
			Load += frmTraceOptions_Load;
			Activated += frmTraceOptions_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		#region "Windows Form Designer generated code "
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		public ToolTip ToolTip1;
		private Button withEventsField_cmdDeselectAll;
		public Button cmdDeselectAll {
			get { return withEventsField_cmdDeselectAll; }
			set {
				if (withEventsField_cmdDeselectAll != null) {
					withEventsField_cmdDeselectAll.Click -= cmdDeselectAll_Click;
				}
				withEventsField_cmdDeselectAll = value;
				if (withEventsField_cmdDeselectAll != null) {
					withEventsField_cmdDeselectAll.Click += cmdDeselectAll_Click;
				}
			}
		}
		private Button withEventsField_cmdSelectAll;
		public Button cmdSelectAll {
			get { return withEventsField_cmdSelectAll; }
			set {
				if (withEventsField_cmdSelectAll != null) {
					withEventsField_cmdSelectAll.Click -= cmdSelectAll_Click;
				}
				withEventsField_cmdSelectAll = value;
				if (withEventsField_cmdSelectAll != null) {
					withEventsField_cmdSelectAll.Click += cmdSelectAll_Click;
				}
			}
		}
		private Button withEventsField_cmdCancel;
		public Button cmdCancel {
			get { return withEventsField_cmdCancel; }
			set {
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click -= cmdCancel_Click;
				}
				withEventsField_cmdCancel = value;
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click += cmdCancel_Click;
				}
			}
		}
		private Button withEventsField_cmdOK;
		public Button cmdOK {
			get { return withEventsField_cmdOK; }
			set {
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click -= cmdOK_Click;
				}
				withEventsField_cmdOK = value;
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click += cmdOK_Click;
				}
			}
		}
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		internal GroupBox fraTraceFile;
		public TextBox txtTraceFile;
		private Button withEventsField_cmdBrowse;
		internal Button cmdBrowse {
			get { return withEventsField_cmdBrowse; }
			set {
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click -= cmdBrowse_Click;
				}
				withEventsField_cmdBrowse = value;
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click += cmdBrowse_Click;
				}
			}
		}
		public CheckBox chkControls;
		public CheckBox chkCL;
		public CheckBox chkSupport;
		public CheckBox chkDB;
		public CheckBox chkLogon;
		public CheckBox chkReports;
		public CheckBox chkMemory;
		public CheckBox chkBinding;
		public CheckBox chkApplication;
		internal OpenFileDialog ofdTraceOptions;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cmdDeselectAll = new System.Windows.Forms.Button();
			this.cmdSelectAll = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.fraTraceFile = new System.Windows.Forms.GroupBox();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.txtTraceFile = new System.Windows.Forms.TextBox();
			this.ofdTraceOptions = new System.Windows.Forms.OpenFileDialog();
			this.chkControls = new System.Windows.Forms.CheckBox();
			this.chkCL = new System.Windows.Forms.CheckBox();
			this.chkSupport = new System.Windows.Forms.CheckBox();
			this.chkDB = new System.Windows.Forms.CheckBox();
			this.chkLogon = new System.Windows.Forms.CheckBox();
			this.chkReports = new System.Windows.Forms.CheckBox();
			this.chkMemory = new System.Windows.Forms.CheckBox();
			this.chkBinding = new System.Windows.Forms.CheckBox();
			this.chkApplication = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			this.fraTraceFile.SuspendLayout();
			this.SuspendLayout();
			//
			//cmdDeselectAll
			//
			this.cmdDeselectAll.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDeselectAll.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDeselectAll.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdDeselectAll.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDeselectAll.Location = new System.Drawing.Point(203, 89);
			this.cmdDeselectAll.Name = "cmdDeselectAll";
			this.cmdDeselectAll.Size = new System.Drawing.Size(81, 21);
			this.cmdDeselectAll.TabIndex = 3;
			this.cmdDeselectAll.Text = "&Deselect All";
			this.cmdDeselectAll.UseVisualStyleBackColor = false;
			//
			//cmdSelectAll
			//
			this.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSelectAll.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSelectAll.Location = new System.Drawing.Point(203, 62);
			this.cmdSelectAll.Name = "cmdSelectAll";
			this.cmdSelectAll.Size = new System.Drawing.Size(81, 21);
			this.cmdSelectAll.TabIndex = 2;
			this.cmdSelectAll.Text = "&Select All";
			this.cmdSelectAll.UseVisualStyleBackColor = false;
			//
			//cmdCancel
			//
			this.cmdCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCancel.Location = new System.Drawing.Point(205, 293);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(81, 21);
			this.cmdCancel.TabIndex = 5;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			//
			//cmdOK
			//
			this.cmdOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
			this.cmdOK.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdOK.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdOK.Location = new System.Drawing.Point(118, 293);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(81, 21);
			this.cmdOK.TabIndex = 4;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = false;
			//
			//fraTraceFile
			//
			this.fraTraceFile.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.fraTraceFile.Controls.Add(this.cmdBrowse);
			this.fraTraceFile.Controls.Add(this.txtTraceFile);
			this.fraTraceFile.Location = new System.Drawing.Point(5, 8);
			this.fraTraceFile.Name = "fraTraceFile";
			this.fraTraceFile.Size = new System.Drawing.Size(281, 48);
			this.fraTraceFile.TabIndex = 0;
			this.fraTraceFile.TabStop = false;
			this.fraTraceFile.Text = "Trace File";
			//
			//cmdBrowse
			//
			this.cmdBrowse.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdBrowse.Location = new System.Drawing.Point(198, 18);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
			this.cmdBrowse.TabIndex = 1;
			this.cmdBrowse.Text = "Browse";
			//
			//txtTraceFile
			//
			this.txtTraceFile.AcceptsReturn = true;
			this.txtTraceFile.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtTraceFile.BackColor = System.Drawing.SystemColors.Control;
			this.txtTraceFile.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtTraceFile.Enabled = false;
			this.txtTraceFile.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtTraceFile.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtTraceFile.Location = new System.Drawing.Point(12, 18);
			this.txtTraceFile.MaxLength = 0;
			this.txtTraceFile.Name = "txtTraceFile";
			this.txtTraceFile.Size = new System.Drawing.Size(182, 21);
			this.txtTraceFile.TabIndex = 0;
			this.txtTraceFile.TabStop = false;
			//
			//chkControls
			//
			this.chkControls.AutoSize = true;
			this.chkControls.BackColor = System.Drawing.SystemColors.Control;
			this.chkControls.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkControls.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkControls.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkControls.Location = new System.Drawing.Point(17, 136);
			this.chkControls.Name = "chkControls";
			this.chkControls.Size = new System.Drawing.Size(73, 19);
			this.chkControls.TabIndex = 15;
			this.chkControls.Text = "Controls";
			this.chkControls.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkControls.UseVisualStyleBackColor = false;
			//
			//chkCL
			//
			this.chkCL.AutoSize = true;
			this.chkCL.BackColor = System.Drawing.SystemColors.Control;
			this.chkCL.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkCL.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkCL.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkCL.Location = new System.Drawing.Point(17, 111);
			this.chkCL.Name = "chkCL";
			this.chkCL.Size = new System.Drawing.Size(77, 19);
			this.chkCL.TabIndex = 11;
			this.chkCL.Text = "CmdLine";
			this.chkCL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkCL.UseVisualStyleBackColor = false;
			//
			//chkSupport
			//
			this.chkSupport.AutoSize = true;
			this.chkSupport.BackColor = System.Drawing.SystemColors.Control;
			this.chkSupport.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkSupport.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkSupport.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkSupport.Location = new System.Drawing.Point(17, 261);
			this.chkSupport.Name = "chkSupport";
			this.chkSupport.Size = new System.Drawing.Size(69, 19);
			this.chkSupport.TabIndex = 9;
			this.chkSupport.Text = "Support";
			this.chkSupport.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSupport.UseVisualStyleBackColor = false;
			//
			//chkDB
			//
			this.chkDB.AutoSize = true;
			this.chkDB.BackColor = System.Drawing.SystemColors.Control;
			this.chkDB.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDB.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkDB.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDB.Location = new System.Drawing.Point(17, 161);
			this.chkDB.Name = "chkDB";
			this.chkDB.Size = new System.Drawing.Size(43, 19);
			this.chkDB.TabIndex = 10;
			this.chkDB.Text = "DB";
			this.chkDB.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkDB.UseVisualStyleBackColor = false;
			//
			//chkLogon
			//
			this.chkLogon.AutoSize = true;
			this.chkLogon.BackColor = System.Drawing.SystemColors.Control;
			this.chkLogon.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkLogon.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkLogon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkLogon.Location = new System.Drawing.Point(17, 186);
			this.chkLogon.Name = "chkLogon";
			this.chkLogon.Size = new System.Drawing.Size(61, 19);
			this.chkLogon.TabIndex = 12;
			this.chkLogon.Text = "Logon";
			this.chkLogon.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkLogon.UseVisualStyleBackColor = false;
			//
			//chkReports
			//
			this.chkReports.AutoSize = true;
			this.chkReports.BackColor = System.Drawing.SystemColors.Control;
			this.chkReports.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkReports.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkReports.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkReports.Location = new System.Drawing.Point(17, 236);
			this.chkReports.Name = "chkReports";
			this.chkReports.Size = new System.Drawing.Size(70, 19);
			this.chkReports.TabIndex = 14;
			this.chkReports.Text = "Reports";
			this.chkReports.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkReports.UseVisualStyleBackColor = false;
			//
			//chkMemory
			//
			this.chkMemory.AutoSize = true;
			this.chkMemory.BackColor = System.Drawing.SystemColors.Control;
			this.chkMemory.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkMemory.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkMemory.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkMemory.Location = new System.Drawing.Point(17, 211);
			this.chkMemory.Name = "chkMemory";
			this.chkMemory.Size = new System.Drawing.Size(69, 19);
			this.chkMemory.TabIndex = 13;
			this.chkMemory.Text = "Memory";
			this.chkMemory.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkMemory.UseVisualStyleBackColor = false;
			//
			//chkBinding
			//
			this.chkBinding.AutoSize = true;
			this.chkBinding.BackColor = System.Drawing.SystemColors.Control;
			this.chkBinding.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkBinding.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkBinding.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkBinding.Location = new System.Drawing.Point(17, 86);
			this.chkBinding.Name = "chkBinding";
			this.chkBinding.Size = new System.Drawing.Size(68, 19);
			this.chkBinding.TabIndex = 17;
			this.chkBinding.Text = "Binding";
			this.chkBinding.UseVisualStyleBackColor = false;
			//
			//chkApplication
			//
			this.chkApplication.AutoSize = true;
			this.chkApplication.BackColor = System.Drawing.SystemColors.Control;
			this.chkApplication.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkApplication.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkApplication.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkApplication.Location = new System.Drawing.Point(17, 62);
			this.chkApplication.Name = "chkApplication";
			this.chkApplication.Size = new System.Drawing.Size(86, 19);
			this.chkApplication.TabIndex = 16;
			this.chkApplication.Text = "Application";
			this.chkApplication.UseVisualStyleBackColor = false;
			//
			//frmTraceOptions
			//
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(298, 326);
			this.Controls.Add(this.chkBinding);
			this.Controls.Add(this.chkApplication);
			this.Controls.Add(this.chkControls);
			this.Controls.Add(this.chkCL);
			this.Controls.Add(this.chkSupport);
			this.Controls.Add(this.chkDB);
			this.Controls.Add(this.chkLogon);
			this.Controls.Add(this.chkReports);
			this.Controls.Add(this.chkMemory);
			this.Controls.Add(this.fraTraceFile);
			this.Controls.Add(this.cmdDeselectAll);
			this.Controls.Add(this.cmdSelectAll);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(725, 364);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(314, 364);
			this.Name = "frmTraceOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trace Options";
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			this.fraTraceFile.ResumeLayout(false);
			this.fraTraceFile.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		public bool fOKtoUnload;
		public bool fCancelClicked;
			#endregion
		public clsTrace.trcOption iTraceOptions;
		#region "Methods"
		private void MarkControls(Control iControl, CheckState CheckedState)
		{
			foreach (Control jControl in iControl.Controls) {
				switch (jControl.GetType().Name) {
					case "CheckBox":
						((CheckBox)jControl).CheckState = CheckedState;
						break;
					case "GroupBox":
					case "frmTraceOptions":
					case "TabControl":
					case "TabPage":
						MarkControls(jControl, CheckedState);
						break;
				}
			}
		}
		#endregion
		#region "Event Handlers"
		private void cmdBrowse_Click(System.Object sender, System.EventArgs e)
		{
			var _with1 = ofdTraceOptions;
			_with1.Reset();
			_with1.CheckFileExists = false;
			_with1.CheckPathExists = true;
			_with1.DefaultExt = "trace";
			_with1.Filter = "Trace Files (*.trace)|*.trace|All Files (*.*)|*.*";
			_with1.FilterIndex = 2;
			_with1.RestoreDirectory = true;
			_with1.FileName = txtTraceFile.Text;
			_with1.ShowDialog();
			txtTraceFile.Text = _with1.FileName;
		}
		private void cmdCancel_Click(System.Object sender, System.EventArgs e)
		{
			try {
				fCancelClicked = true;
				base.Hide();
			} catch (Exception ex) {
			}
		}
		private void cmdDeselectAll_Click(System.Object sender, System.EventArgs e)
		{
			MarkControls(this, CheckState.Unchecked);
		}
		private void cmdOK_Click(System.Object sender, System.EventArgs e)
		{
			System.ComponentModel.CancelEventArgs CancelEventArgs = new System.ComponentModel.CancelEventArgs(false);
			try {
				//Force validation on the TraceFile...
				txtTraceFile_Validating(txtTraceFile, CancelEventArgs);
				if (CancelEventArgs.Cancel)
					return;

				iTraceOptions = 0;
				if (this.chkApplication.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcApplication;
				if (this.chkBinding.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcBinding;
				if (this.chkCL.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcCL;
				if (this.chkControls.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcControls;
				if (this.chkDB.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcDB;
				if (this.chkLogon.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcLogon;
				if (this.chkMemory.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcMemory;
				if (this.chkReports.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcReports;
				if (this.chkSupport.CheckState == CheckState.Checked)
					iTraceOptions |= clsTrace.trcOption.trcSupport;
				this.Hide();
			} catch (Exception ex) {
			}
		}
		private void cmdSelectAll_Click(System.Object sender, System.EventArgs e)
		{
			MarkControls(this, CheckState.Checked);
		}
		private void frmTraceOptions_Activated(System.Object sender, System.EventArgs e)
		{
			try {
				if (((iTraceOptions & clsTrace.trcOption.trcApplication) != 0))
					this.chkApplication.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcBinding) != 0))
					this.chkBinding.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcCL) != 0))
					this.chkCL.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcControls) != 0))
					this.chkControls.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcDB) != 0))
					this.chkDB.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcLogon) != 0))
					this.chkLogon.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcMemory) != 0))
					this.chkMemory.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcReports) != 0))
					this.chkReports.CheckState = CheckState.Checked;
				if (((iTraceOptions & clsTrace.trcOption.trcSupport) != 0))
					this.chkSupport.CheckState = CheckState.Checked;
			} catch (Exception ex) {
			}
		}
		private void frmTraceOptions_Load(System.Object sender, System.EventArgs e)
		{
			try {
				fOKtoUnload = false;
				fCancelClicked = false;
				MarkControls(this, CheckState.Unchecked);
			} catch (Exception ex) {
			}
		}
		private void frmTraceOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try {
				if (!fOKtoUnload)
					e.Cancel = true;
			} catch (Exception ex) {
			}
		}
		private void txtTraceFile_Enter(System.Object sender, System.EventArgs e)
		{
			try {
				TextSelected(txtTraceFile);
			} catch (Exception ex) {
			}
		}
		private void txtTraceFile_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			string strDirectory = null;
			string strFile = null;
			try {
				strDirectory = mSupport.ParsePath(txtTraceFile.Text, ParseParts.DrvDirNoSlash);
				strFile = mSupport.ParsePath(txtTraceFile.Text, ParseParts.FileNameBaseExt);
				if (strDirectory != bpeNullString) {
					if (Strings.Right(strDirectory, 1) == ":")
						strDirectory = strDirectory + "\\";
					if (FileSystem.Dir(strDirectory, FileAttribute.Directory) == bpeNullString) {
						Interaction.MsgBox("\"" + strDirectory + "\" directory does not exist, please respecify.", MsgBoxStyle.Exclamation, this.Text);
						e.Cancel = true;
						TextSelected(txtTraceFile);
                        throw new ExitTryException();
					}
				}
				strDirectory = FileSystem.Dir(strFile, FileAttribute.Normal);
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				Interaction.MsgBox(ex.Message + " (" + ex.GetType().Name + ")", MsgBoxStyle.Exclamation, this.Text);
				e.Cancel = true;
				TextSelected(txtTraceFile);
			}
		}
		#endregion
	}
}
