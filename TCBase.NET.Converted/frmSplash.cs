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
namespace TCBase
{
	//frmSplash.vb
	//   Splash Screen...
	//   Copyright © 1998-2018, Ken Clark
	//*********************************************************************************************************************************
	//
	//   Modification History:
	//   Date:       Description:
	//   09/18/16    Updated object references to reflect architectural changes;
	//   10/25/09    Rewritten in VB.NET;
	//   10/14/02    Added Error Handling;
	//   11/04/99    Created;
	//=================================================================================================================================
	public class frmSplash : Form
	{
		public frmSplash(string Caption, int x, int y, int Width, int Height, Icon IconImage) : base()
		{
			Load += frmSplash_Load;
			Deactivate += frmSplash_Deactivate;
			Activated += frmSplash_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			this.pbIcon.Image = IconImage.ToBitmap();
			this.lblActivity.Text = Caption;
			this.lblStatus.Text = clsSupport.bpeNullString;
			mParentX = x;
			mParentY = y;
			mParentWidth = Width;
			mParentHeight = Height;
		}
		#region " Windows Form Designer generated code "

		public frmSplash() : base()
		{
			Load += frmSplash_Load;
			Deactivate += frmSplash_Deactivate;
			Activated += frmSplash_Activated;

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
		private System.Windows.Forms.GroupBox withEventsField_gbSplash;
		internal System.Windows.Forms.GroupBox gbSplash {
			get { return withEventsField_gbSplash; }
			set {
				if (withEventsField_gbSplash != null) {
					withEventsField_gbSplash.Click -= gbSplash_Click;
				}
				withEventsField_gbSplash = value;
				if (withEventsField_gbSplash != null) {
					withEventsField_gbSplash.Click += gbSplash_Click;
				}
			}
		}
		internal System.Windows.Forms.Label lblSplash;
		internal System.Windows.Forms.Label lblActivity;
		internal System.Windows.Forms.Label lblStatus;
		internal PictureBox pbIcon;
		private System.Windows.Forms.Timer withEventsField_timSplash;
		internal System.Windows.Forms.Timer timSplash {
			get { return withEventsField_timSplash; }
			set {
				if (withEventsField_timSplash != null) {
					withEventsField_timSplash.Tick -= timSplash_Tick;
				}
				withEventsField_timSplash = value;
				if (withEventsField_timSplash != null) {
					withEventsField_timSplash.Tick += timSplash_Tick;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gbSplash = new System.Windows.Forms.GroupBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblActivity = new System.Windows.Forms.Label();
			this.lblSplash = new System.Windows.Forms.Label();
			this.timSplash = new System.Windows.Forms.Timer(this.components);
			this.pbIcon = new System.Windows.Forms.PictureBox();
			this.gbSplash.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pbIcon).BeginInit();
			this.SuspendLayout();
			//
			//gbSplash
			//
			this.gbSplash.Controls.Add(this.pbIcon);
			this.gbSplash.Controls.Add(this.lblStatus);
			this.gbSplash.Controls.Add(this.lblActivity);
			this.gbSplash.Controls.Add(this.lblSplash);
			this.gbSplash.Location = new System.Drawing.Point(8, 8);
			this.gbSplash.Name = "gbSplash";
			this.gbSplash.Size = new System.Drawing.Size(428, 136);
			this.gbSplash.TabIndex = 1;
			this.gbSplash.TabStop = false;
			//
			//lblStatus
			//
			this.lblStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblStatus.Location = new System.Drawing.Point(8, 108);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(416, 23);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.Text = "lblStatus";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lblActivity
			//
			this.lblActivity.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblActivity.Location = new System.Drawing.Point(8, 52);
			this.lblActivity.Name = "lblActivity";
			this.lblActivity.Size = new System.Drawing.Size(416, 52);
			this.lblActivity.TabIndex = 2;
			this.lblActivity.Text = "lblAvtivity";
			this.lblActivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//lblSplash
			//
			this.lblSplash.Font = new System.Drawing.Font("Verdana", 15.75f, (System.Drawing.FontStyle)(System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic), System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblSplash.Location = new System.Drawing.Point(40, 12);
			this.lblSplash.Name = "lblSplash";
			this.lblSplash.Size = new System.Drawing.Size(381, 33);
			this.lblSplash.TabIndex = 1;
			this.lblSplash.Text = "Loading...";
			this.lblSplash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//timSplash
			//
			this.timSplash.Enabled = true;
			//
			//pbIcon
			//
			this.pbIcon.Location = new System.Drawing.Point(6, 12);
			this.pbIcon.Name = "pbIcon";
			this.pbIcon.Size = new System.Drawing.Size(32, 32);
			this.pbIcon.TabIndex = 4;
			this.pbIcon.TabStop = false;
			//
			//frmSplash
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(444, 152);
			this.ControlBox = false;
			this.Controls.Add(this.gbSplash);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmSplash";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmSplash";
			this.gbSplash.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pbIcon).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private int mParentHeight;
		private int mParentWidth;
		private int mParentX;
		private int mParentY;
		public void UpdateStatus(string Status)
		{
			this.lblStatus.Text = Status;
		}
		private void frmSplash_Activated(object sender, System.EventArgs e)
		{
			timSplash.Enabled = true;
		}
		private void frmSplash_Deactivate(object sender, System.EventArgs e)
		{
			timSplash.Enabled = false;
		}
		private void frmSplash_Load(object sender, System.EventArgs e)
		{
			this.SetBounds(mParentX + ((mParentWidth - this.Width) / 2), mParentY + ((mParentHeight - this.Height) / 2), 0, 0, BoundsSpecified.Location);
			this.Cursor = Cursors.WaitCursor;
		}
		private void timSplash_Tick(object sender, System.EventArgs e)
		{
			Application.DoEvents();
		}
		private void gbSplash_Click(object sender, System.EventArgs e)
		{
			if (this.Modal)
				this.Close();
		}
	}
}
