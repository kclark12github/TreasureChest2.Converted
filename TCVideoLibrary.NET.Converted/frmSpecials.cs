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
//frmSpecials.vb
//   Specials Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   05/25/15    Added chkWMV;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen controls to accommodate new fields;
//   01/17/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCVideoLibrary
{
	public class frmSpecials : frmTCStandard
	{
		const string myFormName = "frmSpecials";
		public frmSpecials(clsSupport objSupport, clsSpecials objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmSpecials() : base()
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
		internal System.Windows.Forms.Label lblReleaseDate;
		internal System.Windows.Forms.DateTimePicker dtpReleaseDate;
		internal System.Windows.Forms.CheckBox chkStoreBought;
		internal System.Windows.Forms.Label lblDistributor;
		internal System.Windows.Forms.ComboBox cbDistributor;
		internal System.Windows.Forms.Label lblFormat;
		internal System.Windows.Forms.ComboBox cbFormat;
		internal System.Windows.Forms.Label lblSubject;
		internal System.Windows.Forms.ComboBox cbSubject;
		internal System.Windows.Forms.Label lblTitle;
		internal System.Windows.Forms.CheckBox chkWMV;
		internal System.Windows.Forms.TextBox txtTitle;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.lblReleaseDate = new System.Windows.Forms.Label();
			this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
			this.chkStoreBought = new System.Windows.Forms.CheckBox();
			this.lblDistributor = new System.Windows.Forms.Label();
			this.cbDistributor = new System.Windows.Forms.ComboBox();
			this.lblFormat = new System.Windows.Forms.Label();
			this.cbFormat = new System.Windows.Forms.ComboBox();
			this.lblSubject = new System.Windows.Forms.Label();
			this.cbSubject = new System.Windows.Forms.ComboBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
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
			this.btnLast.Location = new System.Drawing.Point(643, 372);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 372);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(616, 372);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 372);
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
			this.sbpMessage.Width = 483;
			//
			//sbpTime
			//
			this.sbpTime.Text = "2:05 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 411);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(208, 198);
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(280, 254);
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(18, 227);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(8, 143);
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(42, 198);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(512, 408);
			this.btnOK.TabIndex = 14;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(593, 408);
			this.btnExit.TabIndex = 16;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(593, 408);
			this.btnCancel.TabIndex = 15;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 444);
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
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
			this.chkWishList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkWishList.Location = new System.Drawing.Point(396, 167);
			this.chkWishList.Size = new System.Drawing.Size(84, 24);
			this.chkWishList.TabIndex = 10;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(284, 196);
			this.dtpPurchased.TabIndex = 12;
			this.dtpPurchased.Tag = "Nulls,Reset";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(368, 252);
			this.dtpInventoried.TabIndex = 15;
			this.dtpInventoried.Tag = "Nulls,Reset";
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(88, 224);
			this.cbLocation.Size = new System.Drawing.Size(396, 24);
			this.cbLocation.TabIndex = 14;
			this.cbLocation.Tag = "Required";
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Location = new System.Drawing.Point(520, -43);
			this.txtAlphaSort.Size = new System.Drawing.Size(97, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(500, 28);
			this.pbGeneral.Size = new System.Drawing.Size(126, 231);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 372);
			this.txtCaption.Size = new System.Drawing.Size(548, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 412);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(648, 296);
			this.rtfNotes.TabIndex = 13;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(88, 196);
			this.txtPrice.TabIndex = 11;
			this.txtPrice.Tag = "Money,Nulls,Required";
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(88, 140);
			this.cbAlphaSort.Size = new System.Drawing.Size(392, 24);
			this.cbAlphaSort.TabIndex = 7;
			this.cbAlphaSort.Tag = "Required,UPPER";
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.chkWMV);
			this.gbGeneral.Controls.Add(this.lblReleaseDate);
			this.gbGeneral.Controls.Add(this.dtpReleaseDate);
			this.gbGeneral.Controls.Add(this.chkStoreBought);
			this.gbGeneral.Controls.Add(this.lblDistributor);
			this.gbGeneral.Controls.Add(this.cbDistributor);
			this.gbGeneral.Controls.Add(this.lblFormat);
			this.gbGeneral.Controls.Add(this.cbFormat);
			this.gbGeneral.Controls.Add(this.lblSubject);
			this.gbGeneral.Controls.Add(this.cbSubject);
			this.gbGeneral.Controls.Add(this.lblTitle);
			this.gbGeneral.Controls.Add(this.txtTitle);
			this.gbGeneral.Size = new System.Drawing.Size(637, 288);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbFormat, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblFormat, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbDistributor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDistributor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkStoreBought, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWMV, 0);
			//
			//tcMain
			//
			this.tcMain.Size = new System.Drawing.Size(660, 328);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(652, 299);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(656, 270);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(500, 77);
			this.hsbGeneral.Size = new System.Drawing.Size(142, 17);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(500, 28);
			this.pbGeneral2.Size = new System.Drawing.Size(130, 157);
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(37, 170);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(88, 168);
			this.txtValue.TabIndex = 8;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(208, 170);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(264, 168);
			this.dtpVerified.TabIndex = 9;
			this.dtpVerified.Visible = true;
			//
			//lblReleaseDate
			//
			this.lblReleaseDate.AutoSize = true;
			this.lblReleaseDate.Location = new System.Drawing.Point(304, 87);
			this.lblReleaseDate.Name = "lblReleaseDate";
			this.lblReleaseDate.Size = new System.Drawing.Size(66, 16);
			this.lblReleaseDate.TabIndex = 99;
			this.lblReleaseDate.Text = "Released";
			//
			//dtpReleaseDate
			//
			this.dtpReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpReleaseDate.Location = new System.Drawing.Point(372, 85);
			this.dtpReleaseDate.Name = "dtpReleaseDate";
			this.dtpReleaseDate.Size = new System.Drawing.Size(108, 23);
			this.dtpReleaseDate.TabIndex = 4;
			this.dtpReleaseDate.Tag = "Nulls,Required";
			//
			//chkStoreBought
			//
			this.chkStoreBought.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkStoreBought.Location = new System.Drawing.Point(300, 112);
			this.chkStoreBought.Name = "chkStoreBought";
			this.chkStoreBought.Size = new System.Drawing.Size(116, 24);
			this.chkStoreBought.TabIndex = 6;
			this.chkStoreBought.Tag = "Nulls";
			this.chkStoreBought.Text = "Store Bought";
			//
			//lblDistributor
			//
			this.lblDistributor.AutoSize = true;
			this.lblDistributor.Location = new System.Drawing.Point(4, 115);
			this.lblDistributor.Name = "lblDistributor";
			this.lblDistributor.Size = new System.Drawing.Size(76, 16);
			this.lblDistributor.TabIndex = 96;
			this.lblDistributor.Text = "Distributor";
			//
			//cbDistributor
			//
			this.cbDistributor.Location = new System.Drawing.Point(88, 112);
			this.cbDistributor.Name = "cbDistributor";
			this.cbDistributor.Size = new System.Drawing.Size(196, 24);
			this.cbDistributor.TabIndex = 5;
			this.cbDistributor.Tag = "Required";
			this.cbDistributor.Text = "cbDistributor";
			//
			//lblFormat
			//
			this.lblFormat.AutoSize = true;
			this.lblFormat.Location = new System.Drawing.Point(25, 87);
			this.lblFormat.Name = "lblFormat";
			this.lblFormat.Size = new System.Drawing.Size(54, 16);
			this.lblFormat.TabIndex = 95;
			this.lblFormat.Text = "Format";
			//
			//cbFormat
			//
			this.cbFormat.DropDownWidth = 250;
			this.cbFormat.Location = new System.Drawing.Point(88, 84);
			this.cbFormat.Name = "cbFormat";
			this.cbFormat.Size = new System.Drawing.Size(196, 24);
			this.cbFormat.TabIndex = 3;
			this.cbFormat.Tag = "Required";
			this.cbFormat.Text = "cbFormat";
			//
			//lblSubject
			//
			this.lblSubject.AutoSize = true;
			this.lblSubject.Location = new System.Drawing.Point(24, 59);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(59, 16);
			this.lblSubject.TabIndex = 94;
			this.lblSubject.Text = "Subject";
			//
			//cbSubject
			//
			this.cbSubject.Location = new System.Drawing.Point(88, 56);
			this.cbSubject.Name = "cbSubject";
			this.cbSubject.Size = new System.Drawing.Size(392, 24);
			this.cbSubject.TabIndex = 2;
			this.cbSubject.Tag = "Required";
			this.cbSubject.Text = "cbSubject";
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(44, 31);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(37, 16);
			this.lblTitle.TabIndex = 93;
			this.lblTitle.Text = "Title";
			//
			//txtTitle
			//
			this.txtTitle.Location = new System.Drawing.Point(88, 29);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(392, 23);
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Tag = "Required";
			this.txtTitle.Text = "txtTitle";
			//
			//chkWMV
			//
			this.chkWMV.Location = new System.Drawing.Point(412, 194);
			this.chkWMV.Name = "chkWMV";
			this.chkWMV.Size = new System.Drawing.Size(68, 24);
			this.chkWMV.TabIndex = 13;
			this.chkWMV.Tag = "Nulls";
			this.chkWMV.Text = "WMV";
			//
			//frmSpecials
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 466);
			this.MinimumSize = new System.Drawing.Size(692, 525);
			this.Name = "frmSpecials";
			this.Text = "frmSpecials";
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
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(cbFormat, mTCBase.MainDataView, "Format", ((clsSpecials)mTCBase).Formats, "Format", "Format");
			BindControl(chkStoreBought, mTCBase.MainDataView, "StoreBought");
			BindControl(chkWMV, mTCBase.MainDataView, "WMV");
			BindControl(cbSubject, mTCBase.MainDataView, "Subject", ((clsSpecials)mTCBase).Subjects, "Subject", "Subject");
			BindControl(dtpReleaseDate, mTCBase.MainDataView, "ReleaseDate");
			BindControl(cbDistributor, mTCBase.MainDataView, "Distributor", ((clsSpecials)mTCBase).Distributors, "Distributor", "Distributor");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsSpecials)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			BindControl(cbAlphaSort, mTCBase.MainDataView, "Sort");
			//Note that this guy is Simple-Bound...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2241:Provide correct arguments to formatting methods")]
		public void DefaultAlphaSort(System.Windows.Forms.ComboBox cbAlphaSort, string Subject, string Title, string Format, string YearReleased)
		{
			Subject = Subject.ToUpper();
			Title = Title.ToUpper();
			if (Title.StartsWith("THE "))
				Title = Title.Substring("THE ".Length) + ", THE";
			if (Title.StartsWith("A "))
				Title = Title.Substring("A ".Length) + ", A";
			if (Title.StartsWith("AN "))
				Title = Title.Substring("AN ".Length) + ", AN";
			Format = Format.ToUpper();

			if (cbAlphaSort.Items.Count > 0)
				cbAlphaSort.Items.Clear();
			mTCBase.cbAddItem(cbAlphaSort, cbAlphaSort.Text);

			//Patterns:
			//   {Subject}: {Title}
			//   {Subject}: {SeriesName} ##
			//   {Subject}: {Title} ({Year})
			//   {Subject}: {SeriesName} ## ({Year})
			//   {Format}: {Subject}; {Title}
			//   {Format}: {Subject}; {SeriesName} ##
			//   {Format}: {Subject}; {Title} ({Year})
			//   {Format}: {Subject}; {SeriesName} ## ({Year})
			string[] strList = {
				Format,
				Subject,
				Title,
				YearReleased
			};
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{1}: {2}", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{0}: <SeriesName> ##", strList[1]));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{1}: {2} ({3})", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{1}: <SeriesName> ## ({3})", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{0}: {1}; <SeriesName> ##", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2} ({3})", strList));
			mTCBase.cbAddItem(cbAlphaSort, string.Format("{0}: {1}; <SeriesName> ## ({2})", strList));
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
						chkStoreBought.Visible = false;
						if (mTCBase.Mode != clsTCBase.ActionModeEnum.modeDisplay) {
                            mTCBase.CurrentRow["Location"] = DBNull.Value;
                            mTCBase.CurrentRow["DateInventoried"] = DBNull.Value;
                            mTCBase.CurrentRow["Price"] = DBNull.Value;
                            mTCBase.CurrentRow["DatePurchased"] = DBNull.Value;
                            mTCBase.CurrentRow["StoreBought"] = 0;
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
						chkStoreBought.Visible = true;
						break;
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void DefaultAlphaSort(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				ComboBox cb = (ComboBox)sender;
				//Give the user options...
				DefaultAlphaSort(cb, this.cbSubject.Text, this.txtTitle.Text, this.cbFormat.Text, this.dtpReleaseDate.Text);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmSpecials.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating -= SetCaption;
				cbAlphaSort.Enter -= DefaultAlphaSort;
				RemoveControlHandlers(txtTitle);
				RemoveControlHandlers(cbSubject);
				RemoveControlHandlers(cbFormat);
				RemoveControlHandlers(cbDistributor);
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
				RemoveControlHandlers(dtpReleaseDate);
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
			const string EntryName = "frmSpecials.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating += SetCaption;
				cbAlphaSort.Enter += DefaultAlphaSort;
				SetupControlHandlers(txtTitle);
				SetupControlHandlers(cbSubject);
				SetupControlHandlers(cbFormat);
				SetupControlHandlers(cbDistributor);
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
				SetupControlHandlers(dtpReleaseDate);
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
				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["Sort"]) ? bpeNullString : (string)mTCBase.CurrentRow["Sort"]);
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
