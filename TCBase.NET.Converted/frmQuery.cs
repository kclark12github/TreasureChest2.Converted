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
//frmSQL.vb
//   SQL Query Interface...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/27/09    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	internal class frmQuery : frmTCBase
	{
		public frmQuery() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		public frmQuery(clsSupport objSupport, clsTCBase objTCBase, Form objParent = null, string Caption = null) : base(objSupport, "frmQuery", objTCBase, objParent)
		{

			InitializeComponent();
			if ((Caption != null))
				this.Text = Caption;
			mSQLForm = (frmSQL)objParent;
			var _with1 = tooltipQuery;
			_with1.SetToolTip(this.txtName, "Query Name");
			_with1.SetToolTip(this.txtDescription, "Description of Query");
			this.Icon = objParent.Icon;
		}

		#region " Windows Form Designer generated code "
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		public System.Windows.Forms.ToolTip tooltipQuery;
		internal System.Windows.Forms.Label lblName;
		internal System.Windows.Forms.Label lblDescription;
		internal System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox withEventsField_txtDescription;
		internal System.Windows.Forms.TextBox txtDescription {
			get { return withEventsField_txtDescription; }
			set {
				if (withEventsField_txtDescription != null) {
					withEventsField_txtDescription.Validating -= txtDescription_Validating;
					withEventsField_txtDescription.Validating -= txtName_Validating;
				}
				withEventsField_txtDescription = value;
				if (withEventsField_txtDescription != null) {
					withEventsField_txtDescription.Validating += txtDescription_Validating;
					withEventsField_txtDescription.Validating += txtName_Validating;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdOK;
		internal System.Windows.Forms.Button cmdOK {
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
		private System.Windows.Forms.Button withEventsField_cmdCancel;
		internal System.Windows.Forms.Button cmdCancel {
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
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tooltipQuery = new System.Windows.Forms.ToolTip(this.components);
			this.lblName = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			//lblName
			//
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(20, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(34, 16);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			//
			//lblDescription
			//
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(20, 44);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(61, 16);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "Description";
			//
			//txtName
			//
			this.txtName.Location = new System.Drawing.Point(92, 16);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(272, 20);
			this.txtName.TabIndex = 3;
			this.txtName.Text = "";
			//
			//txtDescription
			//
			this.txtDescription.Location = new System.Drawing.Point(92, 44);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(272, 76);
			this.txtDescription.TabIndex = 4;
			this.txtDescription.Text = "";
			//
			//cmdOK
			//
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(112, 132);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.TabIndex = 5;
			this.cmdOK.Text = "OK";
			//
			//cmdCancel
			//
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(192, 132);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.TabIndex = 6;
			this.cmdCancel.Text = "Cancel";
			//
			//frmQuery
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 168);
			this.ControlBox = false;
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmQuery";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmQuery";
			this.ResumeLayout(false);

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		private frmSQL mSQLForm;
		private bool mUnique;
		public string QueryDescription {
			get { return Strings.Trim(this.txtDescription.Text); }
			set { this.txtDescription.Text = value; }
		}
		public string QueryName {
			get { return this.txtName.Text; }
			set { this.txtName.Text = value; }
		}
		public bool Unique {
			get { return mUnique; }
			set { mUnique = value; }
		}
		#endregion
		#region "Methods"
		//None at this time...
		#endregion
		#region "Event Handlers"
		private void cmdCancel_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				this.Hide();
			} catch (Exception ex) {
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void cmdOK_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				this.Hide();
			} catch (Exception ex) {
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void txtDescription_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				var _with2 = this.txtDescription;
				_with2.Text = Strings.Trim(_with2.Text);
				if (_with2.Text == bpeNullString) {
					this.epBase.SetError((Control)sender, "Description must be specified!");
					e.Cancel = true;
				}
			} catch (Exception ex) {
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try {
				this.epBase.SetError((Control)sender, bpeNullString);
				var _with3 = this.txtDescription;
				_with3.Text = Strings.Trim(_with3.Text);
				if (_with3.Text == bpeNullString) {
					this.epBase.SetError((Control)sender, "Name must be specified!");
					e.Cancel = true;
				}
				if (mUnique) {
					if (mSQLForm.CheckForDup(_with3.Text)) {
						this.epBase.SetError((Control)sender, "Query with this name already exists!");
						e.Cancel = true;
					}
				}
			} catch (Exception ex) {
				this.epBase.SetError((Control)sender, ex.Message);
			}
		}
		#endregion
	}
}
