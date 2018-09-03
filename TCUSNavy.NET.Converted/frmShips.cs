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
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTrace;
//frmShips.vb
//   USN Ships Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   07/28/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCUSNavy
{
	public class frmShips : frmTCStandard
	{
		const string myFormName = "frmShips";
		public frmShips(clsSupport objSupport, clsShips objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			this.MinimumSize = new Size(this.Width, this.Height);
			mStatusActiveWidth = cbHomePort.Width;
			mStatusInactiveWidth = cbClass.Width;
			mURLInternetActivePos = this.txtURL_Internet.Top;
			mURLLocalActivePos = this.txtURL_Local.Top;
			mURLInternetInactivePos = this.cbHomePort.Top;
			mURLLocalInactivePos = this.txtURL_Internet.Top;

			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			LoadDefaultImage();
			BindControls();
			EnableControls(false);

			Icon = mTCBase.Icon;
			MinimumSize = new Size(Width, Height);
			ReportPath = mTCBase.ReportPath;

			//Reorder tcMain the way we want it (designer keeps changing it)...
			this.tcMain.SuspendLayout();
			this.tcMain.TabPages.Clear();
			TabPage[] pages = {
				tpGeneral,
				tpCharacteristics,
				tpWeapons,
				tpHistory,
				tpMoreHistory,
				tpImage,
				tpNotes
			};
			this.tcMain.TabPages.AddRange(pages);
			//Relocate pbGeneral to tpImage...
			this.tpImage.SuspendLayout();
			this.tpImage.Controls.Add(this.pbGeneral);
			this.pbGeneral.Location = new System.Drawing.Point(8, 8);
			this.pbGeneral.Size = new System.Drawing.Size(this.tpImage.Width - (2 * 8), this.tpImage.Height - (2 * 8));
			this.pbGeneral.Visible = true;
			this.tpImage.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
		}
		#region " Windows Form Designer generated code "
		public frmShips() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();
			//Add any initialization after the InitializeComponent() call
			this.MinimumSize = new Size(this.Width, this.Height);
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
		internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.Label lblName;
		internal System.Windows.Forms.TabPage tpCharacteristics;
		internal System.Windows.Forms.TabPage tpWeapons;
		internal System.Windows.Forms.Label lblClassification;
		internal System.Windows.Forms.TextBox txtClassDesc;
		internal System.Windows.Forms.GroupBox gbCharacteristics;
		internal System.Windows.Forms.TextBox txtDisplacement;
		internal System.Windows.Forms.TextBox txtLength;
		internal System.Windows.Forms.TextBox txtBeam;
		internal System.Windows.Forms.TextBox txtDraft;
		internal System.Windows.Forms.TextBox txtPropulsion;
		internal System.Windows.Forms.TextBox txtBoilers;
		internal System.Windows.Forms.TextBox txtManning;
		internal System.Windows.Forms.Label lblDisplacement;
		internal System.Windows.Forms.Label lblLength;
		internal System.Windows.Forms.Label lblBeam;
		internal System.Windows.Forms.Label lblDraft;
		internal System.Windows.Forms.Label lblPropulsion;
		internal System.Windows.Forms.Label lblBoilers;
		internal System.Windows.Forms.Label lblManning;
		internal System.Windows.Forms.Label lblSpeed;
		internal System.Windows.Forms.TextBox txtSpeed;
		internal System.Windows.Forms.GroupBox gbWeapons;
		internal System.Windows.Forms.Label lblEW;
		internal System.Windows.Forms.Label lblFireControl;
		internal System.Windows.Forms.Label lblSONARs;
		internal System.Windows.Forms.Label lblRADARs;
		internal System.Windows.Forms.Label lblASW;
		internal System.Windows.Forms.Label lblGuns;
		internal System.Windows.Forms.Label lblMissiles;
		internal System.Windows.Forms.Label lblAircraft;
		internal System.Windows.Forms.TextBox txtAircraft;
		internal System.Windows.Forms.TextBox txtMissiles;
		internal System.Windows.Forms.TextBox txtGuns;
		internal System.Windows.Forms.TextBox txtASW;
		internal System.Windows.Forms.TextBox txtRADARs;
		internal System.Windows.Forms.TextBox txtSONARs;
		internal System.Windows.Forms.TextBox txtFireControl;
		internal System.Windows.Forms.TextBox txtEW;
		internal System.Windows.Forms.TabPage tpImage;
		internal System.Windows.Forms.TextBox txtClassification;
		internal System.Windows.Forms.TabPage tpHistory;
		internal System.Windows.Forms.RichTextBox rtfHistory;
		internal System.Windows.Forms.TabPage tpMoreHistory;
		internal System.Windows.Forms.RichTextBox rtfMoreHistory;
		internal System.Windows.Forms.TextBox txtNumber;
		internal System.Windows.Forms.Label lblNumber;
		internal System.Windows.Forms.TextBox txtDesignation;
		internal System.Windows.Forms.Label lblDesignation;
		internal System.Windows.Forms.Label lblClass;
		internal System.Windows.Forms.Label lblCommand;
		internal System.Windows.Forms.Label lblHomePort;
		internal System.Windows.Forms.TextBox txtZipCode;
		internal System.Windows.Forms.Label lblZipCode;
		internal System.Windows.Forms.Label lblCommissioned;
		internal System.Windows.Forms.ComboBox cbCommand;
		internal System.Windows.Forms.ComboBox cbHomePort;
		internal System.Windows.Forms.Label lblURL_Internet;
		internal System.Windows.Forms.TextBox txtURL_Internet;
		internal System.Windows.Forms.Label lblURL_Local;
		internal System.Windows.Forms.TextBox txtURL_Local;
		private System.Windows.Forms.ComboBox withEventsField_cbClass;
		internal System.Windows.Forms.ComboBox cbClass {
			get { return withEventsField_cbClass; }
			set {
				if (withEventsField_cbClass != null) {
					withEventsField_cbClass.SelectedIndexChanged -= cbClass_SelectedIndexChanged;
					withEventsField_cbClass.SelectedValueChanged -= cbClass_SelectedValueChanged;
					withEventsField_cbClass.SelectionChangeCommitted -= cbClass_SelectionChangeCommitted;
				}
				withEventsField_cbClass = value;
				if (withEventsField_cbClass != null) {
					withEventsField_cbClass.SelectedIndexChanged += cbClass_SelectedIndexChanged;
					withEventsField_cbClass.SelectedValueChanged += cbClass_SelectedValueChanged;
					withEventsField_cbClass.SelectionChangeCommitted += cbClass_SelectionChangeCommitted;
				}
			}
		}
		internal System.Windows.Forms.DateTimePicker dtpCommissioned;
		internal System.Windows.Forms.ComboBox cbStatus;
		internal System.Windows.Forms.Label lblStatus;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.lblNumber = new System.Windows.Forms.Label();
			this.tpCharacteristics = new System.Windows.Forms.TabPage();
			this.gbCharacteristics = new System.Windows.Forms.GroupBox();
			this.txtSpeed = new System.Windows.Forms.TextBox();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.lblManning = new System.Windows.Forms.Label();
			this.lblBoilers = new System.Windows.Forms.Label();
			this.lblPropulsion = new System.Windows.Forms.Label();
			this.lblDraft = new System.Windows.Forms.Label();
			this.lblBeam = new System.Windows.Forms.Label();
			this.lblLength = new System.Windows.Forms.Label();
			this.lblDisplacement = new System.Windows.Forms.Label();
			this.txtManning = new System.Windows.Forms.TextBox();
			this.txtBoilers = new System.Windows.Forms.TextBox();
			this.txtPropulsion = new System.Windows.Forms.TextBox();
			this.txtDraft = new System.Windows.Forms.TextBox();
			this.txtBeam = new System.Windows.Forms.TextBox();
			this.txtLength = new System.Windows.Forms.TextBox();
			this.txtDisplacement = new System.Windows.Forms.TextBox();
			this.tpWeapons = new System.Windows.Forms.TabPage();
			this.gbWeapons = new System.Windows.Forms.GroupBox();
			this.txtEW = new System.Windows.Forms.TextBox();
			this.lblEW = new System.Windows.Forms.Label();
			this.lblFireControl = new System.Windows.Forms.Label();
			this.lblSONARs = new System.Windows.Forms.Label();
			this.lblRADARs = new System.Windows.Forms.Label();
			this.lblASW = new System.Windows.Forms.Label();
			this.lblGuns = new System.Windows.Forms.Label();
			this.lblMissiles = new System.Windows.Forms.Label();
			this.lblAircraft = new System.Windows.Forms.Label();
			this.txtFireControl = new System.Windows.Forms.TextBox();
			this.txtSONARs = new System.Windows.Forms.TextBox();
			this.txtRADARs = new System.Windows.Forms.TextBox();
			this.txtASW = new System.Windows.Forms.TextBox();
			this.txtGuns = new System.Windows.Forms.TextBox();
			this.txtMissiles = new System.Windows.Forms.TextBox();
			this.txtAircraft = new System.Windows.Forms.TextBox();
			this.lblClassification = new System.Windows.Forms.Label();
			this.txtClassDesc = new System.Windows.Forms.TextBox();
			this.tpHistory = new System.Windows.Forms.TabPage();
			this.rtfHistory = new System.Windows.Forms.RichTextBox();
			this.tpImage = new System.Windows.Forms.TabPage();
			this.txtClassification = new System.Windows.Forms.TextBox();
			this.tpMoreHistory = new System.Windows.Forms.TabPage();
			this.rtfMoreHistory = new System.Windows.Forms.RichTextBox();
			this.txtDesignation = new System.Windows.Forms.TextBox();
			this.lblDesignation = new System.Windows.Forms.Label();
			this.cbClass = new System.Windows.Forms.ComboBox();
			this.lblClass = new System.Windows.Forms.Label();
			this.cbCommand = new System.Windows.Forms.ComboBox();
			this.lblCommand = new System.Windows.Forms.Label();
			this.cbHomePort = new System.Windows.Forms.ComboBox();
			this.lblHomePort = new System.Windows.Forms.Label();
			this.txtZipCode = new System.Windows.Forms.TextBox();
			this.lblZipCode = new System.Windows.Forms.Label();
			this.dtpCommissioned = new System.Windows.Forms.DateTimePicker();
			this.lblCommissioned = new System.Windows.Forms.Label();
			this.lblURL_Internet = new System.Windows.Forms.Label();
			this.txtURL_Internet = new System.Windows.Forms.TextBox();
			this.lblURL_Local = new System.Windows.Forms.Label();
			this.txtURL_Local = new System.Windows.Forms.TextBox();
			this.cbStatus = new System.Windows.Forms.ComboBox();
			this.lblStatus = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			this.gbGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tpNotes.SuspendLayout();
			this.tpCharacteristics.SuspendLayout();
			this.gbCharacteristics.SuspendLayout();
			this.tpWeapons.SuspendLayout();
			this.gbWeapons.SuspendLayout();
			this.tpHistory.SuspendLayout();
			this.tpMoreHistory.SuspendLayout();
			this.SuspendLayout();
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 212);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 14;
			this.dtpPurchased.Visible = false;
			//
			//sbpPosition
			//
			this.sbpPosition.Text = "";
			this.sbpPosition.Width = 10;
			//
			//sbpStatus
			//
			this.sbpStatus.Text = "Filter Off";
			this.sbpStatus.Width = 74;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 476;
			//
			//sbpTime
			//
			this.sbpTime.Text = "12:18 PM";
			this.sbpTime.Width = 79;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 367);
			this.lblID.Name = "lblID";
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(639, 328);
			this.btnLast.Name = "btnLast";
			this.btnLast.TabIndex = 4;
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 328);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.TabIndex = 0;
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(508, 364);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 20;
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(612, 328);
			this.btnNext.Name = "btnNext";
			this.btnNext.TabIndex = 3;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(589, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 22;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 216);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.TabIndex = 9;
			this.lblPrice.Visible = false;
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 212);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.TabIndex = 13;
			this.lblPurchased.Visible = false;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 212);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.TabIndex = 15;
			this.lblInventoried.Visible = false;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(589, 364);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 21;
			//
			//sbpFilter
			//
			this.sbpFilter.Text = "";
			this.sbpFilter.Width = 10;
			//
			//sbpMode
			//
			this.sbpMode.Text = "";
			this.sbpMode.Width = 10;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral2.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral2.Visible = false;
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(484, 212);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 17;
			this.chkWishList.Visible = false;
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral.Visible = false;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 328);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(544, 24);
			this.txtCaption.TabIndex = 2;
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 368);
			this.txtID.Name = "txtID";
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(636, 233);
			this.ttBase.SetToolTip(this.rtfNotes, "Description of Class");
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 212);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Visible = false;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(96, 212);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(516, 24);
			this.cbAlphaSort.TabIndex = 12;
			this.cbAlphaSort.Visible = false;
			//
			//hsbGeneral
			//
			this.hsbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.hsbGeneral.Location = new System.Drawing.Point(616, 214);
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(124, 17);
			this.hsbGeneral.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.cbStatus);
			this.gbGeneral.Controls.Add(this.lblStatus);
			this.gbGeneral.Controls.Add(this.lblURL_Local);
			this.gbGeneral.Controls.Add(this.txtURL_Local);
			this.gbGeneral.Controls.Add(this.lblURL_Internet);
			this.gbGeneral.Controls.Add(this.txtURL_Internet);
			this.gbGeneral.Controls.Add(this.lblCommissioned);
			this.gbGeneral.Controls.Add(this.dtpCommissioned);
			this.gbGeneral.Controls.Add(this.txtZipCode);
			this.gbGeneral.Controls.Add(this.lblZipCode);
			this.gbGeneral.Controls.Add(this.cbHomePort);
			this.gbGeneral.Controls.Add(this.lblHomePort);
			this.gbGeneral.Controls.Add(this.cbCommand);
			this.gbGeneral.Controls.Add(this.lblCommand);
			this.gbGeneral.Controls.Add(this.cbClass);
			this.gbGeneral.Controls.Add(this.lblClass);
			this.gbGeneral.Controls.Add(this.lblDesignation);
			this.gbGeneral.Controls.Add(this.txtClassDesc);
			this.gbGeneral.Controls.Add(this.lblClassification);
			this.gbGeneral.Controls.Add(this.txtNumber);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.txtClassification);
			this.gbGeneral.Controls.Add(this.txtDesignation);
			this.gbGeneral.Controls.Add(this.lblNumber);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(636, 244);
			this.gbGeneral.Controls.SetChildIndex(this.lblNumber, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtClassification, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtNumber, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblClassification, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtClassDesc, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblClass, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbClass, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCommand, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCommand, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblHomePort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbHomePort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblZipCode, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtZipCode, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpCommissioned, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCommissioned, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtURL_Internet, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblURL_Internet, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtURL_Local, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblURL_Local, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblStatus, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbStatus, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 397);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 328);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.TabIndex = 1;
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpCharacteristics);
			this.tcMain.Controls.Add(this.tpWeapons);
			this.tcMain.Controls.Add(this.tpHistory);
			this.tcMain.Controls.Add(this.tpMoreHistory);
			this.tcMain.Controls.Add(this.tpImage);
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(656, 284);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpImage, 0);
			this.tcMain.Controls.SetChildIndex(this.tpMoreHistory, 0);
			this.tcMain.Controls.SetChildIndex(this.tpHistory, 0);
			this.tcMain.Controls.SetChildIndex(this.tpWeapons, 0);
			this.tcMain.Controls.SetChildIndex(this.tpCharacteristics, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 212);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 16;
			this.dtpInventoried.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(16, 212);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.TabIndex = 11;
			this.lblAlphaSort.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 216);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.TabIndex = 7;
			this.lblLocation.Visible = false;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(92, 212);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.TabIndex = 8;
			this.cbLocation.Visible = false;
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(648, 255);
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(652, 270);
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(92, 212);
			this.txtAlphaSort.Name = "txtAlphaSort";
			//
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(108, 48);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(496, 23);
			this.txtName.TabIndex = 3;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			this.ttBase.SetToolTip(this.txtName, "Name of Ship");
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(56, 50);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 19);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			//
			//txtNumber
			//
			this.txtNumber.Location = new System.Drawing.Point(108, 48);
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(64, 23);
			this.txtNumber.TabIndex = 3;
			this.txtNumber.TabStop = false;
			this.txtNumber.Tag = "Required, Numeric";
			this.txtNumber.Text = "txtNumber";
			this.ttBase.SetToolTip(this.txtNumber, "Hull \"Number\" (for sorting purposes)");
			this.txtNumber.Visible = false;
			//
			//lblNumber
			//
			this.lblNumber.AutoSize = true;
			this.lblNumber.Location = new System.Drawing.Point(48, 50);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(58, 19);
			this.lblNumber.TabIndex = 2;
			this.lblNumber.Text = "Number";
			this.lblNumber.Visible = false;
			//
			//tpCharacteristics
			//
			this.tpCharacteristics.Controls.Add(this.gbCharacteristics);
			this.tpCharacteristics.Location = new System.Drawing.Point(4, 25);
			this.tpCharacteristics.Name = "tpCharacteristics";
			this.tpCharacteristics.Size = new System.Drawing.Size(652, 270);
			this.tpCharacteristics.TabIndex = 2;
			this.tpCharacteristics.Text = "Characteristics";
			//
			//gbCharacteristics
			//
			this.gbCharacteristics.Controls.Add(this.txtSpeed);
			this.gbCharacteristics.Controls.Add(this.lblSpeed);
			this.gbCharacteristics.Controls.Add(this.lblManning);
			this.gbCharacteristics.Controls.Add(this.lblBoilers);
			this.gbCharacteristics.Controls.Add(this.lblPropulsion);
			this.gbCharacteristics.Controls.Add(this.lblDraft);
			this.gbCharacteristics.Controls.Add(this.lblBeam);
			this.gbCharacteristics.Controls.Add(this.lblLength);
			this.gbCharacteristics.Controls.Add(this.lblDisplacement);
			this.gbCharacteristics.Controls.Add(this.txtManning);
			this.gbCharacteristics.Controls.Add(this.txtBoilers);
			this.gbCharacteristics.Controls.Add(this.txtPropulsion);
			this.gbCharacteristics.Controls.Add(this.txtDraft);
			this.gbCharacteristics.Controls.Add(this.txtBeam);
			this.gbCharacteristics.Controls.Add(this.txtLength);
			this.gbCharacteristics.Controls.Add(this.txtDisplacement);
			this.gbCharacteristics.Location = new System.Drawing.Point(8, 8);
			this.gbCharacteristics.Name = "gbCharacteristics";
			this.gbCharacteristics.Size = new System.Drawing.Size(636, 244);
			this.gbCharacteristics.TabIndex = 0;
			this.gbCharacteristics.TabStop = false;
			//
			//txtSpeed
			//
			this.txtSpeed.Location = new System.Drawing.Point(116, 217);
			this.txtSpeed.Name = "txtSpeed";
			this.txtSpeed.TabIndex = 15;
			this.txtSpeed.Tag = "Numeric";
			this.txtSpeed.Text = "txtSpeed";
			this.ttBase.SetToolTip(this.txtSpeed, "Maximum Speed (in knots)");
			//
			//lblSpeed
			//
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.Location = new System.Drawing.Point(57, 219);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(47, 19);
			this.lblSpeed.TabIndex = 14;
			this.lblSpeed.Text = "Speed";
			this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblManning
			//
			this.lblManning.AutoSize = true;
			this.lblManning.Location = new System.Drawing.Point(42, 191);
			this.lblManning.Name = "lblManning";
			this.lblManning.Size = new System.Drawing.Size(62, 19);
			this.lblManning.TabIndex = 12;
			this.lblManning.Text = "Manning";
			this.lblManning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblBoilers
			//
			this.lblBoilers.AutoSize = true;
			this.lblBoilers.Location = new System.Drawing.Point(54, 163);
			this.lblBoilers.Name = "lblBoilers";
			this.lblBoilers.Size = new System.Drawing.Size(50, 19);
			this.lblBoilers.TabIndex = 10;
			this.lblBoilers.Text = "Boilers";
			this.lblBoilers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblPropulsion
			//
			this.lblPropulsion.AutoSize = true;
			this.lblPropulsion.Location = new System.Drawing.Point(30, 135);
			this.lblPropulsion.Name = "lblPropulsion";
			this.lblPropulsion.Size = new System.Drawing.Size(74, 19);
			this.lblPropulsion.TabIndex = 8;
			this.lblPropulsion.Text = "Propulsion";
			this.lblPropulsion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblDraft
			//
			this.lblDraft.AutoSize = true;
			this.lblDraft.Location = new System.Drawing.Point(65, 107);
			this.lblDraft.Name = "lblDraft";
			this.lblDraft.Size = new System.Drawing.Size(39, 19);
			this.lblDraft.TabIndex = 6;
			this.lblDraft.Text = "Draft";
			this.lblDraft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblBeam
			//
			this.lblBeam.AutoSize = true;
			this.lblBeam.Location = new System.Drawing.Point(61, 79);
			this.lblBeam.Name = "lblBeam";
			this.lblBeam.Size = new System.Drawing.Size(43, 19);
			this.lblBeam.TabIndex = 4;
			this.lblBeam.Text = "Beam";
			this.lblBeam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblLength
			//
			this.lblLength.AutoSize = true;
			this.lblLength.Location = new System.Drawing.Point(53, 51);
			this.lblLength.Name = "lblLength";
			this.lblLength.Size = new System.Drawing.Size(51, 19);
			this.lblLength.TabIndex = 2;
			this.lblLength.Text = "Length";
			this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblDisplacement
			//
			this.lblDisplacement.AutoSize = true;
			this.lblDisplacement.Location = new System.Drawing.Point(8, 23);
			this.lblDisplacement.Name = "lblDisplacement";
			this.lblDisplacement.Size = new System.Drawing.Size(96, 19);
			this.lblDisplacement.TabIndex = 0;
			this.lblDisplacement.Text = "Displacement";
			this.lblDisplacement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//txtManning
			//
			this.txtManning.Location = new System.Drawing.Point(116, 188);
			this.txtManning.Name = "txtManning";
			this.txtManning.Size = new System.Drawing.Size(492, 23);
			this.txtManning.TabIndex = 13;
			this.txtManning.Text = "txtManning";
			this.ttBase.SetToolTip(this.txtManning, "Ship's Complement (size of crew)");
			//
			//txtBoilers
			//
			this.txtBoilers.Location = new System.Drawing.Point(116, 160);
			this.txtBoilers.Name = "txtBoilers";
			this.txtBoilers.Size = new System.Drawing.Size(492, 23);
			this.txtBoilers.TabIndex = 11;
			this.txtBoilers.Text = "txtBoilers";
			this.ttBase.SetToolTip(this.txtBoilers, "Number of Boilers - if applicable)");
			//
			//txtPropulsion
			//
			this.txtPropulsion.Location = new System.Drawing.Point(116, 132);
			this.txtPropulsion.Name = "txtPropulsion";
			this.txtPropulsion.Size = new System.Drawing.Size(492, 23);
			this.txtPropulsion.TabIndex = 9;
			this.txtPropulsion.Text = "txtPropulsion";
			this.ttBase.SetToolTip(this.txtPropulsion, "Propulsion");
			//
			//txtDraft
			//
			this.txtDraft.Location = new System.Drawing.Point(116, 104);
			this.txtDraft.Name = "txtDraft";
			this.txtDraft.Size = new System.Drawing.Size(492, 23);
			this.txtDraft.TabIndex = 7;
			this.txtDraft.Text = "txtDraft";
			this.ttBase.SetToolTip(this.txtDraft, "Draft (i.e. depth below waterline - in feet)");
			//
			//txtBeam
			//
			this.txtBeam.Location = new System.Drawing.Point(116, 76);
			this.txtBeam.Name = "txtBeam";
			this.txtBeam.Size = new System.Drawing.Size(492, 23);
			this.txtBeam.TabIndex = 5;
			this.txtBeam.Text = "txtBeam";
			this.ttBase.SetToolTip(this.txtBeam, "Beam (i.e. Width - in feet)");
			//
			//txtLength
			//
			this.txtLength.Location = new System.Drawing.Point(116, 48);
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(492, 23);
			this.txtLength.TabIndex = 3;
			this.txtLength.Text = "txtLength";
			this.ttBase.SetToolTip(this.txtLength, "Length (in feet)");
			//
			//txtDisplacement
			//
			this.txtDisplacement.Location = new System.Drawing.Point(116, 20);
			this.txtDisplacement.Name = "txtDisplacement";
			this.txtDisplacement.Size = new System.Drawing.Size(492, 23);
			this.txtDisplacement.TabIndex = 1;
			this.txtDisplacement.Text = "txtDisplacement";
			this.ttBase.SetToolTip(this.txtDisplacement, "Displacement (in tons)");
			//
			//tpWeapons
			//
			this.tpWeapons.Controls.Add(this.gbWeapons);
			this.tpWeapons.Location = new System.Drawing.Point(4, 25);
			this.tpWeapons.Name = "tpWeapons";
			this.tpWeapons.Size = new System.Drawing.Size(652, 270);
			this.tpWeapons.TabIndex = 3;
			this.tpWeapons.Text = "Weapons";
			//
			//gbWeapons
			//
			this.gbWeapons.Controls.Add(this.txtEW);
			this.gbWeapons.Controls.Add(this.lblEW);
			this.gbWeapons.Controls.Add(this.lblFireControl);
			this.gbWeapons.Controls.Add(this.lblSONARs);
			this.gbWeapons.Controls.Add(this.lblRADARs);
			this.gbWeapons.Controls.Add(this.lblASW);
			this.gbWeapons.Controls.Add(this.lblGuns);
			this.gbWeapons.Controls.Add(this.lblMissiles);
			this.gbWeapons.Controls.Add(this.lblAircraft);
			this.gbWeapons.Controls.Add(this.txtFireControl);
			this.gbWeapons.Controls.Add(this.txtSONARs);
			this.gbWeapons.Controls.Add(this.txtRADARs);
			this.gbWeapons.Controls.Add(this.txtASW);
			this.gbWeapons.Controls.Add(this.txtGuns);
			this.gbWeapons.Controls.Add(this.txtMissiles);
			this.gbWeapons.Controls.Add(this.txtAircraft);
			this.gbWeapons.Location = new System.Drawing.Point(8, 8);
			this.gbWeapons.Name = "gbWeapons";
			this.gbWeapons.Size = new System.Drawing.Size(636, 244);
			this.gbWeapons.TabIndex = 0;
			this.gbWeapons.TabStop = false;
			//
			//txtEW
			//
			this.txtEW.Location = new System.Drawing.Point(104, 209);
			this.txtEW.Name = "txtEW";
			this.txtEW.Size = new System.Drawing.Size(500, 23);
			this.txtEW.TabIndex = 15;
			this.txtEW.Text = "txtEW";
			this.ttBase.SetToolTip(this.txtEW, "Electronic Warfare Countermeasures");
			//
			//lblEW
			//
			this.lblEW.AutoSize = true;
			this.lblEW.Location = new System.Drawing.Point(68, 212);
			this.lblEW.Name = "lblEW";
			this.lblEW.Size = new System.Drawing.Size(27, 19);
			this.lblEW.TabIndex = 14;
			this.lblEW.Text = "EW";
			this.lblEW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblFireControl
			//
			this.lblFireControl.AutoSize = true;
			this.lblFireControl.Location = new System.Drawing.Point(12, 184);
			this.lblFireControl.Name = "lblFireControl";
			this.lblFireControl.Size = new System.Drawing.Size(83, 19);
			this.lblFireControl.TabIndex = 12;
			this.lblFireControl.Text = "Fire Control";
			this.lblFireControl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblSONARs
			//
			this.lblSONARs.AutoSize = true;
			this.lblSONARs.Location = new System.Drawing.Point(35, 156);
			this.lblSONARs.Name = "lblSONARs";
			this.lblSONARs.Size = new System.Drawing.Size(60, 19);
			this.lblSONARs.TabIndex = 10;
			this.lblSONARs.Text = "SONARs";
			this.lblSONARs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblRADARs
			//
			this.lblRADARs.AutoSize = true;
			this.lblRADARs.Location = new System.Drawing.Point(36, 128);
			this.lblRADARs.Name = "lblRADARs";
			this.lblRADARs.Size = new System.Drawing.Size(59, 19);
			this.lblRADARs.TabIndex = 8;
			this.lblRADARs.Text = "RADARs";
			this.lblRADARs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblASW
			//
			this.lblASW.AutoSize = true;
			this.lblASW.Location = new System.Drawing.Point(59, 100);
			this.lblASW.Name = "lblASW";
			this.lblASW.Size = new System.Drawing.Size(36, 19);
			this.lblASW.TabIndex = 6;
			this.lblASW.Text = "ASW";
			this.lblASW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblGuns
			//
			this.lblGuns.AutoSize = true;
			this.lblGuns.Location = new System.Drawing.Point(56, 72);
			this.lblGuns.Name = "lblGuns";
			this.lblGuns.Size = new System.Drawing.Size(39, 19);
			this.lblGuns.TabIndex = 4;
			this.lblGuns.Text = "Guns";
			this.lblGuns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblMissiles
			//
			this.lblMissiles.AutoSize = true;
			this.lblMissiles.Location = new System.Drawing.Point(39, 44);
			this.lblMissiles.Name = "lblMissiles";
			this.lblMissiles.Size = new System.Drawing.Size(56, 19);
			this.lblMissiles.TabIndex = 2;
			this.lblMissiles.Text = "Missiles";
			this.lblMissiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblAircraft
			//
			this.lblAircraft.AutoSize = true;
			this.lblAircraft.Location = new System.Drawing.Point(41, 16);
			this.lblAircraft.Name = "lblAircraft";
			this.lblAircraft.Size = new System.Drawing.Size(54, 19);
			this.lblAircraft.TabIndex = 0;
			this.lblAircraft.Text = "Aircraft";
			this.lblAircraft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//txtFireControl
			//
			this.txtFireControl.Location = new System.Drawing.Point(104, 181);
			this.txtFireControl.Name = "txtFireControl";
			this.txtFireControl.Size = new System.Drawing.Size(500, 23);
			this.txtFireControl.TabIndex = 13;
			this.txtFireControl.Text = "txtFireControl";
			this.ttBase.SetToolTip(this.txtFireControl, "Types of Fire Control Equipment");
			//
			//txtSONARs
			//
			this.txtSONARs.Location = new System.Drawing.Point(104, 153);
			this.txtSONARs.Name = "txtSONARs";
			this.txtSONARs.Size = new System.Drawing.Size(500, 23);
			this.txtSONARs.TabIndex = 11;
			this.txtSONARs.Text = "txtSONARs";
			this.ttBase.SetToolTip(this.txtSONARs, "Types of SONAR Equipped");
			//
			//txtRADARs
			//
			this.txtRADARs.Location = new System.Drawing.Point(104, 125);
			this.txtRADARs.Name = "txtRADARs";
			this.txtRADARs.Size = new System.Drawing.Size(500, 23);
			this.txtRADARs.TabIndex = 9;
			this.txtRADARs.Text = "txtRADARs";
			this.ttBase.SetToolTip(this.txtRADARs, "Types of RADAR Equipped");
			//
			//txtASW
			//
			this.txtASW.Location = new System.Drawing.Point(104, 97);
			this.txtASW.Name = "txtASW";
			this.txtASW.Size = new System.Drawing.Size(500, 23);
			this.txtASW.TabIndex = 7;
			this.txtASW.Text = "txtASW";
			this.ttBase.SetToolTip(this.txtASW, "Anti-Submarine Warfare Weapons");
			//
			//txtGuns
			//
			this.txtGuns.Location = new System.Drawing.Point(104, 69);
			this.txtGuns.Name = "txtGuns";
			this.txtGuns.Size = new System.Drawing.Size(500, 23);
			this.txtGuns.TabIndex = 5;
			this.txtGuns.Text = "txtGuns";
			this.ttBase.SetToolTip(this.txtGuns, "Guns Equipped");
			//
			//txtMissiles
			//
			this.txtMissiles.Location = new System.Drawing.Point(104, 41);
			this.txtMissiles.Name = "txtMissiles";
			this.txtMissiles.Size = new System.Drawing.Size(500, 23);
			this.txtMissiles.TabIndex = 3;
			this.txtMissiles.Text = "txtMissiles";
			this.ttBase.SetToolTip(this.txtMissiles, "Missile complement");
			//
			//txtAircraft
			//
			this.txtAircraft.Location = new System.Drawing.Point(104, 13);
			this.txtAircraft.Name = "txtAircraft";
			this.txtAircraft.Size = new System.Drawing.Size(500, 23);
			this.txtAircraft.TabIndex = 1;
			this.txtAircraft.Text = "txtAircraft";
			this.ttBase.SetToolTip(this.txtAircraft, "Number of Aircraft carried");
			//
			//lblClassification
			//
			this.lblClassification.AutoSize = true;
			this.lblClassification.Location = new System.Drawing.Point(12, 107);
			this.lblClassification.Name = "lblClassification";
			this.lblClassification.Size = new System.Drawing.Size(92, 19);
			this.lblClassification.TabIndex = 4;
			this.lblClassification.Text = "Classification";
			//
			//txtClassDesc
			//
			this.txtClassDesc.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtClassDesc.Location = new System.Drawing.Point(264, 105);
			this.txtClassDesc.Name = "txtClassDesc";
			this.txtClassDesc.ReadOnly = true;
			this.txtClassDesc.Size = new System.Drawing.Size(344, 23);
			this.txtClassDesc.TabIndex = 5;
			this.txtClassDesc.TabStop = false;
			this.txtClassDesc.Tag = "";
			this.txtClassDesc.Text = "txtClassDesc";
			this.ttBase.SetToolTip(this.txtClassDesc, "Description of Class/Ship");
			//
			//tpHistory
			//
			this.tpHistory.Controls.Add(this.rtfHistory);
			this.tpHistory.Location = new System.Drawing.Point(4, 25);
			this.tpHistory.Name = "tpHistory";
			this.tpHistory.Size = new System.Drawing.Size(652, 270);
			this.tpHistory.TabIndex = 4;
			this.tpHistory.Text = "History";
			//
			//rtfHistory
			//
			this.rtfHistory.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfHistory.Location = new System.Drawing.Point(4, 4);
			this.rtfHistory.Name = "rtfHistory";
			this.rtfHistory.Size = new System.Drawing.Size(644, 263);
			this.rtfHistory.TabIndex = 0;
			this.rtfHistory.Text = "rtfHistory";
			//
			//tpImage
			//
			this.tpImage.Location = new System.Drawing.Point(4, 25);
			this.tpImage.Name = "tpImage";
			this.tpImage.Size = new System.Drawing.Size(652, 270);
			this.tpImage.TabIndex = 5;
			this.tpImage.Text = "Image";
			//
			//txtClassification
			//
			this.txtClassification.Enabled = false;
			this.txtClassification.Location = new System.Drawing.Point(108, 105);
			this.txtClassification.Name = "txtClassification";
			this.txtClassification.Size = new System.Drawing.Size(136, 23);
			this.txtClassification.TabIndex = 82;
			this.txtClassification.TabStop = false;
			this.txtClassification.Tag = "Ignore";
			this.txtClassification.Text = "txtClassification";
			this.ttBase.SetToolTip(this.txtClassification, "Description of Class/Ship");
			//
			//tpMoreHistory
			//
			this.tpMoreHistory.Controls.Add(this.rtfMoreHistory);
			this.tpMoreHistory.Location = new System.Drawing.Point(4, 25);
			this.tpMoreHistory.Name = "tpMoreHistory";
			this.tpMoreHistory.Size = new System.Drawing.Size(652, 270);
			this.tpMoreHistory.TabIndex = 6;
			this.tpMoreHistory.Text = "More History";
			//
			//rtfMoreHistory
			//
			this.rtfMoreHistory.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfMoreHistory.Location = new System.Drawing.Point(4, 4);
			this.rtfMoreHistory.Name = "rtfMoreHistory";
			this.rtfMoreHistory.Size = new System.Drawing.Size(644, 263);
			this.rtfMoreHistory.TabIndex = 1;
			this.rtfMoreHistory.Text = "rtfMoreHistory";
			//
			//txtDesignation
			//
			this.txtDesignation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtDesignation.Location = new System.Drawing.Point(108, 20);
			this.txtDesignation.Name = "txtDesignation";
			this.txtDesignation.Size = new System.Drawing.Size(128, 23);
			this.txtDesignation.TabIndex = 1;
			this.txtDesignation.Tag = "Required";
			this.txtDesignation.Text = "TXTDESIGNATION";
			this.ttBase.SetToolTip(this.txtDesignation, "Ship's designation or Hull Number");
			//
			//lblDesignation
			//
			this.lblDesignation.AutoSize = true;
			this.lblDesignation.Location = new System.Drawing.Point(16, 24);
			this.lblDesignation.Name = "lblDesignation";
			this.lblDesignation.Size = new System.Drawing.Size(84, 19);
			this.lblDesignation.TabIndex = 83;
			this.lblDesignation.Text = "Designation";
			//
			//cbClass
			//
			this.cbClass.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbClass.Location = new System.Drawing.Point(108, 76);
			this.cbClass.Name = "cbClass";
			this.cbClass.Size = new System.Drawing.Size(500, 24);
			this.cbClass.TabIndex = 4;
			this.cbClass.Tag = "Required";
			//
			//lblClass
			//
			this.lblClass.AutoSize = true;
			this.lblClass.Location = new System.Drawing.Point(60, 79);
			this.lblClass.Name = "lblClass";
			this.lblClass.Size = new System.Drawing.Size(40, 19);
			this.lblClass.TabIndex = 84;
			this.lblClass.Text = "Class";
			//
			//cbCommand
			//
			this.cbCommand.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cbCommand.Location = new System.Drawing.Point(424, 132);
			this.cbCommand.Name = "cbCommand";
			this.cbCommand.Size = new System.Drawing.Size(184, 24);
			this.cbCommand.TabIndex = 7;
			//
			//lblCommand
			//
			this.lblCommand.AutoSize = true;
			this.lblCommand.Location = new System.Drawing.Point(348, 136);
			this.lblCommand.Name = "lblCommand";
			this.lblCommand.Size = new System.Drawing.Size(73, 19);
			this.lblCommand.TabIndex = 85;
			this.lblCommand.Text = "Command";
			//
			//cbHomePort
			//
			this.cbHomePort.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbHomePort.Location = new System.Drawing.Point(108, 160);
			this.cbHomePort.Name = "cbHomePort";
			this.cbHomePort.Size = new System.Drawing.Size(240, 24);
			this.cbHomePort.TabIndex = 8;
			//
			//lblHomePort
			//
			this.lblHomePort.AutoSize = true;
			this.lblHomePort.Location = new System.Drawing.Point(28, 163);
			this.lblHomePort.Name = "lblHomePort";
			this.lblHomePort.Size = new System.Drawing.Size(76, 19);
			this.lblHomePort.TabIndex = 87;
			this.lblHomePort.Text = "Home Port";
			//
			//txtZipCode
			//
			this.txtZipCode.Location = new System.Drawing.Point(424, 161);
			this.txtZipCode.Name = "txtZipCode";
			this.txtZipCode.Size = new System.Drawing.Size(180, 23);
			this.txtZipCode.TabIndex = 9;
			this.txtZipCode.Tag = "";
			this.txtZipCode.Text = "txtZipCode";
			//
			//lblZipCode
			//
			this.lblZipCode.AutoSize = true;
			this.lblZipCode.Location = new System.Drawing.Point(356, 163);
			this.lblZipCode.Name = "lblZipCode";
			this.lblZipCode.Size = new System.Drawing.Size(65, 19);
			this.lblZipCode.TabIndex = 90;
			this.lblZipCode.Text = "Zip Code";
			//
			//dtpCommissioned
			//
			this.dtpCommissioned.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpCommissioned.Location = new System.Drawing.Point(472, 20);
			this.dtpCommissioned.Name = "dtpCommissioned";
			this.dtpCommissioned.Size = new System.Drawing.Size(132, 23);
			this.dtpCommissioned.TabIndex = 2;
			//
			//lblCommissioned
			//
			this.lblCommissioned.AutoSize = true;
			this.lblCommissioned.Location = new System.Drawing.Point(364, 22);
			this.lblCommissioned.Name = "lblCommissioned";
			this.lblCommissioned.Size = new System.Drawing.Size(103, 19);
			this.lblCommissioned.TabIndex = 92;
			this.lblCommissioned.Text = "Commissioned";
			//
			//lblURL_Internet
			//
			this.lblURL_Internet.AutoSize = true;
			this.lblURL_Internet.Location = new System.Drawing.Point(44, 190);
			this.lblURL_Internet.Name = "lblURL_Internet";
			this.lblURL_Internet.Size = new System.Drawing.Size(60, 19);
			this.lblURL_Internet.TabIndex = 93;
			this.lblURL_Internet.Text = "WebSite";
			//
			//txtURL_Internet
			//
			this.txtURL_Internet.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtURL_Internet.Location = new System.Drawing.Point(108, 188);
			this.txtURL_Internet.Name = "txtURL_Internet";
			this.txtURL_Internet.Size = new System.Drawing.Size(496, 23);
			this.txtURL_Internet.TabIndex = 10;
			this.txtURL_Internet.Tag = "";
			this.txtURL_Internet.Text = "txtURL_Internet";
			this.ttBase.SetToolTip(this.txtURL_Internet, "Web Address (Internet)");
			//
			//lblURL_Local
			//
			this.lblURL_Local.AutoSize = true;
			this.lblURL_Local.Location = new System.Drawing.Point(5, 218);
			this.lblURL_Local.Name = "lblURL_Local";
			this.lblURL_Local.Size = new System.Drawing.Size(99, 19);
			this.lblURL_Local.TabIndex = 95;
			this.lblURL_Local.Text = "Local WebSite";
			//
			//txtURL_Local
			//
			this.txtURL_Local.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtURL_Local.Location = new System.Drawing.Point(108, 216);
			this.txtURL_Local.Name = "txtURL_Local";
			this.txtURL_Local.Size = new System.Drawing.Size(496, 23);
			this.txtURL_Local.TabIndex = 11;
			this.txtURL_Local.Tag = "";
			this.txtURL_Local.Text = "txtURL_Local";
			this.ttBase.SetToolTip(this.txtURL_Local, "Web Address (Local)");
			//
			//cbStatus
			//
			this.cbStatus.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbStatus.Location = new System.Drawing.Point(108, 132);
			this.cbStatus.Name = "cbStatus";
			this.cbStatus.Size = new System.Drawing.Size(240, 24);
			this.cbStatus.TabIndex = 6;
			this.cbStatus.Text = "cbStatus";
			this.ttBase.SetToolTip(this.cbStatus, "Current/Final Status of this Ship");
			//
			//lblStatus
			//
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(56, 135);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(48, 19);
			this.lblStatus.TabIndex = 97;
			this.lblStatus.Text = "Status";
			//
			//frmShips
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 419);
			this.Name = "frmShips";
			this.Text = "frmShips";
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			this.gbGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tpNotes.ResumeLayout(false);
			this.tpCharacteristics.ResumeLayout(false);
			this.gbCharacteristics.ResumeLayout(false);
			this.tpWeapons.ResumeLayout(false);
			this.gbWeapons.ResumeLayout(false);
			this.tpHistory.ResumeLayout(false);
			this.tpMoreHistory.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		#region "Properties"
		#region "Declarations"
		private int mStatusActiveWidth;
		private int mStatusInactiveWidth;
		private int mURLInternetActivePos;
		private int mURLLocalActivePos;
		private int mURLInternetInactivePos;
			#endregion
		private int mURLLocalInactivePos;
		#endregion
		#region "Methods"
		protected override void BindControls()
		{
			BindControl(txtID, mTCBase.MainDataView, "ID");
			//General
			BindControl(txtDesignation, mTCBase.MainDataView, "HullNumber");
			BindControl(txtNumber, mTCBase.MainDataView, "Number");
			BindControl(txtName, mTCBase.MainDataView, "Name");
			//BindControl(cbClassification, mTCBase.MainDataView, "ClassificationID", CType(mTCBase,clsShips).Classifications, "Type", "ID")
			BindControl(txtClassification, mTCBase.MainDataView, "Classification");
			BindControl(cbClass, mTCBase.MainDataView, "ClassID", ((clsShips)mTCBase).Classes, "Name", "ID");
			//BindControl(txtClassDesc, mTCBase.MainDataView, "ClassificationID", CType(mTCBase,clsShips).Classifications, "Description", "Description")
			BindControl(dtpCommissioned, mTCBase.MainDataView, "Commissioned");
			BindControl(cbStatus, mTCBase.MainDataView, "Status", ((clsShips)mTCBase).Statuses, "Status", "Status");
			BindControl(cbCommand, mTCBase.MainDataView, "Command", ((clsShips)mTCBase).Commands, "Command", "Command");
			BindControl(cbHomePort, mTCBase.MainDataView, "HomePort", ((clsShips)mTCBase).HomePorts, "HomePort", "HomePort");
			BindControl(txtZipCode, mTCBase.MainDataView, "Zip Code");
			BindControl(txtURL_Internet, mTCBase.MainDataView, "URL_Internet");
			BindControl(txtURL_Local, mTCBase.MainDataView, "URL_Local");
			//Characteristics...
			BindControl(txtDisplacement, mTCBase.MainDataView, "Displacement");
			BindControl(txtLength, mTCBase.MainDataView, "Length");
			BindControl(txtBeam, mTCBase.MainDataView, "Beam");
			BindControl(txtDraft, mTCBase.MainDataView, "Draft");
			BindControl(txtPropulsion, mTCBase.MainDataView, "Propulsion");
			BindControl(txtBoilers, mTCBase.MainDataView, "Boilers");
			BindControl(txtManning, mTCBase.MainDataView, "Manning");
			BindControl(txtSpeed, mTCBase.MainDataView, "Speed");
			//Weapons...
			BindControl(txtAircraft, mTCBase.MainDataView, "Aircraft");
			BindControl(txtMissiles, mTCBase.MainDataView, "Missiles");
			BindControl(txtGuns, mTCBase.MainDataView, "Guns");
			BindControl(txtASW, mTCBase.MainDataView, "ASW Weapons");
			BindControl(txtRADARs, mTCBase.MainDataView, "Radars");
			BindControl(txtSONARs, mTCBase.MainDataView, "Sonars");
			BindControl(txtFireControl, mTCBase.MainDataView, "Fire Control");
			BindControl(txtEW, mTCBase.MainDataView, "EW");
			//History...
			BindControl(rtfHistory, mTCBase.MainDataView, "History");
			//More History...
			BindControl(rtfMoreHistory, mTCBase.MainDataView, "More History");
			//Notes...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		protected void HideStuff(bool fInactive)
		{
			if (fInactive) {
				lblCommand.Visible = false;
				cbCommand.Visible = false;
				lblHomePort.Visible = false;
				cbHomePort.Visible = false;
				lblZipCode.Visible = false;
				txtZipCode.Visible = false;
				//Reposition remaining controls to fill the now-vacated area...
				cbStatus.Width = mStatusInactiveWidth;
				txtURL_Internet.Top = mURLInternetInactivePos;
				txtURL_Local.Top = mURLLocalInactivePos;
			} else {
				lblCommand.Visible = true;
				cbCommand.Visible = true;
				lblHomePort.Visible = true;
				cbHomePort.Visible = true;
				lblZipCode.Visible = true;
				txtZipCode.Visible = true;
				//Reposition remaining controls to make room for the now-visible area...
				cbStatus.Width = mStatusActiveWidth;
				txtURL_Internet.Top = mURLInternetActivePos;
				txtURL_Local.Top = mURLLocalActivePos;
			}
			lblURL_Internet.Top = txtURL_Internet.Top + ((txtURL_Internet.Height - lblURL_Internet.Height) / 2);
			lblURL_Local.Top = txtURL_Local.Top + ((txtURL_Local.Height - lblURL_Local.Height) / 2);
		}
		#endregion
		#region "Event Handlers"
		protected override void ActionModeChange(object sender, ActionModeChangeEventArgs e)
		{
			base.ActionModeChange(sender, e);
			switch (e.newMode) {
				case clsTCBase.ActionModeEnum.modeDelete:
					break;
				case clsTCBase.ActionModeEnum.modeDisplay:
					Trace("Removing field default handlers...", trcOption.trcApplication);
					txtDesignation.Validating -= txtDesignation_Validate;
					txtDesignation.Validating -= DefaultTextBox;
					cbClass.Validating -= DefaultClassDetails;
					break;
				default:
					Trace("Adding field default handlers...", trcOption.trcApplication);
					txtDesignation.Validating += txtDesignation_Validate;
					txtDesignation.Validating += DefaultTextBox;
					cbClass.Validating += DefaultClassDetails;
					break;
			}
		}
		protected override void AfterMove(object sender, RowChangeEventArgs e)
		{
			base.AfterMove(sender, e);

			HideStuff(!Convert.ToString(mTCBase.CurrentRow["Status"]).StartsWith("Active"));
			DataView dvClassification = ((clsShips)mTCBase).Classifications;
			foreach (DataRow cRow in dvClassification.Table.Rows) {
				if (!Information.IsDBNull(cRow["Type"]) && cRow["Type"] == mTCBase.CurrentRow["Classification"]){this.txtClassDesc.Text = (string)cRow["Description"];break; 
}
			}
		}
		protected new void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			try {
				foreach (Control ctl in this.gbGeneral.Controls) {
					this.epBase.SetError(ctl, bpeNullString);
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void cbStatus_Validate(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);

				HideStuff(!cbStatus.Text.StartsWith("Active"));
			} catch (Exception ex) {
				epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		private void DefaultClassDetails(object sender, CancelEventArgs e)
		{
			const string EntryName = "DefaultClassDetails";
			try {
				CurrencyManager cm = (CurrencyManager)this.BindingContext[mTCBase.MainDataView];

				base.epBase.SetError((Control)sender, bpeNullString);

				switch (mTCBase.Mode) {
					case clsTCBase.ActionModeEnum.modeDelete:
					case clsTCBase.ActionModeEnum.modeDisplay:
                        throw new ExitTryException();

						break;
				}
				Trace("{0}: cmPos:={1}", EntryName, cm.Position, trcOption.trcApplication);

				if ((cbClass.SelectedValue != null)) {
					//Default null fields with the corresponding values from the class definition...
					DataView dvClass = (DataView)this.cbClass.DataSource;
					foreach (DataRow cRow in dvClass.Table.Rows) {
						if (cRow[cbClass.ValueMember] == cbClass.SelectedValue) {
							//Characteristics...
							if (this.txtDisplacement.Text == bpeNullString && !Information.IsDBNull(cRow["Displacement"]))
								mTCBase.CurrentRow["Displacement"] = cRow["Displacement"];
							if (this.txtLength.Text == bpeNullString && !Information.IsDBNull(cRow["Length"]))
								mTCBase.CurrentRow["Length"] = cRow["Length"];
							if (this.txtBeam.Text == bpeNullString && !Information.IsDBNull(cRow["Beam"]))
								mTCBase.CurrentRow["Beam"] = cRow["Beam"];
							if (this.txtDraft.Text == bpeNullString && !Information.IsDBNull(cRow["Draft"]))
								mTCBase.CurrentRow["Draft"] = cRow["Draft"];
							if (this.txtPropulsion.Text == bpeNullString && !Information.IsDBNull(cRow["Propulsion"]))
								mTCBase.CurrentRow["Propulsion"] = cRow["Propulsion"];
							if (this.txtBoilers.Text == bpeNullString && !Information.IsDBNull(cRow["Boilers"]))
								mTCBase.CurrentRow["Boilers"] = cRow["Boilers"];
							if (this.txtSpeed.Text == bpeNullString && !Information.IsDBNull(cRow["Speed"]))
								mTCBase.CurrentRow["Speed"] = cRow["Speed"];
							if (this.txtManning.Text == bpeNullString && !Information.IsDBNull(cRow["Manning"]))
								mTCBase.CurrentRow["Manning"] = cRow["Manning"];
							//Weapons...
							if (this.txtAircraft.Text == bpeNullString && !Information.IsDBNull(cRow["Aircraft"]))
								mTCBase.CurrentRow["Aircraft"] = cRow["Aircraft"];
							if (this.txtMissiles.Text == bpeNullString && !Information.IsDBNull(cRow["Missiles"]))
								mTCBase.CurrentRow["Missiles"] = cRow["Missiles"];
							if (this.txtGuns.Text == bpeNullString && !Information.IsDBNull(cRow["Guns"]))
								mTCBase.CurrentRow["Guns"] = cRow["Guns"];
							if (this.txtASW.Text == bpeNullString && !Information.IsDBNull(cRow["ASW Weapons"]))
								mTCBase.CurrentRow["ASW Weapons"] = cRow["ASW Weapons"];
							if (this.txtRADARs.Text == bpeNullString && !Information.IsDBNull(cRow["Radars"]))
								mTCBase.CurrentRow["Radars"] = cRow["Radars"];
							if (this.txtSONARs.Text == bpeNullString && !Information.IsDBNull(cRow["Sonars"]))
								mTCBase.CurrentRow["Sonars"] = cRow["Sonars"];
							if (this.txtFireControl.Text == bpeNullString && !Information.IsDBNull(cRow["Fire Control"]))
								mTCBase.CurrentRow["Fire Control"] = cRow["Fire Control"];
							if (this.txtEW.Text == bpeNullString && !Information.IsDBNull(cRow["EW"]))
								mTCBase.CurrentRow["EW"] = cRow["EW"];
							//History...
							if (this.rtfHistory.Text == bpeNullString && !Information.IsDBNull(cRow["Description"]))
								mTCBase.CurrentRow["History"] = cRow["Description"];

							if (!this.txtDesignation.Text.StartsWith((string)cRow["Classification"])) {
								string Message = string.Format("Designation does not reflect {0} [class] classification \"{1}\".", cRow["Name"], cRow["Classification"]);
								if (MessageBox.Show(this, string.Format("{0}  Do you want to override?", Message), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
									throw new ApplicationException(Message);
							}
							break;
						}
					}
				}
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmShips.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				RemoveControlHandlers(txtDesignation);
				txtDesignation.Validating -= SetCaption;
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(cbClass);
				cbClass.Validating -= SetCaption;
				RemoveControlHandlers(txtClassDesc);
				RemoveControlHandlers(cbStatus);
				cbStatus.Validating -= cbStatus_Validate;
				RemoveControlHandlers(cbCommand);
				RemoveControlHandlers(cbHomePort);
				RemoveControlHandlers(txtZipCode);
				RemoveControlHandlers(txtURL_Internet);
				RemoveControlHandlers(txtURL_Local);
				//Characteristics...
				RemoveControlHandlers(txtDisplacement);
				RemoveControlHandlers(txtLength);
				RemoveControlHandlers(txtBeam);
				RemoveControlHandlers(txtDraft);
				RemoveControlHandlers(txtPropulsion);
				RemoveControlHandlers(txtBoilers);
				RemoveControlHandlers(txtManning);
				RemoveControlHandlers(txtSpeed);
				//Weapons...
				RemoveControlHandlers(txtAircraft);
				RemoveControlHandlers(txtMissiles);
				RemoveControlHandlers(txtGuns);
				RemoveControlHandlers(txtASW);
				RemoveControlHandlers(txtRADARs);
				RemoveControlHandlers(txtSONARs);
				RemoveControlHandlers(txtFireControl);
				RemoveControlHandlers(txtEW);
				//History...
				RemoveControlHandlers(rtfHistory);
				//More History...
				RemoveControlHandlers(rtfMoreHistory);
				//Notes...
				RemoveControlHandlers(rtfNotes);

				mTCBase.ActionModeChange -= ActionModeChange;
				mTCBase.BeforeMove -= BeforeMove;
				mTCBase.AfterMove -= AfterMove;
			} catch (ExitTryException ex) {
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected override void Form_Load(object sender, System.EventArgs e)
		{
			const string EntryName = "frmShips.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				SetupControlHandlers(txtDesignation);
				txtDesignation.Validating += SetCaption;
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(cbClass);
				cbClass.Validating += SetCaption;
				SetupControlHandlers(txtClassDesc);
				SetupControlHandlers(cbStatus);
				cbStatus.Validating += cbStatus_Validate;
				SetupControlHandlers(cbCommand);
				SetupControlHandlers(cbHomePort);
				SetupControlHandlers(txtZipCode);
				SetupControlHandlers(txtURL_Internet);
				SetupControlHandlers(txtURL_Local);
				//Characteristics...
				SetupControlHandlers(txtDisplacement);
				SetupControlHandlers(txtLength);
				SetupControlHandlers(txtBeam);
				SetupControlHandlers(txtDraft);
				SetupControlHandlers(txtPropulsion);
				SetupControlHandlers(txtBoilers);
				SetupControlHandlers(txtManning);
				SetupControlHandlers(txtSpeed);
				//Weapons...
				SetupControlHandlers(txtAircraft);
				SetupControlHandlers(txtMissiles);
				SetupControlHandlers(txtGuns);
				SetupControlHandlers(txtASW);
				SetupControlHandlers(txtRADARs);
				SetupControlHandlers(txtSONARs);
				SetupControlHandlers(txtFireControl);
				SetupControlHandlers(txtEW);
				//History...
				SetupControlHandlers(rtfHistory);
				//More History...
				SetupControlHandlers(rtfMoreHistory);
				//Notes...
				SetupControlHandlers(rtfNotes);

				mTCBase.ActionModeChange += ActionModeChange;
				mTCBase.BeforeMove += BeforeMove;
				mTCBase.AfterMove += AfterMove;
				//Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.Size.Height + mTCBase.DynamicMenuHeight)

				mTCBase.Move(mTCBase.FindRowByID((int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), string.Format("{0}.{1}", mTCBase.TableName, mTCBase.TableIDColumn), 0)));
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected override void SetCaption(object sender, CancelEventArgs e)
		{
			try {
				//If Not IsNothing(sender) Then MyBase.epBase.SetError((Control)sender, bpeNullString)

				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["HullNumber"]) ? bpeNullString : mTCBase.CurrentRow["HullNumber"] + " ") + mTCBase.CurrentRow["Name"];
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				if ((sender != null))
					base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		private void txtDesignation_Validate(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);

				//Update hidden txtNumber to keep data current...
				char[] separators = { ' ' };
				//{" ", "-"}
				int iPos = this.txtDesignation.Text.IndexOfAny(separators);
				if (iPos > -1) {
					try {
						string testNumber = this.txtDesignation.Text.Substring(iPos + 1);
						if (testNumber.EndsWith(" 1/2")) {
							mTCBase.CurrentRow["Number"] = Convert.ToInt32(testNumber.Replace(" 1/2", bpeNullString)) + ".5";
						} else {
							mTCBase.CurrentRow["Number"] = Convert.ToInt32(testNumber);
						}
					} catch (Exception ex) {
						throw new ApplicationException("Inappropriate designation (unable to parse number from classification)");
					}

					bool fFound = false;
					string testClassification = this.txtDesignation.Text.Substring(0, iPos);
					DataView dvClassification = ((clsShips)mTCBase).Classifications;
					foreach (DataRow cRow in dvClassification.Table.Rows) {
						if (!Information.IsDBNull(cRow["Type"]) && cRow["Type"] == testClassification) {
							mTCBase.CurrentRow["Classification"] = cRow["Type"];
							mTCBase.CurrentRow["ClassificationID"] = cRow["ID"];
							fFound = true;
							break; 
						}
					}
					if (!fFound)
						throw new ApplicationException(string.Format("Unknown classification specified: {0}", testClassification));
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}

		private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!cbClass.IsDisposed){}
		}

		private void cbClass_SelectedValueChanged(object sender, EventArgs e)
		{
			if (!cbClass.IsDisposed){}
		}

		private void cbClass_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (!cbClass.IsDisposed){}
		}
		#endregion
	}
}
