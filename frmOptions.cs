//frmOptions.vb
//   Options/Properties Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/27/10    Added Export button;
//   10/25/09    Rewritten in VB.NET;
//   02/04/03    Added txtUserID & txtPassword;
//   10/14/02    Added Error Handling;
//               Added ReportsDirectory stuff;
//   09/17/02    Modified for use by TreasureChest;
//   08/20/02    Started History;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
namespace TreasureChest2
{
	internal class frmOptions : frmTCBase
	{
		const string myFormName = "frmOptions";
		public frmOptions() : base()
		{
			Load += frmOptions_Load;
			Activated += frmOptions_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
		}
		public frmOptions(clsSupport objSupport, Form objParent) : base(objSupport, myFormName, objParent)
		{
			Load += frmOptions_Load;
			Activated += frmOptions_Activated;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		public frmOptions(clsSupport objSupport, TCBase.clsTCBase objTCBase, Form objParent) : base(objSupport, myFormName, objTCBase, objParent)
		{
			Load += frmOptions_Load;
			Activated += frmOptions_Activated;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		#region " Windows Form Designer generated code "
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
		internal System.Windows.Forms.GroupBox gbBackgroundImage;
		internal System.Windows.Forms.GroupBox gbDataSource;
		internal System.Windows.Forms.GroupBox gbReportsDirectory;
		private System.Windows.Forms.TextBox withEventsField_txtPassword;
		internal System.Windows.Forms.TextBox txtPassword {
			get { return withEventsField_txtPassword; }
			set {
				if (withEventsField_txtPassword != null) {
					withEventsField_txtPassword.Validating -= txtPassword_Validating;
				}
				withEventsField_txtPassword = value;
				if (withEventsField_txtPassword != null) {
					withEventsField_txtPassword.Validating += txtPassword_Validating;
				}
			}
		}
		internal System.Windows.Forms.GroupBox gbFileDSN;
		private System.Windows.Forms.RadioButton withEventsField_rbConnectionString;
		internal System.Windows.Forms.RadioButton rbConnectionString {
			get { return withEventsField_rbConnectionString; }
			set {
				if (withEventsField_rbConnectionString != null) {
					withEventsField_rbConnectionString.CheckedChanged -= rbDataSource_CheckChanged;
				}
				withEventsField_rbConnectionString = value;
				if (withEventsField_rbConnectionString != null) {
					withEventsField_rbConnectionString.CheckedChanged += rbDataSource_CheckChanged;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtConnectionString;
		internal System.Windows.Forms.TextBox txtConnectionString {
			get { return withEventsField_txtConnectionString; }
			set {
				if (withEventsField_txtConnectionString != null) {
					withEventsField_txtConnectionString.Validating -= txtConnectionString_Validating;
				}
				withEventsField_txtConnectionString = value;
				if (withEventsField_txtConnectionString != null) {
					withEventsField_txtConnectionString.Validating += txtConnectionString_Validating;
				}
			}
		}
		private System.Windows.Forms.RadioButton withEventsField_rbFileDSN;
		internal System.Windows.Forms.RadioButton rbFileDSN {
			get { return withEventsField_rbFileDSN; }
			set {
				if (withEventsField_rbFileDSN != null) {
					withEventsField_rbFileDSN.CheckedChanged -= rbDataSource_CheckChanged;
				}
				withEventsField_rbFileDSN = value;
				if (withEventsField_rbFileDSN != null) {
					withEventsField_rbFileDSN.CheckedChanged += rbDataSource_CheckChanged;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnBrowseFileDSN;
		internal System.Windows.Forms.Button btnBrowseFileDSN {
			get { return withEventsField_btnBrowseFileDSN; }
			set {
				if (withEventsField_btnBrowseFileDSN != null) {
					withEventsField_btnBrowseFileDSN.Click -= btnBrowseFileDSN_Click;
					withEventsField_btnBrowseFileDSN.Leave -= btnBrowseFileDSN_Leave;
				}
				withEventsField_btnBrowseFileDSN = value;
				if (withEventsField_btnBrowseFileDSN != null) {
					withEventsField_btnBrowseFileDSN.Click += btnBrowseFileDSN_Click;
					withEventsField_btnBrowseFileDSN.Leave += btnBrowseFileDSN_Leave;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtUserID;
		internal System.Windows.Forms.TextBox txtUserID {
			get { return withEventsField_txtUserID; }
			set {
				if (withEventsField_txtUserID != null) {
					withEventsField_txtUserID.Validating -= txtUserID_Validating;
				}
				withEventsField_txtUserID = value;
				if (withEventsField_txtUserID != null) {
					withEventsField_txtUserID.Validating += txtUserID_Validating;
				}
			}
		}
		internal System.Windows.Forms.Label lblUserID;
		internal System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button withEventsField_btnOK;
		internal System.Windows.Forms.Button btnOK {
			get { return withEventsField_btnOK; }
			set {
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click -= btnOK_Click;
				}
				withEventsField_btnOK = value;
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click += btnOK_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCancel;
		internal System.Windows.Forms.Button btnCancel {
			get { return withEventsField_btnCancel; }
			set {
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click -= btnCancel_Click;
				}
				withEventsField_btnCancel = value;
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click += btnCancel_Click;
				}
			}
		}
		internal System.Windows.Forms.TextBox txtReportsDirectory;
		private System.Windows.Forms.TextBox withEventsField_txtFileDSN;
		internal System.Windows.Forms.TextBox txtFileDSN {
			get { return withEventsField_txtFileDSN; }
			set {
				if (withEventsField_txtFileDSN != null) {
					withEventsField_txtFileDSN.Validating -= txtFileDSN_Validating;
				}
				withEventsField_txtFileDSN = value;
				if (withEventsField_txtFileDSN != null) {
					withEventsField_txtFileDSN.Validating += txtFileDSN_Validating;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtBackground;
		internal System.Windows.Forms.TextBox txtBackground {
			get { return withEventsField_txtBackground; }
			set {
				if (withEventsField_txtBackground != null) {
					withEventsField_txtBackground.Validating -= txtBackground_Validating;
				}
				withEventsField_txtBackground = value;
				if (withEventsField_txtBackground != null) {
					withEventsField_txtBackground.Validating += txtBackground_Validating;
				}
			}
		}
		internal System.Windows.Forms.FolderBrowserDialog fbdOptions;
		internal System.Windows.Forms.OpenFileDialog ofdOptions;
		private System.Windows.Forms.Button withEventsField_btnBrowseBackgroundImage;
		internal System.Windows.Forms.Button btnBrowseBackgroundImage {
			get { return withEventsField_btnBrowseBackgroundImage; }
			set {
				if (withEventsField_btnBrowseBackgroundImage != null) {
					withEventsField_btnBrowseBackgroundImage.Click -= btnBrowseBackgroundImage_Click;
				}
				withEventsField_btnBrowseBackgroundImage = value;
				if (withEventsField_btnBrowseBackgroundImage != null) {
					withEventsField_btnBrowseBackgroundImage.Click += btnBrowseBackgroundImage_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnBrowseReportsDirectory;
		internal System.Windows.Forms.Button btnBrowseReportsDirectory {
			get { return withEventsField_btnBrowseReportsDirectory; }
			set {
				if (withEventsField_btnBrowseReportsDirectory != null) {
					withEventsField_btnBrowseReportsDirectory.Click -= btnBrowseReportsDirectory_Click;
				}
				withEventsField_btnBrowseReportsDirectory = value;
				if (withEventsField_btnBrowseReportsDirectory != null) {
					withEventsField_btnBrowseReportsDirectory.Click += btnBrowseReportsDirectory_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnExport;
		internal System.Windows.Forms.Button btnExport {
			get { return withEventsField_btnExport; }
			set {
				if (withEventsField_btnExport != null) {
					withEventsField_btnExport.Click -= btnExport_Click;
				}
				withEventsField_btnExport = value;
				if (withEventsField_btnExport != null) {
					withEventsField_btnExport.Click += btnExport_Click;
				}
			}
		}
		internal System.Windows.Forms.SaveFileDialog sfdOptions;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmOptions));
			this.gbBackgroundImage = new System.Windows.Forms.GroupBox();
			this.btnBrowseBackgroundImage = new System.Windows.Forms.Button();
			this.txtBackground = new System.Windows.Forms.TextBox();
			this.gbDataSource = new System.Windows.Forms.GroupBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.lblUserID = new System.Windows.Forms.Label();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.rbFileDSN = new System.Windows.Forms.RadioButton();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.rbConnectionString = new System.Windows.Forms.RadioButton();
			this.gbFileDSN = new System.Windows.Forms.GroupBox();
			this.btnBrowseFileDSN = new System.Windows.Forms.Button();
			this.txtFileDSN = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.gbReportsDirectory = new System.Windows.Forms.GroupBox();
			this.btnBrowseReportsDirectory = new System.Windows.Forms.Button();
			this.txtReportsDirectory = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.fbdOptions = new System.Windows.Forms.FolderBrowserDialog();
			this.ofdOptions = new System.Windows.Forms.OpenFileDialog();
			this.btnExport = new System.Windows.Forms.Button();
			this.sfdOptions = new System.Windows.Forms.SaveFileDialog();
			this.gbBackgroundImage.SuspendLayout();
			this.gbDataSource.SuspendLayout();
			this.gbFileDSN.SuspendLayout();
			this.gbReportsDirectory.SuspendLayout();
			this.SuspendLayout();
			//
			//gbBackgroundImage
			//
			this.gbBackgroundImage.Controls.Add(this.btnBrowseBackgroundImage);
			this.gbBackgroundImage.Controls.Add(this.txtBackground);
			this.gbBackgroundImage.Location = new System.Drawing.Point(8, 8);
			this.gbBackgroundImage.Name = "gbBackgroundImage";
			this.gbBackgroundImage.Size = new System.Drawing.Size(508, 52);
			this.gbBackgroundImage.TabIndex = 0;
			this.gbBackgroundImage.TabStop = false;
			this.gbBackgroundImage.Text = "Background Image";
			//
			//btnBrowseBackgroundImage
			//
			this.btnBrowseBackgroundImage.Location = new System.Drawing.Point(424, 20);
			this.btnBrowseBackgroundImage.Name = "btnBrowseBackgroundImage";
			this.btnBrowseBackgroundImage.TabIndex = 1;
			this.btnBrowseBackgroundImage.Text = "&Browse";
			//
			//txtBackground
			//
			this.txtBackground.Enabled = false;
			this.txtBackground.Location = new System.Drawing.Point(8, 20);
			this.txtBackground.Name = "txtBackground";
			this.txtBackground.ReadOnly = true;
			this.txtBackground.Size = new System.Drawing.Size(404, 23);
			this.txtBackground.TabIndex = 0;
			this.txtBackground.Text = "txtBackground";
			//
			//gbDataSource
			//
			this.gbDataSource.Controls.Add(this.lblPassword);
			this.gbDataSource.Controls.Add(this.lblUserID);
			this.gbDataSource.Controls.Add(this.txtUserID);
			this.gbDataSource.Controls.Add(this.rbFileDSN);
			this.gbDataSource.Controls.Add(this.txtConnectionString);
			this.gbDataSource.Controls.Add(this.rbConnectionString);
			this.gbDataSource.Controls.Add(this.gbFileDSN);
			this.gbDataSource.Controls.Add(this.txtPassword);
			this.gbDataSource.Location = new System.Drawing.Point(12, 64);
			this.gbDataSource.Name = "gbDataSource";
			this.gbDataSource.Size = new System.Drawing.Size(504, 232);
			this.gbDataSource.TabIndex = 1;
			this.gbDataSource.TabStop = false;
			this.gbDataSource.Text = "Data Source";
			//
			//lblPassword
			//
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(136, 202);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(68, 19);
			this.lblPassword.TabIndex = 6;
			this.lblPassword.Text = "Password";
			//
			//lblUserID
			//
			this.lblUserID.AutoSize = true;
			this.lblUserID.Location = new System.Drawing.Point(136, 174);
			this.lblUserID.Name = "lblUserID";
			this.lblUserID.Size = new System.Drawing.Size(51, 19);
			this.lblUserID.TabIndex = 4;
			this.lblUserID.Text = "UserID";
			//
			//txtUserID
			//
			this.txtUserID.Location = new System.Drawing.Point(208, 172);
			this.txtUserID.Name = "txtUserID";
			this.txtUserID.TabIndex = 5;
			this.txtUserID.Text = "txtUserID";
			//
			//rbFileDSN
			//
			this.rbFileDSN.Location = new System.Drawing.Point(8, 88);
			this.rbFileDSN.Name = "rbFileDSN";
			this.rbFileDSN.Size = new System.Drawing.Size(244, 24);
			this.rbFileDSN.TabIndex = 2;
			this.rbFileDSN.Text = "Use File DSN";
			//
			//txtConnectionString
			//
			this.txtConnectionString.Location = new System.Drawing.Point(20, 52);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(476, 23);
			this.txtConnectionString.TabIndex = 1;
			this.txtConnectionString.Text = "txtConnectionString";
			//
			//rbConnectionString
			//
			this.rbConnectionString.Location = new System.Drawing.Point(8, 24);
			this.rbConnectionString.Name = "rbConnectionString";
			this.rbConnectionString.Size = new System.Drawing.Size(244, 24);
			this.rbConnectionString.TabIndex = 0;
			this.rbConnectionString.Text = "Use Connection String";
			//
			//gbFileDSN
			//
			this.gbFileDSN.Controls.Add(this.btnBrowseFileDSN);
			this.gbFileDSN.Controls.Add(this.txtFileDSN);
			this.gbFileDSN.Location = new System.Drawing.Point(20, 116);
			this.gbFileDSN.Name = "gbFileDSN";
			this.gbFileDSN.Size = new System.Drawing.Size(476, 52);
			this.gbFileDSN.TabIndex = 3;
			this.gbFileDSN.TabStop = false;
			this.gbFileDSN.Text = "File DSN";
			//
			//btnBrowseFileDSN
			//
			this.btnBrowseFileDSN.Location = new System.Drawing.Point(392, 20);
			this.btnBrowseFileDSN.Name = "btnBrowseFileDSN";
			this.btnBrowseFileDSN.TabIndex = 1;
			this.btnBrowseFileDSN.Text = "&Browse";
			//
			//txtFileDSN
			//
			this.txtFileDSN.Enabled = false;
			this.txtFileDSN.Location = new System.Drawing.Point(8, 20);
			this.txtFileDSN.Name = "txtFileDSN";
			this.txtFileDSN.ReadOnly = true;
			this.txtFileDSN.Size = new System.Drawing.Size(376, 23);
			this.txtFileDSN.TabIndex = 0;
			this.txtFileDSN.Text = "txtFileDSN";
			//
			//txtPassword
			//
			this.txtPassword.Location = new System.Drawing.Point(208, 200);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = Strings.ChrW(42);
			this.txtPassword.TabIndex = 7;
			this.txtPassword.Text = "txtPassword";
			//
			//gbReportsDirectory
			//
			this.gbReportsDirectory.Controls.Add(this.btnBrowseReportsDirectory);
			this.gbReportsDirectory.Controls.Add(this.txtReportsDirectory);
			this.gbReportsDirectory.Location = new System.Drawing.Point(8, 304);
			this.gbReportsDirectory.Name = "gbReportsDirectory";
			this.gbReportsDirectory.Size = new System.Drawing.Size(508, 52);
			this.gbReportsDirectory.TabIndex = 2;
			this.gbReportsDirectory.TabStop = false;
			this.gbReportsDirectory.Text = "Reports Directory";
			//
			//btnBrowseReportsDirectory
			//
			this.btnBrowseReportsDirectory.Location = new System.Drawing.Point(424, 24);
			this.btnBrowseReportsDirectory.Name = "btnBrowseReportsDirectory";
			this.btnBrowseReportsDirectory.TabIndex = 1;
			this.btnBrowseReportsDirectory.Text = "&Browse";
			//
			//txtReportsDirectory
			//
			this.txtReportsDirectory.Enabled = false;
			this.txtReportsDirectory.Location = new System.Drawing.Point(8, 24);
			this.txtReportsDirectory.Name = "txtReportsDirectory";
			this.txtReportsDirectory.ReadOnly = true;
			this.txtReportsDirectory.Size = new System.Drawing.Size(404, 23);
			this.txtReportsDirectory.TabIndex = 0;
			this.txtReportsDirectory.Text = "txtReportsDirectory";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(360, 364);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			//
			//btnCancel
			//
			this.btnCancel.CausesValidation = false;
			this.btnCancel.Location = new System.Drawing.Point(441, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			//
			//btnExport
			//
			this.btnExport.Location = new System.Drawing.Point(8, 364);
			this.btnExport.Name = "btnExport";
			this.btnExport.TabIndex = 3;
			this.btnExport.Text = "E&xport";
			//
			//sfdOptions
			//
			this.sfdOptions.CheckFileExists = true;
			this.sfdOptions.DefaultExt = "reg";
			this.sfdOptions.RestoreDirectory = true;
			//
			//frmOptions
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(524, 398);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbReportsDirectory);
			this.Controls.Add(this.gbDataSource);
			this.Controls.Add(this.gbBackgroundImage);
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.gbBackgroundImage.ResumeLayout(false);
			this.gbDataSource.ResumeLayout(false);
			this.gbFileDSN.ResumeLayout(false);
			this.gbReportsDirectory.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
			#region "Event Handlers"
		private bool fActivated = false;
		private void btnBrowseFileDSN_Click(System.Object sender, System.EventArgs e)
		{
			var _with1 = this.ofdOptions;
			_with1.AddExtension = true;
			_with1.CheckFileExists = true;
			_with1.CheckPathExists = true;
			_with1.Multiselect = false;
			_with1.Title = "Select Database";
			_with1.InitialDirectory = mTCBase.ODBCFileDSNDir;
			_with1.FileName = mTCBase.FileDSN;
			_with1.Filter = "File DSNs (*.dsn)|*.dsn|All Files (*.*)|*.*";
			_with1.FilterIndex = 1;
			if (_with1.ShowDialog(this) == DialogResult.Cancel) {
				//If Trim(dlgOptions.FileName) = bpeNullString Then
				if (Interaction.MsgBox("Are you trying to clear the existing FileDSN?", MsgBoxStyle.Information | MsgBoxStyle.YesNo, this.Text) == MsgBoxResult.Yes) {
					txtFileDSN.Text = bpeNullString;
					txtFileDSN_Validating(txtFileDSN, new System.ComponentModel.CancelEventArgs(false));
					txtUserID.Text = bpeNullString;
					txtUserID_Validating(txtUserID, new System.ComponentModel.CancelEventArgs(false));
					txtPassword.Text = bpeNullString;
					txtPassword_Validating(txtPassword, new System.ComponentModel.CancelEventArgs(false));
				}
			} else {
				mTCBase.FileDSN = _with1.FileName;
			}
			txtFileDSN.Text = ParsePath(mTCBase.FileDSN, ParseParts.FileNameBaseExt);
		}
		private void btnBrowseFileDSN_Leave(System.Object sender, System.EventArgs e)
		{
			bool fCancel = false;
			txtFileDSN_Validating(txtFileDSN, new System.ComponentModel.CancelEventArgs(fCancel));
			if (fCancel)
				btnBrowseFileDSN.Focus();
		}
		private void btnBrowseBackgroundImage_Click(System.Object sender, System.EventArgs e)
		{
			string CurrentPath = null;
			string CurrentDrive = null;
			string CurrentImage = null;
			CurrentPath = ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash);
			CurrentDrive = ParsePath(mTCBase.ImagePath, ParseParts.DrvOnly);
			CurrentImage = ParsePath(mTCBase.ImagePath, ParseParts.FileNameBaseExt);
			FileSystem.ChDrive(CurrentDrive);
			DirectoryInfo diCurrent = new DirectoryInfo(CurrentPath);
			if (diCurrent.Exists)
				FileSystem.ChDir(CurrentPath);
			var _with2 = this.ofdOptions;
			_with2.AddExtension = true;
			_with2.CheckFileExists = true;
			_with2.CheckPathExists = true;
			_with2.Multiselect = false;
			_with2.Title = "Select New Background Image";
			_with2.InitialDirectory = (mTCBase.ImagePath != bpeNullString ? ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash) : mSupport.ApplicationPath);
			_with2.FileName = CurrentImage;
			_with2.Filter = "All Picture Files|*.jpg;*.gif;*.bmp;*.dib;*.ico;*.cur;*.wmf;*.emf|JPEG Images (*.jpg)|*.jpg|CompuServe GIF Images (*.gif)|*.gif|Windows Bitmaps (*.bmp;*.dib)|*.bmp;*.dib|Icons (*.ico;*.cur)|*.ico;*.cur|Metafiles (*.wmf;*.emf)|*.wmf;*.emf|All Files (*.*)|*.*";
			_with2.FilterIndex = 1;
			if (_with2.ShowDialog(this) != DialogResult.Cancel)
				mTCBase.ImagePath = _with2.FileName;
			txtBackground.Text = ParsePath(mTCBase.ImagePath, ParseParts.FileNameBaseExt);
		}
		private void btnBrowseReportsDirectory_Click(System.Object sender, System.EventArgs e)
		{
			string saveEntry = this.txtReportsDirectory.Text;
			mTCBase.ReportsDirectory = mSupport.UI.ChooseFolder("Select Reports Directory", this.txtReportsDirectory.Text);
			this.txtReportsDirectory.Text = mTCBase.ReportsDirectory;
			if (this.txtReportsDirectory.Text == bpeNullString)
				this.txtReportsDirectory.Text = saveEntry;
		}
		private void btnExport_Click(System.Object sender, System.EventArgs e)
		{
			var _with3 = this.sfdOptions;
			_with3.AddExtension = true;
			_with3.DefaultExt = ".reg";
			_with3.CheckFileExists = false;
			_with3.CheckPathExists = true;
			_with3.Title = "Export Registry Settings";
			_with3.InitialDirectory = mSupport.ApplicationPath;
			_with3.FileName = (mTCBase.UserID == bpeNullString ? bpeNullString : mTCBase.UserID + ".reg");
			_with3.Filter = "Registration Files|*.reg|All Files (*.*)|*.*";
			_with3.FilterIndex = 1;
			if (_with3.ShowDialog(this) != DialogResult.Cancel) {
				mTCBase.ExportRegistrySettings(Microsoft.Win32.Registry.CurrentUser, mTCBase.RegistryKey, _with3.FileName);
			}
		}
		private void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.Cursor = Cursors.WaitCursor;
				if (Strings.Trim(txtFileDSN.Text) == bpeNullString && Strings.Trim(txtConnectionString.Text) == bpeNullString) {
					Interaction.MsgBox("Either Connection String or File DSN must be specified!", MsgBoxStyle.Exclamation, this.Text);
					return;
				}
				if (mTCBase.ConnectionString == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ConnectionString", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ConnectionString");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ConnectionString", mTCBase.ConnectionString);
				}
				if (mTCBase.FileDSN == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "FileDSN", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "FileDSN");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "FileDSN", mTCBase.FileDSN);
				}

				if ((mTCBase.UserID == null) || mTCBase.UserID == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "UserID", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "UserID");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "UserID", mTCBase.UserID);
				}
				if ((mTCBase.Password == null) || mTCBase.Password == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "Password", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "Password");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "Password", mTCBase.Password);
				}

				if ((mTCBase.ImagePath == null) || mTCBase.ImagePath == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ImagePath", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ImagePath");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ImagePath", mTCBase.ImagePath);
				}
				if ((mTCBase.ReportsDirectory == null) || mTCBase.ReportsDirectory == bpeNullString) {
					if (GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ReportsDirectory", bpeNullString) != bpeNullString) {
						try {
							mSupport.Registry.DeleteRegistryKeyValue(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ReportsDirectory");
						} catch (Exception ex) {
						}
					}
				} else {
					SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ReportsDirectory", mTCBase.ReportsDirectory);
				}

				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "TraceMode", false);
				//mSupport.Trace.TraceMode)
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "TraceFile", mSupport.Trace.TraceFile);
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "TraceOptions", Convert.ToInt32(mSupport.Trace.TraceOptions));
				this.Close();
			} finally {
				this.Cursor = Cursors.Default;
			}
		}
		private void frmOptions_Activated(System.Object sender, System.EventArgs e)
		{
			fActivated = true;
		}
		private void frmOptions_Load(System.Object sender, System.EventArgs e)
		{
			//Background Image
			this.txtBackground.Text = ParsePath(mTCBase.ImagePath, ParseParts.FileNameBaseExt);
			EnableControl(this.btnBrowseBackgroundImage, true, false);

			//Data Source
			this.txtConnectionString.Text = mTCBase.ConnectionString;
			this.txtFileDSN.Text = ParsePath(mTCBase.FileDSN, ParseParts.FileNameBaseExt);
			this.txtUserID.Text = mTCBase.UserID;
			this.txtPassword.Text = mTCBase.Password;
			this.rbConnectionString.Checked = Convert.ToBoolean(Strings.Trim(mTCBase.ConnectionString) != bpeNullString);
			this.rbFileDSN.Checked = !this.rbConnectionString.Checked;
			this.rbDataSource_CheckChanged(this.rbConnectionString, new System.EventArgs());

			//Reports Directory
			this.txtReportsDirectory.Text = mTCBase.ReportsDirectory;
			EnableControl(this.btnBrowseReportsDirectory, true, false);
		}
		private void rbDataSource_CheckChanged(object sender, System.EventArgs e)
		{
			EnableControl(this.txtConnectionString, this.rbConnectionString.Checked, false);
			EnableControl(this.btnBrowseFileDSN, this.rbFileDSN.Checked, false);
			EnableControl(this.txtUserID, this.rbFileDSN.Checked, false);
			EnableControl(this.txtPassword, this.rbFileDSN.Checked, false);
		}
		private void txtBackground_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool Cancel = e.Cancel;
			if (Strings.Trim(txtBackground.Text) == bpeNullString)
				Cancel = true;
			e.Cancel = Cancel;
		}
		private void txtConnectionString_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool Cancel = e.Cancel;
			if (Strings.Trim(txtConnectionString.Text) == bpeNullString) {
				EnableControl(btnBrowseFileDSN, true, false);
				EnableControl(txtConnectionString, false, false);
				mTCBase.ConnectionString = Strings.Trim(txtConnectionString.Text);
			} else if (Strings.Trim(txtFileDSN.Text) == bpeNullString) {
				EnableControl(btnBrowseFileDSN, false, false);
				EnableControl(txtConnectionString, true, false);
				mTCBase.ConnectionString = Strings.Trim(txtConnectionString.Text);
			} else {
				Interaction.MsgBox("Either Connection String or File DSN must be specified, but not both!", MsgBoxStyle.Exclamation, this.Text);
				Cancel = true;
			}
			e.Cancel = Cancel;
		}
		private void txtFileDSN_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool Cancel = e.Cancel;
			if (Strings.Trim(txtFileDSN.Text) == bpeNullString && Strings.Trim(txtConnectionString.Text) == bpeNullString) {
				Cancel = true;
			} else if (Strings.Trim(txtFileDSN.Text) == bpeNullString) {
				EnableControl(btnBrowseFileDSN, false, false);
				EnableControl(txtConnectionString, true, false);
				mTCBase.FileDSN = Strings.Trim(txtFileDSN.Text);
				txtConnectionString.Focus();
			} else if (Strings.Trim(txtConnectionString.Text) == bpeNullString) {
				EnableControl(btnBrowseFileDSN, true, false);
				EnableControl(txtConnectionString, false, false);
			} else {
				Interaction.MsgBox("Either File DSN or Connection String must be specified, but not both!", MsgBoxStyle.Exclamation, this.Text);
				Cancel = true;
			}
			e.Cancel = Cancel;
		}
		private void txtPassword_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool Cancel = e.Cancel;
			mTCBase.Password = txtPassword.Text;
			e.Cancel = Cancel;
		}
		private void txtUserID_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool Cancel = e.Cancel;
			mTCBase.UserID = txtUserID.Text;
			e.Cancel = Cancel;
		}
		#endregion
	}
}
