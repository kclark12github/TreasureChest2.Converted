using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Implementation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
//frmError.vb
//   Error Display Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/19/14    Upgraded to VS2013;
//   11/11/09    Created in VB.NET;
//=================================================================================================================================
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	internal class frmError : frmTCBase
	{
		public frmError(clsTCBase objTCBase, Form objParent = null) : base(objTCBase.Support, "frmError", objParent)
		{
			Closing += Form_Closing;
			Resize += Form_Resize;
			mTCBase = objTCBase;
			mSupport = objTCBase.Support;
            if (TCBase.MyComputer.Screen.Bounds.Width > 640 && TCBase.MyComputer.Screen.Bounds.Height > 480) {
				this.MaximumSize = new Size(800, 600);
			} else {
				//Size the form to VGA Minimums...
				this.MaximumSize = new Size(640, 480);
			}

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			picIcon.Image = ImgIcons.Images[(int)ImagesEnum.vbExclamationImage];
			//Use as default...
		}
		#region " Windows Form Designer generated code "

		public frmError() : base()
		{
			Closing += Form_Closing;
			Resize += Form_Resize;

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
		private System.Windows.Forms.RichTextBox withEventsField_rtfMessage;
		internal System.Windows.Forms.RichTextBox rtfMessage {
			get { return withEventsField_rtfMessage; }
			set {
				if (withEventsField_rtfMessage != null) {
					withEventsField_rtfMessage.TextChanged -= rtfMessage_TextChanged;
				}
				withEventsField_rtfMessage = value;
				if (withEventsField_rtfMessage != null) {
					withEventsField_rtfMessage.TextChanged += rtfMessage_TextChanged;
				}
			}
		}
		public System.Windows.Forms.PictureBox picIcon;
		public System.Windows.Forms.Label lblA;
		private System.Windows.Forms.Button withEventsField_btnIgnore;
		public System.Windows.Forms.Button btnIgnore {
			get { return withEventsField_btnIgnore; }
			set {
				if (withEventsField_btnIgnore != null) {
					withEventsField_btnIgnore.Click -= btnIgnore_Click;
				}
				withEventsField_btnIgnore = value;
				if (withEventsField_btnIgnore != null) {
					withEventsField_btnIgnore.Click += btnIgnore_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnRetry;
		public System.Windows.Forms.Button btnRetry {
			get { return withEventsField_btnRetry; }
			set {
				if (withEventsField_btnRetry != null) {
					withEventsField_btnRetry.Click -= btnRetry_Click;
				}
				withEventsField_btnRetry = value;
				if (withEventsField_btnRetry != null) {
					withEventsField_btnRetry.Click += btnRetry_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnAbort;
		public System.Windows.Forms.Button btnAbort {
			get { return withEventsField_btnAbort; }
			set {
				if (withEventsField_btnAbort != null) {
					withEventsField_btnAbort.Click -= btnAbort_Click;
				}
				withEventsField_btnAbort = value;
				if (withEventsField_btnAbort != null) {
					withEventsField_btnAbort.Click += btnAbort_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnNo;
		public System.Windows.Forms.Button btnNo {
			get { return withEventsField_btnNo; }
			set {
				if (withEventsField_btnNo != null) {
					withEventsField_btnNo.Click -= btnNo_Click;
				}
				withEventsField_btnNo = value;
				if (withEventsField_btnNo != null) {
					withEventsField_btnNo.Click += btnNo_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnYes;
		public System.Windows.Forms.Button btnYes {
			get { return withEventsField_btnYes; }
			set {
				if (withEventsField_btnYes != null) {
					withEventsField_btnYes.Click -= btnYes_Click;
				}
				withEventsField_btnYes = value;
				if (withEventsField_btnYes != null) {
					withEventsField_btnYes.Click += btnYes_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCancel;
		public System.Windows.Forms.Button btnCancel {
			get { return withEventsField_btnCancel; }
			set {
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click -= btnCancel_Click;
				}
				withEventsField_btnCancel = value;
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click += btnCancel_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCopyToClipboard;
		public System.Windows.Forms.Button btnCopyToClipboard {
			get { return withEventsField_btnCopyToClipboard; }
			set {
				if (withEventsField_btnCopyToClipboard != null) {
					withEventsField_btnCopyToClipboard.Click -= btnCopyToClipboard_Click;
				}
				withEventsField_btnCopyToClipboard = value;
				if (withEventsField_btnCopyToClipboard != null) {
					withEventsField_btnCopyToClipboard.Click += btnCopyToClipboard_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnOK;
		public System.Windows.Forms.Button btnOK {
			get { return withEventsField_btnOK; }
			set {
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click -= btnOK_Click;
				}
				withEventsField_btnOK = value;
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click += btnOK_Click;
				}
			}
		}
		internal System.Windows.Forms.ImageList ImgIcons;
		public System.Windows.Forms.ToolTip ToolTip1;
		private System.Windows.Forms.LinkLabel withEventsField_lblDetails;
		internal System.Windows.Forms.LinkLabel lblDetails {
			get { return withEventsField_lblDetails; }
			set {
				if (withEventsField_lblDetails != null) {
					withEventsField_lblDetails.LinkClicked -= lblDetails_LinkClicked;
				}
				withEventsField_lblDetails = value;
				if (withEventsField_lblDetails != null) {
					withEventsField_lblDetails.LinkClicked += lblDetails_LinkClicked;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmError));
			this.rtfMessage = new System.Windows.Forms.RichTextBox();
			this.btnIgnore = new System.Windows.Forms.Button();
			this.btnRetry = new System.Windows.Forms.Button();
			this.btnAbort = new System.Windows.Forms.Button();
			this.btnNo = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCopyToClipboard = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblA = new System.Windows.Forms.Label();
			this.ImgIcons = new System.Windows.Forms.ImageList(this.components);
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lblDetails = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			//
			//rtfMessage
			//
			this.rtfMessage.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfMessage.AutoSize = true;
			this.rtfMessage.BackColor = System.Drawing.SystemColors.Control;
			this.rtfMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtfMessage.Location = new System.Drawing.Point(48, 4);
			this.rtfMessage.Name = "rtfMessage";
			this.rtfMessage.ReadOnly = true;
			this.rtfMessage.Size = new System.Drawing.Size(376, 36);
			this.rtfMessage.TabIndex = 22;
			this.rtfMessage.TabStop = false;
			this.rtfMessage.Text = "rtfMessage";
			//
			//btnIgnore
			//
			this.btnIgnore.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnIgnore.BackColor = System.Drawing.SystemColors.Control;
			this.btnIgnore.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnIgnore.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnIgnore.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnIgnore.Location = new System.Drawing.Point(268, 52);
			this.btnIgnore.Name = "btnIgnore";
			this.btnIgnore.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnIgnore.Size = new System.Drawing.Size(65, 25);
			this.btnIgnore.TabIndex = 21;
			this.btnIgnore.Text = "Ignore";
			//
			//btnRetry
			//
			this.btnRetry.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnRetry.BackColor = System.Drawing.SystemColors.Control;
			this.btnRetry.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnRetry.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnRetry.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnRetry.Location = new System.Drawing.Point(132, 52);
			this.btnRetry.Name = "btnRetry";
			this.btnRetry.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnRetry.Size = new System.Drawing.Size(65, 25);
			this.btnRetry.TabIndex = 20;
			this.btnRetry.Text = "Retry";
			//
			//btnAbort
			//
			this.btnAbort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnAbort.BackColor = System.Drawing.SystemColors.Control;
			this.btnAbort.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnAbort.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnAbort.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnAbort.Location = new System.Drawing.Point(200, 52);
			this.btnAbort.Name = "btnAbort";
			this.btnAbort.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnAbort.Size = new System.Drawing.Size(65, 25);
			this.btnAbort.TabIndex = 19;
			this.btnAbort.Text = "Abort";
			//
			//btnNo
			//
			this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnNo.BackColor = System.Drawing.SystemColors.Control;
			this.btnNo.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnNo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnNo.Location = new System.Drawing.Point(200, 52);
			this.btnNo.Name = "btnNo";
			this.btnNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnNo.Size = new System.Drawing.Size(65, 25);
			this.btnNo.TabIndex = 18;
			this.btnNo.Text = "No";
			//
			//btnYes
			//
			this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnYes.BackColor = System.Drawing.SystemColors.Control;
			this.btnYes.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnYes.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnYes.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnYes.Location = new System.Drawing.Point(132, 52);
			this.btnYes.Name = "btnYes";
			this.btnYes.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnYes.Size = new System.Drawing.Size(65, 25);
			this.btnYes.TabIndex = 17;
			this.btnYes.Text = "Yes";
			//
			//btnCancel
			//
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnCancel.Location = new System.Drawing.Point(200, 52);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnCancel.Size = new System.Drawing.Size(65, 25);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.Text = "Cancel";
			//
			//btnCopyToClipboard
			//
			this.btnCopyToClipboard.BackColor = System.Drawing.SystemColors.Control;
			this.btnCopyToClipboard.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnCopyToClipboard.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnCopyToClipboard.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnCopyToClipboard.Image = (System.Drawing.Image)resources.GetObject("btnCopyToClipboard.Image");
			this.btnCopyToClipboard.Location = new System.Drawing.Point(16, 56);
			this.btnCopyToClipboard.Name = "btnCopyToClipboard";
			this.btnCopyToClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnCopyToClipboard.Size = new System.Drawing.Size(21, 21);
			this.btnCopyToClipboard.TabIndex = 15;
			this.btnCopyToClipboard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			//btnOK
			//
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.BackColor = System.Drawing.SystemColors.Control;
			this.btnOK.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnOK.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnOK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnOK.Location = new System.Drawing.Point(132, 52);
			this.btnOK.Name = "btnOK";
			this.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnOK.Size = new System.Drawing.Size(65, 25);
			this.btnOK.TabIndex = 12;
			this.btnOK.Text = "OK";
			//
			//picIcon
			//
			this.picIcon.BackColor = System.Drawing.SystemColors.Control;
			this.picIcon.Cursor = System.Windows.Forms.Cursors.Default;
			this.picIcon.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.picIcon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.picIcon.Location = new System.Drawing.Point(8, 12);
			this.picIcon.Name = "picIcon";
			this.picIcon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.picIcon.Size = new System.Drawing.Size(37, 37);
			this.picIcon.TabIndex = 13;
			this.picIcon.TabStop = false;
			//
			//lblA
			//
			this.lblA.AutoSize = true;
			this.lblA.BackColor = System.Drawing.SystemColors.Control;
			this.lblA.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblA.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblA.Location = new System.Drawing.Point(4, 8);
			this.lblA.Name = "lblA";
			this.lblA.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblA.Size = new System.Drawing.Size(14, 19);
			this.lblA.TabIndex = 14;
			this.lblA.Text = "A";
			this.lblA.Visible = false;
			//
			//ImgIcons
			//
			this.ImgIcons.ImageSize = new System.Drawing.Size(32, 32);
			this.ImgIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImgIcons.ImageStream");
			this.ImgIcons.TransparentColor = System.Drawing.Color.Transparent;
			//
			//lblDetails
			//
			this.lblDetails.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblDetails.AutoSize = true;
			this.lblDetails.Location = new System.Drawing.Point(48, 55);
			this.lblDetails.Name = "lblDetails";
			this.lblDetails.Size = new System.Drawing.Size(51, 19);
			this.lblDetails.TabIndex = 23;
			this.lblDetails.TabStop = true;
			this.lblDetails.Text = "Details";
			//
			//frmError
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(428, 90);
			this.Controls.Add(this.lblDetails);
			this.Controls.Add(this.lblA);
			this.Controls.Add(this.rtfMessage);
			this.Controls.Add(this.btnIgnore);
			this.Controls.Add(this.btnRetry);
			this.Controls.Add(this.btnAbort);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnCopyToClipboard);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.picIcon);
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(436, 110);
			this.Name = "frmError";
			this.Text = "frmError";
			this.ResumeLayout(false);

		}

		#endregion
		#region "Properties"
		new const short Margin = 4;
		private enum ImagesEnum
		{
			vbCriticalImage = 0,
			vbExclamationImage = 1,
			vbInformationImage = 2,
			vbQuestionImage = 3
		}
		protected string mMessage;
		protected string mDetails;
		protected MsgBoxResult mMsgBoxResult = MsgBoxResult.Ok;
		protected MsgBoxStyle mMsgBoxStyle = MsgBoxStyle.Information;
		protected bool mOKtoClose = false;
		public string Details {
			get { return mDetails; }
			set { mDetails = value; }
		}
		public string Message {
			get { return mMessage; }
			set {
				Trace("Message: \"" + value + "\"", trcOption.trcSupport);
				//Clean-up Message before assignment to rtfMessage.Text
				value = Strings.Replace(value, Constants.vbCrLf, Constants.vbVerticalTab);
				//Temporary...
				value = Strings.Replace(value, Constants.vbCr, Constants.vbCrLf);
				value = Strings.Replace(value, Constants.vbVerticalTab, Constants.vbCrLf);
				//Value = Replace(Value, vbTab, "     ")            
				rtfMessage.Text = value;

				Control Control = rtfMessage;
				Graphics g = Control.CreateGraphics();
				SizeF MaxSize = new SizeF(this.MaximumSize.Width - picIcon.Width - (3 * Margin), this.MaximumSize.Height - btnOK.Height - (3 * Margin));
				SizeF MessageSize = g.MeasureString(rtfMessage.Text, rtfMessage.Font, MaxSize);
				float NewHeight = MessageSize.Height;
				float NewWidth = MessageSize.Width;
				Trace("Calculated Height/Width of Message: " + MessageSize.Height.ToString() + "/" + MessageSize.Width.ToString(), trcOption.trcSupport);
				//Trace("Before Me.SetBounds; Me.ClientSize (Height/Width): " & Me.ClientSize.Height.ToString & "/" & Me.ClientSize.Width.ToString, trcOption.trcSupport)
				//Trace("Before Me.SetBounds; Me.ClientRectangle (Height/Width): " & Me.ClientRectangle.Height.ToString & "/" & Me.ClientRectangle.Width.ToString, trcOption.trcSupport)
				//Trace("Before Me.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
				//Trace("Before Me.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
				if ((mMyParent == null)) {
					Trace("Me.SetBounds((Screen.PrimaryScreen.Bounds.Width(" + Screen.PrimaryScreen.Bounds.Width.ToString() + ") - NewWidth(" + NewWidth.ToString() + ")) / 2 = " + ((Screen.PrimaryScreen.Bounds.Width - NewWidth) / 2).ToString() + ", (Screen.PrimaryScreen.Bounds.Height(" + Screen.PrimaryScreen.Bounds.Height.ToString() + ") - NewHeight(" + NewHeight.ToString() + ")) / 2 = " + ((Screen.PrimaryScreen.Bounds.Height - NewHeight) / 2).ToString() + ", Me.Width(" + this.Width.ToString() + ") - rtfMessage.Width(" + rtfMessage.Width.ToString() + ") + NewWidth(" + NewWidth.ToString() + ") = " + (this.Width - rtfMessage.Width + NewWidth).ToString() + ", Me.Height(" + this.Height.ToString() + ") - rtfmessage.Height(" + rtfMessage.Height.ToString() + ") + NewHeight(" + NewHeight.ToString() + " = " + (this.Height - rtfMessage.Height + NewHeight).ToString() + "), BoundsSpecified.All)", trcOption.trcSupport);
					this.SetBounds((Screen.PrimaryScreen.Bounds.Width - (int)NewWidth) / 2, (Screen.PrimaryScreen.Bounds.Height - (int)NewHeight) / 2, this.Width - rtfMessage.Width + (int)NewWidth, this.Height - rtfMessage.Height + (int)NewHeight, BoundsSpecified.All);
				} else {
					Trace("Me.SetBounds(mMyParent.Left(" + mMyParent.Left.ToString() + ") + (mMyParent.Width(" + mMyParent.Width.ToString() + ") - NewWidth(" + NewWidth.ToString() + ")) / 2 = " + (mMyParent.Left + (mMyParent.Width - NewWidth) / 2).ToString() + ", mMyParent.Top(" + mMyParent.Top.ToString() + ") + (mMyParent.Height(" + mMyParent.Height.ToString() + ") - NewHeight(" + NewHeight.ToString() + ")) / 2 = " + (mMyParent.Top + (mMyParent.Height - NewHeight) / 2).ToString() + ", Me.Width(" + this.Width.ToString() + ") - rtfMessage.Width(" + rtfMessage.Width.ToString() + ") + NewWidth(" + NewWidth.ToString() + ") = " + (this.Width - rtfMessage.Width + NewWidth).ToString() + ", Me.Height(" + this.Height.ToString() + ") - rtfmessage.Height(" + rtfMessage.Height.ToString() + ") + NewHeight(" + NewHeight.ToString() + " = " + (this.Height - rtfMessage.Height + NewHeight).ToString() + "), BoundsSpecified.All)", trcOption.trcSupport);
					this.SetBounds(mMyParent.Left + (mMyParent.Width - (int)NewWidth) / 2, mMyParent.Top + (mMyParent.Height - (int)NewHeight) / 2, this.Width - rtfMessage.Width + (int)NewWidth, this.Height - rtfMessage.Height + (int)NewHeight, BoundsSpecified.All);
				}
				ResizeMe();
				//Trace("After Me.SetBounds; Me.ClientSize (Height/Width): " & Me.ClientSize.Height.ToString & "/" & Me.ClientSize.Width.ToString, trcOption.trcSupport)
				//Trace("After Me.SetBounds; Me.ClientRectangle (Height/Width): " & Me.ClientRectangle.Height.ToString & "/" & Me.ClientRectangle.Width.ToString, trcOption.trcSupport)
				//Trace("After Me.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
				//Trace("After Me.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
				//MessageSize = null;
				g.Dispose();
			}
		}
		public MsgBoxResult MsgBoxResult {
			get { return mMsgBoxResult; }
		}
		public MsgBoxStyle MsgBoxStyle {
			get { return mMsgBoxStyle; }
			set {
				mMsgBoxStyle = value;
				Trace("Assigning Icon based on MsgBoxStyle", trcOption.trcSupport);
				switch (MsgBoxStyle & (MsgBoxStyle.Critical | MsgBoxStyle.Question | MsgBoxStyle.Exclamation | MsgBoxStyle.Information)) {
					case MsgBoxStyle.Exclamation:
						picIcon.Image = ImgIcons.Images[(int)ImagesEnum.vbExclamationImage];
						break;
					case MsgBoxStyle.Question:
						picIcon.Image = ImgIcons.Images[(int)ImagesEnum.vbQuestionImage];
						break;
					case MsgBoxStyle.Information:
						picIcon.Image = ImgIcons.Images[(int)ImagesEnum.vbInformationImage];
						break;
					case MsgBoxStyle.Critical:
						picIcon.Image = ImgIcons.Images[(int)ImagesEnum.vbCriticalImage];
						break;
					default:
						break;
				}

				btnOK.Visible = false;
				btnCancel.Visible = false;
				btnAbort.Visible = false;
				btnRetry.Visible = false;
				btnIgnore.Visible = false;
				btnYes.Visible = false;
				btnNo.Visible = false;
				switch (MsgBoxStyle & (MsgBoxStyle.OkOnly | MsgBoxStyle.OkCancel | MsgBoxStyle.AbortRetryIgnore | MsgBoxStyle.YesNoCancel | MsgBoxStyle.YesNo | MsgBoxStyle.RetryCancel | MsgBoxStyle.YesNo)) {
					case MsgBoxStyle.OkOnly:
						btnOK.Visible = true;
						btnOK.TabIndex = 0;
						this.AcceptButton = btnOK;
						break;
					case MsgBoxStyle.OkCancel:
						btnOK.Visible = true;
						btnOK.TabIndex = 0;
						this.AcceptButton = btnOK;
						btnCancel.Visible = true;
						btnCancel.TabIndex = 1;
						break;
					case MsgBoxStyle.AbortRetryIgnore:
						btnAbort.Visible = true;
						btnAbort.TabIndex = 0;
						this.AcceptButton = btnAbort;
						btnRetry.Visible = true;
						btnRetry.TabIndex = 1;
						btnIgnore.Visible = true;
						btnIgnore.TabIndex = 2;
						break;
					case MsgBoxStyle.YesNoCancel:
						btnYes.Visible = true;
						btnYes.TabIndex = 0;
						this.AcceptButton = btnYes;
						btnNo.Visible = true;
						btnNo.TabIndex = 1;
						btnCancel.Visible = true;
						btnCancel.TabIndex = 2;
						break;
					case MsgBoxStyle.YesNo:
						btnYes.Visible = true;
						btnYes.TabIndex = 0;
						this.AcceptButton = btnYes;
						btnNo.Visible = true;
						btnNo.TabIndex = 1;
						break;
					case MsgBoxStyle.RetryCancel:
						btnRetry.Visible = true;
						btnRetry.TabIndex = 0;
						this.AcceptButton = btnRetry;
						btnCancel.Visible = true;
						btnCancel.TabIndex = 1;
						break;
				}

			}
		}
		public bool OKtoClose {
			get { return mOKtoClose; }
			set { mOKtoClose = value; }
		}
		#endregion
		#region "Methods"
		private void ResizeMe()
		{
			//Trace("Before rtfMessage.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("Before rtfMessage.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
			//Trace("rtfMessage.SetBounds(rtfMessage.Left(" & rtfMessage.Left.ToString & "), rtfMessage.Top(" & rtfMessage.Top.ToString & "), Me.ClientRectangle.Width(" & Me.ClientRectangle.Width.ToString & ") - rtfMessage.Left(" & rtfMessage.Left.ToString & ") - (3 * Margin(" & Margin.ToString & ")) = " & (Me.ClientRectangle.Width - rtfMessage.Left - (3 * Margin)).ToString & ", Me.ClientRectangle.Height(" & Me.ClientRectangle.Height.ToString & ") - rtfMessage.Top(" & rtfMessage.Top.ToString & ") - btnOK.Height(" & btnOK.Height.ToString & ") - (2 * Margin(" & Margin.ToString & ")) = " & (Me.ClientRectangle.Height - rtfMessage.Top - btnOK.Height - (2 * Margin)).ToString, trcOption.trcSupport)
			rtfMessage.SetBounds(rtfMessage.Left, rtfMessage.Top, this.ClientRectangle.Width - rtfMessage.Left - (3 * Margin), this.ClientRectangle.Height - rtfMessage.Top - btnOK.Height - (2 * Margin));
			//Trace("After rtfMessage.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("After rtfMessage.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
			switch (MsgBoxStyle & (MsgBoxStyle.OkOnly | MsgBoxStyle.OkCancel | MsgBoxStyle.AbortRetryIgnore | MsgBoxStyle.YesNoCancel | MsgBoxStyle.YesNo | MsgBoxStyle.RetryCancel | MsgBoxStyle.YesNo)) {
				case MsgBoxStyle.OkOnly:
					btnOK.SetBounds((this.ClientRectangle.Width - btnOK.Width) / 2, rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				case MsgBoxStyle.OkCancel:
					btnOK.SetBounds((this.ClientRectangle.Width / 2) - btnOK.Width - (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnCancel.SetBounds((this.ClientRectangle.Width / 2) + (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				case MsgBoxStyle.AbortRetryIgnore:
					btnAbort.SetBounds((this.ClientRectangle.Width / 2) - btnAbort.Width - (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnRetry.SetBounds((this.ClientRectangle.Width - btnRetry.Width) / 2, rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnIgnore.SetBounds((this.ClientRectangle.Width / 2) + (btnRetry.Width / 2) + (int)(Margin * 1.5), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				case MsgBoxStyle.YesNoCancel:
					btnYes.SetBounds((this.ClientRectangle.Width / 2) - btnYes.Width - (btnNo.Width / 2) - (int)(Margin * 1.5), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnNo.SetBounds((this.ClientRectangle.Width - btnNo.Width) / 2, rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnCancel.SetBounds((this.ClientRectangle.Width / 2) + (btnNo.Width / 2) + (int)(Margin * 1.5), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				case MsgBoxStyle.YesNo:
					btnYes.SetBounds((this.ClientRectangle.Width / 2) - btnYes.Width - (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnNo.SetBounds((this.ClientRectangle.Width / 2) + (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				case MsgBoxStyle.RetryCancel:
					btnRetry.SetBounds((this.ClientRectangle.Width / 2) - btnRetry.Width - (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					btnCancel.SetBounds((this.ClientRectangle.Width / 2) + (Margin / 2), rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
				default:
					btnOK.SetBounds((this.ClientRectangle.Width - btnOK.Width) / 2, rtfMessage.Top + rtfMessage.Height + Margin, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
					break;
			}
		}
		#endregion
		#region "Event Handlers"
		private void btnAbort_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Abort;
			this.Hide();
		}
		private void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Cancel;
			this.Hide();
		}
		private void btnCopyToClipboard_Click(System.Object sender, System.EventArgs e)
		{
			System.Windows.Forms.DataObject datobj = new System.Windows.Forms.DataObject();

			datobj.SetData(System.Windows.Forms.DataFormats.Text, rtfMessage.Text);
			System.Windows.Forms.Clipboard.SetDataObject(datobj);
		}
		private void btnIgnore_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Ignore;
			this.Hide();
		}
		private void btnNo_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.No;
			this.Hide();
		}
		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Ok;
			this.Hide();
		}
		private void btnRetry_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Retry;
			this.Hide();
		}
		private void btnYes_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Yes;
			this.Hide();
		}
		private void Form_Resize(System.Object sender, System.EventArgs e)
		{
			//ResizeMe()
		}
		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!mOKtoClose)
				e.Cancel = true;
		}
		private void lblDetails_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			frmError frmDetail = null;
			try {
				frmDetail = new frmError(mTCBase, this);
				var _with1 = frmDetail;
				_with1.lblDetails.Visible = false;
				_with1.Icon = ((Icon != null) ? Icon : this.Icon);
				_with1.Text = this.Text;
				_with1.Message = mDetails;
				_with1.MsgBoxStyle = MsgBoxStyle.Information;
				_with1.StartPosition = FormStartPosition.CenterParent;
				_with1.ShowDialog(this);
				_with1.OKtoClose = true;
				_with1.Close();
			} finally {
				frmDetail = null;
			}
		}
		private void rtfMessage_TextChanged(object sender, System.EventArgs e)
		{
			Control Control = rtfMessage;
			Graphics g = Control.CreateGraphics();
			SizeF MaxSize = new SizeF(this.MaximumSize.Width - picIcon.Width - (3 * Margin), this.MaximumSize.Height - btnOK.Height - (3 * Margin));
			SizeF MessageSize = g.MeasureString(rtfMessage.Text, rtfMessage.Font, MaxSize);
			float NewHeight = MessageSize.Height;
			float NewWidth = MessageSize.Width;
			Trace("Calculated Height/Width of Message: " + MessageSize.Height.ToString() + "/" + MessageSize.Width.ToString(), trcOption.trcSupport);
			//Trace("Before Me.SetBounds; Me.ClientSize (Height/Width): " & Me.ClientSize.Height.ToString & "/" & Me.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("Before Me.SetBounds; Me.ClientRectangle (Height/Width): " & Me.ClientRectangle.Height.ToString & "/" & Me.ClientRectangle.Width.ToString, trcOption.trcSupport)
			//Trace("Before Me.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("Before Me.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
			if ((mMyParent == null)) {
				Trace("Me.SetBounds((Screen.PrimaryScreen.Bounds.Width(" + Screen.PrimaryScreen.Bounds.Width.ToString() + ") - NewWidth(" + NewWidth.ToString() + ")) / 2 = " + ((Screen.PrimaryScreen.Bounds.Width - NewWidth) / 2).ToString() + ", (Screen.PrimaryScreen.Bounds.Height(" + Screen.PrimaryScreen.Bounds.Height.ToString() + ") - NewHeight(" + NewHeight.ToString() + ")) / 2 = " + ((Screen.PrimaryScreen.Bounds.Height - NewHeight) / 2).ToString() + ", Me.Width(" + this.Width.ToString() + ") - rtfMessage.Width(" + rtfMessage.Width.ToString() + ") + NewWidth(" + NewWidth.ToString() + ") = " + (this.Width - rtfMessage.Width + NewWidth).ToString() + ", Me.Height(" + this.Height.ToString() + ") - rtfmessage.Height(" + rtfMessage.Height.ToString() + ") + NewHeight(" + NewHeight.ToString() + " = " + (this.Height - rtfMessage.Height + NewHeight).ToString() + "), BoundsSpecified.All)", trcOption.trcSupport);
				this.SetBounds((Screen.PrimaryScreen.Bounds.Width - (int)NewWidth) / 2, (Screen.PrimaryScreen.Bounds.Height - (int)NewHeight) / 2, this.Width - rtfMessage.Width + (int)NewWidth, this.Height - rtfMessage.Height + (int)NewHeight, BoundsSpecified.All);
			} else {
				Trace("Me.SetBounds(mMyParent.Left(" + mMyParent.Left.ToString() + ") + (mMyParent.Width(" + mMyParent.Width.ToString() + ") - NewWidth(" + NewWidth.ToString() + ")) / 2 = " + (mMyParent.Left + (mMyParent.Width - NewWidth) / 2).ToString() + ", mMyParent.Top(" + mMyParent.Top.ToString() + ") + (mMyParent.Height(" + mMyParent.Height.ToString() + ") - NewHeight(" + NewHeight.ToString() + ")) / 2 = " + (mMyParent.Top + (mMyParent.Height - NewHeight) / 2).ToString() + ", Me.Width(" + this.Width.ToString() + ") - rtfMessage.Width(" + rtfMessage.Width.ToString() + ") + NewWidth(" + NewWidth.ToString() + ") = " + (this.Width - rtfMessage.Width + NewWidth).ToString() + ", Me.Height(" + this.Height.ToString() + ") - rtfmessage.Height(" + rtfMessage.Height.ToString() + ") + NewHeight(" + NewHeight.ToString() + " = " + (this.Height - rtfMessage.Height + NewHeight).ToString() + "), BoundsSpecified.All)", trcOption.trcSupport);
				this.SetBounds(mMyParent.Left + (mMyParent.Width - (int)NewWidth) / 2, mMyParent.Top + (mMyParent.Height - (int)NewHeight) / 2, this.Width - rtfMessage.Width + (int)NewWidth, this.Height - rtfMessage.Height + (int)NewHeight, BoundsSpecified.All);
			}
			ResizeMe();
			//Trace("After Me.SetBounds; Me.ClientSize (Height/Width): " & Me.ClientSize.Height.ToString & "/" & Me.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("After Me.SetBounds; Me.ClientRectangle (Height/Width): " & Me.ClientRectangle.Height.ToString & "/" & Me.ClientRectangle.Width.ToString, trcOption.trcSupport)
			//Trace("After Me.SetBounds; rtfMessage.ClientSize (Height/Width): " & rtfMessage.ClientSize.Height.ToString & "/" & rtfMessage.ClientSize.Width.ToString, trcOption.trcSupport)
			//Trace("After Me.SetBounds; rtfMessage.ClientRectangle (Height/Width): " & rtfMessage.ClientRectangle.Height.ToString & "/" & rtfMessage.ClientRectangle.Width.ToString, trcOption.trcSupport)
			//MessageSize = null;
			g.Dispose();
		}
		#endregion
	}
}
