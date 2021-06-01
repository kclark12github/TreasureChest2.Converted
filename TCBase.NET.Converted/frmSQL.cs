//frmSQL.cs
//   SQL Query Interface...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/27/19    Added logic to adjust form placement to account for preferences from a device with a size larger than the 
//               current device so this form would always displays on the current viewport;
//TODO:   07/21/19    Changed form positioning from being independent to being centered on the parent form;
//   09/18/16    Updated object references to reflect architectural changes;
//   10/19/14    Upgraded to VS2013;
//   07/26/10    Reorganized registry settings;
//   11/27/09    Converted to VB.NET;
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
using static TCBase.clsString;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
namespace TCBase
{
	internal class frmSQL : frmTCBase
	{
		public frmSQL() : base()
		{
			Load += frmSQL_Load;
			Closing += frmSQL_Closing;
			Closed += frmSQL_Closed;
			Activated += frmSQL_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
			DefaultGridBorderStyle = dgResults.BorderStyle;
			//Add any initialization after the InitializeComponent() call
			timClock.Enabled = true;
			//This ensures that the clock isn't activated until run-time...
			ShowStatusPanel(sbStatusPanelEnum.Status, true);
			ShowStatusPanel(sbStatusPanelEnum.DB, false);
			ShowStatusPanel(sbStatusPanelEnum.Count, false);
			ShowStatusPanel(sbStatusPanelEnum.Message, true);
			ShowStatusPanel(sbStatusPanelEnum.Timeout, false);
			ShowStatusPanel(sbStatusPanelEnum.Time, true);
			ShowStatusPanel(sbStatusPanelEnum.EndBorder, true);
		}
		public frmSQL(clsSupport objSupport, clsTCBase objTCBase, Form objParent = null, string Caption = null) : base(objSupport, "frmSQL", objTCBase, objParent)
		{
			Load += frmSQL_Load;
			Closing += frmSQL_Closing;
			Closed += frmSQL_Closed;
			Activated += frmSQL_Activated;

			InitializeComponent();
			if ((Caption != null))
				this.Text = Caption;
			Setup();

			mRegistryKey = string.Format("{0}\\{1} Settings\\SQL Settings", mTCBase.RegistryKey, mTCBase.ActiveForm.Name);
		}

		#region "Windows Form Designer generated code "
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ToolTip tooltipSQL;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		private System.Windows.Forms.Timer withEventsField_timClock;
		protected internal System.Windows.Forms.Timer timClock {
			get { return withEventsField_timClock; }
			set {
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick -= timClock_Tick;
				}
				withEventsField_timClock = value;
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick += timClock_Tick;
				}
			}
		}
		internal System.Windows.Forms.Splitter splitToolbar;
		internal System.Windows.Forms.Panel panelSQL;
		public System.Windows.Forms.GroupBox frameSQL;
		private System.Windows.Forms.TextBox withEventsField_txtSQL;
		public System.Windows.Forms.TextBox txtSQL {
			get { return withEventsField_txtSQL; }
			set {
				if (withEventsField_txtSQL != null) {
					withEventsField_txtSQL.Enter -= txtSQL_Enter;
					withEventsField_txtSQL.KeyPress -= txtSQL_KeyPress;
					withEventsField_txtSQL.Leave -= txtSQL_Leave;
					withEventsField_txtSQL.TextChanged -= txtSQL_TextChanged;
				}
				withEventsField_txtSQL = value;
				if (withEventsField_txtSQL != null) {
					withEventsField_txtSQL.Enter += txtSQL_Enter;
					withEventsField_txtSQL.KeyPress += txtSQL_KeyPress;
					withEventsField_txtSQL.Leave += txtSQL_Leave;
					withEventsField_txtSQL.TextChanged += txtSQL_TextChanged;
				}
			}
		}
		internal System.Windows.Forms.Splitter splitSQL;
		internal System.Windows.Forms.MainMenu menuMain;
		private System.Windows.Forms.ComboBox withEventsField_cbQuery;
		internal System.Windows.Forms.ComboBox cbQuery {
			get { return withEventsField_cbQuery; }
			set {
				if (withEventsField_cbQuery != null) {
					withEventsField_cbQuery.SelectedValueChanged -= cbQuery_SelectedValueChanged;
				}
				withEventsField_cbQuery = value;
				if (withEventsField_cbQuery != null) {
					withEventsField_cbQuery.SelectedValueChanged += cbQuery_SelectedValueChanged;
				}
			}
		}
		internal System.Windows.Forms.MenuItem menuQueryFilesBook;
		internal System.Windows.Forms.OpenFileDialog dlgOpen;
		internal System.Windows.Forms.SaveFileDialog dlgSave;
		internal System.Windows.Forms.ImageList imgSQL;
		internal System.Windows.Forms.ToolBarButton cmdSep1;
		internal System.Windows.Forms.ToolBarButton cmdSep2;
		internal System.Windows.Forms.ToolBarButton cmdFileNew;
		internal System.Windows.Forms.ToolBarButton cmdFileSave;
		internal System.Windows.Forms.ToolBarButton cmdEditCopy;
		internal System.Windows.Forms.ToolBarButton cmdQueryExecute;
		internal System.Windows.Forms.ToolBarButton cmdFileOpen;
		internal System.Windows.Forms.ToolBarButton cmdFileSaveAs;
		internal System.Windows.Forms.ToolBarButton cmdEditCut;
		internal System.Windows.Forms.ToolBarButton cmdEditPaste;
		private System.Windows.Forms.ComboBox withEventsField_cbColumns;
		internal System.Windows.Forms.ComboBox cbColumns {
			get { return withEventsField_cbColumns; }
			set {
				if (withEventsField_cbColumns != null) {
					withEventsField_cbColumns.SelectedValueChanged -= cbColumns_SelectedValueChanged;
				}
				withEventsField_cbColumns = value;
				if (withEventsField_cbColumns != null) {
					withEventsField_cbColumns.SelectedValueChanged += cbColumns_SelectedValueChanged;
				}
			}
		}
		private System.Windows.Forms.ComboBox withEventsField_cbTables;
		internal System.Windows.Forms.ComboBox cbTables {
			get { return withEventsField_cbTables; }
			set {
				if (withEventsField_cbTables != null) {
					withEventsField_cbTables.SelectedValueChanged -= cbTables_SelectedValueChanged;
				}
				withEventsField_cbTables = value;
				if (withEventsField_cbTables != null) {
					withEventsField_cbTables.SelectedValueChanged += cbTables_SelectedValueChanged;
				}
			}
		}
		private System.Windows.Forms.ToolBar withEventsField_tbSQL;
		internal System.Windows.Forms.ToolBar tbSQL {
			get { return withEventsField_tbSQL; }
			set {
				if (withEventsField_tbSQL != null) {
					withEventsField_tbSQL.ButtonClick -= tbSQL_ButtonClick;
				}
				withEventsField_tbSQL = value;
				if (withEventsField_tbSQL != null) {
					withEventsField_tbSQL.ButtonClick += tbSQL_ButtonClick;
				}
			}
		}
		internal System.Windows.Forms.ToolBarButton cmdQueryStore;
		internal System.Windows.Forms.ToolBarButton cmdQueryExport;
		internal System.Windows.Forms.ToolBarButton cmdQueryFilesBook;
		internal System.Windows.Forms.Splitter splitStatus;
		internal System.Windows.Forms.Panel panelStatus;
		protected System.Windows.Forms.StatusBar sbStatus;
		internal System.Windows.Forms.StatusBarPanel Status;
		internal System.Windows.Forms.StatusBarPanel DB;
		internal System.Windows.Forms.StatusBarPanel Count;
		internal System.Windows.Forms.StatusBarPanel Message;
		internal System.Windows.Forms.StatusBarPanel Timeout;
		internal System.Windows.Forms.StatusBarPanel Time;
		internal System.Windows.Forms.StatusBarPanel EndBorder;
		internal System.Windows.Forms.TabControl tabResults;
		internal System.Windows.Forms.TabPage tabpageMessages;
		internal System.Windows.Forms.RichTextBox rtfResults;
		internal System.Windows.Forms.TabPage tabpageGrid;
		private System.Windows.Forms.DataGrid withEventsField_dgResults;
		internal System.Windows.Forms.DataGrid dgResults {
			get { return withEventsField_dgResults; }
			set {
				if (withEventsField_dgResults != null) {
					withEventsField_dgResults.CurrentCellChanged -= dgResults_CurrentCellChanged;
					withEventsField_dgResults.SizeChanged -= dgResults_SizeChanged;
				}
				withEventsField_dgResults = value;
				if (withEventsField_dgResults != null) {
					withEventsField_dgResults.CurrentCellChanged += dgResults_CurrentCellChanged;
					withEventsField_dgResults.SizeChanged += dgResults_SizeChanged;
				}
			}
		}
		internal System.Windows.Forms.Label lblStoredQueries;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSQL));
			this.timClock = new System.Windows.Forms.Timer(this.components);
			this.tooltipSQL = new System.Windows.Forms.ToolTip(this.components);
			this.tbSQL = new System.Windows.Forms.ToolBar();
			this.cmdFileNew = new System.Windows.Forms.ToolBarButton();
			this.cmdFileOpen = new System.Windows.Forms.ToolBarButton();
			this.cmdFileSave = new System.Windows.Forms.ToolBarButton();
			this.cmdFileSaveAs = new System.Windows.Forms.ToolBarButton();
			this.cmdSep1 = new System.Windows.Forms.ToolBarButton();
			this.cmdEditCut = new System.Windows.Forms.ToolBarButton();
			this.cmdEditCopy = new System.Windows.Forms.ToolBarButton();
			this.cmdEditPaste = new System.Windows.Forms.ToolBarButton();
			this.cmdSep2 = new System.Windows.Forms.ToolBarButton();
			this.cmdQueryExecute = new System.Windows.Forms.ToolBarButton();
			this.cmdQueryExport = new System.Windows.Forms.ToolBarButton();
			this.cmdQueryStore = new System.Windows.Forms.ToolBarButton();
			this.cmdQueryFilesBook = new System.Windows.Forms.ToolBarButton();
			this.imgSQL = new System.Windows.Forms.ImageList(this.components);
			this.splitToolbar = new System.Windows.Forms.Splitter();
			this.panelSQL = new System.Windows.Forms.Panel();
			this.tabResults = new System.Windows.Forms.TabControl();
			this.tabpageGrid = new System.Windows.Forms.TabPage();
			this.dgResults = new System.Windows.Forms.DataGrid();
			this.tabpageMessages = new System.Windows.Forms.TabPage();
			this.rtfResults = new System.Windows.Forms.RichTextBox();
			this.splitSQL = new System.Windows.Forms.Splitter();
			this.frameSQL = new System.Windows.Forms.GroupBox();
			this.txtSQL = new System.Windows.Forms.TextBox();
			this.splitStatus = new System.Windows.Forms.Splitter();
			this.menuMain = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuFileSave = new System.Windows.Forms.MenuItem();
			this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
			this.menuFileSep1 = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.menuEdit = new System.Windows.Forms.MenuItem();
			this.menuEditCut = new System.Windows.Forms.MenuItem();
			this.menuEditCopy = new System.Windows.Forms.MenuItem();
			this.menuEditPaste = new System.Windows.Forms.MenuItem();
			this.menuQuery = new System.Windows.Forms.MenuItem();
			this.menuQueryExecute = new System.Windows.Forms.MenuItem();
			this.menuQuerySep1 = new System.Windows.Forms.MenuItem();
			this.menuQueryExport = new System.Windows.Forms.MenuItem();
			this.menuQueryStore = new System.Windows.Forms.MenuItem();
			this.menuQueryFilesBook = new System.Windows.Forms.MenuItem();
			this.cbQuery = new System.Windows.Forms.ComboBox();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.cbColumns = new System.Windows.Forms.ComboBox();
			this.cbTables = new System.Windows.Forms.ComboBox();
			this.panelStatus = new System.Windows.Forms.Panel();
			this.sbStatus = new System.Windows.Forms.StatusBar();
			this.Status = new System.Windows.Forms.StatusBarPanel();
			this.DB = new System.Windows.Forms.StatusBarPanel();
			this.Count = new System.Windows.Forms.StatusBarPanel();
			this.Message = new System.Windows.Forms.StatusBarPanel();
			this.Timeout = new System.Windows.Forms.StatusBarPanel();
			this.Time = new System.Windows.Forms.StatusBarPanel();
			this.EndBorder = new System.Windows.Forms.StatusBarPanel();
			this.lblStoredQueries = new System.Windows.Forms.Label();
			this.panelSQL.SuspendLayout();
			this.tabResults.SuspendLayout();
			this.tabpageGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgResults).BeginInit();
			this.tabpageMessages.SuspendLayout();
			this.frameSQL.SuspendLayout();
			this.panelStatus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.Status).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.DB).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.Count).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.Message).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.Timeout).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.Time).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.EndBorder).BeginInit();
			this.SuspendLayout();
			//
			//timClock
			//
			this.timClock.Enabled = true;
			this.timClock.Interval = 1000;
			//
			//tbSQL
			//
			this.tbSQL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbSQL.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
				this.cmdFileNew,
				this.cmdFileOpen,
				this.cmdFileSave,
				this.cmdFileSaveAs,
				this.cmdSep1,
				this.cmdEditCut,
				this.cmdEditCopy,
				this.cmdEditPaste,
				this.cmdSep2,
				this.cmdQueryExecute,
				this.cmdQueryExport,
				this.cmdQueryStore,
				this.cmdQueryFilesBook
			});
			this.tbSQL.DropDownArrows = true;
			this.tbSQL.Location = new System.Drawing.Point(0, 0);
			this.tbSQL.Name = "tbSQL";
			this.tbSQL.ShowToolTips = true;
			this.tbSQL.Size = new System.Drawing.Size(716, 29);
			this.tbSQL.TabIndex = 12;
			//
			//cmdFileNew
			//
			this.cmdFileNew.ImageIndex = 3;
			this.cmdFileNew.ToolTipText = "New";
			//
			//cmdFileOpen
			//
			this.cmdFileOpen.ImageIndex = 9;
			this.cmdFileOpen.ToolTipText = "Open";
			//
			//cmdFileSave
			//
			this.cmdFileSave.ImageIndex = 5;
			this.cmdFileSave.ToolTipText = "Save";
			//
			//cmdFileSaveAs
			//
			this.cmdFileSaveAs.ImageIndex = 4;
			this.cmdFileSaveAs.ToolTipText = "Save As";
			this.cmdFileSaveAs.Visible = false;
			//
			//cmdSep1
			//
			this.cmdSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			//
			//cmdEditCut
			//
			this.cmdEditCut.ImageIndex = 7;
			this.cmdEditCut.ToolTipText = "Cut";
			this.cmdEditCut.Visible = false;
			//
			//cmdEditCopy
			//
			this.cmdEditCopy.ImageIndex = 2;
			this.cmdEditCopy.ToolTipText = "Copy";
			this.cmdEditCopy.Visible = false;
			//
			//cmdEditPaste
			//
			this.cmdEditPaste.ImageIndex = 8;
			this.cmdEditPaste.ToolTipText = "Paste";
			this.cmdEditPaste.Visible = false;
			//
			//cmdSep2
			//
			this.cmdSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			this.cmdSep2.Visible = false;
			//
			//cmdQueryExecute
			//
			this.cmdQueryExecute.ImageIndex = 1;
			this.cmdQueryExecute.ToolTipText = "Execute";
			//
			//cmdQueryExport
			//
			this.cmdQueryExport.ToolTipText = "Export";
			//
			//cmdQueryStore
			//
			this.cmdQueryStore.ToolTipText = "Store";
			//
			//cmdQueryFilesBook
			//
			this.cmdQueryFilesBook.ToolTipText = "FilesBook";
			//
			//imgSQL
			//
			this.imgSQL.ImageSize = new System.Drawing.Size(16, 16);
			this.imgSQL.TransparentColor = System.Drawing.Color.Transparent;
			//
			//splitToolbar
			//
			this.splitToolbar.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitToolbar.Enabled = false;
			this.splitToolbar.Location = new System.Drawing.Point(0, 29);
			this.splitToolbar.Name = "splitToolbar";
			this.splitToolbar.Size = new System.Drawing.Size(716, 34);
			this.splitToolbar.TabIndex = 14;
			this.splitToolbar.TabStop = false;
			//
			//panelSQL
			//
			this.panelSQL.Controls.Add(this.tabResults);
			this.panelSQL.Controls.Add(this.splitSQL);
			this.panelSQL.Controls.Add(this.frameSQL);
			this.panelSQL.Controls.Add(this.splitStatus);
			this.panelSQL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSQL.Location = new System.Drawing.Point(0, 63);
			this.panelSQL.Name = "panelSQL";
			this.panelSQL.Size = new System.Drawing.Size(716, 299);
			this.panelSQL.TabIndex = 15;
			//
			//tabResults
			//
			this.tabResults.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabResults.Controls.Add(this.tabpageGrid);
			this.tabResults.Controls.Add(this.tabpageMessages);
			this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabResults.ImageList = this.imgSQL;
			this.tabResults.Location = new System.Drawing.Point(0, 112);
			this.tabResults.Name = "tabResults";
			this.tabResults.SelectedIndex = 0;
			this.tabResults.Size = new System.Drawing.Size(716, 159);
			this.tabResults.TabIndex = 18;
			//
			//tabpageGrid
			//
			this.tabpageGrid.Controls.Add(this.dgResults);
			this.tabpageGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.tabpageGrid.Location = new System.Drawing.Point(4, 4);
			this.tabpageGrid.Name = "tabpageGrid";
			this.tabpageGrid.Size = new System.Drawing.Size(708, 130);
			this.tabpageGrid.TabIndex = 0;
			this.tabpageGrid.Text = "Grid";
			//
			//dgResults
			//
			this.dgResults.CaptionFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
			this.dgResults.CaptionText = "Results";
			this.dgResults.DataMember = "";
			this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgResults.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dgResults.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgResults.Location = new System.Drawing.Point(0, 0);
			this.dgResults.Name = "dgResults";
			this.dgResults.ReadOnly = true;
			this.dgResults.Size = new System.Drawing.Size(708, 130);
			this.dgResults.TabIndex = 16;
			//
			//tabpageMessages
			//
			this.tabpageMessages.Controls.Add(this.rtfResults);
			this.tabpageMessages.Location = new System.Drawing.Point(4, 4);
			this.tabpageMessages.Name = "tabpageMessages";
			this.tabpageMessages.Size = new System.Drawing.Size(708, 130);
			this.tabpageMessages.TabIndex = 1;
			this.tabpageMessages.Text = "Messages";
			//
			//rtfResults
			//
			this.rtfResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtfResults.Location = new System.Drawing.Point(0, 0);
			this.rtfResults.Name = "rtfResults";
			this.rtfResults.ReadOnly = true;
			this.rtfResults.Size = new System.Drawing.Size(708, 130);
			this.rtfResults.TabIndex = 17;
			this.rtfResults.Text = "";
			//
			//splitSQL
			//
			this.splitSQL.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitSQL.Location = new System.Drawing.Point(0, 108);
			this.splitSQL.Name = "splitSQL";
			this.splitSQL.Size = new System.Drawing.Size(716, 4);
			this.splitSQL.TabIndex = 14;
			this.splitSQL.TabStop = false;
			//
			//frameSQL
			//
			this.frameSQL.BackColor = System.Drawing.SystemColors.Control;
			this.frameSQL.Controls.Add(this.txtSQL);
			this.frameSQL.Dock = System.Windows.Forms.DockStyle.Top;
			this.frameSQL.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.frameSQL.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frameSQL.Location = new System.Drawing.Point(0, 0);
			this.frameSQL.Name = "frameSQL";
			this.frameSQL.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frameSQL.Size = new System.Drawing.Size(716, 108);
			this.frameSQL.TabIndex = 1;
			this.frameSQL.TabStop = false;
			this.frameSQL.Text = "SQL Statement";
			//
			//txtSQL
			//
			this.txtSQL.AcceptsReturn = true;
			this.txtSQL.AcceptsTab = true;
			this.txtSQL.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtSQL.AutoSize = false;
			this.txtSQL.BackColor = System.Drawing.SystemColors.Window;
			this.txtSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtSQL.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtSQL.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtSQL.Location = new System.Drawing.Point(4, 15);
			this.txtSQL.MaxLength = 0;
			this.txtSQL.Multiline = true;
			this.txtSQL.Name = "txtSQL";
			this.txtSQL.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtSQL.Size = new System.Drawing.Size(708, 89);
			this.txtSQL.TabIndex = 0;
			this.txtSQL.Text = "";
			this.txtSQL.WordWrap = false;
			//
			//splitStatus
			//
			this.splitStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitStatus.Location = new System.Drawing.Point(0, 271);
			this.splitStatus.Name = "splitStatus";
			this.splitStatus.Size = new System.Drawing.Size(716, 28);
			this.splitStatus.TabIndex = 17;
			this.splitStatus.TabStop = false;
			//
			//menuFile
			//
			this.menuFile.Index = -1;
			this.menuFile.Text = "";
			//
			//menuFileOpen
			//
			this.menuFileOpen.Index = -1;
			this.menuFileOpen.Text = "";
			//
			//menuFileSave
			//
			this.menuFileSave.Index = -1;
			this.menuFileSave.Text = "";
			//
			//menuFileSaveAs
			//
			this.menuFileSaveAs.Index = -1;
			this.menuFileSaveAs.Text = "";
			//
			//menuFileSep1
			//
			this.menuFileSep1.Index = -1;
			this.menuFileSep1.Text = "";
			//
			//menuFileExit
			//
			this.menuFileExit.Index = -1;
			this.menuFileExit.Text = "";
			//
			//menuEdit
			//
			this.menuEdit.Index = -1;
			this.menuEdit.Text = "";
			//
			//menuEditCut
			//
			this.menuEditCut.Index = -1;
			this.menuEditCut.Text = "";
			//
			//menuEditCopy
			//
			this.menuEditCopy.Index = -1;
			this.menuEditCopy.Text = "";
			//
			//menuEditPaste
			//
			this.menuEditPaste.Index = -1;
			this.menuEditPaste.Text = "";
			//
			//menuQuery
			//
			this.menuQuery.Index = -1;
			this.menuQuery.Text = "";
			//
			//menuQueryExecute
			//
			this.menuQueryExecute.Index = -1;
			this.menuQueryExecute.Text = "";
			//
			//menuQuerySep1
			//
			this.menuQuerySep1.Index = -1;
			this.menuQuerySep1.Text = "";
			//
			//menuQueryExport
			//
			this.menuQueryExport.Index = -1;
			this.menuQueryExport.Text = "";
			//
			//menuQueryStore
			//
			this.menuQueryStore.Index = -1;
			this.menuQueryStore.Text = "";
			//
			//menuQueryFilesBook
			//
			this.menuQueryFilesBook.Index = -1;
			this.menuQueryFilesBook.Text = "";
			//
			//cbQuery
			//
			this.cbQuery.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbQuery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbQuery.Location = new System.Drawing.Point(288, 4);
			this.cbQuery.Name = "cbQuery";
			this.cbQuery.Size = new System.Drawing.Size(412, 24);
			this.cbQuery.TabIndex = 0;
			//
			//cbColumns
			//
			this.cbColumns.Location = new System.Drawing.Point(364, 36);
			this.cbColumns.Name = "cbColumns";
			this.cbColumns.Size = new System.Drawing.Size(336, 24);
			this.cbColumns.TabIndex = 21;
			this.cbColumns.TabStop = false;
			//
			//cbTables
			//
			this.cbTables.Location = new System.Drawing.Point(4, 36);
			this.cbTables.Name = "cbTables";
			this.cbTables.Size = new System.Drawing.Size(344, 24);
			this.cbTables.TabIndex = 22;
			this.cbTables.TabStop = false;
			//
			//panelStatus
			//
			this.panelStatus.Controls.Add(this.sbStatus);
			this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelStatus.Location = new System.Drawing.Point(0, 334);
			this.panelStatus.Name = "panelStatus";
			this.panelStatus.Size = new System.Drawing.Size(716, 28);
			this.panelStatus.TabIndex = 26;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 4);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
				this.Status,
				this.DB,
				this.Count,
				this.Message,
				this.Timeout,
				this.Time,
				this.EndBorder
			});
			this.sbStatus.ShowPanels = true;
			this.sbStatus.Size = new System.Drawing.Size(716, 24);
			this.sbStatus.TabIndex = 24;
			//
			//Status
			//
			this.Status.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Status.MinWidth = 64;
			this.Status.Width = 64;
			//
			//DB
			//
			this.DB.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.DB.MinWidth = 0;
			this.DB.Width = 10;
			//
			//Count
			//
			this.Count.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Count.MinWidth = 0;
			this.Count.Width = 10;
			//
			//Message
			//
			this.Message.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.Message.MinWidth = 64;
			this.Message.Width = 541;
			//
			//Timeout
			//
			this.Timeout.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Timeout.MinWidth = 0;
			this.Timeout.Width = 10;
			//
			//Time
			//
			this.Time.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.Time.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Time.MinWidth = 64;
			this.Time.Width = 64;
			//
			//EndBorder
			//
			this.EndBorder.MinWidth = 1;
			this.EndBorder.Width = 1;
			//
			//lblStoredQueries
			//
			this.lblStoredQueries.AutoSize = true;
			this.lblStoredQueries.Location = new System.Drawing.Point(180, 8);
			this.lblStoredQueries.Name = "lblStoredQueries";
			this.lblStoredQueries.Size = new System.Drawing.Size(105, 19);
			this.lblStoredQueries.TabIndex = 27;
			this.lblStoredQueries.Text = "Stored Queries";
			//
			//frmSQL
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(716, 362);
			this.Controls.Add(this.lblStoredQueries);
			this.Controls.Add(this.panelStatus);
			this.Controls.Add(this.cbTables);
			this.Controls.Add(this.cbColumns);
			this.Controls.Add(this.cbQuery);
			this.Controls.Add(this.panelSQL);
			this.Controls.Add(this.splitToolbar);
			this.Controls.Add(this.tbSQL);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Location = new System.Drawing.Point(3, 18);
			this.Menu = this.menuMain;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(724, 396);
			this.Name = "frmSQL";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SQL";
			this.panelSQL.ResumeLayout(false);
			this.tabResults.ResumeLayout(false);
			this.tabpageGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgResults).EndInit();
			this.tabpageMessages.ResumeLayout(false);
			this.frameSQL.ResumeLayout(false);
			this.panelStatus.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.Status).EndInit();
			((System.ComponentModel.ISupportInitialize)this.DB).EndInit();
			((System.ComponentModel.ISupportInitialize)this.Count).EndInit();
			((System.ComponentModel.ISupportInitialize)this.Message).EndInit();
			((System.ComponentModel.ISupportInitialize)this.Timeout).EndInit();
			((System.ComponentModel.ISupportInitialize)this.Time).EndInit();
			((System.ComponentModel.ISupportInitialize)this.EndBorder).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Enumerations"
		public enum dbeAccessEnum
		{
			dbaReadOnly = 0,
			dbaDML = 1,
			dbaDDL = 2
		}
		private enum tbSQLButtonEnum : short
		{
			cmdFileNew = 0,
			cmdFileOpen = 1,
			cmdFileSave = 2,
			cmdFileSaveAs = 3,
			//Separator
			cmdEditCut = 5,
			cmdEditCopy = 6,
			cmdEditPaste = 7,
			//Separator
			cmdQueryExecute = 9,
			cmdQueryExport = 10,
			cmdQueryStore = 11,
			cmdQueryFilesBook = 12
		}
		protected new enum sbStatusPanelEnum : int
		{
			Status = 0,
			DB = 1,
			Count = 2,
			Message = 3,
			Timeout = 4,
			Time = 5,
			EndBorder = 6
		}
		#endregion
		#region "Declarations"
		private double BufferLimit;
		private BorderStyle DefaultGridBorderStyle;
		private DataGridTableStyle dgTableStyle;
		private DataSet dsDD;
		private DataView dvColumn;
		private DataView dvQuery;
		private DataView dvTable;
		private DataSet dsSQL;
		private DataTable dtSQL;
		private DataView dvSQL;
		private bool fActivated = false;
		private dbeAccessEnum mAccessLevel = dbeAccessEnum.dbaReadOnly;
		private bool mAllowStore = false;
		private string mCaption = "SQL Interface";
		private string mFilesBookReport = bpeNullString;
		private string mRegistryKey;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuFileNew;
		private System.Windows.Forms.MenuItem menuFileOpen;
		private System.Windows.Forms.MenuItem menuFileSave;
		private System.Windows.Forms.MenuItem menuFileSaveAs;
		private System.Windows.Forms.MenuItem menuFileSep1;
		private System.Windows.Forms.MenuItem menuQueryDelete;
		private System.Windows.Forms.MenuItem menuQueryExport;
		private System.Windows.Forms.MenuItem menuQueryStore;
		private System.Windows.Forms.MenuItem menuQueryUpdate;
		private System.Windows.Forms.MenuItem menuEdit;
		private System.Windows.Forms.MenuItem menuEditCut;
		private System.Windows.Forms.MenuItem menuEditCopy;
		private System.Windows.Forms.MenuItem menuEditPaste;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Windows.Forms.MenuItem menuQuery;
		private System.Windows.Forms.MenuItem menuQueryExecute;
		private System.Windows.Forms.MenuItem menuQuerySep1;
		private string mOriginalText;
		private string mPathName;
			#endregion
		private int mRecordsAffected;
		public dbeAccessEnum AccessLevel {
			get { return mAccessLevel; }
			set { mAccessLevel = value; }
		}
		public bool AllowStore {
			get { return mAllowStore; }
			set {
				mAllowStore = value;
				menuQueryStore.Visible = mAllowStore;
			}
		}
		public string Caption {
			get { return mCaption; }
			set { mCaption = value; }
		}
		public string FilesBookReport {
			get { return mFilesBookReport; }
			set { mFilesBookReport = value; }
		}
		#endregion
		#region "Methods"
		private int GetDataWidth(DataGridColumnStyle ColumnStyle)
		{
			int Width = 0;
			Graphics g = dgResults.CreateGraphics();
			int offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", dgResults.Font).Width));
			Width = Convert.ToInt32(Math.Ceiling(g.MeasureString(ColumnStyle.HeaderText, dgResults.HeaderFont).Width));
			for (int i = 0; i <= dvSQL.Table.Rows.Count - 1; i++) {
				int iWidth = Convert.ToInt32(Math.Ceiling(g.MeasureString(dvSQL.Table.Rows[i][ColumnStyle.MappingName].ToString(), dgResults.Font).Width));
				if (iWidth > Width)
					Width = iWidth;
			}
			return Width + (offset * 4);
		}
		private int GetHeaderWidth(DataGridColumnStyle ColumnStyle)
		{
			Graphics g = dgResults.CreateGraphics();
			int offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", dgResults.HeaderFont).Width));
			return Convert.ToInt32(Math.Ceiling(g.MeasureString(ColumnStyle.HeaderText, dgResults.HeaderFont).Width)) + (offset * 4);
		}
		protected DataGridColumnStyle SetupDataGridBoolColumn(string HeaderText, string MappingName, float Width = -1, HorizontalAlignment Alignment = HorizontalAlignment.Left)
		{
			DataGridColumnStyle functionReturnValue = null;
			DataGridBoolColumn Column = new DataGridBoolColumn();
			functionReturnValue = null;
            Column.HeaderText = HeaderText;
            Column.MappingName = MappingName;
			switch (Width) {
				case -1:
                    Column.Width = GetHeaderWidth(Column);
					break;
				default:
                    Column.Width = (int)Width;
					break;
			}
            Column.Alignment = Alignment;
            Column.FalseValue = false;
            Column.TrueValue = true;
            Column.NullValue = Convert.DBNull;
            Column.NullText = "";
			functionReturnValue = Column;
			return functionReturnValue;
		}
		protected DataGridColumnStyle SetupDataGridTextBoxColumn(string HeaderText, string MappingName, string Format = "", float Width = -2, HorizontalAlignment Alignment = HorizontalAlignment.Left)
		{
			DataGridColumnStyle functionReturnValue = null;
			DataGridTextBoxColumn Column = new DataGridTextBoxColumn();
			functionReturnValue = null;
            Column.HeaderText = HeaderText;
            Column.MappingName = MappingName;
			if (!string.IsNullOrEmpty(Format))
                Column.Format = Format;
			switch (Width) {
				case -1:
                    Column.Width = (this.WindowState == FormWindowState.Maximized ? GetDataWidth(Column) : GetHeaderWidth(Column));
					break;
				case -2:
                    Column.Width = GetDataWidth(Column);
					break;
				default:
                    Column.Width = (int)Width;
					break;
			}
            Column.Alignment = Alignment;
            DataGridTextBox textBox = (DataGridTextBox)Column.TextBox;
            textBox.AutoSize = true;
            textBox.HideSelection = true;
            textBox.ReadOnly = true;
            //.Enabled = False
            textBox.BackColor = Color.Teal;
            textBox.ForeColor = Color.PaleGreen;
			functionReturnValue = Column;
			return functionReturnValue;
		}
		//    Protected Sub AddGridColumnStyle(ByVal MappingName As String, ByVal HeaderText As String, Optional ByVal Alignment As HorizontalAlignment = HorizontalAlignment.Left, Optional ByVal Format As String = bpeNullString, Optional ByVal ColumnType As String = "DataGridTextBoxColumn")
		//        Try
		//            MyBase.epBase.SetError(Me.dgResults, bpeNullString)

		//            'The use of column styles overrides the automatic generation of columns for every column in the DataTable. 
		//            'When column style objects are used, every column you want to display has to have an associate column style object.
		//            Select Case ColumnType
		//                Case "DataGridBoolColumn"
		//                    Dim dgColumnStyle As New DataGridBoolColumn
		//                    With dgColumnStyle
		//                        .MappingName = MappingName
		//                        .HeaderText = HeaderText
		//                        .ReadOnly = True
		//                        .Alignment = Alignment
		//                        .FalseValue = "False"
		//                        .TrueValue = "True"
		//                    End With
		//                    'Add the style objects to the table style's collection of column styles. Without this the styles do not take effect.        
		//                    dgTableStyle.GridColumnStyles.Add(dgColumnStyle)
		//                Case Else
		//                    Dim dgsText As New DataGridTextBoxColumn
		//                    With dgsText
		//                        .MappingName = MappingName
		//                        .HeaderText = HeaderText
		//                        'Format the data as a date. This removes the time from the DateTime Sql data type.
		//                        '.Format = "d"
		//                        'Format the data as currency.
		//                        '.Format = "c"
		//                        .Format = Format
		//                        .Alignment = Alignment
		//                        .ReadOnly = True
		//                        '.TextBox.Enabled = False
		//                        '.TextBox.HideSelection = True
		//                    End With
		//                    'Add the column style objects to the table style's collection of column styles. Without this the styles do not take effect.        
		//                    dgTableStyle.GridColumnStyles.Add(dgsText)
		//            End Select
		//        Catch ex As Exception
		//            Trace(ex.Message, trcOption.trcLookupDetail)
		//            MyBase.epBase.SetError(Me.dgResults, ex.Message)
		//        End Try
		//    End Sub
		protected override void Busy(bool IsBusy)
		{
			const string EntryName = "Busy";
			Cursor myCursor = (IsBusy ? Cursors.WaitCursor : Cursors.Default);
			bool fEnabled = !IsBusy;
			try {
				this.Cursor = myCursor;
				menuFile.Enabled = fEnabled;
				menuEdit.Enabled = fEnabled;
				menuQuery.Enabled = fEnabled;
				cbQuery.Enabled = fEnabled;
				cbTables.Enabled = fEnabled;
				cbColumns.Enabled = fEnabled;
				txtSQL.Enabled = fEnabled;
				tbSQL.Enabled = fEnabled;
				dgResults.Enabled = fEnabled;
				splitSQL.Enabled = fEnabled;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName, null);
			}
		}
		protected string BytesToHex(byte[] Bytes)
		{
			string functionReturnValue = null;
			const string EntryName = "BytesToHex";
			string HexByte = bpeNullString;
			functionReturnValue = bpeNullString;
			try {
				functionReturnValue = "0x";
				for (byte iByte = (byte)Bytes.GetLowerBound(0); iByte <= Bytes.GetUpperBound(0); iByte++) {
					HexByte = Conversion.Hex(Bytes[iByte]);
					functionReturnValue += (Strings.Len(HexByte) < 2 ? "0" : bpeNullString) + HexByte;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
			return functionReturnValue;
		}
		protected internal bool CheckForDup(string QueryName)
		{
			return Convert.ToBoolean((int)mTCBase.ExecuteScalarCommand(string.Format("Select Count(*) From Query Where Name='{0}'", FixQuotes(QueryName))) > 0);
		}
		protected void CheckSaveState()
		{
			const string EntryName = "CheckSaveState";
			try {
				if (mOriginalText != this.txtSQL.Text) {
					if (!this.Text.EndsWith("*"))
						this.Text += "*";
				} else {
					if (this.Text.EndsWith("*"))
						this.Text = this.Text.Substring(0, this.Text.Length - 1);
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected DialogResult CheckToSave()
		{
            DialogResult functionReturnValue = default(DialogResult);
			const string EntryName = "CheckToSave";
            DialogResult Response = default(DialogResult);
			try {
				if (mOriginalText != this.txtSQL.Text) {
					Response = ShowMsgBox("Query has not been saved. Would you like to save before continuing?", MsgBoxStyle.YesNoCancel, this, this.Caption);
					switch (Response) {
						case DialogResult.Yes:
							Response = SaveQuery(this.txtSQL.Text);
							break;
						case DialogResult.No:
							break;
						case DialogResult.Cancel:
							break;
					}
				}
				functionReturnValue = Response;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
			return functionReturnValue;
		}
		protected void CleanUpSQL(ref string SQLSource)
		{
			try {
				while (SQLSource.EndsWith(Constants.vbCrLf)) {
					SQLSource = SQLSource.Substring(0, SQLSource.Length - 2);
				}
				if (SQLSource.EndsWith(";"))
					SQLSource = SQLSource.Substring(0, SQLSource.Length - 1);
				//Remove trailing ";"
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
			}
		}
		protected void Clear()
		{
			try {
				this.txtSQL.Clear();
				mSupport.UI.Display(bpeNullString, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
				this.tabResults.TabPages.Clear();
				this.tabResults.TabPages.Add(this.tabpageMessages);
				this.dgResults.DataSource = null;
				dvSQL = null;
				dtSQL = null;
				dsSQL.Dispose();
				dsSQL = null;
				mPathName = bpeNullString;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
			}
		}
		protected void Execute(string SQLsource)
		{
			dbeAccessEnum SQLCommandType = dbeAccessEnum.dbaReadOnly;
			bool fResponse = false;
			string strOutput = bpeNullString;
			try {
				CleanUpSQL(ref SQLsource);
				if (Strings.Trim(SQLsource) == bpeNullString)
					throw new ExitTryException();

				mSupport.UI.Display(bpeNullString, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
				this.rtfResults.Focus();
				this.tabResults.TabPages.Clear();
				this.tabResults.TabPages.Add(this.tabpageMessages);

				SQLCommandType = GetSQLCommandType(SQLsource);
				switch (SQLCommandType) {
					case dbeAccessEnum.dbaReadOnly:
						break;
					case dbeAccessEnum.dbaDML:
						if ((mAccessLevel & SQLCommandType) == 0) {
							strOutput = "User does not have the authority to modify data in this database.";
							mSupport.UI.Display(strOutput, DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbRed, TriState.True);
                            //Me.rtfResults.AppendText(strOutput)
                            //ShowMsgBox(strOutput, MsgBoxStyle.Exclamation, Me)
                            throw new ExitTryException();
						}
						break;
					case dbeAccessEnum.dbaDDL:
						if ((mAccessLevel & SQLCommandType) == 0) {
							strOutput = "User does not have the authority change database structure.";
							mSupport.UI.Display(strOutput, DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbRed, TriState.True);
                            //Me.rtfResults.AppendText(strOutput)
                            //ShowMsgBox(strOutput, MsgBoxStyle.Exclamation, Me)
                            throw new ExitTryException();
						}
						break;
				}

				//mTCBase.Update("SET QUOTED_IDENTIFIER ON")

				switch (SQLCommandType) {
					case dbeAccessEnum.dbaReadOnly:
						Busy(true);
						this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "Reading...";
						this.sbStatus.Panels[(int)sbStatusPanelEnum.Count].Text = bpeNullString;
						mSupport.UI.Display(bpeNullString, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
						dvSQL = null;
						if ((dsSQL != null)){dsSQL.Dispose();dsSQL = null;}
						ResetGrid();
						dgTableStyle = null;

						mSupport.UI.Display(SQLsource, DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbBlack, TriState.True);
						//Me.rtfResults.AppendText(SQLsource & vbCrLf)

						dvSQL = mTCBase.MakeDataViewCommand(SQLsource, "", false);
                        dgResults.DataSource = dvSQL;
                        dgResults.AlternatingBackColor = Color.Snow;
                        //Color.NavajoWhite  'Color.MintCream
                        dgResults.BackColor = Color.GhostWhite;
                        dgResults.BackgroundColor = System.Drawing.SystemColors.AppWorkspace;
                        dgResults.BorderStyle = BorderStyle.Fixed3D;
                        dgResults.CaptionBackColor = Color.RoyalBlue;
                        dgResults.CaptionFont = new Font("Verdana", 9.75f, FontStyle.Bold);
                        dgResults.Font = new Font("Verdana", 9.75f);
                        dgResults.ForeColor = Color.MidnightBlue;
                        dgResults.ParentRowsBackColor = System.Drawing.SystemColors.AppWorkspace;
                        dgResults.ParentRowsForeColor = Color.MidnightBlue;
                        dgResults.ReadOnly = true;
						ControlEnabled(dgResults, true);

						dgTableStyle = new DataGridTableStyle();
                        dgTableStyle.AlternatingBackColor = Color.Snow;
                        //Color.NavajoWhite  'Color.MintCream
                        dgTableStyle.BackColor = Color.GhostWhite;
                        dgTableStyle.ForeColor = Color.MidnightBlue;
                        dgTableStyle.GridLineColor = Color.RoyalBlue;
                        dgTableStyle.HeaderBackColor = Color.MidnightBlue;
                        dgTableStyle.HeaderFont = new Font("Verdana", 9.75f, FontStyle.Bold);
                        dgTableStyle.HeaderForeColor = Color.Lavender;
                        dgTableStyle.SelectionBackColor = Color.Teal;
                        dgTableStyle.SelectionForeColor = Color.PaleGreen;

                        //You must always set the MappingName, even with a DataView that has only one Table. If not, you will 
                        //get no errors but the formatting will not appear. 
                        dgTableStyle.MappingName = dvSQL.Table.TableName;
                        dgTableStyle.AllowSorting = true;
                        dgTableStyle.ReadOnly = true;
                        dgTableStyle.PreferredColumnWidth = 125;
                        dgTableStyle.PreferredRowHeight = 15;

                        dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(bpeNullString, bpeNullString, bpeNullString, 0));
						foreach (DataColumn col in dvSQL.Table.Columns) {
							switch (col.DataType.Name) {
								case "Currency":
								case "Decimal":
								case "Integer":
								case "Int16":
								case "Int32":
								case "Int64":
								case "Short":
								case "Long":
								case "Double":
								case "Single":
									if (col.ColumnName == "CONVERT(BIGINT,ROWID)") {
                                        dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName, "X16", -2, HorizontalAlignment.Right));
									} else {
                                        dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName, "", -2, HorizontalAlignment.Right));
									}
									break;
								case "Boolean":
                                    dgTableStyle.GridColumnStyles.Add(SetupDataGridBoolColumn(col.Caption, col.ColumnName, -1, HorizontalAlignment.Center));
									break;
								case "Date":
                                    dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName, mSupport.fmtShortDate, -2, HorizontalAlignment.Center));
									break;
								case "DateTime":
                                    dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName, "", -2, HorizontalAlignment.Center));
									break;
								case "String":
								case "Text":
                                    dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName));
									break;
								default:
                                    dgTableStyle.GridColumnStyles.Add(SetupDataGridTextBoxColumn(col.Caption, col.ColumnName));
									break;
							}
						}

						this.dgResults.TableStyles.Add(dgTableStyle);
						this.dgResults.Focus();
						if (this.dgResults.CurrentRowIndex > 0)
							this.dgResults.Select(this.dgResults.CurrentRowIndex);

						dgResults.Visible = true;
						Busy(false);
						//Make sure this happens before adding the TabPage below or the DataGrid's scroll bars won't be enabled until the form is resized...
						this.tabResults.TabPages.Clear();
						this.tabResults.TabPages.AddRange(new TabPage[] {
							this.tabpageGrid,
							this.tabpageMessages
						});
						mRecordsAffected = dvSQL.Count;
						break;
					default:
						Busy(true);
						this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "Executing...";
						this.sbStatus.Panels[(int)sbStatusPanelEnum.Count].Text = bpeNullString;
						mSupport.UI.Display(bpeNullString, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);

						mSupport.UI.Display("Beginning Transaction...", DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbBlack, TriState.False);
						mTCBase.BeginTrans();
						mSupport.UI.Display(SQLsource, DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbBlack, TriState.True);
						Update(ref SQLsource, mRecordsAffected);
						this.tabResults.TabPages.Clear();
						this.tabResults.TabPages.Add(this.tabpageMessages);
						Busy(false);
						if (SQLCommandType == dbeAccessEnum.dbaDDL) {
							fResponse = true;
						} else {
							fResponse = ShowMsgBox(string.Format("{0:#,##0} Records {1}... Commit transaction?", mRecordsAffected, GetOperation(SQLsource)), MsgBoxStyle.YesNo, this) == DialogResult.Yes;
						}
						break;
				}

				if (mTCBase.ActiveTXLevel > 0) {
					if (fResponse) {
						mSupport.UI.Display("Ending Transaction...", DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbBlack, TriState.False);
						mTCBase.EndTrans();
					} else {
						mSupport.UI.Display("Aborting Transaction...", DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbBlack, TriState.False);
						mTCBase.AbortTrans();
					}
				}

				switch (SQLCommandType) {
					case dbeAccessEnum.dbaReadOnly:
						strOutput = string.Format("{0:#,##0} record(s) read...", mRecordsAffected);
						fResponse = true;
						break;
					case dbeAccessEnum.dbaDDL:
						strOutput = "DDL command executed successfully...";
						fResponse = true;
						break;
					default:
						strOutput = string.Format("{0:#,##0} record(s) {1}...", mRecordsAffected, GetOperation(SQLsource));
						break;
				}
				if (fResponse) {
					this.sbStatus.Panels[(int)sbStatusPanelEnum.Count].Text = string.Format("{0:#,##0}", mRecordsAffected);
					mSupport.UI.Display(strOutput, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
				}
				this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "SQL";
				this.txtSQL.Focus();
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError(txtSQL, ex.Message);
				int errorCount = 0;
				ArrayList errors = new ArrayList();

				errors.Add(ex.Message.Trim());
				errorCount += 1;
				if ((ex.InnerException != null)) {
					Exception iException = ex.InnerException;
					while ((iException != null)) {
						errors.Add(iException.Message.Trim());
						errorCount += 1;
						iException = iException.InnerException;
					}
				}
				if (errors.Count > 0) {
					int dbeError = -1;
					for (int iError = 0; iError <= errors.Count - 1; iError++) {
						if (errors[iError] != bpeNullString) {
							if (Convert.ToString(errors[iError]).StartsWith("Unexpected error encountered executing SQL statement")) {
								dbeError = iError;
								strOutput = string.Format("{0}{1}", errors[iError], Constants.vbCrLf);
							}
						}
					}
					for (int iError = 0; iError <= errors.Count - 1; iError++) {
						if (errors[iError] != bpeNullString) {
							if (dbeError != iError) {
								if (dbeError == -1) {
									strOutput += string.Format("{0}{1}", errors[iError], Constants.vbCrLf);
								} else {
									if (errors[iError] != errors[dbeError])
										strOutput += string.Format("{0}{1}", errors[iError], Constants.vbCrLf);
								}
							}
						}
					}
					switch (SQLCommandType) {
						case dbeAccessEnum.dbaReadOnly:
							if ((dtSQL == null)) {
								mSupport.UI.Display(strOutput, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.True);
							} else if (dtSQL.Rows.Count == 0) {
								mSupport.UI.Display(strOutput, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.True);
							}
							break;
						default:
							mSupport.UI.Display(strOutput, DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbRed, TriState.True);
							if (errorCount > 0) {
								mSupport.UI.Display("Aborting Transaction...", DisplayEnum.deTextBoxOnly, vbRGBColorConstants.vbRed, TriState.False);
								mTCBase.AbortTrans();
								return;
							}
							break;
					}
				}
			} finally {
				Busy(false);
				this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "SQL";
			}
		}
		protected void GetQueryData()
		{
			const string EntryName = "GetQueryData";
			DataSet dsQuery = new DataSet();
			string SavedQuery = bpeNullString;
			string SQLSource = bpeNullString;
			var _with6 = this.cbQuery;
			SavedQuery = _with6.Text;
			_with6.DataSource = null;
			_with6.DisplayMember = bpeNullString;
			_with6.ValueMember = bpeNullString;

			SQLSource += "Select * From Query;";
			Trace(EntryName + " - tcDataAdapter.SelectCommand.CommandText=\"" + SQLSource + "\"", trcOption.trcDB | trcOption.trcDB);
            mTCBase.tcDataAdapter.SelectCommand.CommandText = SQLSource;
            mTCBase.tcDataAdapter.SelectCommand.CommandType = CommandType.Text;
            mTCBase.tcDataAdapter.SelectCommand.Connection = mTCBase.tcConnection;
			if ((mTCBase.tcTransaction != null)) {
				if (object.ReferenceEquals(mTCBase.tcTransaction.Connection, mTCBase.tcDataAdapter.SelectCommand.Connection))
                    mTCBase.tcDataAdapter.SelectCommand.Transaction = mTCBase.tcTransaction;
			}
            mTCBase.tcDataAdapter.SelectCommand.CommandTimeout = mTCBase.CommandTimeout;
			Trace(EntryName + " - DataAdapter.SelectCommand.CommandText=\"" + SQLSource + "\"", trcOption.trcDB | trcOption.trcDB);
			int RecordCount = mTCBase.tcDataAdapter.Fill(dsQuery);
			dvQuery = new DataView(dsQuery.Tables[0], bpeNullString, "Name Asc", DataViewRowState.CurrentRows);
			//Add a dummy entry as the first row to be displayed in cbQuery by default...
			DataRow NewRow = dvQuery.Table.NewRow();
			NewRow["Name"] = bpeNullString;
			NewRow["Description"] = bpeNullString;
			NewRow["Query"] = bpeNullString;
			NewRow["Access"] = dbeAccessEnum.dbaReadOnly;
			dvQuery.Table.Rows.Add(NewRow);
			NewRow = null;

			dvQuery.AllowDelete = false;
			dvQuery.AllowEdit = false;
			dvQuery.AllowNew = false;

			_with6.Items.Clear();
			_with6.DataSource = dvQuery;
			_with6.DisplayMember = "Name";
			_with6.ValueMember = "Name";
			_with6.SelectedIndex = dvQuery.Find(SavedQuery);
		}
		protected dbeAccessEnum GetSQLCommandType(string SQLSource)
		{
			dbeAccessEnum functionReturnValue = default(dbeAccessEnum);
			try {
				switch (Strings.UCase(Strings.Mid(SQLSource, 1, 6))) {
					case "SELECT":
						functionReturnValue = dbeAccessEnum.dbaReadOnly;
						break;
					case "UPDATE":
					case "DELETE":
					case "INSERT":
						functionReturnValue = dbeAccessEnum.dbaDML;
						break;
					default:
						functionReturnValue = dbeAccessEnum.dbaDDL;
						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
			}
			return functionReturnValue;
		}
		protected string GetOperation(string SQLSource)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			try {
				switch (Strings.UCase(Strings.Mid(SQLSource, 1, 6))) {
					case "INSERT":
						functionReturnValue = "inserted";
						break;
					case "DELETE":
						functionReturnValue = "deleted";
						break;
					case "UPDATE":
						functionReturnValue = "updated";
						break;
					default:
						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
			}
			return functionReturnValue;
		}
		protected void LoadFile(string FilePathName)
		{
			StreamReader myStreamReader = null;
			try {
				mPathName = FilePathName;

				myStreamReader = File.OpenText(mPathName);
				this.txtSQL.Text = myStreamReader.ReadToEnd();
				this.Text = mCaption + " - " + ParsePath(mPathName, ParseParts.FileNameBaseExt);
				mOriginalText = this.txtSQL.Text;
				CheckSaveState();
			} finally {
				if ((myStreamReader != null))
					myStreamReader.Close();
			}
		}
		protected void ResetGrid()
		{
			this.tabpageGrid.Controls.Remove(this.dgResults);
			this.dgResults = null;
			this.dgResults = new System.Windows.Forms.DataGrid();
			this.tabpageGrid.Controls.Add(this.dgResults);
            this.dgResults.CaptionFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
            this.dgResults.CaptionText = "Results";
            this.dgResults.DataMember = "";
            this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgResults.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.dgResults.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgResults.Location = new System.Drawing.Point(0, 0);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.Size = new System.Drawing.Size(708, 132);
            this.dgResults.TabIndex = 16;

            this.dgResults.DataSource = null;
            this.dgResults.BackgroundColor = SystemColors.InactiveCaptionText;
            //.CaptionText = ""
            //.CaptionBackColor = SystemColors.ActiveCaption
            this.dgResults.TableStyles.Clear();
            this.dgResults.ResetAlternatingBackColor();
            this.dgResults.ResetBackColor();
            this.dgResults.ResetForeColor();
            this.dgResults.ResetGridLineColor();
            this.dgResults.ResetHeaderBackColor();
            this.dgResults.ResetHeaderFont();
            this.dgResults.ResetHeaderForeColor();
            this.dgResults.ResetSelectionBackColor();
            this.dgResults.ResetSelectionForeColor();
            this.dgResults.ResetText();
            this.dgResults.BorderStyle = DefaultGridBorderStyle;
		}
		protected DialogResult SaveFile(string FilePathName, string Query)
		{
            DialogResult functionReturnValue = default(DialogResult);
			StreamWriter myStreamWriter = null;
            try
            {
                functionReturnValue = DialogResult.Yes;
                if (FilePathName == bpeNullString)
                {
                    dlgSave.FileName = bpeNullString;
                    dlgSave.Title = "Save SQL Query";
                    dlgSave.CheckFileExists = false;
                    dlgSave.CheckPathExists = true;
                    //.DefaultExt = "sql"
                    dlgSave.Filter = "SQL Query (SQL)|*.sql|All Files|*.*";
                    dlgSave.FilterIndex = 1;
                    functionReturnValue = dlgSave.ShowDialog(this);
                    if (functionReturnValue == DialogResult.Cancel)
                    {
                        mSupport.UI.Display("Save Canceled.", DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.False);
                        throw new ExitTryException();
                    }
                    mPathName = dlgSave.FileName;
                    FilePathName = dlgSave.FileName;
                }

                //Create a StreamWriter using a Shared (static) System.IO.File class.
                myStreamWriter = File.CreateText(FilePathName);

                //Write the entire contents of the txtFileText text box to the StreamWriter in one shot.
                myStreamWriter.Write(Query);
                myStreamWriter.Flush();
                myStreamWriter.Close();
                myStreamWriter = null;

                this.Text = mCaption + " - " + ParsePath(FilePathName, ParseParts.FileNameBaseExt);
                mOriginalText = Query;
                CheckSaveState();
                this.cbQuery.SelectedIndex = 0;

                LoadFile(FilePathName);

                string strOutput = string.Format("Query Saved to {0}.", FilePathName);
                mSupport.UI.Display(strOutput, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
            } catch (ExitTryException ex) {
            } finally {
				if ((myStreamWriter != null))
					myStreamWriter.Close();
			}
			return functionReturnValue;
		}
		protected DialogResult SaveQuery(string Query, bool fSaveAs = false)
		{
            DialogResult functionReturnValue = default(DialogResult);
			functionReturnValue = DialogResult.Yes;
			bool fStoredQuery = mPathName.StartsWith("[Query]");
			if (fStoredQuery && mAllowStore && !fSaveAs) {
				StoreQuery(this.cbQuery.Text, (string)dvQuery[this.cbQuery.SelectedIndex]["DESCRIPTION"], Query, false);
			} else if (fStoredQuery || mPathName == bpeNullString || fSaveAs) {
				functionReturnValue = SaveFile(bpeNullString, Query);
			} else {
				functionReturnValue = SaveFile(mPathName, Query);
			}
			return functionReturnValue;
		}
		private void Setup()
		{
			const string EntryName = "Setup";
			DefaultGridBorderStyle = dgResults.BorderStyle;

			ShowStatusPanel(sbStatusPanelEnum.Status, true);
			ShowStatusPanel(sbStatusPanelEnum.DB, true);
			ShowStatusPanel(sbStatusPanelEnum.Count, true);
			ShowStatusPanel(sbStatusPanelEnum.Message, true);
			ShowStatusPanel(sbStatusPanelEnum.Timeout, false);
			ShowStatusPanel(sbStatusPanelEnum.Time, true);
			ShowStatusPanel(sbStatusPanelEnum.EndBorder, true);

			this.sbStatus.Panels[(int)sbStatusPanelEnum.DB].Text = " " + mTCBase.DatabaseName;
			this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "SQL";
			this.tabResults.TabPages.Clear();
			this.tabResults.TabPages.Add(this.tabpageMessages);

			if ((dsDD != null)){dsDD = null;dsDD.Dispose();}
			dsDD = new DataSet();

			string SQLsource = "Select " + "[table].name as 'NAME', " + "sys.extended_properties.value As 'REMARKS' " + "From " + "sysobjects [table] " + "Inner Join sysusers On [table].uid=sysusers.uid And sysusers.name='dbo'  " + "Left Outer Join sys.extended_properties On sys.extended_properties.name='MS_Description' And sys.extended_properties.major_id=[table].id And sys.extended_properties.minor_id=0;";
			SQLsource += "Select  " + "[table].name as 'TBNAME', " + "syscolumns.name As 'NAME',  " + "sys.extended_properties.value As 'REMARKS' " + "From " + "sysobjects [table] " + "Inner Join sysusers On [table].uid=sysusers.uid And sysusers.name='dbo' " + "Inner Join syscolumns On [table].id=syscolumns.id " + "Left Outer Join sys.extended_properties On sys.extended_properties.name='MS_Description' And sys.extended_properties.major_id=[table].id And sys.extended_properties.minor_id=syscolumns.colid;";
			var _with10 = mTCBase.tcDataAdapter;
			Trace(EntryName + " - tcDataAdapter.SelectCommand.CommandText=\"" + SQLsource + "\"", trcOption.trcDB | trcOption.trcDB);
			_with10.SelectCommand.CommandText = SQLsource;
			_with10.SelectCommand.CommandType = CommandType.Text;
			_with10.SelectCommand.Connection = mTCBase.tcConnection;
			if ((mTCBase.tcTransaction != null)) {
				if (object.ReferenceEquals(mTCBase.tcTransaction.Connection, _with10.SelectCommand.Connection))
					_with10.SelectCommand.Transaction = mTCBase.tcTransaction;
			}
			_with10.SelectCommand.CommandTimeout = mTCBase.CommandTimeout;
			Trace(EntryName + " - DataAdapter.SelectCommand.CommandText=\"" + SQLsource + "\"", trcOption.trcDB | trcOption.trcDB);
			_with10.FillSchema(dsDD, SchemaType.Source);
			//FillSchema doesn't seem to want to use the default constraint from the database, so roll our own default here...
			mTCBase.initColumnDefaults(_with10.FillSchema(dsDD, SchemaType.Source)[0].Columns);
			int RecordCount = _with10.Fill(dsDD);

			dvTable = new DataView(dsDD.Tables[0], bpeNullString, "NAME Asc", DataViewRowState.CurrentRows);
			dvTable.AllowDelete = false;
			dvTable.AllowEdit = false;
			dvTable.AllowNew = false;
			var _with11 = this.cbTables;
			_with11.Items.Clear();
			_with11.DataSource = dvTable;
			_with11.DisplayMember = "NAME";
			_with11.ValueMember = "NAME";
			_with11.SelectedIndex = 0;
			dvColumn = new DataView(dsDD.Tables[1], bpeNullString, "TBNAME Asc, NAME Asc", DataViewRowState.CurrentRows);
			dvColumn.AllowDelete = false;
			dvColumn.AllowEdit = false;
			dvColumn.AllowNew = false;

			this.cbTables_SelectedValueChanged(this, new System.EventArgs());
			//menuFileNew.PerformClick()

			mSupport.UI.dsbStatusMessage = this.sbStatus.Panels[(int)sbStatusPanelEnum.Message];
			mSupport.UI.drtfDisplay = this.rtfResults;

			GetQueryData();
			this.cbQuery.SelectedIndex = -1;

			Clear();
			this.Text = mCaption + " - Untitled";
			ControlSetFocus(this.txtSQL);
		}
		protected void StoreQuery(string QueryName, string QueryDescription, string Query, bool ConfirmUpdate)
		{
			string SQLSource = null;
			string[] Params = {
				FixQuotes(QueryName),
				FixQuotes(QueryDescription),
				FixQuotes(Query),
				Convert.ToInt32(GetSQLCommandType(Query)).ToString()
			};
			//Check to see if an Query record exists with that name...
			if (CheckForDup(QueryName)) {
				if (ConfirmUpdate) {
					if (ShowMsgBox("A Query with this name already exists, overwrite?", MsgBoxStyle.YesNo, this, "StoreQuery") == DialogResult.No)
						return;
				}
				SQLSource = string.Format("Update Query Set Name='{0}', Description='{1}', Query='{2}', Access={3} Where Name='{0}'", Params);
			} else {
				SQLSource = string.Format("Insert Into Query(Name,Description,Query,Access) Values('{0}','{1}','{2}',{3})", Params);
			}
            //Store/Update the new Query record...
            int RecordAffected = 0;
			mTCBase.ExecuteCommand(SQLSource, ref RecordAffected);

			mOriginalText = this.txtSQL.Text;
			CheckSaveState();

			//Refresh dvQuery with the new record and position there so cbQuery shows the current value...
			GetQueryData();
			mSupport.UI.Display(string.Format("Query ({0}) Stored.", QueryName), DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
		}
		protected virtual void ShowStatusPanel(sbStatusPanelEnum Panel, bool Visible)
		{
			var _with12 = this.sbStatus.Panels[(int)Panel];
			if (Visible) {
				_with12.Text = bpeNullString;
				switch (Panel) {
					case sbStatusPanelEnum.Status:
						_with12.AutoSize = StatusBarPanelAutoSize.Contents;
						_with12.MinWidth = 64;
						break;
					case sbStatusPanelEnum.DB:
						_with12.AutoSize = StatusBarPanelAutoSize.Contents;
						_with12.MinWidth = 10;
						break;
					case sbStatusPanelEnum.Count:
						_with12.AutoSize = StatusBarPanelAutoSize.Contents;
						_with12.MinWidth = 10;
						break;
					case sbStatusPanelEnum.Message:
						_with12.AutoSize = StatusBarPanelAutoSize.Spring;
						_with12.MinWidth = 64;
						break;
					case sbStatusPanelEnum.Timeout:
						_with12.AutoSize = StatusBarPanelAutoSize.Contents;
						_with12.MinWidth = 10;
						break;
					case sbStatusPanelEnum.Time:
						_with12.AutoSize = StatusBarPanelAutoSize.Contents;
						_with12.MinWidth = 10;
						break;
					case sbStatusPanelEnum.EndBorder:
						_with12.AutoSize = StatusBarPanelAutoSize.None;
						_with12.Text = bpeNullString;
						_with12.MinWidth = 1;
						_with12.Width = 1;
						break;
				}
			} else {
				_with12.AutoSize = StatusBarPanelAutoSize.None;
				_with12.Text = bpeNullString;
				switch (Panel) {
					case sbStatusPanelEnum.EndBorder:
						_with12.MinWidth = 1;
						_with12.Width = 1;
						break;
					default:
						_with12.MinWidth = 0;
						_with12.Width = 0;
						break;
				}
			}
		}
		private string TrimWhiteSpace(string Source)
		{
            string functionReturnValue = Source;
            try
            {
                while (functionReturnValue.StartsWith(Constants.vbCrLf))
                {
                    functionReturnValue = Strings.Right(functionReturnValue, Strings.Len(functionReturnValue) - 2).Trim();
                }
                while (functionReturnValue.EndsWith(Constants.vbCrLf))
                {
                    functionReturnValue = Strings.Left(functionReturnValue, Strings.Len(functionReturnValue) - 2).Trim();
                }
            }
            catch (Exception ex)
            {
                Trace(ex.Message, trcOption.trcApplication);
			}
            return functionReturnValue;
		}
		public void Update(ref string Source, int RecordsAffected = 0)
		{
			string Operation = null;
			string TableName = null;
			//Operation = UCase(Left(Trim(Source), 6))
			Operation = Strings.UCase(ParseStr(Strings.Trim(Source), 1, " "));
			switch (Operation) {
				case "UPDATE":
					TableName = ParseStr(Source, 2, " ");
					//UPDATE <TableName> SET ...
					break;
				case "INSERT":
					TableName = ParseStr(Source, 3, " ");
					//INSERT INTO <TableName> ...
					break;
				case "DELETE":
					TableName = ParseStr(Source, 3, " ");
					//DELETE FROM <TableName> ...
					break;
				case "SET":
					TableName = bpeNullString;
					//SET <option> ON|OFF
					break;
				default:
					throw new Exception(string.Format("Unsupported SQL Operation ({0})", Operation));
			}

			if (mTCBase.ActiveTXLevel <= 0)
				throw new Exception(string.Format("No Transaction is active ({0})", Operation));
			mTCBase.ExecuteCommand(Source, ref RecordsAffected);
		}
		#endregion
		#region "Event Handlers"
		protected object FormatROWID(object sender, ConvertEventArgs e)
		{
			object functionReturnValue = null;
			functionReturnValue = null;
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
			return functionReturnValue;
		}
		protected void cbColumns_SelectedValueChanged(System.Object sender, System.EventArgs e)
		{
            try
            {
                this.epBase.SetError((Control)sender, bpeNullString);
                //If we fire before everything is fully hooked-up, our SelectedValue might be DataRowView...
                if (this.cbColumns.SelectedValue.GetType().ToString() == "System.Data.DataRowView")
                    throw new ExitTryException();
                if (this.cbColumns.SelectedIndex != -1)
                    this.tooltipSQL.SetToolTip(this.cbColumns, (string)(Information.IsDBNull(dvColumn[this.cbColumns.SelectedIndex]["REMARKS"]) ? "" : dvColumn[this.cbColumns.SelectedIndex]["REMARKS"]));
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void cbQuery_SelectedValueChanged(object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				if ((this.cbQuery.SelectedValue == null))
                    throw new ExitTryException();
				if (this.cbQuery.SelectedValue.GetType().ToString() == "System.Data.DataRowView")
                    throw new ExitTryException();
				if (CheckToSave() == DialogResult.Cancel)
                    throw new ExitTryException();
				Clear();

				if (this.cbQuery.SelectedIndex >= 0) {
					this.tooltipSQL.SetToolTip(this.cbQuery, (string)dvQuery[this.cbQuery.SelectedIndex]["DESCRIPTION"]);
					this.txtSQL.Text = (string)dvQuery[this.cbQuery.SelectedIndex]["QUERY"];
					if (this.cbQuery.SelectedIndex == 0) {
						this.menuFileNew_Click(this, new EventArgs());
					} else {
						mPathName = "[Query] " + this.cbQuery.SelectedValue;
						this.Text = mCaption + " - " + mPathName;
						mOriginalText = this.txtSQL.Text;
						if ((this.menuQueryDelete != null))
							this.menuQueryDelete.Enabled = true;
						if ((this.menuQueryUpdate != null))
							this.menuQueryUpdate.Enabled = true;
					}
				}
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void cbTables_SelectedValueChanged(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				//If we fire before everything is fully hooked-up, our SelectedValue might be DataRowView...
				if (this.cbTables.SelectedValue.GetType().ToString() == "System.Data.DataRowView")
                    throw new ExitTryException();
				if (this.cbTables.SelectedIndex != -1)
					this.tooltipSQL.SetToolTip(this.cbTables, (string)(Information.IsDBNull(dvTable[this.cbTables.SelectedIndex]["REMARKS"]) ? "" : dvTable[this.cbTables.SelectedIndex]["REMARKS"]));

				if (dvColumn == null)
                    throw new ExitTryException();
				dvColumn.RowFilter = string.Format("TBNAME='{0}'", cbTables.SelectedValue);
                cbColumns.DataSource = dvColumn;
                cbColumns.DisplayMember = "NAME";
                cbColumns.ValueMember = "NAME";
                cbColumns.SelectedIndex = 0;
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void dgResults_CurrentCellChanged(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				dgResults.Select(dgResults.CurrentRowIndex);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		//    Private Sub dgResults_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgResults.MouseUp
		//        Try
		//            RecordEntry(EntryName, "sender:={" & sender.GetType.ToString & "}, e:={" & e.GetType.ToString & "}", trcOption.trcLookupDetail)
		//            MyBase.epBase.SetError((Control)sender, bpeNullString)
		//            Dim myGrid As DataGrid = CType(sender, DataGrid)
		//            hti = myGrid.HitTest(e.X, e.Y)
		//            Dim Message As String = "You clicked "
		//            Select Case hti.Type
		//                Case System.Windows.Forms.DataGrid.HitTestType.None : Message &= "the background."
		//                Case System.Windows.Forms.DataGrid.HitTestType.Cell : Message &= "cell at row " & hti.Row & ", col " & hti.Column
		//                Case System.Windows.Forms.DataGrid.HitTestType.ColumnHeader : Message &= "the column header for column " & hti.Column
		//                Case System.Windows.Forms.DataGrid.HitTestType.RowHeader : Message &= "the row header for row " & hti.Row
		//                Case System.Windows.Forms.DataGrid.HitTestType.ColumnResize : Message &= "the column resizer for column " & hti.Column
		//                Case System.Windows.Forms.DataGrid.HitTestType.RowResize : Message &= "the row resizer for row " & hti.Row
		//                Case System.Windows.Forms.DataGrid.HitTestType.Caption : Message &= "the caption"
		//                Case System.Windows.Forms.DataGrid.HitTestType.ParentRows : Message &= "the parent row"
		//            End Select
		//            Me.sbStatus.Panels(sbStatusPanelEnum.Message).Text = Message
		//        Catch ex As Exception
		//            Trace(ex.Message, trcOption.trcLookupDetail)
		//            MyBase.epBase.SetError((Control)sender, ex.Message)
		//        End Try
		//    End Sub
		private void dgResults_SizeChanged(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				switch (this.WindowState) {
					case FormWindowState.Maximized:
						foreach (DataGridColumnStyle i in dgResults.TableStyles[0].GridColumnStyles) {
							if (i.MappingName != bpeNullString)
								i.Width = GetDataWidth(i);
						}

						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void frmSQL_Activated(object sender, System.EventArgs e)
		{
			if (fActivated)
				return;
			fActivated = true;

			int iLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Left", this.Left);
			int iTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Top", this.Top);
			int iWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Width", this.Width);
			int iHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Height", this.Height);
			base.AdjustFormPlacement(ref iTop, ref iLeft); //Correct for errant form placement...
			this.SetBounds(iLeft, iTop, iWidth, iHeight);
		}
		protected void frmSQL_Closed(System.Object sender, System.EventArgs e)
		{
			try {
				dtSQL = null;
				if ((dsSQL != null)){dsSQL.Dispose();dsSQL = null;}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Form Closed");
			}
		}
		protected void frmSQL_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try {
				if (CheckToSave() == DialogResult.Cancel)
					e.Cancel = true;
				mTCBase.SaveBounds(mRegistryKey, this.Left, this.Top, this.Width, this.Height);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Form Closing");
			}
		}
		protected void frmSQL_Load(System.Object sender, System.EventArgs e)
		{
			try {
				if ((this.ParentForm != null))
					this.Icon = this.ParentForm.Icon;
				else
					this.Icon = (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("frmSQL");

				menuFile = menuMain.MenuItems.Add("&File");
				menuFileNew =  menuFile.MenuItems[menuFile.MenuItems.Add(new clsIconMenuItem("&New", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileNew"), new EventHandler(menuFileNew_Click), System.Windows.Forms.Shortcut.CtrlN))];
				menuFileOpen = menuFile.MenuItems[menuFile.MenuItems.Add(new clsIconMenuItem("&Open...", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileOpen"), new EventHandler(menuFileOpen_Click), System.Windows.Forms.Shortcut.CtrlO))];
				menuFileSave = menuFile.MenuItems[menuFile.MenuItems.Add(new clsIconMenuItem("&Save", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileSave"), new EventHandler(menuFileSave_Click), System.Windows.Forms.Shortcut.CtrlS))];
				menuFileSaveAs = menuFile.MenuItems[menuFile.MenuItems.Add(new clsIconMenuItem("Save &As...", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileSaveAs"), new EventHandler(menuFileSaveAs_Click), System.Windows.Forms.Shortcut.None))];
				menuFile.MenuItems.Add(new MenuItem("-"));
				menuFileExit = menuFile.MenuItems[menuFile.MenuItems.Add(new clsIconMenuItem("E&xit", "Verdana", 8, null, new EventHandler(menuFileExit_Click), System.Windows.Forms.Shortcut.AltF4))];

				//menuEdit = menuMain.MenuItems.Add("&Edit")
				//menuEditCut = menuEdit.MenuItems(menuEdit.MenuItems.Add(New clsIconMenuItem("Cu&t", "Verdana", 8, CType(mTCBase.ProjectResources.GetObject("EditCut"), System.Drawing.Icon), New EventHandler(AddressOf menuEditCut_Click), Windows.Forms.Shortcut.CtrlX)))
				//menuEditCopy = menuEdit.MenuItems(menuEdit.MenuItems.Add(New clsIconMenuItem("&Copy", "Verdana", 8, CType(mTCBase.ProjectResources.GetObject("EditCopy"), System.Drawing.Icon), New EventHandler(AddressOf menuEditCopy_Click), Windows.Forms.Shortcut.CtrlC)))
				//menuEditPaste = menuEdit.MenuItems(menuEdit.MenuItems.Add(New clsIconMenuItem("&Paste", "Verdana", 8, CType(mTCBase.ProjectResources.GetObject("EditPaste"), System.Drawing.Icon), New EventHandler(AddressOf menuEditPaste_Click), Windows.Forms.Shortcut.CtrlV)))

				menuQuery = menuMain.MenuItems.Add("&Query");
				menuQueryExecute = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("&Execute", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryExecute"), new EventHandler(menuQueryExecute_Click), System.Windows.Forms.Shortcut.F5))];
				if (mAllowStore) {
					menuQuery.MenuItems.Add(new MenuItem("-"));
					menuQueryStore = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("&Store Query in Database...", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryStore16"), new EventHandler(menuQueryStore_Click), System.Windows.Forms.Shortcut.CtrlShiftS))];
					menuQueryUpdate = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("&Update Description", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("EditFont"), new EventHandler(menuQueryUpdate_Click), System.Windows.Forms.Shortcut.CtrlShiftU))];
					menuQueryDelete = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("&Delete Query", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryDelete"), new EventHandler(menuQueryDelete_Click), System.Windows.Forms.Shortcut.CtrlShiftD))];
				} else {
					this.cmdQueryStore.Visible = false;
				}
				menuQuery.MenuItems.Add(new MenuItem("-"));
				menuQueryExport = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("E&xport Results...", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryExport"), new EventHandler(menuQueryExport_Click), System.Windows.Forms.Shortcut.CtrlShiftX))];
				if (mFilesBookReport != bpeNullString) {
					menuQueryFilesBook = menuQuery.MenuItems[menuQuery.MenuItems.Add(new clsIconMenuItem("Files &Book", "Verdana", 8, (System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryFilesBook16"), new EventHandler(menuQueryFilesBook_Click), System.Windows.Forms.Shortcut.CtrlShiftB))];
				} else {
					this.cmdQueryFilesBook.Visible = false;
				}

				this.tbSQL.ImageList = imgSQL;
				var _with17 = imgSQL;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("Grid"));
				this.tabpageGrid.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("Messages"));
				this.tabpageMessages.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileNew"));
				this.cmdFileNew.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileOpen"));
				this.cmdFileOpen.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileSave"));
				this.cmdFileSave.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("FileSaveAs"));
				this.cmdFileSaveAs.ImageIndex = _with17.Images.Count - 1;
				//.Images.Add(CType(mTCBase.ProjectResources.GetObject("EditCut"), System.Drawing.Icon)) : Me.cmdEditCut.ImageIndex = .Images.Count - 1
				//.Images.Add(CType(mTCBase.ProjectResources.GetObject("EditCopy"), System.Drawing.Icon)) : Me.cmdEditCopy.ImageIndex = .Images.Count - 1
				//.Images.Add(CType(mTCBase.ProjectResources.GetObject("EditPaste"), System.Drawing.Icon)) : Me.cmdEditPaste.ImageIndex = .Images.Count - 1
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryExecute"));
				this.cmdQueryExecute.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryExport"));
				this.cmdQueryExport.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryStore32"));
				this.cmdQueryStore.ImageIndex = _with17.Images.Count - 1;
				_with17.Images.Add((System.Drawing.Icon)mTCBase.ProjectResources.GetObject("QueryFilesBook32"));
				this.cmdQueryFilesBook.ImageIndex = _with17.Images.Count - 1;
				this.menuFileNew_Click(this, new EventArgs());
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Form Load");
			}
		}
		protected void menuEditCopy_Click(object sender, System.EventArgs e)
		{
			try {
				Clipboard.SetDataObject(txtSQL.SelectedText, true);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuEditCopy.Text, "&", bpeNullString));
			}
		}
		protected void menuEditCut_Click(object sender, System.EventArgs e)
		{
			try {
				Clipboard.SetDataObject(txtSQL.SelectedText, false);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuEditCut.Text, "&", bpeNullString));
			}
		}
		protected void menuEditPaste_Click(object sender, System.EventArgs e)
		{
			object obj = null;
			try {
				if (Clipboard.GetDataObject().GetDataPresent("Text")) {
					obj = Clipboard.GetDataObject().GetData("Text");
					if ((obj != null)) {
						if (obj.GetType().ToString() == "System.String") {
							this.txtSQL.AppendText(Convert.ToString(obj));
						} else {
							this.txtSQL.AppendText(obj.GetType().ToString());
						}
					}
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuEditPaste.Text, "&", bpeNullString));
			}
		}
		protected void menuFileExit_Click(object sender, System.EventArgs e)
		{
			try {
				this.Close();
				//frmSQL_CLosing will check to see if the query needs to be saved...
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuFileExit.Text, "&", bpeNullString));
			}
		}
		protected void menuFileNew_Click(object sender, System.EventArgs e)
		{
			try {
				Clear();
				this.Text = mCaption + " - Untitled";
				mOriginalText = this.txtSQL.Text;
				this.cbQuery.SelectedIndex = 0;
				if ((this.menuQueryDelete != null))
					this.menuQueryDelete.Enabled = false;
				if ((this.menuQueryUpdate != null))
					this.menuQueryUpdate.Enabled = false;
				this.txtSQL.Focus();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuFileNew.Text, "&", bpeNullString));
			}
		}
		protected void menuFileOpen_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (CheckToSave() == DialogResult.Cancel)
                    throw new ExitTryException();
                dlgOpen.FileName = bpeNullString;
                dlgOpen.Multiselect = false;
                dlgOpen.Title = "Open SQL Query";
                dlgOpen.CheckFileExists = true;
                dlgOpen.CheckPathExists = true;
                dlgOpen.DefaultExt = "sql";
                dlgOpen.Filter = "SQL Files (.SQL)|*.sql|All Files|*.*";
                dlgOpen.FilterIndex = 1;
                if (dlgOpen.ShowDialog(this) == DialogResult.Cancel)
                    throw new ExitTryException();

                Clear();
                this.cbQuery.SelectedIndex = 0;
                LoadFile(dlgOpen.FileName);
                this.txtSQL.Focus();
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox("File could not be opened or read." + Constants.vbCrLf + "Please verify that the filename is correct, " + "and that you have read permissions for the desired " + "directory." + Constants.vbCrLf + Constants.vbCrLf + "Exception: " + ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuFileOpen.Text, "&", bpeNullString));
			}
		}
		protected void menuFileSave_Click(object sender, System.EventArgs e)
		{
			try {
				SaveQuery(this.txtSQL.Text);
				this.txtSQL.Focus();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuFileSave.Text, "&", bpeNullString));
			}
		}
		protected void menuFileSaveAs_Click(object sender, System.EventArgs e)
		{
			StreamWriter myStreamWriter = null;
			try {
				SaveQuery(this.txtSQL.Text, true);
				this.txtSQL.Focus();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox("File could not be created or written to." + Constants.vbCrLf + "Please verify that the filename is correct, " + "and that you have write permissions for the desired " + "directory." + Constants.vbCrLf + Constants.vbCrLf + "Exception: " + ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuFileSaveAs.Text, "&", bpeNullString));
			} finally {
				if ((myStreamWriter != null))
					myStreamWriter.Close();
			}
		}
		protected void menuQueryDelete_Click(object sender, System.EventArgs e)
		{
			string QueryName = null;
            try
            {
                QueryName = (string)dvQuery[this.cbQuery.SelectedIndex]["NAME"];
                //Me.cbQuery.Text
                if (ShowMsgBox(string.Format("Are you sure you want to delete {0}?", QueryName), MsgBoxStyle.YesNo, this, "DeleteQuery") == DialogResult.No)
                    throw new ExitTryException();
                int RecordsAffected = 0;
                mTCBase.ExecuteCommand(string.Format("Delete From Query Where Name='{0}'", QueryName), ref RecordsAffected);

                //Refresh dvQuery with the updated record and position there so cbQuery shows the current value...
                GetQueryData();
                this.cbQuery.SelectedIndex = 0;
                this.txtSQL.Focus();
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryDelete.Text, "&", bpeNullString));
			}
		}
		protected void menuQueryExecute_Click(object sender, System.EventArgs e)
		{
			try {
				Execute(this.txtSQL.Text);
				this.txtSQL.Focus();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryExecute.Text, "&", bpeNullString));
			}
		}
		protected void menuQueryExport_Click(object sender, System.EventArgs e)
		{
			StreamWriter myStreamWriter = null;
            try
            {
                dlgSave.FileName = bpeNullString;
                dlgSave.Title = "Export Query Results";
                dlgSave.CheckFileExists = false;
                dlgSave.CheckPathExists = true;
                dlgSave.Filter = "Comma Separated Value File (CSV)|*.csv|XML File|*.xml";
                dlgSave.FilterIndex = 1;
                if (dlgSave.ShowDialog(this) == DialogResult.Cancel)
                {
                    mSupport.UI.Display("Export Canceled.", DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.False);
                    throw new ExitTryException();
                }

                this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "Exporting";
                mSupport.UI.Display(bpeNullString, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
                Busy(true);
                switch (dlgSave.FilterIndex)
                {
                    case 1:
                        string Buffer = bpeNullString;
                        string Value = bpeNullString;
                        int iCount = 0;

                        myStreamWriter = File.CreateText(dlgSave.FileName);
                        //Dim iRow As DataRowView = dvQuery(iPosition)
                        //txtSQL.Text = iRow("QUERY")
                        //Output Header Row...
                        Buffer = bpeNullString;
                        foreach (DataColumn iColumn in dvSQL.Table.Columns)
                        {
                            Buffer += mSupport.FieldEdits.CSVQuote(iColumn.ColumnName) + ",";
                        }

                        Buffer = Buffer.Substring(0, Buffer.Length - 1);
                        //Strip trailing comma
                        myStreamWriter.WriteLine(Buffer);
                        myStreamWriter.Flush();
                        Application.DoEvents();

                        //Output Data Rows...
                        for (int iRow = 0; iRow <= dvSQL.Count - 1; iRow++)
                        {
                            iCount += 1;
                            Buffer = bpeNullString;
                            foreach (DataColumn iColumn in dvSQL.Table.Columns)
                            {
                                Value = bpeNullString;
                                if (!Information.IsDBNull(dvSQL[iRow][iColumn.ColumnName]))
                                {
                                    switch (iColumn.DataType.Name)
                                    {
                                        case "Byte[]":
                                            Value = BytesToHex((byte[])dvSQL[iRow][iColumn.ColumnName]);
                                            break;
                                        default:
                                            Value = mSupport.FieldEdits.ConvertToCSV(dvSQL[iRow][iColumn.ColumnName]);
                                            break;
                                    }
                                }
                                Buffer += Value + ",";
                            }
                            Buffer = Buffer.Substring(0, Buffer.Length - 1);
                            //Strip trailing comma
                            myStreamWriter.WriteLine(Buffer);
                            myStreamWriter.Flush();
                            this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = string.Format("Exporting: {0:P0}", iCount / mRecordsAffected);
                            Application.DoEvents();
                        }

                        break;
                    case 2:
                        //dvSQL.Table.DataSet.WriteXml(.FileName)
                        dvSQL.Table.DataSet.WriteXml(dlgSave.FileName, XmlWriteMode.WriteSchema);
                        break;
                }
                Busy(false);
                string strOutput = string.Format("Export Complete; {0:#,##0} records written to {1}", mRecordsAffected, dlgSave.FileName);
                mSupport.UI.Display(strOutput, DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbBlack, TriState.False);
                //ShowMsgBox(strOutput, MsgBoxStyle.OKOnly, Me, "Export Results")
                this.sbStatus.Panels[(int)sbStatusPanelEnum.Status].Text = "SQL";
                this.txtSQL.Focus();
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryExport.Text, "&", bpeNullString));
			} finally {
				if ((myStreamWriter != null))
					myStreamWriter.Close();
				Busy(false);
			}
		}
		protected void menuQueryFilesBook_Click(object sender, System.EventArgs e)
		{
			try {
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryFilesBook.Text, "&", bpeNullString));
			}
		}
		protected void menuQueryStore_Click(object sender, System.EventArgs e)
		{
			string QueryName = bpeNullString;
			string QueryDescription = bpeNullString;
            try
            {
                frmQuery frm = new frmQuery(mSupport, mTCBase, this, Strings.Replace(this.menuQueryStore.Text, "&", bpeNullString));
                frm.QueryName = bpeNullString;
                frm.QueryDescription = bpeNullString;
                frm.Unique = true;
                switch (frm.ShowDialog())
                {
                    case DialogResult.Cancel:
                        mSupport.UI.Display("Store Canceled.", DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.False);
                        throw new ExitTryException();

                        break;
                    case DialogResult.OK:
                        break;
                }
                QueryName = frm.QueryName;
                QueryDescription = frm.QueryDescription;
                frm.Close();
                frm = null;

                StoreQuery(QueryName, QueryDescription, this.txtSQL.Text, false);
                this.cbQuery.SelectedIndex = dvQuery.Find(QueryName);
                this.txtSQL.Focus();
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryStore.Text, "&", bpeNullString));
			}
		}
		protected void menuQueryUpdate_Click(object sender, System.EventArgs e)
		{
			string QueryName = null;
			string QueryDescription = null;
			try {
				frmQuery frm = new frmQuery(mSupport, mTCBase, this, Strings.Replace(this.menuQueryStore.Text, "&", bpeNullString));
                frm.QueryName = (string)dvQuery[this.cbQuery.SelectedIndex]["NAME"];
                //Me.cbQuery.Text
                frm.QueryDescription = (string)dvQuery[this.cbQuery.SelectedIndex]["DESCRIPTION"];
                frm.Unique = false;
				ControlEnabled(frm.txtName, false);
				switch (frm.ShowDialog()) {
					case DialogResult.Cancel:
						mSupport.UI.Display("Update Canceled.", DisplayEnum.deBothStatusBarAndTextBox, vbRGBColorConstants.vbRed, TriState.False);
                        throw new ExitTryException();

                        break;
					case DialogResult.OK:
						break;
				}
				QueryName = frm.QueryName;
				QueryDescription = frm.QueryDescription;
                frm.Close();
				frm = null;

				StoreQuery(QueryName, QueryDescription, this.txtSQL.Text, false);
				//Me.cbQuery.SelectedIndex = dvQuery.Find(QueryName)
				this.txtSQL.Focus();
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, Strings.Replace(this.menuQueryUpdate.Text, "&", bpeNullString));
			}
		}
		protected virtual void sbStatus_Disposed(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				if ((timClock != null))
					timClock.Enabled = false;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void tbSQL_ButtonClick(System.Object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				switch ((tbSQLButtonEnum)tbSQL.Buttons.IndexOf(e.Button)) {
					case tbSQLButtonEnum.cmdFileNew:
						menuFileNew_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdFileOpen:
						menuFileOpen_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdFileSave:
						menuFileSave_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdFileSaveAs:
						menuFileSaveAs_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdEditCopy:
						menuEditCopy_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdEditCut:
						menuEditCut_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdEditPaste:
						menuEditPaste_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdQueryExecute:
						menuQueryExecute_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdQueryExport:
						menuQueryExport_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdQueryStore:
						menuQueryStore_Click(e.Button, e);
						break;
					case tbSQLButtonEnum.cmdQueryFilesBook:
						menuQueryFilesBook_Click(e.Button, e);
						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected virtual void timClock_Tick(System.Object sender, System.EventArgs e)
		{
			try {
				//RecordEntry(EntryName, "sender:={" & sender.GetType.ToString & "}:=" & Ctype(sender, Object).Name & ", e:={" & e.GetType.ToString & "}", trcOption.trcApplication)
				//sbStatus.Panels(sbStatusPanelEnum.Time).Text = Now.ToLongTimeString
				//sbStatus.Panels(sbStatusPanelEnum.Time).Text = Now.ToLongTimeString & vbCrLf
				//sbStatus.Panels(sbStatusPanelEnum.Time).Text = String.Format("{0:T}", Now)  '"12:34:56"
				this.sbStatus.Panels[(int)sbStatusPanelEnum.Time].Text = string.Format("{0:t}", DateAndTime.Now);
				//"12:34"

				bool fGotQuery = Convert.ToBoolean(Strings.Trim(this.txtSQL.Text) != bpeNullString);
				bool fGotData = (this.dgResults.DataSource != null);
				this.cmdQueryExecute.Enabled = fGotQuery;
				this.menuQueryExecute.Enabled = fGotQuery;
				this.cmdQueryStore.Enabled = fGotQuery;
				this.menuQueryStore.Enabled = fGotQuery;
				this.cmdQueryExport.Enabled = fGotData;
				this.menuQueryExport.Enabled = fGotData;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
			}
		}
		protected void txtSQL_Enter(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
                this.txtSQL.Text = TrimWhiteSpace(this.txtSQL.Text);
				//TextSelected(VB6.GetActiveControl())
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void txtSQL_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			int KeyAscii = Strings.Asc(e.KeyChar);
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				if ((Keys)KeyAscii == System.Windows.Forms.Keys.Return) {
					string tempSource = this.txtSQL.Text;
                    tempSource = TrimWhiteSpace(tempSource);
					if (tempSource.Trim().EndsWith(";")) {
						Execute(tempSource);
						e.Handled = true;
					}
				}
				if (KeyAscii == 0)
					e.Handled = true;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void txtSQL_Leave(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
                this.txtSQL.Text = TrimWhiteSpace(this.txtSQL.Text);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void txtSQL_TextChanged(object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				CheckSaveState();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		#endregion
	}
}
