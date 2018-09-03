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
//frmCompanies.vb
//   Companies Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmCompanies : frmTCStandard
	{
		const string myFormName = "frmCompanies";
		public frmCompanies(clsSupport objSupport, clsCompanies objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
			TabPage[] pages = { tpGeneral };
			this.tcMain.TabPages.AddRange(pages);
			this.tcMain.ResumeLayout(false);
		}
		#region " Windows Form Designer generated code "
		public frmCompanies() : base()
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
		internal System.Windows.Forms.TextBox txtCode;
		internal System.Windows.Forms.Label lblCode;
		internal System.Windows.Forms.Label lblProductType;
		internal System.Windows.Forms.ComboBox cbProductType;
		internal System.Windows.Forms.TextBox txtAccount;
		internal System.Windows.Forms.Label lblAccount;
		internal System.Windows.Forms.TextBox txtShortName;
		internal System.Windows.Forms.Label lblShortName;
		internal System.Windows.Forms.Label lblPhone;
		internal System.Windows.Forms.TextBox txtPhone;
		internal System.Windows.Forms.TextBox txtAddress;
		internal System.Windows.Forms.Label lblAddress;
		internal System.Windows.Forms.TextBox txtWebSite;
		internal System.Windows.Forms.Label lblWebSite;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompanies));
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.lblCode = new System.Windows.Forms.Label();
			this.lblProductType = new System.Windows.Forms.Label();
			this.cbProductType = new System.Windows.Forms.ComboBox();
			this.txtAccount = new System.Windows.Forms.TextBox();
			this.lblAccount = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.lblShortName = new System.Windows.Forms.Label();
			this.lblPhone = new System.Windows.Forms.Label();
			this.txtPhone = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblAddress = new System.Windows.Forms.Label();
			this.txtWebSite = new System.Windows.Forms.TextBox();
			this.lblWebSite = new System.Windows.Forms.Label();
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
			this.sbpTime.Text = "5:46 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 474);
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(23, 278);
			this.lblPurchased.Visible = false;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(15, 278);
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(36, 278);
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(26, 278);
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(600, 55);
			this.lblPrice.Visible = false;
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
			this.chkWishList.Location = new System.Drawing.Point(544, 80);
			this.chkWishList.TabIndex = 6;
			this.chkWishList.Visible = false;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(104, 276);
			this.dtpPurchased.TabIndex = 12;
			this.dtpPurchased.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(104, 276);
			this.dtpInventoried.TabIndex = 13;
			this.dtpInventoried.Visible = false;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(104, 275);
			this.cbLocation.Size = new System.Drawing.Size(308, 24);
			this.cbLocation.TabIndex = 11;
			this.cbLocation.Visible = false;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(104, 275);
			this.txtAlphaSort.Size = new System.Drawing.Size(116, 23);
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(428, 108);
			this.pbGeneral.Size = new System.Drawing.Size(320, 204);
			this.pbGeneral.Visible = false;
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
			this.txtPrice.Location = new System.Drawing.Point(644, 53);
			this.txtPrice.TabIndex = 4;
			this.txtPrice.Visible = false;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(104, 275);
			this.cbAlphaSort.Size = new System.Drawing.Size(108, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtWebSite);
			this.gbGeneral.Controls.Add(this.lblWebSite);
			this.gbGeneral.Controls.Add(this.txtAddress);
			this.gbGeneral.Controls.Add(this.lblAddress);
			this.gbGeneral.Controls.Add(this.txtPhone);
			this.gbGeneral.Controls.Add(this.lblPhone);
			this.gbGeneral.Controls.Add(this.lblShortName);
			this.gbGeneral.Controls.Add(this.txtAccount);
			this.gbGeneral.Controls.Add(this.lblAccount);
			this.gbGeneral.Controls.Add(this.txtCode);
			this.gbGeneral.Controls.Add(this.lblCode);
			this.gbGeneral.Controls.Add(this.lblProductType);
			this.gbGeneral.Controls.Add(this.cbProductType);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.txtShortName);
			this.gbGeneral.Size = new System.Drawing.Size(756, 343);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtShortName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbProductType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblProductType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblCode, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtCode, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAccount, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAccount, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblShortName, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPhone, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPhone, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAddress, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAddress, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblWebSite, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtWebSite, 0);
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
			this.tpNotes.Size = new System.Drawing.Size(656, 270);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(424, 316);
			this.hsbGeneral.Size = new System.Drawing.Size(324, 17);
			this.hsbGeneral.Visible = false;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(428, 108);
			this.pbGeneral2.Size = new System.Drawing.Size(320, 204);
			this.pbGeneral2.Visible = false;
			//
			//dtpVerified
			//
			this.dtpVerified.Location = new System.Drawing.Point(176, 155);
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
			//txtCode
			//
			this.txtCode.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtCode.Location = new System.Drawing.Point(104, 81);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(304, 23);
			this.txtCode.TabIndex = 3;
			this.txtCode.Tag = "Required";
			this.txtCode.Text = "TXTCODE";
			//
			//lblCode
			//
			this.lblCode.AutoSize = true;
			this.lblCode.Location = new System.Drawing.Point(58, 83);
			this.lblCode.Name = "lblCode";
			this.lblCode.Size = new System.Drawing.Size(41, 16);
			this.lblCode.TabIndex = 108;
			this.lblCode.Text = "Code";
			//
			//lblProductType
			//
			this.lblProductType.AutoSize = true;
			this.lblProductType.Location = new System.Drawing.Point(3, 112);
			this.lblProductType.Name = "lblProductType";
			this.lblProductType.Size = new System.Drawing.Size(97, 16);
			this.lblProductType.TabIndex = 106;
			this.lblProductType.Text = "Product Type";
			//
			//cbProductType
			//
			this.cbProductType.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.cbProductType.DropDownWidth = 300;
			this.cbProductType.Location = new System.Drawing.Point(104, 108);
			this.cbProductType.Name = "cbProductType";
			this.cbProductType.Size = new System.Drawing.Size(308, 24);
			this.cbProductType.TabIndex = 5;
			this.cbProductType.Text = "cbProductType";
			//
			//txtAccount
			//
			this.txtAccount.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.txtAccount.Location = new System.Drawing.Point(504, 81);
			this.txtAccount.Name = "txtAccount";
			this.txtAccount.Size = new System.Drawing.Size(240, 23);
			this.txtAccount.TabIndex = 4;
			this.txtAccount.Tag = "";
			this.txtAccount.Text = "txtAccount";
			//
			//lblAccount
			//
			this.lblAccount.AutoSize = true;
			this.lblAccount.Location = new System.Drawing.Point(436, 83);
			this.lblAccount.Name = "lblAccount";
			this.lblAccount.Size = new System.Drawing.Size(63, 16);
			this.lblAccount.TabIndex = 112;
			this.lblAccount.Text = "Account";
			//
			//txtShortName
			//
			this.txtShortName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtShortName.Location = new System.Drawing.Point(104, 53);
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(640, 23);
			this.txtShortName.TabIndex = 2;
			this.txtShortName.Tag = "Required";
			this.txtShortName.Text = "txtShortName";
			//
			//lblShortName
			//
			this.lblShortName.AutoSize = true;
			this.lblShortName.Location = new System.Drawing.Point(12, 55);
			this.lblShortName.Name = "lblShortName";
			this.lblShortName.Size = new System.Drawing.Size(85, 16);
			this.lblShortName.TabIndex = 114;
			this.lblShortName.Text = "Short Name";
			//
			//lblPhone
			//
			this.lblPhone.AutoSize = true;
			this.lblPhone.Location = new System.Drawing.Point(448, 111);
			this.lblPhone.Name = "lblPhone";
			this.lblPhone.Size = new System.Drawing.Size(48, 16);
			this.lblPhone.TabIndex = 115;
			this.lblPhone.Text = "Phone";
			//
			//txtPhone
			//
			this.txtPhone.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.txtPhone.Location = new System.Drawing.Point(504, 109);
			this.txtPhone.Name = "txtPhone";
			this.txtPhone.Size = new System.Drawing.Size(240, 23);
			this.txtPhone.TabIndex = 6;
			this.txtPhone.Tag = "";
			this.txtPhone.Text = "txtPhone";
			//
			//txtAddress
			//
			this.txtAddress.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtAddress.Location = new System.Drawing.Point(104, 136);
			this.txtAddress.Multiline = true;
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(640, 132);
			this.txtAddress.TabIndex = 7;
			this.txtAddress.Tag = "";
			this.txtAddress.Text = "txtAddress";
			//
			//lblAddress
			//
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(38, 140);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(60, 16);
			this.lblAddress.TabIndex = 117;
			this.lblAddress.Text = "Address";
			//
			//txtWebSite
			//
			this.txtWebSite.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtWebSite.Location = new System.Drawing.Point(104, 276);
			this.txtWebSite.Name = "txtWebSite";
			this.txtWebSite.Size = new System.Drawing.Size(640, 23);
			this.txtWebSite.TabIndex = 8;
			this.txtWebSite.Tag = "";
			this.txtWebSite.Text = "txtWebSite";
			//
			//lblWebSite
			//
			this.lblWebSite.AutoSize = true;
			this.lblWebSite.Location = new System.Drawing.Point(36, 278);
			this.lblWebSite.Name = "lblWebSite";
			this.lblWebSite.Size = new System.Drawing.Size(63, 16);
			this.lblWebSite.TabIndex = 120;
			this.lblWebSite.Text = "WebSite";
			//
			//frmCompanies
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(792, 529);
			this.MinimumSize = new System.Drawing.Size(808, 567);
			this.Name = "frmCompanies";
			this.Text = "frmCompanies";
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
			BindControl(txtShortName, mTCBase.MainDataView, "ShortName");
			BindControl(txtCode, mTCBase.MainDataView, "Code");
			BindControl(txtAccount, mTCBase.MainDataView, "Account");
			BindControl(cbProductType, mTCBase.MainDataView, "ProductType", ((clsCompanies)mTCBase).ProductTypes, "ProductType", "ProductType");
			BindControl(txtPhone, mTCBase.MainDataView, "Phone");
			BindControl(txtAddress, mTCBase.MainDataView, "Address");
			BindControl(txtWebSite, mTCBase.MainDataView, "WebSite");
		}
		#endregion
		#region "Event Handlers"
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmCompanies.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(txtShortName);
				RemoveControlHandlers(txtCode);
				RemoveControlHandlers(txtAccount);
				txtAccount.Validating -= DefaultTextBox;
				RemoveControlHandlers(cbProductType);
				RemoveControlHandlers(txtPhone);
				RemoveControlHandlers(txtAddress);
				RemoveControlHandlers(txtWebSite);

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
			const string EntryName = "frmCompanies.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(txtShortName);
				SetupControlHandlers(txtCode);
				SetupControlHandlers(txtAccount);
				txtAccount.Validating += DefaultTextBox;
				SetupControlHandlers(cbProductType);
				SetupControlHandlers(txtPhone);
				SetupControlHandlers(txtAddress);
				SetupControlHandlers(txtWebSite);

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
				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["Name"]) ? bpeNullString : Convert.ToString(mTCBase.CurrentRow["Name"]).ToUpper());
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
