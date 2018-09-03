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
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmViewer : frmTCBase
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing) {
					if (components != null)
						components.Dispose();
					if (BooksTableAdapter != null)
						BooksTableAdapter.Dispose();
					if (ReportDataSet != null)
						ReportDataSet.Dispose();
				}

			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Microsoft.Reporting.WinForms.ReportDataSource ReportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.ReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.ReportDataSet = new TCBase.ReportDataSet();
			this.rdlcViewer = new Microsoft.Reporting.WinForms.ReportViewer();
			this.BooksTableAdapter = new TCBase.ReportDataSetTableAdapters.BooksTableAdapter();
			((System.ComponentModel.ISupportInitialize)this.ReportBindingSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.ReportDataSet).BeginInit();
			this.SuspendLayout();
			//
			//ReportBindingSource
			//
			this.ReportBindingSource.DataMember = "";
			this.ReportBindingSource.DataSource = this.ReportDataSet;
			//
			//ReportDataSet
			//
			this.ReportDataSet.DataSetName = "ReportDataSet";
			this.ReportDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			//
			//rdlcViewer
			//
			this.rdlcViewer.AutoSize = true;
			this.rdlcViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			ReportDataSource.Name = "ReportDataSet";
			ReportDataSource.Value = this.ReportBindingSource;
			this.rdlcViewer.LocalReport.DataSources.Add(ReportDataSource);
			this.rdlcViewer.LocalReport.ReportEmbeddedResource = "TCBase.Books.rdlc";
			this.rdlcViewer.Location = new System.Drawing.Point(0, 0);
			this.rdlcViewer.Name = "rdlcViewer";
			this.rdlcViewer.Size = new System.Drawing.Size(1054, 346);
			this.rdlcViewer.TabIndex = 0;
			this.rdlcViewer.UseWaitCursor = true;
			//
			//BooksTableAdapter
			//
			this.BooksTableAdapter.ClearBeforeFill = true;
			//
			//frmViewer
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ClientSize = new System.Drawing.Size(1054, 346);
			this.Controls.Add(this.rdlcViewer);
			this.Name = "frmViewer";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "frmViewer";
			((System.ComponentModel.ISupportInitialize)this.ReportBindingSource).EndInit();
			((System.ComponentModel.ISupportInitialize)this.ReportDataSet).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal Microsoft.Reporting.WinForms.ReportViewer rdlcViewer;
		internal System.Windows.Forms.BindingSource ReportBindingSource;
		internal ReportDataSet ReportDataSet;
		internal ReportDataSetTableAdapters.BooksTableAdapter BooksTableAdapter;
	}
}
