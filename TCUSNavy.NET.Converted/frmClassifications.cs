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
//frmClassifications.vb
//   USN Classifications Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/18/16    Reworked to reflect architectural changes;
//   12/03/15    Corrected implementation of Form Load and Closing event-handlers;
//   10/19/14    Upgraded to VS2013;
//   07/25/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCUSNavy
{
	public class frmClassifications : frmTCStandard
	{
		const string myFormName = "frmClassifications";
		public frmClassifications(clsSupport objSupport, clsClassifications objBase, Form objParent = null, string Caption = null) : base(myFormName, objSupport, objBase, Caption, objParent)
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			//Add any initialization after the InitializeComponent() call
			this.tcMain.Controls.Remove(this.tpNotes);

			this.Text = Strings.Replace(Caption, "&", bpeNullString);
			LoadDefaultImage();
			BindControls();
			EnableControls(false);

			Icon = mTCBase.Icon;
			MinimumSize = new Size(Width, Height);
			ReportPath = mTCBase.ReportPath;
		}
		#region " Windows Form Designer generated code "
		public frmClassifications() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			this.tcMain.Controls.Remove(this.tpNotes);
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
		internal System.Windows.Forms.TextBox txtType;
		internal System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.TextBox txtDescription;
		internal System.Windows.Forms.Label lblDescription;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.txtType = new System.Windows.Forms.TextBox();
			this.lblType = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpFilter).BeginInit();
			this.gbGeneral.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tpNotes.SuspendLayout();
			this.SuspendLayout();
			//
			//btnLast
			//
			this.btnLast.Location = new System.Drawing.Point(643, 165);
			this.btnLast.Name = "btnLast";
			//
			//btnFirst
			//
			this.btnFirst.Location = new System.Drawing.Point(8, 165);
			this.btnFirst.Name = "btnFirst";
			//
			//btnNext
			//
			this.btnNext.Location = new System.Drawing.Point(616, 165);
			this.btnNext.Name = "btnNext";
			//
			//btnPrev
			//
			this.btnPrev.Location = new System.Drawing.Point(36, 165);
			this.btnPrev.Name = "btnPrev";
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
			this.sbpMessage.Width = 484;
			//
			//sbpTime
			//
			this.sbpTime.Text = "8:18 PM";
			this.sbpTime.Width = 71;
			//
			//lblID
			//
			this.lblID.Location = new System.Drawing.Point(12, 204);
			this.lblID.Name = "lblID";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(512, 201);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 20;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(593, 201);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 22;
			//
			//lblPurchased
			//
			this.lblPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblPurchased.Location = new System.Drawing.Point(20, 189);
			this.lblPurchased.Name = "lblPurchased";
			this.lblPurchased.Visible = false;
			//
			//pbGeneral
			//
			this.pbGeneral.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral.Name = "pbGeneral";
			this.pbGeneral.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral.Visible = false;
			//
			//lblInventoried
			//
			this.lblInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblInventoried.Location = new System.Drawing.Point(256, 189);
			this.lblInventoried.Name = "lblInventoried";
			this.lblInventoried.Visible = false;
			//
			//lblLocation
			//
			this.lblLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblLocation.Location = new System.Drawing.Point(20, 125);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Visible = false;
			//
			//lblAlphaSort
			//
			this.lblAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.lblAlphaSort.Location = new System.Drawing.Point(16, 156);
			this.lblAlphaSort.Name = "lblAlphaSort";
			this.lblAlphaSort.Visible = false;
			//
			//btnExit
			//
			this.btnExit.Location = new System.Drawing.Point(593, 201);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 21;
			//
			//sbpMode
			//
			this.sbpMode.Text = "";
			this.sbpMode.Width = 10;
			//
			//chkWishList
			//
			this.chkWishList.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.chkWishList.Location = new System.Drawing.Point(484, 186);
			this.chkWishList.Name = "chkWishList";
			this.chkWishList.Visible = false;
			//
			//dtpPurchased
			//
			this.dtpPurchased.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpPurchased.Location = new System.Drawing.Point(100, 187);
			this.dtpPurchased.Name = "dtpPurchased";
			this.dtpPurchased.Visible = false;
			//
			//dtpInventoried
			//
			this.dtpInventoried.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.dtpInventoried.Location = new System.Drawing.Point(340, 187);
			this.dtpInventoried.Name = "dtpInventoried";
			this.dtpInventoried.Visible = false;
			//
			//sbpFilter
			//
			this.sbpFilter.Text = "";
			this.sbpFilter.Width = 10;
			//
			//cbLocation
			//
			this.cbLocation.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbLocation.Location = new System.Drawing.Point(96, 122);
			this.cbLocation.Name = "cbLocation";
			this.cbLocation.Size = new System.Drawing.Size(516, 24);
			this.cbLocation.Visible = false;
			//
			//txtCaption
			//
			this.txtCaption.Location = new System.Drawing.Point(64, 165);
			this.txtCaption.Name = "txtCaption";
			this.txtCaption.Size = new System.Drawing.Size(548, 24);
			//
			//lblPrice
			//
			this.lblPrice.Location = new System.Drawing.Point(252, 125);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.Visible = false;
			//
			//txtID
			//
			this.txtID.Location = new System.Drawing.Point(40, 205);
			this.txtID.Name = "txtID";
			//
			//rtfNotes
			//
			this.rtfNotes.Name = "rtfNotes";
			this.rtfNotes.Size = new System.Drawing.Size(644, 84);
			this.rtfNotes.TabIndex = 19;
			//
			//txtPrice
			//
			this.txtPrice.Location = new System.Drawing.Point(296, 123);
			this.txtPrice.Name = "txtPrice";
			this.txtPrice.Visible = false;
			//
			//cbAlphaSort
			//
			this.cbAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.cbAlphaSort.Location = new System.Drawing.Point(96, 154);
			this.cbAlphaSort.Name = "cbAlphaSort";
			this.cbAlphaSort.Size = new System.Drawing.Size(516, 24);
			this.cbAlphaSort.Visible = false;
			//
			//gbGeneral
			//
			this.gbGeneral.Controls.Add(this.txtDescription);
			this.gbGeneral.Controls.Add(this.lblDescription);
			this.gbGeneral.Controls.Add(this.lblType);
			this.gbGeneral.Controls.Add(this.txtType);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(640, 77);
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
			this.gbGeneral.Controls.SetChildIndex(this.txtType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblType, 0);
			this.gbGeneral.Controls.SetChildIndex(this.lblDescription, 0);
			this.gbGeneral.Controls.SetChildIndex(this.txtDescription, 0);
			//
			//tcMain
			//
			this.tcMain.Name = "tcMain";
			this.tcMain.Size = new System.Drawing.Size(660, 121);
			//
			//txtAlphaSort
			//
			this.txtAlphaSort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.txtAlphaSort.Location = new System.Drawing.Point(96, 156);
			this.txtAlphaSort.Name = "txtAlphaSort";
			//
			//tpGeneral
			//
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(652, 92);
			//
			//pbGeneral2
			//
			this.pbGeneral2.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			this.pbGeneral2.Location = new System.Drawing.Point(616, 142);
			this.pbGeneral2.Name = "pbGeneral2";
			this.pbGeneral2.Size = new System.Drawing.Size(125, 67);
			this.pbGeneral2.Visible = false;
			//
			//tpNotes
			//
			this.tpNotes.Name = "tpNotes";
			this.tpNotes.Size = new System.Drawing.Size(652, 92);
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
			this.sbStatus.Location = new System.Drawing.Point(0, 232);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(676, 22);
			//
			//txtType
			//
			this.txtType.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtType.Location = new System.Drawing.Point(96, 20);
			this.txtType.Name = "txtType";
			this.txtType.Size = new System.Drawing.Size(120, 23);
			this.txtType.TabIndex = 1;
			this.txtType.Tag = "Required";
			this.txtType.Text = "txtType";
			//
			//lblType
			//
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(48, 22);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 19);
			this.lblType.TabIndex = 96;
			this.lblType.Text = "Type";
			//
			//txtDescription
			//
			this.txtDescription.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDescription.Location = new System.Drawing.Point(96, 49);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(532, 23);
			this.txtDescription.TabIndex = 3;
			this.txtDescription.Tag = "Required";
			this.txtDescription.Text = "txtDescription";
			//
			//lblDescription
			//
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(5, 51);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(80, 19);
			this.lblDescription.TabIndex = 98;
			this.lblDescription.Text = "Description";
			//
			//frmClassifications
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(676, 254);
			this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
			this.Name = "frmClassifications";
			this.Text = "frmClassifications";
			((System.ComponentModel.ISupportInitialize)this.sbpPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpMessage).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
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
			BindControl(txtType, mTCBase.MainDataView, "Type");
			BindControl(txtDescription, mTCBase.MainDataView, "Description");
		}
		#endregion
		#region "Event Handlers"
		protected override void Form_Closing(object sender, CancelEventArgs e)
		{
			const string EntryName = "frmClassifications.Form_Closing";
			try {
				base.Form_Closing(sender, e);
				//Remove handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				RemoveControlHandlers(txtType);
				txtType.Validating -= SetCaption;
				RemoveControlHandlers(txtDescription);
				txtDescription.Validating -= SetCaption;

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
			const string EntryName = "frmClassifications.Form_Load";
			try {
				base.Form_Load(sender, e);
				//Add handlers for non-frmStandard controls (or non-standard handlers for frmStandard controls)...
				SetupControlHandlers(txtType);
				txtType.Validating += SetCaption;
				SetupControlHandlers(txtDescription);
				txtDescription.Validating += SetCaption;

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
				string tempType = (Information.IsDBNull(mTCBase.CurrentRow["Type"]) ? bpeNullString : (string)mTCBase.CurrentRow["Type"]);
				string tempDescription = (Information.IsDBNull(mTCBase.CurrentRow["Description"]) ? bpeNullString : (string)mTCBase.CurrentRow["Description"]);
				string tempCaption = tempType + "; ";
				if (tempDescription.ToUpper() != "N/A" && tempDescription.ToUpper() != "UNKNOWN")
					tempCaption += tempDescription + " ";
				this.txtCaption.Text = tempCaption.ToUpper();
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
