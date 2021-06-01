//frmTCBase.cs
//   Base Form for TreasureChest2 Project...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   05/12/21    Updated BindControl for binding ComboBox controls to underlying DataView to be fault-tolerant, meaning such
//               conditions are now ignored if the binding is already set to the requested DataSource/DataMember, otherwise the
//               binding is removed and reattempted;
//   07/27/19    Added logic to adjust form placement to account for preferences from a device with a size larger than the 
//               current device so this form would always displays on the current viewport;
//   07/08/19    Moved ttBase (ToolTip) from frmTCStandard into base class;
//   09/01/18    Implemented ResizeControl functionality;
//   04/21/18    Added DataGridView Support;
//   04/10/18    Finally implemented context menu for RichTextBox controls missing since upgrading to VB.NET;
//   04/09/18    Updated logic to only consider ComboBox.SelectionStart/Length if DropDownStyle <> ComboBoxStyle.DropDownList as
//               these properties are undefined when DropDownList (no underlying TextBox);
//   02/03/18    Restored RichTextBox properties in EnableControl;
//   12/22/17    Suppressed validation errors on DateTimePicker controls during Display and Delete operations;
//   12/19/17    Reworked BindControl and UnbindControl methods (especially to remove handlers added during binding)in an effort to 
//               eliminate weird binding behavior most notably in DateTimePicker controls when adding or copying new records to the 
//               end of the list;
//               Improved Tracing;
//   08/08/17    Added dtpValueChanged to troubleshoot and ultimately addressed issue with DateTimePicker controls seemingly 
//               validating before the year component of the date is evaluated (and binding to the underlying data at that time);
//   07/30/17    Added log messages to NullToDate and DateToNull;
//   06/19/17    Added ClearErrors methods;
//   06/04/17    Moved DefaultTextBox from various forms down into base-class;
//   10/06/16    Attempting to track down issues with empty ComboBox Text on data-entry;
//   09/18/16    Reworked architecture to eliminate references (i.e. controls, bindings, CurrencyManager) from clsTCBase and moved
//               them here in an effort to release memory otherwise held forever;
//   10/25/09    Created in VB.NET;
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
	public class frmTCBase : frmSupportBase
	{
		public frmTCBase() : base()
		{
			mSupport = new clsSupport();
			mMyModuleName = "frmTCBase";
			mMyParent = null;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, mSupport.ExecutingComponentName, mMyModuleName);
            mDPI = CreateGraphics().DpiX;

            //This call is required by the Windows Form Designer.
            InitializeComponent();
		}
		public frmTCBase(clsSupport objSupport, string ModuleName, Form objParent = null) : base()
		{
			mSupport = objSupport;
			mMyModuleName = ModuleName;
			mMyParent = objParent;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
            mDPI = CreateGraphics().DpiX;

            //This call is required by the Windows Form Designer.
            InitializeComponent();
		}
		public frmTCBase(clsSupport objSupport, string ModuleName, clsTCBase objTCBase, Form objParent = null) : base()
		{
			mSupport = objSupport;
			mTCBase = objTCBase;
			mMyModuleName = ModuleName;
			mMyParent = objParent;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
            mDPI = CreateGraphics().DpiX;

            Load += Form_Load;
            Layout += Form_Layout;
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

		protected ImageList imgBase;
		protected internal ToolTip ttBase;
		private ContextMenu withEventsField_ctxRTF;
		protected ContextMenu ctxRTF {
			get { return withEventsField_ctxRTF; }
			set {
				if (withEventsField_ctxRTF != null) {
					withEventsField_ctxRTF.Popup -= ctxRTF_Popup;
				}
				withEventsField_ctxRTF = value;
				if (withEventsField_ctxRTF != null) {
					withEventsField_ctxRTF.Popup += ctxRTF_Popup;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuCut;
		internal MenuItem mnuRTFContextMenuCut {
			get { return withEventsField_mnuRTFContextMenuCut; }
			set {
				if (withEventsField_mnuRTFContextMenuCut != null) {
					withEventsField_mnuRTFContextMenuCut.Click -= mnuRTFContextMenuCut_Click;
				}
				withEventsField_mnuRTFContextMenuCut = value;
				if (withEventsField_mnuRTFContextMenuCut != null) {
					withEventsField_mnuRTFContextMenuCut.Click += mnuRTFContextMenuCut_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuCopy;
		internal MenuItem mnuRTFContextMenuCopy {
			get { return withEventsField_mnuRTFContextMenuCopy; }
			set {
				if (withEventsField_mnuRTFContextMenuCopy != null) {
					withEventsField_mnuRTFContextMenuCopy.Click -= mnuRTFContextMenuCopy_Click;
				}
				withEventsField_mnuRTFContextMenuCopy = value;
				if (withEventsField_mnuRTFContextMenuCopy != null) {
					withEventsField_mnuRTFContextMenuCopy.Click += mnuRTFContextMenuCopy_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuPaste;
		internal MenuItem mnuRTFContextMenuPaste {
			get { return withEventsField_mnuRTFContextMenuPaste; }
			set {
				if (withEventsField_mnuRTFContextMenuPaste != null) {
					withEventsField_mnuRTFContextMenuPaste.Click -= mnuRTFContextMenuPaste_Click;
				}
				withEventsField_mnuRTFContextMenuPaste = value;
				if (withEventsField_mnuRTFContextMenuPaste != null) {
					withEventsField_mnuRTFContextMenuPaste.Click += mnuRTFContextMenuPaste_Click;
				}
			}
		}
		internal MenuItem mnuRTFContextMenuSep1;
		private MenuItem withEventsField_mnuRTFContextMenuSelectAll;
		internal MenuItem mnuRTFContextMenuSelectAll {
			get { return withEventsField_mnuRTFContextMenuSelectAll; }
			set {
				if (withEventsField_mnuRTFContextMenuSelectAll != null) {
					withEventsField_mnuRTFContextMenuSelectAll.Click -= mnuRTFContextMenuSelectAll_Click;
				}
				withEventsField_mnuRTFContextMenuSelectAll = value;
				if (withEventsField_mnuRTFContextMenuSelectAll != null) {
					withEventsField_mnuRTFContextMenuSelectAll.Click += mnuRTFContextMenuSelectAll_Click;
				}
			}
		}
		internal MenuItem mnuRTFContextMenuSep2;
		internal MenuItem mnuRTFContextMenuFont;
		private MenuItem withEventsField_mnuRTFContextMenuFontChange;
		internal MenuItem mnuRTFContextMenuFontChange {
			get { return withEventsField_mnuRTFContextMenuFontChange; }
			set {
				if (withEventsField_mnuRTFContextMenuFontChange != null) {
					withEventsField_mnuRTFContextMenuFontChange.Click -= mnuRTFContextMenuFontChange_Click;
				}
				withEventsField_mnuRTFContextMenuFontChange = value;
				if (withEventsField_mnuRTFContextMenuFontChange != null) {
					withEventsField_mnuRTFContextMenuFontChange.Click += mnuRTFContextMenuFontChange_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuFontDefault;
		internal MenuItem mnuRTFContextMenuFontDefault {
			get { return withEventsField_mnuRTFContextMenuFontDefault; }
			set {
				if (withEventsField_mnuRTFContextMenuFontDefault != null) {
					withEventsField_mnuRTFContextMenuFontDefault.Click -= mnuRTFContextMenuFontDefault_Click;
				}
				withEventsField_mnuRTFContextMenuFontDefault = value;
				if (withEventsField_mnuRTFContextMenuFontDefault != null) {
					withEventsField_mnuRTFContextMenuFontDefault.Click += mnuRTFContextMenuFontDefault_Click;
				}
			}
		}
		internal MenuItem mnuRTFContextMenuParagraph;
		internal MenuItem mnuRTFContextMenuParagraphAlign;
		private MenuItem withEventsField_mnuRTFContextMenuParagraphAlignLeft;
		internal MenuItem mnuRTFContextMenuParagraphAlignLeft {
			get { return withEventsField_mnuRTFContextMenuParagraphAlignLeft; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphAlignLeft != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignLeft.Click -= mnuRTFContextMenuParagraphAlignLeft_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphAlignLeft = value;
				if (withEventsField_mnuRTFContextMenuParagraphAlignLeft != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignLeft.Click += mnuRTFContextMenuParagraphAlignLeft_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuParagraphAlignCenter;
		internal MenuItem mnuRTFContextMenuParagraphAlignCenter {
			get { return withEventsField_mnuRTFContextMenuParagraphAlignCenter; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphAlignCenter != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignCenter.Click -= mnuRTFContextMenuParagraphAlignCenter_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphAlignCenter = value;
				if (withEventsField_mnuRTFContextMenuParagraphAlignCenter != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignCenter.Click += mnuRTFContextMenuParagraphAlignCenter_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuParagraphAlignRight;
		internal MenuItem mnuRTFContextMenuParagraphAlignRight {
			get { return withEventsField_mnuRTFContextMenuParagraphAlignRight; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphAlignRight != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignRight.Click -= mnuRTFContextMenuParagraphAlignRight_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphAlignRight = value;
				if (withEventsField_mnuRTFContextMenuParagraphAlignRight != null) {
					withEventsField_mnuRTFContextMenuParagraphAlignRight.Click += mnuRTFContextMenuParagraphAlignRight_Click;
				}
			}
		}
		internal MenuItem mnuRTFContextMenuParagraphSep1;
		private MenuItem withEventsField_mnuRTFContextMenuParagraphBullet;
		internal MenuItem mnuRTFContextMenuParagraphBullet {
			get { return withEventsField_mnuRTFContextMenuParagraphBullet; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphBullet != null) {
					withEventsField_mnuRTFContextMenuParagraphBullet.Click -= mnuRTFContextMenuParagraphBullet_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphBullet = value;
				if (withEventsField_mnuRTFContextMenuParagraphBullet != null) {
					withEventsField_mnuRTFContextMenuParagraphBullet.Click += mnuRTFContextMenuParagraphBullet_Click;
				}
			}
		}
		internal MenuItem mnuRTFContextMenuParagraphSep2;
		private MenuItem withEventsField_mnuRTFContextMenuParagraphIndent;
		internal MenuItem mnuRTFContextMenuParagraphIndent {
			get { return withEventsField_mnuRTFContextMenuParagraphIndent; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphIndent != null) {
					withEventsField_mnuRTFContextMenuParagraphIndent.Click -= mnuRTFContextMenuParagraphIndent_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphIndent = value;
				if (withEventsField_mnuRTFContextMenuParagraphIndent != null) {
					withEventsField_mnuRTFContextMenuParagraphIndent.Click += mnuRTFContextMenuParagraphIndent_Click;
				}
			}
		}
		private MenuItem withEventsField_mnuRTFContextMenuParagraphUnindent;
		internal MenuItem mnuRTFContextMenuParagraphUnindent {
			get { return withEventsField_mnuRTFContextMenuParagraphUnindent; }
			set {
				if (withEventsField_mnuRTFContextMenuParagraphUnindent != null) {
					withEventsField_mnuRTFContextMenuParagraphUnindent.Click -= mnuRTFContextMenuParagraphUnindent_Click;
				}
				withEventsField_mnuRTFContextMenuParagraphUnindent = value;
				if (withEventsField_mnuRTFContextMenuParagraphUnindent != null) {
					withEventsField_mnuRTFContextMenuParagraphUnindent.Click += mnuRTFContextMenuParagraphUnindent_Click;
				}
			}
		}

		internal FontDialog dlgFont;
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTCBase));
			this.imgBase = new System.Windows.Forms.ImageList(this.components);
			this.ctxRTF = new System.Windows.Forms.ContextMenu();
			this.mnuRTFContextMenuCut = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuCopy = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuPaste = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuSep1 = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuSelectAll = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuSep2 = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuFont = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuFontChange = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuFontDefault = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraph = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphAlign = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphAlignLeft = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphAlignCenter = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphAlignRight = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphSep1 = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphBullet = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphSep2 = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphIndent = new System.Windows.Forms.MenuItem();
			this.mnuRTFContextMenuParagraphUnindent = new System.Windows.Forms.MenuItem();
			this.ttBase = new System.Windows.Forms.ToolTip(this.components);
			this.dlgFont = new System.Windows.Forms.FontDialog();
			((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			this.SuspendLayout();
			//
			//imgBase
			//
			this.imgBase.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgBase.ImageStream");
			this.imgBase.TransparentColor = System.Drawing.Color.Transparent;
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
			//ctxRTF
			//
			this.ctxRTF.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuRTFContextMenuCut,
				this.mnuRTFContextMenuCopy,
				this.mnuRTFContextMenuPaste,
				this.mnuRTFContextMenuSep1,
				this.mnuRTFContextMenuSelectAll,
				this.mnuRTFContextMenuSep2,
				this.mnuRTFContextMenuFont,
				this.mnuRTFContextMenuParagraph
			});
			//
			//mnuRTFContextMenuCut
			//
			this.mnuRTFContextMenuCut.Index = 0;
			this.mnuRTFContextMenuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.mnuRTFContextMenuCut.Text = "Cu&t";
			//
			//mnuRTFContextMenuCopy
			//
			this.mnuRTFContextMenuCopy.Index = 1;
			this.mnuRTFContextMenuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.mnuRTFContextMenuCopy.Text = "&Copy";
			//
			//mnuRTFContextMenuPaste
			//
			this.mnuRTFContextMenuPaste.Index = 2;
			this.mnuRTFContextMenuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.mnuRTFContextMenuPaste.Text = "&Paste";
			//
			//mnuRTFContextMenuSep1
			//
			this.mnuRTFContextMenuSep1.Index = 3;
			this.mnuRTFContextMenuSep1.Text = "-";
			//
			//mnuRTFContextMenuSelectAll
			//
			this.mnuRTFContextMenuSelectAll.Index = 4;
			this.mnuRTFContextMenuSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.mnuRTFContextMenuSelectAll.Text = "Select &All";
			//
			//mnuRTFContextMenuSep2
			//
			this.mnuRTFContextMenuSep2.Index = 5;
			this.mnuRTFContextMenuSep2.Text = "-";
			//
			//mnuRTFContextMenuFontChange
			//
			this.mnuRTFContextMenuFontChange.Index = 0;
			this.mnuRTFContextMenuFontChange.Text = "&Change";
			//
			//mnuRTFContextMenuFontDefault
			//
			this.mnuRTFContextMenuFontDefault.Index = 1;
			this.mnuRTFContextMenuFontDefault.Text = "&Default";
			//
			//mnuRTFContextMenuFont
			//
			this.mnuRTFContextMenuFont.Index = 6;
			this.mnuRTFContextMenuFont.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuRTFContextMenuFontChange,
				this.mnuRTFContextMenuFontDefault
			});
			this.mnuRTFContextMenuFont.Text = "&Font";
			//
			//mnuRTFContextMenuParagraph
			//
			this.mnuRTFContextMenuParagraph.Index = 7;
			this.mnuRTFContextMenuParagraph.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuRTFContextMenuParagraphAlign,
				this.mnuRTFContextMenuParagraphSep1,
				this.mnuRTFContextMenuParagraphBullet,
				this.mnuRTFContextMenuParagraphSep2,
				this.mnuRTFContextMenuParagraphIndent,
				this.mnuRTFContextMenuParagraphUnindent
			});
			this.mnuRTFContextMenuParagraph.Text = "Paragraph";
			//
			//mnuRTFContextMenuParagraphAlign
			//
			this.mnuRTFContextMenuParagraphAlign.Index = 0;
			this.mnuRTFContextMenuParagraphAlign.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuRTFContextMenuParagraphAlignLeft,
				this.mnuRTFContextMenuParagraphAlignCenter,
				this.mnuRTFContextMenuParagraphAlignRight
			});
			this.mnuRTFContextMenuParagraphAlign.Text = "&Alignment";
			//
			//mnuRTFContextMenuParagraphAlignLeft
			//
			this.mnuRTFContextMenuParagraphAlignLeft.Index = 0;
			this.mnuRTFContextMenuParagraphAlignLeft.Text = "&Left";
			//
			//mnuRTFContextMenuParagraphAlignCenter
			//
			this.mnuRTFContextMenuParagraphAlignCenter.Index = 1;
			this.mnuRTFContextMenuParagraphAlignCenter.Text = "&Center";
			//
			//mnuRTFContextMenuParagraphAlignRight
			//
			this.mnuRTFContextMenuParagraphAlignRight.Index = 2;
			this.mnuRTFContextMenuParagraphAlignRight.Text = "&Right";
			//
			//mnuRTFContextMenuParagraphSep1
			//
			this.mnuRTFContextMenuParagraphSep1.Index = 1;
			this.mnuRTFContextMenuParagraphSep1.Text = "-";
			//
			//mnuRTFContextMenuParagraphBullet
			//
			this.mnuRTFContextMenuParagraphBullet.Index = 2;
			this.mnuRTFContextMenuParagraphBullet.Text = "Bullet";
			//
			//mnuRTFContextMenuParagraphSep2
			//
			this.mnuRTFContextMenuParagraphSep2.Index = 3;
			this.mnuRTFContextMenuParagraphSep2.Text = "-";
			//
			//mnuRTFContextMenuParagraphIndent
			//
			this.mnuRTFContextMenuParagraphIndent.Index = 4;
			this.mnuRTFContextMenuParagraphIndent.Text = "&Indent 0.25\"";
			//
			//mnuRTFContextMenuParagraphUnindent
			//
			this.mnuRTFContextMenuParagraphUnindent.Index = 5;
			this.mnuRTFContextMenuParagraphUnindent.Text = "&Unindent 0.25\"";
			//
			//ttBase
			//
			this.ttBase.ShowAlways = true;
			//
			//frmTCBase
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "frmTCBase";
			this.Text = "frmTCBase";
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region "Properties"
		#region "Enumerations"
		protected enum imgMainEnum
		{
			Books,
			Collectables,
			HobbyKits,
			HobbyDecals,
			HobbyDetailSets,
			HobbyFinishingProducts,
			HobbyTools,
			HobbyVideoResearch,
			HobbyRockets,
			HobbyTrains,
			HobbyCompanies,
			HobbyAircraftDesignations,
			HobbyBlueAngelsHistory,
			Images,
			Music,
			Software,
			USNavy,
			VideoLibraryMovies,
			VideoLibrarySpecials,
			VideoLibraryTVEpisodes,
			WebLinks,
			CrystalReportsXI,
			Options,
			Trace,
			TreasureChest
		}
		#endregion
		private clsTCBase withEventsField_mTCBase;
		protected internal clsTCBase mTCBase {
			get { return withEventsField_mTCBase; }
			set {
				if (withEventsField_mTCBase != null) {
					withEventsField_mTCBase.UnbindControls -= mTCBase_UnbindControls;
				}
				withEventsField_mTCBase = value;
				if (withEventsField_mTCBase != null) {
					withEventsField_mTCBase.UnbindControls += mTCBase_UnbindControls;
				}
			}
		}
		private RichTextBox mActiveRichTextBox = null;
		protected CurrencyManager mCurrencyManager = null;
        protected float mDPI;
        protected int mFormHeight;
        protected int mFormWidth;
        private KeyEventArgs mKeyEventArgs;
		private byte[] mDefaultImage = {
			
		};
		protected byte[] DefaultImage {
			get { return mDefaultImage; }
			set { mDefaultImage = value; }
		}
		protected clsTCBase TCBase {
			get { return mTCBase; }
		}
		protected string TraceText {
			get { return (mSupport.Trace.TraceMode ? "TRACE" : bpeNullString); }
		}
		protected int TraceWidth {
			get { return (mSupport.Trace.TraceMode ? 50 : 0); }
		}
		#endregion
		#region "Methods"
		#region "Controls & Binding"
		#region "Converters"
		private string GetBindingInfo(Binding sender)
		{
			string functionReturnValue = null;
			//Don't trace this guy on purpose... He's providing trace information to his callers...
			functionReturnValue = bpeNullString;
			CurrencyManager cm = (CurrencyManager)sender.BindingManagerBase;
			string Member = sender.BindingMemberInfo.BindingMember;
            DataRowView drv = ((DataRowView)cm.Current);

            functionReturnValue = string.Format("{0}{1}.{2} bound to [{3}].[{4}] ({5})", new object[] {
				Constants.vbTab,
				sender.Control.Name,
				sender.PropertyName,
				mTCBase.TableName,
				Member,
				FormatData(drv.DataView.Table.Columns[Member], drv[Member], true)
			});
			functionReturnValue += string.Format(" @ Row #{0} [{1}].[{2}]:={3};{4}", new object[] {
				cm.Position,
				mTCBase.TableName,
				mTCBase.TableIDColumn,
				(Information.IsDBNull(drv[mTCBase.TableIDColumn]) ? "DBNull" : drv[mTCBase.TableIDColumn]),
				(drv.IsEdit ? " IsEdit" : bpeNullString)
			});
			if (sender.Control is ComboBox){}
			return functionReturnValue;
		}
		protected internal void BinaryToImage(object sender, ConvertEventArgs e)
		{
			byte[] arrPicture = null;
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);
			if (Information.IsDBNull(e.Value) | e.Value.ToString().Trim().Length == 0) {
				Trace("e.Value is Null; Defaulting to TestPattern Image", trcOption.trcControls | trcOption.trcDB);
				//Use the default image...
				arrPicture = DefaultImage;
			} else {
				//The SQL Server Image datatype is a binary datatype. Therefore, to generate an image from it you must first create a stream object 
				//containing the binary data. Then you can generate the image by calling Image.FromStream().
				arrPicture = (byte[])e.Value;
			}
			Trace("Streaming Image...", trcOption.trcControls | trcOption.trcDB);
			System.IO.MemoryStream ms = new System.IO.MemoryStream(arrPicture);
			e.Value = System.Drawing.Image.FromStream(ms);
			//Close the stream object to release the resource.
			ms.Close();
		}
		protected void BooleanToSmallInt(object sender, ConvertEventArgs e)
		{
            Binding binding = (Binding)sender;
			Trace(GetBindingInfo(binding), trcOption.trcControls | trcOption.trcDB);

			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				e.Value = false;
				Trace("e.Value is Null; Converting to " + e.Value, trcOption.trcControls | trcOption.trcDB);
			}
			if (object.ReferenceEquals(e.Value.GetType(), typeof(int))) {
				Trace("{0}.Value is already {1} (and isn't DBNull) - bugging out...", binding.Control.Name, e.Value.ToString(), trcOption.trcControls | trcOption.trcDB);
			} else {
				switch (e.Value) {
					case true:
						e.Value = 1;
						break;
					default:
						e.Value = 0;
						break;
				}
				Trace("e.Value is " + e.Value, trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void DateToNull(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcApplication);
			switch (mTCBase.TableName) {
				case "Class":
				case "Classification":
				case "Ships":
					if (System.DateTime.Compare((DateTime)e.Value, new System.DateTime(1775, 10, 13)) == 0){e.Value = DBNull.Value;mTCBase.LogMessage("Converted 10/13/1775 to DBNull", 0);}
					break;
				default:
					if (System.DateTime.Compare((DateTime)e.Value, new System.DateTime(1963, 7, 31)) == 0){e.Value = DBNull.Value;mTCBase.LogMessage("Converted 07/31/1963 to DBNull", 0);}
					break;
			}
		}
		protected internal void NullToID(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);
			//Trace(New StackTrace(True).ToString, trcOption.trcControls or trcOption.trcDB)
			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				e.Value = bpeNullString;
				Trace("e.Value is Null; Converting to \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			} else {
				Trace("e.Value is \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void NullToString(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);
			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				e.Value = bpeNullString;
				Trace("e.Value is Null; Converting to \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			} else {
				Trace("e.Value is \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void NullToChecked(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);

			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				e.Value = false;
				Trace("e.Value is Null; Converting to \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			} else {
				Trace("e.Value is \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void NullToDate(object sender, ConvertEventArgs e)
		{
            Binding binding = (Binding)sender;
			Trace(GetBindingInfo(binding), trcOption.trcControls | trcOption.trcDB);
			if (Information.IsDBNull(e.Value) | e.Value.ToString().Trim().Length == 0) {
				switch (mTCBase.TableName) {
					case "Class":
					case "Classification":
					case "Ships":
						e.Value = new System.DateTime(1775, 10, 13);
						break;
					default:
						e.Value = new System.DateTime(1963, 7, 31);
						break;
				}
				Trace(string.Format("{0}: e.Value is Null; Converting to \"{1}\"", binding.Control.Name, e.Value.ToString()), trcOption.trcControls | trcOption.trcDB);
				mTCBase.LogMessage(string.Format("{0}: e.Value is Null; Converting to \"{1}\"", binding.Control.Name, e.Value.ToString()), 0);
			} else {
				Trace(string.Format("{0}: e.Value is \"{1}\"", binding.Control.Name, e.Value), trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void NullToMoney(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);
			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				int zero = 0;
				e.Value = zero.ToString("c");
				Trace("e.Value is Null; Converting to \"" + e.Value.ToString() + "\"", trcOption.trcControls | trcOption.trcDB);
			} else {
				Trace("e.Value is \"" + e.Value + "\"", trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void MoneyToString(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcControls | trcOption.trcDB);
			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				int zero = 0;
				Trace("e.Value is Null; Converting to " + e.Value.ToString(), trcOption.trcControls | trcOption.trcDB);
				e.Value = zero.ToString("c");
			} else {
				try {
					Trace("Converting e.Value from {0} to {1}", e.Value.ToString(), Convert.ToDecimal(e.Value).ToString("c"), trcOption.trcControls | trcOption.trcDB);
					e.Value = Convert.ToDecimal(e.Value).ToString("c");
				} catch (InvalidCastException ex) {
					if (object.ReferenceEquals(e.DesiredType, e.Value.GetType())) {
						Trace("Caught {0}; Since e.DesiredType is already {1}, we're leaving the value alone.", ex.GetType().Name, e.DesiredType.FullName, trcOption.trcControls | trcOption.trcDB);
					} else {
						int zero = 0;
						Trace("Caught {0}; Converting to \"{1}\"", ex.GetType().Name, e.Value.ToString(), trcOption.trcControls | trcOption.trcDB);
						e.Value = zero.ToString("c");
					}
				} catch (Exception ex) {
					throw new Exception(string.Format("Caught {0}; Attempting to convert {1} to {2}", ex.GetType().Name, e.Value, e.DesiredType.ToString()), ex);
				}
			}
		}
		protected internal void SmallIntToBoolean(object sender, ConvertEventArgs e)
		{
            Binding binding = (Binding)sender;
			Trace(GetBindingInfo(binding), trcOption.trcControls | trcOption.trcDB);

			if (Information.IsDBNull(e.Value) || e.Value.ToString().Trim().Length == 0) {
				e.Value = 0;
				Trace("e.Value is Null; Converting to " + e.Value, trcOption.trcControls | trcOption.trcDB);
			}
			if (object.ReferenceEquals(e.Value.GetType(), typeof(bool))) {
				Trace("{0}.Value is already {1} (and isn't DBNull) - bugging out...", binding.Control.Name, e.Value.ToString(), trcOption.trcControls | trcOption.trcDB);
			} else {
				switch (e.Value) {
					case 1:
						e.Value = true;
						break;
					default:
						e.Value = false;
						break;
				}
				Trace("e.Value is " + e.Value, trcOption.trcControls | trcOption.trcDB);
			}
		}
		protected internal void StringToMoney(object sender, ConvertEventArgs e)
		{
			Trace(GetBindingInfo((Binding)sender), trcOption.trcApplication);
			try {
				Trace("Converting e.Value from {0} to {1} ({2})", e.Value, Convert.ToDouble(e.Value).ToString(), e.DesiredType.ToString(), trcOption.trcControls | trcOption.trcDB);
				e.Value = Convert.ToDouble(e.Value);
				//Double is equivalent to a Money SQL data type.
			} catch (InvalidCastException ex) {
				int zero = 0;
				Trace("Caught {0}; Converting to {1}", ex.GetType().Name, e.Value.ToString(), trcOption.trcControls | trcOption.trcDB);
				e.Value = zero.ToString("c");
			} catch (Exception ex) {
				throw new Exception(string.Format("Caught {0}; Attempting to convert {1} to {2}", ex.GetType().Name, e.Value, e.DesiredType.ToString()), ex);
			}
		}
		#endregion
		protected void AdjustFormPlacement(ref int iTop, ref int iLeft)
		{
			if (iTop < 0) iTop = 0;
			if (iLeft < 0) {
				iLeft = 0;
			} 
			else if (iLeft > mTCBase.MyComputer.Screen.Bounds.Left) {
				iLeft -= mTCBase.MyComputer.Screen.Bounds.Left;
			}
		}
		private bool AnythingHasChanged(object oldValue, object newValue)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			if (!(Information.IsDBNull(oldValue) && Information.IsDBNull(newValue))) {
				if (Information.IsDBNull(oldValue) && !Information.IsDBNull(newValue)){functionReturnValue = true;return functionReturnValue;}
				if (!Information.IsDBNull(oldValue) && Information.IsDBNull(newValue)){functionReturnValue = true;return functionReturnValue;}
				switch (Information.TypeName(oldValue)) {
					case "Byte()":
						byte[] oldBytes = (byte[])oldValue;
						byte[] newBytes = (byte[])newValue;
						if (oldBytes.LongLength != newBytes.LongLength){functionReturnValue = true;return functionReturnValue;}
						if ((!object.ReferenceEquals(oldBytes, newBytes))){functionReturnValue = true;return functionReturnValue;}
						break;
					default:
						if (oldValue != newValue){functionReturnValue = true;return functionReturnValue;}
						break;
				}
			}
			return functionReturnValue;
		}
		public void AutoComplete_KeyDown(object sender, KeyEventArgs e)
		{
			//No tracing on purpose (performance)...
			ComboBox cb = (ComboBox)sender;
			try {
				mKeyEventArgs = e;
				switch (e.KeyCode) {
					case Keys.End:
					case Keys.Home:
					case Keys.Up:
					case Keys.Down:
					case Keys.PageUp:
					case Keys.PageDown:
						e.Handled = false;
						return;

						break;
					case Keys.Left:
						if (cb.SelectionStart > 0) {
							cb.SelectionStart -= 1;
							e.Handled = true;
						} else {
							e.Handled = false;
						}
						return;

						break;
					case Keys.Right:
						if (cb.SelectionStart < cb.SelectionLength) {
							cb.SelectionStart += 1;
							e.Handled = true;
						} else {
							e.Handled = false;
						}
						return;

						break;
					default:
						if (e.Control){e.Handled = false;return;
}
						break;
				}
			} finally {
				Trace("frmTCBase.AutoComplete_KeyDown: cb.Text = \"{0}\"", new object[] { cb.Text }, trcOption.trcControls);
				Trace("frmTCBase.AutoComplete_KeyDown: cb.SelectedText = \"{0}\"", new object[] { cb.SelectedText }, trcOption.trcControls);
			}
		}
		//Handles cbAlphaSort.KeyPress, cbLocation.KeyPress
		public void AutoComplete_KeyPress(object sender, KeyPressEventArgs e)
		{
			//No tracing on purpose (performance)...
			ComboBox cb = (ComboBox)sender;
			//If we're a DropDownList, bail and let it do its own thing...
			if (cb.DropDownStyle == ComboBoxStyle.DropDownList){e.Handled = false;return;
}

			string searchText = bpeNullString;
			short newChar = 0;
			try {
				switch ((Keys)Strings.Asc(e.KeyChar)) {
					case Keys.Escape:
						cb.SelectedIndex = -1;
						cb.Text = "";
						e.Handled = true;
						return;

						break;
					case Keys.Back:
						if (cb.SelectionStart <= 1){cb.Text = "";return;
}
						searchText = cb.Text.Substring(0, (cb.SelectionLength == 0 ? cb.Text.Length : cb.SelectionStart) - 1);
						break;
					default:
						if (mKeyEventArgs.Control && mKeyEventArgs.Shift) {
							switch (mKeyEventArgs.KeyCode) {
								case Keys.C:
									newChar = 169;
									//©
									break;
								case Keys.D:
									newChar = 176;
									//°
									break;
								case Keys.R:
									newChar = 174;
									//®
									break;
								case Keys.T:
									newChar = 153;
									//™
									break;
								default:
									if (char.IsControl(e.KeyChar)){e.Handled = false;return;
}
									break;
							}
							if (newChar != 0)
								searchText = (cb.SelectionLength == 0 ? cb.Text : cb.Text.Substring(0, cb.SelectionStart)) + Strings.Chr(newChar);
						} else {
							searchText = (cb.SelectionLength == 0 ? cb.Text : cb.Text.Substring(0, cb.SelectionStart)) + char.ToUpper(e.KeyChar);
						}
						break;
				}

				int MatchIndex = cb.FindString(searchText);
				Trace("cb.FindString(\"{0}\") returned: {1}", new object[] {
					searchText,
					MatchIndex
				}, trcOption.trcControls);
				if (MatchIndex != -1) {
					cb.SelectedText = "";
					cb.SelectedIndex = MatchIndex;
					cb.SelectionStart = searchText.Length;
					cb.SelectionLength = cb.Text.Length;
					e.Handled = true;
				} else if (mKeyEventArgs.Control && mKeyEventArgs.Shift && newChar != 0) {
					if (cb.SelectionLength > 0) {
						cb.SelectedText = Strings.Chr(newChar).ToString();
					} else {
						int ss = cb.SelectionStart;
						cb.Text = cb.Text.Substring(0, cb.SelectionStart) + Strings.Chr(newChar) + cb.Text.Substring(cb.SelectionStart);
						cb.SelectionStart = ss + 1;
					}
					e.Handled = true;
				} else if (!char.IsLetter(e.KeyChar)) {
					//TODO: AutoComplete: Fix Force Upper-Case logic...
					e.Handled = false;
					//ElseIf Not IsNothing(cb.Tag) AndAlso CType(cb.Tag, String).ToUpper.IndexOf("UPPER") >= 0 Then
					//Dim saveText As String = cb.Text
					//Dim selectionStart As Integer = cb.SelectionStart
					//Dim selectionLength As Integer = cb.SelectionLength
					//cb.SelectedIndex = -1
					//Mid(saveText, selectionStart, selectionLength) = Char.ToUpper(e.KeyChar)
					//cb.Text = saveText
					//cb.SelectionStart = selectionStart + 1
					//cb.SelectionLength = 0
					//e.Handled = True
				} else {
					e.Handled = false;
				}
			} finally {
				Trace("frmTCBase.AutoComplete_KeyPress: cb.Text = \"{0}\"", new object[] { cb.Text }, trcOption.trcControls);
				Trace("frmTCBase.AutoComplete_KeyPress: cb.SelectedText = \"{0}\"", new object[] { cb.SelectedText }, trcOption.trcControls);
			}
		}
		public void BindControl(Control ctl, DataView dataSource, string dataMember, DataView displaySource, string displayMember, string valueMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\",{4},\"{5}\",\"{6}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember,
					displaySource.GetType().Name,
					displayMember,
					valueMember
				}, trcOption.trcBinding | trcOption.trcControls);
				switch (Information.TypeName(ctl)) {
					case "ComboBox":
						var _with2 = (ComboBox)ctl;
						//Example:
						//displaySource: dvClassifications
						//displayMember: Type
						//valueMember:   ID
						//dataSource:    dvClass
						//dataMember:    ClassificationID (i.e. the field in the dataSource we want bound to the valueMember of the displaySource)
						_with2.DataSource = displaySource;
						_with2.DisplayMember = displayMember;
						_with2.ValueMember = valueMember;
						//This really does need to be "valueMember" from the displaySource...
						int maxLength = dataSource.Table.Columns[dataMember].MaxLength;
						if (maxLength > 0)
							_with2.MaxLength = maxLength;
						try {
							_with2.DataBindings.Add("SelectedValue", dataSource, dataMember);
						} catch (ArgumentException ex) {
							Binding iBinding = _with2.DataBindings["SelectedValue"];
							if (iBinding == null) throw;
							//Check if we're already bound the way we want (It's OK)...
							if (dataSource.Equals(iBinding.DataSource) && iBinding.BindingMemberInfo.BindingMember.Equals(dataMember)) return;

							//If we're already bound to a different dataDource / dataMember, unbind and try again...
							String Message = GetBindingInfo(iBinding);
							if (!iBinding.DataSource.Equals(dataSource)) Message += "; dataSource does not match existing binding";
							if (!iBinding.BindingMemberInfo.BindingMember.Equals(dataMember)) Message += "; dataMember does not match existing binding";
							Message = ex.Message + "; " + Message;
							Trace(Message, trcOption.trcBinding); if (!TraceMode) PrintOut(Message);
							_with2.DataBindings.Remove(iBinding);
							_with2.DataBindings.Add("SelectedValue", dataSource, dataMember);
  						}
						break;
					default:
						throw new Exception(string.Format("Unexpected control type ({0}) encountered in {1}(Control,DataView,String,String). Control: {2}", Information.TypeName(ctl), EntryName, ctl.Name));
				}
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(ComboBox ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Text", dataSource, dataMember);
				tmpBinding.Format += NullToString;
				var _with3 = (ComboBox)ctl;
				int maxLength = dataSource.Table.Columns[dataMember].MaxLength;
				if (maxLength > 0)
					_with3.MaxLength = maxLength;
				try {
					_with3.DataBindings.Add(tmpBinding);
				} catch (ArgumentException ex) {
					Binding iBinding = _with3.DataBindings["SelectedValue"];
					if (iBinding == null) throw;
					//Check if we're already bound the way we want (It's OK)...
					if (dataSource.Equals(iBinding.DataSource) && iBinding.BindingMemberInfo.BindingMember.Equals(dataMember)) return;

					//If we're already bound to a different dataDource / dataMember, unbind and try again...
					String Message = GetBindingInfo(iBinding);
					if (!iBinding.DataSource.Equals(dataSource)) Message += "; dataSource does not match existing binding";
					if (!iBinding.BindingMemberInfo.BindingMember.Equals(dataMember)) Message += "; dataMember does not match existing binding";
					Message = ex.Message + "; " + Message;
					Trace(Message, trcOption.trcBinding); if (!TraceMode) PrintOut(Message);
					_with3.DataBindings.Remove(iBinding);
					_with3.DataBindings.Add("SelectedValue", dataSource, dataMember);
				}
			}
			catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(CheckBox ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Checked", dataSource, dataMember);
				tmpBinding.Format += SmallIntToBoolean;
				tmpBinding.Parse += BooleanToSmallInt;
				((CheckBox)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(DateTimePicker ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Value", dataSource, dataMember);
				tmpBinding.Format += NullToDate;
				tmpBinding.Parse += DateToNull;
				((DateTimePicker)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(Label ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Text", dataSource, dataMember);
				tmpBinding.Format += NullToString;
				((Label)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(PictureBox ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Image", dataSource, dataMember);
				tmpBinding.Format += BinaryToImage;
				((PictureBox)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(RadioButton ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Checked", dataSource, dataMember);
				tmpBinding.Format += SmallIntToBoolean;
				tmpBinding.Parse += BooleanToSmallInt;
				((RadioButton)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(RichTextBox ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				Binding tmpBinding = new Binding("Rtf", dataSource, dataMember);
				//Dim tmpBinding As New Binding("Text", dataSource, dataMember)
				tmpBinding.Format += NullToString;
				((RichTextBox)ctl).DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		public void BindControl(TextBox ctl, DataView dataSource, string dataMember)
		{
			string EntryName = "BindControl";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({1},{2},\"{3}\")", new object[] {
					EntryName,
					ctl.Name,
					dataSource.GetType().Name,
					dataMember
				}, trcOption.trcBinding | trcOption.trcControls);
				var _with4 = (TextBox)ctl;
				Binding tmpBinding = new Binding("Text", dataSource, dataMember);
                if (dataMember == mTCBase.TableIDColumn) 
					//Distinguish only for debugging purposes...
						tmpBinding.Format += NullToID;
					else
						tmpBinding.Format += NullToString;
				string strTag = (_with4.Tag == null ? "" : Convert.ToString(_with4.Tag));
				string[] tagParams = strTag.Split(",".ToCharArray());
				for (int i = 0; i <= tagParams.Length - 1; i++) {
					switch (tagParams[i].ToUpper()) {
						case "MONEY":
							tmpBinding.Format += MoneyToString;
							tmpBinding.Parse += StringToMoney;
							break;
						case "NULLS":
							break;
					}
				}
				int maxLength = dataSource.Table.Columns[dataMember].MaxLength;
				if (maxLength > 0)
					_with4.MaxLength = maxLength;
				_with4.DataBindings.Add(tmpBinding);
			} catch (Exception ex) {
				throw;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding | trcOption.trcControls);
			}
		}
		protected void ClearErrors()
		{
			const string EntryName = "ClearErrors";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcControls);
				Trace("Clearing All Errors...", trcOption.trcControls);
				this.ClearErrors(this.Controls);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcControls);
			}
		}
		protected void ClearErrors(Control.ControlCollection Controls, int indent = 0)
		{
			const string EntryName = "ClearErrors";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcControls);
				foreach (Control ctl in Controls) {
					this.ClearErrors(ctl, indent);
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcControls);
			}
		}
		protected internal void ClearErrors(Control ctl, int indent = 0)
		{
			const string EntryName = "ClearErrors";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcControls);
				switch (Information.TypeName(ctl)) {
					case "Form":
					case "GroupBox":
					case "StatusBar":
					case "TabControl":
					case "TabPage":
						Trace(string.Format("{0}Clearing {1}", new string('\t', indent), ctl.Name), trcOption.trcControls);
						this.ClearErrors(ctl.Controls, indent + 1);
						break;
					default:
						Trace(string.Format("{0}Clearing {1}", new string('\t', indent), ctl.Name), trcOption.trcControls);
						break;
				}
				this.epBase.SetError(ctl, bpeNullString);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcControls);
			}
		}
		protected void cbForceUpperCase(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				((ComboBox)sender).Text = ((ComboBox)sender).Text.ToUpper();
			} catch (Exception ex) {
				base.Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected void cbValidating(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				mTCBase.cbValidate((ComboBox)sender, e);
			} catch (Exception ex) {
				base.Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		public void Control_KeyDown(object sender, KeyEventArgs e)
		{
			//No tracing on purpose (performance)...
			mKeyEventArgs = e;
		}
		public void Control_KeyPress(object sender, KeyPressEventArgs e)
		{
			//No tracing on purpose (performance)...
			try {
				((frmTCBase)this).epBase.SetError((Control)sender, bpeNullString);
				switch (sender.GetType().Name) {
					case "TextBox":
						TextBox tb = (TextBox)sender;
						string remainingText = tb.Text;
						if (tb.SelectionLength > 0)
							remainingText = tb.Text.Replace(tb.Text.Substring(tb.SelectionStart, tb.SelectionLength), bpeNullString);
						if ((tb.Tag == null ? "" : Convert.ToString(tb.Tag)).ToUpper().Contains("MONEY")) {
							//Note: The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events.
							//(i.e. Not attempting Undo)
							if ((mKeyEventArgs.Control && mKeyEventArgs.KeyCode == Keys.Z)) {
                                if ((mKeyEventArgs.KeyCode >= Keys.D0 && mKeyEventArgs.KeyCode <= Keys.D9) ||
                                    (mKeyEventArgs.KeyCode >= Keys.NumPad0 && mKeyEventArgs.KeyCode <= Keys.NumPad9) ||
                                    mKeyEventArgs.KeyCode == Keys.Home || mKeyEventArgs.KeyCode == Keys.End ||
                                    mKeyEventArgs.KeyCode == Keys.Left || mKeyEventArgs.KeyCode == Keys.Right ||
                                    mKeyEventArgs.KeyCode == Keys.Back || mKeyEventArgs.KeyCode == Keys.Delete) {
                                } else if (mKeyEventArgs.KeyCode == Keys.Decimal || mKeyEventArgs.KeyCode == Keys.OemPeriod) {
									//KeyPad-Decimal, Keyboard-Period (respectively)
									if (remainingText.IndexOf(".") >= 0)
										throw new Exception("Invalid value entered.");
                                }
                                else if (mKeyEventArgs.KeyCode == Keys.Oemcomma) {
									//Keyboard-Comma
									if (remainingText.IndexOf(".") >= 0)
										throw new Exception("Invalid value entered.");
                                } else {
									//Use the following initialization to trigger the conversion exception (if any)
									string strPrice = Convert.ToDecimal(tb.Text).ToString("c");
                                }
							}
						} else if ((tb.Tag == null ? "" : Convert.ToString(tb.Tag)).ToUpper().Contains("INTEGER")) {
							//Note: The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events.
							//(i.e. Not attempting Undo)
							if (!(mKeyEventArgs.Control && mKeyEventArgs.KeyCode == Keys.Z)) {
                                if ((mKeyEventArgs.KeyCode >= Keys.D0 && mKeyEventArgs.KeyCode <= Keys.D9) ||
                                    (mKeyEventArgs.KeyCode >= Keys.NumPad0 && mKeyEventArgs.KeyCode <= Keys.NumPad9) ||
                                    mKeyEventArgs.KeyCode == Keys.Home || mKeyEventArgs.KeyCode == Keys.End ||
                                    mKeyEventArgs.KeyCode == Keys.Left || mKeyEventArgs.KeyCode == Keys.Right ||
                                    mKeyEventArgs.KeyCode == Keys.Back || mKeyEventArgs.KeyCode == Keys.Delete) {
                                } else {
                                    //Use the following initialization to trigger the conversion exception (if any)
                                    string strPrice = Convert.ToDecimal(tb.Text).ToString("c");
                                }
							}
						} else {
							short newChar = 0;
							if (mKeyEventArgs.Control && mKeyEventArgs.Shift) {
								switch (mKeyEventArgs.KeyCode) {
									//Asc(e.KeyChar)
									case Keys.C:
										newChar = 169;
										//©
										break;
									case Keys.D:
										newChar = 176;
										//°
										break;
									case Keys.R:
										newChar = 174;
										//®
										break;
									case Keys.T:
										newChar = 153;
										//™
										break;
									default:
										newChar = 0;
										break;
								}
							}
							if (newChar != 0) {
								if (tb.SelectionLength > 0) {
									tb.SelectedText = Strings.Chr(newChar).ToString();
								} else {
									int ss = tb.SelectionStart;
									tb.Text = tb.Text.Substring(0, tb.SelectionStart) + Strings.Chr(newChar) + tb.Text.Substring(tb.SelectionStart);
									tb.SelectionStart = ss + 1;
								}
								e.Handled = true;
							}
						}
						break;
				}
			} catch (Exception ex) {
				((frmTCBase)this).epBase.SetError((Control)sender, ex.Message);
				e.Handled = true;
			}
		}
		public void Control_KeyUp(object sender, KeyEventArgs e)
		{
			//No tracing on purpose (performance)...
			mKeyEventArgs = e;
			try {
				((frmTCBase)this).epBase.SetError((Control)sender, bpeNullString);
				switch (sender.GetType().Name) {
					case "DateTimePicker":
						DateTimePicker dtp = (DateTimePicker)sender;
						switch (mKeyEventArgs.KeyCode) {
							case Keys.F5:
								dtp.Value = DateAndTime.Now;
								e.Handled = true;
								break;
							case Keys.F6:
								dtp.Value = new System.DateTime(1963, 7, 31);
								e.Handled = true;
								break;
						}
						break;
				}
			} catch (Exception ex) {
				((frmTCBase)this).epBase.SetError((Control)sender, ex.Message);
				e.Handled = true;
			}
		}
		protected void DefaultTextBox(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				if (Strings.Trim(((TextBox)sender).Text) == bpeNullString)
					((TextBox)sender).Text = "UNKNOWN";
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected void dtpValidating(object sender, CancelEventArgs e)
		{
			DateTimePicker dtp = (DateTimePicker)sender;
			if (dtp.DataBindings["Value"] == null)
				return;
            try
            {
                base.epBase.SetError((Control)sender, bpeNullString);
                DataView dv = (DataView)dtp.DataBindings["Value"].DataSource;
                string MemberName = dtp.DataBindings["Value"].BindingMemberInfo.BindingMember;
                object dataValue = mTCBase.CurrentRow[MemberName];
                if (Information.IsDBNull(dataValue))
                    dataValue = "Null";
                Trace(string.Format("dtpValidating: {0}.Value:={1}; [{2}].[{3}]:={4};", new object[] {
                    dtp.Name,
                    dtp.Value,
                    dv.Table.TableName,
                    MemberName,
                    dataValue
                }), trcOption.trcControls | trcOption.trcDB);
            } catch (Exception ex) when (mTCBase.Mode == ActionModeEnum.modeDelete || mTCBase.Mode == ActionModeEnum.modeDisplay) {
            } catch (Exception ex) {
                base.Trace(ex.Message, trcOption.trcAll);
                base.epBase.SetError((Control)sender, ex.Message);
                e.Cancel = true;
            }
		}
		protected void dtpValidated(object sender, object e)
		{
			DateTimePicker dtp = (DateTimePicker)sender;
			if (dtp.DataBindings["Value"] == null)
				return;
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				DataView dv = (DataView)dtp.DataBindings["Value"].DataSource;
				string MemberName = dtp.DataBindings["Value"].BindingMemberInfo.BindingMember;
				object dataValue = mTCBase.CurrentRow[MemberName];
				if (Information.IsDBNull(dataValue))
					dataValue = "Null";
				Trace(string.Format("dtpValidated: {0}.Value:={1}; [{2}].[{3}]:={4};", new object[] {
					dtp.Name,
					dtp.Value,
					dv.Table.TableName,
					MemberName,
					dataValue
				}), trcOption.trcControls | trcOption.trcDB);
            } catch (Exception ex) when (mTCBase.Mode == ActionModeEnum.modeDelete || mTCBase.Mode == ActionModeEnum.modeDisplay) {
            } catch (Exception ex) {
				base.Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected void dtpValueChanged(object sender, object e)
		{
			DateTimePicker dtp = (DateTimePicker)sender;
			if (dtp.DataBindings["Value"] == null)
				return;
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				DataView dv = (DataView)dtp.DataBindings["Value"].DataSource;
				string MemberName = dtp.DataBindings["Value"].BindingMemberInfo.BindingMember;
				object dataValue = mTCBase.CurrentRow[MemberName];
				if (Information.IsDBNull(dataValue))
					dataValue = "Null";
				Trace(string.Format("dtpValueChanged: {0}.Value:={1}; [{2}].[{3}]:={4};", new object[] {
					dtp.Name,
					dtp.Value,
					dv.Table.TableName,
					MemberName,
					dataValue
				}), trcOption.trcControls | trcOption.trcDB);
				if (mTCBase.Mode != ActionModeEnum.modeDisplay) {
					//Correct issue with DateTimePicker validation firing before year is evaluated...
					if (Information.IsDBNull(mTCBase.CurrentRow[MemberName]) || (DateTime)dataValue != dtp.Value)
						mTCBase.CurrentRow[MemberName] = dtp.Value;
				}
			} catch (Exception ex) when (mTCBase.Mode == ActionModeEnum.modeDelete || mTCBase.Mode == ActionModeEnum.modeDisplay) {
			} catch (Exception ex) {
				base.Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected internal void EnableControl(Control ctl, bool Enable)
		{
			const string EntryName = "EnableControl";
			Trace("{0}({1},{2})", new object[] {
				EntryName,
				ctl.Name,
				Enable
			}, trcOption.trcControls);
			EnableControl(ctl, Enable, false);
		}
		protected internal void EnableControl(Control ctl, bool Enable, bool Clear)
		{
			const string EntryName = "EnableControl";
			Trace("{0}({1},{2},{3})", new object[] {
				EntryName,
				ctl.Name,
				Enable,
				Clear
			}, trcOption.trcControls);
			string strTag = (ctl.Tag == null ? "" : Convert.ToString(ctl.Tag));
			string[] tagParams = strTag.Split(",".ToCharArray());
			for (int i = 0; i <= tagParams.Length - 1; i++) {
				switch (tagParams[i].ToUpper()) {
					case "IGNORE":
						return;

						break;
				}
			}

			Color BackColor = (Enable ? System.Drawing.SystemColors.Window : System.Drawing.SystemColors.Control);
			switch (Information.TypeName(ctl)) {
				case "Button":		((Button)ctl).Enabled = Enable;	break;
				case "CheckBox":
					CheckBox cbx = (CheckBox)ctl;
					cbx.Enabled = Enable;
					break;
				case "ComboBox":
					ComboBox cb = (ComboBox)ctl;
					cb.Enabled = Enable;
					//Something's trashing our valid values here (maybe image processing?), for now ignore the error...
					try {
						if (!cb.Focused && cb.DropDownStyle != ComboBoxStyle.DropDownList){cb.SelectionStart = 0;cb.SelectionLength = 0;}
					} catch {
					}
					if (Clear)
						cb.SelectedIndex = -1;
					cb.BackColor = BackColor;
					break;
				case "DataGrid":
					DataGrid dg = (DataGrid)ctl;
					dg.Enabled = true;
					//Allow scrolling, etc.
					dg.ReadOnly = true;
					dg.BackColor = BackColor;
					break;
				case "DataGridView":
					DataGridView dgv = (DataGridView)ctl;
					dgv.Enabled = true;
					//Allow scrolling, etc.
					dgv.ReadOnly = true;
					dgv.BackColor = BackColor;
					break;
				case "DateTimePicker":
					DateTimePicker dtp = (DateTimePicker)ctl;
					dtp.Enabled = Enable;
					dtp.CalendarMonthBackground = BackColor;
					dtp.CalendarTitleBackColor = BackColor;
					break;
				case "Form":		((Form)ctl).Enabled = Enable;	break;
				case "GroupBox":	EnableControls(((GroupBox)ctl).Controls, Enable, Clear);	break;
				case "HScrollBar":	break;
				case "Label":		break;
				case "PictureBox":	((PictureBox)ctl).Enabled = Enable;	break;
				case "RichTextBox":
					RichTextBox rtf = (RichTextBox)ctl;
					rtf.Enabled = true;
					//Allow scrolling, etc.
					rtf.ReadOnly = !Enable;
					rtf.BackColor = BackColor;
					break;
				case "StatusBar":	break;
				case "StatusStrip": break;
				case "TabControl":	EnableControls(((TabControl)ctl).Controls, Enable, Clear);	break;
				case "TabPage":		EnableControls(((TabPage)ctl).Controls, Enable, Clear);		break;
				case "TextBox":
					TextBox txt = (TextBox)ctl;
					txt.Enabled = Enable;
					//txt.ReadOnly = Not Enable
					txt.BackColor = (txt.ReadOnly ? System.Drawing.SystemColors.Control : BackColor);
					break;
				case "TreeView":
					TreeView tvw = (TreeView)ctl;
					tvw.Enabled = Enable;
					tvw.BackColor = BackColor;
					break;
				case "ToolBar":		break;
				case "VScrollBar":	break;
				default:			throw new Exception(string.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", Information.TypeName(ctl), EntryName, ctl.Name));
			}
		}
		protected internal void EnableControls(Control.ControlCollection pControls, bool Enable, bool Clear)
		{
			const string EntryName = "EnableControls";
			Trace("{0}({{ControlCollection}},{1},{2})", new object[] {
				EntryName,
				Enable,
				Clear
			}, trcOption.trcControls);
			foreach (Control ctl in pControls) {
				EnableControl(ctl, Enable, Clear);
			}
		}
		protected internal void EnableControlsByBinding(bool Enable, bool Clear)
		{
			const string EntryName = "EnableControlsByBinding";
			Trace("{0}({1},{2})", new object[] {
				EntryName,
				Enable,
				Clear
			}, trcOption.trcControls);
			foreach (Binding iBinding in mCurrencyManager.Bindings) {
				EnableControl(iBinding.Control, Enable, Clear);
			}
		}
		protected internal void FixComboBoxes(Control ctl)
		{
			const string EntryName = "FixComboBoxes";
			Trace("{0}({1})", new object[] {
				EntryName,
				ctl.Name
			}, trcOption.trcControls);
			string strTag = (ctl.Tag == null ? "" : Convert.ToString(ctl.Tag));
			switch (Information.TypeName(ctl)) {
				case "ComboBox":
					ComboBox cb = (ComboBox)ctl;
					if (!cb.Focused && cb.DropDownStyle != ComboBoxStyle.DropDownList)
						cb.SelectionLength = 0;
					break;
				case "GroupBox":
					FixComboBoxes(((GroupBox)ctl).Controls);
					break;
				case "TabControl":
					FixComboBoxes(((TabControl)ctl).Controls);
					break;
				case "TabPage":
					FixComboBoxes(((TabPage)ctl).Controls);
					break;
			}
		}
		protected internal void FixComboBoxes(Control.ControlCollection pControls)
		{
			const string EntryName = "FixComboBoxes";
			Trace("{0}({{ControlCollection}})", new object[] { EntryName }, trcOption.trcControls);
			foreach (Control ctl in pControls) {
				FixComboBoxes(ctl);
			}
		}
		protected void RemoveControlHandlers(Control ctl)
		{
			const string EntryName = "RemoveControlHandlers";
			Trace("{0}({1})", new object[] {
				EntryName,
				ctl.Name
			}, trcOption.trcControls);
			switch (Information.TypeName(ctl)) {
				case "ComboBox":
					ComboBox cb = (ComboBox)ctl;
					cb.KeyPress -= AutoComplete_KeyPress;
					cb.KeyDown -= AutoComplete_KeyDown;
					cb.Validating -= cbValidating;
					if ((cb.Tag == null ? "" : Convert.ToString(cb.Tag)).ToUpper().IndexOf("UPPER") >= 0)
						cb.Validating -= cbForceUpperCase;
					break;
				case "DateTimePicker":
					DateTimePicker dtp = (DateTimePicker)ctl;
					dtp.KeyUp -= Control_KeyUp;
					dtp.Validating -= dtpValidating;
					dtp.Validated -= dtpValidated;
					dtp.ValueChanged -= dtpValueChanged;
					break;
				case "RichTextBox":
					RichTextBox rtb = (RichTextBox)ctl;
					rtb.KeyPress -= rtbKeyPress;
					rtb.MouseEnter -= rtbMouseEnter;
					//: RemoveHandler rtb.MouseLeave, AddressOf rtbMouseLeave
					rtb.SelectionChanged -= rtbSelectionChanged;
					break;
				case "TextBox":
					TextBox tb = (TextBox)ctl;
					tb.KeyDown -= Control_KeyDown;
					tb.KeyPress -= Control_KeyPress;
					tb.Validating -= TextBox_Validating;
					break;
			}
		}
        protected void ResizeControl(Control iControl, int LeftDelta, int WidthDelta)
        {
            try {
                base.epBase.SetError(iControl, bpeNullString);
                //Debug.WriteLine(String.Format("ResizeControl: {0}.SetBounds({1:D}, {2:D}, {3:D}, {4:D}, {5})", New Object[] {iControl.Name, .Left + LeftDelta, 0, .Width + WidthDelta, 0, BoundsSpecified.X Or BoundsSpecified.Width}));
                iControl.SetBounds(iControl.Left + LeftDelta, 0, iControl.Width + WidthDelta, 0, BoundsSpecified.X | BoundsSpecified.Width);
            } catch (Exception ex) {
                base.epBase.SetError(iControl, ex.Message);
            }
        }
        protected void ResizeControl(Control iControl, int LeftDelta, int TopDelta, int WidthDelta, int HeightDelta)
        {
            try {
                base.epBase.SetError(iControl, bpeNullString);
                //Debug.WriteLine(String.Format("ResizeControl: {0}.SetBounds({1:D}, {2:D}, {3:D}, {4:D}, {5})", New Object() {iControl.Name, .Left + LeftDelta, .Top + TopDelta, .Width + WidthDelta, .Height + HeightDelta, BoundsSpecified.All}));
                iControl.SetBounds(iControl.Left + LeftDelta, iControl.Top + TopDelta, iControl.Width + WidthDelta, iControl.Height + HeightDelta, BoundsSpecified.All);
            } catch (Exception ex) {
                base.epBase.SetError(iControl, ex.Message);
            }
        }
        protected void SetupControlHandlers(Control ctl)
		{
			const string EntryName = "SetupControlHandlers";
			Trace("{0}({1})", new object[] {
				EntryName,
				ctl.Name
			}, trcOption.trcControls);
			switch (Information.TypeName(ctl)) {
				case "ComboBox":
					ComboBox cb = (ComboBox)ctl;
					cb.KeyPress += AutoComplete_KeyPress;
					cb.KeyDown += AutoComplete_KeyDown;
					cb.Validating += cbValidating;
					if (TagContains(ctl, "UPPER"))
						cb.Validating += cbForceUpperCase;
					break;
				case "DateTimePicker":
					DateTimePicker dtp = (DateTimePicker)ctl;
					dtp.KeyUp += Control_KeyUp;
					dtp.Validating += dtpValidating;
					dtp.Validated += dtpValidated;
					dtp.ValueChanged += dtpValueChanged;
					break;
				case "RichTextBox":
					RichTextBox rtb = (RichTextBox)ctl;
					rtb.ContextMenu = this.ctxRTF;
					rtb.KeyPress += rtbKeyPress;
					rtb.MouseEnter += rtbMouseEnter;
					//: AddHandler rtb.MouseLeave, AddressOf rtbMouseLeave
					rtb.SelectionChanged += rtbSelectionChanged;
					break;
				case "TextBox":
					TextBox tb = (TextBox)ctl;
					tb.KeyDown += Control_KeyDown;
					tb.KeyPress += Control_KeyPress;
					tb.Validating += TextBox_Validating;
					break;
			}
		}
		private void rtbMouseEnter(object sender, EventArgs e)
		{
			mActiveRichTextBox = (RichTextBox)sender;
			//Debug.Print("rtbMouseEnter")
		}
		//Private Sub rtbMouseLeave(sender As Object, e As EventArgs)
		//    mActiveRichTextBox = Nothing
		//    Debug.Print("rtbMouseLeave")
		//End Sub
		private void rtbKeyPress(object sender, KeyPressEventArgs e)
		{
            //Debug.Print KeyAscii
            if (e.KeyChar == Strings.ChrW(1))
            {
                //Ctrl-A/Select All...
                mActiveRichTextBox.SelectAll();
                e.Handled = true;
            }
            else if (e.KeyChar == Strings.ChrW(3))
            {
                //Ctrl-C/Copy...
                mActiveRichTextBox.Copy();
                e.Handled = true;
            }
            //Clipboard.SetDataObject(.SelectedText, True) : e.Handled = True
            else if (e.KeyChar == Strings.ChrW(22))
            {
                //Ctrl-V/Paste...
                //If .CanPaste(Clipboard.GetDataobject.GetFormats(true)(0) Then
                mActiveRichTextBox.Paste();
                e.Handled = true;
            }
            //.SelectedText = Clipboard.GetDataObject().GetData(DataFormats.Rtf) : e.Handled = True
            else if (e.KeyChar == Strings.ChrW(24))
            {
                //Ctrl-X/Cut...
                mActiveRichTextBox.Cut();
                e.Handled = true;
            }
            //Clipboard.SetDataObject(.SelectedText, False) : .SelectedText = Nothing : e.Handled = True
            else if (e.KeyChar == Strings.ChrW(25))
            {
                //Ctrl-Y/Redo...
            }
            else if (e.KeyChar == Strings.ChrW(26))
            {
                //Ctrl-Z/Undo...
            }
            else if (e.KeyChar == Strings.ChrW(2) || e.KeyChar == Strings.ChrW(9) || e.KeyChar == Strings.ChrW(21))
            {
                FontStyle newFontStyle = mActiveRichTextBox.SelectionFont.Style;
                if (e.KeyChar == Strings.ChrW(2))
                {
                    //Ctrl-B/Bold...
                    if ((newFontStyle & FontStyle.Bold) == FontStyle.Bold)
                        newFontStyle -= FontStyle.Bold;
                    else
                        newFontStyle |= FontStyle.Bold;
                }
                else if (e.KeyChar == Strings.ChrW(9))
                {
                    //Ctrl-I/Italic...
                    if ((newFontStyle & FontStyle.Italic) == FontStyle.Italic)
                        newFontStyle -= FontStyle.Italic;
                    else
                        newFontStyle |= FontStyle.Italic;
                }
                else if (e.KeyChar == Strings.ChrW(21))
                {
                    //Ctrl-U/Underline...
                    if ((newFontStyle & FontStyle.Underline) == FontStyle.Underline)
                        newFontStyle -= FontStyle.Underline;
                    else
                        newFontStyle |= FontStyle.Underline;
                }
                mActiveRichTextBox.SelectionFont = new Font(mActiveRichTextBox.SelectionFont, newFontStyle);
				e.Handled = true;
            }
		}
		private void rtbSelectionChanged(object sender, EventArgs e)
		{
			mnuRTFContextMenuParagraphAlignLeft.Checked = false;
			mnuRTFContextMenuParagraphAlignCenter.Checked = false;
			mnuRTFContextMenuParagraphAlignRight.Checked = false;
			switch (mActiveRichTextBox.SelectionAlignment) {
				case HorizontalAlignment.Left:
					mnuRTFContextMenuParagraphAlignLeft.Checked = true;
					break;
				case HorizontalAlignment.Center:
					mnuRTFContextMenuParagraphAlignCenter.Checked = true;
					break;
				case HorizontalAlignment.Right:
					mnuRTFContextMenuParagraphAlignRight.Checked = true;
					break;
			}
			mnuRTFContextMenuParagraphBullet.Checked = mActiveRichTextBox.SelectionBullet;
		}
		protected internal bool TagContains(object Tag, string Token)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			if ((Tag == null) || (Token == null))
				throw new ApplicationException("Tag argument is required!");
			if ((Token == null) || Token == bpeNullString)
				throw new ApplicationException("Token argument is required!");
			string[] Tokens = Convert.ToString(Tag).ToUpper().Split(new char[] {',',';'});
			for (short iToken = 0; iToken <= Tokens.Length - 1; iToken++) {
				if (Tokens[iToken] == Token.ToUpper()){functionReturnValue = true;return functionReturnValue;}
			}
			functionReturnValue = false;
			return functionReturnValue;
		}
		protected void TextBox_Validating(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				TextBox tb = (TextBox)sender;
				tb.Text = tb.Text.Trim();
				if (tb.Text == bpeNullString) {
					if ((tb.Tag == null ? "" : Convert.ToString(tb.Tag)).ToUpper().IndexOf("MONEY") >= 0) {
						tb.Text = "0.00";
					} else if ((tb.Tag == null ? "" : Convert.ToString(tb.Tag)).ToUpper().IndexOf("REQUIRED") >= 0) {
						throw new Exception("Value is required.");
					}
				} else if (!Information.IsNumeric(tb.Text) && (tb.Tag == null ? "" : Convert.ToString(tb.Tag)).ToUpper().IndexOf("NUMERIC") >= 0) {
					throw new Exception("Value must be numeric.");
				} else if (tb.TextLength > tb.MaxLength) {
					throw new Exception(string.Format("Length ({0}) exceeds maximum ({1}).", tb.TextLength, tb.MaxLength));
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected virtual void UnbindControl(CheckBox iControl, bool fEnable)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Checked"];
			if (tmpBinding != null) {
				tmpBinding.Format -= SmallIntToBoolean;
				tmpBinding.Parse -= BooleanToSmallInt;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(CheckedListBox iControl, bool fEnable)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["ItemCheck"];
			if (tmpBinding != null) {
				//RemoveHandler tmpBinding.Format, AddressOf SmallIntToBoolean
				//RemoveHandler tmpBinding.Parse, AddressOf BooleanToSmallInt
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(ComboBox iControl, bool fEnable, bool fClear)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3},{4})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable,
				fClear
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Text"];
			if (tmpBinding != null) {
				try {
					tmpBinding.Format -= NullToString;
				} catch (Exception ex) {
				}
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			if (fClear)
				((ComboBox)iControl).SelectedIndex = -1;
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(DataGrid iControl)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...

			//Before we go, save Column widths as user's preferences...
			if (iControl.TableStyles.Count > 0) {
				//For Each iColumn As DataGridColumnStyle In iControl.TableStyles(0).GridColumnStyles
				//    mFiRRe.SaveFiRReSetting(mSubKey, String.Format("{0}.ColumnWidth.{1}", iControl.TableStyles(0).MappingName, iColumn.MappingName), iColumn.Width)
				//Next
			}

			var _with7 = iControl;
			_with7.DataSource = null;
			_with7.BackgroundColor = SystemColors.InactiveCaptionText;
			//.CaptionText = ""
			//.CaptionBackColor = SystemColors.ActiveCaption
			_with7.TableStyles.Clear();
			_with7.ResetAlternatingBackColor();
			_with7.ResetBackColor();
			_with7.ResetForeColor();
			_with7.ResetGridLineColor();
			_with7.ResetHeaderBackColor();
			_with7.ResetHeaderFont();
			_with7.ResetHeaderForeColor();
			_with7.ResetSelectionBackColor();
			_with7.ResetSelectionForeColor();
			_with7.ResetText();
			//.BorderStyle = mDefaultGridBorderStyle
			//.Refresh()
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			ControlEnabled(iControl, false);
		}
		protected virtual void UnbindControl(DateTimePicker iControl, bool fEnable, bool fClear)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3},{4})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable,
				fClear
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Value"];
			if (tmpBinding != null) {
				tmpBinding.Format -= NullToDate;
				tmpBinding.Parse -= DateToNull;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			ControlEnabled(iControl, fEnable);
			if (fClear)
				((DateTimePicker)iControl).Text = bpeNullString;
		}
		protected virtual void UnbindControl(Label iControl, bool fEnable)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Text"];
			if (tmpBinding != null) {
				tmpBinding.Format -= NullToString;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(PictureBox iControl)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Image"];
			if (tmpBinding != null) {
				tmpBinding.Format -= BinaryToImage;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			ControlEnabled(iControl, true);
		}
		protected virtual void UnbindControl(RadioButton iControl, bool fEnable)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3},{4})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Checked"];
			if (tmpBinding != null) {
				tmpBinding.Format -= SmallIntToBoolean;
				tmpBinding.Parse -= BooleanToSmallInt;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(RichTextBox iControl, bool fEnable, bool fClear)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3},{4})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable,
				fClear
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			Binding tmpBinding = iControl.DataBindings["Rtf"];
			if (tmpBinding != null) {
				tmpBinding.Format -= NullToString;
				iControl.DataBindings.Remove(tmpBinding);
			}
			iControl.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			if (fClear)
				iControl.Text = bpeNullString;
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControl(TextBox iControl, bool fEnable, bool fClear)
		{
			const string EntryName = "UnbindControl";
			Trace("{0}({1}:={2},{3},{4})", new object[] {
				EntryName,
				"{"+Information.TypeName(iControl)+"}",
				iControl.Name,
				fEnable,
				fClear
			}, trcOption.trcBinding | trcOption.trcControls);
			//No error handler on purpose - let it fail up to the caller's handler...
			var _with8 = iControl;
			Binding tmpBinding = iControl.DataBindings["Text"];
			if (tmpBinding != null) {
                if (tmpBinding.BindingMemberInfo.BindingMember == mTCBase.TableIDColumn) {
					//Distinguish only for debugging purposes...
					tmpBinding.Format -= NullToID;
                } else {  
					tmpBinding.Format -= NullToString;
				}
				string strTag = (_with8.Tag == null ? "" : Convert.ToString(_with8.Tag));
				string[] tagParams = strTag.Split(",".ToCharArray());
				for (int i = 0; i <= tagParams.Length - 1; i++) {
					switch (tagParams[i].ToUpper()) {
						case "MONEY":
							tmpBinding.Format -= MoneyToString;
							tmpBinding.Parse -= StringToMoney;
							break;
						case "NULLS":
							break;
					}
				}
				_with8.DataBindings.Remove(tmpBinding);
			}
			_with8.DataBindings.Clear();
			//Just for good measure...
			if (this.Disposing)
				return;
			if (fClear)
				_with8.Text = bpeNullString;
			ControlEnabled(iControl, fEnable);
		}
		protected virtual void UnbindControls(Control.ControlCollection pControls, bool fEnable, bool fClear)
		{
			const string EntryName = "UnbindControls";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding | trcOption.trcControls);
				Trace("{0}({{ControlCollection}},{1})", new object[] {
					EntryName,
					fClear
				}, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcControls);
				foreach (Control iControl in pControls) {
					//UnbindControl(iControl, fEnable, fClear)
					switch (iControl.GetType().Name) {
						case "Button":			break;
						case "CheckBox":		UnbindControl((CheckBox)iControl, fEnable);							break;
						case "CheckedListBox":	UnbindControl((CheckedListBox)iControl, fEnable);					break;
						case "ComboBox":		UnbindControl((ComboBox)iControl, fEnable, fClear);					break;
						case "DataGrid":		UnbindControl((DataGrid)iControl);									break;
						case "DateTimePicker":	UnbindControl((DateTimePicker)iControl, fEnable, fClear);			break;
						case "Label":			UnbindControl((Label)iControl, fEnable);							break;
						case "GroupBox":		UnbindControls(((GroupBox)iControl).Controls, fEnable, fClear);		break;
						case "HScrollBar":		break;
						case "Panel":			UnbindControls(((Panel)iControl).Controls, fEnable, fClear);		break;
						case "PictureBox":		UnbindControl((PictureBox)iControl);								break;
						case "RadioButton":		UnbindControl((RadioButton)iControl, fEnable);						break;
						case "RichTextBox":		UnbindControl((RichTextBox)iControl, fEnable, fClear);				break;
						case "StatusBar":		break;
						case "StatusStrip":		break;
						case "TabControl":		UnbindControls(((TabControl)iControl).Controls, fEnable, fClear);	break;
						case "TabPage":			UnbindControls(((TabPage)iControl).Controls, fEnable, fClear);		break;
						case "TextBox":			UnbindControl((TextBox)iControl, fEnable, fClear);					break;
						case "ToolBar":			break;
						default:
							//Throw New Exception(String.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", TypeName(iControl), EntryName, iControl.Name))
							Trace(string.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", Information.TypeName(iControl), EntryName, iControl.Name), trcOption.trcApplication | trcOption.trcBinding | trcOption.trcControls);
							try {
								iControl.DataBindings.Clear();
							} catch (Exception ex) {
							}
							break;
					}
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication | trcOption.trcBinding | trcOption.trcControls);
			}
		}
		#region "Utility Methods"
		protected virtual void Busy(bool IsBusy)
		{
			Cursor myCursor = (IsBusy ? Cursors.WaitCursor : Cursors.Default);
			bool fEnabled = !IsBusy;
			var _with9 = this;
			_with9.Cursor = myCursor;
			_with9.Enabled = fEnabled;
		}
		public void cbAddItem(ComboBox cb, string item)
		{
			foreach (string iItem in cb.Items) {
				if (iItem.CompareTo(item) == 0)
					return;
			}
			cb.Items.Add(item);
		}
		private int cbFind(ComboBox cb, string item)
		{
			int functionReturnValue = 0;
			bool found = false;
            for (int i = 0; i <= cb.Items.Count - 1 && !found; i++) {
				if (item == Strings.Trim((string)cb.Items[i])) {
					functionReturnValue = i;
					found = true;
				}
			}
			if (!found)
				functionReturnValue = -1;
			return functionReturnValue;
		}
		public void cbValidate(ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...

			//Determine if we are Simple or Complex bound...
			if ((sender.DataBindings["SelectedValue"] == null)) {
				cbValidateSimple(sender, e);
			} else {
				cbValidateComplex(sender, e);
			}
		}
		private void cbValidateComplex(ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...
			Binding dataBinding = sender.DataBindings["SelectedValue"];
			DataView dataSource = (DataView)dataBinding.DataSource;
			string dataMember = sender.ValueMember;
			//dataBinding.BindingMemberInfo.BindingMember
			object dataValue = dataSource[sender.BindingContext[dataSource].Position][dataMember];
			DataView displaySource = (DataView)sender.DataSource;
			string displayMember = sender.DisplayMember;
			string displayValue = Strings.Trim(sender.Text);

			if (displayValue == bpeNullString && (sender.Tag == null ? "" : Convert.ToString(sender.Tag)).ToUpper().IndexOf("REQUIRED") >= 0) {
				throw new Exception("Value is required.");
			} else if ((sender.SelectedItem == null)) {
				//Something was entered, but not selected from the list... See if it matches something already in the list,
				//otherwise prompt the user if he wants it added...
				for (int i = 0; i <= displaySource.Count - 1; i++) {
					if (!Information.IsDBNull(displaySource[i][displayMember])) {
						if (displayValue == Strings.Trim((string)displaySource[i][displayMember])) {
							//We found the entry in the list, so make it look like they clicked this one...
							sender.SelectedIndex = i;
							return;
						}
					}
				}
				//OK, it's not in the list... Ask if it should be added...
				if (MessageBox.Show(this, "\"" + displayValue + "\" isn't in the list... Do you want it added...?", "Confirm Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
					sender.Text = bpeNullString;
					throw new Exception("Select an item from the list.");
				}
				//OK, add it to the list...
				DataRowView newRow = displaySource.AddNew();
				newRow[displayMember] = displayValue;
				Trace("newRow.EndEdit()", trcOption.trcDB | trcOption.trcApplication);
				newRow.EndEdit();
				Trace("displaySource.Table.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
				displaySource.Table.AcceptChanges();
				sender.SelectedIndex = displaySource.Find(displayValue);
			}
		}
		private void cbValidateSimple(ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...
			Binding dataBinding = sender.DataBindings["Text"];
			DataView displaySource = (DataView)sender.DataSource;
			string displayMember = sender.DisplayMember;
			string displayValue = Strings.Trim(sender.Text);

			if (displayValue == bpeNullString) {
				throw new Exception("Value is required.");
			} else if ((sender.SelectedItem == null)) {
				//Something was entered, but not selected from the list... See if it matches something already in the list,
				//otherwise add it...
				sender.SelectedIndex = cbFind(sender, displayValue);
				if (sender.SelectedIndex == -1) {
					//OK, add it to the list...
					cbAddItem(sender, displayValue);
					sender.SelectedIndex = cbFind(sender, displayValue);
				}
			}
		}
		public void GenericErrorHandler(Exception exception, bool fLogToScreen, string ExtraMessage = bpeNullString, Control ctl = null)
		{
			//Note: This routine is intended for use by host applications. Internal error handling is done through InternalErrorHandler...
			//      The typical calling sequence is as follows...
			//       - Deepest routine calls RaiseError which registers the error in clsErrors and throws the exception to its caller...
			//       - Next Deepest routine follows suit...
			//           .
			//           .
			//           .
			//       - Eventually the error gets bubbles-up to a top-level routine responsible for UI... That routine calls 
			//         GenericErrorHandler with the exception.
			//         GenericErrorHandler then formats the top-most exception, and reports each of the underlying exceptions. 
			//         The clsErrors collection is then cleared resetting the mechanism for the next run...

			try {
				string Caption = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				string ScreenName = bpeNullString;
				if ((this != null)){Caption = this.Text;ScreenName = this.Name;}
				string Message = string.Format("{0} ({1})", exception.Message, exception.GetType().Name);
				string Detail = string.Format("{1} raised.{0}{2}{0}{0}StackTrace:{0}{3}{0}{0}", new object[] {
					Constants.vbCrLf,
					exception.GetType().Name,
					exception.Message,
					exception.StackTrace
				});
				if ((exception.InnerException != null)) {
					Detail += string.Format("Underlying Error(s):{0}", Constants.vbCrLf);
					Exception iException = exception.InnerException;
					while ((iException != null)) {
						Detail += string.Format("{1} raised.{0}{2}{0}{0}StackTrace:{0}{3}{0}{0}", new object[] {
							Constants.vbCrLf,
							iException.GetType().Name,
							iException.Message,
							iException.StackTrace
						});
						iException = iException.InnerException;
					}
				}

				if (fLogToScreen)
					ShowError(Message, Detail, MsgBoxStyle.Exclamation, this, string.Format("{0} ({1})", Caption, exception.GetType().Name));
				//ShowMsgBox(ex.Message, MsgBoxStyle.OKOnly, Me, ex.GetType.Name)
				if ((ctl != null) && ctl.Enabled && ctl.Visible)
					ctl.Focus();
			} catch (Exception ex) {
			} finally {
				if (mTCBase.ActiveTXLevel > 0)
					mTCBase.AbortTrans();
			}
		}
		public ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo functionReturnValue = null;
			functionReturnValue = null;
			int i = 0;
			ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageEncoders();
			for (i = 0; i <= Encoders.Length - 1; i++) {
				if (Encoders[i].MimeType == mimeType) {
					functionReturnValue = Encoders[i];
					return functionReturnValue;
				}
			}
			functionReturnValue = null;
			return functionReturnValue;
		}
		private void ReSort(ref object pRecordSet)
		{
			//TODO: Determine if ReSort is obsolete...
			//        Const EntryName As String = "ReSort"
			//        Dim pRS As ADODB.Recordset
			//        Dim SaveID As Integer
			//        '    Dim DBinfo As clsDataBaseInfo
			//        '    Dim FieldList As String
			//        '    Dim TableList As String
			//        '    Dim WhereClause As String
			//        '    Dim OrderByClause As String

			//        On Error GoTo ErrorHandler

			//        pRS = pRecordSet
			//        '    For Each DBinfo In mDBCollection
			//        '        If DBinfo.Recordset Is pRS Then
			//        '          ParseSQLSelect(DBinfo.SQLSource, FieldList, TableList, WhereClause, OrderByClause)
			//        '            pRS.Sort = OrderByClause
			//        '            Exit For
			//        '        End If
			//        '    Next
			//        SaveID = RecordID
			//        pRS.Sort = mOrderByClause
			//        FindID(pRS, SaveID)
			//        If pRS.EOF Then Call ClearCommand()

			//        'UPGRADE_NOTE: Object pRS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        pRS = Nothing
			//        Exit Sub

			//ErrorHandler:
			//        RaiseError()
		}
		public void rtfControl_KeyPress(ref object ctl, ref short KeyAscii)
		{
			//        Const EntryName As String = "rtfControl_KeyPress"
			//        Dim rtfControl As AxRichTextLib.AxRichTextBox

			//        On Error GoTo ErrorHandler
			//        rtfControl = ctl


			//        'Debug.Print KeyAscii
			//        If KeyAscii = 1 Then 'Ctrl-A/Select All...
			//            rtfControl.SelStart = 0
			//            rtfControl.SelLength = Len(rtfControl.TextRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 2 Then  'Ctrl-B/Bold...
			//            rtfControl.SelBold = Not rtfControl.SelBold
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 3 Then  'Ctrl-C/Copy...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.SetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            VB.Clipboard.SetText(rtfControl.SelRTF, vbCFRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 9 Then  'Ctrl-I/Italic...
			//            rtfControl.SelItalic = Not rtfControl.SelItalic
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 21 Then  'Ctrl-U/Underline...
			//            rtfControl.SelUnderline = Not rtfControl.SelUnderline
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 22 Then  'Ctrl-V/Paste...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.GetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            rtfControl.SelRTF = VB.Clipboard.GetText(vbCFRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 24 Then  'Ctrl-X/Cut...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.SetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            VB.Clipboard.SetText(rtfControl.SelRTF, vbCFRTF)
			//            rtfControl.SelRTF = bpeNullString
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 26 Then  'Ctrl-Y/Redo...

			//            KeyAscii = 0
			//        ElseIf KeyAscii = 26 Then  'Ctrl-Z/Undo...

			//            KeyAscii = 0
			//        End If

			//ExitSub:
			//        'UPGRADE_NOTE: Object rtfControl may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        rtfControl = Nothing
			//        Exit Sub

			//ErrorHandler:
			//        RaiseError()
		}
		public MsgBoxResult ShowError(string Message, string Details, MsgBoxStyle MsgBoxStyle, Form Parent = null, string Caption = bpeNullString, System.Drawing.Icon Icon = null)
		{
			MsgBoxResult functionReturnValue = default(MsgBoxResult);
			frmError frm = null;
			functionReturnValue = MsgBoxResult.Cancel;
			try {
				frm = new frmError(mTCBase, Parent);

				var _with10 = frm;
				_with10.ShowInTaskbar = false;
				_with10.lblDetails.Visible = true;
				//Default from Parent Form...
				if ((Parent != null)) {
					_with10.Icon = ((Icon != null) ? Icon : Parent.Icon);
					_with10.Text = (Caption != bpeNullString ? Caption : Parent.Text);
				//Default from project/assembly (if possible)...
				} else {
					if ((Icon != null)) {
						_with10.Icon = Icon;
					} else if (mSupport.EntryComponentName != bpeNullString) {
						Trace(mMyTraceID + " Defaulting .Icon from project/assembly...", trcOption.trcSupport);
						_with10.Icon = mSupport.Icon(string.Format("{0}\\{1}.exe", mSupport.ApplicationPath, mSupport.EntryComponentName));
					}
					_with10.Text = (Caption != bpeNullString ? Caption : mSupport.ApplicationName);
				}
				_with10.Message = Message;
				_with10.Details = Details;
				_with10.MsgBoxStyle = MsgBoxStyle;
				Trace(mMyTraceID + "Calling ShowError.ShowDialog()", trcOption.trcSupport);
				_with10.StartPosition = FormStartPosition.CenterParent;
				_with10.ShowDialog(Parent);
				functionReturnValue = _with10.MsgBoxResult;
				_with10.OKtoClose = true;
				Trace(mMyTraceID + "Calling ShowError.Close()", trcOption.trcSupport);
				_with10.Close();
			} finally {
				frm = null;
			}
			return functionReturnValue;
		}
		public string StripBraces(string Source, string Delimiter = "{")
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			functionReturnValue = Source;
			if (Source.StartsWith(Delimiter))
				functionReturnValue = Source.Substring(1, Source.Length - 2);
			return functionReturnValue;
		}
		#endregion
		#endregion
		#region "Debugging Tools"
		private StreamWriter dumper = null;
		private short funit = 0;
		public void DumpBindings(CurrencyManager cm = null)
		{
			string Message = string.Format("Dumping Bindings for {0}...", this.Name);
			Trace(Message, trcOption.trcBinding);
			if (!TraceMode)
				PrintOut(Message);
			if (cm == null)
				cm = mCurrencyManager;
			foreach (Binding iBinding in cm.Bindings) {
				Message = GetBindingInfo(iBinding);
				Trace(Message, trcOption.trcBinding);
				if (!TraceMode)
					PrintOut(Message);
			}
		}
		protected void DumpControl(Control ctl)
		{
			const string EntryName = "DumpControl";
			switch (Information.TypeName(ctl)) {
				case "Button":			break;
				case "CheckBox":		break;
				case "ComboBox":		break;
				case "DateTimePicker":	break;
				case "Form":			break;
				case "GroupBox":		DumpControls(((GroupBox)ctl).Controls); break;
				case "Label":			break;
				case "PictureBox":		break;
				case "RichTextBox":		break;
				case "StatusBar":		break;
				case "StatusStrip":		break;
				case "TextBox":			break;
				case "TabControl":		DumpControls(((TabControl)ctl).Controls); break;
				case "TabPage":			DumpControls(((TabPage)ctl).Controls); break;
				case "ToolBar":			break;
				default:				throw new Exception(string.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", Information.TypeName(ctl), EntryName, ctl.Name));
			}
		}
		protected void DumpControls(Form pForm)
		{
			try {
				foreach (Control ctl in pForm.Controls) {
					DumpControl(ctl);
				}
			} catch (Exception e) {
				Debug.WriteLine(e.Message);
			}
		}
		protected internal void DumpControls(Control.ControlCollection pControls)
		{
			foreach (Control ctl in pControls) {
				DumpControl(ctl);
			}
		}
		private string FormatData(DataColumn Column, object Data, bool fDelimitted = false)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			if (Information.IsDBNull(Data)){functionReturnValue = "Null";return functionReturnValue;}

			SqlDbType dType = MapSystemToSQLDBType(Column.DataType);
			switch (dType) {
				case SqlDbType.BigInt:
				case SqlDbType.Int:
				case SqlDbType.SmallInt:
				case SqlDbType.TinyInt:
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Bit:
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					functionReturnValue = string.Format((fDelimitted ? "\"{0}\"" : "{0}"), Data);
					break;
				case SqlDbType.DateTime:
					//, SqlDbType.Date
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Money:
				case SqlDbType.Real:
				case SqlDbType.SmallMoney:
					functionReturnValue = Data.ToString();
					break;
				default:
					functionReturnValue = string.Format("[{0}]", dType.ToString());
					break;
			}
			//Apparently with .NET 2.0 "" is now literally a Null string (one character - that being CHR(0)) - P.S. vbNullString seems now to match bpeNullString
			if (functionReturnValue.Length == 1 && Strings.Asc(functionReturnValue.Substring(0, 1)) == 0)
				functionReturnValue = bpeNullString;
			return functionReturnValue;
		}
		private SqlDbType MapSystemToSQLDBType(System.Type Type)
		{
			switch (Type.Name) {
				case "Boolean":
					return SqlDbType.Bit;
				case "Byte":
					return SqlDbType.TinyInt;
				case "Byte[]":
					//or SqlDbType.Binary
					return SqlDbType.Image;
				case "DateTime":
					return SqlDbType.DateTime;
				case "Decimal":
					//or SqlDbType.Decimal, SqlDbType.SmallMoney
					return SqlDbType.Money;
				case "Double":
					return SqlDbType.Float;
				case "Int64":
					return SqlDbType.BigInt;
				case "Int32":
					return SqlDbType.Int;
				case "Int16":
					return SqlDbType.SmallInt;
				case "Single":
					return SqlDbType.Real;
				case "String":
					// or SqlDbType.VarChar, SqlDbType.NText, SqlDbType.Text
					return SqlDbType.NVarChar;
				default:
					return SqlDbType.VarChar;
			}
		}
		private void PrintOut(string data)
		{
			if ((dumper != null)) {
				dumper.WriteLine(data);
			} else if (funit != 0) {
				FileSystem.PrintLine(funit, data);
			} else {
				Debug.WriteLine(data);
			}
		}
		public void Dump(DataRow dr, string FileName = null)
		{
			this.DumpDataRow(dr, FileName);
		}
		public void Dump(DataRow dr, DataRowVersion Version, string FileName = null)
		{
			this.DumpDataRow(dr, Version, FileName);
		}
		public void Dump(DataRowView drv, CurrencyManager cm = null, string FileName = null)
		{
			this.DumpDataRowView(drv, cm, FileName);
		}
		public void Dump(DataTable dt, string FileName = null)
		{
			this.DumpDataTable(dt, FileName);
		}
		public void Dump(DataView dv, CurrencyManager cm = null, string FileName = null)
		{
			this.DumpDataView(dv, cm, FileName);
		}
		private void DumpDataTable(DataTable dt, string FileName = bpeNullString)
		{
			int[] ColLen = null;
			try {
				if (FileName != bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((dt == null)) {
                    PrintOut("DataTable is Nothing");
                    throw new ExitTryException();
                }

				ColLen = new int[dt.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dt.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (int iRow = 0; iRow <= dt.Rows.Count - 1; iRow++) {
					DataRow dr = dt.Rows[iRow];
					for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
						int newLength = FormatData(dt.Columns[iColumn], dr[iColumn]).Length;
						if (newLength > ColLen[iColumn])
							ColLen[iColumn] = newLength;
					}
				}
				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

				PrintOut(string.Format("Dumping DataTable {0} (DefaultView: Count: {1:#,##0})...", dt.TableName, dt.Rows.Count));
				string strOut = "Record   RowState  ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					string ColumnName = dt.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				for (int iRow = 0; iRow <= dt.Rows.Count - 1; iRow++) {
					DataRow dr = dt.Rows[iRow];
					strOut = string.Format("[{0:000000}] {1}{2} ", iRow, dr.RowState.ToString(), new string(' ', "Unchanged".Length - dr.RowState.ToString().Length));
					for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
						string Data = FormatData(dt.Columns[iColumn], dr[iColumn]);
						strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
					}
					PrintOut(strOut);
				}
				PrintOut(string.Format("{0}{1:#,##0} Records", Constants.vbCr, dt.Rows.Count));
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataRow(DataRow dr, string FileName = bpeNullString)
		{
			this.DumpDataRow(dr, DataRowVersion.Current, FileName);
		}
		private void DumpDataRow(DataRow dr, DataRowVersion Version, string FileName = bpeNullString)
		{
			int[] ColLen = null;
			DataTable dt = null;
			int iRow = -1;
			try {
				if (FileName != bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((dr == null)) {
                    PrintOut("DataRow is Nothing");
                    throw new ExitTryException();
                }
				dt = dr.Table;

				ColLen = new int[dt.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dt.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					int newLength = FormatData(dt.Columns[iColumn], dr[iColumn]).Length;
					if (newLength > ColLen[iColumn])
						ColLen[iColumn] = newLength;
				}
				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

                //Find our row in the collection...
                bool continueLoop = true;
                for (int i = 0; i <= dt.Rows.Count - 1 && continueLoop; i++) {
					DataRow drRow = dt.Rows[i];
					if (object.ReferenceEquals(drRow, dr)) {
                        iRow = i;
                        continueLoop = false;
                    }
                }

				PrintOut(string.Format("Dumping DataRow {0} ({1:#,##0} of {2:#,##0})...", dt.TableName, iRow, dt.DefaultView.Count));
				PrintOut(string.Format("RowVersion {0}", Version.ToString()));
				string strOut = "Record   RowState  ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					string ColumnName = dt.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				strOut = string.Format("[{0:000000}] {1}{2} ", iRow, dr.RowState.ToString(), new string(' ', "Unchanged".Length - dr.RowState.ToString().Length));
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					string Data = FormatData(dt.Columns[iColumn], dr[iColumn, Version]);
					strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
				}
				PrintOut(strOut);
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataRowView(DataRowView drv, CurrencyManager cm = null, string FileName = bpeNullString)
		{
			int[] ColLen = null;
			DataView dv = null;
			int iRow = -1;
			try {
				int iPos = 0;
				if ((cm != null))
					iPos = cm.Position;
				if (FileName != bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((drv == null)) {
                    PrintOut("DataRowView is Nothing"); 
                    throw new ExitTryException();
                }
				dv = drv.DataView;

				ColLen = new int[dv.Table.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dv.Table.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					int newLength = FormatData(dv.Table.Columns[iColumn], drv[iColumn]).Length;
					if (newLength > ColLen[iColumn])
						ColLen[iColumn] = newLength;
				}

				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

                //Find our row in the collection...
                bool continueLoop = false;
				for (int i = 0; i <= dv.Count - 1 && continueLoop; i++) {
					DataRowView drvRowView = dv[i];
					if (object.ReferenceEquals(drvRowView, drv)) {
                        iRow = i;
                        continueLoop = false;
                    }
                }

				PrintOut(string.Format("Dumping DataRowView {0} ({1:#,##0} of {2:#,##0})...", dv.Table.TableName, iRow, dv.Count));
				PrintOut(string.Format("RowFilter:      {0}{1}", Constants.vbTab, dv.RowFilter));
				PrintOut(string.Format("Sort:           {0}{1}", Constants.vbTab, dv.Sort));
				PrintOut(string.Format("RowStateFilter: {0}{1}", Constants.vbTab, dv.RowStateFilter));
				string strOut = "Record   RowVersion ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string ColumnName = dv.Table.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				string fmt = ((cm == null) ? "[{0:000000}] {1}{2} " : "[->{0:0000}] {1}{2} ");
				strOut = string.Format(fmt, iRow, drv.RowVersion, new string(' ', "RowVersion".Length - drv.RowVersion.ToString().Length));
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string Data = FormatData(dv.Table.Columns[iColumn], drv[iColumn]);
					strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
				}
				PrintOut(string.Format("{0} (RowState: {1}; {2}{3})", new object[] {
					strOut,
					drv.Row.RowState.ToString(),
					(drv.IsEdit ? " IsEdit; " : ""),
					(drv.IsNew ? " IsNew; " : "")
				}));
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataView(DataView dv, CurrencyManager cm = null, string FileName = bpeNullString)
		{
			int[] ColLen = null;
			try {
				int iPos = 0;
				if ((cm != null))
					iPos = cm.Position;
				if (FileName != bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((dv == null)) {
                    PrintOut("DataView is Nothing"); 
                    throw new ExitTryException();
                }

				ColLen = new int[dv.Table.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dv.Table.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (int iRow = 0; iRow <= dv.Count - 1; iRow++) {
					DataRowView drv = dv[iRow];
					for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
						int newLength = FormatData(dv.Table.Columns[iColumn], drv[iColumn]).Length;
						if (newLength > ColLen[iColumn])
							ColLen[iColumn] = newLength;
					}
				}
				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

				PrintOut(string.Format("Dumping DataView {0} {1:#,##0} Records...", dv.Table.TableName, dv.Count));
				PrintOut(string.Format("RowFilter:      {0}{1}", Constants.vbTab, dv.RowFilter));
				PrintOut(string.Format("Sort:           {0}{1}", Constants.vbTab, dv.Sort));
				PrintOut(string.Format("RowStateFilter: {0}{1}", Constants.vbTab, dv.RowStateFilter));
				string strOut = "Record   RowVersion ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string ColumnName = dv.Table.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				for (int iRow = 0; iRow <= dv.Count - 1; iRow++) {
					DataRowView drv = dv[iRow];
					string fmt = ((cm == null) ? "[{0:000000}] {1}{2} " : "[->{0:0000}] {1}{2} ");
					strOut = string.Format(fmt, iRow, drv.RowVersion, new string(' ', "RowVersion".Length - drv.RowVersion.ToString().Length));
					for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
						string Data = FormatData(dv.Table.Columns[iColumn], drv[iColumn]);
						strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
					}
					PrintOut(string.Format("{0} (RowState: {1}; {2}{3})", new object[] {
						strOut,
						drv.Row.RowState.ToString(),
						(drv.IsEdit ? " IsEdit; " : ""),
						(drv.IsNew ? " IsNew; " : "")
					}));
				}
				PrintOut(string.Format("{0}{1:#,##0} Records", Constants.vbCr, dv.Count));
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		#endregion
		#endregion
		#region "Event Handlers"
		public void CurrencyManager_CurrentChanged(object sender, System.EventArgs e)
		{
			const string EntryName = "CurrencyManager_CurrentChanged";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding);
				CurrencyManager cm = (CurrencyManager)sender;
				if ((!object.ReferenceEquals(cm.Current.GetType(), typeof(DataRowView)))){Trace("Current dataSource is not DataRowView - Bugging-Out...", trcOption.trcBinding);return;
}
				Trace("Handling {0} event from {1}; Current Position is Row #{2}", EntryName, sender.ToString(), cm.Position, trcOption.trcBinding);
				if (TraceMode)
					DumpBindings(cm);
				if (mTCBase.Mode == ActionModeEnum.modeDisplay && mTCBase.CurrentRow != null && mTCBase.CurrentRow.IsEdit)
					mTCBase.CurrentRow.CancelEdit();
				//TODO: Don't know why in Display mode our iRow.IsEdit is true, but deal with it here...
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding);
			}
		}
		public void CurrencyManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			const string EntryName = "CurrencyManager_ItemChanged";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding);
				CurrencyManager cm = (CurrencyManager)sender;
				if ((!object.ReferenceEquals(cm.Current.GetType(), typeof(DataRowView)))){Trace("Current dataSource is not DataRowView - Bugging-Out...", trcOption.trcBinding);return;
}
				Trace("Handling {0} event from {1}; Item @ Index #{2} has changed", EntryName, sender.ToString(), e.Index, trcOption.trcBinding);
				foreach (Binding iBinding in cm.Bindings) {
					if (iBinding.BindingMemberInfo.BindingMember == mTCBase.TableIDColumn){Trace(GetBindingInfo(iBinding), trcOption.trcApplication);break; 
}
				}
				if (mTCBase.Mode == ActionModeEnum.modeDisplay && mTCBase.CurrentRow != null && mTCBase.CurrentRow.IsEdit)
					mTCBase.CurrentRow.CancelEdit();
				//TODO: Don't know why in Display mode our iRow.IsEdit is true, but deal with it here...
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding);
			}
		}
		public void CurrencyManager_PositionChanged(object sender, System.EventArgs e)
		{
			const string EntryName = "CurrencyManager_PositionChanged";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcBinding);
				CurrencyManager cm = (CurrencyManager)sender;
				Trace("Handling {0} event from {1}; Positioned to Row #{2}", EntryName, sender.ToString(), cm.Position, trcOption.trcApplication);
				//Dim iPosition As Integer = cm.Position
				//iRow = tcDataView.Item(iPosition)
				//OnRowChange(iPosition, iRow, tcDataView.Count)
				foreach (Binding iBinding in cm.Bindings) {
					if (iBinding.BindingMemberInfo.BindingMember == mTCBase.TableIDColumn){Trace(GetBindingInfo(iBinding), trcOption.trcBinding);break; 
}
				}
				if (mTCBase.Mode == ActionModeEnum.modeDisplay && mTCBase.CurrentRow != null && mTCBase.CurrentRow.IsEdit)
					mTCBase.CurrentRow.CancelEdit();
				//TODO: Don't know why in Display mode our iRow.IsEdit is true, but deal with it here...
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcBinding);
			}
		}
		protected virtual void mnuFileTrace_Click(object sender, System.EventArgs e)
		{
			var _with11 = (MenuItem)sender;
			if (!_with11.Checked) {
				mSupport.Trace.ShowTraceOptions();
				if (mSupport.Trace.TraceMode) {
					mTCBase.Trace(new string('=', 132), trcOption.trcAll);
					mTCBase.Trace("Trace File Opened - " + mSupport.Trace.TraceFile, trcOption.trcAll);
				}
			} else {
				mTCBase.Trace("Trace File Closed.", trcOption.trcAll);
				mTCBase.Trace(new string('=', 132), trcOption.trcAll);
				mSupport.Trace.TraceMode = false;
			}
			_with11.Checked = mSupport.Trace.TraceMode;
			((clsIconMenuItem)sender).Icon = (_with11.Checked ? clsTCBase.ImageToIcon(this.imgBase.Images[(int)imgMainEnum.Trace]) : null);
		}
		public void mTCBase_UnbindControls(object sender, System.EventArgs e)
		{
			UnbindControls(this.Controls, false, false);
		}
		//Private Sub mTCBase_RowChange(ByVal sender As Object, ByVal e As RowChangeEventArgs) Handles mTCBase.RowChange
		//End Sub
		//Private Sub mTCBase_FilterChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles mTCBase.FilterChange
		//End Sub
		//Private Sub mTCBase_ActionModeChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles mTCBase.ActionModeChange
		//End Sub
		private void mnuRTFContextMenuCopy_Click(object sender, EventArgs e)
		{
			rtbKeyPress(mActiveRichTextBox, new KeyPressEventArgs(Strings.ChrW(3)));
		}
		private void mnuRTFContextMenuCut_Click(object sender, EventArgs e)
		{
			rtbKeyPress(mActiveRichTextBox, new KeyPressEventArgs(Strings.ChrW(24)));
		}
		private void mnuRTFContextMenuFontChange_Click(object sender, EventArgs e)
		{
			var _with12 = dlgFont;
			_with12.ShowApply = false;
			_with12.ShowHelp = false;
			_with12.Font = mActiveRichTextBox.SelectionFont;
			_with12.ShowEffects = true;
			_with12.Color = mActiveRichTextBox.SelectionColor;
			_with12.ShowColor = true;
			DialogResult dlgResult = _with12.ShowDialog(this);
			switch (dlgResult) {
				case DialogResult.OK:
					mActiveRichTextBox.SelectionFont = _with12.Font;
					mActiveRichTextBox.SelectionColor = _with12.Color;
					break;
			}
		}
		private void mnuRTFContextMenuFontDefault_Click(object sender, EventArgs e)
		{
			mActiveRichTextBox.SelectionFont = new Font("Verdana", 10f);
		}
		private void mnuRTFContextMenuParagraphAlignCenter_Click(object sender, EventArgs e)
		{
			mnuRTFContextMenuParagraphAlignLeft.Checked = false;
			mnuRTFContextMenuParagraphAlignCenter.Checked = true;
			mnuRTFContextMenuParagraphAlignRight.Checked = false;
			mActiveRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
		}
		private void mnuRTFContextMenuParagraphAlignLeft_Click(object sender, EventArgs e)
		{
			mnuRTFContextMenuParagraphAlignLeft.Checked = true;
			mnuRTFContextMenuParagraphAlignCenter.Checked = false;
			mnuRTFContextMenuParagraphAlignRight.Checked = false;
			mActiveRichTextBox.SelectionAlignment = HorizontalAlignment.Left;
		}
		private void mnuRTFContextMenuParagraphAlignRight_Click(object sender, EventArgs e)
		{
			mnuRTFContextMenuParagraphAlignLeft.Checked = false;
			mnuRTFContextMenuParagraphAlignCenter.Checked = false;
			mnuRTFContextMenuParagraphAlignRight.Checked = true;
			mActiveRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
		}
		private void mnuRTFContextMenuParagraphBullet_Click(object sender, EventArgs e)
		{
			if (mActiveRichTextBox.SelectionIndent == 0 && !mActiveRichTextBox.SelectionBullet)
				this.mnuRTFContextMenuParagraphIndent_Click(sender, e);
            mActiveRichTextBox.BulletIndent = (int)(0.25 * mDPI);
            mActiveRichTextBox.SelectionBullet = !mActiveRichTextBox.SelectionBullet;
		}
		private void mnuRTFContextMenuParagraphIndent_Click(object sender, EventArgs e)
		{
            mActiveRichTextBox.SelectionIndent += (int)(0.25 * mDPI);
		}
		private void mnuRTFContextMenuParagraphUnindent_Click(object sender, EventArgs e)
		{
			if (mActiveRichTextBox.SelectionIndent > (int)(0.25 * mDPI))
                mActiveRichTextBox.SelectionIndent -= (int)(0.25 * mDPI);
			else
                mActiveRichTextBox.SelectionIndent = 0;
		}
		private void mnuRTFContextMenuPaste_Click(object sender, EventArgs e)
		{
			rtbKeyPress(mActiveRichTextBox, new KeyPressEventArgs(Strings.ChrW(22)));
		}
		private void mnuRTFContextMenuSelectAll_Click(object sender, EventArgs e)
		{
			rtbKeyPress(mActiveRichTextBox, new KeyPressEventArgs(Strings.ChrW(1)));
		}
		private void ctxRTF_Popup(object sender, EventArgs e)
		{
			if (mActiveRichTextBox != null && !mActiveRichTextBox.Focused)
				mActiveRichTextBox.Focus();
		}
        protected virtual void Form_Layout(object sender, LayoutEventArgs e)
        {
            if (DesignMode) return;
            try
            {
                base.epBase.SetError((Control)sender, bpeNullString);
                base.SuspendLayout();
                mFormHeight = base.ClientRectangle.Height;
                mFormWidth = base.ClientRectangle.Width;

                base.ResumeLayout();
            } catch (Exception ex) {
                base.epBase.SetError((Control)sender, ex.Message);
            }
        }
        protected virtual void Form_Load(object sender, EventArgs e)
        {
            mFormHeight = base.ClientRectangle.Height;
            mFormWidth = base.ClientRectangle.Width;
        }
        #endregion
    }
}
