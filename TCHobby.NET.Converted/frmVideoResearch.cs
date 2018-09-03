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
//frmVideoResearch.vb
//   Video Research Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   02/22/17    Added Missing lblFormat/cbFormat controls;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   05/25/15    Added chkWMV;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen controls to accommodate new fields;
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmVideoResearch : frmTCStandard
	{
		const string myFormName = "frmVideoResearch";
		public frmVideoResearch(clsSupport objSupport, clsVideoResearch objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmVideoResearch() : base()
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
		internal System.Windows.Forms.TextBox txtTitle;
		internal System.Windows.Forms.Label lblTitle;
		internal System.Windows.Forms.ComboBox cbSubject;
		internal System.Windows.Forms.Label lblSubject;
		internal System.Windows.Forms.Label lblDistributor;
		internal System.Windows.Forms.ComboBox cbDistributor;
		internal System.Windows.Forms.Label lblReleaseDate;
		internal System.Windows.Forms.CheckBox chkWMV;
		internal System.Windows.Forms.DateTimePicker dtpReleaseDate;
		internal System.Windows.Forms.Label lblFormat;
		internal System.Windows.Forms.ComboBox cbFormat;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblDistributor = new System.Windows.Forms.Label();
			this.cbDistributor = new System.Windows.Forms.ComboBox();
			this.lblSubject = new System.Windows.Forms.Label();
			this.cbSubject = new System.Windows.Forms.ComboBox();
			this.lblFormat = new System.Windows.Forms.Label();
			this.cbFormat = new System.Windows.Forms.ComboBox();
			this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
			this.lblReleaseDate = new System.Windows.Forms.Label();
			this.chkWMV = new System.Windows.Forms.CheckBox();
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
			this.btnLast.Location = new System.Drawing.Point(611, 364);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 364);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(584, 364);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 364);
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
			this.sbpMessage.Width = 451;
			//
			//sbpTime
			//
			this.sbpTime.Text = "2:08 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 407);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(224, 194);
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(212, 250);
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(36, 223);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(26, 112);
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(60, 194);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(480, 404);
			this.btnOK.TabIndex = 15;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(561, 404);
			this.btnExit.TabIndex = 17;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(561, 404);
			this.btnCancel.TabIndex = 16;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 440);
			this.sbStatus.Size = new System.Drawing.Size(644, 22);
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
			this.chkWishList.Location = new System.Drawing.Point(312, 163);
			this.chkWishList.TabIndex = 9;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(300, 192);
			this.dtpPurchased.TabIndex = 11;
			this.dtpPurchased.Tag = "Nulls,Reset";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(300, 248);
			this.dtpInventoried.TabIndex = 13;
			this.dtpInventoried.Tag = "Nulls,Reset";
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(104, 220);
			this.cbLocation.Size = new System.Drawing.Size(312, 24);
			this.cbLocation.TabIndex = 12;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(104, 109);
			this.txtAlphaSort.Size = new System.Drawing.Size(312, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(428, 56);
			this.pbGeneral.Size = new System.Drawing.Size(168, 201);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 364);
			this.txtCaption.Size = new System.Drawing.Size(516, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 408);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(764, 355);
			this.rtfNotes.TabIndex = 14;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(104, 192);
			this.txtPrice.TabIndex = 10;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(104, 108);
			this.cbAlphaSort.Size = new System.Drawing.Size(312, 24);
			this.cbAlphaSort.TabIndex = 4;
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.chkWMV);
			this.gbGeneral.Controls.Add(this.lblReleaseDate);
			this.gbGeneral.Controls.Add(this.dtpReleaseDate);
			this.gbGeneral.Controls.Add(this.lblSubject);
			this.gbGeneral.Controls.Add(this.cbSubject);
			this.gbGeneral.Controls.Add(this.lblFormat);
			this.gbGeneral.Controls.Add(this.cbFormat);
			this.gbGeneral.Controls.Add(this.lblDistributor);
			this.gbGeneral.Controls.Add(this.cbDistributor);
			this.gbGeneral.Controls.Add(this.txtTitle);
			this.gbGeneral.Controls.Add(this.lblTitle);
			this.gbGeneral.Size = new System.Drawing.Size(608, 280);
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
			this.gbGeneral.Controls.SetChildIndex(this.lblTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbDistributor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDistributor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblFormat, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbFormat, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWMV, 0);
			//
			//tcMain
			//
			this.tcMain.Size = new System.Drawing.Size(628, 320);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(620, 291);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(768, 379);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(424, -32);
			this.hsbGeneral.Size = new System.Drawing.Size(0, 17);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(428, 56);
			this.pbGeneral2.Size = new System.Drawing.Size(0, 0);
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(55, 138);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(104, 136);
			this.txtValue.TabIndex = 5;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(244, 138);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(300, 136);
			this.dtpVerified.TabIndex = 6;
			this.dtpVerified.Visible = true;
			//
			//txtTitle
			//
			this.txtTitle.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtTitle.Location = new System.Drawing.Point(104, 24);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(492, 23);
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Tag = "Required";
			this.txtTitle.Text = "txtTitle";
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(63, 26);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(37, 16);
			this.lblTitle.TabIndex = 102;
			this.lblTitle.Text = "Title";
			//
			//lblDistributor
			//
			this.lblDistributor.AutoSize = true;
			this.lblDistributor.Location = new System.Drawing.Point(21, 84);
			this.lblDistributor.Name = "lblDistributor";
			this.lblDistributor.Size = new System.Drawing.Size(76, 16);
			this.lblDistributor.TabIndex = 106;
			this.lblDistributor.Text = "Distributor";
			//
			//cbDistributor
			//
			this.cbDistributor.DropDownWidth = 300;
			this.cbDistributor.Location = new System.Drawing.Point(104, 80);
			this.cbDistributor.Name = "cbDistributor";
			this.cbDistributor.Size = new System.Drawing.Size(312, 24);
			this.cbDistributor.TabIndex = 3;
			this.cbDistributor.Text = "cbDistributor";
			//
			//lblSubject
			//
			this.lblSubject.AutoSize = true;
			this.lblSubject.Location = new System.Drawing.Point(41, 56);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(59, 16);
			this.lblSubject.TabIndex = 3;
			this.lblSubject.Text = "Subject";
			//
			//cbSubject
			//
			this.cbSubject.DropDownWidth = 400;
			this.cbSubject.Location = new System.Drawing.Point(104, 52);
			this.cbSubject.Name = "cbSubject";
			this.cbSubject.Size = new System.Drawing.Size(312, 24);
			this.cbSubject.TabIndex = 2;
			this.cbSubject.Text = "cbSubject";
			//
			//lblFormat
			//
			this.lblFormat.AutoSize = true;
			this.lblFormat.Location = new System.Drawing.Point(41, 112);
			this.lblFormat.Name = "lblFormat";
			this.lblFormat.Size = new System.Drawing.Size(59, 16);
			this.lblFormat.TabIndex = 115;
			this.lblFormat.Text = "Format";
			//
			//cbFormat
			//
			this.cbFormat.DropDownWidth = 400;
			this.cbFormat.Location = new System.Drawing.Point(104, 108);
			this.cbFormat.Name = "cbFormat";
			this.cbFormat.Size = new System.Drawing.Size(312, 24);
			this.cbFormat.TabIndex = 4;
			this.cbFormat.Text = "cbFormat";
			//
			//dtpReleaseDate
			//
			this.dtpReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpReleaseDate.Location = new System.Drawing.Point(104, 164);
			this.dtpReleaseDate.Name = "dtpReleaseDate";
			this.dtpReleaseDate.Size = new System.Drawing.Size(116, 23);
			this.dtpReleaseDate.TabIndex = 7;
			//
			//lblReleaseDate
			//
			this.lblReleaseDate.AutoSize = true;
			this.lblReleaseDate.Location = new System.Drawing.Point(32, 166);
			this.lblReleaseDate.Name = "lblReleaseDate";
			this.lblReleaseDate.Size = new System.Drawing.Size(66, 16);
			this.lblReleaseDate.TabIndex = 117;
			this.lblReleaseDate.Text = "Released";
			//
			//chkWMV
			//
			this.chkWMV.Location = new System.Drawing.Point(241, 162);
			this.chkWMV.Name = "chkWMV";
			this.chkWMV.Size = new System.Drawing.Size(65, 24);
			this.chkWMV.TabIndex = 8;
			this.chkWMV.Tag = "Nulls";
			this.chkWMV.Text = "WMV";
			//
			//frmVideoResearch
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(644, 462);
			this.MinimumSize = new System.Drawing.Size(660, 521);
			this.Name = "frmVideoResearch";
			this.Text = "frmVideoResearch";
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
			BindControl(txtTitle, mTCBase.MainDataView, "Title");
			BindControl(cbDistributor, mTCBase.MainDataView, "Distributor", ((clsVideoResearch)mTCBase).Distributors, "Distributor", "Distributor");
			BindControl(cbFormat, mTCBase.MainDataView, "Format", ((clsVideoResearch)mTCBase).Formats, "Format", "Format");
			BindControl(cbSubject, mTCBase.MainDataView, "Subject", ((clsVideoResearch)mTCBase).Subjects, "Subject", "Subject");
			BindControl(dtpReleaseDate, mTCBase.MainDataView, "ReleaseDate");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(chkWMV, mTCBase.MainDataView, "WMV");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsVideoResearch)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			//BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort")  'Note that this guy is Simple-Bound...
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
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
			const string EntryName = "frmVideoResearch.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtTitle);
				txtTitle.Validating -= SetCaption;
				RemoveControlHandlers(cbFormat);
				RemoveControlHandlers(cbDistributor);
				RemoveControlHandlers(cbSubject);
				cbSubject.Validating -= SetCaption;
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
			const string EntryName = "frmVideoResearch.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtTitle);
				txtTitle.Validating += SetCaption;
				SetupControlHandlers(cbFormat);
				SetupControlHandlers(cbDistributor);
				SetupControlHandlers(cbSubject);
				cbSubject.Validating += SetCaption;
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
				string temp = mTCBase.CurrentRow["Subject"] + "; " + mTCBase.CurrentRow["Title"];
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
