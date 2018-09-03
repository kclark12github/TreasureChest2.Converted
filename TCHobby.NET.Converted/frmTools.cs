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
//frmTools.vb
//   Tools Form...
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
	public class frmTools : frmTCStandard
	{
		const string myFormName = "frmTools";
		public frmTools(clsSupport objSupport, clsTools objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmTools() : base()
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
		internal System.Windows.Forms.TextBox txtReference;
		internal System.Windows.Forms.Label lblReference;
		internal System.Windows.Forms.Label lblCatalog;
		internal System.Windows.Forms.ComboBox cbCatalog;
		internal System.Windows.Forms.Label lblManufacturer;
		internal System.Windows.Forms.ComboBox cbManufacturer;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtReference = new System.Windows.Forms.TextBox();
			this.lblReference = new System.Windows.Forms.Label();
			this.lblCatalog = new System.Windows.Forms.Label();
			this.cbCatalog = new System.Windows.Forms.ComboBox();
			this.lblManufacturer = new System.Windows.Forms.Label();
			this.cbManufacturer = new System.Windows.Forms.ComboBox();
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
			this.dtpPurchased.Location = new System.Drawing.Point(296, 192);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 9;
			this.ttBase.SetToolTip(this.dtpPurchased, "Date this item was purchased");
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(104, 164);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 7;
			this.ttBase.SetToolTip(this.chkWishList, "Is this a WishList item?");
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(222, 194);
			this.lblPurchased.Name = "lblPurchased";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(296, 248);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 11;
			this.ttBase.SetToolTip(this.dtpInventoried, "Date this item was last inventoried (i.e. location confirmed)");
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 474);
			this.lblID.Name = "lblID";
			//
			//sbpTime
			//
			this.sbpTime.Text = "9:49 AM";
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
			this.lblInventoried.Location = new System.Drawing.Point(204, 250);
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
			this.cbLocation.Location = new System.Drawing.Point(104, 220);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(308, 24);
			this.cbLocation.TabIndex = 10;
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
			this.dtpVerified.Location = new System.Drawing.Point(296, 136);
			this.dtpVerified.Name = "dtpVerified";
			this.dtpVerified.TabIndex = 6;
			this.dtpVerified.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(240, 138);
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
			this.txtValue.Location = new System.Drawing.Point(104, 136);
			this.txtValue.Name = "txtValue";
			this.txtValue.TabIndex = 5;
			this.txtValue.Visible = true;
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(55, 138);
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
			this.lblLocation.Location = new System.Drawing.Point(36, 223);
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
			this.txtAlphaSort.Location = new System.Drawing.Point(548, 244);
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
			this.lblAlphaSort.Location = new System.Drawing.Point(472, 248);
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
			this.txtPrice.Location = new System.Drawing.Point(104, 192);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 8;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(548, 244);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(108, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtReference);
			this.gbGeneral.Controls.Add(this.lblReference);
			this.gbGeneral.Controls.Add(this.lblCatalog);
			this.gbGeneral.Controls.Add(this.cbCatalog);
			this.gbGeneral.Controls.Add(this.lblManufacturer);
			this.gbGeneral.Controls.Add(this.cbManufacturer);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblName);
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
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblManufacturer, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCatalog, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReference, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtReference, 0);
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
			this.lblPrice.Location = new System.Drawing.Point(60, 194);
			this.lblPrice.Name = "lblPrice";
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
			this.lblName.Size = new System.Drawing.Size(44, 19);
			this.lblName.TabIndex = 102;
			this.lblName.Text = "Name";
			//
			//txtReference
			//
			this.txtReference.Location = new System.Drawing.Point(104, 80);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(116, 23);
			this.txtReference.TabIndex = 3;
			this.txtReference.Tag = "";
			this.txtReference.Text = "txtReference";
			//
			//lblReference
			//
			this.lblReference.AutoSize = true;
			this.lblReference.Location = new System.Drawing.Point(28, 84);
			this.lblReference.Name = "lblReference";
			this.lblReference.Size = new System.Drawing.Size(72, 19);
			this.lblReference.TabIndex = 108;
			this.lblReference.Text = "Reference";
			//
			//lblCatalog
			//
			this.lblCatalog.AutoSize = true;
			this.lblCatalog.Location = new System.Drawing.Point(41, 112);
			this.lblCatalog.Name = "lblCatalog";
			this.lblCatalog.Size = new System.Drawing.Size(56, 19);
			this.lblCatalog.TabIndex = 107;
			this.lblCatalog.Text = "Catalog";
			//
			//cbCatalog
			//
			this.cbCatalog.Location = new System.Drawing.Point(104, 108);
			this.cbCatalog.Name = "cbCatalog";
			this.cbCatalog.Size = new System.Drawing.Size(308, 24);
			this.cbCatalog.TabIndex = 4;
			this.cbCatalog.Text = "cbCatalog";
			//
			//lblManufacturer
			//
			this.lblManufacturer.AutoSize = true;
			this.lblManufacturer.Location = new System.Drawing.Point(4, 56);
			this.lblManufacturer.Name = "lblManufacturer";
			this.lblManufacturer.Size = new System.Drawing.Size(94, 19);
			this.lblManufacturer.TabIndex = 106;
			this.lblManufacturer.Text = "Manufacturer";
			//
			//cbManufacturer
			//
			this.cbManufacturer.DropDownWidth = 300;
			this.cbManufacturer.Location = new System.Drawing.Point(104, 52);
			this.cbManufacturer.Name = "cbManufacturer";
			this.cbManufacturer.Size = new System.Drawing.Size(308, 24);
			this.cbManufacturer.TabIndex = 2;
			this.cbManufacturer.Text = "cbManufacturer";
			//
			//frmTools
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(792, 529);
			this.MinimumSize = new System.Drawing.Size(800, 563);
			this.Name = "frmTools";
			this.Text = "frmTools";
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
			BindControl(txtName, mTCBase.MainDataView, "Name");
			BindControl(cbManufacturer, mTCBase.MainDataView, "Manufacturer", ((clsTools)mTCBase).Manufacturers, "Manufacturer", "Manufacturer");
			BindControl(cbCatalog, mTCBase.MainDataView, "ProductCatalog", ((clsTools)mTCBase).Catalogs, "ProductCatalog", "ProductCatalog");
			BindControl(txtReference, mTCBase.MainDataView, "Reference");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsTools)mTCBase).Locations, "Location", "Location");
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
			const string EntryName = "frmTools.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
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
			const string EntryName = "frmTools.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
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
				string temp = (Information.IsDBNull(mTCBase.CurrentRow["Manufacturer"]) ? bpeNullString : mTCBase.CurrentRow["Manufacturer"] + "; ") + (Information.IsDBNull(mTCBase.CurrentRow["Name"]) ? bpeNullString : mTCBase.CurrentRow["Name"]);
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
