//frmTCStandard.cs
//   Base Data Entry Form for TreasureChest2 Project...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/27/19    Added logic to adjust form placement to account for preferences from a device with a size larger than the 
//               current device so this form would always displays on the current viewport;
//TODO:   07/21/19    Moved User Preferences from local registry into database;
//TODO:   07/08/19    Moved ttBase (ToolTip) from frmTCStandard into base class;
//TODO:   06/12/19    Implemented support for HistoryCommand;
//TODO:               Added mode-specific "Edit Mode" clarification on StatusBar, distinguishing between Add/Copy/Modify modes;
//TODO:   09/01/18    Implemented new Sort functionality;
//TODO:   08/26/18    Reworked Shell32 logic used to determine file properties;
//   02/03/18    Restored ActionModeChange's use of EnableControls (from EnableControlsByBinding) as the former method would not
//               trigger handling of RichTextBox controls (despite sub-classing the control to make the Rtf property bind-able);
//   12/19/17    Added UnbindControls call before CopyCommand and NewCommand in an effort to eliminate weird binding behavior most
//               notably in DateTimePicker controls when adding or copying new records to the end of the list;
//               Improved Tracing;
//   12/02/17    Changed Move-related Trace calls to use trcAll in an effort to see why my Dates are always getting screwed-up;
//   10/06/16    Attempting to track down issues clearing controls on Add operations;
//   09/18/16    Reworked architecture to eliminate references (i.e. controls, bindings, CurrencyManager) from clsTCBase and moved
//               them here in an effort to release memory otherwise held forever;
//   12/03/15    Added menu items and tbMain to controls handled by EnableButtons;
//               Made Form_Closing and Form_Load overridable to allow proper operation of derived components;
//   10/19/14    Upgraded to VS2013;
//   10/15/11    Implemented hot-keys for record navigation and OK/Cancel/Exit defaults;
//               Added dynamic support of clearing DateTimePicker controls on Add/Copy operations allowing derived screens to 
//               decide which controls should be reset based on Tag property;
//   07/11/11    Added initialization of DateTimePicker controls to Now on Add operations;
//   10/03/10    Added lblValue, txtValue, lblVerified, and dtpVerified;
//   10/02/10    Added ctxImage menu options based on whether the image is the default or not;
//   08/05/10    Added mnuImageClear;
//   07/26/10    Reorganized registry settings;
//   10/25/09    Created in VB.NET;
//=================================================================================================================================
//=================================================================================================================================
// ERROR: Not supported in C#: OptionDeclaration
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.MyServices;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
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
	//Public MustInherit Class frmTCStandard
	public class frmTCStandard : frmTCBase
	{
		public frmTCStandard() : base()
		{
			Leave += Form_Leave;
			Enter += Form_Enter;
			BindingContextChanged += Form_BindingContextChanged;
			Shown += Form_Shown;
			Load += Form_Load;
			KeyUp += Form_KeyUp;
			Closing += Form_Closing;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			timClock.Enabled = true;
			//This ensures that the clock isn't activated until run-time...
		}
		public frmTCStandard(string formName, clsSupport objSupport, clsTCBase objTCBase, string Caption, Form objParent = null) : base(objSupport, formName, objTCBase, objParent)
		{
			Leave += Form_Leave;
			Enter += Form_Enter;
			BindingContextChanged += Form_BindingContextChanged;
			Shown += Form_Shown;
			Load += Form_Load;
			KeyUp += Form_KeyUp;
			Closing += Form_Closing;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
            //Size the form to VGA Minimums...
			if (!(TCBase.MyComputer.Screen.Bounds.Width > 640 && TCBase.MyComputer.Screen.Bounds.Height > 480))
				this.MaximumSize = new Size(640, 480);

			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			Trace("frmTCStandard.New: Me.tcMain.Size: {0}; Me.gbGeneral.Size: {1}; Me.sbStatus.Top({2}) - Me.btnExit.Bottom({3}) = {4};", new object[] {
				this.tcMain.Size.ToString(),
				this.gbGeneral.Size.ToString(),
				this.sbStatus.Top,
				(this.btnExit.Top + this.btnExit.Height),
				this.sbStatus.Top - (this.btnExit.Top + this.btnExit.Height)
			}, trcOption.trcApplication);
			mTCBase = objTCBase;

			mRegistryKey = string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, formName);

			if (mTCBase != null && mTCBase.MainDataView != null) {
				mCurrencyManager = (CurrencyManager)this.BindingContext[mTCBase.MainDataView];
				mCurrencyManager.CurrentChanged += CurrencyManager_CurrentChanged;
				mCurrencyManager.ItemChanged += CurrencyManager_ItemChanged;
				mCurrencyManager.PositionChanged += CurrencyManager_PositionChanged;
			}
		}
		#region " Windows Form Designer generated code "
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
		private Button withEventsField_btnLast;
		protected internal Button btnLast {
			get { return withEventsField_btnLast; }
			set {
				if (withEventsField_btnLast != null) {
					withEventsField_btnLast.Click -= btnLast_Click;
				}
				withEventsField_btnLast = value;
				if (withEventsField_btnLast != null) {
					withEventsField_btnLast.Click += btnLast_Click;
				}
			}
		}
		private Button withEventsField_btnFirst;
		protected internal Button btnFirst {
			get { return withEventsField_btnFirst; }
			set {
				if (withEventsField_btnFirst != null) {
					withEventsField_btnFirst.Click -= btnFirst_Click;
				}
				withEventsField_btnFirst = value;
				if (withEventsField_btnFirst != null) {
					withEventsField_btnFirst.Click += btnFirst_Click;
				}
			}
		}
		private Button withEventsField_btnNext;
		protected internal Button btnNext {
			get { return withEventsField_btnNext; }
			set {
				if (withEventsField_btnNext != null) {
					withEventsField_btnNext.Click -= btnNext_Click;
				}
				withEventsField_btnNext = value;
				if (withEventsField_btnNext != null) {
					withEventsField_btnNext.Click += btnNext_Click;
				}
			}
		}
		private Button withEventsField_btnPrev;
		protected internal Button btnPrev {
			get { return withEventsField_btnPrev; }
			set {
				if (withEventsField_btnPrev != null) {
					withEventsField_btnPrev.Click -= btnPrev_Click;
				}
				withEventsField_btnPrev = value;
				if (withEventsField_btnPrev != null) {
					withEventsField_btnPrev.Click += btnPrev_Click;
				}
			}
		}
		protected internal StatusBarPanel sbpPosition;
		protected internal StatusBarPanel sbpStatus;
		protected internal StatusBarPanel sbpMessage;
		protected internal StatusBarPanel sbpTime;
		protected internal StatusBarPanel sbpEndBorder;
		protected internal Label lblID;
        protected internal StatusStrip StatusStrip1;
        protected internal ToolStripStatusLabel tsslblRows;
        protected internal ToolStripStatusLabel tsslblRowFilter;
        protected internal ToolStripStatusLabel tsslblSort;
        private ToolBar withEventsField_tbMain;
		private ToolBar tbMain {
			get { return withEventsField_tbMain; }
			set {
				if (withEventsField_tbMain != null) {
					withEventsField_tbMain.ButtonClick -= tbMain_ButtonClick;
				}
				withEventsField_tbMain = value;
				if (withEventsField_tbMain != null) {
					withEventsField_tbMain.ButtonClick += tbMain_ButtonClick;
				}
			}
		}
		private System.Windows.Forms.ToolBarButton tbbSelect;
		private ToolBarButton tbbNew;
		private ToolBarButton tbbCopyAppend;
		private ToolBarButton tbbModify;
		private ToolBarButton tbbDelete;
		private ToolBarButton tbbSep1;
		private ToolBarButton tbbRefresh;
		private ToolBarButton tbbSep2;
		private ToolBarButton tbbFilter;
        private ToolBarButton tbbSort;
		private ToolBarButton tbbList;
		private ToolBarButton tbbSep3;
		private ToolBarButton tbbReport;
		private ToolBarButton tbbSQL;
		private ToolBarButton tbbSep4;
		private ToolBarButton tbbDumpDataView;
		private ToolBarButton tbbDumpDataTable;
		private System.Windows.Forms.ToolBarButton tbbSep5;
		private ImageList imgToolbar;
		private System.Windows.Forms.Timer withEventsField_timClock;
		protected internal System.Windows.Forms.Timer timClock {
			get { return withEventsField_timClock; }
			set {
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick -= Tick;
				}
				withEventsField_timClock = value;
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick += Tick;
				}
			}
		}
		protected internal Label lblPurchased;
		protected internal Label lblInventoried;
		protected internal Label lblLocation;
		protected internal Label lblAlphaSort;
		protected internal Label lblPrice;
		private Button withEventsField_btnOK;
		protected internal Button btnOK {
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
		private Button withEventsField_btnExit;
		protected internal Button btnExit {
			get { return withEventsField_btnExit; }
			set {
				if (withEventsField_btnExit != null) {
					withEventsField_btnExit.Click -= btnExit_Click;
				}
				withEventsField_btnExit = value;
				if (withEventsField_btnExit != null) {
					withEventsField_btnExit.Click += btnExit_Click;
				}
			}
		}
		private Button withEventsField_btnCancel;
		protected internal Button btnCancel {
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
		protected internal StatusBar sbStatus;
		protected internal StatusBarPanel sbpFilter;
		protected internal StatusBarPanel sbpMode;
		protected internal OpenFileDialog ofdTCStandard;
		protected internal SaveFileDialog sfdTCStandard;
		public CheckBox chkWishList;
		public DateTimePicker dtpPurchased;
		public DateTimePicker dtpInventoried;
		private System.Windows.Forms.ComboBox withEventsField_cbLocation;
		public System.Windows.Forms.ComboBox cbLocation {
			get { return withEventsField_cbLocation; }
			set {
				if (withEventsField_cbLocation != null) {
					withEventsField_cbLocation.BindingContextChanged -= cbBindingContextChanged;
					withEventsField_cbLocation.SelectedIndexChanged -= cbSelectedIndexChanged;
					withEventsField_cbLocation.SelectedValueChanged -= cbSelectedValueChanged;
					withEventsField_cbLocation.TextChanged -= cbTextChanged;
				}
				withEventsField_cbLocation = value;
				if (withEventsField_cbLocation != null) {
					withEventsField_cbLocation.BindingContextChanged += cbBindingContextChanged;
					withEventsField_cbLocation.SelectedIndexChanged += cbSelectedIndexChanged;
					withEventsField_cbLocation.SelectedValueChanged += cbSelectedValueChanged;
					withEventsField_cbLocation.TextChanged += cbTextChanged;
				}
			}
		}
		public TextBox txtAlphaSort;
		private PictureBox withEventsField_pbGeneral;
		public PictureBox pbGeneral {
			get { return withEventsField_pbGeneral; }
			set {
				if (withEventsField_pbGeneral != null) {
					withEventsField_pbGeneral.DoubleClick -= pbGeneral_DoubleClick;
				}
				withEventsField_pbGeneral = value;
				if (withEventsField_pbGeneral != null) {
					withEventsField_pbGeneral.DoubleClick += pbGeneral_DoubleClick;
				}
			}
		}
		public TextBox txtCaption;
		public TextBox txtID;
		public RichTextBox rtfNotes;
		public TextBox txtPrice;
		public System.Windows.Forms.ComboBox cbAlphaSort;
		private GroupBox withEventsField_gbGeneral;
		public GroupBox gbGeneral {
			get { return withEventsField_gbGeneral; }
			set {
				if (withEventsField_gbGeneral != null) {
					withEventsField_gbGeneral.ClientSizeChanged -= gbGeneral_ClientSizeChanged;
					withEventsField_gbGeneral.SizeChanged -= gbGeneral_SizeChanged;
				}
				withEventsField_gbGeneral = value;
				if (withEventsField_gbGeneral != null) {
					withEventsField_gbGeneral.ClientSizeChanged += gbGeneral_ClientSizeChanged;
					withEventsField_gbGeneral.SizeChanged += gbGeneral_SizeChanged;
				}
			}
		}
		public TabControl tcMain;
		public TabPage tpGeneral;
		public TabPage tpNotes;
		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuRecords;
		private System.Windows.Forms.MenuItem mnuFileReport;
		private System.Windows.Forms.MenuItem mnuFileSQL;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuFileSep;
		private System.Windows.Forms.MenuItem mnuFileTrace;
		private System.Windows.Forms.MenuItem mnuRecordsNew;
		private System.Windows.Forms.MenuItem mnuRecordsCopy;
		private System.Windows.Forms.MenuItem mnuRecordsModify;
		private System.Windows.Forms.MenuItem mnuRecordsDelete;
		private System.Windows.Forms.MenuItem mnuRecordsSep1;
		private System.Windows.Forms.MenuItem mnuRecordsRefresh;
		private System.Windows.Forms.MenuItem mnuRecordsSep2;
		private System.Windows.Forms.MenuItem mnuRecordsFilter;
        private System.Windows.Forms.MenuItem mnuRecordsSort;
        private System.Windows.Forms.MenuItem mnuRecordsList;
		private System.Windows.Forms.MenuItem mnuRecordsSelect;
		private System.Windows.Forms.MenuItem mnuRecordsSep3;
		private ContextMenu withEventsField_ctxImage;
		private ContextMenu ctxImage {
			get { return withEventsField_ctxImage; }
			set {
				if (withEventsField_ctxImage != null) {
					withEventsField_ctxImage.Popup -= ctxImage_Popup;
				}
				withEventsField_ctxImage = value;
				if (withEventsField_ctxImage != null) {
					withEventsField_ctxImage.Popup += ctxImage_Popup;
				}
			}
		}
		private MenuItem withEventsField_mnuImageImport;
		private MenuItem mnuImageImport {
			get { return withEventsField_mnuImageImport; }
			set {
				if (withEventsField_mnuImageImport != null) {
					withEventsField_mnuImageImport.Click -= mnuImageImport_Click;
				}
				withEventsField_mnuImageImport = value;
				if (withEventsField_mnuImageImport != null) {
					withEventsField_mnuImageImport.Click += mnuImageImport_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuImageSaveAs;
		private MenuItem mnuImageSaveAs {
			get { return withEventsField_mnuImageSaveAs; }
			set {
				if (withEventsField_mnuImageSaveAs != null) {
					withEventsField_mnuImageSaveAs.Click -= mnuImageSaveAs_Click;
				}
				withEventsField_mnuImageSaveAs = value;
				if (withEventsField_mnuImageSaveAs != null) {
					withEventsField_mnuImageSaveAs.Click += mnuImageSaveAs_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuImageCopy;
		private System.Windows.Forms.MenuItem mnuImageCopy {
			get { return withEventsField_mnuImageCopy; }
			set {
				if (withEventsField_mnuImageCopy != null) {
					withEventsField_mnuImageCopy.Click -= mnuImageCopy_Click;
				}
				withEventsField_mnuImageCopy = value;
				if (withEventsField_mnuImageCopy != null) {
					withEventsField_mnuImageCopy.Click += mnuImageCopy_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuImagePaste;
		private System.Windows.Forms.MenuItem mnuImagePaste {
			get { return withEventsField_mnuImagePaste; }
			set {
				if (withEventsField_mnuImagePaste != null) {
					withEventsField_mnuImagePaste.Click -= mnuImagePaste_Click;
				}
				withEventsField_mnuImagePaste = value;
				if (withEventsField_mnuImagePaste != null) {
					withEventsField_mnuImagePaste.Click += mnuImagePaste_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem mnuImageSep;
		public System.Windows.Forms.HScrollBar hsbGeneral;
		private System.Windows.Forms.PictureBox withEventsField_pbGeneral2;
		public System.Windows.Forms.PictureBox pbGeneral2 {
			get { return withEventsField_pbGeneral2; }
			set {
				if (withEventsField_pbGeneral2 != null) {
					withEventsField_pbGeneral2.DoubleClick -= pbGeneral_DoubleClick;
				}
				withEventsField_pbGeneral2 = value;
				if (withEventsField_pbGeneral2 != null) {
					withEventsField_pbGeneral2.DoubleClick += pbGeneral_DoubleClick;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuImageClear;
		private System.Windows.Forms.MenuItem mnuImageClear {
			get { return withEventsField_mnuImageClear; }
			set {
				if (withEventsField_mnuImageClear != null) {
					withEventsField_mnuImageClear.Click -= mnuImageClear_Click;
				}
				withEventsField_mnuImageClear = value;
				if (withEventsField_mnuImageClear != null) {
					withEventsField_mnuImageClear.Click += mnuImageClear_Click;
				}
			}
		}
		protected internal System.Windows.Forms.Label lblValue;
		public System.Windows.Forms.TextBox txtValue;
		protected internal System.Windows.Forms.Label lblVerified;
		internal StatusBarPanel sbpTrace;
		public System.Windows.Forms.DateTimePicker dtpVerified;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTCStandard));
			this.btnLast = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.txtCaption = new System.Windows.Forms.TextBox();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrev = new System.Windows.Forms.Button();
			this.sbStatus = new System.Windows.Forms.StatusBar();
			this.sbpPosition = new System.Windows.Forms.StatusBarPanel();
			this.sbpMode = new System.Windows.Forms.StatusBarPanel();
			this.sbpFilter = new System.Windows.Forms.StatusBarPanel();
			this.sbpStatus = new System.Windows.Forms.StatusBarPanel();
			this.sbpMessage = new System.Windows.Forms.StatusBarPanel();
			this.sbpTrace = new System.Windows.Forms.StatusBarPanel();
			this.sbpTime = new System.Windows.Forms.StatusBarPanel();
			this.sbpEndBorder = new System.Windows.Forms.StatusBarPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblID = new System.Windows.Forms.Label();
			this.tbMain = new System.Windows.Forms.ToolBar();
			this.tbbSelect = new System.Windows.Forms.ToolBarButton();
			this.tbbSep1 = new System.Windows.Forms.ToolBarButton();
			this.tbbNew = new System.Windows.Forms.ToolBarButton();
			this.tbbCopyAppend = new System.Windows.Forms.ToolBarButton();
			this.tbbModify = new System.Windows.Forms.ToolBarButton();
			this.tbbDelete = new System.Windows.Forms.ToolBarButton();
			this.tbbSep2 = new System.Windows.Forms.ToolBarButton();
			this.tbbRefresh = new System.Windows.Forms.ToolBarButton();
			this.tbbSep3 = new System.Windows.Forms.ToolBarButton();
			this.tbbFilter = new System.Windows.Forms.ToolBarButton();
            this.tbbSort = new System.Windows.Forms.ToolBarButton();
            this.tbbList = new System.Windows.Forms.ToolBarButton();
			this.tbbSep4 = new System.Windows.Forms.ToolBarButton();
			this.tbbReport = new System.Windows.Forms.ToolBarButton();
			this.tbbSQL = new System.Windows.Forms.ToolBarButton();
			this.tbbSep5 = new System.Windows.Forms.ToolBarButton();
			this.tbbDumpDataView = new System.Windows.Forms.ToolBarButton();
			this.tbbDumpDataTable = new System.Windows.Forms.ToolBarButton();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.gbGeneral = new System.Windows.Forms.GroupBox();
			this.lblVerified = new System.Windows.Forms.Label();
			this.dtpVerified = new System.Windows.Forms.DateTimePicker();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.pbGeneral2 = new System.Windows.Forms.PictureBox();
			this.ctxImage = new System.Windows.Forms.ContextMenu();
			this.mnuImageClear = new System.Windows.Forms.MenuItem();
			this.mnuImageCopy = new System.Windows.Forms.MenuItem();
			this.mnuImagePaste = new System.Windows.Forms.MenuItem();
			this.mnuImageSep = new System.Windows.Forms.MenuItem();
			this.mnuImageImport = new System.Windows.Forms.MenuItem();
			this.mnuImageSaveAs = new System.Windows.Forms.MenuItem();
			this.hsbGeneral = new System.Windows.Forms.HScrollBar();
			this.cbAlphaSort = new System.Windows.Forms.ComboBox();
			this.txtPrice = new System.Windows.Forms.TextBox();
			this.lblPrice = new System.Windows.Forms.Label();
			this.pbGeneral = new System.Windows.Forms.PictureBox();
			this.chkWishList = new System.Windows.Forms.CheckBox();
			this.lblPurchased = new System.Windows.Forms.Label();
			this.dtpPurchased = new System.Windows.Forms.DateTimePicker();
			this.lblInventoried = new System.Windows.Forms.Label();
			this.dtpInventoried = new System.Windows.Forms.DateTimePicker();
			this.cbLocation = new System.Windows.Forms.ComboBox();
			this.lblLocation = new System.Windows.Forms.Label();
			this.txtAlphaSort = new System.Windows.Forms.TextBox();
			this.lblAlphaSort = new System.Windows.Forms.Label();
			this.tpNotes = new System.Windows.Forms.TabPage();
			this.rtfNotes = new System.Windows.Forms.RichTextBox();
			this.timClock = new System.Windows.Forms.Timer(this.components);
			this.btnExit = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ofdTCStandard = new System.Windows.Forms.OpenFileDialog();
			this.sfdTCStandard = new System.Windows.Forms.SaveFileDialog();
			this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileReport = new System.Windows.Forms.MenuItem();
			this.mnuFileSQL = new System.Windows.Forms.MenuItem();
			this.mnuFileSep = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuRecords = new System.Windows.Forms.MenuItem();
			this.mnuRecordsSelect = new System.Windows.Forms.MenuItem();
			this.mnuRecordsSep1 = new System.Windows.Forms.MenuItem();
			this.mnuRecordsNew = new System.Windows.Forms.MenuItem();
			this.mnuRecordsCopy = new System.Windows.Forms.MenuItem();
			this.mnuRecordsModify = new System.Windows.Forms.MenuItem();
			this.mnuRecordsDelete = new System.Windows.Forms.MenuItem();
			this.mnuRecordsSep2 = new System.Windows.Forms.MenuItem();
			this.mnuRecordsRefresh = new System.Windows.Forms.MenuItem();
			this.mnuRecordsSep3 = new System.Windows.Forms.MenuItem();
			this.mnuRecordsFilter = new System.Windows.Forms.MenuItem();
            this.mnuRecordsSort = new System.Windows.Forms.MenuItem();
			this.mnuRecordsList = new System.Windows.Forms.MenuItem();
			this.mnuFileTrace = new System.Windows.Forms.MenuItem();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslblRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblRowFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblSort = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTrace).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			this.tcMain.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.gbGeneral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral).BeginInit();
			this.tpNotes.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.SuspendLayout();
			//
			//imgBase
			//
			this.imgBase.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgBase.ImageStream");
			this.imgBase.Images.SetKeyName(0, "");
			this.imgBase.Images.SetKeyName(1, "");
			this.imgBase.Images.SetKeyName(2, "");
			this.imgBase.Images.SetKeyName(3, "");
			this.imgBase.Images.SetKeyName(4, "");
			this.imgBase.Images.SetKeyName(5, "");
			this.imgBase.Images.SetKeyName(6, "");
			this.imgBase.Images.SetKeyName(7, "");
			this.imgBase.Images.SetKeyName(8, "");
			this.imgBase.Images.SetKeyName(9, "");
			this.imgBase.Images.SetKeyName(10, "");
			this.imgBase.Images.SetKeyName(11, "");
			this.imgBase.Images.SetKeyName(12, "");
			this.imgBase.Images.SetKeyName(13, "");
			this.imgBase.Images.SetKeyName(14, "");
			this.imgBase.Images.SetKeyName(15, "");
			this.imgBase.Images.SetKeyName(16, "");
			this.imgBase.Images.SetKeyName(17, "");
			this.imgBase.Images.SetKeyName(18, "");
			this.imgBase.Images.SetKeyName(19, "");
			this.imgBase.Images.SetKeyName(20, "");
			this.imgBase.Images.SetKeyName(21, "");
			this.imgBase.Images.SetKeyName(22, "");
			this.imgBase.Images.SetKeyName(23, "CHECKMRK.ICO");
			this.imgBase.Images.SetKeyName(24, "");
			//
			//btnLast
			//
			this.btnLast.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnLast.Image = (System.Drawing.Image)resources.GetObject("btnLast.Image");
			this.btnLast.Location = new System.Drawing.Point(488, 344);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(25, 25);
			this.btnLast.TabIndex = 25;
			this.btnLast.TabStop = false;
			//
			//btnFirst
			//
			this.btnFirst.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnFirst.Image = (System.Drawing.Image)resources.GetObject("btnFirst.Image");
			this.btnFirst.Location = new System.Drawing.Point(104, 344);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(25, 25);
			this.btnFirst.TabIndex = 24;
			this.btnFirst.TabStop = false;
			//
			//txtCaption
			//
			this.txtCaption.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtCaption.Enabled = false;
			this.txtCaption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCaption.Location = new System.Drawing.Point(156, 344);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.ReadOnly = true;
			this.txtCaption.Size = new System.Drawing.Size(296, 23);
			this.txtCaption.TabIndex = 23;
			this.txtCaption.TabStop = false;
			this.txtCaption.Tag = "Ignore";
			this.txtCaption.Text = "txtCaption";
			//
			//btnNext
			//
			this.btnNext.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnNext.Image = (System.Drawing.Image)resources.GetObject("btnNext.Image");
			this.btnNext.Location = new System.Drawing.Point(460, 344);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(25, 25);
			this.btnNext.TabIndex = 22;
			this.btnNext.TabStop = false;
			//
			//btnPrev
			//
			this.btnPrev.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnPrev.Image = (System.Drawing.Image)resources.GetObject("btnPrev.Image");
			this.btnPrev.Location = new System.Drawing.Point(128, 344);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.Size = new System.Drawing.Size(25, 25);
			this.btnPrev.TabIndex = 21;
			this.btnPrev.TabStop = false;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 244);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
				this.sbpPosition,
				this.sbpMode,
				this.sbpFilter,
				this.sbpStatus,
				this.sbpMessage,
				this.sbpTrace,
				this.sbpTime,
				this.sbpEndBorder
			});
			this.sbStatus.ShowPanels = true;
			this.sbStatus.Size = new System.Drawing.Size(296, 22);
			this.sbStatus.TabIndex = 20;
			//
			//sbpPosition
			//
			this.sbpPosition.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpPosition.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpPosition.Name = "sbpPosition";
			this.sbpPosition.Text = "Record x of y";
			this.sbpPosition.ToolTipText = "Record Position";
			this.sbpPosition.Width = 80;
			//
			//sbpMode
			//
			this.sbpMode.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.sbpMode.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpMode.Name = "sbpMode";
			this.sbpMode.Text = "Mode";
			this.sbpMode.ToolTipText = "Mode";
			this.sbpMode.Width = 43;
			//
			//sbpFilter
			//
			this.sbpFilter.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpFilter.Name = "sbpFilter";
			this.sbpFilter.Text = "Filter";
			this.sbpFilter.ToolTipText = "Filter";
			this.sbpFilter.Width = 39;
			//
			//sbpStatus
			//
			this.sbpStatus.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.sbpStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpStatus.Name = "sbpStatus";
			this.sbpStatus.Text = "Status";
			this.sbpStatus.ToolTipText = "Status";
			this.sbpStatus.Width = 46;
			//
			//sbpMessage
			//
			this.sbpMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpMessage.Name = "sbpMessage";
			this.sbpMessage.Text = "Message";
			this.sbpMessage.ToolTipText = "Message";
			this.sbpMessage.Width = 10;
			//
			//sbpTrace
			//
			this.sbpTrace.MinWidth = 0;
			this.sbpTrace.Name = "sbpTrace";
			this.sbpTrace.Width = 0;
			//
			//sbpTime
			//
			this.sbpTime.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpTime.MinWidth = 70;
			this.sbpTime.Name = "sbpTime";
			this.sbpTime.Text = "12:59 PM";
			this.sbpTime.Width = 70;
			//
			//sbpEndBorder
			//
			this.sbpEndBorder.MinWidth = 1;
			this.sbpEndBorder.Name = "sbpEndBorder";
			this.sbpEndBorder.Width = 1;
			//
			//btnOK
			//
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.Location = new System.Drawing.Point(520, 344);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 25);
			this.btnOK.TabIndex = 18;
			this.btnOK.Text = "OK";
			//
			//txtID
			//
			this.txtID.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.txtID.BackColor = System.Drawing.SystemColors.Control;
			this.txtID.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtID.Enabled = false;
			this.txtID.Location = new System.Drawing.Point(40, 348);
			this.txtID.Name = "txtID";
			this.txtID.ReadOnly = true;
			this.txtID.Size = new System.Drawing.Size(52, 16);
			this.txtID.TabIndex = 17;
			this.txtID.TabStop = false;
			this.txtID.Tag = "Ignore";
			this.txtID.Text = "txtID";
			//
			//lblID
			//
			this.lblID.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(12, 347);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(22, 16);
			this.lblID.TabIndex = 16;
			this.lblID.Text = "ID";
			//
			//tbMain
			//
			this.tbMain.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.tbMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
				this.tbbSelect,
				this.tbbSep1,
				this.tbbNew,
				this.tbbCopyAppend,
				this.tbbModify,
				this.tbbDelete,
				this.tbbSep2,
				this.tbbRefresh,
				this.tbbSep3,
				this.tbbFilter,
                this.tbbSort,
				this.tbbList,
				this.tbbSep4,
				this.tbbReport,
				this.tbbSQL,
				this.tbbSep5,
				this.tbbDumpDataView,
				this.tbbDumpDataTable
			});
			this.tbMain.ButtonSize = new System.Drawing.Size(24, 24);
			this.tbMain.Dock = System.Windows.Forms.DockStyle.None;
			this.tbMain.DropDownArrows = true;
			this.tbMain.ImageList = this.imgToolbar;
			this.tbMain.Location = new System.Drawing.Point(0, 0);
			this.tbMain.Name = "tbMain";
			this.tbMain.ShowToolTips = true;
			this.tbMain.Size = new System.Drawing.Size(776, 28);
			this.tbMain.TabIndex = 14;
			//
			//tbbSelect
			//
			this.tbbSelect.ImageIndex = 0;
			this.tbbSelect.Name = "tbbSelect";
			this.tbbSelect.ToolTipText = "Select";
			this.tbbSelect.Visible = false;
			//
			//tbbSep1
			//
			this.tbbSep1.Name = "tbbSep1";
			this.tbbSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			this.tbbSep1.Visible = false;
			//
			//tbbNew
			//
			this.tbbNew.ImageIndex = 1;
			this.tbbNew.Name = "tbbNew";
			this.tbbNew.ToolTipText = "New";
			//
			//tbbCopyAppend
			//
			this.tbbCopyAppend.ImageIndex = 11;
			this.tbbCopyAppend.Name = "tbbCopyAppend";
			this.tbbCopyAppend.ToolTipText = "Copy/Append";
			//
			//tbbModify
			//
			this.tbbModify.ImageIndex = 2;
			this.tbbModify.Name = "tbbModify";
			this.tbbModify.ToolTipText = "Modify";
			//
			//tbbDelete
			//
			this.tbbDelete.ImageIndex = 3;
			this.tbbDelete.Name = "tbbDelete";
			this.tbbDelete.ToolTipText = "Delete";
			//
			//tbbSep2
			//
			this.tbbSep2.Name = "tbbSep2";
			this.tbbSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			this.tbbSep2.Visible = false;
			//
			//tbbRefresh
			//
			this.tbbRefresh.ImageIndex = 4;
			this.tbbRefresh.Name = "tbbRefresh";
			this.tbbRefresh.ToolTipText = "Refresh";
			this.tbbRefresh.Visible = false;
			//
			//tbbSep3
			//
			this.tbbSep3.Name = "tbbSep3";
			this.tbbSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			//
			//tbbFilter
			//
			this.tbbFilter.ImageIndex = 5;
			this.tbbFilter.Name = "tbbFilter";
			this.tbbFilter.ToolTipText = "Filter";
            //
            //tbbSort
            //
            this.tbbSort.ImageIndex = 13;
            this.tbbSort.Name = "tbbSort";
            this.tbbSort.ToolTipText = "Sort";
            //
            //tbbList
            //
            this.tbbList.ImageIndex = 6;
			this.tbbList.Name = "tbbList";
			this.tbbList.ToolTipText = "List";
			//
			//tbbSep4
			//
			this.tbbSep4.Name = "tbbSep4";
			this.tbbSep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			//
			//tbbReport
			//
			this.tbbReport.ImageIndex = 10;
			this.tbbReport.Name = "tbbReport";
			this.tbbReport.ToolTipText = "Report";
			//
			//tbbSQL
			//
			this.tbbSQL.ImageIndex = 9;
			this.tbbSQL.Name = "tbbSQL";
			this.tbbSQL.ToolTipText = "SQL";
			//
			//tbbSep5
			//
			this.tbbSep5.Name = "tbbSep5";
			this.tbbSep5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			//
			//tbbDumpDataView
			//
			this.tbbDumpDataView.ImageIndex = 12;
			this.tbbDumpDataView.Name = "tbbDumpDataView";
			this.tbbDumpDataView.ToolTipText = "Dump tcDataView";
			this.tbbDumpDataView.Visible = false;
			//
			//tbbDumpDataTable
			//
			this.tbbDumpDataTable.ImageIndex = 12;
			this.tbbDumpDataTable.Name = "tbbDumpDataTable";
			this.tbbDumpDataTable.ToolTipText = "Dump tcDataTable";
			this.tbbDumpDataTable.Visible = false;
			//
			//imgToolbar
			//
			this.imgToolbar.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgToolbar.ImageStream");
			this.imgToolbar.TransparentColor = System.Drawing.Color.Transparent;
			this.imgToolbar.Images.SetKeyName(0, "");
			this.imgToolbar.Images.SetKeyName(1, "");
			this.imgToolbar.Images.SetKeyName(2, "");
			this.imgToolbar.Images.SetKeyName(3, "");
			this.imgToolbar.Images.SetKeyName(4, "");
			this.imgToolbar.Images.SetKeyName(5, "");
			this.imgToolbar.Images.SetKeyName(6, "");
			this.imgToolbar.Images.SetKeyName(7, "");
			this.imgToolbar.Images.SetKeyName(8, "");
			this.imgToolbar.Images.SetKeyName(9, "");
			this.imgToolbar.Images.SetKeyName(10, "");
			this.imgToolbar.Images.SetKeyName(11, "");
			this.imgToolbar.Images.SetKeyName(12, "");
            this.imgToolbar.Images.SetKeyName(13, "Sort.ico");
			//
            //tcMain
			//
			this.tcMain.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.tcMain.Controls.Add(this.tpGeneral);
			this.tcMain.Controls.Add(this.tpNotes);
			this.tcMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.tcMain.Location = new System.Drawing.Point(8, 32);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(664, 300);
			this.tcMain.TabIndex = 0;
			//
			//tpGeneral
			//
			this.tpGeneral.Controls.Add(this.gbGeneral);
			this.tpGeneral.Location = new System.Drawing.Point(4, 25);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(656, 271);
			this.tpGeneral.TabIndex = 0;
			this.tpGeneral.Text = "General";
			//
			//gbGeneral
			//
			this.gbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbGeneral.AutoSize = false;
			this.gbGeneral.BackColor = System.Drawing.SystemColors.Control;
			this.gbGeneral.Controls.Add(this.lblVerified);
			this.gbGeneral.Controls.Add(this.dtpVerified);
			this.gbGeneral.Controls.Add(this.txtValue);
			this.gbGeneral.Controls.Add(this.lblValue);
			this.gbGeneral.Controls.Add(this.pbGeneral2);
			this.gbGeneral.Controls.Add(this.hsbGeneral);
			this.gbGeneral.Controls.Add(this.cbAlphaSort);
			this.gbGeneral.Controls.Add(this.txtPrice);
			this.gbGeneral.Controls.Add(this.lblPrice);
			this.gbGeneral.Controls.Add(this.pbGeneral);
			this.gbGeneral.Controls.Add(this.chkWishList);
			this.gbGeneral.Controls.Add(this.lblPurchased);
			this.gbGeneral.Controls.Add(this.dtpPurchased);
			this.gbGeneral.Controls.Add(this.lblInventoried);
			this.gbGeneral.Controls.Add(this.dtpInventoried);
			this.gbGeneral.Controls.Add(this.cbLocation);
			this.gbGeneral.Controls.Add(this.lblLocation);
			this.gbGeneral.Controls.Add(this.txtAlphaSort);
			this.gbGeneral.Controls.Add(this.lblAlphaSort);
			this.gbGeneral.Location = new System.Drawing.Point(8, 8);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(644, 256);
			this.gbGeneral.TabIndex = 0;
			this.gbGeneral.TabStop = false;
			//
			//lblVerified
			//
			this.lblVerified.AutoSize = true;
			this.lblVerified.Location = new System.Drawing.Point(72, 72);
			this.lblVerified.Name = "lblVerified";
			this.lblVerified.Size = new System.Drawing.Size(57, 16);
			this.lblVerified.TabIndex = 84;
			this.lblVerified.Text = "Verified";
			this.lblVerified.Visible = false;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpVerified.Location = new System.Drawing.Point(176, 68);
			this.dtpVerified.MinDate = new System.DateTime(1963, 7, 31, 0, 0, 0, 0);
			this.dtpVerified.Name = "dtpVerified";
			this.dtpVerified.Size = new System.Drawing.Size(116, 23);
			this.dtpVerified.TabIndex = 83;
			this.dtpVerified.Tag = "Nulls";
			this.ttBase.SetToolTip(this.dtpVerified, "Date the replacement value of this item was verified with common retailers");
			this.dtpVerified.Visible = false;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(64, 96);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(100, 23);
			this.txtValue.TabIndex = 82;
			this.txtValue.Tag = "Money,Nulls";
			this.txtValue.Text = "txtValue";
			this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ttBase.SetToolTip(this.txtValue, "Replacement value of this item");
			this.txtValue.Visible = false;
			//
			//lblValue
			//
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(16, 100);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(44, 16);
			this.lblValue.TabIndex = 81;
			this.lblValue.Text = "Value";
			this.lblValue.Visible = false;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.pbGeneral2.ContextMenu = this.ctxImage;
			this.pbGeneral2.Image = (System.Drawing.Image)resources.GetObject("pbGeneral2.Image");
			this.pbGeneral2.Location = new System.Drawing.Point(500, 38);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(129, 89);
			this.pbGeneral2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbGeneral2.TabIndex = 80;
			this.pbGeneral2.TabStop = false;
			//
			//ctxImage
			//
			this.ctxImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuImageClear,
				this.mnuImageCopy,
				this.mnuImagePaste,
				this.mnuImageSep,
				this.mnuImageImport,
				this.mnuImageSaveAs
			});
			//
			//mnuImageClear
			//
			this.mnuImageClear.Index = 0;
			this.mnuImageClear.Text = "C&lear";
			this.mnuImageClear.Visible = false;
			//
			//mnuImageCopy
			//
			this.mnuImageCopy.Index = 1;
			this.mnuImageCopy.Text = "&Copy";
			//
			//mnuImagePaste
			//
			this.mnuImagePaste.Index = 2;
			this.mnuImagePaste.Text = "&Paste";
			this.mnuImagePaste.Visible = false;
			//
			//mnuImageSep
			//
			this.mnuImageSep.Index = 3;
			this.mnuImageSep.Text = "-";
			this.mnuImageSep.Visible = false;
			//
			//mnuImageImport
			//
			this.mnuImageImport.Index = 4;
			this.mnuImageImport.Text = "&Import";
			this.mnuImageImport.Visible = false;
			//
			//mnuImageSaveAs
			//
			this.mnuImageSaveAs.Index = 5;
			this.mnuImageSaveAs.Text = "Save &As";
			//
			//hsbGeneral
			//
			this.hsbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.hsbGeneral.LargeChange = 1;
			this.hsbGeneral.Location = new System.Drawing.Point(540, 148);
			this.hsbGeneral.Maximum = 1;
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(48, 17);
			this.hsbGeneral.TabIndex = 79;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbAlphaSort.Location = new System.Drawing.Point(88, 188);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(520, 24);
			this.cbAlphaSort.Sorted = true;
			this.cbAlphaSort.TabIndex = 48;
			this.cbAlphaSort.Tag = "UPPER";
			this.cbAlphaSort.Text = "cbAlphaSort";
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(294, 117);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.Size = new System.Drawing.Size(100, 23);
			this.txtPrice.TabIndex = 59;
			this.txtPrice.Tag = "Money,Nulls";
			this.txtPrice.Text = "txtPrice";
			this.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ttBase.SetToolTip(this.txtPrice, "Purchase price of this item");
			//
			//lblPrice
			//
			this.lblPrice.AutoSize = true;
			this.lblPrice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblPrice.Location = new System.Drawing.Point(246, 119);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.Size = new System.Drawing.Size(40, 16);
			this.lblPrice.TabIndex = 58;
			this.lblPrice.Text = "Price";
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.pbGeneral.BackgroundImage = (System.Drawing.Image)resources.GetObject("pbGeneral.BackgroundImage");
			this.pbGeneral.ContextMenu = this.ctxImage;
			this.pbGeneral.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.pbGeneral.Image = (System.Drawing.Image)resources.GetObject("pbGeneral.Image");
			this.pbGeneral.Location = new System.Drawing.Point(500, 38);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(129, 89);
			this.pbGeneral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbGeneral.TabIndex = 57;
			this.pbGeneral.TabStop = false;
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.chkWishList.Location = new System.Drawing.Point(488, 220);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.Size = new System.Drawing.Size(96, 24);
			this.chkWishList.TabIndex = 56;
			this.chkWishList.Text = "Wish List";
			this.ttBase.SetToolTip(this.chkWishList, "Is this a WishList item?");
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.AutoSize = true;
			this.lblPurchased.Location = new System.Drawing.Point(12, 223);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.Size = new System.Drawing.Size(76, 16);
			this.lblPurchased.TabIndex = 53;
			this.lblPurchased.Text = "Purchased";
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpPurchased.Location = new System.Drawing.Point(92, 221);
			this.dtpPurchased.MinDate = new System.DateTime(1963, 7, 31, 0, 0, 0, 0);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.Size = new System.Drawing.Size(116, 23);
			this.dtpPurchased.TabIndex = 52;
			this.dtpPurchased.Tag = "Nulls";
			this.ttBase.SetToolTip(this.dtpPurchased, "Date this item was purchased");
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.AutoSize = true;
			this.lblInventoried.Location = new System.Drawing.Point(248, 223);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.Size = new System.Drawing.Size(83, 16);
			this.lblInventoried.TabIndex = 51;
			this.lblInventoried.Text = "Inventoried";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpInventoried.Location = new System.Drawing.Point(336, 221);
			this.dtpInventoried.MinDate = new System.DateTime(1963, 7, 31, 0, 0, 0, 0);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.Size = new System.Drawing.Size(116, 23);
			this.dtpInventoried.TabIndex = 50;
			this.dtpInventoried.Tag = "Nulls";
			this.ttBase.SetToolTip(this.dtpInventoried, "Date this item was last inventoried (i.e. location confirmed)");
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbLocation.Location = new System.Drawing.Point(88, 156);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(520, 24);
			this.cbLocation.TabIndex = 49;
			this.cbLocation.Text = "cbLocation";
			this.ttBase.SetToolTip(this.cbLocation, "Current location of this item (i.e. Where is it?)");
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.AutoSize = true;
			this.lblLocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblLocation.Location = new System.Drawing.Point(12, 159);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(64, 16);
			this.lblLocation.TabIndex = 48;
			this.lblLocation.Text = "Location";
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtAlphaSort.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtAlphaSort.Location = new System.Drawing.Point(88, 188);
			this.txtAlphaSort.Name = "txtAlphaSort";
			this.txtAlphaSort.Size = new System.Drawing.Size(520, 23);
			this.txtAlphaSort.TabIndex = 47;
			this.txtAlphaSort.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.AutoSize = true;
			this.lblAlphaSort.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblAlphaSort.Location = new System.Drawing.Point(8, 190);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.Size = new System.Drawing.Size(72, 16);
			this.lblAlphaSort.TabIndex = 46;
			this.lblAlphaSort.Text = "AlphaSort";
			//
			//tpNotes
			//
			this.tpNotes.Controls.Add(this.rtfNotes);
			this.tpNotes.Location = new System.Drawing.Point(4, 25);
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(656, 271);
			this.tpNotes.TabIndex = 1;
			this.tpNotes.Text = "Notes";
			this.tpNotes.Visible = false;
			//
			//rtfNotes
			//
			this.rtfNotes.AcceptsTab = true;
			this.rtfNotes.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfNotes.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.rtfNotes.Location = new System.Drawing.Point(4, 4);
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(652, 268);
			this.rtfNotes.TabIndex = 0;
			this.rtfNotes.Text = "rtfNotes";
			//
			//timClock
			//
			this.timClock.Enabled = true;
			this.timClock.Interval = 1000;
			//
			//btnExit
			//
			this.btnExit.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnExit.CausesValidation = false;
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(600, 344);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 25);
			this.btnExit.TabIndex = 19;
			this.btnExit.Text = "E&xit";
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.CausesValidation = false;
			this.btnCancel.Location = new System.Drawing.Point(600, 344);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 26;
			this.btnCancel.Text = "Cancel";
			//
			//mnuMain
			//
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuFile,
				this.mnuRecords
			});
			//
			//mnuFile
			//
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuFileReport,
				this.mnuFileSQL,
				this.mnuFileSep,
				this.mnuFileExit
			});
			this.mnuFile.Text = "&File";
			//
			//mnuFileReport
			//
			this.mnuFileReport.Index = 0;
			this.mnuFileReport.Text = "&Report";
			//
			//mnuFileSQL
			//
			this.mnuFileSQL.Index = 1;
			this.mnuFileSQL.Text = "&SQL";
			//
			//mnuFileSep
			//
			this.mnuFileSep.Index = 2;
			this.mnuFileSep.Text = "-";
			//
			//mnuFileExit
			//
			this.mnuFileExit.Index = 3;
			this.mnuFileExit.Text = "E&xit";
			//
			//mnuRecords
			//
			this.mnuRecords.Index = 1;
			this.mnuRecords.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuRecordsSelect,
				this.mnuRecordsSep1,
				this.mnuRecordsNew,
				this.mnuRecordsCopy,
				this.mnuRecordsModify,
				this.mnuRecordsDelete,
				this.mnuRecordsSep2,
				this.mnuRecordsRefresh,
				this.mnuRecordsSep3,
				this.mnuRecordsFilter,
                this.mnuRecordsSort,
				this.mnuRecordsList
			});
			this.mnuRecords.Text = "&Records";
			//
			//mnuRecordsSelect
			//
			this.mnuRecordsSelect.Index = 0;
			this.mnuRecordsSelect.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftR;
			this.mnuRecordsSelect.Text = "&Select";
			//
			//mnuRecordsSep1
			//
			this.mnuRecordsSep1.Index = 1;
			this.mnuRecordsSep1.Text = "-";
			//
			//mnuRecordsNew
			//
			this.mnuRecordsNew.Index = 2;
			this.mnuRecordsNew.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftN;
			this.mnuRecordsNew.Text = "&New";
			//
			//mnuRecordsCopy
			//
			this.mnuRecordsCopy.Index = 3;
			this.mnuRecordsCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftI;
			this.mnuRecordsCopy.Text = "&Copy/Append";
			//
			//mnuRecordsModify
			//
			this.mnuRecordsModify.Index = 4;
			this.mnuRecordsModify.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.mnuRecordsModify.Text = "&Modify";
			//
			//mnuRecordsDelete
			//
			this.mnuRecordsDelete.Index = 5;
			this.mnuRecordsDelete.Text = "&Delete";
			//
			//mnuRecordsSep2
			//
			this.mnuRecordsSep2.Index = 6;
			this.mnuRecordsSep2.Text = "-";
			//
			//mnuRecordsRefresh
			//
			this.mnuRecordsRefresh.Index = 7;
			this.mnuRecordsRefresh.Text = "&Refresh";
			this.mnuRecordsRefresh.Visible = false;
			//
			//mnuRecordsSep3
			//
			this.mnuRecordsSep3.Index = 8;
			this.mnuRecordsSep3.Text = "-";
			this.mnuRecordsSep3.Visible = false;
			//
			//mnuRecordsFilter
			//
			this.mnuRecordsFilter.Index = 9;
			this.mnuRecordsFilter.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.mnuRecordsFilter.Text = "&Filter";
			this.mnuRecordsFilter.Visible = false;
            //
            //mnuRecordsSort
            //
            this.mnuRecordsSort.Index = 10;
            this.mnuRecordsSort.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.mnuRecordsSort.Text = "&Sort";
            this.mnuRecordsSort.Visible = false;
            //
            //mnuRecordsList
            //
            this.mnuRecordsList.Index = 11;
			this.mnuRecordsList.Shortcut = System.Windows.Forms.Shortcut.F3;
			this.mnuRecordsList.Text = "&List";
			//
			//mnuFileTrace
			//
			this.mnuFileTrace.Index = -1;
			this.mnuFileTrace.Text = "&Trace";
            //
            //StatusStrip1
            //
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.tsslblRows, this.tsslblRowFilter, this.tsslblSort });
            this.StatusStrip1.Location = new System.Drawing.Point(0, 244);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(444, 22);
            this.StatusStrip1.TabIndex = 1;
            this.StatusStrip1.Text = "StatusStrip1";
            //
            //tsslblRows
            //
            this.tsslblRows.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsslblRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslblRows.Name = "tsslblRows";
            this.tsslblRows.Size = new System.Drawing.Size(47, 17);
            this.tsslblRows.Text = "Rows: 0";
            //
            //tsslblRowFilter
            //
            this.tsslblRowFilter.AutoToolTip = true;
            this.tsslblRowFilter.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsslblRowFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslblRowFilter.Name = "tsslblRowFilter";
            this.tsslblRowFilter.Size = new System.Drawing.Size(62, 17);
            this.tsslblRowFilter.Text = "RowFilter: ";
            //
            //tsslblSort
            //
            this.tsslblSort.AutoToolTip = true;
            this.tsslblSort.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsslblSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslblSort.Name = "tsslblSort";
            this.tsslblSort.Size = new System.Drawing.Size(31, 17);
            this.tsslblSort.Text = "Sort:";
            //
            //frmTCStandard
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(296, 266);
			this.Controls.Add(this.btnLast);
			this.Controls.Add(this.btnFirst);
			this.Controls.Add(this.txtCaption);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnPrev);
			this.Controls.Add(this.sbStatus);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.lblID);
			this.Controls.Add(this.tbMain);
			this.Controls.Add(this.tcMain);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.StatusStrip1);
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.KeyPreview = true;
			this.Menu = this.mnuMain;
			this.Name = "frmTCStandard";
			this.Text = "frmTCStandard";
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTrace).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			this.tcMain.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.gbGeneral.ResumeLayout(false);
			this.gbGeneral.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral).EndInit();
			this.tpNotes.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		#region "Events"
		public event ImageImportedEventHandler ImageImported;
		public delegate void ImageImportedEventHandler(object sender, ImageImportedEventArgs e);
		public virtual void OnImageImported(string ImagePath)
		{
			const string EntryName = "OnImageImported";
			string strArgs = string.Format("ImagePath:=\"{0}\"", ImagePath);
			Trace(string.Format("{0}: RaiseEvent {1}(Me, New ImageImportedEventArgs(\"{2}\")", EntryName, EntryName.Substring("On".Length), ImagePath), trcOption.trcApplication);
			if (ImageImported != null) {
				ImageImported(this, new ImageImportedEventArgs(ImagePath));
			}
		}
		#endregion
		#region "Properties"
		#region "Declarations"
		#region "Enumerations"
		protected enum imgToolBarEnum
		{
			Select,
			New,
			Modify,
			Delete,
			Refresh,
			Filter,
			List,
			Previous,
			Next,
			SQL,
			Report,
			Copy,
			Dump,
            Sort
		}
		protected enum tbButtonEnum : short
		{
			Select,
			Sep1,
			New,
			CopyAppend,
			Modify,
			Delete,
			Sep2,
			Refresh,
			Sep3,
			Filter,
            Sort,
			List,
			Sep4,
			Report,
			SQL,
			Sep5,
			DumpDataView,
			DumpDataTable
		}
		#endregion
		//Dim mKeyEventArgs As KeyEventArgs
		protected bool fActivated = false;
		protected bool fOKtoUnload = true;
			//Used to counter-act default/DialogResult behavior...
		protected string mLastButtonClicked;
		protected string mRegistryKey;
		protected string mReportPath = bpeNullString;
			#endregion
		private ActionModeEnum mMode = ActionModeEnum.modeDisplay;
		public string ReportPath {
			get { return mReportPath; }
			set { mReportPath = value; }
		}
		#endregion
		#region "Methods"
		//TODO: Protected MustOverride Sub BindControls()
		protected virtual void BindControls()
		{
			throw new NotSupportedException();
		}
		protected internal void EnableButtons(bool Enable)
		{
			foreach (MenuItem iMenu in this.Menu.MenuItems) {
				iMenu.Enabled = !Enable;
			}
			//For Each iToolBarButton As ToolBarButton In Me.tbMain.Controls : iToolBarButton.Enabled = Not Enable : Next iToolBarButton
			this.tbMain.Enabled = !Enable;
			this.btnCancel.Visible = Enable;
			this.btnCancel.Enabled = Enable;
			this.btnOK.Visible = Enable;
			this.btnOK.Enabled = Enable;
			//If Me.btnOK.Visible Then
			//    Me.txtCaption.Width -= Me.btnOK.Width
			//    Me.btnNext.Left -= Me.btnOK.Width
			//    Me.btnLast.Left -= Me.btnOK.Width
			//Else
			//    Me.txtCaption.Width += Me.btnOK.Width
			//    Me.btnNext.Left += Me.btnOK.Width
			//    Me.btnLast.Left += Me.btnOK.Width
			//End If
			this.btnExit.Visible = !Enable;
			ControlEnabled(this.btnExit, !Enable);
			ControlEnabled(this.btnFirst, !Enable);
			ControlEnabled(this.btnPrev, !Enable);
			ControlEnabled(this.btnNext, !Enable);
			ControlEnabled(this.btnLast, !Enable);
			ControlEnabled(this.pbGeneral, true);
		}
		protected internal void EnableControls(bool Enable)
		{
			EnableControls(this, Enable, false);
		}
		protected internal void EnableControls(bool Enable, bool Clear)
		{
			EnableControls(this, Enable, Clear);
		}
		protected internal void EnableControls(Form frm, bool Enable, bool Clear)
		{
			foreach (Control ctl in frm.Controls) {
				EnableControl(ctl, Enable, Clear);
			}
			EnableButtons(Enable);
		}
		protected void EnableImageContextMenu(bool Enable)
		{
			mnuImageSep.Visible = Enable;
			mnuImageClear.Visible = Enable;
			mnuImagePaste.Visible = Enable;
			mnuImageImport.Visible = Enable;
		}
		protected void EnableRichTextContextMenu(bool Enable)
		{
			mnuRTFContextMenuCut.Visible = Enable;
			//mnuRTFContextMenuCopy.Visible = Enable
			mnuRTFContextMenuPaste.Visible = Enable;
			mnuRTFContextMenuSep1.Visible = Enable;
			//mnuRTFContextMenuSelectAll.Visible = Enable
			mnuRTFContextMenuSep2.Visible = Enable;
			mnuRTFContextMenuFont.Visible = Enable;
			mnuRTFContextMenuParagraph.Visible = Enable;
			//mnuRTFContextMenuParagraphAlign.Visible = Enable
			//mnuRTFContextMenuParagraphAlignLeft.Visible = Enable
			//mnuRTFContextMenuParagraphAlignCenter.Visible = Enable
			//mnuRTFContextMenuParagraphAlignRight.Visible = Enable
			//mnuRTFContextMenuParagraphSep1.Visible = Enable
			//mnuRTFContextMenuParagraphBullet.Visible = Enable
			//mnuRTFContextMenuParagraphSep2.Visible = Enable
			//mnuRTFContextMenuParagraphIndent.Visible = Enable
			//mnuRTFContextMenuParagraphUnindent.Visible = Enable
		}
		protected internal void FixComboBoxes(Form frm)
		{
			foreach (Control ctl in frm.Controls) {
				FixComboBoxes(ctl);
			}
		}
		protected void LoadDefaultImage()
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			pbGeneral.Image.Save(ms, pbGeneral.Image.RawFormat);
			DefaultImage = ms.GetBuffer();
			//Close the stream object to release the resource.
			ms.Close();
			ms.Dispose();
		}
		//TODO: Protected MustOverride Sub SetCaption(ByVal sender As Object, ByVal e As CancelEventArgs)
		protected virtual void SetCaption(object sender, CancelEventArgs e)
		{
			throw new NotSupportedException();
		}
		#region "Image Processing"
		protected void ClearImage(PictureBox ctl)
		{
			Image img = null;
			//Reset our PictureBox back to the default image...
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmTCStandard));
			Image tmpImage = (System.Drawing.Image)resources.GetObject("pbGeneral.Image");
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			tmpImage.Save(ms, tmpImage.RawFormat);
			byte[] tmpBinary = ms.GetBuffer();
			string dataMember = ctl.DataBindings[0].BindingMemberInfo.BindingMember;
			mTCBase.CurrentRow[dataMember] = tmpBinary;
			ctl.Image = tmpImage;
			tmpImage = null;
			ms.Close();
			ms.Dispose();
		}
		protected void CopyImage(PictureBox ctl)
		{
			DataObject objImage = new DataObject();
			objImage.SetData(DataFormats.Bitmap, true, ctl.Image);
			Clipboard.SetDataObject(objImage, true);
		}
		protected void ExportImage(PictureBox ctl)
		{
			string CurrentPath = ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash);
			string CurrentDrive = ParsePath(mTCBase.ImagePath, ParseParts.DrvOnly);
			string CurrentImage = ParsePath(mTCBase.ImagePath, ParseParts.FileNameBaseExt);
			FileSystem.ChDrive(CurrentDrive);
			DirectoryInfo diCurrent = new DirectoryInfo(CurrentPath);
			if (diCurrent.Exists)
				FileSystem.ChDir(CurrentPath);
            this.sfdTCStandard.AddExtension = true;
            this.sfdTCStandard.CheckFileExists = false;
            this.sfdTCStandard.CheckPathExists = true;
            this.sfdTCStandard.Title = "Save Image As";
            this.sfdTCStandard.InitialDirectory = (mTCBase.ImagePath != bpeNullString ? ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash) : mSupport.ApplicationPath);
			string filter = "All Picture Files|*.bmp;*.emf;*.exif;*.gif;*.ico;*.jpg;*.png;*.tiff;*.wmf|";
			filter += "Windows bitmap image (BMP) format (*.bmp)|*.bmp|";
			filter += "Enhanced Windows meta-file (EMF) image format (*.emf)|*.emf|";
			filter += "Exchangeable Image File (EXIF) format (*.exif)|*.exif|";
			filter += "Graphics Interchange Format (GIF) image format (*.gif)|*.gif|";
			filter += "Windows icon (ICO) image format (*.ico)|*.ico|";
			filter += "Joint Photographic Experts Group (JPEG) image format (*.jpg)|*.jpg|";
			filter += "W3C Portable Network Graphics (PNG) image format (*.png)|*.png|";
			filter += "Tag Image File Format (TIFF) image format (*.tiff)|*.tiff|";
			filter += "Windows meta-file (WMF) image format (*.wmf)|*.wmf";
            this.sfdTCStandard.Filter = filter;
            this.sfdTCStandard.FilterIndex = 7;
			if (this.sfdTCStandard.ShowDialog(this) == DialogResult.Cancel)
				return;
			FileInfo fi = new FileInfo(this.sfdTCStandard.FileName);

			string dataMember = ctl.DataBindings[0].BindingMemberInfo.BindingMember;
			byte[] arrPicture = (byte[])mTCBase.CurrentRow[dataMember];
			MemoryStream ms = new MemoryStream(arrPicture);
			Image tmpImage = Image.FromStream(ms);
			switch (fi.Extension.ToLower()) {
				case ".bmp":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Bmp);
					break;
				case ".emf":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Emf);
					break;
				case ".exif":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Exif);
					break;
				case ".gif":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Gif);
					break;
				case ".ico":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Icon);
					break;
				case ".jpg":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Jpeg);
					break;
				case ".png":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Png);
					break;
				case ".tiff":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Tiff);
					break;
				case ".wmf":
					tmpImage.Save(this.sfdTCStandard.FileName, ImageFormat.Wmf);
					break;
			}
			tmpImage = null;
			ms.Close();
			ms.Dispose();
		}
		protected void ImportImage(PictureBox ctl)
		{
			string ImagePath = (string)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, mTCBase.ActiveForm.Name), "ImagePath", mTCBase.ImagePath);
			string CurrentDrive = null;
			CurrentDrive = ParsePath(ImagePath, ParseParts.DrvOnly);
			FileSystem.ChDrive(CurrentDrive);
			DirectoryInfo diCurrent = new DirectoryInfo(ImagePath);
			if (diCurrent.Exists)
				FileSystem.ChDir(ImagePath);
            this.ofdTCStandard.AddExtension = true;
            this.ofdTCStandard.CheckFileExists = true;
            this.ofdTCStandard.CheckPathExists = true;
            this.ofdTCStandard.Multiselect = false;
            this.ofdTCStandard.Title = "Select Image";
            this.ofdTCStandard.InitialDirectory = (ImagePath != bpeNullString ? ParsePath(ImagePath, ParseParts.DrvDirNoSlash) : mSupport.ApplicationPath);
            this.ofdTCStandard.Filter = "All Picture Files|*.jpg;*.gif;|JPEG Images (*.jpg)|*.jpg|CompuServe GIF Images (*.gif)|*.gif|All Files (*.*)|*.*";
            this.ofdTCStandard.FilterIndex = 1;
			if (this.ofdTCStandard.ShowDialog(this) == DialogResult.Cancel)
				return;
			ImagePath = ParsePath(this.ofdTCStandard.FileName, ParseParts.DrvDir);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, mTCBase.ActiveForm.Name), "ImagePath", ImagePath);
			Image tmpImage = Image.FromFile(ofdTCStandard.FileName);
			MemoryStream ms = new MemoryStream();
			tmpImage.Save(ms, tmpImage.RawFormat);
			byte[] tmpBinary = ms.GetBuffer();
			mTCBase.CurrentRow["Image"] = tmpBinary;
			ms.Close();
			ms.Dispose();
			ctl.Image = tmpImage;
			tmpImage = null;
			this.OnImageImported(ofdTCStandard.FileName);
		}
		protected void PasteImage(PictureBox ctl)
		{
			object obj = null;
			Image img = null;
			//First get the image from the Clipboard...
			string[] fmtArray = Clipboard.GetDataObject().GetFormats();
			//For Each iFormat As String In fmtArray
			//    Debug.WriteLine(iFormat)
			//Next
			string myFormat = null;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap))
				myFormat = DataFormats.Bitmap;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.MetafilePict))
				myFormat = DataFormats.MetafilePict;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib))
				myFormat = DataFormats.Dib;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.Dif))
				myFormat = DataFormats.Dif;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.Tiff))
				myFormat = DataFormats.Tiff;
			if ((myFormat == null) && Clipboard.GetDataObject().GetDataPresent(DataFormats.EnhancedMetafile))
				myFormat = DataFormats.EnhancedMetafile;
			if ((myFormat != null)) {
				obj = Clipboard.GetDataObject().GetData(myFormat);
				if ((obj != null)) {
					if (object.ReferenceEquals(typeof(MemoryStream), obj.GetType())) {
						//Dim metaFile As New System.Drawing.Imaging.Metafile(CType(obj, MemoryStream))
						const int CF_ENHMETAFILE = 14;
						IntPtr henmetafile = default(IntPtr);
						System.Drawing.Imaging.Metafile metaFile = null;
						if (ClipboardAPI.OpenClipboard(this.Handle)) {
							if (ClipboardAPI.IsClipboardFormatAvailable(CF_ENHMETAFILE) != 0) {
								henmetafile = ClipboardAPI.GetClipboardData(CF_ENHMETAFILE);
								metaFile = new Metafile(henmetafile, true);
								ClipboardAPI.CloseClipboard();

								img = (Image)metaFile;
							}
						}
					} else {
						img = (Image)obj;
					}
				}
			}
			if (img == null)
				throw new Exception("Unable to identify image from clipboard (" + myFormat + ")");

			//Next take our bitmap as Image and save it as JPEG to a temporary file...
			string tempFile = Path.GetTempFileName();
			FileInfo fi = new FileInfo(tempFile);
			fi.Attributes = FileAttributes.Temporary;
			img.Save(tempFile, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Now read the JPG file from the temp file...
			Image tmpImage = Image.FromFile(tempFile);
			MemoryStream ms = new MemoryStream();
			tmpImage.Save(ms, tmpImage.RawFormat);
			byte[] tmpBinary = ms.GetBuffer();
			string dataMember = ctl.DataBindings[0].BindingMemberInfo.BindingMember;
			mTCBase.CurrentRow[dataMember] = tmpBinary;
			ctl.Image = tmpImage;
			tmpImage = null;
			ms.Close();
			ms.Dispose();
			//fi.Delete()
		}
		#endregion
		#endregion
		#region "Event Handlers"
		protected virtual void ActionModeChange(object sender, ActionModeChangeEventArgs e)
		{
			const string EntryName = "ActionModeChange";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				CurrencyManager cm = (CurrencyManager)this.BindingContext[mTCBase.MainDataView];
				base.Trace("{0}: oldMode:={1}; newMode:={2}; cmPos:={3}", new object[] {
					EntryName,
					e.oldMode.ToString(),
					e.newMode.ToString(),
					cm.Position
				}, trcOption.trcApplication);
				switch (e.newMode) {
					case ActionModeEnum.modeDisplay:
						//modeDelete is never used to trigger UI changes, so avoid making UI changes based on a change from modeDelete...
						if (e.oldMode != ActionModeEnum.modeDelete) {
							//EnableControlsByBinding(False, False)
							base.EnableControls(this.Controls, false, false);
							this.pbGeneral.ContextMenu = this.ctxImage;
							this.pbGeneral2.ContextMenu = this.ctxImage;
							this.EnableImageContextMenu(false);
							this.EnableRichTextContextMenu(false);

							this.EnableButtons(false);
							this.tcMain.Focus();
							this.sbpMode.Text = null;
							fOKtoUnload = true;
						}
						this.AcceptButton = this.btnExit;
						this.CancelButton = this.btnExit;
						break;
					case ActionModeEnum.modeAdd:
					case ActionModeEnum.modeCopy:
					case ActionModeEnum.modeModify:
						//EnableControlsByBinding(True, CBool(e.newMode = ActionModeEnum.modeAdd))
						base.EnableControls(this.Controls, true, Convert.ToBoolean(e.newMode == ActionModeEnum.modeAdd));
						this.pbGeneral.ContextMenu = this.ctxImage;
						this.pbGeneral2.ContextMenu = this.ctxImage;
						this.EnableImageContextMenu(true);
						this.EnableRichTextContextMenu(true);

						if (e.newMode != ActionModeEnum.modeModify) {
							foreach (Binding iBinding in mTCBase.CurrencyManager.Bindings) {
								if (iBinding.Control is DateTimePicker && mTCBase.TagContains(iBinding.Control, "Reset"))
									((DateTimePicker)iBinding.Control).Value = DateAndTime.Now;
							}
						}
						this.EnableButtons(true);
						this.tcMain.Focus();
						if (e.newMode == ActionModeEnum.modeAdd)
							this.sbpPosition.Text = "Adding";
						this.sbpMode.Text = "Edit Mode";
						fOKtoUnload = false;
						this.AcceptButton = this.btnOK;
						this.CancelButton = this.btnCancel;
						break;
					case ActionModeEnum.modeDelete:
						break;
					//modeDelete never opens screen fields, so there would never be anything to do here...
				}
				mMode = e.newMode;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		protected virtual void AfterMove(object sender, RowChangeEventArgs e)
		{
			const string EntryName = "AfterMove";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
				string message = "Adding";
				if (e.RowIndex != -1)
					message = string.Format("{0:#,##0} of {1:#,##0}", e.RowIndex + 1, e.TotalRows);
				Trace("{0}: {1}", EntryName, message, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
				this.sbpPosition.Text = message;
				//Me.pbGeneral.Location = New System.Drawing.Point(500, 20)   'Something keeps causing this to creep down the form, so re-anchor it at 500,20...
				//Me.pbGeneral2.Location = New System.Drawing.Point(500, 20)   'Something keeps causing this to creep down the form, so re-anchor it at 500,20...
				//Avoid annoyance that SelectedText in ComboBox controls is sometimes displayed even through the control is disabled...
				foreach (Control ctl in mTCBase.ActiveForm.Controls) {
					if (ctl is System.Windows.Forms.ComboBox) {
						System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)ctl;
						cb.SelectionLength = 0;
						cb.SelectionStart = 0;
					}
				}
				//Since our Arrow key handling seems to fail to prevent the TabControl from changing Tabs, let's counteract by reseting to the "General" tab here...
				this.tcMain.SelectedTab = this.tpGeneral;

				Trace("{0}: Setting BindingContext(DataView).Position to Row #{1}", EntryName, e.RowIndex.ToString(), trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
				this.BindingContext[mTCBase.MainDataView].Position = e.RowIndex;
				mTCBase.TraceDataView(true);
				BindControls();

				UnbindControl(pbGeneral);
				UnbindControl(pbGeneral2);
				mTCBase.RemoveImages(e.OldRowIndex);
				mTCBase.GetDeferredImages(e.RowIndex);
				if (mTCBase.MainDataView.Table.Columns.Contains("Image"))
					BindControl(pbGeneral, mTCBase.MainDataView, "Image");
				if (mTCBase.MainDataView.Table.Columns.Contains("OtherImage"))
					BindControl(pbGeneral2, mTCBase.MainDataView, "OtherImage");
				SetCaption(null, new CancelEventArgs(false));
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
			}
		}
		protected virtual void BeforeMove(object sender, RowChangeEventArgs e)
		{
			const string EntryName = "BeforeMove";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
				UnbindControls(this.Controls, false, false);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcDB);
			}
		}
		private void FilterCanceled(object sender, FilterChangeEventArgs e)
		{
			base.Trace("{0}:={1}", "newFilter", e.NewFilter, trcOption.trcApplication);
			this.sbpStatus.Text = "Filter Canceled";
			this.sbpFilter.Text = e.NewFilter;
		}
		private void FilterChange(object sender, FilterChangeEventArgs e)
		{
			string newFilter = ((e.NewFilter == null) ? bpeNullString : e.NewFilter);
			base.Trace("{0}:={1}", "newFilter", e.NewFilter, trcOption.trcApplication);
			this.sbpStatus.Text = string.Format("Filter {0}", (newFilter == bpeNullString ? "Off" : "On"));
			this.sbpFilter.Text = newFilter;
		}
		protected virtual void Tick(System.Object sender, System.EventArgs e)
		{
			try {
				sbpTime.Text = string.Format("{0:t}", DateAndTime.Now);
			} catch (Exception ex) {
			}
		}
		protected void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.ClearErrors();
				mTCBase.CancelCommand();
				mLastButtonClicked = "Cancel";
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void btnExit_Click(System.Object sender, System.EventArgs e)
		{
			try {
				mLastButtonClicked = "Exit";
				this.Close();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
				try {
					this.Close();
					return;
				} catch (Exception ex2) {
				}
			}
		}
		protected void btnFirst_Click(object sender, System.EventArgs e)
		{
			try {
				if (this.BindingContext[mTCBase.MainDataView].Position == 0) {
					Interaction.Beep();
				} else {
					mTCBase.MoveFirst();
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void btnLast_Click(object sender, System.EventArgs e)
		{
			try {
				if (this.BindingContext[mTCBase.MainDataView].Position == mTCBase.MainDataView.Count - 1) {
					Interaction.Beep();
				} else {
					mTCBase.MoveLast();
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void btnPrev_Click(object sender, System.EventArgs e)
		{
			try {
				if (this.BindingContext[mTCBase.MainDataView].Position == 0) {
					Interaction.Beep();
				} else {
					mTCBase.MovePrevious();
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void btnNext_Click(object sender, System.EventArgs e)
		{
			try {
				if (this.BindingContext[mTCBase.MainDataView].Position == mTCBase.MainDataView.Count - 1) {
					Interaction.Beep();
				} else {
					mTCBase.MoveNext();
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			try {
				mTCBase.OKCommand();
				mLastButtonClicked = "OK";
				this.ClearErrors();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void ctxImage_Popup(object sender, System.EventArgs e)
		{
			bool isDefault = false;
			PictureBox pb = null;
			byte[] tmpBinary = null;
            try
            {
                switch (mMode)
                {
                    case ActionModeEnum.modeDisplay:
                        break;
                    default:
                        pb = (PictureBox)((ContextMenu)sender).SourceControl;
                        if (pb == null)
                            throw new ExitTryException();
                        if (pb.DataBindings == null)
                            throw new ExitTryException();
                        string dataMember = pb.DataBindings[0].BindingMemberInfo.BindingMember;
                        if (dataMember == null)
                            throw new ExitTryException();
                        if (Information.IsDBNull(mTCBase.CurrentRow[dataMember]))
                        {
                            isDefault = true;
                        }
                        else
                        {
                            tmpBinary = (byte[])mTCBase.CurrentRow[dataMember];
                            if (tmpBinary == null)
                                throw new ExitTryException();
                            isDefault = tmpBinary.Equals(DefaultImage);
                        }
                        this.mnuImageClear.Visible = !isDefault;
                        this.mnuImageCopy.Visible = !isDefault;
                        this.mnuImageSaveAs.Visible = !isDefault;
                        break;
                }
            } catch (ExitTryException ex) {
			} finally {
				tmpBinary = null;
			}
		}
		protected virtual void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (DesignMode)
				return;
			try {
				if (!(fOKtoUnload && mLastButtonClicked != "Cancel")){e.Cancel = true;throw new ExitTryException();}
				mTCBase.UnloadCommand(this, mTCBase.MainDataView);

				//Remove handlers for frmStandard controls...
				RemoveControlHandlers(cbAlphaSort);
				RemoveControlHandlers(cbLocation);
				RemoveControlHandlers(dtpInventoried);
				RemoveControlHandlers(txtPrice);
				RemoveControlHandlers(dtpPurchased);
				RemoveControlHandlers(txtValue);
				RemoveControlHandlers(dtpVerified);
				RemoveControlHandlers(chkWishList);
				hsbGeneral.Scroll -= hsbGeneral_Scroll;

				UnbindControls(Controls, false, false);
				if (mCurrencyManager != null) {
					mCurrencyManager.CurrentChanged -= CurrencyManager_CurrentChanged;
					mCurrencyManager.ItemChanged -= CurrencyManager_ItemChanged;
					mCurrencyManager.PositionChanged -= CurrencyManager_PositionChanged;
					mCurrencyManager = null;
				}
				e.Cancel = false;
			} catch (ExitTryException ex) {
				throw;
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void Form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (DesignMode)
				return;
			try {
				e.Handled = false;
				switch (e.KeyCode) {
					case Keys.Left:
					case Keys.Right:
						if (mMode == ActionModeEnum.modeDisplay) {
							if (e.Shift && e.Control && !e.Alt) {
								switch (e.KeyCode) {
									case Keys.Left:
										btnFirst_Click(sender, new System.EventArgs());
										e.Handled = true;
										break;
									case Keys.Right:
										btnLast_Click(sender, new System.EventArgs());
										e.Handled = true;
										break;
								}
							} else if (e.Shift && !e.Control && !e.Alt) {
								switch (e.KeyCode) {
									case Keys.Left:
										btnPrev_Click(sender, new System.EventArgs());
										e.Handled = true;
										break;
									case Keys.Right:
										btnNext_Click(sender, new System.EventArgs());
										e.Handled = true;
										break;
								}
							}
						}
						break;
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected override void Form_Load(object sender, System.EventArgs e)
		{
            base.Form_Load(sender, e);
            if (DesignMode)
				return;
			mnuMain.MenuItems.Clear();
			mnuFile = mnuMain.MenuItems.Add("&File");
			mnuFileReport = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("&Report", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Report]), new EventHandler(this.mnuFileReport_Click), Shortcut.None))];
			mnuFileSQL = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("&SQL", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.SQL]), new EventHandler(mnuFileSQL_Click), Shortcut.None))];
			mnuFileTrace = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("&Trace", "Verdana", 10, (mSupport.Trace.TraceMode ? clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Trace]) : null), new EventHandler(this.mnuFileTrace_Click), Shortcut.None))];
			mnuFileTrace.Checked = mSupport.Trace.TraceMode;
			mnuFileSep = mnuFile.MenuItems[mnuFile.MenuItems.Add(new MenuItem("-"))];
			mnuFileExit = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("E&xit", "Verdana", 10, null, new EventHandler(mnuFileExit_Click), System.Windows.Forms.Shortcut.AltF4))];

			mnuRecords = mnuMain.MenuItems.Add("&Records");
			//mnuRecordsSelect = mnuRecords.MenuItems(mnuRecords.MenuItems.Add(New clsIconMenuItem("&Select", "Verdana", 10, ImageToIcon(Me.imgToolbar.Images.Item(imgToolBarEnum.Select)), New EventHandler(AddressOf Me.mnuRecordsSelect_Click), Shortcut.CtrlShiftS)))
			//mnuRecordsSep1 = mnuRecords.MenuItems(mnuRecords.MenuItems.Add(New MenuItem("-")))
			mnuRecordsNew = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&New", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.New]), new EventHandler(this.mnuRecordsNew_Click), Shortcut.CtrlShiftN))];
			mnuRecordsCopy = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&Copy/Append", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Copy]), new EventHandler(this.mnuRecordsCopy_Click), Shortcut.CtrlShiftI))];
			mnuRecordsModify = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&Modify", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Modify]), new EventHandler(this.mnuRecordsModify_Click), Shortcut.F2))];
			mnuRecordsDelete = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&Delete", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Delete]), new EventHandler(this.mnuRecordsDelete_Click), Shortcut.None))];
			//mnuRecordsSep2 = mnuRecords.MenuItems(mnuRecords.MenuItems.Add(New MenuItem("-")))
			//mnuRecordsRefresh = mnuRecords.MenuItems(mnuRecords.MenuItems.Add(New clsIconMenuItem("&Refresh", "Verdana", 10, ImageToIcon(Me.imgToolbar.Images.Item(imgToolBarEnum.Refresh)), New EventHandler(AddressOf Me.mnuRecordsRefresh_Click), Nothing)))
			mnuRecordsSep3 = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new MenuItem("-"))];
			mnuRecordsFilter = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&Filter", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Filter]), new EventHandler(this.mnuRecordsFilter_Click), Shortcut.CtrlShiftF))];
            mnuRecordsSort = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&Sort", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.Sort]), new EventHandler(this.mnuRecordsSort_Click), Shortcut.CtrlShiftS))];
            mnuRecordsList = mnuRecords.MenuItems[mnuRecords.MenuItems.Add(new clsIconMenuItem("&List", "Verdana", 10, clsTCBase.ImageToIcon(this.imgToolbar.Images[(int)imgToolBarEnum.List]), new EventHandler(this.mnuRecordsList_Click), Shortcut.F3))];

			tbMain.Buttons[(int)tbButtonEnum.Select].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.Sep1].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.Sep2].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.Refresh].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.Sep5].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.DumpDataTable].Visible = false;
			tbMain.Buttons[(int)tbButtonEnum.DumpDataView].Visible = false;

			//Add handlers for frmStandard controls...
			SetupControlHandlers(cbAlphaSort);
			SetupControlHandlers(cbLocation);
			SetupControlHandlers(dtpInventoried);
			TagSet(dtpInventoried, "Reset");
			SetupControlHandlers(txtPrice);
			SetupControlHandlers(dtpPurchased);
			TagSet(dtpPurchased, "Reset");
			SetupControlHandlers(txtValue);
			SetupControlHandlers(dtpVerified);

			SetupControlHandlers(chkWishList);
			hsbGeneral.Scroll += hsbGeneral_Scroll;

			foreach (StatusBarPanel iPanel in this.sbStatus.Panels) {
				iPanel.Text = null;
			}
			//Pick-up any Filter changes that may have transpired before we were fully loaded...
			FilterChange(this, new FilterChangeEventArgs(bpeNullString, mTCBase.SQLFilter));

			//Resize our rtfNotes to fit properly inside its TabPanel...
			this.rtfNotes.Size = new Size(this.tpNotes.Width - (2 * 4), this.tpNotes.Height - (2 * 4));
			this.pbGeneral.ContextMenu = this.ctxImage;
			this.pbGeneral2.ContextMenu = this.ctxImage;
			this.pbGeneral2.Location = new Point(this.pbGeneral.Location.X, this.pbGeneral.Location.Y);
			this.pbGeneral2.Size = new Size(this.pbGeneral.Size.Width, this.pbGeneral.Size.Height);
			this.hsbGeneral.Location = new Point(this.pbGeneral.Location.X, this.pbGeneral.Location.Y + this.pbGeneral.Size.Height);
			this.hsbGeneral.Size = new Size(this.pbGeneral.Size.Width, this.hsbGeneral.Size.Height);
			this.EnableImageContextMenu(false);
			this.EnableRichTextContextMenu(false);
			Trace("frmTCStandard.Form_Load: Me.tcMain.Size: {0}; Me.gbGeneral.Size: {1}; Me.sbStatus.Top({2}) - Me.btnExit.Bottom({3}) = {4};", new object[] {
				this.tcMain.Size.ToString(),
				this.gbGeneral.Size.ToString(),
				this.sbStatus.Top,
				(this.btnExit.Top + this.btnExit.Height),
				this.sbStatus.Top - (this.btnExit.Top + this.btnExit.Height)
			}, trcOption.trcApplication);
		}
		protected virtual void Form_Shown(object sender, System.EventArgs e)
		{
			if (DesignMode)
				return;
            try
            {
                if (fActivated)
                    throw new ExitTryException();
                fActivated = true;

                int iLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Left", this.Left);
                int iTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Top", this.Top);
                int iWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Width", this.Width);
                int iHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Height", this.Height);
				base.AdjustFormPlacement(ref iTop, ref iLeft); //Correct for errant form placement...
				this.SetBounds(iLeft, iTop, iWidth, iHeight);

                //Since SaveRegistrySetting isn't [yet] smart enough to create a missing parent key when creating sub-keys, 
                //we'll save our would-be parent key here even though it makes more sense to do so in the UnloadCommand...
                mTCBase.SaveBounds(mRegistryKey, iLeft, iTop, iWidth, iHeight);

                //Fix ComboBox controls in the active form (SelectionLength)...
                this.FixComboBoxes(mTCBase.ActiveForm);
                Trace("frmTCStandard.Form_Shown: Me.tcMain.Size: {0}; Me.gbGeneral.Size: {1}; Me.sbStatus.Top({2}) - Me.btnExit.Bottom({3}) = {4};", new object[] {
                    this.tcMain.Size.ToString(),
                    this.gbGeneral.Size.ToString(),
                    this.sbStatus.Top,
                    (this.btnExit.Top + this.btnExit.Height),
                    this.sbStatus.Top - (this.btnExit.Top + this.btnExit.Height)
                }, trcOption.trcApplication);
            } catch (ExitTryException ex) {
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void pbGeneral_DoubleClick(System.Object sender, System.EventArgs e)
		{
			frmImage frm = null;
			try {
				PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
				frm = new frmImage(mSupport, mTCBase, this, ctl.Image);
				frm.Icon = this.Icon;
				frm.Text = this.txtCaption.Text;
				frm.ShowDialog(this);
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				if (frm != null){frm.Dispose();frm = null;}
			}
		}
		protected void hsbGeneral_Scroll(System.Object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			try {
				switch (e.NewValue) {
					case 0:
						pbGeneral.BringToFront();
						pbGeneral.Enabled = true;
						pbGeneral2.Enabled = false;
						break;
					case 1:
						//Double-check the size and location of pbGeneral2... If different than pbGeneral, adjust as necessary...
						if (pbGeneral2.Location.X != pbGeneral.Location.X || pbGeneral2.Location.Y != pbGeneral.Location.Y)
							this.pbGeneral2.Location = new Point(this.pbGeneral.Location.X, this.pbGeneral.Location.Y);
						if (pbGeneral2.Size.Width != pbGeneral.Size.Width || pbGeneral2.Size.Height != pbGeneral.Size.Height)
							this.pbGeneral2.Size = new Size(this.pbGeneral.Size.Width, this.pbGeneral.Size.Height);

						pbGeneral2.BringToFront();
						pbGeneral.Enabled = false;
						pbGeneral2.Enabled = true;
						break;
				}
			} catch (Exception ex) {
				base.Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Exception");
			}
		}
		protected void tbMain_ButtonClick(System.Object sender, ToolBarButtonClickEventArgs e)
		{
			try {
				switch ((tbButtonEnum)tbMain.Buttons.IndexOf(e.Button)) {
					case tbButtonEnum.New:
						mnuRecordsNew_Click(sender, e);
						break;
					case tbButtonEnum.CopyAppend:
						mnuRecordsCopy_Click(sender, e);
						break;
					case tbButtonEnum.Modify:
						mnuRecordsModify_Click(sender, e);
						break;
					case tbButtonEnum.Delete:
						mnuRecordsDelete_Click(sender, e);
						break;
					case tbButtonEnum.Refresh:
						mnuRecordsRefresh_Click(sender, e);
						break;
                    case tbButtonEnum.Filter:
                        mnuRecordsFilter_Click(sender, e);
                        break;
                    case tbButtonEnum.Select:
						mnuRecordsSelect_Click(sender, e);
						break;
                    case tbButtonEnum.Sort:
                        mnuRecordsSort_Click(sender, e);
                        break;
					case tbButtonEnum.List:
						mnuRecordsList_Click(sender, e);
						break;
					case tbButtonEnum.Report:
						mnuFileReport_Click(sender, e);
						break;
					case tbButtonEnum.SQL:
						mnuFileSQL_Click(sender, e);
						break;
					case tbButtonEnum.DumpDataView:
						mTCBase.DumpDataView();
						break;
					case tbButtonEnum.DumpDataTable:
						mTCBase.DumpDataTable();
						break;
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		//Protected Sub tcMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tcMain.SelectedIndexChanged
		//    Try
		//        Select Case tcMain.SelectedTab.Text
		//            Case "Notes"
		//                'Dim Position As Integer = Me.BindingContext(mTCBase.MainDataView).Position
		//                'If Position >= 0 Then mTCBase.BindNotes(Me.rtfNotes, mTCBase.MainDataView.Item(Position)("ID"))
		//        End Select
		//    Catch ex As Exception
		//        mTCBase.GenericErrorHandler(ex, True)
		//    End Try
		//End Sub
		//Protected Friend Sub Control_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtPrice.KeyDown
		//    'No tracing on purpose (performance)...
		//    mKeyEventArgs = e
		//End Sub
		//Protected Friend Sub Control_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtPrice.KeyPress
		//    'No tracing on purpose (performance)...
		//    Try
		//        MyBase.epBase.SetError((Control)sender, bpeNullString)
		//        'Note: The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events.
		//        Select Case sender.GetType.Name
		//            Case "TextBox"
		//                Dim control As TextBox = CType(sender, TextBox)
		//                If CType(control.Tag, String).ToUpper.IndexOf("MONEY") >= 0 Then
		//                    If Not (mKeyEventArgs.Control And mKeyEventArgs.KeyCode = Keys.Z) Then  '(i.e. Not attempting Undo)
		//                        Select Case mKeyEventArgs.KeyCode
		//                            Case Keys.D0 To Keys.D9
		//                            Case Keys.NumPad0 To Keys.NumPad9
		//                            Case Keys.Home, Keys.End, Keys.Left, Keys.Right 'Directional and arrow keys
		//                            Case Keys.Back  'Backspace
		//                            Case Keys.Delete
		//                            Case Keys.Decimal, Keys.OemPeriod 'KeyPad-Decimal, Keyboard-Period (respectively)
		//                                If CType(sender, TextBox).Text.IndexOf(".") >= 0 Then Throw New Exception("Invalid value entered.")
		//                            Case Keys.Oemcomma  'Keyboard-Comma
		//                                If CType(sender, TextBox).Text.IndexOf(".") >= 0 Then Throw New Exception("Invalid value entered.")
		//                            Case Else
		//                                'Use the following initialization to trigger the conversion exception (if any)
		//                                Dim strPrice As String = CType(txtPrice.Text, Decimal).ToString("c")
		//                        End Select
		//                    End If
		//                End If
		//        End Select
		//    Catch ex As Exception
		//        MyBase.epBase.SetError((Control)sender, ex.Message)
		//        e.Handled = True
		//    End Try
		//End Sub
		//Protected Friend Sub Control_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles dtpInventoried.KeyUp, dtpPurchased.KeyUp
		//    'No tracing on purpose (performance)...
		//    mKeyEventArgs = e
		//    Try
		//        MyBase.epBase.SetError((Control)sender, bpeNullString)
		//        Select Case sender.GetType.Name
		//            Case "DateTimePicker"
		//                Dim dtp As DateTimePicker = CType(sender, DateTimePicker)
		//                Select Case mKeyEventArgs.KeyCode
		//                    Case Keys.F5
		//                        dtp.Value = Now
		//                        e.Handled = True
		//                End Select
		//        End Select
		//    Catch ex As Exception
		//        MyBase.epBase.SetError((Control)sender, ex.Message)
		//        e.Handled = True
		//    End Try
		//End Sub
		//Protected Friend Sub AutoComplete_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
		//    'No tracing on purpose (performance)...
		//    Dim control As Windows.Forms.ComboBox = CType(sender, Windows.Forms.ComboBox)
		//    Select Case e.KeyCode
		//        Case Keys.End, Keys.Home, Keys.Up, Keys.Down, Keys.PageUp, Keys.PageDown
		//            e.Handled = False : Exit Sub
		//        Case Keys.Left
		//            If control.SelectionStart > 0 Then
		//                control.SelectionStart -= 1
		//                e.Handled = True
		//            Else
		//                e.Handled = False
		//            End If
		//            Exit Sub
		//        Case Keys.Right
		//            If control.SelectionStart < control.SelectionLength Then
		//                control.SelectionStart += 1
		//                e.Handled = True
		//            Else
		//                e.Handled = False
		//            End If
		//            Exit Sub
		//        Case Else
		//            If e.Control Then e.Handled = False : Exit Sub
		//    End Select
		//End Sub
		//Protected Friend Sub AutoComplete_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) 'Handles cbAlphaSort.KeyPress, cbLocation.KeyPress
		//    'No tracing on purpose (performance)...
		//    Dim control As Windows.Forms.ComboBox = CType(sender, Windows.Forms.ComboBox)
		//    Dim searchText As String
		//    With control
		//        Select Case Asc(e.KeyChar)
		//            Case Keys.Escape
		//                .SelectedIndex = -1
		//                .Text = ""
		//                e.Handled = True
		//                Exit Sub
		//            Case Keys.Back
		//                If .SelectionStart <= 1 Then .Text = "" : Exit Sub
		//                searchText = .Text.Substring(0, IIf(.SelectionLength = 0, .Text.Length, .SelectionStart) - 1)
		//            Case Else
		//                If Char.IsControl(e.KeyChar) Then e.Handled = False : Exit Sub
		//                searchText = IIf(.SelectionLength = 0, .Text, .Text.Substring(0, .SelectionStart)) & Char.ToUpper(e.KeyChar)
		//        End Select

		//        Dim MatchIndex As Integer = .FindString(searchText)
		//        If MatchIndex <> -1 Then
		//            .SelectedText = ""
		//            .SelectedIndex = MatchIndex
		//            .SelectionStart = searchText.Length
		//            .SelectionLength = .Text.Length
		//            e.Handled = True
		//        ElseIf Not Char.IsLetter(e.KeyChar) Then
		//            'TODO: AutoComplete: Fix Force Upper-Case logic...
		//            e.Handled = False
		//            'ElseIf Not IsNothing(.Tag) AndAlso CType(.Tag, String).ToUpper.IndexOf("UPPER") >= 0 Then
		//            'Dim saveText As String = .Text
		//            'Dim selectionStart As Integer = .SelectionStart
		//            'Dim selectionLength As Integer = .SelectionLength
		//            'cb.SelectedIndex = -1
		//            'Mid(saveText, selectionStart, selectionLength) = Char.ToUpper(e.KeyChar)
		//            '.Text = saveText
		//            '.SelectionStart = selectionStart + 1
		//            '.SelectionLength = 0
		//            'e.Handled = True
		//        Else
		//            e.Handled = False
		//        End If
		//    End With
		//End Sub
		//#Region "WhereAmI"
		//    Protected Friend Sub FindFocus(ByVal control As Control)
		//        Const EntryName As String = "FindFocus"
		//        Try
		//            If control.GetType() Is GetType(Form) Then Me.sbpMessage.Text = bpeNullString
		//            'Debug.WriteLine(String.Format("{0}Checking {1}...", vbTab, control.Name))
		//            If control.Focused Then Me.sbpMessage.Text = String.Format("{0} has Focus", control.Name)
		//            If control.HasChildren Then
		//                For Each iControl As Control In control.Controls
		//                    FindFocus(iControl)
		//                Next
		//            End If
		//        Catch ex As Exception
		//            Debug.WriteLine(ex.Message)
		//        End Try
		//    End Sub
		//    Private Sub Control_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Enter, btnExit.Enter, btnFirst.Enter, btnLast.Enter, btnNext.Enter, btnOK.Enter, btnPrev.Enter, cbAlphaSort.Enter, cbLocation.Enter, chkWishList.Enter, dtpInventoried.Enter, dtpPurchased.Enter, gbGeneral.Enter, pbGeneral.Enter, lblAlphaSort.Enter, lblID.Enter, lblInventoried.Enter, lblLocation.Enter, lblPrice.Enter, lblPurchased.Enter, rtfNotes.Enter, sbStatus.Enter, txtAlphaSort.Enter, txtCaption.Enter, txtID.Enter, txtPrice.Enter, tcMain.Enter, tbMain.Enter
		//        Debug.WriteLine(String.Format("{0}:={1}.Enter()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Leave, btnExit.Leave, btnFirst.Leave, btnLast.Leave, btnNext.Leave, btnOK.Leave, btnPrev.Leave, cbAlphaSort.Leave, cbLocation.Leave, chkWishList.Leave, dtpInventoried.Leave, dtpPurchased.Leave, gbGeneral.Leave, pbGeneral.Leave, lblAlphaSort.Leave, lblID.Leave, lblInventoried.Leave, lblLocation.Leave, lblPrice.Leave, lblPurchased.Leave, rtfNotes.Leave, sbStatus.Leave, txtAlphaSort.Leave, txtCaption.Leave, txtID.Leave, txtPrice.Leave, tcMain.Leave, tbMain.Leave
		//        Debug.WriteLine(String.Format("{0}:={1}.Leave()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.GotFocus, btnExit.GotFocus, btnFirst.GotFocus, btnLast.GotFocus, btnNext.GotFocus, btnOK.GotFocus, btnPrev.GotFocus, cbAlphaSort.GotFocus, cbLocation.GotFocus, chkWishList.GotFocus, dtpInventoried.GotFocus, dtpPurchased.GotFocus, gbGeneral.GotFocus, pbGeneral.GotFocus, lblAlphaSort.GotFocus, lblID.GotFocus, lblInventoried.GotFocus, lblLocation.GotFocus, lblPrice.GotFocus, lblPurchased.GotFocus, rtfNotes.GotFocus, sbStatus.GotFocus, txtAlphaSort.GotFocus, txtCaption.GotFocus, txtID.GotFocus, txtPrice.GotFocus, tcMain.GotFocus, tbMain.GotFocus
		//        Debug.WriteLine(String.Format("{0}:={1}.GotFocus()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.LostFocus, btnExit.LostFocus, btnFirst.LostFocus, btnLast.LostFocus, btnNext.LostFocus, btnOK.LostFocus, btnPrev.LostFocus, cbAlphaSort.LostFocus, cbLocation.LostFocus, chkWishList.LostFocus, dtpInventoried.LostFocus, dtpPurchased.LostFocus, gbGeneral.LostFocus, pbGeneral.LostFocus, lblAlphaSort.LostFocus, lblID.LostFocus, lblInventoried.LostFocus, lblLocation.LostFocus, lblPrice.LostFocus, lblPurchased.LostFocus, rtfNotes.LostFocus, sbStatus.LostFocus, txtAlphaSort.LostFocus, txtCaption.LostFocus, txtID.LostFocus, txtPrice.LostFocus, tcMain.LostFocus, tbMain.LostFocus
		//        Debug.WriteLine(String.Format("{0}:={1}.LostFocus()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Validating(ByVal sender As Object, ByVal e As CancelEventArgs) Handles btnCancel.Validating, btnExit.Validating, btnFirst.Validating, btnLast.Validating, btnNext.Validating, btnOK.Validating, btnPrev.Validating, cbAlphaSort.Validating, cbLocation.Validating, chkWishList.Validating, dtpInventoried.Validating, dtpPurchased.Validating, gbGeneral.Validating, pbGeneral.Validating, lblAlphaSort.Validating, lblID.Validating, lblInventoried.Validating, lblLocation.Validating, lblPrice.Validating, lblPurchased.Validating, rtfNotes.Validating, sbStatus.Validating, txtAlphaSort.Validating, txtCaption.Validating, txtID.Validating, txtPrice.Validating, tcMain.Validating, tbMain.Validating
		//        Debug.WriteLine(String.Format("{0}:={1}.Validating()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Validated, btnExit.Validated, btnFirst.Validated, btnLast.Validated, btnNext.Validated, btnOK.Validated, btnPrev.Validated, cbAlphaSort.Validated, cbLocation.Validated, chkWishList.Validated, dtpInventoried.Validated, dtpPurchased.Validated, gbGeneral.Validated, pbGeneral.Validated, lblAlphaSort.Validated, lblID.Validated, lblInventoried.Validated, lblLocation.Validated, lblPrice.Validated, lblPurchased.Validated, rtfNotes.Validated, sbStatus.Validated, txtAlphaSort.Validated, txtCaption.Validated, txtID.Validated, txtPrice.Validated, tcMain.Validated, tbMain.Validated
		//        Debug.WriteLine(String.Format("{0}:={1}.Validated()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//#End Region
		#region "Menu Handlers"
		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			try {
				this.Close();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void mnuFileReport_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ReportCommand(mReportPath);
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void mnuFileSQL_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.SQLCommand(TCBase.TableName);
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected override void mnuFileTrace_Click(object sender, System.EventArgs e)
		{
			var _with3 = (MenuItem)sender;
			base.mnuFileTrace_Click(sender, e);
			this.sbpTrace.Text = base.TraceText;
			this.sbpTrace.Width = base.TraceWidth;
		}
		protected void mnuImageClear_Click(object sender, System.EventArgs e)
		{
			PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
			try {
				base.epBase.SetError(ctl, bpeNullString);
				ClearImage(ctl);
			} catch (Exception ex) {
				base.epBase.SetError(ctl, ex.Message);
			}
		}
		protected void mnuImageCopy_Click(object sender, System.EventArgs e)
		{
			PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
			try {
				base.epBase.SetError(ctl, bpeNullString);
				CopyImage(ctl);
			} catch (Exception ex) {
				base.epBase.SetError(ctl, ex.Message);
			}
		}
		protected void mnuImageImport_Click(object sender, System.EventArgs e)
		{
			PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
			try {
				base.epBase.SetError(ctl, bpeNullString);
				ImportImage(ctl);
			} catch (Exception ex) {
				base.epBase.SetError(ctl, ex.Message);
			}
		}
		protected void mnuImagePaste_Click(object sender, System.EventArgs e)
		{
			PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
			Image img = null;
			try {
				base.epBase.SetError(ctl, bpeNullString);
				PasteImage(ctl);
			} catch (Exception ex) {
				base.epBase.SetError(ctl, ex.Message);
			}
		}
		protected void mnuImageSaveAs_Click(object sender, System.EventArgs e)
		{
			PictureBox ctl = (this.hsbGeneral.Value == 0 ? this.pbGeneral : this.pbGeneral2);
			try {
				base.epBase.SetError(ctl, bpeNullString);
				ExportImage(ctl);
			} catch (Exception ex) {
				base.epBase.SetError(ctl, ex.Message);
			}
		}
		private void mnuRecordsCopy_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsCopy_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				TagRemove(dtpVerified, "Reset");
				base.UnbindControls(this.Controls, false, true);
				mTCBase.CopyCommand();
				//Note: Resulting Move command will re-bind controls
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsDelete_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsDelete_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.DeleteCommand();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsFilter_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsFilter_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.FilterCommand();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsList_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsList_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.ListCommand(false);
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsModify_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsModify_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.ModifyCommand();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsNew_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsNew_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				TagSet(dtpVerified, "Reset");
				base.UnbindControls(this.Controls, false, true);
				mTCBase.NewCommand();
				//Note: Resulting Move command will re-bind controls
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsRefresh_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsRefresh_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.RefreshCommand();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		private void mnuRecordsSelect_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuRecordsSelect_Click";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mTCBase.SelectCommand();
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
        private void mnuRecordsSort_Click(object sender, System.EventArgs e)
        {
            const string EntryName = "mnuRecordsSort_Click";
            try
            {
                Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
                mTCBase.SortCommand();
            }
            catch (Exception ex)
            {
                mTCBase.GenericErrorHandler(ex, true);
            }
            finally
            {
                Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
            }
        }
        #endregion
        #region "Debugging Tools"
        private void cbBindingContextChanged(object sender, EventArgs e)
		{
			Trace("{0}BindingContextChanged: BindingContext:={1}", new object[] {
				((ComboBox)sender).Name,
				((ComboBox)sender).BindingContext
			}, trcOption.trcApplication);
		}
		private void cbSelectedIndexChanged(object sender, EventArgs e)
		{
			Trace("{0}SelectedIndexChanged: SelectedIndex:={1}", new object[] {
				((ComboBox)sender).Name,
				((ComboBox)sender).SelectedIndex
			}, trcOption.trcApplication);
		}
		private void cbSelectedValueChanged(object sender, EventArgs e)
		{
			Trace("{0}SelectedValueChanged: SelectedValue:=\"{1}\"", new object[] {
				((ComboBox)sender).Name,
				((ComboBox)sender).SelectedValue
			}, trcOption.trcApplication);
		}
		private void cbTextChanged(object sender, EventArgs e)
		{
			Trace("{0}TextChanged: Text:=\"{1}\"", new object[] {
				((ComboBox)sender).Name,
				((ComboBox)sender).Text
			}, trcOption.trcApplication);
		}
		protected void Form_BindingContextChanged(object sender, System.EventArgs e)
		{
			string[] strArgs = {
				"frmTCStandard",
				"BindingContextChanged",
				"Nothing",
				"Nothing"
			};
			if ((sender != null))
				strArgs[2] = "{" + sender.GetType().ToString() + "}";
			if ((e != null))
				strArgs[3] = "{" + e.GetType().ToString() + "}";
			base.Trace("{0}.{1}(sender:={2}, e:={3})", strArgs, trcOption.trcApplication);
		}
		protected void Form_Enter(object sender, System.EventArgs e)
		{
			string[] strArgs = {
				"frmTCStandard",
				"Enter",
				"Nothing",
				"Nothing"
			};
			if ((sender != null))
				strArgs[2] = "{" + sender.GetType().ToString() + "}";
			if ((e != null))
				strArgs[3] = "{" + e.GetType().ToString() + "}";
			base.Trace("{0}.{1}(sender:={2}, e:={3})", strArgs, trcOption.trcApplication);
		}
		protected void Form_Leave(object sender, System.EventArgs e)
		{
			string[] strArgs = {
				"frmTCStandard",
				"Leave",
				"Nothing",
				"Nothing"
			};
			if ((sender != null))
				strArgs[2] = "{" + sender.GetType().ToString() + "}";
			if ((e != null))
				strArgs[3] = "{" + e.GetType().ToString() + "}";
			base.Trace("{0}.{1}(sender:={2}, e:={3})", strArgs, trcOption.trcApplication);
		}
		protected void gbGeneral_ClientSizeChanged(object sender, EventArgs e)
		{
			Trace("gbGeneral_ClientSizeChanged: {0}", new object[] { this.gbGeneral.ClientSize }, trcOption.trcApplication);
		}
		protected void gbGeneral_SizeChanged(object sender, EventArgs e)
		{
			Trace("gbGeneral_SizeChanged: {0}", new object[] { this.gbGeneral.Size }, trcOption.trcApplication);
		}
		#endregion
		#endregion
	}
}
namespace TCBase
{
	#region "Helper Classes"
	public class ClipboardAPI
	{
		[DllImport("user32.dll", EntryPoint = "OpenClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool OpenClipboard(IntPtr hWnd);
		[DllImport("user32.dll", EntryPoint = "EmptyClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool EmptyClipboard();
		[DllImport("user32.dll", EntryPoint = "SetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr SetClipboardData(int uFormat, IntPtr ByValhWnd);
		[DllImport("user32.dll", EntryPoint = "CloseClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool CloseClipboard();
		[DllImport("user32.dll", EntryPoint = "GetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr GetClipboardData(int uFormat);
		[DllImport("user32.dll", EntryPoint = "IsClipboardFormatAvailable", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern short IsClipboardFormatAvailable(int uFormat);
	}
}
namespace TCBase
{
	public class ImageImportedEventArgs : EventArgs
	{
        #region "Declarations"
        private string mImagePath = clsSupport.bpeNullString;
        private int mHeight = 0;
        private int mWidth = 0;
        private Image mImage = null;
        private List<string> mHeaders = new List<string>();
        Dictionary<string, object> mProperties = new Dictionary<string, object>();
        #endregion
        public ImageImportedEventArgs(string ImagePath) : base()
		{
            mImage = Image.FromFile(ImagePath);
			mImagePath = ImagePath;
            mHeight = mImage.Height;
            mWidth = mImage.Width;

            int MaxProperties = 0;
			FileInfo fi = new FileInfo(ImagePath);
			Shell32.Folder folder = GetShell32Folder(fi.DirectoryName);
			//First capture the available properties from the folder...
            for (int iProperty = 0; iProperty < short.MaxValue; iProperty++)
            {
                string PropName = folder.GetDetailsOf(null, iProperty);
                if (String.IsNullOrEmpty(PropName))
                    break;
                mHeaders.Add(PropName);
            }
			//Now fill in the values of those properties from our image file itself...
			Shell32.FolderItem folderItem = folder.ParseName(fi.Name);
            for (int iProperty = 0; iProperty < mHeaders.Count; iProperty++)
            {
                var propertyName = mHeaders[iProperty];
                var propertyValue = folder.GetDetailsOf(folderItem, iProperty);
                var propertyIndex = iProperty;
                if (!String.IsNullOrEmpty(propertyValue) && !mProperties.ContainsKey(propertyName))
                    mProperties.Add(propertyName, propertyValue);
            }
#if UseShell32
#else
#endif


		}
        #region "Properties"
        public int Height
        {
            get { return mHeight; }
        }
        public Image Image {
            get { return mImage; }
        }
		public string ImagePath {
			get { return mImagePath; }
		}
        public int Width
        {
            get { return mWidth; }
        }
        public Dictionary<string, object> Properties {
			get { return mProperties; }
		}
        #endregion
        #region "Methods"
        public object GetProperty(string PropertyName)
        {
            if (!mProperties.ContainsKey(PropertyName)) return null;
            return mProperties[PropertyName];
        }
        private Shell32.Folder GetShell32Folder(string folderPath)
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            return (Shell32.Folder)shellAppType.InvokeMember("NameSpace",
            System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { folderPath });
        }
        #endregion
    }
}
#endregion
