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
//frmBlueAngelsHistory.vb
//   Blue Angels History Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCHobby
{
	public class frmBlueAngelsHistory : frmTCStandard
	{
		const string myFormName = "frmBlueAngelsHistory";
		public frmBlueAngelsHistory(clsSupport objSupport, clsBlueAngelsHistory objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
				tpKits,
				tpDecalSets,
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
		public frmBlueAngelsHistory() : base()
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
		internal System.Windows.Forms.TabPage tpImage;
		internal System.Windows.Forms.TextBox txtAircraftType;
		internal System.Windows.Forms.Label lblAircraftType;
		internal System.Windows.Forms.TextBox txtDates;
		internal System.Windows.Forms.Label lblDates;
		internal System.Windows.Forms.TabPage tpKits;
		internal System.Windows.Forms.TabPage tpDecalSets;
		internal System.Windows.Forms.GroupBox gbKits;
		internal System.Windows.Forms.TextBox txtKits;
		internal System.Windows.Forms.GroupBox gbDecalSets;
		internal System.Windows.Forms.TextBox txtDecalSets;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtAircraftType = new System.Windows.Forms.TextBox();
			this.lblAircraftType = new System.Windows.Forms.Label();
			this.tpImage = new System.Windows.Forms.TabPage();
			this.txtDates = new System.Windows.Forms.TextBox();
			this.lblDates = new System.Windows.Forms.Label();
			this.tpKits = new System.Windows.Forms.TabPage();
			this.tpDecalSets = new System.Windows.Forms.TabPage();
			this.gbKits = new System.Windows.Forms.GroupBox();
			this.txtKits = new System.Windows.Forms.TextBox();
			this.gbDecalSets = new System.Windows.Forms.GroupBox();
			this.txtDecalSets = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			this.tpNotes.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.gbGeneral.SuspendLayout();
			this.tpKits.SuspendLayout();
			this.tpDecalSets.SuspendLayout();
			this.gbKits.SuspendLayout();
			this.gbDecalSets.SuspendLayout();
			this.SuspendLayout();
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 212);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.TabIndex = 13;
			this.lblPurchased.Visible = false;
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
			//sbpStatus
			//
			this.sbpStatus.Text = "Filter Off";
			this.sbpStatus.Width = 74;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 476;
			//
			//sbpTime
			//
			this.sbpTime.Text = "10:31 PM";
			this.sbpTime.Width = 79;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 367);
			this.lblID.Name = "lblID";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(508, 364);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 20;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(589, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 22;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 212);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.TabIndex = 15;
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 216);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.TabIndex = 7;
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(16, 212);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.TabIndex = 11;
			this.lblAlphaSort.Visible = false;
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 216);
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
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(92, 212);
			this.txtAlphaSort.Name = "txtAlphaSort";
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(484, 212);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.TabIndex = 17;
			this.chkWishList.Visible = false;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 212);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.TabIndex = 14;
			this.dtpPurchased.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 212);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.TabIndex = 16;
			this.dtpInventoried.Visible = false;
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral.Visible = false;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 328);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(544, 24);
			this.txtCaption.TabIndex = 2;
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(648, 255);
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(648, 255);
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tpImage);
			this.tcMain.Controls.Add(this.tpKits);
			this.tcMain.Controls.Add(this.tpDecalSets);
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(656, 284);
			this.tcMain.Controls.SetChildIndex(this.tpDecalSets, 0);
			this.tcMain.Controls.SetChildIndex(this.tpKits, 0);
			this.tcMain.Controls.SetChildIndex(this.tpImage, 0);
			this.tcMain.Controls.SetChildIndex(this.tpNotes, 0);
			this.tcMain.Controls.SetChildIndex(this.tpGeneral, 0);
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtDates);
			this.gbGeneral.Controls.Add(this.lblDates);
			this.gbGeneral.Controls.Add(this.lblAircraftType);
			this.gbGeneral.Controls.Add(this.txtAircraftType);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(636, 244);
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
			this.gbGeneral.Controls.SetChildIndex(this.txtAircraftType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAircraftType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDates, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDates, 0);
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(96, 212);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(516, 24);
			this.cbAlphaSort.TabIndex = 12;
			this.cbAlphaSort.Visible = false;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 212);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.TabIndex = 10;
			this.txtPrice.Visible = false;
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(628, 203);
			this.ttBase.SetToolTip(this.rtfNotes, "Description of Class");
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 368);
			this.txtID.Name = "txtID";
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(92, 212);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.TabIndex = 8;
			this.cbLocation.Visible = false;
			//
			//txtAircraftType
			//
			this.txtAircraftType.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtAircraftType.Location = new System.Drawing.Point(108, 48);
			this.txtAircraftType.Name = "txtAircraftType";
			this.txtAircraftType.Size = new System.Drawing.Size(496, 23);
			this.txtAircraftType.TabIndex = 2;
			this.txtAircraftType.Tag = "Required";
			this.txtAircraftType.Text = "txtAircraftType";
			this.ttBase.SetToolTip(this.txtAircraftType, "Aircraft type flown during this period");
			//
			//lblAircraftType
			//
			this.lblAircraftType.AutoSize = true;
			this.lblAircraftType.Location = new System.Drawing.Point(9, 50);
			this.lblAircraftType.Name = "lblAircraftType";
			this.lblAircraftType.Size = new System.Drawing.Size(91, 19);
			this.lblAircraftType.TabIndex = 0;
			this.lblAircraftType.Text = "Aircraft Type";
			//
			//tpImage
			//
			this.tpImage.Location = new System.Drawing.Point(4, 25);
			this.tpImage.Name = "tpImage";
			this.tpImage.Size = new System.Drawing.Size(648, 255);
			this.tpImage.TabIndex = 5;
			this.tpImage.Text = "Image";
			//
			//txtDates
			//
			this.txtDates.Location = new System.Drawing.Point(108, 20);
			this.txtDates.Name = "txtDates";
			this.txtDates.Size = new System.Drawing.Size(224, 23);
			this.txtDates.TabIndex = 1;
			this.txtDates.Tag = "Required";
			this.txtDates.Text = "txtDates";
			this.ttBase.SetToolTip(this.txtDates, "Period in which this aircraft was in service with the Blue Angels");
			//
			//lblDates
			//
			this.lblDates.AutoSize = true;
			this.lblDates.Location = new System.Drawing.Point(57, 22);
			this.lblDates.Name = "lblDates";
			this.lblDates.Size = new System.Drawing.Size(43, 19);
			this.lblDates.TabIndex = 4;
			this.lblDates.Text = "Dates";
			//
			//tpKits
			//
			this.tpKits.Controls.Add(this.gbKits);
			this.tpKits.Location = new System.Drawing.Point(4, 25);
			this.tpKits.Name = "tpKits";
			this.tpKits.Size = new System.Drawing.Size(648, 255);
			this.tpKits.TabIndex = 6;
			this.tpKits.Text = "Kits";
			//
			//tpDecalSets
			//
			this.tpDecalSets.Controls.Add(this.gbDecalSets);
			this.tpDecalSets.Location = new System.Drawing.Point(4, 25);
			this.tpDecalSets.Name = "tpDecalSets";
			this.tpDecalSets.Size = new System.Drawing.Size(648, 255);
			this.tpDecalSets.TabIndex = 7;
			this.tpDecalSets.Text = "Decal Sets";
			//
			//gbKits
			//
			this.gbKits.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbKits.Controls.Add(this.txtKits);
			this.gbKits.Location = new System.Drawing.Point(4, 4);
			this.gbKits.Name = "gbKits";
			this.gbKits.Size = new System.Drawing.Size(640, 244);
			this.gbKits.TabIndex = 0;
			this.gbKits.TabStop = false;
			//
			//txtKits
			//
			this.txtKits.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtKits.Location = new System.Drawing.Point(4, 16);
			this.txtKits.Multiline = true;
			this.txtKits.Name = "txtKits";
			this.txtKits.Size = new System.Drawing.Size(632, 224);
			this.txtKits.TabIndex = 0;
			this.txtKits.Text = "txtKits";
			this.ttBase.SetToolTip(this.txtKits, "Model Kits commercially available for this aircraft");
			//
			//gbDecalSets
			//
			this.gbDecalSets.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.gbDecalSets.Controls.Add(this.txtDecalSets);
			this.gbDecalSets.Location = new System.Drawing.Point(4, 4);
			this.gbDecalSets.Name = "gbDecalSets";
			this.gbDecalSets.Size = new System.Drawing.Size(640, 244);
			this.gbDecalSets.TabIndex = 1;
			this.gbDecalSets.TabStop = false;
			//
			//txtDecalSets
			//
			this.txtDecalSets.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDecalSets.Location = new System.Drawing.Point(4, 19);
			this.txtDecalSets.Multiline = true;
			this.txtDecalSets.Name = "txtDecalSets";
			this.txtDecalSets.Size = new System.Drawing.Size(632, 221);
			this.txtDecalSets.TabIndex = 0;
			this.txtDecalSets.Text = "txtDecalSets";
			this.ttBase.SetToolTip(this.txtDecalSets, "Decal Sets commercially available for kits for this aircraft");
			//
			//frmBlueAngelsHistory
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 419);
			this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
			this.Name = "frmBlueAngelsHistory";
			this.Text = "frmBlueAngelsHistory";
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).EndInit();
			this.tpNotes.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.gbGeneral.ResumeLayout(false);
			this.tpKits.ResumeLayout(false);
			this.tpDecalSets.ResumeLayout(false);
			this.gbKits.ResumeLayout(false);
			this.gbDecalSets.ResumeLayout(false);
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
			BindControl(txtAircraftType, mTCBase.MainDataView, "Aircraft Type");
			BindControl(txtDates, mTCBase.MainDataView, "Dates");
			//Kits...
			BindControl(txtKits, mTCBase.MainDataView, "Kits");
			//Decal Sets...
			BindControl(txtDecalSets, mTCBase.MainDataView, "Decal Sets");
			//Notes...
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
			//Image...
		}
		#endregion
		#region "Event Handlers"
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmBlueAngelsHistory.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				RemoveControlHandlers(txtDates);
				txtDates.Validating -= SetCaption;
				RemoveControlHandlers(txtAircraftType);
				txtAircraftType.Validating -= SetCaption;
				//Kits...
				RemoveControlHandlers(txtKits);
				//Decal Sets...
				RemoveControlHandlers(txtDecalSets);
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
			const string EntryName = "frmBlueAngelsHistory.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				//General
				SetupControlHandlers(txtDates);
				txtDates.Validating += SetCaption;
				SetupControlHandlers(txtAircraftType);
				txtAircraftType.Validating += SetCaption;
				//Kits...
				SetupControlHandlers(txtKits);
				//Decal Sets...
				SetupControlHandlers(txtDecalSets);
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
				this.txtCaption.Text = (Information.IsDBNull(mTCBase.CurrentRow["Dates"]) ? bpeNullString : mTCBase.CurrentRow["Dates"] + " ") + (Information.IsDBNull(mTCBase.CurrentRow["Aircraft Type"]) ? bpeNullString : mTCBase.CurrentRow["Aircraft Type"]);
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
