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
//frmClasses.vb
//   USN Classifications Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   07/25/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCUSNavy
{
	public class frmClasses : frmTCStandard
	{
		const string myFormName = "frmClasses";
		public frmClasses(clsSupport objSupport, clsClasses objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
				tpCharacteristics,
				tpWeapons,
				tpDescription,
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
		public frmClasses() : base()
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
		internal System.Windows.Forms.TextBox txtYear;
		internal System.Windows.Forms.Label lblYear;
		internal System.Windows.Forms.TabPage tpCharacteristics;
		internal System.Windows.Forms.TabPage tpWeapons;
		internal System.Windows.Forms.Label lblClassification;
		internal System.Windows.Forms.ComboBox cbClassification;
		internal System.Windows.Forms.TextBox txtClassDesc;
		internal System.Windows.Forms.GroupBox gbCharacteristics;
		internal System.Windows.Forms.TextBox txtDisplacement;
		internal System.Windows.Forms.TextBox txtLength;
		internal System.Windows.Forms.TextBox txtBeam;
		internal System.Windows.Forms.TextBox txtDraft;
		internal System.Windows.Forms.TextBox txtPropulsion;
		internal System.Windows.Forms.TextBox txtBoilers;
		internal System.Windows.Forms.TextBox txtManning;
		internal System.Windows.Forms.Label lblDisplacement;
		internal System.Windows.Forms.Label lblLength;
		internal System.Windows.Forms.Label lblBeam;
		internal System.Windows.Forms.Label lblDraft;
		internal System.Windows.Forms.Label lblPropulsion;
		internal System.Windows.Forms.Label lblBoilers;
		internal System.Windows.Forms.Label lblManning;
		internal System.Windows.Forms.Label lblSpeed;
		internal System.Windows.Forms.TextBox txtSpeed;
		internal System.Windows.Forms.GroupBox gbWeapons;
		internal System.Windows.Forms.Label lblEW;
		internal System.Windows.Forms.Label lblFireControl;
		internal System.Windows.Forms.Label lblSONARs;
		internal System.Windows.Forms.Label lblRADARs;
		internal System.Windows.Forms.Label lblASW;
		internal System.Windows.Forms.Label lblGuns;
		internal System.Windows.Forms.Label lblMissiles;
		internal System.Windows.Forms.Label lblAircraft;
		internal System.Windows.Forms.TextBox txtAircraft;
		internal System.Windows.Forms.TextBox txtMissiles;
		internal System.Windows.Forms.TextBox txtGuns;
		internal System.Windows.Forms.TextBox txtASW;
		internal System.Windows.Forms.TextBox txtRADARs;
		internal System.Windows.Forms.TextBox txtSONARs;
		internal System.Windows.Forms.TextBox txtFireControl;
		internal System.Windows.Forms.TextBox txtEW;
		internal System.Windows.Forms.TabPage tpDescription;
		internal System.Windows.Forms.TabPage tpImage;
		internal System.Windows.Forms.RichTextBox rtfDescription;
		internal System.Windows.Forms.TextBox txtClassification;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtYear = new System.Windows.Forms.TextBox();
			this.lblYear = new System.Windows.Forms.Label();
			this.tpCharacteristics = new System.Windows.Forms.TabPage();
			this.gbCharacteristics = new System.Windows.Forms.GroupBox();
			this.txtSpeed = new System.Windows.Forms.TextBox();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.lblManning = new System.Windows.Forms.Label();
			this.lblBoilers = new System.Windows.Forms.Label();
			this.lblPropulsion = new System.Windows.Forms.Label();
			this.lblDraft = new System.Windows.Forms.Label();
			this.lblBeam = new System.Windows.Forms.Label();
			this.lblLength = new System.Windows.Forms.Label();
			this.lblDisplacement = new System.Windows.Forms.Label();
			this.txtManning = new System.Windows.Forms.TextBox();
			this.txtBoilers = new System.Windows.Forms.TextBox();
			this.txtPropulsion = new System.Windows.Forms.TextBox();
			this.txtDraft = new System.Windows.Forms.TextBox();
			this.txtBeam = new System.Windows.Forms.TextBox();
			this.txtLength = new System.Windows.Forms.TextBox();
			this.txtDisplacement = new System.Windows.Forms.TextBox();
			this.tpWeapons = new System.Windows.Forms.TabPage();
			this.gbWeapons = new System.Windows.Forms.GroupBox();
			this.txtEW = new System.Windows.Forms.TextBox();
			this.lblEW = new System.Windows.Forms.Label();
			this.lblFireControl = new System.Windows.Forms.Label();
			this.lblSONARs = new System.Windows.Forms.Label();
			this.lblRADARs = new System.Windows.Forms.Label();
			this.lblASW = new System.Windows.Forms.Label();
			this.lblGuns = new System.Windows.Forms.Label();
			this.lblMissiles = new System.Windows.Forms.Label();
			this.lblAircraft = new System.Windows.Forms.Label();
			this.txtFireControl = new System.Windows.Forms.TextBox();
			this.txtSONARs = new System.Windows.Forms.TextBox();
			this.txtRADARs = new System.Windows.Forms.TextBox();
			this.txtASW = new System.Windows.Forms.TextBox();
			this.txtGuns = new System.Windows.Forms.TextBox();
			this.txtMissiles = new System.Windows.Forms.TextBox();
			this.txtAircraft = new System.Windows.Forms.TextBox();
			this.lblClassification = new System.Windows.Forms.Label();
			this.cbClassification = new System.Windows.Forms.ComboBox();
			this.txtClassDesc = new System.Windows.Forms.TextBox();
			this.tpDescription = new System.Windows.Forms.TabPage();
			this.rtfDescription = new System.Windows.Forms.RichTextBox();
			this.tpImage = new System.Windows.Forms.TabPage();
			this.txtClassification = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			this.tpNotes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			this.tpGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.gbGeneral.SuspendLayout();
			this.tpCharacteristics.SuspendLayout();
			this.gbCharacteristics.SuspendLayout();
			this.tpWeapons.SuspendLayout();
			this.gbWeapons.SuspendLayout();
			this.tpDescription.SuspendLayout();
			this.SuspendLayout();
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 367);
			this.lblID.Name = "lblID";
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
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(508, 364);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 20;
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
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(589, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 22;
			//
			//sbpTime
			//
			this.sbpTime.Text = "5:09 PM";
			this.sbpTime.Width = 71;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 125);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.TabIndex = 7;
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(16, 156);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.TabIndex = 11;
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 125);
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
			//sbpStatus
			//
			this.sbpStatus.Text = "Filter Off";
			this.sbpStatus.Width = 74;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 484;
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(484, 186);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 17;
			this.chkWishList.Visible = false;
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 189);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.TabIndex = 13;
			this.lblPurchased.Visible = false;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 189);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.TabIndex = 15;
			this.lblInventoried.Visible = false;
			//
			//sbpFilter
			//
			this.sbpFilter.Text = "";
			this.sbpFilter.Width = 10;
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(652, 270);
			//
			//sbpMode
			//
			this.sbpMode.Text = "";
			this.sbpMode.Width = 10;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 187);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 14;
			this.dtpPurchased.Visible = false;
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(648, 255);
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpWeapons);
			this.tcMain.Controls.Add(this.tpDescription);
			this.tcMain.Controls.Add(this.tpImage);
			this.tcMain.Controls.Add(this.tpCharacteristics);
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(656, 284);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpCharacteristics, 0);
			this.tcMain.Controls.SetChildIndex(this.tpImage, 0);
			this.tcMain.Controls.SetChildIndex(this.tpDescription, 0);
			this.tcMain.Controls.SetChildIndex(this.tpWeapons, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtClassDesc);
			this.gbGeneral.Controls.Add(this.lblClassification);
			this.gbGeneral.Controls.Add(this.txtYear);
			this.gbGeneral.Controls.Add(this.lblYear);
			this.gbGeneral.Controls.Add(this.lblName);
			this.gbGeneral.Controls.Add(this.txtName);
			this.gbGeneral.Controls.Add(this.cbClassification);
			this.gbGeneral.Controls.Add(this.txtClassification);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(636, 240);
			this.gbGeneral.Controls.SetChildIndex(this.txtClassification, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbClassification, 0);
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
			this.gbGeneral.Controls.SetChildIndex(this.lblYear, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtYear, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblClassification, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtClassDesc, 0);
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(96, 154);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(516, 24);
			this.cbAlphaSort.TabIndex = 12;
			this.cbAlphaSort.Visible = false;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 123);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Visible = false;
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(620, 173);
			this.ttBase.SetToolTip(this.rtfNotes, "Description of Class");
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 368);
			this.txtID.Name = "txtID";
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 328);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(544, 24);
			this.txtCaption.TabIndex = 2;
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral.Visible = false;
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(96, 156);
			this.txtAlphaSort.Name = "txtAlphaSort";
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(96, 122);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.TabIndex = 8;
			this.cbLocation.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 187);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 16;
			this.dtpInventoried.Visible = false;
			//
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(108, 20);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(500, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Tag = "Required";
			this.txtName.Text = "txtName";
			this.ttBase.SetToolTip(this.txtName, "Name of Class/Ship");
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
			//txtYear
			//
			this.txtYear.Location = new System.Drawing.Point(108, 49);
			this.txtYear.Name = "txtYear";
			this.txtYear.Size = new System.Drawing.Size(64, 23);
			this.txtYear.TabIndex = 3;
			this.txtYear.Tag = "Required, Numeric";
			this.txtYear.Text = "txtYear";
			this.ttBase.SetToolTip(this.txtYear, "Year Class was Established");
			//
			//lblYear
			//
			this.lblYear.AutoSize = true;
			this.lblYear.Location = new System.Drawing.Point(65, 51);
			this.lblYear.Name = "lblYear";
			this.lblYear.Size = new System.Drawing.Size(35, 19);
			this.lblYear.TabIndex = 2;
			this.lblYear.Text = "Year";
			//
			//tpCharacteristics
			//
			this.tpCharacteristics.Controls.Add(this.gbCharacteristics);
			this.tpCharacteristics.Location = new System.Drawing.Point(4, 25);
			this.tpCharacteristics.Name = "tpCharacteristics";
			this.tpCharacteristics.Size = new System.Drawing.Size(648, 255);
			this.tpCharacteristics.TabIndex = 2;
			this.tpCharacteristics.Text = "Characteristics";
			//
			//gbCharacteristics
			//
			this.gbCharacteristics.Controls.Add(this.txtSpeed);
			this.gbCharacteristics.Controls.Add(this.lblSpeed);
			this.gbCharacteristics.Controls.Add(this.lblManning);
			this.gbCharacteristics.Controls.Add(this.lblBoilers);
			this.gbCharacteristics.Controls.Add(this.lblPropulsion);
			this.gbCharacteristics.Controls.Add(this.lblDraft);
			this.gbCharacteristics.Controls.Add(this.lblBeam);
			this.gbCharacteristics.Controls.Add(this.lblLength);
			this.gbCharacteristics.Controls.Add(this.lblDisplacement);
			this.gbCharacteristics.Controls.Add(this.txtManning);
			this.gbCharacteristics.Controls.Add(this.txtBoilers);
			this.gbCharacteristics.Controls.Add(this.txtPropulsion);
			this.gbCharacteristics.Controls.Add(this.txtDraft);
			this.gbCharacteristics.Controls.Add(this.txtBeam);
			this.gbCharacteristics.Controls.Add(this.txtLength);
			this.gbCharacteristics.Controls.Add(this.txtDisplacement);
			this.gbCharacteristics.Location = new System.Drawing.Point(8, 8);
			this.gbCharacteristics.Name = "gbCharacteristics";
			this.gbCharacteristics.Size = new System.Drawing.Size(636, 244);
			this.gbCharacteristics.TabIndex = 0;
			this.gbCharacteristics.TabStop = false;
			//
			//txtSpeed
			//
			this.txtSpeed.Location = new System.Drawing.Point(116, 217);
			this.txtSpeed.Name = "txtSpeed";
			this.txtSpeed.TabIndex = 15;
			this.txtSpeed.Tag = "Numeric";
			this.txtSpeed.Text = "txtSpeed";
			this.ttBase.SetToolTip(this.txtSpeed, "Maximum Speed (in knots)");
			//
			//lblSpeed
			//
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.Location = new System.Drawing.Point(57, 219);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(47, 19);
			this.lblSpeed.TabIndex = 14;
			this.lblSpeed.Text = "Speed";
			this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblManning
			//
			this.lblManning.AutoSize = true;
			this.lblManning.Location = new System.Drawing.Point(42, 191);
			this.lblManning.Name = "lblManning";
			this.lblManning.Size = new System.Drawing.Size(62, 19);
			this.lblManning.TabIndex = 12;
			this.lblManning.Text = "Manning";
			this.lblManning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblBoilers
			//
			this.lblBoilers.AutoSize = true;
			this.lblBoilers.Location = new System.Drawing.Point(54, 163);
			this.lblBoilers.Name = "lblBoilers";
			this.lblBoilers.Size = new System.Drawing.Size(50, 19);
			this.lblBoilers.TabIndex = 10;
			this.lblBoilers.Text = "Boilers";
			this.lblBoilers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblPropulsion
			//
			this.lblPropulsion.AutoSize = true;
			this.lblPropulsion.Location = new System.Drawing.Point(30, 135);
			this.lblPropulsion.Name = "lblPropulsion";
			this.lblPropulsion.Size = new System.Drawing.Size(74, 19);
			this.lblPropulsion.TabIndex = 8;
			this.lblPropulsion.Text = "Propulsion";
			this.lblPropulsion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblDraft
			//
			this.lblDraft.AutoSize = true;
			this.lblDraft.Location = new System.Drawing.Point(65, 107);
			this.lblDraft.Name = "lblDraft";
			this.lblDraft.Size = new System.Drawing.Size(39, 19);
			this.lblDraft.TabIndex = 6;
			this.lblDraft.Text = "Draft";
			this.lblDraft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblBeam
			//
			this.lblBeam.AutoSize = true;
			this.lblBeam.Location = new System.Drawing.Point(61, 79);
			this.lblBeam.Name = "lblBeam";
			this.lblBeam.Size = new System.Drawing.Size(43, 19);
			this.lblBeam.TabIndex = 4;
			this.lblBeam.Text = "Beam";
			this.lblBeam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblLength
			//
			this.lblLength.AutoSize = true;
			this.lblLength.Location = new System.Drawing.Point(53, 51);
			this.lblLength.Name = "lblLength";
			this.lblLength.Size = new System.Drawing.Size(51, 19);
			this.lblLength.TabIndex = 2;
			this.lblLength.Text = "Length";
			this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblDisplacement
			//
			this.lblDisplacement.AutoSize = true;
			this.lblDisplacement.Location = new System.Drawing.Point(8, 23);
			this.lblDisplacement.Name = "lblDisplacement";
			this.lblDisplacement.Size = new System.Drawing.Size(96, 19);
			this.lblDisplacement.TabIndex = 0;
			this.lblDisplacement.Text = "Displacement";
			this.lblDisplacement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//txtManning
			//
			this.txtManning.Location = new System.Drawing.Point(116, 188);
			this.txtManning.Name = "txtManning";
			this.txtManning.Size = new System.Drawing.Size(492, 24);
			this.txtManning.TabIndex = 13;
			this.txtManning.Text = "txtManning";
			this.ttBase.SetToolTip(this.txtManning, "Ship's Complement (size of crew)");
			//
			//txtBoilers
			//
			this.txtBoilers.Location = new System.Drawing.Point(116, 160);
			this.txtBoilers.Name = "txtBoilers";
			this.txtBoilers.Size = new System.Drawing.Size(492, 24);
			this.txtBoilers.TabIndex = 11;
			this.txtBoilers.Text = "txtBoilers";
			this.ttBase.SetToolTip(this.txtBoilers, "Number of Boilers - if applicable)");
			//
			//txtPropulsion
			//
			this.txtPropulsion.Location = new System.Drawing.Point(116, 132);
			this.txtPropulsion.Name = "txtPropulsion";
			this.txtPropulsion.Size = new System.Drawing.Size(492, 24);
			this.txtPropulsion.TabIndex = 9;
			this.txtPropulsion.Text = "txtPropulsion";
			this.ttBase.SetToolTip(this.txtPropulsion, "Propulsion");
			//
			//txtDraft
			//
			this.txtDraft.Location = new System.Drawing.Point(116, 104);
			this.txtDraft.Name = "txtDraft";
			this.txtDraft.Size = new System.Drawing.Size(492, 24);
			this.txtDraft.TabIndex = 7;
			this.txtDraft.Text = "txtDraft";
			this.ttBase.SetToolTip(this.txtDraft, "Draft (i.e. depth below waterline - in feet)");
			//
			//txtBeam
			//
			this.txtBeam.Location = new System.Drawing.Point(116, 76);
			this.txtBeam.Name = "txtBeam";
			this.txtBeam.Size = new System.Drawing.Size(492, 24);
			this.txtBeam.TabIndex = 5;
			this.txtBeam.Text = "txtBeam";
			this.ttBase.SetToolTip(this.txtBeam, "Beam (i.e. Width - in feet)");
			//
			//txtLength
			//
			this.txtLength.Location = new System.Drawing.Point(116, 48);
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(492, 24);
			this.txtLength.TabIndex = 3;
			this.txtLength.Text = "txtLength";
			this.ttBase.SetToolTip(this.txtLength, "Length (in feet)");
			//
			//txtDisplacement
			//
			this.txtDisplacement.Location = new System.Drawing.Point(116, 20);
			this.txtDisplacement.Name = "txtDisplacement";
			this.txtDisplacement.Size = new System.Drawing.Size(492, 24);
			this.txtDisplacement.TabIndex = 1;
			this.txtDisplacement.Text = "txtDisplacement";
			this.ttBase.SetToolTip(this.txtDisplacement, "Displacement (in tons)");
			//
			//tpWeapons
			//
			this.tpWeapons.Controls.Add(this.gbWeapons);
			this.tpWeapons.Location = new System.Drawing.Point(4, 25);
			this.tpWeapons.Name = "tpWeapons";
			this.tpWeapons.Size = new System.Drawing.Size(648, 255);
			this.tpWeapons.TabIndex = 3;
			this.tpWeapons.Text = "Weapons";
			//
			//gbWeapons
			//
			this.gbWeapons.Controls.Add(this.txtEW);
			this.gbWeapons.Controls.Add(this.lblEW);
			this.gbWeapons.Controls.Add(this.lblFireControl);
			this.gbWeapons.Controls.Add(this.lblSONARs);
			this.gbWeapons.Controls.Add(this.lblRADARs);
			this.gbWeapons.Controls.Add(this.lblASW);
			this.gbWeapons.Controls.Add(this.lblGuns);
			this.gbWeapons.Controls.Add(this.lblMissiles);
			this.gbWeapons.Controls.Add(this.lblAircraft);
			this.gbWeapons.Controls.Add(this.txtFireControl);
			this.gbWeapons.Controls.Add(this.txtSONARs);
			this.gbWeapons.Controls.Add(this.txtRADARs);
			this.gbWeapons.Controls.Add(this.txtASW);
			this.gbWeapons.Controls.Add(this.txtGuns);
			this.gbWeapons.Controls.Add(this.txtMissiles);
			this.gbWeapons.Controls.Add(this.txtAircraft);
			this.gbWeapons.Location = new System.Drawing.Point(8, 8);
			this.gbWeapons.Name = "gbWeapons";
			this.gbWeapons.Size = new System.Drawing.Size(636, 244);
			this.gbWeapons.TabIndex = 0;
			this.gbWeapons.TabStop = false;
			//
			//txtEW
			//
			this.txtEW.Location = new System.Drawing.Point(104, 209);
			this.txtEW.Name = "txtEW";
			this.txtEW.Size = new System.Drawing.Size(500, 24);
			this.txtEW.TabIndex = 15;
			this.txtEW.Text = "txtEW";
			this.ttBase.SetToolTip(this.txtEW, "Electronic Warfare Countermeasures");
			//
			//lblEW
			//
			this.lblEW.AutoSize = true;
			this.lblEW.Location = new System.Drawing.Point(68, 212);
			this.lblEW.Name = "lblEW";
			this.lblEW.Size = new System.Drawing.Size(27, 19);
			this.lblEW.TabIndex = 14;
			this.lblEW.Text = "EW";
			this.lblEW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblFireControl
			//
			this.lblFireControl.AutoSize = true;
			this.lblFireControl.Location = new System.Drawing.Point(12, 184);
			this.lblFireControl.Name = "lblFireControl";
			this.lblFireControl.Size = new System.Drawing.Size(83, 19);
			this.lblFireControl.TabIndex = 12;
			this.lblFireControl.Text = "Fire Control";
			this.lblFireControl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblSONARs
			//
			this.lblSONARs.AutoSize = true;
			this.lblSONARs.Location = new System.Drawing.Point(35, 156);
			this.lblSONARs.Name = "lblSONARs";
			this.lblSONARs.Size = new System.Drawing.Size(60, 19);
			this.lblSONARs.TabIndex = 10;
			this.lblSONARs.Text = "SONARs";
			this.lblSONARs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblRADARs
			//
			this.lblRADARs.AutoSize = true;
			this.lblRADARs.Location = new System.Drawing.Point(36, 128);
			this.lblRADARs.Name = "lblRADARs";
			this.lblRADARs.Size = new System.Drawing.Size(59, 19);
			this.lblRADARs.TabIndex = 8;
			this.lblRADARs.Text = "RADARs";
			this.lblRADARs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblASW
			//
			this.lblASW.AutoSize = true;
			this.lblASW.Location = new System.Drawing.Point(59, 100);
			this.lblASW.Name = "lblASW";
			this.lblASW.Size = new System.Drawing.Size(36, 19);
			this.lblASW.TabIndex = 6;
			this.lblASW.Text = "ASW";
			this.lblASW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblGuns
			//
			this.lblGuns.AutoSize = true;
			this.lblGuns.Location = new System.Drawing.Point(56, 72);
			this.lblGuns.Name = "lblGuns";
			this.lblGuns.Size = new System.Drawing.Size(39, 19);
			this.lblGuns.TabIndex = 4;
			this.lblGuns.Text = "Guns";
			this.lblGuns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblMissiles
			//
			this.lblMissiles.AutoSize = true;
			this.lblMissiles.Location = new System.Drawing.Point(39, 44);
			this.lblMissiles.Name = "lblMissiles";
			this.lblMissiles.Size = new System.Drawing.Size(56, 19);
			this.lblMissiles.TabIndex = 2;
			this.lblMissiles.Text = "Missiles";
			this.lblMissiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblAircraft
			//
			this.lblAircraft.AutoSize = true;
			this.lblAircraft.Location = new System.Drawing.Point(41, 16);
			this.lblAircraft.Name = "lblAircraft";
			this.lblAircraft.Size = new System.Drawing.Size(54, 19);
			this.lblAircraft.TabIndex = 0;
			this.lblAircraft.Text = "Aircraft";
			this.lblAircraft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//txtFireControl
			//
			this.txtFireControl.Location = new System.Drawing.Point(104, 181);
			this.txtFireControl.Name = "txtFireControl";
			this.txtFireControl.Size = new System.Drawing.Size(500, 24);
			this.txtFireControl.TabIndex = 13;
			this.txtFireControl.Text = "txtFireControl";
			this.ttBase.SetToolTip(this.txtFireControl, "Types of Fire Control Equipment");
			//
			//txtSONARs
			//
			this.txtSONARs.Location = new System.Drawing.Point(104, 153);
			this.txtSONARs.Name = "txtSONARs";
			this.txtSONARs.Size = new System.Drawing.Size(500, 24);
			this.txtSONARs.TabIndex = 11;
			this.txtSONARs.Text = "txtSONARs";
			this.ttBase.SetToolTip(this.txtSONARs, "Types of SONAR Equipped");
			//
			//txtRADARs
			//
			this.txtRADARs.Location = new System.Drawing.Point(104, 125);
			this.txtRADARs.Name = "txtRADARs";
			this.txtRADARs.Size = new System.Drawing.Size(500, 24);
			this.txtRADARs.TabIndex = 9;
			this.txtRADARs.Text = "txtRADARs";
			this.ttBase.SetToolTip(this.txtRADARs, "Types of RADAR Equipped");
			//
			//txtASW
			//
			this.txtASW.Location = new System.Drawing.Point(104, 97);
			this.txtASW.Name = "txtASW";
			this.txtASW.Size = new System.Drawing.Size(500, 24);
			this.txtASW.TabIndex = 7;
			this.txtASW.Text = "txtASW";
			this.ttBase.SetToolTip(this.txtASW, "Anti-Submarine Warfare Weapons");
			//
			//txtGuns
			//
			this.txtGuns.Location = new System.Drawing.Point(104, 69);
			this.txtGuns.Name = "txtGuns";
			this.txtGuns.Size = new System.Drawing.Size(500, 24);
			this.txtGuns.TabIndex = 5;
			this.txtGuns.Text = "txtGuns";
			this.ttBase.SetToolTip(this.txtGuns, "Guns Equipped");
			//
			//txtMissiles
			//
			this.txtMissiles.Location = new System.Drawing.Point(104, 41);
			this.txtMissiles.Name = "txtMissiles";
			this.txtMissiles.Size = new System.Drawing.Size(500, 24);
			this.txtMissiles.TabIndex = 3;
			this.txtMissiles.Text = "txtMissiles";
			this.ttBase.SetToolTip(this.txtMissiles, "Missile complement");
			//
			//txtAircraft
			//
			this.txtAircraft.Location = new System.Drawing.Point(104, 13);
			this.txtAircraft.Name = "txtAircraft";
			this.txtAircraft.Size = new System.Drawing.Size(500, 24);
			this.txtAircraft.TabIndex = 1;
			this.txtAircraft.Text = "txtAircraft";
			this.ttBase.SetToolTip(this.txtAircraft, "Number of Aircraft carried");
			//
			//lblClassification
			//
			this.lblClassification.AutoSize = true;
			this.lblClassification.Location = new System.Drawing.Point(12, 79);
			this.lblClassification.Name = "lblClassification";
			this.lblClassification.Size = new System.Drawing.Size(92, 19);
			this.lblClassification.TabIndex = 4;
			this.lblClassification.Text = "Classification";
			//
			//cbClassification
			//
			this.cbClassification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbClassification.Location = new System.Drawing.Point(108, 76);
			this.cbClassification.Name = "cbClassification";
			this.cbClassification.Size = new System.Drawing.Size(136, 24);
			this.cbClassification.TabIndex = 5;
			this.cbClassification.Tag = "Required";
			this.ttBase.SetToolTip(this.cbClassification, "Classification of Class/Ship (i.e. Carrier, Cruiser, Destroyer, etc.)");
			//
			//txtClassDesc
			//
			this.txtClassDesc.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtClassDesc.Location = new System.Drawing.Point(252, 76);
			this.txtClassDesc.Name = "txtClassDesc";
			this.txtClassDesc.ReadOnly = true;
			this.txtClassDesc.Size = new System.Drawing.Size(356, 23);
			this.txtClassDesc.TabIndex = 6;
			this.txtClassDesc.Tag = "";
			this.txtClassDesc.Text = "txtClassDesc";
			this.ttBase.SetToolTip(this.txtClassDesc, "Description of Class/Ship");
			//
			//tpDescription
			//
			this.tpDescription.Controls.Add(this.rtfDescription);
			this.tpDescription.Location = new System.Drawing.Point(4, 25);
			this.tpDescription.Name = "tpDescription";
			this.tpDescription.Size = new System.Drawing.Size(652, 270);
			this.tpDescription.TabIndex = 4;
			this.tpDescription.Text = "Description";
			//
			//rtfDescription
			//
			this.rtfDescription.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfDescription.Location = new System.Drawing.Point(4, 4);
			this.rtfDescription.Name = "rtfDescription";
			this.rtfDescription.Size = new System.Drawing.Size(644, 263);
			this.rtfDescription.TabIndex = 0;
			this.rtfDescription.Text = "rtfDescription";
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
			this.txtClassification.Location = new System.Drawing.Point(108, 77);
			this.txtClassification.Name = "txtClassification";
			this.txtClassification.ReadOnly = true;
			this.txtClassification.Size = new System.Drawing.Size(136, 23);
			this.txtClassification.TabIndex = 81;
			this.txtClassification.Tag = "Ignore";
			this.txtClassification.Text = "txtClassification";
			this.ttBase.SetToolTip(this.txtClassification, "Description of Class/Ship");
			this.txtClassification.Visible = false;
			//
			//frmClasses
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 419);
			this.Name = "frmClasses";
			this.Text = "frmClassifications";
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			this.tpNotes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			this.tpGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.gbGeneral.ResumeLayout(false);
			this.tpCharacteristics.ResumeLayout(false);
			this.gbCharacteristics.ResumeLayout(false);
			this.tpWeapons.ResumeLayout(false);
			this.gbWeapons.ResumeLayout(false);
			this.tpDescription.ResumeLayout(false);
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
			BindControl(cbClassification, mTCBase.MainDataView, "ClassificationID", ((clsClasses)mTCBase).Classifications, "Type", "ID");
			BindControl(txtClassification, mTCBase.MainDataView, "Classification");
			//BindControl(txtClassDesc, mTCBase.MainDataView, "ClassificationID", CType(mTCBase, clsClasses).Classifications, "Description", "Description")
			BindControl(txtYear, mTCBase.MainDataView, "Year");
			//Characteristics...
			BindControl(txtDisplacement, mTCBase.MainDataView, "Displacement");
			BindControl(txtLength, mTCBase.MainDataView, "Length");
			BindControl(txtBeam, mTCBase.MainDataView, "Beam");
			BindControl(txtDraft, mTCBase.MainDataView, "Draft");
			BindControl(txtPropulsion, mTCBase.MainDataView, "Propulsion");
			BindControl(txtBoilers, mTCBase.MainDataView, "Boilers");
			BindControl(txtManning, mTCBase.MainDataView, "Manning");
			BindControl(txtSpeed, mTCBase.MainDataView, "Speed");
			//Weapons...
			BindControl(txtAircraft, mTCBase.MainDataView, "Aircraft");
			BindControl(txtMissiles, mTCBase.MainDataView, "Missiles");
			BindControl(txtGuns, mTCBase.MainDataView, "Guns");
			BindControl(txtASW, mTCBase.MainDataView, "ASW Weapons");
			BindControl(txtRADARs, mTCBase.MainDataView, "Radars");
			BindControl(txtSONARs, mTCBase.MainDataView, "Sonars");
			BindControl(txtFireControl, mTCBase.MainDataView, "Fire Control");
			BindControl(txtEW, mTCBase.MainDataView, "EW");
			//Description...
			BindControl(rtfDescription, mTCBase.MainDataView, "Description");
			//Notes...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		#endregion
		#region "Event Handlers"
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmClasses.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				RemoveControlHandlers(txtName);
				txtName.Validating -= SetCaption;
				RemoveControlHandlers(cbClassification);
				cbClassification.Validating -= SetCaption;
				RemoveControlHandlers(txtClassDesc);
				RemoveControlHandlers(txtYear);
				//Characteristics...
				RemoveControlHandlers(txtDisplacement);
				RemoveControlHandlers(txtLength);
				RemoveControlHandlers(txtBeam);
				RemoveControlHandlers(txtDraft);
				RemoveControlHandlers(txtPropulsion);
				RemoveControlHandlers(txtBoilers);
				RemoveControlHandlers(txtManning);
				RemoveControlHandlers(txtSpeed);
				//Weapons...
				RemoveControlHandlers(txtAircraft);
				RemoveControlHandlers(txtMissiles);
				RemoveControlHandlers(txtGuns);
				RemoveControlHandlers(txtASW);
				RemoveControlHandlers(txtRADARs);
				RemoveControlHandlers(txtSONARs);
				RemoveControlHandlers(txtFireControl);
				RemoveControlHandlers(txtEW);
				//Description...
				RemoveControlHandlers(rtfDescription);
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
			const string EntryName = "frmClasses.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				SetupControlHandlers(txtName);
				txtName.Validating += SetCaption;
				SetupControlHandlers(cbClassification);
				cbClassification.Validating += SetCaption;
				SetupControlHandlers(txtClassDesc);
				SetupControlHandlers(txtYear);
				//Characteristics...
				SetupControlHandlers(txtDisplacement);
				SetupControlHandlers(txtLength);
				SetupControlHandlers(txtBeam);
				SetupControlHandlers(txtDraft);
				SetupControlHandlers(txtPropulsion);
				SetupControlHandlers(txtBoilers);
				SetupControlHandlers(txtManning);
				SetupControlHandlers(txtSpeed);
				//Weapons...
				SetupControlHandlers(txtAircraft);
				SetupControlHandlers(txtMissiles);
				SetupControlHandlers(txtGuns);
				SetupControlHandlers(txtASW);
				SetupControlHandlers(txtRADARs);
				SetupControlHandlers(txtSONARs);
				SetupControlHandlers(txtFireControl);
				SetupControlHandlers(txtEW);
				//Description...
				SetupControlHandlers(rtfDescription);
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

				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["Classification"]) ? bpeNullString : mTCBase.CurrentRow["Classification"] + " - ") + mTCBase.CurrentRow["Name"];

				//Populate hidden txtClassification to keep data current (albeit redundant)...
				this.txtClassification.Text = this.cbClassification.Text;

				if ((cbClassification.SelectedValue != null)) {
					//Populate txtClassDesc from cbClassification's DataSource...
					DataView dvClassifications = (DataView)this.cbClassification.DataSource;
					foreach (DataRow iRow in dvClassifications.Table.Rows) {
						if (iRow[cbClassification.ValueMember] == cbClassification.SelectedValue) {
							this.txtClassDesc.Text = (string)iRow["Description"];
							break; 
						}
					}
				}
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
