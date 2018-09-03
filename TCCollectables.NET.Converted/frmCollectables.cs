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
//frmCollectables.vb
//   Collectables Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen controls to accommodate new fields;
//   01/04/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCCollectables
{
	public class frmCollectables : frmTCStandard
	{
		const string myFormName = "frmCollectables";
		public frmCollectables(clsSupport objSupport, clsCollectables objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmCollectables() : base()
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
		internal System.Windows.Forms.Label lblName;
		internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.CheckBox chkOutOfProduction;
		internal System.Windows.Forms.Label lblReference;
		internal System.Windows.Forms.TextBox txtReference;
		internal System.Windows.Forms.ComboBox cbType;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		internal System.Windows.Forms.Label lblSeries;
		internal System.Windows.Forms.ComboBox cbSeries;
		internal System.Windows.Forms.Label lblCondition;
		internal System.Windows.Forms.ComboBox cbCondition;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.chkOutOfProduction = new System.Windows.Forms.CheckBox();
			this.lblReference = new System.Windows.Forms.Label();
			this.txtReference = new System.Windows.Forms.TextBox();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
			this.lblSeries = new System.Windows.Forms.Label();
			this.cbSeries = new System.Windows.Forms.ComboBox();
			this.lblCondition = new System.Windows.Forms.Label();
			this.cbCondition = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			this.gbGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tpNotes.SuspendLayout();
			this.SuspendLayout();
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(320, 216);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 11;
			this.ttBase.SetToolTip(this.dtpPurchased, "Date this item was purchased");
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(292, 192);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 9;
			this.ttBase.SetToolTip(this.chkWishList, "Is this a WishList item?");
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(240, 218);
			this.lblPurchased.Name = "lblPurchased";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Location = new System.Drawing.Point(136, 273);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 13;
			this.ttBase.SetToolTip(this.dtpInventoried, "Date this item was last inventoried (i.e. location confirmed)");
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 435);
			this.lblID.Name = "lblID";
			//
			//sbpTime
			//
			this.sbpTime.Text = "1:37 AM";
			this.sbpTime.Width = 72;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 483;
			//
			//sbpStatus
			//
			this.sbpStatus.Text = "Filter Off";
			this.sbpStatus.Width = 74;
			//
			//sbpPosition
			//
			this.sbpPosition.Text = "";
			this.sbpPosition.Width = 10;
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 396);
			this.btnPrev.Name = "btnPrev";
			//
			//lblInventoried
			//
			this.lblInventoried.Location = new System.Drawing.Point(52, 275);
			this.lblInventoried.Name = "lblInventoried";
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(616, 396);
			this.btnNext.Name = "btnNext";
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 396);
			this.btnFirst.Name = "btnFirst";
			//
			//cbLocation
			//
			this.cbLocation.Location = new System.Drawing.Point(88, 244);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.TabIndex = 12;
			this.cbLocation.Tag = "Required";
			this.ttBase.SetToolTip(this.cbLocation, "Current location of this item (i.e. Where is it?)");
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(643, 396);
			this.btnLast.Name = "btnLast";
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(320, 168);
			this.dtpVerified.Name = "dtpVerified";
			this.dtpVerified.TabIndex = 7;
			this.dtpVerified.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(258, 170);
			this.lblVerified.Name = "lblVerified";
			this.lblVerified.Visible = true;
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(512, 432);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 16;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(116, 168);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(92, 23);
			this.txtValue.TabIndex = 6;
			this.txtValue.Tag = "Money,Nulls,Required";
			this.txtValue.Visible = true;
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(64, 170);
			this.lblValue.Name = "lblValue";
			this.lblValue.Visible = true;
			//
			//btnCancel
			//
			this.btnCancel.CausesValidation = true;
			this.btnCancel.Location = new System.Drawing.Point(593, 432);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 17;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 468);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(500, 28);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(128, 192);
			//
			//lblLocation
			//
			this.lblLocation.Location = new System.Drawing.Point(16, 247);
			this.lblLocation.Name = "lblLocation";
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(500, 224);
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(128, 17);
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Location = new System.Drawing.Point(508, 120);
			this.txtAlphaSort.Name = "txtAlphaSort";
			this.txtAlphaSort.Size = new System.Drawing.Size(116, 23);
			//
			//sbpMode
			//
			this.sbpMode.Text = "";
			this.sbpMode.Width = 10;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Location = new System.Drawing.Point(520, 112);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.Visible = false;
			//
			//sbpFilter
			//
			this.sbpFilter.Text = "";
			this.sbpFilter.Width = 10;
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(500, 28);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(128, 192);
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(593, 432);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 18;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 396);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(548, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 436);
			this.txtID.Name = "txtID";
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(648, 312);
			this.rtfNotes.TabIndex = 15;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(116, 216);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.Size = new System.Drawing.Size(92, 23);
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Tag = "Money,Nulls,Required";
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Location = new System.Drawing.Point(512, 112);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(108, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.lblCondition);
			this.gbGeneral.Controls.Add(this.cbCondition);
			this.gbGeneral.Controls.Add(this.lblSeries);
			this.gbGeneral.Controls.Add(this.cbSeries);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Controls.Add(this.lblReference);
			this.gbGeneral.Controls.Add(this.txtReference);
			this.gbGeneral.Controls.Add(this.chkOutOfProduction);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(640, 308);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkOutOfProduction, 0);
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
			this.gbGeneral.Controls.SetChildIndex(this.txtReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbSeries, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblSeries, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCondition, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCondition, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			//
			//tcMain
			//
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(660, 352);
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(652, 323);
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(652, 323);
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(69, 218);
			this.lblPrice.Name = "lblPrice";
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(17, 31);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 19);
			this.lblName.TabIndex = 60;
			this.lblName.Text = "Name";
			//
			//txtName
			//
			this.txtName.Location = new System.Drawing.Point(64, 29);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(420, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			//
			//chkOutOfProduction
			//
			this.chkOutOfProduction.Location = new System.Drawing.Point(116, 192);
			this.chkOutOfProduction.Name = "chkOutOfProduction";
			this.chkOutOfProduction.Size = new System.Drawing.Size(144, 24);
			this.chkOutOfProduction.TabIndex = 8;
			this.chkOutOfProduction.Text = "Out of Production";
			//
			//lblReference
			//
			this.lblReference.AutoSize = true;
			this.lblReference.Location = new System.Drawing.Point(34, 142);
			this.lblReference.Name = "lblReference";
			this.lblReference.Size = new System.Drawing.Size(72, 19);
			this.lblReference.TabIndex = 66;
			this.lblReference.Text = "Reference";
			//
			//txtReference
			//
			this.txtReference.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtReference.Location = new System.Drawing.Point(116, 140);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(368, 23);
			this.txtReference.TabIndex = 5;
			this.txtReference.Tag = "Nulls,Required";
			this.txtReference.Text = "TXTREFERENCE";
			//
			//cbType
			//
			this.cbType.Location = new System.Drawing.Point(64, 56);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(420, 24);
			this.cbType.TabIndex = 2;
			this.cbType.Tag = "Required";
			this.cbType.Text = "cbType";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(24, 60);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 19);
			this.lblType.TabIndex = 68;
			this.lblType.Text = "Type";
			//
			//lblManufacturer
			//
			this.lblManufacturer.AutoSize = true;
			this.lblManufacturer.Location = new System.Drawing.Point(12, 87);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(94, 19);
			this.lblManufacturer.TabIndex = 70;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//cbManufacturer
			//
			this.cbManufacturer.Location = new System.Drawing.Point(116, 84);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(368, 24);
			this.cbManufacturer.TabIndex = 3;
			this.cbManufacturer.Tag = "Required";
			this.cbManufacturer.Text = "cbManufacturer";
			//
			//lblSeries
			//
			this.lblSeries.AutoSize = true;
			this.lblSeries.Location = new System.Drawing.Point(60, 115);
			this.lblSeries.Name = "lblSeries";
			this.lblSeries.Size = new System.Drawing.Size(46, 19);
			this.lblSeries.TabIndex = 72;
			this.lblSeries.Text = "Series";
			//
			//cbSeries
			//
			this.cbSeries.Location = new System.Drawing.Point(116, 112);
			this.cbSeries.Name = "cbSeries";
			this.cbSeries.Size = new System.Drawing.Size(368, 24);
			this.cbSeries.TabIndex = 4;
			this.cbSeries.Tag = "Required";
			this.cbSeries.Text = "cbSeries";
			//
			//lblCondition
			//
			this.lblCondition.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblCondition.AutoSize = true;
			this.lblCondition.Location = new System.Drawing.Point(272, 275);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(68, 19);
			this.lblCondition.TabIndex = 74;
			this.lblCondition.Text = "Condition";
			//
			//cbCondition
			//
			this.cbCondition.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbCondition.Location = new System.Drawing.Point(344, 272);
			this.cbCondition.Name = "cbCondition";
			this.cbCondition.Size = new System.Drawing.Size(260, 24);
			this.cbCondition.TabIndex = 14;
			this.cbCondition.Tag = "Required";
			this.cbCondition.Text = "cbCondition";
			//
			//frmCollectables
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 490);
			this.MinimumSize = new System.Drawing.Size(684, 524);
			this.Name = "frmCollectables";
			this.Text = "frmCollectables";
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			this.gbGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tpNotes.ResumeLayout(false);
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
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsCollectables)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(cbSeries, mTCBase.MainDataView, "Series", ((clsCollectables)mTCBase).Series, "Series", "Series");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsCollectables)mTCBase).Types, "Type", "Type");
			BindControl(cbCondition, mTCBase.MainDataView, "Condition", ((clsCollectables)mTCBase).Conditions, "Condition", "Condition");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsCollectables)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			BindControl(txtName, mTCBase.MainDataView, "Name");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(chkOutOfProduction, mTCBase.MainDataView, "OutOfProduction");
			BindControl(txtReference, mTCBase.MainDataView, "Reference");
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
			const string EntryName = "frmCollectables.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(txtReference);
				txtReference.Validating -= SetCaption;
				RemoveControlHandlers(cbType);
				cbType.Validating -= SetCaption;
				RemoveControlHandlers(cbManufacturer);
				RemoveControlHandlers(cbSeries);
				cbSeries.Validating -= SetCaption;
				RemoveControlHandlers(cbCondition);
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
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
			const string EntryName = "frmCollectables.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(txtReference);
				txtReference.Validating += SetCaption;
				SetupControlHandlers(cbType);
				cbType.Validating += SetCaption;
				SetupControlHandlers(cbManufacturer);
				SetupControlHandlers(cbSeries);
				cbSeries.Validating += SetCaption;
				SetupControlHandlers(cbCondition);
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
				SetupControlHandlers(rtfNotes);

				mTCBase.ActionModeChange += ActionModeChange;
				mTCBase.BeforeMove += BeforeMove;
				mTCBase.AfterMove += AfterMove;
				this.MinimumSize = new Size(this.MinimumSize.Width, this.Size.Height + mTCBase.DynamicMenuHeight);

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
				string temp = mTCBase.CurrentRow["Type"] + "; " + mTCBase.CurrentRow["Series"] + " " + mTCBase.CurrentRow["Reference"] + " - " + mTCBase.CurrentRow["Name"];
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
