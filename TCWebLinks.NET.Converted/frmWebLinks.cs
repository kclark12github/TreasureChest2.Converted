using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using TCBase.clsRegistry;
using TCBase.clsSupport;
using TCBase.clsTrace;
//frmWebLinks.vb
//   Web Links Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/18/16    Reworked to reflect architectural changes;
//   08/07/10    Converted to VB.NET;
//=================================================================================================================================
//Note:
//   frmWebLinks is not a typical TreasureChest2 screen in that its purpose is not database record manipulation, at least not in 
//   same manner as the rest of the screens in the application. This screen simply does not fit the frmTCStandard mold. As 
//   such, many of the built-in features the other screens can take for granted have to be handled directly in this screen.
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCWebLinks
{
	public class frmWebLinks : TCBase.frmTCBase
	{
		const string myFormName = "frmWebLinks";
		public frmWebLinks(clsSupport objSupport, clsWebLinks objBase, Form objParent = null, string Caption = null) : base(objSupport, myFormName, objBase, objParent)
		{
			Closing += frmWebLinks_Closing;
			Activated += frmWebLinks_Activated;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			mRegistryKey = string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, myFormName);
			//Load Initial TreeView Nodes representing buttons (and their immediate children)...
			var _with1 = tvwDB;
			_with1.Nodes.Clear();
			rootNode = new TreeNode("Web Menu Buttons", imlIconsEnum.WebButtons, imlIconsEnum.WebButtons);
			int rootIndex = _with1.Nodes.Add(rootNode);

			_with1.BeginUpdate();
			for (tvwDBEnum iButton = tvwDBEnum.Aircraft; iButton <= tvwDBEnum.Web; iButton++) {
				string SQLstatement = string.Format("Select * From MenuEntries Where ButtonLabel='{0}' and ParentID=0 Order By ButtonLabel, ParentID, Label", iButton.ToString());
				DataView dvEntries = mTCBase.MakeDataViewCommand(SQLstatement, "MenuEntries", false);
				if (dvEntries.Count > 0) {
					TreeNode buttonNode = new TreeNode(iButton.ToString(), imlIconsEnum.WebButton, imlIconsEnum.WebButton);
					buttonNode.Tag = "Button: 0";
					for (long i = 0; i <= dvEntries.Count - 1; i++) {
						DataRowView iRow = dvEntries[i];
						Trace("Processing Entry: ButtonLabel: {0}; ID: {1}; Label: {2}", iRow["ButtonLabel"], iRow["ID"], iRow["Label"], trcOption.trcApplication);

						AddNode(buttonNode, iRow["ID"], VBdecode(iRow["Label"]), iButton.ToString(), VBdecode(iRow["ParentID"]), iRow["HasMembers"], false);
					}
					rootNode.Nodes.Add(buttonNode);
				}
				dvEntries.Dispose();
				dvEntries = null;
			}
			_with1.EndUpdate();
			EnableMyControls(false);
			lblLoad.Visible = false;
			lblLoad.Text = bpeNullString;
			prgLoad.Visible = false;
			prgLoad.Minimum = 0;
			prgLoad.Value = 0;
			prgLoad.Maximum = 100;
		}
		#region " Windows Form Designer generated code "

		public frmWebLinks() : base()
		{
			Closing += frmWebLinks_Closing;
			Activated += frmWebLinks_Activated;

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
		internal System.Windows.Forms.GroupBox gbButtonLayout;
		internal System.Windows.Forms.StatusBar sbStatus;
		internal System.Windows.Forms.GroupBox gbDetail;
		private System.Windows.Forms.TreeView withEventsField_tvwDB;
		internal System.Windows.Forms.TreeView tvwDB {
			get { return withEventsField_tvwDB; }
			set {
				if (withEventsField_tvwDB != null) {
					withEventsField_tvwDB.AfterExpand -= tvwDB_AfterExpand;
					withEventsField_tvwDB.BeforeCollapse -= tvwDB_BeforeCollapse;
					withEventsField_tvwDB.BeforeExpand -= tvwDB_BeforeExpand;
					withEventsField_tvwDB.BeforeSelect -= tvwDB_BeforeSelect;
					withEventsField_tvwDB.DragDrop -= tvwDB_DragDrop;
					withEventsField_tvwDB.DragEnter -= tvwDB_DragEnter;
					withEventsField_tvwDB.ItemDrag -= tvwDB_ItemDrag;
				}
				withEventsField_tvwDB = value;
				if (withEventsField_tvwDB != null) {
					withEventsField_tvwDB.AfterExpand += tvwDB_AfterExpand;
					withEventsField_tvwDB.BeforeCollapse += tvwDB_BeforeCollapse;
					withEventsField_tvwDB.BeforeExpand += tvwDB_BeforeExpand;
					withEventsField_tvwDB.BeforeSelect += tvwDB_BeforeSelect;
					withEventsField_tvwDB.DragDrop += tvwDB_DragDrop;
					withEventsField_tvwDB.DragEnter += tvwDB_DragEnter;
					withEventsField_tvwDB.ItemDrag += tvwDB_ItemDrag;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtLabel;
		protected internal System.Windows.Forms.TextBox txtLabel {
			get { return withEventsField_txtLabel; }
			set {
				if (withEventsField_txtLabel != null) {
					withEventsField_txtLabel.Validating -= TextBox_Validating;
				}
				withEventsField_txtLabel = value;
				if (withEventsField_txtLabel != null) {
					withEventsField_txtLabel.Validating += TextBox_Validating;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtURL;
		protected internal System.Windows.Forms.TextBox txtURL {
			get { return withEventsField_txtURL; }
			set {
				if (withEventsField_txtURL != null) {
					withEventsField_txtURL.Validating -= TextBox_Validating;
				}
				withEventsField_txtURL = value;
				if (withEventsField_txtURL != null) {
					withEventsField_txtURL.Validating += TextBox_Validating;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtTargetFrame;
		protected internal System.Windows.Forms.TextBox txtTargetFrame {
			get { return withEventsField_txtTargetFrame; }
			set {
				if (withEventsField_txtTargetFrame != null) {
					withEventsField_txtTargetFrame.Validating -= TextBox_Validating;
				}
				withEventsField_txtTargetFrame = value;
				if (withEventsField_txtTargetFrame != null) {
					withEventsField_txtTargetFrame.Validating += TextBox_Validating;
				}
			}
		}
		internal System.Windows.Forms.CheckBox chkHasMembers;
		internal System.Windows.Forms.Label lblLabel;
		internal System.Windows.Forms.Label lblURL;
		internal System.Windows.Forms.Label lblTargetFrame;
		internal System.Windows.Forms.Label lblParentID;
		private System.Windows.Forms.TextBox withEventsField_txtParentID;
		protected internal System.Windows.Forms.TextBox txtParentID {
			get { return withEventsField_txtParentID; }
			set {
				if (withEventsField_txtParentID != null) {
					withEventsField_txtParentID.Validating -= TextBox_Validating;
				}
				withEventsField_txtParentID = value;
				if (withEventsField_txtParentID != null) {
					withEventsField_txtParentID.Validating += TextBox_Validating;
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
		private System.Windows.Forms.Button withEventsField_btnHyperlink;
		internal System.Windows.Forms.Button btnHyperlink {
			get { return withEventsField_btnHyperlink; }
			set {
				if (withEventsField_btnHyperlink != null) {
					withEventsField_btnHyperlink.Click -= btnHyperlink_Click;
				}
				withEventsField_btnHyperlink = value;
				if (withEventsField_btnHyperlink != null) {
					withEventsField_btnHyperlink.Click += btnHyperlink_Click;
				}
			}
		}
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
		protected internal System.Windows.Forms.StatusBarPanel sbpEndBorder;
		protected internal System.Windows.Forms.StatusBarPanel sbpTime;
		protected internal System.Windows.Forms.StatusBarPanel sbpPosition;
		internal System.Windows.Forms.StatusBarPanel sbpStatus;
		protected internal System.Windows.Forms.StatusBarPanel sbpMessage;
		private System.Windows.Forms.Timer withEventsField_timClock;
		internal System.Windows.Forms.Timer timClock {
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
		internal System.Windows.Forms.ImageList imlIcons;
		internal System.Windows.Forms.Label lblLoad;
		internal System.Windows.Forms.ProgressBar prgLoad;
		internal System.Windows.Forms.Label lblButtonLabel;
		private System.Windows.Forms.TextBox withEventsField_txtButtonLabel;
		protected internal System.Windows.Forms.TextBox txtButtonLabel {
			get { return withEventsField_txtButtonLabel; }
			set {
				if (withEventsField_txtButtonLabel != null) {
					withEventsField_txtButtonLabel.Validating -= TextBox_Validating;
				}
				withEventsField_txtButtonLabel = value;
				if (withEventsField_txtButtonLabel != null) {
					withEventsField_txtButtonLabel.Validating += TextBox_Validating;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnEdit;
		internal System.Windows.Forms.Button btnEdit {
			get { return withEventsField_btnEdit; }
			set {
				if (withEventsField_btnEdit != null) {
					withEventsField_btnEdit.Click -= btnEdit_Click;
				}
				withEventsField_btnEdit = value;
				if (withEventsField_btnEdit != null) {
					withEventsField_btnEdit.Click += btnEdit_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenu ctxMenu;
		internal System.Windows.Forms.MenuItem mnuContextNew;
		private System.Windows.Forms.MenuItem withEventsField_mnuContextNewGroup;
		internal System.Windows.Forms.MenuItem mnuContextNewGroup {
			get { return withEventsField_mnuContextNewGroup; }
			set {
				if (withEventsField_mnuContextNewGroup != null) {
					withEventsField_mnuContextNewGroup.Click -= mnuContextNewGroup_Click;
				}
				withEventsField_mnuContextNewGroup = value;
				if (withEventsField_mnuContextNewGroup != null) {
					withEventsField_mnuContextNewGroup.Click += mnuContextNewGroup_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuContextNewLink;
		internal System.Windows.Forms.MenuItem mnuContextNewLink {
			get { return withEventsField_mnuContextNewLink; }
			set {
				if (withEventsField_mnuContextNewLink != null) {
					withEventsField_mnuContextNewLink.Click -= mnuContextNewLink_Click;
				}
				withEventsField_mnuContextNewLink = value;
				if (withEventsField_mnuContextNewLink != null) {
					withEventsField_mnuContextNewLink.Click += mnuContextNewLink_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuContextDelete;
		internal System.Windows.Forms.MenuItem mnuContextDelete {
			get { return withEventsField_mnuContextDelete; }
			set {
				if (withEventsField_mnuContextDelete != null) {
					withEventsField_mnuContextDelete.Click -= mnuContextDelete_Click;
				}
				withEventsField_mnuContextDelete = value;
				if (withEventsField_mnuContextDelete != null) {
					withEventsField_mnuContextDelete.Click += mnuContextDelete_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnExit;
		internal System.Windows.Forms.Button btnExit {
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
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmWebLinks));
			this.gbButtonLayout = new System.Windows.Forms.GroupBox();
			this.tvwDB = new System.Windows.Forms.TreeView();
			this.ctxMenu = new System.Windows.Forms.ContextMenu();
			this.mnuContextNew = new System.Windows.Forms.MenuItem();
			this.mnuContextNewGroup = new System.Windows.Forms.MenuItem();
			this.mnuContextNewLink = new System.Windows.Forms.MenuItem();
			this.mnuContextDelete = new System.Windows.Forms.MenuItem();
			this.imlIcons = new System.Windows.Forms.ImageList(this.components);
			this.sbStatus = new System.Windows.Forms.StatusBar();
			this.sbpPosition = new System.Windows.Forms.StatusBarPanel();
			this.sbpStatus = new System.Windows.Forms.StatusBarPanel();
			this.sbpMessage = new System.Windows.Forms.StatusBarPanel();
			this.sbpTime = new System.Windows.Forms.StatusBarPanel();
			this.sbpEndBorder = new System.Windows.Forms.StatusBarPanel();
			this.gbDetail = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnHyperlink = new System.Windows.Forms.Button();
			this.lblTargetFrame = new System.Windows.Forms.Label();
			this.lblURL = new System.Windows.Forms.Label();
			this.lblLabel = new System.Windows.Forms.Label();
			this.txtTargetFrame = new System.Windows.Forms.TextBox();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.txtLabel = new System.Windows.Forms.TextBox();
			this.lblParentID = new System.Windows.Forms.Label();
			this.lblButtonLabel = new System.Windows.Forms.Label();
			this.txtParentID = new System.Windows.Forms.TextBox();
			this.txtButtonLabel = new System.Windows.Forms.TextBox();
			this.chkHasMembers = new System.Windows.Forms.CheckBox();
			this.timClock = new System.Windows.Forms.Timer(this.components);
			this.lblLoad = new System.Windows.Forms.Label();
			this.prgLoad = new System.Windows.Forms.ProgressBar();
			this.btnExit = new System.Windows.Forms.Button();
			this.gbButtonLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			this.gbDetail.SuspendLayout();
			this.SuspendLayout();
			//
			//gbButtonLayout
			//
			this.gbButtonLayout.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbButtonLayout.Controls.Add(this.tvwDB);
			this.gbButtonLayout.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.gbButtonLayout.Location = new System.Drawing.Point(4, 8);
			this.gbButtonLayout.Name = "gbButtonLayout";
			this.gbButtonLayout.Size = new System.Drawing.Size(344, 344);
			this.gbButtonLayout.TabIndex = 0;
			this.gbButtonLayout.TabStop = false;
			this.gbButtonLayout.Text = "Button Layout";
			//
			//tvwDB
			//
			this.tvwDB.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.tvwDB.ContextMenu = this.ctxMenu;
			this.tvwDB.ImageList = this.imlIcons;
			this.tvwDB.Location = new System.Drawing.Point(8, 23);
			this.tvwDB.Name = "tvwDB";
			this.tvwDB.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { new System.Windows.Forms.TreeNode("Web Menu Buttons", 0, 0, new System.Windows.Forms.TreeNode[] {
				new System.Windows.Forms.TreeNode("Aircraft", 1, 1),
				new System.Windows.Forms.TreeNode("Books", 1, 1),
				new System.Windows.Forms.TreeNode("Games", 1, 1),
				new System.Windows.Forms.TreeNode("Hobby", 1, 1),
				new System.Windows.Forms.TreeNode("Microsoft", 1, 1),
				new System.Windows.Forms.TreeNode("Military", 1, 1),
				new System.Windows.Forms.TreeNode("Music", 1, 1),
				new System.Windows.Forms.TreeNode("Movies", 1, 1),
				new System.Windows.Forms.TreeNode("NASA", 1, 1),
				new System.Windows.Forms.TreeNode("SciFi", 1, 1),
				new System.Windows.Forms.TreeNode("Ships", 1, 1),
				new System.Windows.Forms.TreeNode("Software", 1, 1),
				new System.Windows.Forms.TreeNode("SunGard", 1, 1),
				new System.Windows.Forms.TreeNode("TV", 1, 1),
				new System.Windows.Forms.TreeNode("Web", 1, 1)
			}) });
			this.tvwDB.Size = new System.Drawing.Size(328, 313);
			this.tvwDB.Sorted = true;
			this.tvwDB.TabIndex = 0;
			//
			//ctxMenu
			//
			this.ctxMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuContextNew,
				this.mnuContextDelete
			});
			//
			//mnuContextNew
			//
			this.mnuContextNew.Index = 0;
			this.mnuContextNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuContextNewGroup,
				this.mnuContextNewLink
			});
			this.mnuContextNew.Text = "&New";
			//
			//mnuContextNewGroup
			//
			this.mnuContextNewGroup.Index = 0;
			this.mnuContextNewGroup.Text = "&Group";
			//
			//mnuContextNewLink
			//
			this.mnuContextNewLink.Index = 1;
			this.mnuContextNewLink.Text = "&Link";
			//
			//mnuContextDelete
			//
			this.mnuContextDelete.Index = 1;
			this.mnuContextDelete.Text = "&Delete";
			//
			//imlIcons
			//
			this.imlIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.imlIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imlIcons.ImageStream");
			this.imlIcons.TransparentColor = System.Drawing.Color.Transparent;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 356);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
				this.sbpPosition,
				this.sbpStatus,
				this.sbpMessage,
				this.sbpTime,
				this.sbpEndBorder
			});
			this.sbStatus.ShowPanels = true;
			this.sbStatus.Size = new System.Drawing.Size(744, 22);
			this.sbStatus.TabIndex = 1;
			this.sbStatus.Text = "StatusBar1";
			//
			//sbpPosition
			//
			this.sbpPosition.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpPosition.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpPosition.Text = "Position";
			this.sbpPosition.ToolTipText = "Record Position";
			this.sbpPosition.Width = 55;
			//
			//sbpStatus
			//
			this.sbpStatus.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.sbpStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpStatus.Text = "Status";
			this.sbpStatus.ToolTipText = "Status";
			this.sbpStatus.Width = 46;
			//
			//sbpMessage
			//
			this.sbpMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpMessage.Text = "Message";
			this.sbpMessage.Width = 556;
			//
			//sbpTime
			//
			this.sbpTime.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpTime.MinWidth = 70;
			this.sbpTime.Text = "12:00 PM";
			this.sbpTime.Width = 70;
			//
			//sbpEndBorder
			//
			this.sbpEndBorder.MinWidth = 1;
			this.sbpEndBorder.Width = 1;
			//
			//gbDetail
			//
			this.gbDetail.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.gbDetail.Controls.Add(this.btnCancel);
			this.gbDetail.Controls.Add(this.btnEdit);
			this.gbDetail.Controls.Add(this.btnOK);
			this.gbDetail.Controls.Add(this.btnHyperlink);
			this.gbDetail.Controls.Add(this.lblTargetFrame);
			this.gbDetail.Controls.Add(this.lblURL);
			this.gbDetail.Controls.Add(this.lblLabel);
			this.gbDetail.Controls.Add(this.txtTargetFrame);
			this.gbDetail.Controls.Add(this.txtURL);
			this.gbDetail.Controls.Add(this.txtLabel);
			this.gbDetail.Controls.Add(this.lblParentID);
			this.gbDetail.Controls.Add(this.lblButtonLabel);
			this.gbDetail.Controls.Add(this.txtParentID);
			this.gbDetail.Controls.Add(this.txtButtonLabel);
			this.gbDetail.Controls.Add(this.chkHasMembers);
			this.gbDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.gbDetail.Location = new System.Drawing.Point(352, 8);
			this.gbDetail.Name = "gbDetail";
			this.gbDetail.Size = new System.Drawing.Size(384, 156);
			this.gbDetail.TabIndex = 2;
			this.gbDetail.TabStop = false;
			this.gbDetail.Text = "Detail";
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(280, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(84, 28);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			//
			//btnEdit
			//
			this.btnEdit.Location = new System.Drawing.Point(280, 112);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(84, 28);
			this.btnEdit.TabIndex = 7;
			this.btnEdit.Text = "&Edit";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(188, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(84, 28);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			//
			//btnHyperlink
			//
			this.btnHyperlink.Location = new System.Drawing.Point(188, 112);
			this.btnHyperlink.Name = "btnHyperlink";
			this.btnHyperlink.Size = new System.Drawing.Size(84, 28);
			this.btnHyperlink.TabIndex = 6;
			this.btnHyperlink.Text = "&Hyperlink";
			//
			//lblTargetFrame
			//
			this.lblTargetFrame.AutoSize = true;
			this.lblTargetFrame.Location = new System.Drawing.Point(36, 78);
			this.lblTargetFrame.Name = "lblTargetFrame";
			this.lblTargetFrame.Size = new System.Drawing.Size(96, 19);
			this.lblTargetFrame.TabIndex = 9;
			this.lblTargetFrame.Text = "Target Frame";
			//
			//lblURL
			//
			this.lblURL.AutoSize = true;
			this.lblURL.Location = new System.Drawing.Point(45, 50);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(31, 19);
			this.lblURL.TabIndex = 8;
			this.lblURL.Text = "URL";
			//
			//lblLabel
			//
			this.lblLabel.AutoSize = true;
			this.lblLabel.Location = new System.Drawing.Point(36, 22);
			this.lblLabel.Name = "lblLabel";
			this.lblLabel.Size = new System.Drawing.Size(40, 19);
			this.lblLabel.TabIndex = 7;
			this.lblLabel.Text = "Label";
			//
			//txtTargetFrame
			//
			this.txtTargetFrame.Location = new System.Drawing.Point(136, 76);
			this.txtTargetFrame.MaxLength = 32;
			this.txtTargetFrame.Name = "txtTargetFrame";
			this.txtTargetFrame.Size = new System.Drawing.Size(224, 23);
			this.txtTargetFrame.TabIndex = 3;
			this.txtTargetFrame.Tag = "Required";
			this.txtTargetFrame.Text = "txtTargetFrame";
			//
			//txtURL
			//
			this.txtURL.Location = new System.Drawing.Point(84, 48);
			this.txtURL.MaxLength = 255;
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(276, 23);
			this.txtURL.TabIndex = 2;
			this.txtURL.Tag = "Required";
			this.txtURL.Text = "txtURL";
			//
			//txtLabel
			//
			this.txtLabel.Location = new System.Drawing.Point(84, 20);
			this.txtLabel.MaxLength = 80;
			this.txtLabel.Name = "txtLabel";
			this.txtLabel.Size = new System.Drawing.Size(276, 23);
			this.txtLabel.TabIndex = 1;
			this.txtLabel.Tag = "Required";
			this.txtLabel.Text = "txtLabel";
			//
			//lblParentID
			//
			this.lblParentID.AutoSize = true;
			this.lblParentID.Location = new System.Drawing.Point(7, 52);
			this.lblParentID.Name = "lblParentID";
			this.lblParentID.Size = new System.Drawing.Size(69, 19);
			this.lblParentID.TabIndex = 13;
			this.lblParentID.Text = "Parent ID";
			//
			//lblButtonLabel
			//
			this.lblButtonLabel.AutoSize = true;
			this.lblButtonLabel.Location = new System.Drawing.Point(26, 24);
			this.lblButtonLabel.Name = "lblButtonLabel";
			this.lblButtonLabel.Size = new System.Drawing.Size(50, 19);
			this.lblButtonLabel.TabIndex = 12;
			this.lblButtonLabel.Text = "Button";
			//
			//txtParentID
			//
			this.txtParentID.Location = new System.Drawing.Point(84, 48);
			this.txtParentID.Name = "txtParentID";
			this.txtParentID.Size = new System.Drawing.Size(276, 23);
			this.txtParentID.TabIndex = 11;
			this.txtParentID.Tag = "Required,Numeric";
			this.txtParentID.Text = "txtParentID";
			//
			//txtButtonLabel
			//
			this.txtButtonLabel.Location = new System.Drawing.Point(84, 20);
			this.txtButtonLabel.MaxLength = 80;
			this.txtButtonLabel.Name = "txtButtonLabel";
			this.txtButtonLabel.Size = new System.Drawing.Size(276, 23);
			this.txtButtonLabel.TabIndex = 10;
			this.txtButtonLabel.Tag = "Required";
			this.txtButtonLabel.Text = "txtButtonLabel";
			//
			//chkHasMembers
			//
			this.chkHasMembers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkHasMembers.Location = new System.Drawing.Point(236, 76);
			this.chkHasMembers.Name = "chkHasMembers";
			this.chkHasMembers.Size = new System.Drawing.Size(136, 24);
			this.chkHasMembers.TabIndex = 5;
			this.chkHasMembers.Text = "Has Members?";
			//
			//timClock
			//
			this.timClock.Interval = 200;
			//
			//lblLoad
			//
			this.lblLoad.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.lblLoad.AutoSize = true;
			this.lblLoad.Location = new System.Drawing.Point(360, 320);
			this.lblLoad.Name = "lblLoad";
			this.lblLoad.Size = new System.Drawing.Size(52, 19);
			this.lblLoad.TabIndex = 17;
			this.lblLoad.Text = "lblLoad";
			//
			//prgLoad
			//
			this.prgLoad.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.prgLoad.Location = new System.Drawing.Point(360, 340);
			this.prgLoad.Name = "prgLoad";
			this.prgLoad.Size = new System.Drawing.Size(368, 8);
			this.prgLoad.TabIndex = 16;
			//
			//btnExit
			//
			this.btnExit.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnExit.Location = new System.Drawing.Point(644, 304);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(84, 28);
			this.btnExit.TabIndex = 8;
			this.btnExit.Text = "E&xit";
			//
			//frmWebLinks
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(744, 378);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lblLoad);
			this.Controls.Add(this.sbStatus);
			this.Controls.Add(this.prgLoad);
			this.Controls.Add(this.gbDetail);
			this.Controls.Add(this.gbButtonLayout);
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Name = "frmWebLinks";
			this.Text = "frmWebLinks";
			this.gbButtonLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			this.gbDetail.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		#region "Properties"
		#region "Declarations"
		#region "Enumerations"
		protected internal enum sbStatusPanelEnum : short
		{
			Position,
			Status,
			Message,
			Time,
			EndBorder
		}
		protected internal enum imlIconsEnum : short
		{
			WebButtons,
			WebButton,
			ClosedFolder,
			OpenFolder,
			Link,
			AltLink
		}
		protected internal enum tvwDBEnum : short
		{
			WebMenuButtons,
			Aircraft,
			Books,
			Games,
			Hobby,
			Microsoft,
			Military,
			Music,
			Movies,
			NASA,
			SciFi,
			Ships,
			Software,
			SunGard,
			TV,
			Web
		}
		#endregion
		protected const byte CtrlMask = 8;
		protected bool fActivated = false;
		protected bool fAdding = false;
		protected bool fOKtoUnload = true;
		protected string mRegistryKey;
			#endregion
		protected TreeNode rootNode;
		#endregion
		#region "Methods"
		private int AddNode(TreeNode ParentNode, string strID, string strLabel, string strButton, string strParentID, bool fHasMembers, bool fSelectNode)
		{
			int functionReturnValue = 0;
			TreeNode mNode = new TreeNode(strLabel);

			if (IsRoot(ParentNode))
				throw new ArgumentException("AddNode called with Root Node");

			//mNode.Key = strButton & strParentID & strID
			if (fHasMembers) {
				mNode.Tag = "Group: " + strID;
				mNode.ImageIndex = imlIconsEnum.ClosedFolder;
				mNode.SelectedImageIndex = imlIconsEnum.ClosedFolder;
			} else {
				mNode.Tag = "Link: " + strID;
				mNode.ImageIndex = imlIconsEnum.Link;
				mNode.SelectedImageIndex = imlIconsEnum.Link;
			}

			functionReturnValue = ParentNode.Nodes.Add(mNode);

			if (fSelectNode) {
				mNode.EnsureVisible();
				mNode.TreeView.SelectedNode = mNode;
				PopulateDetail(mNode);
			}
			return functionReturnValue;
		}
		private void ClearDetail()
		{
			foreach (Control ctl in this.gbDetail.Controls) {
				switch (ctl.GetType().Name) {
					case "TextBox":
						var _with2 = (TextBox)ctl;
						_with2.Text = bpeNullString;
						EnableControl(ctl, false);
						break;
					case "CheckBox":
						((CheckBox)ctl).CheckState = CheckState.Unchecked;
						EnableControl(ctl, false);
						break;
				}
			}
			this.sbStatus.Panels[sbStatusPanelEnum.Position].Text = bpeNullString;
		}
		protected internal void EnableMyControls(bool Enable)
		{
			base.EnableControl(txtLabel, Enable, false);
			base.EnableControl(txtTargetFrame, Enable, false);
			base.EnableControl(txtURL, Enable, false);
			base.EnableControl(chkHasMembers, Enable, false);
			base.EnableControl(txtParentID, Enable, false);
			base.EnableControl(txtButtonLabel, Enable, false);

			base.EnableControl(tvwDB, !Enable, false);
		}
		private bool IsButton(TreeNode Node)
		{
			bool functionReturnValue = false;
			if (Node == null)
				return functionReturnValue;
			functionReturnValue = false;
			if ((Node.Tag == null))
				return functionReturnValue;
			if (Convert.ToString(Node.Tag).ToUpper().StartsWith("BUTTON:"))
				functionReturnValue = true;
			return functionReturnValue;
		}
		private bool IsGroup(TreeNode Node)
		{
			bool functionReturnValue = false;
			if (Node == null)
				return functionReturnValue;
			functionReturnValue = false;
			if ((Node.Tag == null))
				return functionReturnValue;
			if (Convert.ToString(Node.Tag).ToUpper().StartsWith("GROUP:"))
				functionReturnValue = true;
			return functionReturnValue;
		}
		private bool IsLink(TreeNode Node)
		{
			bool functionReturnValue = false;
			if (Node == null)
				return functionReturnValue;
			functionReturnValue = false;
			if ((Node.Tag == null))
				return functionReturnValue;
			if (Convert.ToString(Node.Tag).ToUpper().StartsWith("LINK:"))
				functionReturnValue = true;
			return functionReturnValue;
		}
		private bool IsRoot(TreeNode Node)
		{
			bool functionReturnValue = false;
			if (Node == null)
				return functionReturnValue;
			functionReturnValue = Convert.ToBoolean(object.ReferenceEquals(Node, rootNode));
			return functionReturnValue;
		}
		private int GetID(TreeNode Node)
		{
			int functionReturnValue = 0;
			if (Node == null)
				return functionReturnValue;
			if (IsRoot(Node))
				return 0;
			if (IsButton(Node))
				return Convert.ToInt32(Convert.ToString(Node.Tag).Substring(Strings.Len("Button: ")));
			if (IsGroup(Node))
				return Convert.ToInt32(Convert.ToString(Node.Tag).Substring(Strings.Len("Group: ")));
			if (IsLink(Node))
				return Convert.ToInt32(Convert.ToString(Node.Tag).Substring(Strings.Len("Link: ")));
			throw new ArgumentException(string.Format("Unknown Node.Tag ({0}) encountered for Node.Text ({1})", Node.Tag.ToString(), Node.Text));
			return functionReturnValue;
		}
		private long NewID()
		{
			return mTCBase.ExecuteScalarCommand("Select Max(ID)+1 From MenuEntries");
		}
		private void PopulateDetail(TreeNode Node)
		{
			DataView dvEntries = null;
			try {
				int intID = GetID(Node);
				lblLabel.Visible = true;
				txtLabel.Visible = true;
				//Hide everything else...
				lblURL.Visible = false;
				txtURL.Visible = false;
				lblTargetFrame.Visible = false;
				txtTargetFrame.Visible = false;
				chkHasMembers.Visible = false;
				lblParentID.Visible = false;
				txtParentID.Visible = false;
				lblButtonLabel.Visible = false;
				txtButtonLabel.Visible = false;
				btnHyperlink.Visible = false;
				btnEdit.Visible = false;
				btnOK.Visible = false;
				btnCancel.Visible = false;
				if (IsRoot(Node)) {
					gbDetail.Text = "Root Detail";
					txtLabel.Text = Node.Text;
					txtButtonLabel.Text = Node.Text;
					break; // TODO: might not be correct. Was : Exit Try
				} else if (IsButton(Node)) {
					gbDetail.Text = "Button Detail";
					txtLabel.Text = Node.Text;
					txtButtonLabel.Text = Node.Text;
					break; // TODO: might not be correct. Was : Exit Try
				} else if (IsGroup(Node)) {
					gbDetail.Text = "Group Detail";
				} else if (IsLink(Node)) {
					gbDetail.Text = "Link Detail";
					lblURL.Visible = true;
					txtURL.Visible = true;
					lblTargetFrame.Visible = true;
					txtTargetFrame.Visible = true;
					btnHyperlink.Visible = true;
				}
				this.sbStatus.Panels[sbStatusPanelEnum.Position].Text = intID;

				string SQLstatement = string.Format("Select * From MenuEntries Where ID={0}", intID);
				dvEntries = mTCBase.MakeDataViewCommand(SQLstatement, "MenuEntries", false);
				DataRowView iRow = dvEntries[0];
				Trace("Processing Entry: ID: {0}; Label: {1}", iRow["ID"], iRow["Label"], trcOption.trcApplication);

				txtLabel.Text = VBdecode(iRow["Label"]);
				txtURL.Text = URLdecode(iRow["URL"]);
				txtTargetFrame.Text = VBdecode(iRow["TargetFrame"]);
				txtParentID.Text = VBdecode(iRow["ParentID"]);
				txtButtonLabel.Text = VBdecode(iRow["ButtonLabel"]);
				switch (iRow["HasMembers"]) {
					case 0:
						chkHasMembers.CheckState = CheckState.Unchecked;
						break;
					case 1:
						chkHasMembers.CheckState = CheckState.Checked;
						break;
				}
				btnEdit.Visible = true;
			} finally {
				if ((dvEntries != null))
					dvEntries.Dispose();
				dvEntries = null;
			}
		}
		#region "Utility Methods"
		public string URLdecode(object vURL)
		{
			string functionReturnValue = null;
			int i = 0;

			if (Information.IsDBNull(vURL)) {
				functionReturnValue = "";
				return functionReturnValue;
			}

			functionReturnValue = vURL;
			while (true) {
				i = Strings.InStr(URLdecode(), "%20");
				if (i > 0) {
					functionReturnValue = Strings.Mid(URLdecode(), 1, i - 1) + " " + Strings.Mid(URLdecode(), i + 3);
				} else {
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}
		public object URLencode(object vURL)
		{
			object functionReturnValue = null;
			int i = 0;

			functionReturnValue = vURL;
			if (string.IsNullOrEmpty(vURL)) {
				functionReturnValue = DBNull.Value;
				return functionReturnValue;
			}

			while (true) {
				i = Strings.InStr(URLencode(), " ");
				if (i > 0) {
					functionReturnValue = Strings.Mid(URLencode(), 1, i - 1) + "%20" + Strings.Mid(URLencode(), i + 1);
				} else {
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}
		public string VBdecode(object vString)
		{
			string functionReturnValue = null;
			int i = 0;

			if (Information.IsDBNull(vString)) {
				functionReturnValue = "";
				return functionReturnValue;
			}

			functionReturnValue = vString;
			while (true) {
				i = Strings.InStr(VBdecode(), "&&");
				if (i > 0) {
					functionReturnValue = Strings.Mid(VBdecode(), 1, i - 1) + Strings.Mid(VBdecode(), i + 1);
				} else {
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}
		public object VBencode(object vString)
		{
			object functionReturnValue = null;
			int i = 0;
			int Start = 0;

			functionReturnValue = vString;
			if (string.IsNullOrEmpty(vString)) {
				functionReturnValue = DBNull.Value;
				return functionReturnValue;
			}

			Start = 1;
			while (true) {
				i = Strings.InStr(Start, VBencode(), "&");
				if (i > 0) {
					functionReturnValue = Strings.Mid(VBencode(), 1, i - 1) + "&&" + Strings.Mid(VBencode(), i + 1);
					Start = i + 2;
				} else {
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}
		#endregion
		#endregion
		#region "Event Handlers"
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "btnCancel_Click";
			try {
				if (MessageBox.Show("Are you sure you want to lose any changes?", "Cancel Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
					break; // TODO: might not be correct. Was : Exit Try

				EnableMyControls(false);

				//Restore original values...
				PopulateDetail(tvwDB.SelectedNode);

				if (IsLink(tvwDB.SelectedNode))
					this.btnHyperlink.Visible = true;
				this.btnEdit.Visible = true;
				this.btnOK.Visible = false;
				this.btnCancel.Visible = false;
				this.btnExit.Visible = true;
				this.sbStatus.Panels[sbStatusPanelEnum.Status].Text = null;
				fOKtoUnload = true;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "btnEdit_Click";
			try {
				EnableMyControls(true);

				this.btnHyperlink.Visible = false;
				this.btnEdit.Visible = false;
				this.btnOK.Visible = true;
				this.btnCancel.Visible = true;
				this.btnExit.Visible = false;
				this.sbStatus.Panels[sbStatusPanelEnum.Status].Text = "Edit Mode";
				fOKtoUnload = false;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		private void btnExit_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "btnExit_Click";
			try {
				this.Close();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		private void btnHyperlink_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "btnHyperlink_Click";
			//TODO: Implement programmatic invocation of IE with the link data similar to the old frmWebLinks.IEhyperlink method...
			try {
				MessageBox.Show("Implement programmatic invocation of IE with the link data similar to the old frmWebLinks.IEhyperlink method...", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "btnOK_Click";
			try {
				string SQL = bpeNullString;

				if (fAdding) {
					object[] dataArgs = {
						this.sbStatus.Panels[sbStatusPanelEnum.Position].Text,
						txtLabel.Text,
						txtURL.Text,
						txtParentID.Text,
						txtButtonLabel.Text,
						(this.gbDetail.Text == "Group Detail" ? 1 : 0),
						txtTargetFrame.Text
					};
					SQL = string.Format("Insert Into [MenuEntries]([ID], [Label], [URL], [ParentID], [ButtonLabel], [HasMembers], [TargetFrame]) Values ({0}, '{1}', '{2}', {3}, '{4}', {5}, '{6}')", dataArgs);
					mTCBase.BeginTrans();
					mTCBase.ExecuteCommand(SQL);
					mTCBase.EndTrans();

					AddNode(tvwDB.SelectedNode, this.sbStatus.Panels[sbStatusPanelEnum.Position].Text, txtLabel.Text, txtButtonLabel.Text, txtParentID.Text, Convert.ToBoolean(this.gbDetail.Text == "Group Detail"), true);
					fAdding = false;
				} else {
					SQL = "Update MenuEntries Set ";
					SQL += string.Format("{0}='{1}', ", "Label", VBencode(txtLabel.Text));
					SQL += string.Format("{0}={1}, ", "ParentID", VBencode(txtParentID.Text));
					//Set behind the scenes...
					SQL += string.Format("{0}='{1}', ", "TargetFrame", VBencode(txtTargetFrame.Text));
					SQL += string.Format("{0}='{1}', ", "ButtonLabel", VBencode(txtButtonLabel.Text));
					SQL += string.Format("{0}='{1}', ", "URL", URLencode(txtURL.Text));
					SQL += string.Format("{0}={1} ", "HasMembers", (this.gbDetail.Text == "Group Detail" ? 1 : 0));
					SQL += string.Format("Where {0}={1} ", "ID", this.sbStatus.Panels[sbStatusPanelEnum.Position].Text);
					mTCBase.BeginTrans();
					mTCBase.ExecuteCommand(SQL);
					mTCBase.EndTrans();
					tvwDB.SelectedNode.Text = txtLabel.Text;
				}

				EnableMyControls(false);

				if (IsLink(tvwDB.SelectedNode))
					this.btnHyperlink.Visible = true;
				this.btnEdit.Visible = true;
				this.btnOK.Visible = false;
				this.btnCancel.Visible = false;
				this.btnExit.Visible = true;
				this.sbStatus.Panels[sbStatusPanelEnum.Status].Text = null;
				fOKtoUnload = true;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void frmWebLinks_Activated(object sender, System.EventArgs e)
		{
			const string EntryName = "frmWebLinks_Activated";
			try {
				if (fActivated)
					break; // TODO: might not be correct. Was : Exit Try
				fActivated = true;

				int iLeft = GetRegistrySetting(ref RootKeyConstants.HKEY_CURRENT_USER, ref mRegistryKey, ref "Form Left", this.Left);
				int iTop = GetRegistrySetting(ref RootKeyConstants.HKEY_CURRENT_USER, ref mRegistryKey, ref "Form Top", this.Top);
				int iWidth = GetRegistrySetting(ref RootKeyConstants.HKEY_CURRENT_USER, ref mRegistryKey, ref "Form Width", this.Width);
				int iHeight = GetRegistrySetting(ref RootKeyConstants.HKEY_CURRENT_USER, ref mRegistryKey, ref "Form Height", this.Height);
				this.SetBounds(iLeft, iTop, iWidth, iHeight);

				//Since SaveRegistrySetting isn't [yet] smart enough to create a missing parent key when creating sub-keys, 
				//we'll save our would-be parent key here even though it makes more sense to do so in the UnloadCommand...
				mTCBase.SaveBounds(mRegistryKey, iLeft, iTop, iWidth, iHeight);
				this.timClock.Enabled = true;
				sbStatus.Panels[sbStatusPanelEnum.Position].Text = bpeNullString;
				sbStatus.Panels[sbStatusPanelEnum.Status].Text = bpeNullString;
				sbStatus.Panels[sbStatusPanelEnum.Message].Text = bpeNullString;
				tvwDB.Nodes[0].Expand();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void frmWebLinks_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try {
				if (!fOKtoUnload){e.Cancel = true;break; // TODO: might not be correct. Was : Exit Try
}
				mTCBase.UnloadCommand(this, null);
				mTCBase.CloseConnection();
				e.Cancel = false;
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void mnuContextDelete_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuContextDelete_Click";
			//TODO: Implement Delete logic (and recycle-bin?)...
			try {
				MessageBox.Show("Implement Delete logic (and recycle-bin?)...", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void mnuContextNewGroup_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuContextNewGroup_Click";
			try {
				lblLabel.Visible = true;
				txtLabel.Visible = true;
				//Hide everything else...
				lblURL.Visible = false;
				txtURL.Visible = false;
				lblTargetFrame.Visible = false;
				txtTargetFrame.Visible = false;
				chkHasMembers.Visible = false;
				lblParentID.Visible = false;
				txtParentID.Visible = false;
				lblButtonLabel.Visible = false;
				txtButtonLabel.Visible = false;
				btnHyperlink.Visible = false;
				btnEdit.Visible = false;
				btnOK.Visible = false;
				btnCancel.Visible = false;

				string saveButtonLabel = txtButtonLabel.Text;
				ClearDetail();
				gbDetail.Text = "Group Detail";
				this.sbStatus.Panels[sbStatusPanelEnum.Position].Text = NewID();
				txtButtonLabel.Text = saveButtonLabel;
				txtParentID.Text = GetID(tvwDB.SelectedNode);
				EnableMyControls(true);

				this.btnHyperlink.Visible = false;
				this.btnEdit.Visible = false;
				this.btnOK.Visible = true;
				this.btnCancel.Visible = true;
				this.btnExit.Visible = false;
				this.sbStatus.Panels[sbStatusPanelEnum.Status].Text = "Edit Mode";
				fOKtoUnload = false;
				fAdding = true;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void mnuContextNewLink_Click(object sender, System.EventArgs e)
		{
			const string EntryName = "mnuContextNewLink_Click";
			try {
				lblLabel.Visible = true;
				txtLabel.Visible = true;
				//Hide everything else...
				lblURL.Visible = false;
				txtURL.Visible = false;
				lblTargetFrame.Visible = false;
				txtTargetFrame.Visible = false;
				chkHasMembers.Visible = false;
				lblParentID.Visible = false;
				txtParentID.Visible = false;
				lblButtonLabel.Visible = false;
				txtButtonLabel.Visible = false;
				btnHyperlink.Visible = false;
				btnEdit.Visible = false;
				btnOK.Visible = false;
				btnCancel.Visible = false;
				lblURL.Visible = true;
				txtURL.Visible = true;
				lblTargetFrame.Visible = true;
				txtTargetFrame.Visible = true;
				btnHyperlink.Visible = true;

				string saveButtonLabel = txtButtonLabel.Text;
				ClearDetail();
				gbDetail.Text = "Link Detail";
				this.sbStatus.Panels[sbStatusPanelEnum.Position].Text = NewID();
				txtButtonLabel.Text = saveButtonLabel;
				txtParentID.Text = GetID(tvwDB.SelectedNode);
				EnableMyControls(true);

				this.btnHyperlink.Visible = false;
				this.btnEdit.Visible = false;
				this.btnOK.Visible = true;
				this.btnCancel.Visible = true;
				this.btnExit.Visible = false;
				this.sbStatus.Panels[sbStatusPanelEnum.Status].Text = "Edit Mode";
				fOKtoUnload = false;
				fAdding = true;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected new void TextBox_Validating(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError(sender, bpeNullString);
				TextBox tb = (TextBox)sender;
				tb.Text = tb.Text.Trim();
				if (tb.Text == bpeNullString) {
					if ((sender.Tag == null ? "" : Convert.ToString(sender.Tag)).ToUpper().IndexOf("MONEY") >= 0) {
						tb.Text = "0.00";
					} else if ((sender.Tag == null ? "" : Convert.ToString(sender.Tag)).ToUpper().IndexOf("REQUIRED") >= 0) {
						throw new Exception("Value is required.");
					}
				} else if (!Information.IsNumeric(tb.Text) & (sender.Tag == null ? "" : Convert.ToString(sender.Tag)).ToUpper().IndexOf("NUMERIC") >= 0) {
					throw new Exception("Value must be numeric.");
				} else if (tb.TextLength > tb.MaxLength) {
					throw new Exception(string.Format("Length ({0}) exceeds maximum ({1}).", tb.TextLength, tb.MaxLength));
				}
			} catch (Exception ex) {
				base.epBase.SetError(sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected void timClock_Tick(System.Object sender, System.EventArgs e)
		{
			string EntryName = "timClock_Tick";
			try {
				//Too much overheard to be used in a timer...
				//RecordEntry(EntryName, "sender:={" & sender.GetType.ToString & "}, e:={" & e.GetType.ToString, trcOption.trcApplication)
				sbStatus.Panels[sbStatusPanelEnum.Time].Text = string.Format("{0:t}", DateAndTime.Now);
			} catch (Exception ex) {
			}
			//Too much overheard to be used in a timer...
			//RecordExit(EntryName)
			return;
		}
		protected void tvwDB_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			const string EntryName = "tvwDB_AfterExpand";
			try {
				//e.Node.EnsureVisible()
				e.Node.TreeView.SelectedNode = e.Node;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_BeforeCollapse(System.Object sender, TreeViewCancelEventArgs e)
		{
			const string EntryName = "tvwDB_BeforeCollapse";
			try {
				//For performance reasons and to avoid TreeView "flickering" during an 
				//large node update, it is best to wrap the update code in BeginUpdate...
				//EndUpdate statements.
				tvwDB.BeginUpdate();

				if (IsGroup(e.Node)){e.Node.ImageIndex = imlIconsEnum.ClosedFolder;e.Node.SelectedImageIndex = imlIconsEnum.ClosedFolder;}
				tvwDB.EndUpdate();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_BeforeExpand(System.Object sender, TreeViewCancelEventArgs e)
		{
			const string EntryName = "tvwDB_BeforeExpand";
			try {
				var _with3 = tvwDB;
				//For performance reasons and to avoid TreeView "flickering" during an large node update, it is best to wrap the update code in BeginUpdate/EndUpdate statements.
				_with3.BeginUpdate();
				foreach (TreeNode iNode in e.Node.Nodes) {
					//We need to populate it...
					if (iNode.GetNodeCount(false) == 0) {
						string SQLstatement = string.Format("Select * From MenuEntries Where ParentID={0} Order By ButtonLabel, ParentID, Label", GetID(iNode));
						DataView dvEntries = mTCBase.MakeDataViewCommand(SQLstatement, "MenuEntries", false);
						if (dvEntries.Count > 0) {
							lblLoad.Text = string.Format("Loading {0} Links...", e.Node.Text);
							lblLoad.Visible = true;
							prgLoad.Minimum = 0;
							prgLoad.Value = 0;
							prgLoad.Maximum = dvEntries.Count;
							prgLoad.Visible = true;
							for (long i = 0; i <= dvEntries.Count - 1; i++) {
								DataRowView iRow = dvEntries[i];
								Trace("Processing Entry: ButtonLabel: {0}; ID: {1}; Label: {2}", iRow["ButtonLabel"], iRow["ID"], iRow["Label"], trcOption.trcApplication);
								AddNode(iNode, iRow["ID"], VBdecode(iRow["Label"]), iRow["ButtonLabel"], VBdecode(iRow["ParentID"]), iRow["HasMembers"], false);
								prgLoad.Value += 1;
							}
						}
						dvEntries.Dispose();
						dvEntries = null;
						lblLoad.Visible = false;
						lblLoad.Text = bpeNullString;
						prgLoad.Visible = false;
						prgLoad.Minimum = 0;
						prgLoad.Value = 0;
						prgLoad.Maximum = 100;
					}
				}
				if (IsGroup(e.Node)){e.Node.ImageIndex = imlIconsEnum.OpenFolder;e.Node.SelectedImageIndex = imlIconsEnum.OpenFolder;}
				_with3.EndUpdate();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			const string EntryName = "tvwDB_BeforeSelect";
			try {
				PopulateDetail(e.Node);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_DragDrop(System.Object sender, System.Windows.Forms.DragEventArgs e)
		{
			const string EntryName = "tvwDB_DragDrop";
			try {
				//Initialize variable that holds the node dragged by the user.
				TreeNode OriginationNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

				//Calling GetDataPresent is a little different for a TreeView than for an
				//image or text because a TreeNode is not a member of the DataFormats
				//class. That is, it's not a predefined type. As such, you need to use a
				//different overload, one that takes the type as a string.
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) {
					Point pt = default(Point);
					TreeNode DestinationNode = null;

					//Use PointToClient to compute the location of the mouse over the destination TreeView.
					pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
					//Use this Point to get the closest node in the destination TreeView.
					DestinationNode = ((TreeView)sender).GetNodeAt(pt);

					//The If statement ensures that the user doesn't completely lose the
					//node if they accidentally release the mouse button over the node they
					//attempted to drag. Without a check to see if the original node is the
					//same as the destination node, they could make the node disappear.
					if ((!object.ReferenceEquals(DestinationNode.TreeView, OriginationNode.TreeView))) {
						DestinationNode.Nodes.Add((TreeNode)OriginationNode.Clone());
						//Expand the parent node when adding the new node so that the drop
						//is obvious. Without this, only a + symbol would appear.
						DestinationNode.Expand();
						//If the Ctrl key was not pressed, remove the original node to 
						//effect a drag-and-drop move.
						if ((e.KeyState & CtrlMask) != CtrlMask) {
							OriginationNode.Remove();
						}
					}
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_DragEnter(System.Object sender, System.Windows.Forms.DragEventArgs e)
		{
			const string EntryName = "tvwDB_DragEnter";
			try {
				//Check to be sure that the drag content is the correct type for this control. If not, reject the drop.
				if ((e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))) {
					//If the Ctrl key was pressed during the drag operation then perform a Copy. If not, perform a Move.
					if ((e.KeyState & CtrlMask) == CtrlMask) {
						e.Effect = DragDropEffects.Copy;
					} else {
						e.Effect = DragDropEffects.Move;
					}
				} else {
					e.Effect = DragDropEffects.None;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected void tvwDB_ItemDrag(System.Object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			const string EntryName = "tvwDB_ItemDrag";
			try {
				if (e.Button == MouseButtons.Left) {
					DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Copy);
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		#endregion
	}
}
