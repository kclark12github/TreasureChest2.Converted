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
//frmMusic.vb
//   Music Form...
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
//   10/23/09    Converted to VB.NET;
//   04/13/08    Added DatePurchased support;
//   03/24/09    Added Location DataCombo;
//   02/16/08    Introduced ssatMain_TabClick and modified vrsMain_MoveComplete to use new BindNotes to handle the migration of
//               Notes field from individual tables into a centralized [Notes] table where the field is read on-demand rather than
//               as part of the vrsMain recordset which over time has come to consume too much memory;
//   10/06/07    Corrected problems with context menus and rtfNotes;
//   08/05/03    Replaced pvCurrency controls with SIASCurrency;
//               Made Form minimize-able;
//   10/15/02    Started History;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCMusic
{
	public class frmMusic : frmTCStandard
	{
		const string myFormName = "frmMusic";
		public frmMusic(clsSupport objSupport, clsMusic objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
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
		public frmMusic() : base()
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
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.Label lblYear;
		internal System.Windows.Forms.Label lblTitle;
		internal System.Windows.Forms.Label lblArtist;
		internal System.Windows.Forms.Label lblMedia;
		protected internal System.Windows.Forms.ComboBox cbType;
		//Protected Friend WithEvents chkLP As System.Windows.Forms.CheckBox
		//Protected Friend WithEvents chkCassette As System.Windows.Forms.CheckBox
		//Protected Friend WithEvents chkMP3 As System.Windows.Forms.CheckBox
		//Protected Friend WithEvents chkCD As System.Windows.Forms.CheckBox
		protected internal System.Windows.Forms.TextBox txtYear;
		protected internal System.Windows.Forms.ComboBox cbArtist;
		protected internal System.Windows.Forms.ComboBox cbMedia;
		protected internal System.Windows.Forms.TextBox txtTitle;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.cbType = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.txtYear = new System.Windows.Forms.TextBox();
			this.cbMedia = new System.Windows.Forms.ComboBox();
			this.lblMedia = new System.Windows.Forms.Label();
			this.cbArtist = new System.Windows.Forms.ComboBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblYear = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblArtist = new System.Windows.Forms.Label();
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
			this.btnLast.Location = new System.Drawing.Point(643, 361);
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 361);
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(616, 361);
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 361);
			//
			//sbpPosition
			//
			this.sbpPosition.Text = "";
			this.sbpPosition.Width = 10;
			//
			//sbpStatus
			//
			this.sbpStatus.Text = "";
			this.sbpStatus.Width = 10;
			//
			//sbpMessage
			//
			this.sbpMessage.Text = "";
			this.sbpMessage.Width = 547;
			//
			//sbpTime
			//
			this.sbpTime.Text = "4:13 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 401);
			//
			//lblPurchased
			//
			this.lblPurchased.Location = new System.Drawing.Point(432, 234);
			this.lblPurchased.TabIndex = 18;
			//
			//lblInventoried
			//
			this.lblInventoried.Location = new System.Drawing.Point(24, 234);
			this.lblInventoried.TabIndex = 20;
			//
			//lblLocation
			//
			this.lblLocation.Location = new System.Drawing.Point(18, 207);
			this.lblLocation.TabIndex = 14;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Location = new System.Drawing.Point(8, 176);
			this.lblAlphaSort.TabIndex = 16;
			//
			//lblPrice
			//
			this.lblPrice.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblPrice.Location = new System.Drawing.Point(272, 234);
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(516, 398);
			this.btnOK.TabIndex = 15;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(593, 398);
			this.btnExit.TabIndex = 17;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(593, 398);
			this.btnCancel.TabIndex = 16;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 434);
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
			this.chkWishList.Location = new System.Drawing.Point(180, 107);
			this.chkWishList.TabIndex = 6;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Location = new System.Drawing.Point(512, 232);
			this.dtpPurchased.TabIndex = 16;
			this.dtpPurchased.Tag = "Nulls,Reset";
			//
			//dtpInventoried
			//
			this.dtpInventoried.Location = new System.Drawing.Point(112, 232);
			this.dtpInventoried.TabIndex = 14;
			this.dtpInventoried.Tag = "Nulls,Reset";
			//
			//cbLocation
			//
			this.cbLocation.Location = new System.Drawing.Point(88, 204);
			this.cbLocation.Size = new System.Drawing.Size(540, 24);
			this.cbLocation.TabIndex = 13;
			this.cbLocation.Tag = "Required";
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Location = new System.Drawing.Point(528, 96);
			this.txtAlphaSort.Size = new System.Drawing.Size(76, 23);
			this.txtAlphaSort.TabIndex = 10;
			//
			//pbGeneral
			//
			this.pbGeneral.Location = new System.Drawing.Point(500, 20);
			this.pbGeneral.Size = new System.Drawing.Size(125, 145);
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 361);
			this.txtCaption.Size = new System.Drawing.Size(548, 24);
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 402);
			this.txtID.Text = "5";
			//
			//rtfNotes
			//
			this.rtfNotes.Size = new System.Drawing.Size(648, 287);
			this.rtfNotes.TabIndex = 14;
			//
			//txtPrice
			//
			this.txtPrice.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.txtPrice.Location = new System.Drawing.Point(316, 232);
			this.txtPrice.Size = new System.Drawing.Size(96, 23);
			this.txtPrice.TabIndex = 15;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Location = new System.Drawing.Point(88, 173);
			this.cbAlphaSort.Size = new System.Drawing.Size(540, 24);
			this.cbAlphaSort.TabIndex = 12;
			this.cbAlphaSort.Tag = "Required,UPPER";
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.cbType);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.lblMedia);
			this.gbGeneral.Controls.Add(this.cbMedia);
			this.gbGeneral.Controls.Add(this.txtYear);
			this.gbGeneral.Controls.Add(this.cbArtist);
			this.gbGeneral.Controls.Add(this.txtTitle);
			this.gbGeneral.Controls.Add(this.lblYear);
			this.gbGeneral.Controls.Add(this.lblTitle);
			this.gbGeneral.Controls.Add(this.lblArtist);
			this.gbGeneral.Size = new System.Drawing.Size(636, 291);
			this.gbGeneral.Controls.SetChildIndex(this.lblValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtValue, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblVerified, 0);
			this.gbGeneral.Controls.SetChildIndex(this.hsbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral2, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtPrice, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbLocation, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.chkWishList, 0);
			this.gbGeneral.Controls.SetChildIndex(this.pbGeneral, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblArtist, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblYear, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtTitle, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbArtist, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtYear, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbMedia, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblMedia, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.cbAlphaSort, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpInventoried, 0);
			this.gbGeneral.Controls.SetChildIndex(this.dtpPurchased, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblInventoried, 0);
			//
			//tcMain
			//
			this.tcMain.Size = new System.Drawing.Size(660, 317);
			//
			//tpGeneral
			//
			this.tpGeneral.Size = new System.Drawing.Size(652, 288);
			//
			//tpNotes
			//
			this.tpNotes.Size = new System.Drawing.Size(652, 288);
			//
			//hsbGeneral
			//
			this.hsbGeneral.Location = new System.Drawing.Point(500, 112);
			this.hsbGeneral.Size = new System.Drawing.Size(125, 17);
			this.hsbGeneral.Visible = false;
			//
			//pbGeneral2
			//
			this.pbGeneral2.Location = new System.Drawing.Point(500, 20);
			this.pbGeneral2.Size = new System.Drawing.Size(125, 145);
			this.pbGeneral2.Visible = false;
			//
			//lblValue
			//
			this.lblValue.Location = new System.Drawing.Point(52, 138);
			this.lblValue.Visible = true;
			//
			//txtValue
			//
			this.txtValue.Location = new System.Drawing.Point(100, 136);
			this.txtValue.TabIndex = 10;
			this.txtValue.Visible = true;
			//
			//lblVerified
			//
			this.lblVerified.Location = new System.Drawing.Point(228, 138);
			this.lblVerified.Visible = true;
			//
			//dtpVerified
			//
			this.dtpVerified.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpVerified.Location = new System.Drawing.Point(292, 136);
			this.dtpVerified.TabIndex = 11;
			this.dtpVerified.Visible = true;
			//
			//cbType
			//
			this.cbType.Location = new System.Drawing.Point(52, 80);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(240, 24);
			this.cbType.TabIndex = 3;
			this.cbType.Tag = "Required";
			this.cbType.Text = "cbType";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblType.Location = new System.Drawing.Point(8, 83);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(41, 16);
			this.lblType.TabIndex = 8;
			this.lblType.Text = "Type";
			//
			//txtYear
			//
			this.txtYear.Location = new System.Drawing.Point(52, 108);
			this.txtYear.Name = "txtYear";
			this.txtYear.Size = new System.Drawing.Size(64, 23);
			this.txtYear.TabIndex = 5;
			this.txtYear.Tag = "Required";
			this.txtYear.Text = "txtYear";
			//
			//cbMedia
			//
			this.cbMedia.Location = new System.Drawing.Point(350, 80);
			this.cbMedia.Name = "cbMedia";
			this.cbMedia.Size = new System.Drawing.Size(122, 24);
			this.cbMedia.TabIndex = 4;
			this.cbMedia.Tag = "Required";
			this.cbMedia.Text = "cbMedia";
			//
			//lblMedia
			//
			this.lblMedia.AutoSize = true;
			this.lblMedia.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblMedia.Location = new System.Drawing.Point(298, 83);
			this.lblMedia.Name = "lblMedia";
			this.lblMedia.Size = new System.Drawing.Size(46, 16);
			this.lblMedia.TabIndex = 8;
			this.lblMedia.Text = "Media";
			//
			//cbArtist
			//
			this.cbArtist.Location = new System.Drawing.Point(52, 24);
			this.cbArtist.Name = "cbArtist";
			this.cbArtist.Size = new System.Drawing.Size(420, 24);
			this.cbArtist.TabIndex = 1;
			this.cbArtist.Tag = "Required";
			this.cbArtist.Text = "cbArtist";
			//
			//txtTitle
			//
			this.txtTitle.Location = new System.Drawing.Point(52, 52);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(420, 23);
			this.txtTitle.TabIndex = 2;
			this.txtTitle.Tag = "Required";
			this.txtTitle.Text = "txtTitle";
			//
			//lblYear
			//
			this.lblYear.AutoSize = true;
			this.lblYear.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblYear.Location = new System.Drawing.Point(8, 110);
			this.lblYear.Name = "lblYear";
			this.lblYear.Size = new System.Drawing.Size(38, 16);
			this.lblYear.TabIndex = 4;
			this.lblYear.Text = "Year";
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblTitle.Location = new System.Drawing.Point(8, 54);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(37, 16);
			this.lblTitle.TabIndex = 2;
			this.lblTitle.Text = "Title";
			//
			//lblArtist
			//
			this.lblArtist.AutoSize = true;
			this.lblArtist.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblArtist.Location = new System.Drawing.Point(4, 28);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.Size = new System.Drawing.Size(44, 16);
			this.lblArtist.TabIndex = 0;
			this.lblArtist.Text = "Artist";
			//
			//frmMusic
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 456);
			this.MinimumSize = new System.Drawing.Size(684, 490);
			this.Name = "frmMusic";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmMusic";
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
			BindControl(cbArtist, mTCBase.MainDataView, "Artist", ((clsMusic)mTCBase).Artists, "Artist", "Artist");
			BindControl(cbMedia, mTCBase.MainDataView, "Media", ((clsMusic)mTCBase).Media, "Media", "Media");
			BindControl(cbLocation, mTCBase.MainDataView, "Location", ((clsMusic)mTCBase).Locations, "Location", "Location");
			BindControl(dtpInventoried, mTCBase.MainDataView, "DateInventoried");
			BindControl(cbType, mTCBase.MainDataView, "Type", ((clsMusic)mTCBase).Types, "Type", "Type");
			BindControl(txtTitle, mTCBase.MainDataView, "Title");
			BindControl(txtYear, mTCBase.MainDataView, "Year");
			BindControl(txtPrice, mTCBase.MainDataView, "Price");
			BindControl(dtpPurchased, mTCBase.MainDataView, "DatePurchased");
			BindControl(txtValue, mTCBase.MainDataView, "Value");
			BindControl(dtpVerified, mTCBase.MainDataView, "DateVerified");
			//BindControl(chkMP3, mTCBase.MainDataView, "MP3")
			//BindControl(chkCD, mTCBase.MainDataView, "CD")
			//BindControl(chkCassette, mTCBase.MainDataView, "CS")
			//BindControl(chkLP, mTCBase.MainDataView, "LP")
			//BindControl(txtAlphaSort, mTCBase.MainDataView, "AlphaSort")
			BindControl(cbAlphaSort, mTCBase.MainDataView, "AlphaSort");
			//Note that this guy is Simple-Bound...
			BindControl(chkWishList, mTCBase.MainDataView, "WishList");
			BindControl(rtfNotes, mTCBase.MainDataView, "Notes");
		}
		public void DefaultAlphaSort(System.Windows.Forms.ComboBox cbAlphaSort, string Artist, string Title, string MusicType, string YearReleased)
		{
			Artist = Artist.ToUpper();
			if (Artist.StartsWith("THE "))
				Artist = Artist.Substring("THE ".Length) + ", THE";
			if (Artist.StartsWith("A "))
				Artist = Artist.Substring("A ".Length) + ", A";
			if (Artist.StartsWith("AN "))
				Artist = Artist.Substring("AN ".Length) + ", AN";
			Title = Title.ToUpper();
			if (Title.StartsWith("THE "))
				Title = Title.Substring("THE ".Length) + ", THE";
			if (Title.StartsWith("A "))
				Title = Title.Substring("A ".Length) + ", A";
			if (Title.StartsWith("AN "))
				Title = Title.Substring("AN ".Length) + ", AN";
			MusicType = MusicType.ToUpper();

			if (cbAlphaSort.Items.Count > 0)
				cbAlphaSort.Items.Clear();
			base.cbAddItem(cbAlphaSort, cbAlphaSort.Text);

			//Patterns:
			//   {Type}: {Artist}: {Year}; {Title}
			//{Artist} can be unaltered, "LastName, FirstName", LastName-Only, "'The'-Adjusted", if "Various Artists" skipped...
			switch (MusicType) {
				case "SOUNDTRACK":
					base.cbAddItem(cbAlphaSort, string.Format("{0}: {1}; {2}", MusicType, Title, YearReleased));
					break;
				default:
					base.cbAddItem(cbAlphaSort, MusicType + ": " + string.Format("{0}: {1}; {2}", Artist, YearReleased, Title));
					if (Artist.StartsWith("VARIOUS ARTISTS")) {
						base.cbAddItem(cbAlphaSort, MusicType + ": " + string.Format("{0}; {1}", YearReleased, Title));
					} else {
						string[] words = Artist.Split(" ".ToCharArray());
						//Let's only deal with two-word patterns for now...
						if (words.Length == 2) {
							string adjArtist = string.Format("{0}, {1}", words[1], words[0]);
							base.cbAddItem(cbAlphaSort, MusicType + ": " + string.Format("{0}: {1}; {2}", adjArtist, YearReleased, Title));
							base.cbAddItem(cbAlphaSort, MusicType + ": " + string.Format("{0}: {1}; {2}", words[1], YearReleased, Title));
						}
					}
					if (Artist.IndexOf("TIME LIFE") >= 0 || Artist.IndexOf("TIMELIFE") >= 0)
						base.cbAddItem(cbAlphaSort, MusicType + ": " + string.Format("{0}: TIME LIFE; {1}", YearReleased, Title));
					break;
			}
		}
		#endregion
		#region "Event Handlers"
		protected override void ActionModeChange(object sender, ActionModeChangeEventArgs e)
		{
			base.ActionModeChange(sender, e);
			switch (e.newMode) {
				case clsTCBase.ActionModeEnum.modeDisplay:
				case clsTCBase.ActionModeEnum.modeDelete:
					break;
				default:
					this.cbArtist.Focus();
					break;
			}
		}
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
		private void DefaultAlphaSort(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				ComboBox cb = (ComboBox)sender;
				//Give the user options...
				this.DefaultAlphaSort(cb, cbArtist.Text, txtTitle.Text, cbType.Text, txtYear.Text);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmMusic.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating -= SetCaption;
				cbAlphaSort.Enter -= DefaultAlphaSort;
				RemoveControlHandlers(cbArtist);
				RemoveControlHandlers(txtTitle);
				RemoveControlHandlers(txtYear);
				RemoveControlHandlers(cbType);
				RemoveControlHandlers(cbMedia);
				chkWishList.CheckStateChanged -= chkWishList_CheckStateChanged;
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
			const string EntryName = "frmMusic.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				cbAlphaSort.Validating += SetCaption;
				cbAlphaSort.Enter += DefaultAlphaSort;
				SetupControlHandlers(cbArtist);
				SetupControlHandlers(txtTitle);
				SetupControlHandlers(txtYear);
				SetupControlHandlers(cbType);
				SetupControlHandlers(cbMedia);
				chkWishList.CheckStateChanged += chkWishList_CheckStateChanged;
				chkWishList_CheckStateChanged(chkWishList, null);
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
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				if ((sender != null))
					base.epBase.SetError((Control)sender, ex.Message);
				e.Cancel = true;
			}
		}
		//#Region "WhereAmI"
		//    Private Sub Control_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbArtist.Enter, cbType.Enter, chkCassette.Enter, chkCD.Enter, chkLP.Enter, chkMP3.Enter, lblArtist.Enter, lblTitle.Enter, lblType.Enter, lblYear.Enter, txtTitle.Enter, txtYear.Enter
		//        Debug.WriteLine(String.Format("{0}:={1}.Enter()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbArtist.Leave, cbType.Leave, chkCassette.Leave, chkCD.Leave, chkLP.Leave, chkMP3.Leave, lblArtist.Leave, lblTitle.Leave, lblType.Leave, lblYear.Leave, txtTitle.Leave, txtYear.Leave
		//        Debug.WriteLine(String.Format("{0}:={1}.Leave()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbArtist.GotFocus, cbType.GotFocus, chkCassette.GotFocus, chkCD.GotFocus, chkLP.GotFocus, chkMP3.GotFocus, lblArtist.GotFocus, lblTitle.GotFocus, lblType.GotFocus, lblYear.GotFocus, txtTitle.GotFocus, txtYear.GotFocus
		//        Debug.WriteLine(String.Format("{0}:={1}.GotFocus()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbArtist.LostFocus, cbType.LostFocus, chkCassette.LostFocus, chkCD.LostFocus, chkLP.LostFocus, chkMP3.LostFocus, lblArtist.LostFocus, lblTitle.LostFocus, lblType.LostFocus, lblYear.LostFocus, txtTitle.LostFocus, txtYear.LostFocus
		//        Debug.WriteLine(String.Format("{0}:={1}.LostFocus()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Validating(ByVal sender As Object, ByVal e As CancelEventArgs) Handles cbArtist.Validating, cbType.Validating, chkCassette.Validating, chkCD.Validating, chkLP.Validating, chkMP3.Validating, lblArtist.Validating, lblTitle.Validating, lblType.Validating, lblYear.Validating, txtTitle.Validating, txtYear.Validating
		//        Debug.WriteLine(String.Format("{0}:={1}.Validating()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//    Private Sub Control_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbArtist.Validated, cbType.Validated, chkCassette.Validated, chkCD.Validated, chkLP.Validated, chkMP3.Validated, lblArtist.Validated, lblTitle.Validated, lblType.Validated, lblYear.Validated, txtTitle.Validated, txtYear.Validated
		//        Debug.WriteLine(String.Format("{0}:={1}.Validated()", "{" & sender.GetType.Name & "}", CType(sender, Control).Name))
		//        FindFocus(Me)
		//    End Sub
		//#End Region
		#endregion
	}
}
