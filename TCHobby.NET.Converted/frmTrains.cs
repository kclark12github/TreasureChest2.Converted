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
//frmTrains.vb
//   Trains Form...
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
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmTrains : frmTCStandard
	{
		const string myFormName = "frmTrains";
		public frmTrains(clsSupport objSupport, clsTrains objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmTrains() : base()
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
		internal System.Windows.Forms.TextBox txtLine;
		internal System.Windows.Forms.Label lblLine;
		internal System.Windows.Forms.TextBox txtReference;
		internal System.Windows.Forms.Label lblReference;
		internal System.Windows.Forms.Label lblCatalog;
		internal System.Windows.Forms.ComboBox cbCatalog;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		internal System.Windows.Forms.Label lblScale;
		internal System.Windows.Forms.ComboBox cbScale;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.ComboBox cbType;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtLine = new System.Windows.Forms.TextBox();
			this.lblLine = new System.Windows.Forms.Label();
			this.txtReference = new System.Windows.Forms.TextBox();
			this.lblReference = new System.Windows.Forms.Label();
			this.lblCatalog = new System.Windows.Forms.Label();
			this.cbCatalog = new System.Windows.Forms.ComboBox();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
			this.lblScale = new System.Windows.Forms.Label();
			this.cbScale = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.cbType = new System.Windows.Forms.ComboBox();
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
			this.dtpPurchased.Location = new System.Drawing.Point(296, 220);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 11;
			this.ttBase.SetToolTip(this.dtpPurchased, "Date this item was purchased");
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(104, 192);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 9;
			this.ttBase.SetToolTip(this.chkWishList, "Is this a WishList item?");
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(218, 224);
			this.lblPurchased.Name = "lblPurchased";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(296, 276);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 13;
			this.ttBase.SetToolTip(this.dtpInventoried, "Date this item was last inventoried (i.e. location confirmed)");
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 474);
			this.lblID.Name = "lblID";
			//
			//sbpTime
			//
			this.sbpTime.Text = "9:48 AM";
			this.sbpTime.Width = 72;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 599;
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
			this.btnPrev.Location = new System.Drawing.Point(36, 431);
			this.btnPrev.Name = "btnPrev";
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(208, 278);
			this.lblInventoried.Name = "lblInventoried";
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(732, 431);
			this.btnNext.Name = "btnNext";
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 431);
			this.btnFirst.Name = "btnFirst";
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(104, 248);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(308, 24);
			this.cbLocation.TabIndex = 12;
			this.ttBase.SetToolTip(this.cbLocation, "Current location of this item (i.e. Where is it?)");
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(759, 431);
			this.btnLast.Name = "btnLast";
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(296, 164);
			this.dtpVerified.Name = "dtpVerified";
			this.dtpVerified.TabIndex = 8;
			this.dtpVerified.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(236, 168);
			this.lblVerified.Name = "lblVerified";
			this.lblVerified.Visible = true;
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(628, 471);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 15;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(104, 164);
			this.txtValue.Name = "txtValue";
			this.txtValue.TabIndex = 7;
			this.txtValue.Visible = true;
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(56, 168);
			this.lblValue.Name = "lblValue";
			this.lblValue.Visible = true;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(709, 471);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 16;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 507);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(792, 22);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(428, 56);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(320, 256);
			this.pbGeneral2.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(37, 251);
			this.lblLocation.Name = "lblLocation";
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(424, 316);
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(324, 17);
			this.hsbGeneral.Visible = false;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(552, 216);
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
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(472, 216);
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
			this.pbGeneral.Location = new System.Drawing.Point(428, 56);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(320, 256);
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(709, 471);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 17;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 431);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(664, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 475);
			this.txtID.Name = "txtID";
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(764, 355);
			this.rtfNotes.TabIndex = 14;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(104, 220);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 10;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(552, 216);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(108, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.lblScale);
			this.gbGeneral.Controls.Add(this.cbScale);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Controls.Add(this.txtReference);
			this.gbGeneral.Controls.Add(this.lblReference);
			this.gbGeneral.Controls.Add(this.lblCatalog);
			this.gbGeneral.Controls.Add(this.cbCatalog);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.txtLine);
			this.gbGeneral.Controls.Add(this.lblLine);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(756, 343);
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
			this.gbGeneral.Controls.SetChildIndex(this.lblLine, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtLine, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblScale, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			//
			//tcMain
			//
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(776, 387);
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(768, 358);
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(768, 358);
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(61, 224);
			this.lblPrice.Name = "lblPrice";
			//
			//txtLine
			//
			this.txtLine.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtLine.Location = new System.Drawing.Point(104, 24);
			this.txtLine.Name = "txtLine";
			this.txtLine.Size = new System.Drawing.Size(640, 23);
			this.txtLine.TabIndex = 1;
			this.txtLine.Tag = "Required";
			this.txtLine.Text = "txtLine";
			//
			//lblLine
			//
			this.lblLine.AutoSize = true;
			this.lblLine.Location = new System.Drawing.Point(66, 26);
			this.lblLine.Name = "lblLine";
			this.lblLine.Size = new System.Drawing.Size(32, 19);
			this.lblLine.TabIndex = 102;
			this.lblLine.Text = "Line";
			//
			//txtReference
			//
			this.txtReference.Location = new System.Drawing.Point(104, 108);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(128, 23);
			this.txtReference.TabIndex = 4;
			this.txtReference.Tag = "";
			this.txtReference.Text = "txtReference";
			//
			//lblReference
			//
			this.lblReference.AutoSize = true;
			this.lblReference.Location = new System.Drawing.Point(26, 112);
			this.lblReference.Name = "lblReference";
			this.lblReference.Size = new System.Drawing.Size(72, 19);
			this.lblReference.TabIndex = 108;
			this.lblReference.Text = "Reference";
			//
			//lblCatalog
			//
			this.lblCatalog.AutoSize = true;
			this.lblCatalog.Location = new System.Drawing.Point(42, 140);
			this.lblCatalog.Name = "lblCatalog";
			this.lblCatalog.Size = new System.Drawing.Size(56, 19);
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
			this.lblManufacturer.Location = new System.Drawing.Point(4, 84);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(94, 19);
			this.lblManufacturer.TabIndex = 106;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//cbManufacturer
			//
			this.cbManufacturer.DropDownWidth = 300;
			this.cbManufacturer.Location = new System.Drawing.Point(104, 80);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(308, 24);
			this.cbManufacturer.TabIndex = 3;
			this.cbManufacturer.Text = "cbManufacturer";
			//
			//lblScale
			//
			this.lblScale.AutoSize = true;
			this.lblScale.Location = new System.Drawing.Point(268, 112);
			this.lblScale.Name = "lblScale";
			this.lblScale.Size = new System.Drawing.Size(41, 19);
			this.lblScale.TabIndex = 116;
			this.lblScale.Text = "Scale";
			//
			//cbScale
			//
			this.cbScale.Location = new System.Drawing.Point(316, 108);
			this.cbScale.Name = "cbScale";
			this.cbScale.Size = new System.Drawing.Size(96, 24);
			this.cbScale.TabIndex = 5;
			this.cbScale.Text = "cbScale";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(61, 56);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 19);
			this.lblType.TabIndex = 115;
			this.lblType.Text = "Type";
			//
			//cbType
			//
			this.cbType.DropDownWidth = 400;
			this.cbType.Location = new System.Drawing.Point(104, 52);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(308, 24);
			this.cbType.TabIndex = 2;
			this.cbType.Text = "cbType";
			//
			//frmTrains
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(792, 529);
			this.MinimumSize = new System.Drawing.Size(800, 563);
			this.Name = "frmTrains";
			this.Text = "frmTrains";
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
			BindControl(txtLine, mTCBase.MainDataView, "Line");
			BindControl(cbScale, mTCBase.MainDataView, "Scale", ((clsTrains)mTCBase).Scales, "Scale", "Scale");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsTrains)mTCBase).Types, "Type", "Type");
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsTrains)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(cbCatalog, mTCBase.MainDataView, "ProductCatalog", ((clsTrains)mTCBase).Catalogs, "ProductCatalog", "ProductCatalog");
			BindControl(txtReference, mTCBase.MainDataView, "Reference");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsTrains)mTCBase).Locations, "Location", "Location");
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
			const string EntryName = "frmTrains.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtLine);
				txtLine.Validating -= DefaultTextBox;
				txtLine.Validating -= SetCaption;
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
			const string EntryName = "frmTrains.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtLine);
				txtLine.Validating += DefaultTextBox;
				txtLine.Validating += SetCaption;
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
				string temp = (Information.IsDBNull(mTCBase.CurrentRow["Scale"]) ? bpeNullString : mTCBase.CurrentRow["Scale"] + " Scale; ") + (Information.IsDBNull(mTCBase.CurrentRow["Type"]) ? bpeNullString : mTCBase.CurrentRow["Type"] + "; ") + (Information.IsDBNull(mTCBase.CurrentRow["Line"]) ? bpeNullString : mTCBase.CurrentRow["Line"]);
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
