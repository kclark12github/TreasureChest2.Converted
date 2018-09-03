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
//frmAircraftDesignations.vb
//   Aircraft Designations Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmAircraftDesignations : frmTCStandard
	{
		const string myFormName = "frmAircraftDesignations";
		public frmAircraftDesignations(clsSupport objSupport, clsAircraftDesignations objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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

			//Reorder tcMain the way we want it (designer keeps changing it)...
			this.tcMain.SuspendLayout();
			this.tcMain.TabPages.Clear();
			TabPage[] pages = {
				tpGeneral,
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
		public frmAircraftDesignations() : base()
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
		internal System.Windows.Forms.TabPage tpImage;
		internal System.Windows.Forms.TextBox txtClassification;
		internal System.Windows.Forms.TextBox txtNumber;
		internal System.Windows.Forms.Label lblNumber;
		internal System.Windows.Forms.TextBox txtDesignation;
		internal System.Windows.Forms.Label lblDesignation;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.ComboBox cbType;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.TextBox txtVersion;
		internal System.Windows.Forms.Label lblVersion;
		internal System.Windows.Forms.DateTimePicker dtpServiceDate;
		internal System.Windows.Forms.Label lblServiceDate;
		internal System.Windows.Forms.Label lblNicknames;
		internal System.Windows.Forms.TextBox txtNicknames;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.lblNumber = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.tpImage = new System.Windows.Forms.TabPage();
			this.txtClassification = new System.Windows.Forms.TextBox();
			this.txtDesignation = new System.Windows.Forms.TextBox();
			this.lblDesignation = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.dtpServiceDate = new System.Windows.Forms.DateTimePicker();
			this.lblServiceDate = new System.Windows.Forms.Label();
			this.lblNicknames = new System.Windows.Forms.Label();
			this.txtNicknames = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			this.tpNotes.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.gbGeneral.SuspendLayout();
			this.SuspendLayout();
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 212);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.TabIndex = 13;
			this.lblPurchased.Visible = false;
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
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(612, 328);
			this.btnNext.Name = "btnNext";
			this.btnNext.TabIndex = 3;
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 328);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.TabIndex = 1;
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
			this.sbpTime.Text = "10:08 PM";
			this.sbpTime.Width = 79;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 367);
			this.lblID.Name = "lblID";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(508, 364);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 20;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(589, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 22;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 212);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.TabIndex = 15;
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 216);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.TabIndex = 7;
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(16, 212);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.TabIndex = 11;
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 216);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.TabIndex = 9;
			this.lblPrice.Visible = false;
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
			//hsbGeneral
			//
			this.hsbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.hsbGeneral.Location = new System.Drawing.Point(616, 214);
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(124, 17);
			this.hsbGeneral.Visible = false;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 397);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(92, 212);
			this.txtAlphaSort.Name = "txtAlphaSort";
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(484, 212);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 17;
			this.chkWishList.Visible = false;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 212);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 14;
			this.dtpPurchased.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 212);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 16;
			this.dtpInventoried.Visible = false;
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
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(652, 270);
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(648, 255);
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpImage);
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(656, 284);
			this.tcMain.Controls.SetChildIndex(this.tpImage, 0);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.lblNicknames);
			this.gbGeneral.Controls.Add(this.txtNicknames);
			this.gbGeneral.Controls.Add(this.lblServiceDate);
			this.gbGeneral.Controls.Add(this.dtpServiceDate);
			this.gbGeneral.Controls.Add(this.txtVersion);
			this.gbGeneral.Controls.Add(this.lblVersion);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.txtDesignation);
			this.gbGeneral.Controls.Add(this.lblDesignation);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.txtNumber);
			this.gbGeneral.Controls.Add(this.lblNumber);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.txtClassification);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(636, 244);
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
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblNumber, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtNumber, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDesignation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVersion, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtVersion, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpServiceDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblServiceDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtNicknames, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblNicknames, 0);
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
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 212);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Visible = false;
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(632, 218);
			this.ttBase.SetToolTip(this.rtfNotes, "Description of Class");
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 368);
			this.txtID.Name = "txtID";
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
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(108, 20);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(496, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			this.ttBase.SetToolTip(this.txtName, "Official name of this aircraft");
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(56, 22);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 19);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			//
			//txtNumber
			//
			this.txtNumber.Location = new System.Drawing.Point(108, 133);
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(224, 23);
			this.txtNumber.TabIndex = 11;
			this.txtNumber.TabStop = false;
			this.txtNumber.Tag = "Required, Numeric";
			this.txtNumber.Text = "txtNumber";
			this.ttBase.SetToolTip(this.txtNumber, "Designation number for this aircraft");
			this.txtNumber.Visible = false;
			//
			//lblNumber
			//
			this.lblNumber.AutoSize = true;
			this.lblNumber.Location = new System.Drawing.Point(42, 135);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(58, 19);
			this.lblNumber.TabIndex = 10;
			this.lblNumber.Text = "Number";
			this.lblNumber.Visible = false;
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(63, 107);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 19);
			this.lblType.TabIndex = 8;
			this.lblType.Text = "Type";
			//
			//cbType
			//
			this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbType.Enabled = false;
			this.cbType.Location = new System.Drawing.Point(108, 104);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(500, 24);
			this.cbType.TabIndex = 9;
			this.cbType.Tag = "Required";
			this.ttBase.SetToolTip(this.cbType, "Type of aircraft");
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
			this.txtClassification.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtClassification.Location = new System.Drawing.Point(108, 105);
			this.txtClassification.Name = "txtClassification";
			this.txtClassification.ReadOnly = true;
			this.txtClassification.Size = new System.Drawing.Size(136, 23);
			this.txtClassification.TabIndex = 82;
			this.txtClassification.Tag = "Ignore";
			this.txtClassification.Text = "txtClassification";
			this.ttBase.SetToolTip(this.txtClassification, "Description of Class/Ship");
			this.txtClassification.Visible = false;
			//
			//txtDesignation
			//
			this.txtDesignation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtDesignation.Location = new System.Drawing.Point(108, 76);
			this.txtDesignation.Name = "txtDesignation";
			this.txtDesignation.Size = new System.Drawing.Size(224, 23);
			this.txtDesignation.TabIndex = 5;
			this.txtDesignation.Tag = "Required";
			this.txtDesignation.Text = "TXTDESIGNATION";
			this.ttBase.SetToolTip(this.txtDesignation, "Official designation of this aircraft");
			//
			//lblDesignation
			//
			this.lblDesignation.AutoSize = true;
			this.lblDesignation.Location = new System.Drawing.Point(16, 80);
			this.lblDesignation.Name = "lblDesignation";
			this.lblDesignation.Size = new System.Drawing.Size(84, 19);
			this.lblDesignation.TabIndex = 4;
			this.lblDesignation.Text = "Designation";
			//
			//cbManufacturer
			//
			this.cbManufacturer.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbManufacturer.Location = new System.Drawing.Point(108, 48);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(500, 24);
			this.cbManufacturer.TabIndex = 3;
			this.cbManufacturer.Tag = "Required";
			this.ttBase.SetToolTip(this.cbManufacturer, "Manufacturer of this aircraft");
			//
			//lblManufacturer
			//
			this.lblManufacturer.AutoSize = true;
			this.lblManufacturer.Location = new System.Drawing.Point(6, 51);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(94, 19);
			this.lblManufacturer.TabIndex = 2;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//txtVersion
			//
			this.txtVersion.Location = new System.Drawing.Point(424, 133);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.Size = new System.Drawing.Size(168, 23);
			this.txtVersion.TabIndex = 13;
			this.txtVersion.Tag = "";
			this.txtVersion.Text = "txtVersion";
			this.ttBase.SetToolTip(this.txtVersion, "Version or variant of this aircraft");
			//
			//lblVersion
			//
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(356, 135);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(55, 19);
			this.lblVersion.TabIndex = 12;
			this.lblVersion.Text = "Version";
			//
			//dtpServiceDate
			//
			this.dtpServiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpServiceDate.Location = new System.Drawing.Point(464, 76);
			this.dtpServiceDate.Name = "dtpServiceDate";
			this.dtpServiceDate.Size = new System.Drawing.Size(132, 23);
			this.dtpServiceDate.TabIndex = 7;
			this.ttBase.SetToolTip(this.dtpServiceDate, "Date this aircraft entered service");
			//
			//lblServiceDate
			//
			this.lblServiceDate.AutoSize = true;
			this.lblServiceDate.Location = new System.Drawing.Point(354, 78);
			this.lblServiceDate.Name = "lblServiceDate";
			this.lblServiceDate.Size = new System.Drawing.Size(91, 19);
			this.lblServiceDate.TabIndex = 6;
			this.lblServiceDate.Text = "Service Date";
			//
			//lblNicknames
			//
			this.lblNicknames.AutoSize = true;
			this.lblNicknames.Location = new System.Drawing.Point(22, 163);
			this.lblNicknames.Name = "lblNicknames";
			this.lblNicknames.Size = new System.Drawing.Size(78, 19);
			this.lblNicknames.TabIndex = 14;
			this.lblNicknames.Text = "Nicknames";
			//
			//txtNicknames
			//
			this.txtNicknames.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtNicknames.Location = new System.Drawing.Point(108, 161);
			this.txtNicknames.Name = "txtNicknames";
			this.txtNicknames.Size = new System.Drawing.Size(496, 23);
			this.txtNicknames.TabIndex = 15;
			this.txtNicknames.Tag = "";
			this.txtNicknames.Text = "txtNicknames";
			this.ttBase.SetToolTip(this.txtNicknames, "Unofficial nickname(s) for this aircraft");
			//
			//frmAircraftDesignations
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 419);
			this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
			this.Name = "frmAircraftDesignations";
			this.Text = "frmAircraftDesignations";
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			this.tpNotes.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.gbGeneral.ResumeLayout(false);
			this.ResumeLayout(false);

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
			//General
			BindControl(txtName, mTCBase.MainDataView, "Name");
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsAircraftDesignations)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(txtDesignation, mTCBase.MainDataView, "Designation");
			BindControl(dtpServiceDate, mTCBase.MainDataView, "Service Date");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsAircraftDesignations)mTCBase).Types, "Type", "Type");
			BindControl(txtNumber, mTCBase.MainDataView, "Number");
			BindControl(txtVersion, mTCBase.MainDataView, "Version");
			BindControl(txtNicknames, mTCBase.MainDataView, "Nicknames");
			//Notes...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		#endregion
		#region "Event Handlers"
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmAircraftDesignations.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(cbManufacturer);
				RemoveControlHandlers(txtDesignation);
				txtDesignation.Validating -= SetCaption;
				RemoveControlHandlers(cbType);
				RemoveControlHandlers(txtNumber);
				RemoveControlHandlers(txtVersion);
				RemoveControlHandlers(txtNicknames);
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
			const string EntryName = "frmAircraftDesignations.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(cbManufacturer);
				SetupControlHandlers(txtDesignation);
				txtDesignation.Validating += SetCaption;
				SetupControlHandlers(cbType);
				SetupControlHandlers(txtNumber);
				SetupControlHandlers(txtVersion);
				SetupControlHandlers(txtNicknames);
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
				if ((sender != null))
					base.epBase.SetError((Control)sender, bpeNullString);

				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["Designation"]) ? bpeNullString : mTCBase.CurrentRow["Designation"] + " ") + (Information.IsDBNull(mTCBase.CurrentRow["Name"]) ? bpeNullString : mTCBase.CurrentRow["Name"]);
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
