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
//frmBooks.vb
//   Books Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   06/09/17    Introduced FixAlphasort to add logic to truncating values exceeding its MaxLength;
//   06/04/17    Added txtISBN default;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   10/03/10    Rearranged screen layout to accommodate new fields;
//   01/04/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBooks
{
	public class frmBooks : frmTCStandard
	{
		const string myFormName = "frmBooks";
		public frmBooks(clsSupport objSupport, clsBooks objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			mISBN = new clsISBN(mSupport);

			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			LoadDefaultImage();
			BindControls();
			EnableControls(false);

			Icon = mTCBase.Icon;
			MinimumSize = new Size(Width, Height);
			//ReportPath = "TCBase.Books.rdlc"
			ReportPath = mTCBase.ReportPath;
		}
		#region " Windows Form Designer generated code "
		public frmBooks() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			mISBN = null;
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		protected internal System.Windows.Forms.TextBox txtTitle;
		internal System.Windows.Forms.Label lblTitle;
		protected internal System.Windows.Forms.ComboBox cbAuthor;
		internal System.Windows.Forms.Label lblAuthor;
		protected internal System.Windows.Forms.TextBox txtISBN;
		internal System.Windows.Forms.Label lblISBN;
		protected internal System.Windows.Forms.ComboBox cbSubject;
		internal System.Windows.Forms.Label lblSubject;
		internal System.Windows.Forms.Label lblMisc;
		protected internal System.Windows.Forms.TextBox txtMisc;
		internal System.Windows.Forms.CheckBox chkCataloged;
		protected internal ComboBox cbFormat;
		internal System.Windows.Forms.Label lblFormat;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.cbAuthor = new System.Windows.Forms.ComboBox();
			this.lblAuthor = new System.Windows.Forms.Label();
			this.txtISBN = new System.Windows.Forms.TextBox();
			this.lblISBN = new System.Windows.Forms.Label();
			this.cbSubject = new System.Windows.Forms.ComboBox();
			this.lblSubject = new System.Windows.Forms.Label();
			this.lblMisc = new System.Windows.Forms.Label();
			this.txtMisc = new System.Windows.Forms.TextBox();
			this.chkCataloged = new System.Windows.Forms.CheckBox();
			this.lblFormat = new System.Windows.Forms.Label();
			this.cbFormat = new System.Windows.Forms.ComboBox();
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
			this.btnLast.Location = new System.Drawing.Point(739, 356);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(12, 356);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(712, 356);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(40, 356);
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
			this.sbpMessage.Width = 570;
			//
			//sbpTime
			//
			this.sbpTime.Text = "8:28 AM";
			this.sbpTime.Width = 72;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 395);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(232, 234);
			//
			//lblInventoried
			//
			this.lblInventoried.Location = new System.Drawing.Point(484, 208);
			this.lblInventoried.TabIndex = 13;
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(12, 206);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(8, 177);
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(36, 234);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(608, 392);
			this.btnOK.TabIndex = 0;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(688, 392);
			this.btnExit.TabIndex = 1;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(688, 392);
			this.btnCancel.TabIndex = 14;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 428);
			this.sbStatus.Size = new System.Drawing.Size(772, 22);
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
			this.chkWishList.Location = new System.Drawing.Point(492, 174);
			this.chkWishList.Size = new System.Drawing.Size(88, 24);
			this.chkWishList.TabIndex = 10;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(316, 232);
			this.dtpPurchased.Size = new System.Drawing.Size(108, 23);
			this.dtpPurchased.TabIndex = 15;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(472, 204);
			this.dtpInventoried.Size = new System.Drawing.Size(108, 23);
			this.dtpInventoried.TabIndex = 12;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(88, 203);
			this.cbLocation.Size = new System.Drawing.Size(372, 24);
			this.cbLocation.TabIndex = 11;
			this.cbLocation.Tag = "Required";
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(90, 175);
			this.txtAlphaSort.Size = new System.Drawing.Size(378, 23);
			this.txtAlphaSort.TabIndex = 9;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(68, 356);
			this.txtCaption.Size = new System.Drawing.Size(640, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 396);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(648, 280);
			this.rtfNotes.TabIndex = 12;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(88, 232);
			this.txtPrice.TabIndex = 14;
			this.txtPrice.Tag = "Money,Nulls,Required";
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(88, 174);
			this.cbAlphaSort.Size = new System.Drawing.Size(388, 24);
			this.cbAlphaSort.TabIndex = 8;
			this.cbAlphaSort.Tag = "Required,UPPER";
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.cbFormat);
			this.gbGeneral.Controls.Add(this.chkCataloged);
			this.gbGeneral.Controls.Add(this.txtMisc);
			this.gbGeneral.Controls.Add(this.lblMisc);
			this.gbGeneral.Controls.Add(this.lblFormat);
			this.gbGeneral.Controls.Add(this.cbSubject);
			this.gbGeneral.Controls.Add(this.lblSubject);
			this.gbGeneral.Controls.Add(this.txtISBN);
			this.gbGeneral.Controls.Add(this.lblISBN);
			this.gbGeneral.Controls.Add(this.cbAuthor);
			this.gbGeneral.Controls.Add(this.lblAuthor);
			this.gbGeneral.Controls.Add(this.txtTitle);
			this.gbGeneral.Controls.Add(this.lblTitle);
			this.gbGeneral.Size = new System.Drawing.Size(828, 281);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAuthor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAuthor, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblISBN, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtISBN, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbSubject, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblFormat, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblMisc, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtMisc, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkCataloged, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbFormat, 0);
			//
			//tcMain
			//
			this.tcMain.Size = new System.Drawing.Size(756, 312);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(748, 283);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(748, 283);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(592, 220);
			this.hsbGeneral.Size = new System.Drawing.Size(125, 17);
			this.hsbGeneral.BringToFront();
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(596, 29);
			this.pbGeneral.Size = new System.Drawing.Size(125, 192);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(596, 29);
			this.pbGeneral2.Size = new System.Drawing.Size(125, 192);
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(236, 86);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(284, 84);
			this.txtValue.TabIndex = 3;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(404, 86);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(460, 84);
			this.dtpVerified.TabIndex = 4;
			this.dtpVerified.Visible = true;
			//
			//txtTitle
			//
			this.txtTitle.Location = new System.Drawing.Point(64, 28);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(512, 23);
			this.txtTitle.TabIndex = 0;
			this.txtTitle.Tag = "Required";
			this.txtTitle.Text = "txtTitle";
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblTitle.Location = new System.Drawing.Point(28, 30);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(37, 16);
			this.lblTitle.TabIndex = 61;
			this.lblTitle.Text = "Title";
			//
			//cbAuthor
			//
			this.cbAuthor.Location = new System.Drawing.Point(64, 56);
			this.cbAuthor.Name = "cbAuthor";
			this.cbAuthor.Size = new System.Drawing.Size(512, 24);
			this.cbAuthor.TabIndex = 1;
			this.cbAuthor.Tag = "Required";
			this.cbAuthor.Text = "cbAuthor";
			//
			//lblAuthor
			//
			this.lblAuthor.AutoSize = true;
			this.lblAuthor.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblAuthor.Location = new System.Drawing.Point(12, 59);
			this.lblAuthor.Name = "lblAuthor";
			this.lblAuthor.Size = new System.Drawing.Size(52, 16);
			this.lblAuthor.TabIndex = 63;
			this.lblAuthor.Text = "Author";
			//
			//txtISBN
			//
			this.txtISBN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtISBN.Location = new System.Drawing.Point(64, 84);
			this.txtISBN.Name = "txtISBN";
			this.txtISBN.Size = new System.Drawing.Size(144, 23);
			this.txtISBN.TabIndex = 2;
			this.txtISBN.Text = "TXTISBN";
			//
			//lblISBN
			//
			this.lblISBN.AutoSize = true;
			this.lblISBN.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblISBN.Location = new System.Drawing.Point(23, 88);
			this.lblISBN.Name = "lblISBN";
			this.lblISBN.Size = new System.Drawing.Size(39, 16);
			this.lblISBN.TabIndex = 65;
			this.lblISBN.Text = "ISBN";
			//
			//cbSubject
			//
			this.cbSubject.Location = new System.Drawing.Point(64, 112);
			this.cbSubject.Name = "cbSubject";
			this.cbSubject.Size = new System.Drawing.Size(302, 24);
			this.cbSubject.TabIndex = 5;
			this.cbSubject.Tag = "Required";
			this.cbSubject.Text = "cbSubject";
			//
			//lblSubject
			//
			this.lblSubject.AutoSize = true;
			this.lblSubject.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblSubject.Location = new System.Drawing.Point(6, 115);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(59, 16);
			this.lblSubject.TabIndex = 67;
			this.lblSubject.Text = "Subject";
			//
			//lblMisc
			//
			this.lblMisc.AutoSize = true;
			this.lblMisc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblMisc.Location = new System.Drawing.Point(28, 145);
			this.lblMisc.Name = "lblMisc";
			this.lblMisc.Size = new System.Drawing.Size(37, 16);
			this.lblMisc.TabIndex = 69;
			this.lblMisc.Text = "Misc";
			//
			//txtMisc
			//
			this.txtMisc.Location = new System.Drawing.Point(64, 143);
			this.txtMisc.Name = "txtMisc";
			this.txtMisc.Size = new System.Drawing.Size(512, 23);
			this.txtMisc.TabIndex = 7;
			this.txtMisc.Text = "txtMisc";
			//
			//chkCataloged
			//
			this.chkCataloged.Location = new System.Drawing.Point(472, 233);
			this.chkCataloged.Name = "chkCataloged";
			this.chkCataloged.Size = new System.Drawing.Size(112, 20);
			this.chkCataloged.TabIndex = 16;
			this.chkCataloged.Text = "Cataloged";
			//
			//lblFormat
			//
			this.lblFormat.AutoSize = true;
			this.lblFormat.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblFormat.Location = new System.Drawing.Point(372, 116);
			this.lblFormat.Name = "lblFormat";
			this.lblFormat.Size = new System.Drawing.Size(54, 16);
			this.lblFormat.TabIndex = 67;
			this.lblFormat.Text = "Format";
			//
			//cbFormat
			//
			this.cbFormat.Location = new System.Drawing.Point(432, 113);
			this.cbFormat.Name = "cbFormat";
			this.cbFormat.Size = new System.Drawing.Size(144, 24);
			this.cbFormat.TabIndex = 6;
			this.cbFormat.Tag = "Required";
			this.cbFormat.Text = "cbFormat";
			//
			//frmBooks
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(772, 450);
			this.MinimumSize = new System.Drawing.Size(684, 484);
			this.Name = "frmBooks";
			this.Text = "frmBooks";
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
		private clsISBN mISBN;
		#region "Methods"
		protected override void BindControls()
		{
			BindControl(txtID, mTCBase.MainDataView, "ID");
			BindControl(txtTitle, mTCBase.MainDataView, "Title");
			BindControl(cbAuthor, mTCBase.MainDataView, "Author", ((clsBooks)mTCBase).Authors, "Author", "Author");
			BindControl(txtISBN, mTCBase.MainDataView, "ISBN");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(cbSubject, mTCBase.MainDataView, "Subject", ((clsBooks)mTCBase).Subjects, "Subject", "Subject");
			BindControl(cbFormat, mTCBase.MainDataView, "Format", ((clsBooks)mTCBase).Formats, "Format", "Format");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsBooks)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			///'BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort");
			//Note that this guy is Simple-Bound...
			BindControl(txtMisc, mTCBase.MainDataView, "Misc");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(chkCataloged, mTCBase.MainDataView, "Cataloged");
			BindControl(pbGeneral, mTCBase.MainDataView, "Image");
			BindControl(pbGeneral2, mTCBase.MainDataView, "OtherImage");
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		private void AlphasortByISBN(System.Windows.Forms.ComboBox cbAlphaSort, string ISBN, string LastName, string Title)
		{
			int CountryCode = Convert.ToInt32(ParseStr(ISBN, 1, "-"));
			//If we begin to get dups on PublisherCode, we'll have to start using the CountryCode, but not until then...
			int PublisherCode = Convert.ToInt32(ParseStr(ISBN, 2, "-"));
			//PublisherCode
			string Value = bpeNullString;
			switch (PublisherCode) {
				case 880588:
				case 874023:
				case 86184:
					//CountryCode: 1
					Value = "AIRTIME: " + Title;
					break;
				case 361:
					//CountryCode: 962
					Value = "CONCORD: <SeriesName> #nnnn; " + Title;
					break;
				case 7603:
				case 87398:
				case 85045:
					//CountryCode: 0
					Value = "MOTORBOOKS: " + Strings.UCase(LastName) + ": " + Title;
					break;
				case 914845:
					//CountryCode: 0
					Value = "MSPRESS: " + Strings.UCase(LastName) + ": " + Title;
					break;
				case 57231:
				case 55615:
					//CountryCode: 1
					Value = "MSPRESS: " + Strings.UCase(LastName) + ": " + Title;
					break;
				case 896522:
					//CountryCode: 1
					Value = "NASA MISSION REPORTS: " + Title;
					break;
				case 87938:
				case 89141:
					//CountryCode: 0
					Value = "OSPREY: " + Title;
					break;
				case 85532:
				case 84176:
					//CountryCode: 1
					Value = "OSPREY: AIRCRAFT OF THE ACES #nn; " + Title;
					Value = "OSPREY: CLASSIC BATTLES; " + Title;
					Value = "OSPREY: COMBAT AIRCRAFT #nn; " + Title;
					Value = "OSPREY: NEW VANGUARD #nn; " + Title;
					Value = "OSPREY: <SeriesName> #nn; " + Title;
					break;
				case 672:
					//CountryCode: 0
					Value = "SAMS: " + Title;
					break;
				case 944055:
					//CountryCode: 0
					Value = "SCALE MODELING: FLOATING DRYDOCK: " + Title;
					break;
				case 89024:
					//CountryCode: 0
					Value = "SCALE MODELING: KALMBACH: " + Title;
					break;
				case 7643:
				case 88740:
					//CountryCode: 0
					Value = "SCHIFFER MILITARY HISTORY: " + LastName + ": " + Title;
					break;
				case 8094:
				case 670:
				case 939526:
					//CountryCode: 0
					Value = "TIMELIFE: <SeriesName>: BOOK ##; " + Title;
					break;
				case 85488:
					//CountryCode: 1
					Value = "TRISERVICE: " + Title;
					break;
				case 935696:
				case 88038:
					//CountryCode: 0
					Value = "TSR: #### " + Title;
					break;
				case 89747:
					//CountryCode: 0
					Value = "SQUADRON/SIGNAL: AIRCRAFT MINI NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: AIRCRAFT NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: ARMOR NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: C&M NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: COMBAT TROOPS NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: D&S NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: FIGHTING COLORS NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: MODERN MILITARY AIRCRAFT NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: ON DECK NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: SPECIALS NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: WALK AROUND (ARMOR) NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: WALK AROUND NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: WARSHIPS NO. ##; " + Title;
					Value = "SQUADRON/SIGNAL: <SeriesName> NO. ##; " + Title;
					break;
				case 87021:
					//CountryCode: 0
					Value = "USNI: " + Title;
					break;
				case 55750:
					//CountryCode: 1
					Value = "USNI: " + Title;
					break;
				case 930607:
					//CountryCode: 1
					Value = "VERLINDEN: LOCK ON NO. ##; " + Title;
					Value = "VERLINDEN: WARMACHINES NO. ##; " + Title;
					Value = "VERLINDEN: <SeriesName> NO. ##; " + Title;
					break;
				case 70932:
					//CountryCode: 90
					Value = "VERLINDEN: LOCK ON NO. ##; " + Title;
					Value = "VERLINDEN: WARMACHINES NO. ##; " + Title;
					Value = "VERLINDEN: <SeriesName> NO. ##; " + Title;
					break;
				case 9654829:
				case 9710687:
				case 9745687:
					//CountryCode: 0
					Value = "WARSHIP PICTORIAL SERIES #nn; " + Title;
					break;
				case 933126:
				case 929521:
					//CountryCode: 0
					Value = "WARSHIP SERIES #n; <ShipDesignation>; U.S.S. <ShipName>";
					break;
				case 57510:
					//CountryCode: 1
					Value = "WARSHIP'S DATA #n; <ShipDesignation>; U.S.S. <ShipName>";
					break;
				default:
					Value = string.Format("{0}: {1}", LastName.ToUpper(), Title.ToUpper());
					break;
			}
			cbAddItem(cbAlphaSort, FixAlphasort(Value));
		}
		public void DefaultAlphaSort(System.Windows.Forms.ComboBox cbAlphaSort, string Title, string Author, string ISBN, string Subject)
		{
			if (cbAlphaSort.Items.Count > 0)
				cbAlphaSort.Items.Clear();
			cbAddItem(cbAlphaSort, cbAlphaSort.Text);

			Title = Title.ToUpper();
			Author = Author.ToUpper();
			ISBN = ISBN.ToUpper();
			Subject = Subject.ToUpper();

			//Start with the Author's last name...
			string LastName = Author;
			int iAnd = Strings.InStr(Author, " AND ");
			int iAmpersand = Strings.InStr(Author, " & ");
			int iComma = Strings.InStr(Author, ",");
			int iSemiColon = Strings.InStr(Author, ";");
			int iSeparator = 0;
			int iWith = Strings.InStr(Author, " WITH ");

			if (iComma > 0) {
				//Assume the comma separates authors, and...
				iSeparator = iComma;
			} else if (iSemiColon > 0) {
				//Assume the semicolon separates authors, and...
				iSeparator = iSemiColon;
			} else if (iAnd > 0) {
				//Assume the "AND" separates authors, and...
				iSeparator = iAnd;
			} else if (iAmpersand > 0) {
				//Assume the "&" separates authors, and...
				iSeparator = iAmpersand;
			} else if (iWith > 0) {
				//Assume the "WITH" separates authors, and...
				iSeparator = iWith;
			}

			//...take the first Author...
			if (iSeparator > 0)
				LastName = LastName.Substring(0, iSeparator - 1).Trim();

			//OK, we have a single person's name (theoretically)...
			//Grab the last word on the line and assume it's his last name (unless it's "(Editor)", etc)...
			if (Strings.InStr(LastName, " ") > 0) {
				if (LastName.EndsWith("(ED.)")) {
					LastName = LastName.Substring(0, LastName.Length - "(ED.)".Length).Trim();
				} else if (LastName.EndsWith("(EDITOR)")) {
					LastName = LastName.Substring(0, LastName.Length - "(EDITOR)".Length).Trim();
				} else if (LastName.EndsWith("(EDITORS)")) {
					LastName = LastName.Substring(0, LastName.Length - "(EDITORS)".Length).Trim();
				}
				iSeparator = Strings.InStrRev(LastName, " ", LastName.Length);
				LastName = LastName.Substring(iSeparator);
			}

			//Check for "The" at the beginning of the title...
			if (Title.StartsWith("THE "))
				Title = Title.Substring("THE ".Length) + ", THE";
			if (Title.StartsWith("A "))
				Title = Title.Substring("A ".Length) + ", A";
			if (Title.StartsWith("AN "))
				Title = Title.Substring("AN ".Length) + ", AN";

			//Check ISBN to detect publishers of special series that we'd rather sort by series name than author(s)...
			if (ISBN.Trim() != bpeNullString && ISBN.Length >= 9 && Information.IsNumeric(ISBN.Replace("-", bpeNullString).Replace(" ", bpeNullString)))
				AlphasortByISBN(cbAlphaSort, ISBN, LastName, Title);
			switch (Subject) {
				case "SCIENCE FICTION; STAR TREK":
					cbAddItem(cbAlphaSort, FixAlphasort("STAR TREK: " + LastName + ": " + Title));
					cbAddItem(cbAlphaSort, FixAlphasort("STAR TREK: <Series>: BOOK ##"));
					break;
				case "SCIENCE FICTION; STAR WARS":
					cbAddItem(cbAlphaSort, FixAlphasort("STAR WARS: " + LastName + ": " + Title));
					cbAddItem(cbAlphaSort, FixAlphasort("STAR WARS: <Series>: BOOK ##"));
					break;
				default:
					cbAddItem(cbAlphaSort, FixAlphasort(LastName + ": " + Title));
					cbAddItem(cbAlphaSort, FixAlphasort(LastName + ": <Series>: BOOK ##"));
					cbAddItem(cbAlphaSort, FixAlphasort(LastName + "(##): " + Title));
					break;
			}
		}
		private string FixAlphasort(string Value)
		{
			int MaxLength = ((clsBooks)mTCBase).AlphasortMaxLength;
			if (Value.Length <= MaxLength)
				return Value;
			if (TokenCount(Value, ":") > 1) {
				while (Value.Length > MaxLength) {
					Value = Value.Substring(0, Value.LastIndexOf(":"));
				}
				return Value;
			} else {
				return Value.Substring(0, MaxLength);
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
						//: lblInventoried.Visible = False
						txtPrice.Visible = false;
						lblPrice.Visible = false;
						dtpPurchased.Visible = false;
						lblPurchased.Visible = false;
						chkCataloged.CheckState = CheckState.Unchecked;
						chkCataloged.Visible = false;
						if (mTCBase.Mode != clsTCBase.ActionModeEnum.modeDisplay) {
                            mTCBase.CurrentRow["Location"] = DBNull.Value;
                            mTCBase.CurrentRow["DateInventoried"] = DBNull.Value;
                            mTCBase.CurrentRow["Price"] = DBNull.Value;
                            mTCBase.CurrentRow["DatePurchased"] = DBNull.Value;
                            mTCBase.CurrentRow["Cataloged"] = 0;
						}
						break;
					case CheckState.Indeterminate:
						break;
					case CheckState.Unchecked:
						//Show applicable controls, and adjust screen size accordingly...
						cbLocation.Visible = true;
						lblLocation.Visible = true;
						dtpInventoried.Visible = true;
						//: lblInventoried.Visible = True
						txtPrice.Visible = true;
						lblPrice.Visible = true;
						dtpPurchased.Visible = true;
						lblPurchased.Visible = true;
						chkCataloged.Visible = true;
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
				System.Windows.Forms.ComboBox cb = (ComboBox)sender;
				//Give the user options...
				DefaultAlphaSort(cb, this.txtTitle.Text, this.cbAuthor.Text, this.txtISBN.Text, this.cbSubject.Text);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmBooks.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				cbAlphaSort.Validating -= SetCaption;
				cbAlphaSort.Enter -= DefaultAlphaSort;
				RemoveControlHandlers(cbAuthor);
				RemoveControlHandlers(cbSubject);
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
				RemoveControlHandlers(txtTitle);
				txtISBN.Validating -= txtISBN_Validating;
				RemoveControlHandlers(txtMisc);
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
			const string EntryName = "frmBooks.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating += SetCaption;
				cbAlphaSort.Enter += DefaultAlphaSort;
				SetupControlHandlers(cbAuthor);
				SetupControlHandlers(cbSubject);
				SetupControlHandlers(cbFormat);
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
				SetupControlHandlers(txtTitle);
				txtISBN.Validating += txtISBN_Validating;
				SetupControlHandlers(txtMisc);
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
				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["AlphaSort"]) ? bpeNullString : (string)mTCBase.CurrentRow["AlphaSort"]);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				if ((sender != null))
					base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		private void txtISBN_Validating(object sender, CancelEventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				txtISBN.Text = txtISBN.Text.Trim();
				if (txtISBN.Text == bpeNullString) {
					if (!chkWishList.Checked)
						base.epBase.SetError((Control)sender, "ISBN should be specified!");
					txtISBN.Text = "UNKNOWN";
				} else {
                    //txtISBN.Text = objISBN.FormatISBN(txtISBN.Text)
                    char cStr = txtISBN.Text.Substring(0, 1).ToCharArray()[0];
                    if (cStr == 'M' || (cStr >= '0' && cStr <= '9')) {
							//Some forms of ISBN (EAN) may begin with "M"...
							txtISBN.Text = mISBN.CheckISBN(txtISBN.Text);
					}
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		#endregion
	}
}
