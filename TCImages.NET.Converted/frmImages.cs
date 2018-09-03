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
//frmImages.vb
//   Images Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   08/06/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCImages
{
	public class frmImages : frmTCStandard
	{
		const string myFormName = "frmImages";
		public frmImages(clsSupport objSupport, clsImages objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
		{
			ImageImported += Form_ImageImported;
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			this.MinimumSize = new Size(this.Width, this.Height);

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
		public frmImages() : base()
		{
			ImageImported += Form_ImageImported;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
			//Add any initialization after the InitializeComponent() call
			this.MinimumSize = new Size(this.Width, this.Height);
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
		internal System.Windows.Forms.TextBox txtFileName;
		internal System.Windows.Forms.ComboBox cbCategory;
		internal System.Windows.Forms.Label lblCategory;
		internal System.Windows.Forms.Label lblURL;
		internal System.Windows.Forms.Label lblFileName;
		internal System.Windows.Forms.TextBox txtHeight;
		internal System.Windows.Forms.Label lblX;
		internal System.Windows.Forms.TextBox txtWidth;
		internal System.Windows.Forms.Label lblPixels;
		internal System.Windows.Forms.CheckBox chkThumbnail;
		internal System.Windows.Forms.GroupBox gbRelatedInfo;
		internal System.Windows.Forms.ComboBox cbThumbnailImage;
		internal System.Windows.Forms.Label lblThumbnailImage;
		internal System.Windows.Forms.ComboBox cbTable;
		internal System.Windows.Forms.Label lblTable;
		internal System.Windows.Forms.Label lblRecord;
		internal System.Windows.Forms.GroupBox gbCaption;
		internal System.Windows.Forms.TextBox txtURL;
		internal System.Windows.Forms.ComboBox cbRecord;
		internal System.Windows.Forms.RichTextBox rtfCaption;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.tpImage = new System.Windows.Forms.TabPage();
			this.cbCategory = new System.Windows.Forms.ComboBox();
			this.lblCategory = new System.Windows.Forms.Label();
			this.lblURL = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lblFileName = new System.Windows.Forms.Label();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.lblX = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.lblPixels = new System.Windows.Forms.Label();
			this.chkThumbnail = new System.Windows.Forms.CheckBox();
			this.gbRelatedInfo = new System.Windows.Forms.GroupBox();
			this.cbThumbnailImage = new System.Windows.Forms.ComboBox();
			this.lblThumbnailImage = new System.Windows.Forms.Label();
			this.cbTable = new System.Windows.Forms.ComboBox();
			this.lblTable = new System.Windows.Forms.Label();
			this.cbRecord = new System.Windows.Forms.ComboBox();
			this.lblRecord = new System.Windows.Forms.Label();
			this.gbCaption = new System.Windows.Forms.GroupBox();
			this.rtfCaption = new System.Windows.Forms.RichTextBox();
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
			this.gbRelatedInfo.SuspendLayout();
			this.gbCaption.SuspendLayout();
			this.SuspendLayout();
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(639, 487);
			this.btnLast.TabIndex = 4;
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 487);
			this.btnFirst.TabIndex = 0;
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(612, 487);
			this.btnNext.TabIndex = 3;
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 487);
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
			this.sbpMessage.Width = 474;
			//
			//sbpTime
			//
			this.sbpTime.Text = "10:09 AM";
			this.sbpTime.Width = 80;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 526);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 212);
			this.lblPurchased.TabIndex = 13;
			this.lblPurchased.Visible = false;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 212);
			this.lblInventoried.TabIndex = 15;
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 216);
			this.lblLocation.TabIndex = 7;
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(29, 132);
			this.lblAlphaSort.TabIndex = 13;
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 216);
			this.lblPrice.TabIndex = 9;
			this.lblPrice.Visible = false;
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(508, 523);
			this.btnOK.TabIndex = 20;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(589, 523);
			this.btnExit.TabIndex = 21;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(589, 523);
			this.btnCancel.TabIndex = 22;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 556);
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
			this.chkWishList.Location = new System.Drawing.Point(484, 212);
			this.chkWishList.TabIndex = 17;
			this.chkWishList.Visible = false;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 212);
			this.dtpPurchased.TabIndex = 14;
			this.dtpPurchased.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 212);
			this.dtpInventoried.TabIndex = 16;
			this.dtpInventoried.Visible = false;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(92, 212);
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.TabIndex = 8;
			this.cbLocation.Visible = false;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(108, 212);
			this.txtAlphaSort.Size = new System.Drawing.Size(512, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral.Visible = false;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 487);
			this.txtCaption.Size = new System.Drawing.Size(544, 23);
			this.txtCaption.TabIndex = 2;
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 527);
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(636, 233);
			this.ttBase.SetToolTip(this.rtfNotes, "Description of Class");
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 212);
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Visible = false;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(108, 132);
			this.cbAlphaSort.Size = new System.Drawing.Size(516, 24);
			this.cbAlphaSort.TabIndex = 14;
			this.ttBase.SetToolTip(this.cbAlphaSort, "AlphaSort");
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.lblPixels);
			this.gbGeneral.Controls.Add(this.txtWidth);
			this.gbGeneral.Controls.Add(this.lblX);
			this.gbGeneral.Controls.Add(this.txtHeight);
			this.gbGeneral.Controls.Add(this.lblFileName);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblURL);
			this.gbGeneral.Controls.Add(this.txtURL);
			this.gbGeneral.Controls.Add(this.lblCategory);
			this.gbGeneral.Controls.Add(this.txtFileName);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.gbCaption);
			this.gbGeneral.Controls.Add(this.gbRelatedInfo);
			this.gbGeneral.Controls.Add(this.chkThumbnail);
			this.gbGeneral.Controls.Add(this.cbCategory);
			this.gbGeneral.Size = new System.Drawing.Size(636, 403);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
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
			this.gbGeneral.Controls.SetChildIndex(this.cbCategory, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkThumbnail, 0);
			this.gbGeneral.Controls.SetChildIndex(this.gbRelatedInfo, 0);
			this.gbGeneral.Controls.SetChildIndex(this.gbCaption, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtFileName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCategory, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtURL, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblURL, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblFileName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtHeight, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblX, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtWidth, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPixels, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpImage);
			this.tcMain.Size = new System.Drawing.Size(656, 443);
			this.tcMain.Controls.SetChildIndex(this.tpImage, 0);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(648, 414);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(656, 270);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.hsbGeneral.Location = new System.Drawing.Point(616, 214);
			this.hsbGeneral.Size = new System.Drawing.Size(124, 17);
			this.hsbGeneral.Visible = false;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral2.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral2.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral2.Visible = false;
			//
			//dtpVerified
			//
			this.dtpVerified.Location = new System.Drawing.Point(176, 211);
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
			this.ttBase.SetToolTip(this.txtName, "Image Name");
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(56, 22);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 16);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			//
			//txtFileName
			//
			this.txtFileName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtFileName.Location = new System.Drawing.Point(108, 48);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(228, 23);
			this.txtFileName.TabIndex = 3;
			this.txtFileName.TabStop = false;
			this.txtFileName.Tag = "";
			this.txtFileName.Text = "txtFileName";
			this.ttBase.SetToolTip(this.txtFileName, "Filename of Image");
			//
			//tpImage
			//
			this.tpImage.Location = new System.Drawing.Point(4, 25);
			this.tpImage.Name = "tpImage";
			this.tpImage.Size = new System.Drawing.Size(656, 270);
			this.tpImage.TabIndex = 5;
			this.tpImage.Text = "Image";
			//
			//cbCategory
			//
			this.cbCategory.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbCategory.Location = new System.Drawing.Point(108, 104);
			this.cbCategory.Name = "cbCategory";
			this.cbCategory.Size = new System.Drawing.Size(264, 24);
			this.cbCategory.TabIndex = 11;
			this.cbCategory.Tag = "Required";
			this.cbCategory.Text = "cbCategory";
			this.ttBase.SetToolTip(this.cbCategory, "Image Category");
			//
			//lblCategory
			//
			this.lblCategory.AutoSize = true;
			this.lblCategory.Location = new System.Drawing.Point(34, 107);
			this.lblCategory.Name = "lblCategory";
			this.lblCategory.Size = new System.Drawing.Size(68, 16);
			this.lblCategory.TabIndex = 10;
			this.lblCategory.Text = "Category";
			//
			//lblURL
			//
			this.lblURL.AutoSize = true;
			this.lblURL.Location = new System.Drawing.Point(69, 79);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(32, 16);
			this.lblURL.TabIndex = 8;
			this.lblURL.Text = "URL";
			//
			//txtURL
			//
			this.txtURL.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtURL.Location = new System.Drawing.Point(108, 77);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(496, 23);
			this.txtURL.TabIndex = 9;
			this.txtURL.Tag = "";
			this.txtURL.Text = "txtURL";
			this.ttBase.SetToolTip(this.txtURL, "Source URL of image");
			//
			//lblFileName
			//
			this.lblFileName.AutoSize = true;
			this.lblFileName.Location = new System.Drawing.Point(28, 50);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(71, 16);
			this.lblFileName.TabIndex = 2;
			this.lblFileName.Text = "File Name";
			//
			//txtHeight
			//
			this.txtHeight.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.txtHeight.BackColor = System.Drawing.SystemColors.Control;
			this.txtHeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtHeight.Enabled = false;
			this.txtHeight.Location = new System.Drawing.Point(432, 50);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(49, 16);
			this.txtHeight.TabIndex = 4;
			this.txtHeight.TabStop = false;
			this.txtHeight.Text = "Height";
			this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.ttBase.SetToolTip(this.txtHeight, "Image height in pixels");
			//
			//lblX
			//
			this.lblX.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lblX.AutoSize = true;
			this.lblX.Location = new System.Drawing.Point(412, 50);
			this.lblX.Name = "lblX";
			this.lblX.Size = new System.Drawing.Size(15, 16);
			this.lblX.TabIndex = 5;
			this.lblX.Text = "x";
			//
			//txtWidth
			//
			this.txtWidth.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.txtWidth.BackColor = System.Drawing.SystemColors.Control;
			this.txtWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtWidth.Enabled = false;
			this.txtWidth.Location = new System.Drawing.Point(356, 50);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.Size = new System.Drawing.Size(44, 16);
			this.txtWidth.TabIndex = 6;
			this.txtWidth.TabStop = false;
			this.txtWidth.Text = "Width";
			this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ttBase.SetToolTip(this.txtWidth, "Image width in pixels");
			//
			//lblPixels
			//
			this.lblPixels.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lblPixels.AutoSize = true;
			this.lblPixels.Location = new System.Drawing.Point(480, 50);
			this.lblPixels.Name = "lblPixels";
			this.lblPixels.Size = new System.Drawing.Size(116, 16);
			this.lblPixels.TabIndex = 7;
			this.lblPixels.Text = "(W x H in pixels)";
			//
			//chkThumbnail
			//
			this.chkThumbnail.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.chkThumbnail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkThumbnail.Location = new System.Drawing.Point(396, 104);
			this.chkThumbnail.Name = "chkThumbnail";
			this.chkThumbnail.Size = new System.Drawing.Size(104, 24);
			this.chkThumbnail.TabIndex = 12;
			this.chkThumbnail.Text = "Thumbnail?";
			this.ttBase.SetToolTip(this.chkThumbnail, "Is this image represent a thumbnail (tiny representation of the real image)?");
			//
			//gbRelatedInfo
			//
			this.gbRelatedInfo.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbRelatedInfo.Controls.Add(this.cbThumbnailImage);
			this.gbRelatedInfo.Controls.Add(this.lblThumbnailImage);
			this.gbRelatedInfo.Controls.Add(this.cbTable);
			this.gbRelatedInfo.Controls.Add(this.lblTable);
			this.gbRelatedInfo.Controls.Add(this.cbRecord);
			this.gbRelatedInfo.Controls.Add(this.lblRecord);
			this.gbRelatedInfo.Location = new System.Drawing.Point(8, 160);
			this.gbRelatedInfo.Name = "gbRelatedInfo";
			this.gbRelatedInfo.Size = new System.Drawing.Size(620, 88);
			this.gbRelatedInfo.TabIndex = 15;
			this.gbRelatedInfo.TabStop = false;
			this.gbRelatedInfo.Text = "Related Information";
			//
			//cbThumbnailImage
			//
			this.cbThumbnailImage.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbThumbnailImage.Location = new System.Drawing.Point(148, 60);
			this.cbThumbnailImage.Name = "cbThumbnailImage";
			this.cbThumbnailImage.Size = new System.Drawing.Size(448, 24);
			this.cbThumbnailImage.TabIndex = 5;
			this.cbThumbnailImage.Text = "cbThumbnailImage";
			//
			//lblThumbnailImage
			//
			this.lblThumbnailImage.AutoSize = true;
			this.lblThumbnailImage.Location = new System.Drawing.Point(16, 60);
			this.lblThumbnailImage.Name = "lblThumbnailImage";
			this.lblThumbnailImage.Size = new System.Drawing.Size(119, 16);
			this.lblThumbnailImage.TabIndex = 4;
			this.lblThumbnailImage.Text = "Thumbnail Image";
			//
			//cbTable
			//
			this.cbTable.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTable.Location = new System.Drawing.Point(88, 27);
			this.cbTable.Name = "cbTable";
			this.cbTable.Size = new System.Drawing.Size(236, 24);
			this.cbTable.TabIndex = 1;
			//
			//lblTable
			//
			this.lblTable.AutoSize = true;
			this.lblTable.Location = new System.Drawing.Point(40, 27);
			this.lblTable.Name = "lblTable";
			this.lblTable.Size = new System.Drawing.Size(44, 16);
			this.lblTable.TabIndex = 0;
			this.lblTable.Text = "Table";
			//
			//cbRecord
			//
			this.cbRecord.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cbRecord.Location = new System.Drawing.Point(400, 27);
			this.cbRecord.Name = "cbRecord";
			this.cbRecord.Size = new System.Drawing.Size(196, 24);
			this.cbRecord.TabIndex = 3;
			//
			//lblRecord
			//
			this.lblRecord.AutoSize = true;
			this.lblRecord.Location = new System.Drawing.Point(344, 27);
			this.lblRecord.Name = "lblRecord";
			this.lblRecord.Size = new System.Drawing.Size(53, 16);
			this.lblRecord.TabIndex = 2;
			this.lblRecord.Text = "Record";
			//
			//gbCaption
			//
			this.gbCaption.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbCaption.Controls.Add(this.rtfCaption);
			this.gbCaption.Location = new System.Drawing.Point(8, 256);
			this.gbCaption.Name = "gbCaption";
			this.gbCaption.Size = new System.Drawing.Size(620, 144);
			this.gbCaption.TabIndex = 111;
			this.gbCaption.TabStop = false;
			this.gbCaption.Text = "Caption";
			//
			//rtfCaption
			//
			this.rtfCaption.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfCaption.Location = new System.Drawing.Point(4, 20);
			this.rtfCaption.Name = "rtfCaption";
			this.rtfCaption.Size = new System.Drawing.Size(608, 120);
			this.rtfCaption.TabIndex = 0;
			this.rtfCaption.Text = "rtfCaption";
			this.ttBase.SetToolTip(this.rtfCaption, "Image Caption");
			//
			//frmImages
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 578);
			this.Name = "frmImages";
			this.Text = "frmImages";
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
			this.gbRelatedInfo.ResumeLayout(false);
			this.gbRelatedInfo.PerformLayout();
			this.gbCaption.ResumeLayout(false);
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
			//General
			BindControl(txtName, mTCBase.MainDataView, "Name");
			BindControl(txtFileName, mTCBase.MainDataView, "FileName");
			BindControl(txtHeight, mTCBase.MainDataView, "Height");
			BindControl(txtWidth, mTCBase.MainDataView, "Width");
			BindControl(cbCategory, mTCBase.MainDataView, "Category", ((clsImages)mTCBase).Categories, "Category", "Category");
			BindControl(chkThumbnail, mTCBase.MainDataView, "Thumbnail");
			BindControl(txtURL, mTCBase.MainDataView, "URL");
			BindControl(cbAlphaSort, mTCBase.MainDataView, "Sort");
			//Note that this guy is Simple-Bound...
			BindControl(rtfCaption, mTCBase.MainDataView, "Caption");

			//BindControl(cbTable, mTCBase.MainDataView, "TableName", dvTables, "TableName", "TableName")
			//BindControl(cbRecord, mTCBase.MainDataView, "TableID", dvRecords, "TableID", "TableID")
			//BindControl(cbThumbnailImage, mTCBase.MainDataView, "ThumbnailImageID", dvThumbnailImages, "ThumbnailImageID", "ThumbnailImageID")
			//Notes...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
			//Image...
		}
		public void DefaultAlphaSort(System.Windows.Forms.ComboBox cbAlphaSort, string Name, string Category)
		{
			if (cbAlphaSort.Items.Count > 0)
				cbAlphaSort.Items.Clear();
			cbAddItem(cbAlphaSort, cbAlphaSort.Text);

			Name = Name.ToUpper();
			Category = Category.ToUpper();
			cbAddItem(cbAlphaSort, string.Format("{0}", Name));
			cbAddItem(cbAlphaSort, string.Format("{0}: {1}", Category, Name));
		}
		private string FormatDimensions(string Prop, ref string Unit)
		{
			string functionReturnValue = null;
			functionReturnValue = Prop;
			Unit = "Unknown";
			if (Strings.Asc(functionReturnValue.Substring(0, 1)) == 63)
				functionReturnValue = functionReturnValue.Substring(1);
			if (functionReturnValue.Contains(" ")) {
				Unit = ParseStr(functionReturnValue, 2, " ");
				functionReturnValue = ParseStr(functionReturnValue, 1, " ");
			}
			return functionReturnValue;
		}
		#endregion
		#region "Event Handlers"
		protected new void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			try {
				foreach (Control ctl in this.gbGeneral.Controls) {
					this.epBase.SetError(ctl, bpeNullString);
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void DefaultAlphaSort(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				ComboBox cb = (ComboBox)sender;
				//Give the user options...
				DefaultAlphaSort(cb, this.txtName.Text, this.cbCategory.Text);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmImages.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(txtFileName);
				RemoveControlHandlers(txtHeight);
				RemoveControlHandlers(txtWidth);
				RemoveControlHandlers(cbCategory);
				RemoveControlHandlers(chkThumbnail);
				RemoveControlHandlers(txtURL);
				cbAlphaSort.Enter -= DefaultAlphaSort;
				cbAlphaSort.Validating -= SetCaption;
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
			const string EntryName = "frmImages.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(txtFileName);
				SetupControlHandlers(txtHeight);
				SetupControlHandlers(txtWidth);
				SetupControlHandlers(cbCategory);
				SetupControlHandlers(chkThumbnail);
				SetupControlHandlers(txtURL);
				cbAlphaSort.Enter += DefaultAlphaSort;
				cbAlphaSort.Validating += SetCaption;
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
				//If Not IsNothing(sender) Then MyBase.epBase.SetError((Control)sender, bpeNullString)

				this.txtCaption.Text = string.Format("{0}: {1}", mTCBase.CurrentRow["ID"], mTCBase.CurrentRow["Name"]);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				if ((sender != null))
					base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		protected void Form_ImageImported(object sender, ImageImportedEventArgs e)
		{
            //TODO: Review whole FileProperty functionality...
            this.txtFileName.Text = e.ImagePath;
            if (this.txtURL.Text == bpeNullString)
                this.txtURL.Text = (string)e.GetProperty("URL");
			string Unit = "pixels";
            //this.txtWidth.Text = this.FormatDimensions((string)e.GetProperty("Width"), ref Unit);
            //this.txtHeight.Text = this.FormatDimensions((string)e.GetProperty("Height"), ref Unit);
            this.txtWidth.Text = Convert.ToString(e.Width);     this.ttBase.SetToolTip(this.txtWidth, $"Image width in {Unit}");
            this.txtHeight.Text = Convert.ToString(e.Height);   this.ttBase.SetToolTip(this.txtHeight, $"Image height in {Unit}");
			this.lblPixels.Text = $"(W x H in {Unit})";
			//e.Test()
			//For some reason Binding isn't behaving as expected here. It seems if the data isn't changed through the UI,
			//values don't propagate down to the DataRow...
			mTCBase.CurrentRow["FileName"] = this.txtFileName.Text;
			mTCBase.CurrentRow["URL"] = this.txtURL.Text;
			mTCBase.CurrentRow["Width"] = this.txtWidth.Text;
			mTCBase.CurrentRow["Height"] = this.txtHeight.Text;
		}
		#endregion
	}
}
