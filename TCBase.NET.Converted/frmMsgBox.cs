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
//frmMsgBox - frmMsgBox.vb
//   TreasureChest2 Custom MsgBox Form with Selectable Output...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/08/14    Upgraded project to Visual Studio 2013;
//   10/02/14    Added .BringToFront in frmMsgBox_Shown to always display this form on top;
//   03/27/14    Corrected sizing geometry, especially for large messages (larger than the form's max);
//   03/25/14    Upgraded frmMsgBox_Activated to frmMsgBox_Shown;
//   03/13/14    Upgraded project to Visual Studio 2005;
//               Added premature exit from ResizeMessage when rtfMessage.Text is empty;
//               Added padding of two vbCrLf characters;
//   07/09/12    Upgraded to Visual Studio 2005/Visual Basic 8.0;
//   03/01/12    More dynamic sizing adjustments;
//   02/13/12    Reworked dynamic sizing issues;
//   07/19/05    Added resizing logic to rtfMessage_TextChanged event handler in an attempt to resize form during ShowMsgBoxRFT();
//   03/03/05    Upgraded for use as a VB.NET Component;
//   03/26/04    Added logic to support remaining vbMsgBoxStyle values;
//   01/25/03    Added SetTopmostWindow;
//   11/19/02    Updated to support MsgBox-like button handling (although not all MsgBox functions are handled yet);
//   10/30/02    Added "On Error Resume Next" to functions without any error handling;
//   06/20/02    Added cmdCopyToClipboard;
//   12/05/01    line wrapping before vbCrLf
//   11/08/01    Updated for v2.0;
//   09/20/01    Reviewed case-sensitivity issues for SQL Server 'dictionary_iso (51)' character set support;
//   03/13/01    Removed dependency on libDeviceCaps.bas in-lieu of clsSystem.cls;
//   04/28/00    Added better resizing logic;
//   04/25/00    Created;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	internal class frmMsgBox : frmTCBase
	{
		public frmMsgBox(clsSupport objSupport, Form objParent = null) : base(objSupport, "frmMsgBox", objParent)
		{
			Closing += frmMsgBox_Closing;
			Shown += frmMsgBox_Shown;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			if (TCBase.MyComputer.Screen.Bounds.Width > 640 && TCBase.MyComputer.Screen.Bounds.Height > 480) {
				this.MaximumSize = new Size(800, 600);
			} else {
				//Size the form to VGA Minimums...
				this.MaximumSize = new Size(640, 480);
			}
			picIcon.Image = ImgIcons.Images[(int)MsgBoxImagesEnum.vbExclamationImage];
			//Use as default...
			//Debug.WriteLine(String.Format("New(): Me.Size: {0}; rtfMessage.Size: {1}", Me.Size.ToString, Me.rtfMessage.Size.ToString))
		}

		#region "Windows Form Designer generated code "
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ToolTip ToolTip1;
		private System.Windows.Forms.Button withEventsField_cmdIgnore;
		public System.Windows.Forms.Button cmdIgnore {
			get { return withEventsField_cmdIgnore; }
			set {
				if (withEventsField_cmdIgnore != null) {
					withEventsField_cmdIgnore.Click -= cmdIgnore_Click;
				}
				withEventsField_cmdIgnore = value;
				if (withEventsField_cmdIgnore != null) {
					withEventsField_cmdIgnore.Click += cmdIgnore_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdRetry;
		public System.Windows.Forms.Button cmdRetry {
			get { return withEventsField_cmdRetry; }
			set {
				if (withEventsField_cmdRetry != null) {
					withEventsField_cmdRetry.Click -= cmdRetry_Click;
				}
				withEventsField_cmdRetry = value;
				if (withEventsField_cmdRetry != null) {
					withEventsField_cmdRetry.Click += cmdRetry_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdAbort;
		public System.Windows.Forms.Button cmdAbort {
			get { return withEventsField_cmdAbort; }
			set {
				if (withEventsField_cmdAbort != null) {
					withEventsField_cmdAbort.Click -= cmdAbort_Click;
				}
				withEventsField_cmdAbort = value;
				if (withEventsField_cmdAbort != null) {
					withEventsField_cmdAbort.Click += cmdAbort_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdNo;
		public System.Windows.Forms.Button cmdNo {
			get { return withEventsField_cmdNo; }
			set {
				if (withEventsField_cmdNo != null) {
					withEventsField_cmdNo.Click -= cmdNo_Click;
				}
				withEventsField_cmdNo = value;
				if (withEventsField_cmdNo != null) {
					withEventsField_cmdNo.Click += cmdNo_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdYes;
		public System.Windows.Forms.Button cmdYes {
			get { return withEventsField_cmdYes; }
			set {
				if (withEventsField_cmdYes != null) {
					withEventsField_cmdYes.Click -= cmdYes_Click;
				}
				withEventsField_cmdYes = value;
				if (withEventsField_cmdYes != null) {
					withEventsField_cmdYes.Click += cmdYes_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdCancel;
		public System.Windows.Forms.Button cmdCancel {
			get { return withEventsField_cmdCancel; }
			set {
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click -= cmdCancel_Click;
				}
				withEventsField_cmdCancel = value;
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click += cmdCancel_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdCopyToClipboard;
		public System.Windows.Forms.Button cmdCopyToClipboard {
			get { return withEventsField_cmdCopyToClipboard; }
			set {
				if (withEventsField_cmdCopyToClipboard != null) {
					withEventsField_cmdCopyToClipboard.Click -= cmdCopyToClipboard_Click;
				}
				withEventsField_cmdCopyToClipboard = value;
				if (withEventsField_cmdCopyToClipboard != null) {
					withEventsField_cmdCopyToClipboard.Click += cmdCopyToClipboard_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdOK;
		public System.Windows.Forms.Button cmdOK {
			get { return withEventsField_cmdOK; }
			set {
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click -= cmdOK_Click;
				}
				withEventsField_cmdOK = value;
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click += cmdOK_Click;
				}
			}
		}
		public System.Windows.Forms.PictureBox picIcon;
		public System.Windows.Forms.Label lblA;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		internal System.Windows.Forms.ImageList ImgIcons;
		private System.Windows.Forms.RichTextBox withEventsField_rtfMessage;
		internal System.Windows.Forms.RichTextBox rtfMessage {
			get { return withEventsField_rtfMessage; }
			set {
				if (withEventsField_rtfMessage != null) {
					withEventsField_rtfMessage.FontChanged -= rtfMessage_FontChanged;
				}
				withEventsField_rtfMessage = value;
				if (withEventsField_rtfMessage != null) {
					withEventsField_rtfMessage.FontChanged += rtfMessage_FontChanged;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMsgBox));
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cmdCopyToClipboard = new System.Windows.Forms.Button();
			this.cmdIgnore = new System.Windows.Forms.Button();
			this.cmdRetry = new System.Windows.Forms.Button();
			this.cmdAbort = new System.Windows.Forms.Button();
			this.cmdNo = new System.Windows.Forms.Button();
			this.cmdYes = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblA = new System.Windows.Forms.Label();
			this.ImgIcons = new System.Windows.Forms.ImageList(this.components);
			this.rtfMessage = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.picIcon).BeginInit();
			this.SuspendLayout();
			//
			//cmdCopyToClipboard
			//
			this.cmdCopyToClipboard.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCopyToClipboard.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCopyToClipboard.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCopyToClipboard.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCopyToClipboard.Image = (System.Drawing.Image)resources.GetObject("cmdCopyToClipboard.Image");
			this.cmdCopyToClipboard.Location = new System.Drawing.Point(12, 48);
			this.cmdCopyToClipboard.Name = "cmdCopyToClipboard";
			this.cmdCopyToClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCopyToClipboard.Size = new System.Drawing.Size(21, 21);
			this.cmdCopyToClipboard.TabIndex = 4;
			this.cmdCopyToClipboard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.ToolTip1.SetToolTip(this.cmdCopyToClipboard, "Copy message text to clipboard...");
			this.cmdCopyToClipboard.UseVisualStyleBackColor = false;
			//
			//cmdIgnore
			//
			this.cmdIgnore.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdIgnore.BackColor = System.Drawing.SystemColors.Control;
			this.cmdIgnore.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdIgnore.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdIgnore.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdIgnore.Location = new System.Drawing.Point(264, 44);
			this.cmdIgnore.Name = "cmdIgnore";
			this.cmdIgnore.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdIgnore.Size = new System.Drawing.Size(65, 25);
			this.cmdIgnore.TabIndex = 10;
			this.cmdIgnore.Text = "Ignore";
			this.cmdIgnore.UseVisualStyleBackColor = false;
			//
			//cmdRetry
			//
			this.cmdRetry.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdRetry.BackColor = System.Drawing.SystemColors.Control;
			this.cmdRetry.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdRetry.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdRetry.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdRetry.Location = new System.Drawing.Point(128, 44);
			this.cmdRetry.Name = "cmdRetry";
			this.cmdRetry.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdRetry.Size = new System.Drawing.Size(65, 25);
			this.cmdRetry.TabIndex = 9;
			this.cmdRetry.Text = "Retry";
			this.cmdRetry.UseVisualStyleBackColor = false;
			//
			//cmdAbort
			//
			this.cmdAbort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdAbort.BackColor = System.Drawing.SystemColors.Control;
			this.cmdAbort.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdAbort.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdAbort.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdAbort.Location = new System.Drawing.Point(196, 44);
			this.cmdAbort.Name = "cmdAbort";
			this.cmdAbort.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdAbort.Size = new System.Drawing.Size(65, 25);
			this.cmdAbort.TabIndex = 8;
			this.cmdAbort.Text = "Abort";
			this.cmdAbort.UseVisualStyleBackColor = false;
			//
			//cmdNo
			//
			this.cmdNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdNo.BackColor = System.Drawing.SystemColors.Control;
			this.cmdNo.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdNo.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdNo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdNo.Location = new System.Drawing.Point(196, 44);
			this.cmdNo.Name = "cmdNo";
			this.cmdNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdNo.Size = new System.Drawing.Size(65, 25);
			this.cmdNo.TabIndex = 7;
			this.cmdNo.Text = "No";
			this.cmdNo.UseVisualStyleBackColor = false;
			//
			//cmdYes
			//
			this.cmdYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdYes.BackColor = System.Drawing.SystemColors.Control;
			this.cmdYes.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdYes.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdYes.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdYes.Location = new System.Drawing.Point(128, 44);
			this.cmdYes.Name = "cmdYes";
			this.cmdYes.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdYes.Size = new System.Drawing.Size(65, 25);
			this.cmdYes.TabIndex = 6;
			this.cmdYes.Text = "Yes";
			this.cmdYes.UseVisualStyleBackColor = false;
			//
			//cmdCancel
			//
			this.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCancel.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCancel.Location = new System.Drawing.Point(196, 44);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCancel.Size = new System.Drawing.Size(65, 25);
			this.cmdCancel.TabIndex = 5;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			//
			//cmdOK
			//
			this.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
			this.cmdOK.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdOK.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdOK.Location = new System.Drawing.Point(128, 44);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdOK.Size = new System.Drawing.Size(65, 25);
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = false;
			//
			//picIcon
			//
			this.picIcon.BackColor = System.Drawing.SystemColors.Control;
			this.picIcon.Cursor = System.Windows.Forms.Cursors.Default;
			this.picIcon.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.picIcon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.picIcon.Location = new System.Drawing.Point(4, 4);
			this.picIcon.Name = "picIcon";
			this.picIcon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.picIcon.Size = new System.Drawing.Size(37, 37);
			this.picIcon.TabIndex = 1;
			this.picIcon.TabStop = false;
			//
			//lblA
			//
			this.lblA.AutoSize = true;
			this.lblA.BackColor = System.Drawing.SystemColors.Control;
			this.lblA.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblA.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblA.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblA.Location = new System.Drawing.Point(0, 0);
			this.lblA.Name = "lblA";
			this.lblA.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblA.Size = new System.Drawing.Size(15, 14);
			this.lblA.TabIndex = 3;
			this.lblA.Text = "A";
			this.lblA.Visible = false;
			//
			//ImgIcons
			//
			this.ImgIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImgIcons.ImageStream");
			this.ImgIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.ImgIcons.Images.SetKeyName(0, "");
			this.ImgIcons.Images.SetKeyName(1, "");
			this.ImgIcons.Images.SetKeyName(2, "");
			this.ImgIcons.Images.SetKeyName(3, "");
			//
			//rtfMessage
			//
			this.rtfMessage.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtfMessage.BackColor = System.Drawing.SystemColors.Control;
			this.rtfMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtfMessage.Font = new System.Drawing.Font("Arial", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.rtfMessage.Location = new System.Drawing.Point(48, 4);
			this.rtfMessage.Name = "rtfMessage";
			this.rtfMessage.ReadOnly = true;
			this.rtfMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtfMessage.Size = new System.Drawing.Size(372, 36);
			this.rtfMessage.TabIndex = 11;
			this.rtfMessage.TabStop = false;
			this.rtfMessage.Text = "rtfMessage";
			//
			//frmMsgBox
			//
			this.AcceptButton = this.cmdRetry;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(428, 75);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdNo);
			this.Controls.Add(this.cmdYes);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cmdIgnore);
			this.Controls.Add(this.cmdRetry);
			this.Controls.Add(this.cmdAbort);
			this.Controls.Add(this.cmdCopyToClipboard);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblA);
			this.Controls.Add(this.rtfMessage);
			this.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(436, 109);
			this.Name = "frmMsgBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmMsgBox";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			((System.ComponentModel.ISupportInitialize)this.picIcon).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		#region "Properties"
		const short myMargin = 4;
		private enum MsgBoxImagesEnum
		{
			vbCriticalImage = 0,
			vbExclamationImage = 1,
			vbInformationImage = 2,
			vbQuestionImage = 3
		}
		protected string mMessage;
		protected MsgBoxResult mMsgBoxResult = MsgBoxResult.Ok;
		protected MsgBoxStyle mMsgBoxStyle = MsgBoxStyle.Information;
		protected bool mOKtoClose = false;
		public string Message {
			get { return mMessage; }
			set {
				Trace("Message: \"" + value + "\"", trcOption.trcSupport);
				if (value.StartsWith("{\\rtf")) {
					this.rtfMessage.Rtf = value;
				} else {
					//Clean-up Message before assignment to rtfMessage.Text
					value = Strings.Replace(value, Constants.vbCrLf, Constants.vbVerticalTab);
					//Temporary...
					value = Strings.Replace(value, Constants.vbCr, Constants.vbCrLf);
					value = Strings.Replace(value, Constants.vbVerticalTab, Constants.vbCrLf);
					//Value = Replace(Value, vbTab, "     ")            
					this.rtfMessage.Text = value;
				}
				this.ResizeMessage();
			}
		}
		public MsgBoxResult MsgBoxResult {
			get { return mMsgBoxResult; }
		}
		public MsgBoxStyle MsgBoxStyle {
			get { return mMsgBoxStyle; }
			set {
				this.SuspendLayout();
				mMsgBoxStyle = value;
				Trace("Assigning Icon based on MsgBoxStyle", trcOption.trcSupport);
				switch (MsgBoxStyle & (MsgBoxStyle.Critical | MsgBoxStyle.Question | MsgBoxStyle.Exclamation | MsgBoxStyle.Information)) {
					case MsgBoxStyle.Exclamation:
						this.picIcon.Image = ImgIcons.Images[(int)MsgBoxImagesEnum.vbExclamationImage];
						break;
					case MsgBoxStyle.Question:
						this.picIcon.Image = ImgIcons.Images[(int)MsgBoxImagesEnum.vbQuestionImage];
						break;
					case MsgBoxStyle.Information:
						this.picIcon.Image = ImgIcons.Images[(int)MsgBoxImagesEnum.vbInformationImage];
						break;
					case MsgBoxStyle.Critical:
						this.picIcon.Image = ImgIcons.Images[(int)MsgBoxImagesEnum.vbCriticalImage];
						break;
					default:
						break;
				}

				this.cmdOK.Visible = false;
				this.cmdCancel.Visible = false;
				this.cmdAbort.Visible = false;
				this.cmdRetry.Visible = false;
				this.cmdIgnore.Visible = false;
				this.cmdYes.Visible = false;
				this.cmdNo.Visible = false;
				switch (MsgBoxStyle & (MsgBoxStyle.OkOnly | MsgBoxStyle.OkCancel | MsgBoxStyle.AbortRetryIgnore | MsgBoxStyle.YesNoCancel | MsgBoxStyle.YesNo | MsgBoxStyle.RetryCancel | MsgBoxStyle.YesNo)) {
					case MsgBoxStyle.OkOnly:
						this.cmdOK.Visible = true;
						this.cmdOK.TabIndex = 0;
						this.AcceptButton = this.cmdOK;
						this.cmdOK.SetBounds((this.ClientRectangle.Width - this.cmdOK.Width) / 2, this.ClientRectangle.Height - this.cmdOK.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					case MsgBoxStyle.OkCancel:
						this.cmdOK.Visible = true;
						this.cmdOK.TabIndex = 0;
						this.AcceptButton = this.cmdOK;
						this.cmdCancel.Visible = true;
						this.cmdCancel.TabIndex = 1;
						this.cmdOK.SetBounds((this.ClientRectangle.Width / 2) - this.cmdOK.Width - (myMargin / 2), this.ClientRectangle.Height - this.cmdOK.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdCancel.SetBounds((this.ClientRectangle.Width / 2) + (myMargin / 2), this.ClientRectangle.Height - this.cmdCancel.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					case MsgBoxStyle.AbortRetryIgnore:
						this.cmdAbort.Visible = true;
						this.cmdAbort.TabIndex = 0;
						this.AcceptButton = this.cmdAbort;
						this.cmdRetry.Visible = true;
						this.cmdRetry.TabIndex = 1;
						this.cmdIgnore.Visible = true;
						this.cmdIgnore.TabIndex = 2;
						this.cmdAbort.SetBounds((this.ClientRectangle.Width / 2) - this.cmdAbort.Width - (myMargin / 2), this.ClientRectangle.Height - this.cmdAbort.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdRetry.SetBounds((this.ClientRectangle.Width - this.cmdRetry.Width) / 2, this.ClientRectangle.Height - this.cmdRetry.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdIgnore.SetBounds((this.ClientRectangle.Width / 2) + (this.cmdRetry.Width / 2) + (int)(myMargin * 1.5), this.ClientRectangle.Height - this.cmdIgnore.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					case MsgBoxStyle.YesNoCancel:
						this.cmdYes.Visible = true;
						this.cmdYes.TabIndex = 0;
						this.AcceptButton = this.cmdYes;
						this.cmdNo.Visible = true;
						this.cmdNo.TabIndex = 1;
						this.cmdCancel.Visible = true;
						this.cmdCancel.TabIndex = 2;
						this.cmdYes.SetBounds((this.ClientRectangle.Width / 2) - this.cmdYes.Width - (this.cmdNo.Width / 2) - (int)(myMargin * 1.5), this.ClientRectangle.Height - this.cmdYes.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdNo.SetBounds((this.ClientRectangle.Width - this.cmdNo.Width) / 2, this.ClientRectangle.Height - this.cmdNo.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdCancel.SetBounds((this.ClientRectangle.Width / 2) + (this.cmdNo.Width / 2) + (int)(myMargin * 1.5), this.ClientRectangle.Height - this.cmdCancel.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					case MsgBoxStyle.YesNo:
						this.cmdYes.Visible = true;
						this.cmdYes.TabIndex = 0;
						this.AcceptButton = this.cmdYes;
						this.cmdNo.Visible = true;
						this.cmdNo.TabIndex = 1;
						this.cmdYes.SetBounds((this.ClientRectangle.Width / 2) - this.cmdYes.Width - (myMargin / 2), this.ClientRectangle.Height - this.cmdYes.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdNo.SetBounds((this.ClientRectangle.Width / 2) + (myMargin / 2), this.ClientRectangle.Height - this.cmdNo.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					case MsgBoxStyle.RetryCancel:
						this.cmdRetry.Visible = true;
						this.cmdRetry.TabIndex = 0;
						this.AcceptButton = this.cmdRetry;
						this.cmdCancel.Visible = true;
						this.cmdCancel.TabIndex = 1;
						this.cmdRetry.SetBounds((this.ClientRectangle.Width / 2) - this.cmdRetry.Width - (myMargin / 2), this.ClientRectangle.Height - this.cmdRetry.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						this.cmdCancel.SetBounds((this.ClientRectangle.Width / 2) + (myMargin / 2), this.ClientRectangle.Height - this.cmdCancel.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
					default:
						this.cmdOK.SetBounds((this.ClientRectangle.Width - this.cmdOK.Width) / 2, this.ClientRectangle.Height - this.cmdOK.Height - (myMargin * 2), 0, 0, System.Windows.Forms.BoundsSpecified.Location);
						break;
				}
				this.ResumeLayout(false);
			}
		}
		public bool OKtoClose {
			get { return mOKtoClose; }
			set { mOKtoClose = value; }
		}
		#endregion
		#region "Methods"
		private void CenterOnParent()
		{
			this.SuspendLayout();
			try {
				if ((mMyParent == null)) {
					this.SetBounds((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2, 0, 0, BoundsSpecified.Location);
				} else if (this.StartPosition != FormStartPosition.CenterParent) {
					this.SetBounds(mMyParent.Left + (mMyParent.Width - this.Width) / 2, mMyParent.Top + (mMyParent.Height - this.Height) / 2, 0, 0, BoundsSpecified.Location);
				} else {
					//Don't interfere with Windows - let it do what we want...
				}
			} finally {
				this.ResumeLayout(false);
			}
		}
		private void ResizeMessage()
		{
			if ((this.rtfMessage.Text == null) || this.rtfMessage.Text.Trim().Length == 0)
				return;
			Graphics g = null;
			this.SuspendLayout();
			try {
				this.rtfMessage.SuspendLayout();
				g = this.rtfMessage.CreateGraphics();
				int MaxWidth = this.MaximumSize.Width - this.rtfMessage.Left - (3 * myMargin);
				//Me.picIcon.Width
				int MaxHeight = this.MaximumSize.Height - this.rtfMessage.Top - this.cmdOK.Height - (4 * myMargin) - (this.Height - this.ClientSize.Height);
				SizeF MaxSize = new SizeF(MaxWidth, 0);
				Size MessageSize = g.MeasureString(this.rtfMessage.Text + Constants.vbCrLf + Constants.vbCrLf, this.rtfMessage.Font, MaxSize).ToSize();
				int NewWidth = MessageSize.Width + (myMargin * 2);
				int NewHeight = MessageSize.Height;
				if (NewHeight < (this.picIcon.Top + this.picIcon.Height))
					NewHeight = (this.picIcon.Top + this.picIcon.Height);
				this.SetBounds(0, 0, this.Width - this.rtfMessage.ClientSize.Width + NewWidth, this.Height - this.rtfMessage.ClientSize.Height + NewHeight, BoundsSpecified.Size);
				this.MinimumSize = this.Size;
				if (NewWidth >= MaxWidth && NewHeight >= MaxHeight) {
					this.rtfMessage.ScrollBars = RichTextBoxScrollBars.Both;
					NewWidth = MaxWidth;
					NewHeight = MaxHeight;
				} else if (NewWidth >= MaxWidth) {
					this.rtfMessage.ScrollBars = RichTextBoxScrollBars.Horizontal;
					NewWidth = MaxWidth;
				} else if (NewHeight >= MaxHeight) {
					this.rtfMessage.ScrollBars = RichTextBoxScrollBars.Vertical;
					NewHeight = MaxHeight;
				} else {
					this.rtfMessage.ScrollBars = RichTextBoxScrollBars.None;
				}
				this.rtfMessage.ClientSize = new Size(NewWidth, NewHeight);
				this.rtfMessage.ResumeLayout(false);
			} finally {
				this.ResumeLayout(false);
				g.Dispose();
				//Debug.WriteLine(String.Format("ResizeMessage(): Me.Size: {0}; rtfMessage.Size: {1}", Me.Size.ToString, Me.rtfMessage.Size.ToString))
			}
		}
		#endregion
		#region "Event Handlers"
		private void cmdAbort_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Abort;
			this.Hide();
		}
		private void cmdCancel_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Cancel;
			this.Hide();
		}
		private void cmdCopyToClipboard_Click(System.Object sender, System.EventArgs e)
		{
			System.Windows.Forms.DataObject datobj = new System.Windows.Forms.DataObject();
			datobj.SetData(System.Windows.Forms.DataFormats.Text, rtfMessage.Text);
			System.Windows.Forms.Clipboard.SetDataObject(datobj);
		}
		private void cmdIgnore_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Ignore;
			this.Hide();
		}
		private void cmdNo_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.No;
			this.Hide();
		}
		private void cmdOK_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Ok;
			this.Hide();
		}
		private void cmdRetry_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Retry;
			this.Hide();
		}
		private void cmdYes_Click(System.Object sender, System.EventArgs e)
		{
			mMsgBoxResult = MsgBoxResult.Yes;
			this.Hide();
		}
		private void frmMsgBox_Shown(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			try {
				this.CenterOnParent();
				this.BringToFront();
			} finally {
				this.ResumeLayout(false);
			}
		}
		private void frmMsgBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!mOKtoClose)
				e.Cancel = true;
		}
		private void rtfMessage_FontChanged(object sender, System.EventArgs e)
		{
			this.ResizeMessage();
		}
		#endregion
	}
}
