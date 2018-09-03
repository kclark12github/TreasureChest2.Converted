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
//frmSoftware.vb
//   Software Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/23/17    Added logic to set WishList hidden fields to NULL;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   10/06/16    Attempting to track down issues clearing controls on Add operations;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   05/14/11    Added AlphaSort & OtherImage;
//   10/03/10    Rearranged screen to accommodate new fields;
//   01/17/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCSoftware
{
	public class frmSoftware : frmTCStandard
	{
		const string myFormName = "frmSoftware";
		public frmSoftware(clsSupport objSupport, clsSoftware objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmSoftware() : base()
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
		internal System.Windows.Forms.Label lblTitle;
		internal System.Windows.Forms.TextBox txtTitle;
		internal System.Windows.Forms.Label lblPublisher;
		internal System.Windows.Forms.ComboBox cbPublisher;
		internal System.Windows.Forms.Label lblVersion;
		internal System.Windows.Forms.TextBox txtVersion;
		internal System.Windows.Forms.Label lblISBN;
		internal System.Windows.Forms.TextBox txtISBN;
		internal System.Windows.Forms.CheckBox chkCataloged;
		internal System.Windows.Forms.Label lblPlatform;
		internal System.Windows.Forms.ComboBox cbPlatform;
		internal System.Windows.Forms.Label lblMedia;
		internal System.Windows.Forms.ComboBox cbMedia;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.ComboBox cbType;
		internal System.Windows.Forms.Label lblCDKey;
		internal System.Windows.Forms.TextBox txtCDKey;
		internal System.Windows.Forms.Label lblDeveloper;
		internal System.Windows.Forms.ComboBox cbDeveloper;
		internal System.Windows.Forms.Label lblReleaseDate;
		internal System.Windows.Forms.DateTimePicker dtpReleaseDate;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblPublisher = new System.Windows.Forms.Label();
			this.cbPublisher = new System.Windows.Forms.ComboBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.lblISBN = new System.Windows.Forms.Label();
			this.txtISBN = new System.Windows.Forms.TextBox();
			this.chkCataloged = new System.Windows.Forms.CheckBox();
			this.lblPlatform = new System.Windows.Forms.Label();
			this.cbPlatform = new System.Windows.Forms.ComboBox();
			this.lblMedia = new System.Windows.Forms.Label();
			this.cbMedia = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.lblCDKey = new System.Windows.Forms.Label();
			this.txtCDKey = new System.Windows.Forms.TextBox();
			this.lblDeveloper = new System.Windows.Forms.Label();
			this.cbDeveloper = new System.Windows.Forms.ComboBox();
			this.lblReleaseDate = new System.Windows.Forms.Label();
			this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			this.tpNotes.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.gbGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.SuspendLayout();
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 516);
			this.txtID.Name = "txtID";
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(304, 252);
			this.dtpPurchased.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 15;
			this.ttBase.SetToolTip(this.dtpPurchased, "Date this item was purchased");
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkWishList.Location = new System.Drawing.Point(224, 195);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 10;
			this.ttBase.SetToolTip(this.chkWishList, "Is this a WishList item?");
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.dtpInventoried.Location = new System.Drawing.Point(508, 356);
			this.dtpInventoried.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 19;
			this.ttBase.SetToolTip(this.dtpInventoried, "Date this item was last inventoried (i.e. location confirmed)");
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 515);
			this.lblID.Name = "lblID";
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(220, 254);
			this.lblPurchased.Name = "lblPurchased";
			//
			//sbpTime
			//
			this.sbpTime.Text = "11:37 PM";
			this.sbpTime.Width = 79;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 476;
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
			this.btnPrev.Location = new System.Drawing.Point(36, 476);
			this.btnPrev.Name = "btnPrev";
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(616, 476);
			this.btnNext.Name = "btnNext";
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 476);
			this.btnFirst.Name = "btnFirst";
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(643, 476);
			this.btnLast.Name = "btnLast";
			//
			//cbLocation
			//
			this.cbLocation.Location = new System.Drawing.Point(88, 356);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(400, 24);
			this.cbLocation.TabIndex = 18;
			this.cbLocation.Tag = "Required";
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(304, 224);
			this.dtpVerified.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
			this.dtpVerified.Name = "dtpVerified";
			this.dtpVerified.TabIndex = 13;
			this.dtpVerified.Visible = true;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(636, 312);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.Visible = false;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(236, 226);
			this.lblVerified.Name = "lblVerified";
			this.lblVerified.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(88, 224);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(104, 23);
			this.txtValue.TabIndex = 12;
			this.txtValue.Tag = "Money,Nulls,Required";
			this.txtValue.Visible = true;
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(512, 512);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 19;
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(40, 226);
			this.lblValue.Name = "lblValue";
			this.lblValue.Visible = true;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 548);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(593, 512);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 20;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(125, 268);
			//
			//sbpMode
			//
			this.sbpMode.Text = "";
			this.sbpMode.Width = 10;
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(500, 304);
			this.hsbGeneral.Name = "hsbGeneral";
			this.hsbGeneral.Size = new System.Drawing.Size(125, 17);
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(88, 356);
			this.txtAlphaSort.Name = "txtAlphaSort";
			this.txtAlphaSort.Size = new System.Drawing.Size(392, 23);
			//
			//lblLocation
			//
			this.lblLocation.Location = new System.Drawing.Point(12, 360);
			this.lblLocation.Name = "lblLocation";
			//
			//sbpFilter
			//
			this.sbpFilter.Text = "";
			this.sbpFilter.Width = 10;
			//
			//pbGeneral
			//
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(125, 268);
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Location = new System.Drawing.Point(8, 303);
			this.lblAlphaSort.Name = "lblAlphaSort";
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(593, 512);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 21;
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(656, 270);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 476);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(548, 24);
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(652, 403);
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(648, 348);
			this.rtfNotes.TabIndex = 18;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(88, 252);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.Size = new System.Drawing.Size(104, 23);
			this.txtPrice.TabIndex = 14;
			this.txtPrice.Tag = "Money,Nulls,Required";
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(88, 300);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(392, 24);
			this.cbAlphaSort.TabIndex = 16;
			this.cbAlphaSort.Tag = "Required,UPPER";
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(44, 252);
			this.lblPrice.Name = "lblPrice";
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.lblReleaseDate);
			this.gbGeneral.Controls.Add(this.dtpReleaseDate);
			this.gbGeneral.Controls.Add(this.lblDeveloper);
			this.gbGeneral.Controls.Add(this.cbDeveloper);
			this.gbGeneral.Controls.Add(this.lblCDKey);
			this.gbGeneral.Controls.Add(this.txtCDKey);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Controls.Add(this.lblMedia);
			this.gbGeneral.Controls.Add(this.cbMedia);
			this.gbGeneral.Controls.Add(this.lblPlatform);
			this.gbGeneral.Controls.Add(this.cbPlatform);
			this.gbGeneral.Controls.Add(this.chkCataloged);
			this.gbGeneral.Controls.Add(this.lblISBN);
			this.gbGeneral.Controls.Add(this.txtISBN);
			this.gbGeneral.Controls.Add(this.lblVersion);
			this.gbGeneral.Controls.Add(this.txtVersion);
			this.gbGeneral.Controls.Add(this.lblPublisher);
			this.gbGeneral.Controls.Add(this.cbPublisher);
			this.gbGeneral.Controls.Add(this.lblTitle);
			this.gbGeneral.Controls.Add(this.txtTitle);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(640, 388);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbPublisher, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPublisher, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtVersion, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVersion, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtISBN, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblISBN, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkCataloged, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbPlatform, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPlatform, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbMedia, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblMedia, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtCDKey, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCDKey, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbDeveloper, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDeveloper, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblReleaseDate, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			//
			//tcMain
			//
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(660, 432);
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(48, 30);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(34, 19);
			this.lblTitle.TabIndex = 95;
			this.lblTitle.Text = "Title";
			//
			//txtTitle
			//
			this.txtTitle.Location = new System.Drawing.Point(88, 28);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(392, 23);
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Tag = "Required";
			this.txtTitle.Text = "txtTitle";
			//
			//lblPublisher
			//
			this.lblPublisher.AutoSize = true;
			this.lblPublisher.Location = new System.Drawing.Point(16, 144);
			this.lblPublisher.Name = "lblPublisher";
			this.lblPublisher.Size = new System.Drawing.Size(66, 19);
			this.lblPublisher.TabIndex = 97;
			this.lblPublisher.Text = "Publisher";
			//
			//cbPublisher
			//
			this.cbPublisher.Location = new System.Drawing.Point(88, 140);
			this.cbPublisher.Name = "cbPublisher";
			this.cbPublisher.Size = new System.Drawing.Size(392, 24);
			this.cbPublisher.TabIndex = 6;
			this.cbPublisher.Tag = "Required";
			this.cbPublisher.Text = "cbPublisher";
			//
			//lblVersion
			//
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(28, 86);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(55, 19);
			this.lblVersion.TabIndex = 99;
			this.lblVersion.Text = "Version";
			//
			//txtVersion
			//
			this.txtVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtVersion.Location = new System.Drawing.Point(88, 84);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.Size = new System.Drawing.Size(104, 23);
			this.txtVersion.TabIndex = 3;
			this.txtVersion.Tag = "";
			this.txtVersion.Text = "TXTVERSION";
			//
			//lblISBN
			//
			this.lblISBN.AutoSize = true;
			this.lblISBN.Location = new System.Drawing.Point(40, 170);
			this.lblISBN.Name = "lblISBN";
			this.lblISBN.Size = new System.Drawing.Size(39, 19);
			this.lblISBN.TabIndex = 101;
			this.lblISBN.Text = "ISBN";
			//
			//txtISBN
			//
			this.txtISBN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtISBN.Location = new System.Drawing.Point(88, 168);
			this.txtISBN.Name = "txtISBN";
			this.txtISBN.Size = new System.Drawing.Size(164, 23);
			this.txtISBN.TabIndex = 7;
			this.txtISBN.Text = "TXTISBN";
			//
			//chkCataloged
			//
			this.chkCataloged.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkCataloged.Location = new System.Drawing.Point(360, 195);
			this.chkCataloged.Name = "chkCataloged";
			this.chkCataloged.TabIndex = 11;
			this.chkCataloged.Text = "Cataloged";
			//
			//lblPlatform
			//
			this.lblPlatform.AutoSize = true;
			this.lblPlatform.Location = new System.Drawing.Point(22, 115);
			this.lblPlatform.Name = "lblPlatform";
			this.lblPlatform.Size = new System.Drawing.Size(61, 19);
			this.lblPlatform.TabIndex = 106;
			this.lblPlatform.Text = "Platform";
			//
			//cbPlatform
			//
			this.cbPlatform.Location = new System.Drawing.Point(88, 112);
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(392, 24);
			this.cbPlatform.TabIndex = 5;
			this.cbPlatform.Tag = "Required";
			this.cbPlatform.Text = "cbPlatform";
			//
			//lblMedia
			//
			this.lblMedia.AutoSize = true;
			this.lblMedia.Location = new System.Drawing.Point(308, 170);
			this.lblMedia.Name = "lblMedia";
			this.lblMedia.Size = new System.Drawing.Size(44, 19);
			this.lblMedia.TabIndex = 108;
			this.lblMedia.Text = "Media";
			//
			//cbMedia
			//
			this.cbMedia.Location = new System.Drawing.Point(356, 167);
			this.cbMedia.Name = "cbMedia";
			this.cbMedia.Size = new System.Drawing.Size(124, 24);
			this.cbMedia.TabIndex = 8;
			this.cbMedia.Tag = "Required";
			this.cbMedia.Text = "cbMedia";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(240, 86);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 19);
			this.lblType.TabIndex = 110;
			this.lblType.Text = "Type";
			//
			//cbType
			//
			this.cbType.Location = new System.Drawing.Point(288, 83);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(164, 24);
			this.cbType.TabIndex = 4;
			this.cbType.Tag = "Required";
			this.cbType.Text = "cbType";
			//
			//lblCDKey
			//
			this.lblCDKey.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblCDKey.AutoSize = true;
			this.lblCDKey.Location = new System.Drawing.Point(24, 330);
			this.lblCDKey.Name = "lblCDKey";
			this.lblCDKey.Size = new System.Drawing.Size(54, 19);
			this.lblCDKey.TabIndex = 112;
			this.lblCDKey.Text = "CD Key";
			//
			//txtCDKey
			//
			this.txtCDKey.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtCDKey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtCDKey.Location = new System.Drawing.Point(88, 328);
			this.txtCDKey.Name = "txtCDKey";
			this.txtCDKey.Size = new System.Drawing.Size(536, 23);
			this.txtCDKey.TabIndex = 17;
			this.txtCDKey.Text = "TXTCDKEY";
			//
			//lblDeveloper
			//
			this.lblDeveloper.AutoSize = true;
			this.lblDeveloper.Location = new System.Drawing.Point(9, 60);
			this.lblDeveloper.Name = "lblDeveloper";
			this.lblDeveloper.Size = new System.Drawing.Size(73, 19);
			this.lblDeveloper.TabIndex = 114;
			this.lblDeveloper.Text = "Developer";
			//
			//cbDeveloper
			//
			this.cbDeveloper.Location = new System.Drawing.Point(88, 56);
			this.cbDeveloper.Name = "cbDeveloper";
			this.cbDeveloper.Size = new System.Drawing.Size(392, 24);
			this.cbDeveloper.TabIndex = 2;
			this.cbDeveloper.Tag = "Required";
			this.cbDeveloper.Text = "cbDeveloper";
			//
			//lblReleaseDate
			//
			this.lblReleaseDate.AutoSize = true;
			this.lblReleaseDate.Location = new System.Drawing.Point(14, 200);
			this.lblReleaseDate.Name = "lblReleaseDate";
			this.lblReleaseDate.Size = new System.Drawing.Size(65, 19);
			this.lblReleaseDate.TabIndex = 116;
			this.lblReleaseDate.Text = "Released";
			//
			//dtpReleaseDate
			//
			this.dtpReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpReleaseDate.Location = new System.Drawing.Point(88, 196);
			this.dtpReleaseDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
			this.dtpReleaseDate.Name = "dtpReleaseDate";
			this.dtpReleaseDate.Size = new System.Drawing.Size(116, 23);
			this.dtpReleaseDate.TabIndex = 9;
			this.dtpReleaseDate.Tag = "Nulls";
			//
			//frmSoftware
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 570);
			this.MinimumSize = new System.Drawing.Size(684, 604);
			this.Name = "frmSoftware";
			this.Text = "frmSoftware";
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			this.tpNotes.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.gbGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
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
			BindControl(txtTitle, mTCBase.MainDataView, "Title");
			BindControl(cbDeveloper, mTCBase.MainDataView, "Developer", ((clsSoftware)mTCBase).Developers, "Developer", "Developer");
			BindControl(cbPublisher, mTCBase.MainDataView, "Publisher", ((clsSoftware)mTCBase).Publishers, "Publisher", "Publisher");
			BindControl(txtVersion, mTCBase.MainDataView, "Version");
			BindControl(txtISBN, mTCBase.MainDataView, "ISBN");
			BindControl(chkCataloged, mTCBase.MainDataView, "Cataloged");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(dtpReleaseDate, mTCBase.MainDataView, "DateReleased");
			BindControl(cbPlatform, mTCBase.MainDataView, "Platform", ((clsSoftware)mTCBase).Platforms, "Platform", "Platform");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsSoftware)mTCBase).Types, "Type", "Type");
			BindControl(cbMedia, mTCBase.MainDataView, "Media", ((clsSoftware)mTCBase).Media, "Media", "Media");
			BindControl(txtCDKey, mTCBase.MainDataView, "CDkey");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsSoftware)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort");
			//Note that this guy is Simple-Bound...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		public void DefaultAlphaSort(System.Windows.Forms.ComboBox cbAlphaSort, string SoftwareType, string Publisher, string Developer, string Title, string YearReleased, string Media, string Platform)
		{
			SoftwareType = SoftwareType.ToUpper();
			Publisher = Publisher.ToUpper();
			Developer = Developer.ToUpper();
			Title = Title.ToUpper();
			Media = Media.ToUpper();
			Platform = Platform.ToUpper();

			if (cbAlphaSort.Items.Count > 0)
				cbAlphaSort.Items.Clear();
			cbAddItem(cbAlphaSort, cbAlphaSort.Text);

			cbAddItem(cbAlphaSort, string.Format("{0}: {1}", SoftwareType, Title));
			if (Title.StartsWith("THE "))
				cbAddItem(cbAlphaSort, string.Format("{0}: {1}", SoftwareType, Title.Substring(4) + ", THE"));

			cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", SoftwareType, Title, YearReleased));
			if (Title.StartsWith("THE "))
				cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", SoftwareType, Title.Substring(4) + ", THE", YearReleased));

			switch (SoftwareType) {
				case "GAME":
					cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", new string[] {
						SoftwareType,
						Media,
						Title
					}));
					if (Title.StartsWith("THE "))
						cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", new string[] {
							SoftwareType,
							Media,
							Title.Substring(4) + ", THE"
						}));

					cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", new string[] {
						SoftwareType,
						Platform,
						Title
					}));
					if (Title.StartsWith("THE "))
						cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", new string[] {
							SoftwareType,
							Platform,
							Title.Substring(4) + ", THE"
						}));
					break;
				default:
					break;
			}

			cbAddItem(cbAlphaSort, string.Format("{0}: {1}/{2}; {3}", new string[] {
				SoftwareType,
				Publisher,
				Developer,
				Title
			}));
			if (Title.StartsWith("THE "))
				cbAddItem(cbAlphaSort, string.Format("{0}: {1}/{2}; {3}", new string[] {
					SoftwareType,
					Publisher,
					Developer,
					Title.Substring(4) + ", THE"
				}));
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
						chkCataloged.Visible = false;
						chkCataloged.CheckState = CheckState.Unchecked;
						txtCDKey.Visible = false;
						lblCDKey.Visible = false;
						if (mTCBase.Mode != clsTCBase.ActionModeEnum.modeDisplay) {
                            mTCBase.CurrentRow["Location"] = DBNull.Value;
                            mTCBase.CurrentRow["DateInventoried"] = DBNull.Value;
                            mTCBase.CurrentRow["Price"] = DBNull.Value;
                            mTCBase.CurrentRow["DatePurchased"] = DBNull.Value;
                            mTCBase.CurrentRow["Cataloged"] = 0;
                            mTCBase.CurrentRow["CDKey"] = DBNull.Value;
						}
						break;
					case CheckState.Indeterminate:
						break;
					case CheckState.Unchecked:
						//Show applicable controls, and adjust screen size accordingly...
						cbLocation.Visible = true;
						lblLocation.Visible = true;
						dtpInventoried.Visible = true;
						//: lblInventoried.Visible = true
						txtPrice.Visible = true;
						lblPrice.Visible = true;
						dtpPurchased.Visible = true;
						lblPurchased.Visible = true;
						chkCataloged.Visible = true;
						txtCDKey.Visible = true;
						lblCDKey.Visible = true;
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
				DefaultAlphaSort(cb, this.cbType.Text, this.cbPublisher.Text, this.cbDeveloper.Text, this.txtTitle.Text, this.dtpReleaseDate.Value.Year.ToString(), this.cbMedia.Text, this.cbPlatform.Text);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmSoftware.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating -= SetCaption;
				cbAlphaSort.Enter -= DefaultAlphaSort;
				RemoveControlHandlers(txtTitle);
				txtTitle.Validating -= SetCaption;
				RemoveControlHandlers(txtISBN);
				txtISBN.Validating -= DefaultTextBox;
				RemoveControlHandlers(txtVersion);
				txtVersion.Validating -= DefaultTextBox;
				RemoveControlHandlers(txtCDKey);
				RemoveControlHandlers(cbDeveloper);
				RemoveControlHandlers(cbPublisher);
				RemoveControlHandlers(cbMedia);
				RemoveControlHandlers(cbType);
				cbType.Validating -= SetCaption;
				RemoveControlHandlers(cbPlatform);
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
			const string EntryName = "frmSoftware.Form_Load";
			if (DesignMode)
				return;
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating += SetCaption;
				cbAlphaSort.Enter += DefaultAlphaSort;
				SetupControlHandlers(txtTitle);
				txtTitle.Validating += SetCaption;
				SetupControlHandlers(txtISBN);
				txtISBN.Validating += DefaultTextBox;
				SetupControlHandlers(txtVersion);
				txtVersion.Validating += DefaultTextBox;
				SetupControlHandlers(txtCDKey);
				SetupControlHandlers(cbDeveloper);
				SetupControlHandlers(cbPublisher);
				SetupControlHandlers(cbMedia);
				SetupControlHandlers(cbType);
				cbType.Validating += SetCaption;
				SetupControlHandlers(cbPlatform);
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
				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["AlphaSort"]) ? bpeNullString : (string)mTCBase.CurrentRow["AlphaSort"]);
				//Me.txtCaption.Text = String.Format("{0}; {1}", mTCBase.CurrentRow("Type"), mTCBase.CurrentRow("Title")).ToUpper
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
