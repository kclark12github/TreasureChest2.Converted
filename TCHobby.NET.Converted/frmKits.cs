//frmKits.vb
//   Model Kits Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/20/18    Implemented positioning of [Detail Set] and/or [Decal] data in those respective screens based on row selected on
//               each respective DataGridView on this screen;
//   05/31/18    Changed SetCaption to deal with null Type and Designation data now that they're used to display related
//               [Detail Set] and [Decal] data;
//   04/21/18    Introduced functionality to link Decals and Detail Sets to a Kit enabling separate Location tracking;
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen controls to accommodate new fields;
//   02/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
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
namespace TCHobby
{
	public class frmKits : frmTCStandard
	{
		const string myFormName = "frmKits";
		public frmKits(clsSupport objSupport, clsKits objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			LoadDefaultImage();
			BindControls();
			EnableControls(false);

			Icon = mTCBase.Icon;
			MinimumSize = new Size(Width, Height);
			ReportPath = mTCBase.ReportPath;
		}
		#region " Windows Form Designer generated code "
		public frmKits() : base()
		{

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
		internal System.Windows.Forms.ComboBox cbType;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.Label lblService;
		internal System.Windows.Forms.ComboBox cbService;
		internal System.Windows.Forms.Label lblEra;
		internal System.Windows.Forms.ComboBox cbEra;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		internal System.Windows.Forms.Label lblScale;
		internal System.Windows.Forms.ComboBox cbScale;
		internal System.Windows.Forms.Label lblNation;
		internal System.Windows.Forms.ComboBox cbNation;
		internal System.Windows.Forms.Label lblCatalog;
		internal System.Windows.Forms.ComboBox cbCatalog;
		internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.Label lblName;
		internal System.Windows.Forms.TextBox txtDesignation;
		internal System.Windows.Forms.Label lblDesignation;
		internal System.Windows.Forms.TextBox txtReference;
		internal System.Windows.Forms.Label lblReference;
		internal System.Windows.Forms.Label lblCondition;
		internal System.Windows.Forms.ComboBox cbCondition;
		internal System.Windows.Forms.CheckBox chkOutOfProduction;
		public TabPage tpDecals;
		internal System.Windows.Forms.DataGridView dgvDecals;
		public TabPage tpDetailSets;
		internal GroupBox gbIncludes;
		internal CheckBox chkDetailSet;
		internal CheckBox chkDecals;
		internal System.Windows.Forms.DataGridView dgvDetailSets;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKits));
			this.cbType = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.lblService = new System.Windows.Forms.Label();
			this.cbService = new System.Windows.Forms.ComboBox();
			this.lblEra = new System.Windows.Forms.Label();
			this.cbEra = new System.Windows.Forms.ComboBox();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
			this.lblScale = new System.Windows.Forms.Label();
			this.cbScale = new System.Windows.Forms.ComboBox();
			this.lblNation = new System.Windows.Forms.Label();
			this.cbNation = new System.Windows.Forms.ComboBox();
			this.lblCatalog = new System.Windows.Forms.Label();
			this.cbCatalog = new System.Windows.Forms.ComboBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtDesignation = new System.Windows.Forms.TextBox();
			this.lblDesignation = new System.Windows.Forms.Label();
			this.txtReference = new System.Windows.Forms.TextBox();
			this.lblReference = new System.Windows.Forms.Label();
			this.chkOutOfProduction = new System.Windows.Forms.CheckBox();
			this.lblCondition = new System.Windows.Forms.Label();
			this.cbCondition = new System.Windows.Forms.ComboBox();
			this.tpDecals = new System.Windows.Forms.TabPage();
			this.dgvDecals = new System.Windows.Forms.DataGridView();
			this.tpDetailSets = new System.Windows.Forms.TabPage();
			this.dgvDetailSets = new System.Windows.Forms.DataGridView();
			this.gbIncludes = new System.Windows.Forms.GroupBox();
			this.chkDetailSet = new System.Windows.Forms.CheckBox();
			this.chkDecals = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral).BeginInit();
			this.gbGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tpNotes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			this.tpDecals.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvDecals).BeginInit();
			this.tpDetailSets.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvDetailSets).BeginInit();
			this.gbIncludes.SuspendLayout();
			this.SuspendLayout();
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(759, 440);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 440);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(732, 440);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 440);
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
			this.sbpMessage.Width = 591;
			//
			//sbpTime
			//
			this.sbpTime.Text = "11:43 PM";
			this.sbpTime.Width = 79;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 479);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(234, 188);
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(10, 246);
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(31, 220);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(512, 272);
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(55, 188);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(628, 476);
			this.btnOK.TabIndex = 20;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(709, 476);
			this.btnExit.TabIndex = 21;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(709, 476);
			this.btnCancel.TabIndex = 22;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 507);
			this.sbStatus.Size = new System.Drawing.Size(792, 22);
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
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.chkWishList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkWishList.Location = new System.Drawing.Point(608, 132);
			this.chkWishList.TabIndex = 12;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(320, 188);
			this.dtpPurchased.TabIndex = 16;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(96, 244);
			this.dtpInventoried.TabIndex = 18;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.DropDownWidth = 420;
			this.cbLocation.Location = new System.Drawing.Point(96, 216);
			this.cbLocation.Size = new System.Drawing.Size(340, 24);
			this.cbLocation.TabIndex = 17;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(596, 268);
			this.txtAlphaSort.Size = new System.Drawing.Size(92, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(456, 164);
			this.pbGeneral.Size = new System.Drawing.Size(272, 162);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 440);
			this.txtCaption.Size = new System.Drawing.Size(664, 23);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 480);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(764, 344);
			this.rtfNotes.TabIndex = 19;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(96, 188);
			this.txtPrice.TabIndex = 15;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(592, 268);
			this.cbAlphaSort.Size = new System.Drawing.Size(92, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.gbIncludes);
			this.gbGeneral.Controls.Add(this.lblCondition);
			this.gbGeneral.Controls.Add(this.cbCondition);
			this.gbGeneral.Controls.Add(this.chkOutOfProduction);
			this.gbGeneral.Controls.Add(this.txtReference);
			this.gbGeneral.Controls.Add(this.lblReference);
			this.gbGeneral.Controls.Add(this.txtDesignation);
			this.gbGeneral.Controls.Add(this.lblDesignation);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.lblNation);
			this.gbGeneral.Controls.Add(this.cbNation);
			this.gbGeneral.Controls.Add(this.lblCatalog);
			this.gbGeneral.Controls.Add(this.cbCatalog);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.lblScale);
			this.gbGeneral.Controls.Add(this.cbScale);
			this.gbGeneral.Controls.Add(this.lblEra);
			this.gbGeneral.Controls.Add(this.cbEra);
			this.gbGeneral.Controls.Add(this.lblService);
			this.gbGeneral.Controls.Add(this.cbService);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Size = new System.Drawing.Size(756, 356);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbService, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblService, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbEra, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblEra, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbNation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblNation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkOutOfProduction, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCondition, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCondition, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.gbIncludes, 0);
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpDecals);
			this.tcMain.Controls.Add(this.tpDetailSets);
			this.tcMain.Size = new System.Drawing.Size(776, 396);
			this.tcMain.Controls.SetChildIndex(this.tpDetailSets, 0);
			this.tcMain.Controls.SetChildIndex(this.tpDecals, 0);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(768, 367);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(768, 367);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(468, 166);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(456, 164);
			this.pbGeneral2.Size = new System.Drawing.Size(160, 65);
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(50, 162);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(96, 160);
			this.txtValue.TabIndex = 13;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(252, 162);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(320, 160);
			this.dtpVerified.TabIndex = 14;
			this.dtpVerified.Visible = true;
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
			//cbType
			//
			this.cbType.DropDownWidth = 400;
			this.cbType.Location = new System.Drawing.Point(96, 48);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(320, 24);
			this.cbType.TabIndex = 2;
			this.cbType.Tag = "Required";
			this.cbType.Text = "cbType";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(55, 52);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(41, 16);
			this.lblType.TabIndex = 82;
			this.lblType.Text = "Type";
			//
			//lblService
			//
			this.lblService.AutoSize = true;
			this.lblService.Location = new System.Drawing.Point(317, 79);
			this.lblService.Name = "lblService";
			this.lblService.Size = new System.Drawing.Size(57, 16);
			this.lblService.TabIndex = 84;
			this.lblService.Text = "Service";
			//
			//cbService
			//
			this.cbService.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbService.DropDownWidth = 160;
			this.cbService.Location = new System.Drawing.Point(380, 76);
			this.cbService.Name = "cbService";
			this.cbService.Size = new System.Drawing.Size(179, 24);
			this.cbService.TabIndex = 5;
			this.cbService.Tag = "Required";
			this.cbService.Text = "cbService";
			//
			//lblEra
			//
			this.lblEra.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lblEra.AutoSize = true;
			this.lblEra.Location = new System.Drawing.Point(565, 80);
			this.lblEra.Name = "lblEra";
			this.lblEra.Size = new System.Drawing.Size(29, 16);
			this.lblEra.TabIndex = 86;
			this.lblEra.Text = "Era";
			//
			//cbEra
			//
			this.cbEra.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cbEra.DropDownWidth = 150;
			this.cbEra.Location = new System.Drawing.Point(596, 76);
			this.cbEra.Name = "cbEra";
			this.cbEra.Size = new System.Drawing.Size(132, 24);
			this.cbEra.TabIndex = 6;
			this.cbEra.Tag = "Required";
			this.cbEra.Text = "cbEra";
			//
			//lblManufacturer
			//
			this.lblManufacturer.AutoSize = true;
			this.lblManufacturer.Location = new System.Drawing.Point(212, 107);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(96, 16);
			this.lblManufacturer.TabIndex = 90;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//cbManufacturer
			//
			this.cbManufacturer.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbManufacturer.DropDownWidth = 300;
			this.cbManufacturer.Location = new System.Drawing.Point(308, 104);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(264, 24);
			this.cbManufacturer.TabIndex = 8;
			this.cbManufacturer.Tag = "Required";
			this.cbManufacturer.Text = "cbManufacturer";
			//
			//lblScale
			//
			this.lblScale.AutoSize = true;
			this.lblScale.Location = new System.Drawing.Point(51, 107);
			this.lblScale.Name = "lblScale";
			this.lblScale.Size = new System.Drawing.Size(44, 16);
			this.lblScale.TabIndex = 88;
			this.lblScale.Text = "Scale";
			//
			//cbScale
			//
			this.cbScale.Location = new System.Drawing.Point(96, 104);
			this.cbScale.Name = "cbScale";
			this.cbScale.Size = new System.Drawing.Size(96, 24);
			this.cbScale.TabIndex = 7;
			this.cbScale.Tag = "Required";
			this.cbScale.Text = "Unknown";
			//
			//lblNation
			//
			this.lblNation.AutoSize = true;
			this.lblNation.Location = new System.Drawing.Point(44, 80);
			this.lblNation.Name = "lblNation";
			this.lblNation.Size = new System.Drawing.Size(50, 16);
			this.lblNation.TabIndex = 94;
			this.lblNation.Text = "Nation";
			//
			//cbNation
			//
			this.cbNation.Location = new System.Drawing.Point(96, 76);
			this.cbNation.Name = "cbNation";
			this.cbNation.Size = new System.Drawing.Size(214, 24);
			this.cbNation.TabIndex = 4;
			this.cbNation.Tag = "Required";
			this.cbNation.Text = "cbNation";
			//
			//lblCatalog
			//
			this.lblCatalog.AutoSize = true;
			this.lblCatalog.Location = new System.Drawing.Point(36, 135);
			this.lblCatalog.Name = "lblCatalog";
			this.lblCatalog.Size = new System.Drawing.Size(58, 16);
			this.lblCatalog.TabIndex = 92;
			this.lblCatalog.Text = "Catalog";
			//
			//cbCatalog
			//
			this.cbCatalog.Location = new System.Drawing.Point(96, 132);
			this.cbCatalog.Name = "cbCatalog";
			this.cbCatalog.Size = new System.Drawing.Size(272, 24);
			this.cbCatalog.TabIndex = 10;
			this.cbCatalog.Tag = "Required";
			this.cbCatalog.Text = "cbCatalog";
			//
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(96, 20);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(632, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(48, 22);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 16);
			this.lblName.TabIndex = 96;
			this.lblName.Text = "Name";
			//
			//txtDesignation
			//
			this.txtDesignation.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDesignation.Location = new System.Drawing.Point(532, 49);
			this.txtDesignation.Name = "txtDesignation";
			this.txtDesignation.Size = new System.Drawing.Size(196, 23);
			this.txtDesignation.TabIndex = 3;
			this.txtDesignation.Tag = "";
			this.txtDesignation.Text = "txtDesignation";
			//
			//lblDesignation
			//
			this.lblDesignation.AutoSize = true;
			this.lblDesignation.Location = new System.Drawing.Point(444, 51);
			this.lblDesignation.Name = "lblDesignation";
			this.lblDesignation.Size = new System.Drawing.Size(84, 16);
			this.lblDesignation.TabIndex = 98;
			this.lblDesignation.Text = "Designation";
			//
			//txtReference
			//
			this.txtReference.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtReference.Location = new System.Drawing.Point(456, 133);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(116, 23);
			this.txtReference.TabIndex = 11;
			this.txtReference.Tag = "";
			this.txtReference.Text = "txtReference";
			//
			//lblReference
			//
			this.lblReference.AutoSize = true;
			this.lblReference.Location = new System.Drawing.Point(377, 135);
			this.lblReference.Name = "lblReference";
			this.lblReference.Size = new System.Drawing.Size(74, 16);
			this.lblReference.TabIndex = 100;
			this.lblReference.Text = "Reference";
			//
			//chkOutOfProduction
			//
			this.chkOutOfProduction.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.chkOutOfProduction.Location = new System.Drawing.Point(584, 104);
			this.chkOutOfProduction.Name = "chkOutOfProduction";
			this.chkOutOfProduction.Size = new System.Drawing.Size(144, 24);
			this.chkOutOfProduction.TabIndex = 9;
			this.chkOutOfProduction.Text = "Out of Production";
			//
			//lblCondition
			//
			this.lblCondition.AutoSize = true;
			this.lblCondition.Location = new System.Drawing.Point(228, 246);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(69, 16);
			this.lblCondition.TabIndex = 105;
			this.lblCondition.Text = "Condition";
			//
			//cbCondition
			//
			this.cbCondition.DropDownWidth = 256;
			this.cbCondition.Location = new System.Drawing.Point(304, 243);
			this.cbCondition.Name = "cbCondition";
			this.cbCondition.Size = new System.Drawing.Size(132, 24);
			this.cbCondition.TabIndex = 19;
			this.cbCondition.Tag = "Required";
			this.cbCondition.Text = "cbCondition";
			//
			//tpDecals
			//
			this.tpDecals.Controls.Add(this.dgvDecals);
			this.tpDecals.Location = new System.Drawing.Point(4, 25);
			this.tpDecals.Name = "tpDecals";
			this.tpDecals.Size = new System.Drawing.Size(656, 270);
			this.tpDecals.TabIndex = 2;
			this.tpDecals.Text = "Decals";
			//
			//dgvDecals
			//
			this.dgvDecals.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.dgvDecals.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dgvDecals.Location = new System.Drawing.Point(0, 0);
			this.dgvDecals.Name = "dgvDecals";
			this.dgvDecals.Size = new System.Drawing.Size(656, 270);
			this.dgvDecals.TabIndex = 0;
			//
			//tpDetailSets
			//
			this.tpDetailSets.Controls.Add(this.dgvDetailSets);
			this.tpDetailSets.Location = new System.Drawing.Point(4, 25);
			this.tpDetailSets.Name = "tpDetailSets";
			this.tpDetailSets.Size = new System.Drawing.Size(656, 270);
			this.tpDetailSets.TabIndex = 3;
			this.tpDetailSets.Text = "Detail Sets";
			//
			//dgvDetailSets
			//
			this.dgvDetailSets.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.dgvDetailSets.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dgvDetailSets.Location = new System.Drawing.Point(0, 0);
			this.dgvDetailSets.Name = "dgvDetailSets";
			this.dgvDetailSets.Size = new System.Drawing.Size(656, 270);
			this.dgvDetailSets.TabIndex = 0;
			//
			//gbIncludes
			//
			this.gbIncludes.Controls.Add(this.chkDetailSet);
			this.gbIncludes.Controls.Add(this.chkDecals);
			this.gbIncludes.Location = new System.Drawing.Point(96, 273);
			this.gbIncludes.Name = "gbIncludes";
			this.gbIncludes.Size = new System.Drawing.Size(201, 48);
			this.gbIncludes.TabIndex = 20;
			this.gbIncludes.TabStop = false;
			this.gbIncludes.Text = "Includes";
			//
			//chkDetailSet
			//
			this.chkDetailSet.AutoSize = true;
			this.chkDetailSet.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkDetailSet.Location = new System.Drawing.Point(103, 22);
			this.chkDetailSet.Name = "chkDetailSet";
			this.chkDetailSet.Size = new System.Drawing.Size(92, 20);
			this.chkDetailSet.TabIndex = 23;
			this.chkDetailSet.Text = "Detail Set";
			this.chkDetailSet.UseVisualStyleBackColor = true;
			//
			//chkDecals
			//
			this.chkDecals.AutoSize = true;
			this.chkDecals.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkDecals.Location = new System.Drawing.Point(6, 22);
			this.chkDecals.Name = "chkDecals";
			this.chkDecals.Size = new System.Drawing.Size(70, 20);
			this.chkDecals.TabIndex = 22;
			this.chkDecals.Text = "Decals";
			this.chkDecals.UseVisualStyleBackColor = true;
			//
			//frmKits
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(792, 529);
			this.Location = new System.Drawing.Point(0, 0);
			this.MinimumSize = new System.Drawing.Size(800, 563);
			this.Name = "frmKits";
			this.Text = "frmKits";
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.pbGeneral).EndInit();
			this.gbGeneral.ResumeLayout(false);
			this.gbGeneral.PerformLayout();
			this.tcMain.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tpNotes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pbGeneral2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			this.tpDecals.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvDecals).EndInit();
			this.tpDetailSets.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvDetailSets).EndInit();
			this.gbIncludes.ResumeLayout(false);
			this.gbIncludes.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		#region "Properties"
		#region "Declarations"
		#endregion
		#endregion
		#region "Methods"
		protected override void BindControls()
		{
			BindControl(txtID, mTCBase.MainDataView, "ID");
			BindControl(txtName, mTCBase.MainDataView, "Name");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsKits)mTCBase).Types, "Type", "Type");
			BindControl(txtDesignation, mTCBase.MainDataView, "Designation");
			BindControl(cbNation, mTCBase.MainDataView, "Nation", ((clsKits)mTCBase).Nations, "Nation", "Nation");
			BindControl(cbService, mTCBase.MainDataView, "Service", ((clsKits)mTCBase).Services, "Service", "Service");
			BindControl(cbEra, mTCBase.MainDataView, "Era", ((clsKits)mTCBase).Eras, "Era", "Era");
			BindControl(cbScale, mTCBase.MainDataView, "Scale", ((clsKits)mTCBase).Scales, "Scale", "Scale");
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsKits)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(chkOutOfProduction, mTCBase.MainDataView, "OutOfProduction");
			BindControl(cbCatalog, mTCBase.MainDataView, "ProductCatalog", ((clsKits)mTCBase).Catalogs, "ProductCatalog", "ProductCatalog");
			BindControl(txtReference, mTCBase.MainDataView, "Reference");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsKits)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			BindControl(cbCondition, mTCBase.MainDataView, "Condition", ((clsKits)mTCBase).Conditions, "Condition", "Condition");
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			//BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort")  'Note that this guy is Simple-Bound...
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
			BindControl(chkDecals, mTCBase.MainDataView, "HasDecals");
			BindControl(chkDetailSet, mTCBase.MainDataView, "HasDetailSet");
		}
		protected void ResetGrid(DataGridView dgv, DataView DataSource)
		{
			System.Drawing.Color altBackColor = Color.Snow;
			//Color.NavajoWhite  'Color.MintCream
			var _with1 = dgv;
			_with1.DataSource = null;
			_with1.DataSource = DataSource;

			_with1.BackgroundColor = SystemColors.InactiveCaptionText;

			_with1.ResetBackColor();
			_with1.ResetForeColor();
			_with1.ResetText();

			_with1.AllowUserToAddRows = false;
			_with1.AllowUserToDeleteRows = false;
			_with1.AllowUserToOrderColumns = true;
			_with1.AllowUserToResizeColumns = true;
			_with1.AllowUserToResizeRows = true;
			_with1.BackColor = Color.GhostWhite;
			_with1.BackgroundColor = SystemColors.AppWorkspace;
			_with1.BorderStyle = BorderStyle.Fixed3D;
			_with1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			_with1.EditMode = DataGridViewEditMode.EditProgrammatically;
			//.Font = New Font("Tahoma", 8.0!)
			_with1.ForeColor = Color.MidnightBlue;
			_with1.ReadOnly = true;
			_with1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			_with1.RowsDefaultCellStyle.BackColor = Color.GhostWhite;
			_with1.AlternatingRowsDefaultCellStyle.BackColor = altBackColor;
			_with1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

			ResetColumns(dgv);

			dgv.Focus();
			//If dgv.CurrentRow.Index > 0 Then dgv.Select(dgv.CurrentRowIndex) : dgv.CurrentCell = New DataGridCell(dgv.CurrentRowIndex, 1)
			dgv.Visible = true;
		}
		protected void ResetColumns(DataGridView dgv)
		{
			DataGridViewContentAlignment Alignment = DataGridViewContentAlignment.TopLeft;
			string Format = "";
			int Order = 0;
			bool Visible = true;
			int Width = 0;

			foreach (DataGridViewColumn iColumn in dgv.Columns) {
				Alignment = DataGridViewContentAlignment.TopLeft;
				Format = "";
				Visible = true;
				Order = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}.{1}", mRegistryKey, dgv.Name), string.Format("ColumnOrder.{0}", iColumn.Name), iColumn.DisplayIndex);
				Width = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}.{1}", mRegistryKey, dgv.Name), string.Format("ColumnWidth.{0}", iColumn.Name), iColumn.Width);
				switch (iColumn.ValueType.Name) {
					case "Byte[]":
						Visible = false;
						break;
					case "Currency":
					case "Money":
					case "Decimal":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = "$#,##0.00";
						break;
					case "Boolean":
						Alignment = DataGridViewContentAlignment.TopCenter;
						break;
					case "Date":
					case "DateTime":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = "MM/dd/yyyy hh:mm tt";
						break;
					case "Integer":
					case "Int16":
					case "Int32":
					case "Int64":
					case "Short":
					case "Long":
					case "Double":
					case "Single":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = (iColumn.Name == "CONVERT(BIGINT,ROWID)" ? "X16" : "0");
						break;
					case "String":
					case "Text":
						if (iColumn.Name == "Notes")
							Visible = false;
						break;
				}
				iColumn.DefaultCellStyle.Alignment = Alignment;
				iColumn.DefaultCellStyle.Format = Format;
				iColumn.DisplayIndex = Order;
				iColumn.Visible = (iColumn.Name == "ID" ? false : Visible);
				iColumn.Width = Width;
			}
		}
		protected void SaveColumns(DataGridView dgv)
		{
			if (dgv.CurrentRow != null) {
				string TableName = ((DataView)dgv.DataSource).Table.TableName;
				string RegistryKey = string.Format("{0}\\frm{1} Settings", mTCBase.RegistryKey, TableName.Replace(" ", bpeNullString));
                //DataView dv = (DataView)dgv.DataSource;
                //DataGridViewRow dgvr = ((DataGridViewRow)dgv.CurrentRow);
                //DataRowView drv = (DataRowView)(dv[dgvr.Index]);
                SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, string.Format("{0}.ID", TableName), (int)((DataRowView)((DataView)dgv.DataSource)[((DataGridViewRow)dgv.CurrentRow).Index])["ID"]);
            }
			foreach (DataGridViewColumn iColumn in dgv.Columns) {
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}.{1}", mRegistryKey, dgv.Name), string.Format("ColumnOrder.{0}", iColumn.Name), Convert.ToInt32(iColumn.DisplayIndex));
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}.{1}", mRegistryKey, dgv.Name), string.Format("ColumnWidth.{0}", iColumn.Name), Convert.ToInt32(iColumn.Width));
			}
		}
		#endregion
		#region "Event Handlers"
		private void chkWishList_CheckStateChanged(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				switch (chkWishList.CheckState) {
					case CheckState.Checked:
						//Hide applicable controls, and adjust screen size accordingly...
						cbLocation.Visible = false;
						lblLocation.Visible = false;
						dtpInventoried.Visible = false;
						lblInventoried.Visible = false;
						txtPrice.Visible = false;
						lblPrice.Visible = false;
						dtpPurchased.Visible = false;
						lblPurchased.Visible = false;
						cbCondition.Visible = false;
						lblCondition.Visible = false;
						if (mTCBase.Mode != clsTCBase.ActionModeEnum.modeDisplay) {
                            mTCBase.CurrentRow["Location"] = DBNull.Value;
                            mTCBase.CurrentRow["DateInventoried"] = DBNull.Value;
                            mTCBase.CurrentRow["Price"] = DBNull.Value;
                            mTCBase.CurrentRow["DatePurchased"] = DBNull.Value;
                            mTCBase.CurrentRow["Condition"] = DBNull.Value;
						}
						break;
					case CheckState.Indeterminate:
						break;
					case CheckState.Unchecked:
						//Show applicable controls, and adjust screen size accordingly...
						cbLocation.Visible = true;
						lblLocation.Visible = true;
						dtpInventoried.Visible = true;
						lblInventoried.Visible = true;
						txtPrice.Visible = true;
						lblPrice.Visible = true;
						dtpPurchased.Visible = true;
						lblPurchased.Visible = true;
						cbCondition.Visible = true;
						lblCondition.Visible = true;
						break;
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmKits.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				this.SaveColumns(this.dgvDecals);
				this.SaveColumns(this.dgvDetailSets);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(cbType);
				cbType.Validating -= SetCaption;
				RemoveControlHandlers(txtDesignation);
				txtDesignation.Validating -= DefaultTextBox;
				txtDesignation.Validating -= SetCaption;
				RemoveControlHandlers(cbNation);
				RemoveControlHandlers(cbService);
				RemoveControlHandlers(cbEra);
				RemoveControlHandlers(cbScale);
				cbScale.Validating -= SetCaption;
				RemoveControlHandlers(cbManufacturer);
				cbManufacturer.Validating -= SetCaption;
				RemoveControlHandlers(cbCatalog);
				RemoveControlHandlers(txtReference);
				txtReference.Validating -= DefaultTextBox;
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
				RemoveControlHandlers(cbCondition);
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
			const string EntryName = "frmKits.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(cbType);
				cbType.Validating += SetCaption;
				SetupControlHandlers(txtDesignation);
				txtDesignation.Validating += DefaultTextBox;
				txtDesignation.Validating += SetCaption;
				SetupControlHandlers(cbNation);
				SetupControlHandlers(cbService);
				SetupControlHandlers(cbEra);
				SetupControlHandlers(cbScale);
				cbScale.Validating += SetCaption;
				SetupControlHandlers(cbManufacturer);
				cbManufacturer.Validating += SetCaption;
				SetupControlHandlers(cbCatalog);
				SetupControlHandlers(txtReference);
				txtReference.Validating += DefaultTextBox;
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
				SetupControlHandlers(cbCondition);
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
				if ((sender != null))
					base.epBase.SetError((Control)sender, bpeNullString);
				//Me.txtCaption.Text = IIf(IsDBNull(mTCBase.CurrentRow("Sort")), bpeNullString, mTCBase.CurrentRow("Sort"))
				string tempType = (Information.IsDBNull(mTCBase.CurrentRow["Type"]) ? bpeNullString : (string)mTCBase.CurrentRow["Type"]);
				string tempScale = (Information.IsDBNull(mTCBase.CurrentRow["Scale"]) ? bpeNullString : (string)mTCBase.CurrentRow["Scale"]);
				string tempManufacturer = (Information.IsDBNull(mTCBase.CurrentRow["Manufacturer"]) ? bpeNullString : (string)mTCBase.CurrentRow["Manufacturer"]);
				string tempReference = (Information.IsDBNull(mTCBase.CurrentRow["Reference"]) ? bpeNullString : (string)mTCBase.CurrentRow["Reference"]);
				string tempDesignation = (Information.IsDBNull(mTCBase.CurrentRow["Designation"]) ? bpeNullString : (string)mTCBase.CurrentRow["Designation"]);
				string tempCaption = tempType + "; " + tempScale + " Scale; " + tempManufacturer;
				if (tempReference.ToUpper() != "UNKNOWN")
					tempCaption += "(" + tempReference + "); ";
				else
					tempCaption += "; ";
				if (tempDesignation.ToUpper() != "N/A" && tempDesignation.ToUpper() != "UNKNOWN")
					tempCaption += tempDesignation + " ";
				tempCaption += (Information.IsDBNull(mTCBase.CurrentRow["Name"]) ? bpeNullString : mTCBase.CurrentRow["Name"]);
				this.txtCaption.Text = tempCaption.ToUpper();
				//Deal with Add operations generically...
				if (!Information.IsDBNull(mTCBase.CurrentRow["Type"]) && !Information.IsDBNull(mTCBase.CurrentRow["Designation"])) {
					((clsKits)mTCBase).PopulateRelated(mTCBase.CurrentRow);
					this.ResetGrid(dgvDecals, ((clsKits)mTCBase).Decals);
					this.ResetGrid(dgvDetailSets, ((clsKits)mTCBase).DetailSets);
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				if ((sender != null))
					base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		#endregion
	}
}
