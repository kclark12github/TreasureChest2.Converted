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
//frmAbout.vb
//   Help About Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/19/14    Upgraded to VS2013;
//   12/05/09    Started History;
//=================================================================================================================================
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	public class frmAbout : frmTCBase
	{
		const string myFormName = "frmAbout";
		public frmAbout(clsSupport objSupport, TCBase.clsTCBase mTCBase, Form objParent) : base(objSupport, myFormName, mTCBase, objParent)
		{
			Load += frmAbout_Load;
			Activated += frmAbout_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
			this.Icon = objParent.Icon;
			this.Text = "Help About " + mSupport.ApplicationName;
		}
		#region " Windows Form Designer generated code "

		public frmAbout() : base()
		{
			Load += frmAbout_Load;
			Activated += frmAbout_Activated;

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
		internal System.Windows.Forms.Label lblDisclaimer;
		internal System.Windows.Forms.Label lblDescription;
		internal System.Windows.Forms.Label lblVersion;
		internal System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.PictureBox withEventsField_pbImage;
		internal System.Windows.Forms.PictureBox pbImage {
			get { return withEventsField_pbImage; }
			set {
				if (withEventsField_pbImage != null) {
					withEventsField_pbImage.DoubleClick -= pbImage_DoubleClick;
				}
				withEventsField_pbImage = value;
				if (withEventsField_pbImage != null) {
					withEventsField_pbImage.DoubleClick += pbImage_DoubleClick;
				}
			}
		}
		private System.Windows.Forms.DataGrid withEventsField_dgComponents;
		internal System.Windows.Forms.DataGrid dgComponents {
			get { return withEventsField_dgComponents; }
			set {
				if (withEventsField_dgComponents != null) {
					withEventsField_dgComponents.CurrentCellChanged -= dgComponents_CurrentCellChanged;
				}
				withEventsField_dgComponents = value;
				if (withEventsField_dgComponents != null) {
					withEventsField_dgComponents.CurrentCellChanged += dgComponents_CurrentCellChanged;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAbout));
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pbImage = new System.Windows.Forms.PictureBox();
			this.dgComponents = new System.Windows.Forms.DataGrid();
			((System.ComponentModel.ISupportInitialize)this.dgComponents).BeginInit();
			this.SuspendLayout();
			//
			//lblDisclaimer
			//
			this.lblDisclaimer.BackColor = System.Drawing.Color.Transparent;
			this.lblDisclaimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblDisclaimer.ForeColor = System.Drawing.Color.White;
			this.lblDisclaimer.Location = new System.Drawing.Point(5, 716);
			this.lblDisclaimer.Name = "lblDisclaimer";
			this.lblDisclaimer.Size = new System.Drawing.Size(712, 40);
			this.lblDisclaimer.TabIndex = 14;
			this.lblDisclaimer.Text = "Warning: Unauthorized use of this product is subject to prosecution under United " + "States and International copyright laws. This software is licensed, not owned.";
			this.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			//
			//lblDescription
			//
			this.lblDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblDescription.ForeColor = System.Drawing.Color.Silver;
			this.lblDescription.Location = new System.Drawing.Point(7, 104);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(708, 88);
			this.lblDescription.TabIndex = 13;
			this.lblDescription.Text = "App Description";
			//
			//lblVersion
			//
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblVersion.ForeColor = System.Drawing.Color.White;
			this.lblVersion.Location = new System.Drawing.Point(7, 64);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(708, 32);
			this.lblVersion.TabIndex = 12;
			this.lblVersion.Text = "Version";
			//
			//lblTitle
			//
			this.lblTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(7, 8);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(708, 44);
			this.lblTitle.TabIndex = 11;
			this.lblTitle.Text = "Application Title";
			//
			//pbImage
			//
			this.pbImage.Image = (System.Drawing.Image)resources.GetObject("pbImage.Image");
			this.pbImage.Location = new System.Drawing.Point(4, 4);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(716, 760);
			this.pbImage.TabIndex = 16;
			this.pbImage.TabStop = false;
			//
			//dgComponents
			//
			this.dgComponents.CaptionText = "Loaded Assemblies";
			this.dgComponents.DataMember = "";
			this.dgComponents.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dgComponents.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgComponents.Location = new System.Drawing.Point(9, 380);
			this.dgComponents.Name = "dgComponents";
			this.dgComponents.ReadOnly = true;
			this.dgComponents.Size = new System.Drawing.Size(704, 320);
			this.dgComponents.TabIndex = 19;
			//
			//frmAbout
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
			this.ClientSize = new System.Drawing.Size(722, 764);
			this.Controls.Add(this.dgComponents);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.pbImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmAbout";
			((System.ComponentModel.ISupportInitialize)this.dgComponents).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		#region "Properties"
		#region "Declarations"
		private DataSet dsAssemblies;
		private DataTable dtAssemblies;
		private DataTable dtTypes;
		private DataTable dtMembers;
			#endregion
		private bool fActivated = false;
		#endregion
		#region "Methods"
		private void AddCustomDataTableStyle()
		{
			DataGridTableStyle tsAssemblies = new DataGridTableStyle();
			var _with1 = tsAssemblies;
			_with1.MappingName = "Assemblies";
			_with1.AlternatingBackColor = Color.Snow;

			DataGridTextBoxColumn colID = new DataGridTextBoxColumn();
			var _with2 = colID;
			_with2.MappingName = "ID";
			_with2.HeaderText = "ID";
			_with2.Width = 0;
			_with1.GridColumnStyles.Add(colID);

			DataGridTextBoxColumn colAssemblyName = new DataGridTextBoxColumn();
			var _with3 = colAssemblyName;
			_with3.MappingName = "Name";
			_with3.HeaderText = "Assembly Name";
			_with3.Width = GetDataWidth(dtAssemblies, colAssemblyName);
			_with1.GridColumnStyles.Add(colAssemblyName);

			DataGridTextBoxColumn colVersion = new DataGridTextBoxColumn();
			var _with4 = colVersion;
			_with4.MappingName = "Version";
			_with4.HeaderText = "Version";
			_with4.Width = GetDataWidth(dtAssemblies, colVersion);
			_with1.GridColumnStyles.Add(colVersion);

			DataGridTextBoxColumn colPath = new DataGridTextBoxColumn();
			var _with5 = colPath;
			_with5.MappingName = "Path";
			_with5.HeaderText = "Assembly Path";
			_with5.Width = GetDataWidth(dtAssemblies, colPath);
			_with1.GridColumnStyles.Add(colPath);

			DataGridTableStyle tsTypes = new DataGridTableStyle();
			var _with6 = tsTypes;
			_with6.MappingName = "Types";
			_with6.AlternatingBackColor = Color.Snow;

			DataGridTextBoxColumn colAssemblyID = new DataGridTextBoxColumn();
			var _with7 = colAssemblyID;
			_with7.MappingName = "AssemblyID";
			_with7.HeaderText = "Assembly ID";
			_with7.Width = 0;
			_with6.GridColumnStyles.Add(colAssemblyID);

			DataGridTextBoxColumn colType = new DataGridTextBoxColumn();
			var _with8 = colType;
			_with8.MappingName = "TypeName";
			_with8.HeaderText = "Type Name";
			_with8.Width = GetDataWidth(dtTypes, colType);
			_with6.GridColumnStyles.Add(colType);

			DataGridTableStyle tsMembers = new DataGridTableStyle();
			var _with9 = tsMembers;
			_with9.MappingName = "Members";
			_with9.AlternatingBackColor = Color.Snow;

			DataGridTextBoxColumn colTypeID = new DataGridTextBoxColumn();
			var _with10 = colTypeID;
			_with10.MappingName = "TypeID";
			_with10.HeaderText = "Type ID";
			_with10.Width = 0;
			_with9.GridColumnStyles.Add(colTypeID);

			DataGridTextBoxColumn colMemberName = new DataGridTextBoxColumn();
			var _with11 = colMemberName;
			_with11.MappingName = "MemberName";
			_with11.HeaderText = "Member Name";
			_with11.Width = GetDataWidth(dtMembers, colMemberName);
			_with9.GridColumnStyles.Add(colMemberName);

			DataGridTextBoxColumn colMemberType = new DataGridTextBoxColumn();
			var _with12 = colMemberType;
			_with12.MappingName = "MemberType";
			_with12.HeaderText = "Member Type";
			_with12.Width = GetDataWidth(dtMembers, colMemberType);
			_with9.GridColumnStyles.Add(colMemberType);

			DataGridTextBoxColumn colSignature = new DataGridTextBoxColumn();
			var _with13 = colSignature;
			_with13.MappingName = "Signature";
			_with13.HeaderText = "Signature";
			_with13.Width = GetDataWidth(dtMembers, colSignature);
			_with9.GridColumnStyles.Add(colSignature);

			DataGridTextBoxColumn colDeclaredIn = new DataGridTextBoxColumn();
			var _with14 = colDeclaredIn;
			_with14.MappingName = "DeclaredIn";
			_with14.HeaderText = "Declared In";
			_with14.Width = GetDataWidth(dtMembers, colDeclaredIn);
			_with9.GridColumnStyles.Add(colDeclaredIn);

			DataGridTextBoxColumn colAttributes = new DataGridTextBoxColumn();
			var _with15 = colAttributes;
			_with15.MappingName = "Attributes";
			_with15.HeaderText = "Attributes";
			_with15.Width = GetDataWidth(dtMembers, colAttributes);
			_with9.GridColumnStyles.Add(colAttributes);

			//' Use a PropertyDescriptor to create a formatted
			//' column. First get the PropertyDescriptorCollection
			//' for the data source and data member. 
			//Dim pcol As PropertyDescriptorCollection = Me.BindingContext(myDataSet, "Customers.custToOrders").GetItemProperties()

			//' Create a formatted column using a PropertyDescriptor.
			//' The formatting character "c" specifies a currency format. */     

			//Dim csOrderAmount As New DataGridTextBoxColumn(pcol("OrderAmount"), "c", True)
			//csOrderAmount.MappingName = "OrderAmount"
			//csOrderAmount.HeaderText = "Total"
			//csOrderAmount.Width = 100
			//ts2.GridColumnStyles.Add(csOrderAmount)

			// Add the DataGridTableStyle instances to 
			// the GridTableStylesCollection. 
			dgComponents.TableStyles.Add(tsAssemblies);
			dgComponents.TableStyles.Add(tsTypes);
			dgComponents.TableStyles.Add(tsMembers);
		}
		private void CreateDataSet()
		{
			//Create an in-memory DataSet of the loaded assemblies and related details for display...
			dtAssemblies = new DataTable("Assemblies");
			var _with16 = dtAssemblies;
			_with16.Columns.Add(new DataColumn("ID", typeof(int)));
			_with16.Columns.Add(new DataColumn("Name", typeof(string)));
			_with16.Columns.Add(new DataColumn("Version", typeof(string)));
			_with16.Columns.Add(new DataColumn("Path", typeof(string)));

			dtTypes = new DataTable("Types");
			var _with17 = dtTypes;
			_with17.Columns.Add(new DataColumn("ID", typeof(int)));
			_with17.Columns.Add(new DataColumn("AssemblyID", typeof(int)));
			_with17.Columns.Add(new DataColumn("TypeName", typeof(string)));

			dtMembers = new DataTable("Members");
			var _with18 = dtMembers;
			_with18.Columns.Add(new DataColumn("ID", typeof(int)));
			_with18.Columns.Add(new DataColumn("TypeID", typeof(int)));
			_with18.Columns.Add(new DataColumn("MemberName", typeof(string)));
			_with18.Columns.Add(new DataColumn("MemberType", typeof(string)));
			_with18.Columns.Add(new DataColumn("Signature", typeof(string)));
			_with18.Columns.Add(new DataColumn("DeclaredIn", typeof(string)));
			_with18.Columns.Add(new DataColumn("Attributes", typeof(string)));

			dsAssemblies = new DataSet("Assemblies");
			dsAssemblies.Tables.Add(dtAssemblies);
			dsAssemblies.Tables.Add(dtTypes);
			dsAssemblies.Tables.Add(dtMembers);

			dsAssemblies.Relations.Add(new DataRelation("TypesToMembers", dtTypes.Columns["ID"], dtMembers.Columns["TypeID"]));
			dsAssemblies.Relations.Add(new DataRelation("AssembliesToTypes", dtAssemblies.Columns["ID"], dtTypes.Columns["AssemblyID"]));

			int iAssembly = 0;
			int iType = 0;
			int iMember = 0;
			Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly asm in asms) {
				DataRow newRow = dtAssemblies.NewRow();
				newRow["ID"] = iAssembly;
				newRow["Name"] = asm.GetName().Name;
				newRow["Path"] = asm.Location;
				newRow["Version"] = asm.GetName().Version.ToString();
				dtAssemblies.Rows.Add(newRow);
				foreach (Type t in asm.GetTypes()) {
					newRow = dtTypes.NewRow();
					newRow["ID"] = iType;
					newRow["AssemblyID"] = iAssembly;
					newRow["TypeName"] = t.FullName;
					dtTypes.Rows.Add(newRow);

					foreach (MemberInfo m in t.GetMembers()) {
						//If m.Name = "SetSplashStatus" Then
						//    Debug.WriteLine("We're Here!")
						//End If
						//If m.GetType.IsPublic Then
						newRow = dtMembers.NewRow();
						newRow["ID"] = iMember;
						newRow["TypeID"] = iType;
						newRow["MemberName"] = m.Name;
						newRow["DeclaredIn"] = string.Format("{0} ({1})", m.DeclaringType.Name, m.DeclaringType.FullName);
						newRow["MemberType"] = m.MemberType.ToString();
						newRow["Signature"] = m.ToString();
						switch (m.MemberType) {
							case MemberTypes.Constructor:
								newRow["Attributes"] = ((ConstructorInfo)m).Attributes;
								break;
							case MemberTypes.Method:
								newRow["Attributes"] = ((MethodInfo)m).Attributes;
								break;
							case MemberTypes.Event:
								newRow["Attributes"] = ((EventInfo)m).Attributes;
								break;
							case MemberTypes.Property:
								newRow["Attributes"] = ((PropertyInfo)m).Attributes;
								break;
							default:
								break;
						}
						//newRow("Attributes") = m.GetType.Attributes
						dtMembers.Rows.Add(newRow);
						iMember += 1;
						//End If
					}
					iType += 1;
				}
				iAssembly += 1;
			}
			dsAssemblies.AcceptChanges();
			AddCustomDataTableStyle();
			dgComponents.SetDataBinding(dsAssemblies, "Assemblies");
		}
		private int GetDataWidth(DataTable dataTable, DataGridColumnStyle ColumnStyle)
		{
			int Width = 0;
			Graphics g = dgComponents.CreateGraphics();
			int offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", dgComponents.Font).Width));
			Width = Convert.ToInt32(Math.Ceiling(g.MeasureString(ColumnStyle.HeaderText, dgComponents.HeaderFont).Width));
			for (int i = 0; i <= dataTable.Rows.Count - 1; i++) {
				if (!Information.IsDBNull(dataTable.Rows[i][ColumnStyle.MappingName])) {
					int iWidth = Convert.ToInt32(Math.Ceiling(g.MeasureString(Strings.Format(dataTable.Rows[i][ColumnStyle.MappingName], ((DataGridTextBoxColumn)ColumnStyle).Format), dgComponents.Font).Width));
					if (iWidth > Width)
						Width = iWidth;
				}
			}
			return Width + (offset * 4);
		}
		private int GetHeaderWidth(DataGridColumnStyle ColumnStyle)
		{
			Graphics g = dgComponents.CreateGraphics();
			int offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", dgComponents.HeaderFont).Width));
			return Convert.ToInt32(Math.Ceiling(g.MeasureString(ColumnStyle.HeaderText, dgComponents.HeaderFont).Width)) + (offset * 4);
		}
		#endregion
		#region "Event Handlers"
		private void dgComponents_CurrentCellChanged(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				dgComponents.Select(dgComponents.CurrentRowIndex);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void frmAbout_Activated(object sender, System.EventArgs e)
		{
			frmSplash frm = null;
			try {
				if (fActivated)
                    throw new ExitTryException();
				fActivated = true;
				this.Cursor = Cursors.WaitCursor;
				frm = new frmSplash();
				var _with19 = frm;
				_with19.Cursor = Cursors.WaitCursor;
				_with19.lblActivity.Text = "Assembly Version Data...";
				_with19.lblStatus.Text = bpeNullString;
				_with19.SetBounds(this.Left + ((this.Width - _with19.Width) / 2), this.Top + ((this.Height - _with19.Height) / 2), 0, 0, BoundsSpecified.Location);
				_with19.Show();
				Application.DoEvents();

				CreateDataSet();
            } catch (ExitTryException) {
            } catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				if ((frm != null)){frm.Close();frm = null;}
				this.Cursor = Cursors.Default;
			}
		}
		private void frmAbout_Load(object sender, System.EventArgs e)
		{
			try {
				this.lblTitle.Text = mSupport.ApplicationName;
				this.lblVersion.Text = "Version " + mSupport.ApplicationVersion;
				this.lblDescription.Text = bpeNullString;
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		protected void pbImage_DoubleClick(System.Object sender, System.EventArgs e)
		{
			try {
				frmImage frm = new frmImage(mSupport, mTCBase, this, this.pbImage.Image);
				frm.Icon = this.Icon;
				frm.Text = this.Text;
				frm.ShowDialog(this);
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		#endregion
	}
}
