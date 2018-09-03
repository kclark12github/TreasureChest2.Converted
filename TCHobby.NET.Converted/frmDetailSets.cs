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
//frmDetailSets.vb
//   Detail Sets Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   04/22/18    Introduced functionality to link Decals and Detail Sets to a Kit enabling separate Location tracking;
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen controls to accommodate new fields;
//   02/06/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmDetailSets : frmTCStandard
	{
		const string myFormName = "frmDetailSets";
		public frmDetailSets(clsSupport objSupport, clsDetailSets objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmDetailSets() : base()
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
		internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.Label lblName;
		internal System.Windows.Forms.Label lblScale;
		internal System.Windows.Forms.ComboBox cbScale;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.ComboBox cbType;
		internal System.Windows.Forms.TextBox txtReference;
		internal System.Windows.Forms.Label lblReference;
		internal System.Windows.Forms.Label lblCatalog;
		internal System.Windows.Forms.ComboBox cbCatalog;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		internal System.Windows.Forms.Label lblNation;
		internal TextBox txtDesignation;
		internal Label lblDesignation;
		internal System.Windows.Forms.ComboBox cbNation;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetailSets));
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblScale = new System.Windows.Forms.Label();
			this.cbScale = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.txtReference = new System.Windows.Forms.TextBox();
			this.lblReference = new System.Windows.Forms.Label();
			this.lblCatalog = new System.Windows.Forms.Label();
			this.cbCatalog = new System.Windows.Forms.ComboBox();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
			this.lblNation = new System.Windows.Forms.Label();
			this.cbNation = new System.Windows.Forms.ComboBox();
			this.txtDesignation = new System.Windows.Forms.TextBox();
			this.lblDesignation = new System.Windows.Forms.Label();
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
			this.SuspendLayout();
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(759, 431);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 431);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(732, 431);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 431);
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
			this.sbpMessage.Width = 599;
			//
			//sbpTime
			//
			this.sbpTime.Text = "6:26 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 474);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(220, 246);
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(208, 302);
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(36, 275);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(488, 212);
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(60, 246);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(628, 471);
			this.btnOK.TabIndex = 15;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(709, 471);
			this.btnExit.TabIndex = 17;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(709, 471);
			this.btnCancel.TabIndex = 16;
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
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(108, 220);
			this.chkWishList.TabIndex = 11;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(296, 244);
			this.dtpPurchased.TabIndex = 13;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(296, 300);
			this.dtpInventoried.TabIndex = 15;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(104, 272);
			this.cbLocation.Size = new System.Drawing.Size(308, 24);
			this.cbLocation.TabIndex = 14;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(568, 208);
			this.txtAlphaSort.Size = new System.Drawing.Size(116, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(428, 80);
			this.pbGeneral.Size = new System.Drawing.Size(320, 232);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 431);
			this.txtCaption.Size = new System.Drawing.Size(664, 23);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 475);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(764, 355);
			this.rtfNotes.TabIndex = 14;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(104, 244);
			this.txtPrice.TabIndex = 12;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(568, 208);
			this.cbAlphaSort.Size = new System.Drawing.Size(108, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtDesignation);
			this.gbGeneral.Controls.Add(this.lblDesignation);
			this.gbGeneral.Controls.Add(this.lblNation);
			this.gbGeneral.Controls.Add(this.cbNation);
			this.gbGeneral.Controls.Add(this.txtReference);
			this.gbGeneral.Controls.Add(this.lblReference);
			this.gbGeneral.Controls.Add(this.lblCatalog);
			this.gbGeneral.Controls.Add(this.cbCatalog);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.lblScale);
			this.gbGeneral.Controls.Add(this.cbScale);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Size = new System.Drawing.Size(756, 347);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbNation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblNation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDesignation, 0);
			//
			//tcMain
			//
			this.tcMain.Size = new System.Drawing.Size(776, 387);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(768, 358);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(768, 358);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(424, 316);
			this.hsbGeneral.Size = new System.Drawing.Size(324, 17);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(428, 80);
			this.pbGeneral2.Size = new System.Drawing.Size(320, 232);
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(55, 194);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(104, 192);
			this.txtValue.TabIndex = 9;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(240, 194);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(296, 192);
			this.dtpVerified.TabIndex = 10;
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
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(104, 24);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(640, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(53, 26);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 16);
			this.lblName.TabIndex = 102;
			this.lblName.Text = "Name";
			//
			//lblScale
			//
			this.lblScale.AutoSize = true;
			this.lblScale.Location = new System.Drawing.Point(272, 166);
			this.lblScale.Name = "lblScale";
			this.lblScale.Size = new System.Drawing.Size(44, 16);
			this.lblScale.TabIndex = 101;
			this.lblScale.Text = "Scale";
			//
			//cbScale
			//
			this.cbScale.Location = new System.Drawing.Point(316, 163);
			this.cbScale.Name = "cbScale";
			this.cbScale.Size = new System.Drawing.Size(96, 24);
			this.cbScale.TabIndex = 8;
			this.cbScale.Text = "cbScale";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(60, 55);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(41, 16);
			this.lblType.TabIndex = 100;
			this.lblType.Text = "Type";
			//
			//cbType
			//
			this.cbType.DropDownWidth = 400;
			this.cbType.Location = new System.Drawing.Point(104, 52);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(312, 24);
			this.cbType.TabIndex = 2;
			this.cbType.Text = "cbType";
			//
			//txtReference
			//
			this.txtReference.Location = new System.Drawing.Point(104, 164);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(144, 23);
			this.txtReference.TabIndex = 7;
			this.txtReference.Tag = "";
			this.txtReference.Text = "txtReference";
			//
			//lblReference
			//
			this.lblReference.AutoSize = true;
			this.lblReference.Location = new System.Drawing.Point(25, 166);
			this.lblReference.Name = "lblReference";
			this.lblReference.Size = new System.Drawing.Size(74, 16);
			this.lblReference.TabIndex = 108;
			this.lblReference.Text = "Reference";
			//
			//lblCatalog
			//
			this.lblCatalog.AutoSize = true;
			this.lblCatalog.Location = new System.Drawing.Point(41, 140);
			this.lblCatalog.Name = "lblCatalog";
			this.lblCatalog.Size = new System.Drawing.Size(58, 16);
			this.lblCatalog.TabIndex = 107;
			this.lblCatalog.Text = "Catalog";
			//
			//cbCatalog
			//
			this.cbCatalog.Location = new System.Drawing.Point(104, 136);
			this.cbCatalog.Name = "cbCatalog";
			this.cbCatalog.Size = new System.Drawing.Size(308, 24);
			this.cbCatalog.TabIndex = 6;
			this.cbCatalog.Text = "cbCatalog";
			//
			//lblManufacturer
			//
			this.lblManufacturer.AutoSize = true;
			this.lblManufacturer.Location = new System.Drawing.Point(3, 112);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(96, 16);
			this.lblManufacturer.TabIndex = 106;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//cbManufacturer
			//
			this.cbManufacturer.DropDownWidth = 300;
			this.cbManufacturer.Location = new System.Drawing.Point(104, 108);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(308, 24);
			this.cbManufacturer.TabIndex = 5;
			this.cbManufacturer.Text = "cbManufacturer";
			//
			//lblNation
			//
			this.lblNation.AutoSize = true;
			this.lblNation.Location = new System.Drawing.Point(49, 84);
			this.lblNation.Name = "lblNation";
			this.lblNation.Size = new System.Drawing.Size(50, 16);
			this.lblNation.TabIndex = 110;
			this.lblNation.Text = "Nation";
			//
			//cbNation
			//
			this.cbNation.Location = new System.Drawing.Point(104, 80);
			this.cbNation.Name = "cbNation";
			this.cbNation.Size = new System.Drawing.Size(312, 24);
			this.cbNation.TabIndex = 4;
			this.cbNation.Text = "cbNation";
			//
			//txtDesignation
			//
			this.txtDesignation.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDesignation.Location = new System.Drawing.Point(515, 53);
			this.txtDesignation.Name = "txtDesignation";
			this.txtDesignation.Size = new System.Drawing.Size(229, 23);
			this.txtDesignation.TabIndex = 3;
			this.txtDesignation.Tag = "Required";
			this.txtDesignation.Text = "txtDesignation";
			//
			//lblDesignation
			//
			this.lblDesignation.AutoSize = true;
			this.lblDesignation.Location = new System.Drawing.Point(425, 56);
			this.lblDesignation.Name = "lblDesignation";
			this.lblDesignation.Size = new System.Drawing.Size(84, 16);
			this.lblDesignation.TabIndex = 112;
			this.lblDesignation.Text = "Designation";
			//
			//frmDetailSets
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(792, 529);
			this.Location = new System.Drawing.Point(0, 0);
			this.MinimumSize = new System.Drawing.Size(800, 563);
			this.Name = "frmDetailSets";
			this.Text = "frmDetailSets";
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
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsDetailSets)mTCBase).Types, "Type", "Type");
			BindControl(txtDesignation, mTCBase.MainDataView, "Designation");
			BindControl(cbNation, mTCBase.MainDataView, "Nation", ((clsDetailSets)mTCBase).Nations, "Nation", "Nation");
			BindControl(cbScale, mTCBase.MainDataView, "Scale", ((clsDetailSets)mTCBase).Scales, "Scale", "Scale");
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsDetailSets)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(cbCatalog, mTCBase.MainDataView, "ProductCatalog", ((clsDetailSets)mTCBase).Catalogs, "ProductCatalog", "ProductCatalog");
			BindControl(txtReference, mTCBase.MainDataView, "Reference");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsDetailSets)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			//BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort")  'Note that this guy is Simple-Bound...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
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
						if (mTCBase.Mode != clsTCBase.ActionModeEnum.modeDisplay) {
                            mTCBase.CurrentRow["Location"] = DBNull.Value;
                            mTCBase.CurrentRow["DateInventoried"] = DBNull.Value;
                            mTCBase.CurrentRow["Price"] = DBNull.Value;
                            mTCBase.CurrentRow["DatePurchased"] = DBNull.Value;
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
						break;
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmDetailSets.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(cbType);
				cbType.Validating -= SetCaption;
				RemoveControlHandlers(txtDesignation);
				txtDesignation.Validating -= DefaultTextBox;
				txtDesignation.Validating -= SetCaption;
				RemoveControlHandlers(cbNation);
				RemoveControlHandlers(cbScale);
				cbScale.Validating -= SetCaption;
				RemoveControlHandlers(cbManufacturer);
				cbManufacturer.Validating -= SetCaption;
				RemoveControlHandlers(cbCatalog);
				RemoveControlHandlers(txtReference);
				txtReference.Validating -= DefaultTextBox;
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
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
			const string EntryName = "frmDetailSets.Form_Load";
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
				SetupControlHandlers(cbScale);
				cbScale.Validating += SetCaption;
				SetupControlHandlers(cbManufacturer);
				cbManufacturer.Validating += SetCaption;
				SetupControlHandlers(cbCatalog);
				SetupControlHandlers(txtReference);
				txtReference.Validating += DefaultTextBox;
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
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
				string temp = (Information.IsDBNull(mTCBase.CurrentRow["Type"]) ? bpeNullString : mTCBase.CurrentRow["Type"] + "; ") + (Information.IsDBNull(mTCBase.CurrentRow["Scale"]) ? bpeNullString : mTCBase.CurrentRow["Scale"] + " Scale; ") + (Information.IsDBNull(mTCBase.CurrentRow["Manufacturer"]) ? bpeNullString : mTCBase.CurrentRow["Manufacturer"]);
				string reference = (Information.IsDBNull(mTCBase.CurrentRow["Reference"]) ? bpeNullString : (string)mTCBase.CurrentRow["Reference"]);
				if (reference.ToUpper() != "UNKNOWN")
					temp += "(" + reference + "); ";
				else
					temp += "; ";
				temp += (Information.IsDBNull(mTCBase.CurrentRow["Name"]) ? bpeNullString : mTCBase.CurrentRow["Name"]);
				this.txtCaption.Text = temp.ToUpper();
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
