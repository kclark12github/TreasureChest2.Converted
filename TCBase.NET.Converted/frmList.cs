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
	//frmList.vb
	//   List Form...
	//   Copyright © 1998-2018, Ken Clark
	//*********************************************************************************************************************************
	//
	//   Modification History:
	//   Date:       Description:
	//   12/21/17    Addressed CurrencyManager synchronization issue introduced by image processing (RemoveImage) that had caused 
	//               tcDataView_ListChanged to fire after moving to the new row while cleaning up image memory used by the old row;
	//   06/09/17    Addressed NullReferenceException in dgList_CurrentCellChanged;
	//   10/01/16    Implemented DataGridView;
	//   09/18/16    Updated object references to reflect architectural changes;
	//   10/19/14    Upgraded to VS2013;
	//   10/22/11    Added Return/Escape/Enter KeyUp handling to close screen;
	//   09/18/10    Added logic to save column widths;
	//   07/26/10    Reorganized registry settings;
	//   11/28/09    Migrated to VB.NET;
	//   12/28/07    Corrected sorting by setting .Layout.Override.FetchRows = ssFetchRowsPreloadWithParent in
	//               ssugList_InitializeLayout;
	//   10/14/02    Added Error Handling;
	//   10/13/02    Refitted for use in TreasureChest;
	//=================================================================================================================================
	//Notes:
	//   - Add error handling...
	//   - Column sizing... Especially ID column...
	//   - Currency column formatting...
	//   - Fully Test...
	//=================================================================================================================================
	public class frmList : TCBase.frmTCBase
	{
		public frmList(clsSupport objSupport, clsTCBase objTCBase, Form objParent, string Caption, DataView DataSource, int Position, bool AllowUpdate, string KeyField) : base(objSupport, "frmList", objTCBase, objParent)
		{
			KeyUp += Form_KeyUp;
			Closing += frmList_Closing;
			Load += frmList_Load;
			InitializeComponent();
			if ((Caption != null))
				this.Text = Caption;

			mRegistryKey = string.Format("{0}\\{1} Settings\\List Settings", mTCBase.RegistryKey, mTCBase.ActiveForm.Name);
			mKeyField = KeyField;
			this.Icon = mTCBase.ActiveForm.Icon;
			this.dgList.DataSource = DataSource;
			this.Text = "Unfiltered";
			if ((DataSource.RowFilter != null) && DataSource.RowFilter != bpeNullString)
				this.Text = "RowFilter: " + DataSource.RowFilter;
			this.Text += string.Format(" ({0:#,##0} Rows)", DataSource.Count);
			this.dgList.ReadOnly = !AllowUpdate;
			this.dgList.CurrentCell = this.dgList[0, Position];
			this.dgList.CurrentRow.Selected = true;
			mTCBase.ListChanged += tcDataView_ListChanged;
		}
		#region " Windows Form Designer generated code "
		public frmList() : base()
		{
			KeyUp += Form_KeyUp;
			Closing += frmList_Closing;
			Load += frmList_Load;
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
		private System.Windows.Forms.DataGridView withEventsField_dgList;
		internal System.Windows.Forms.DataGridView dgList {
			get { return withEventsField_dgList; }
			set {
				if (withEventsField_dgList != null) {
					withEventsField_dgList.ColumnWidthChanged -= dgList_ColumnWidthChanged;
					withEventsField_dgList.CurrentCellChanged -= dgList_CurrentCellChanged;
					withEventsField_dgList.MouseDown -= dgList_MouseDown;
					withEventsField_dgList.Sorted -= dgList_Sorted;
				}
				withEventsField_dgList = value;
				if (withEventsField_dgList != null) {
					withEventsField_dgList.ColumnWidthChanged += dgList_ColumnWidthChanged;
					withEventsField_dgList.CurrentCellChanged += dgList_CurrentCellChanged;
					withEventsField_dgList.MouseDown += dgList_MouseDown;
					withEventsField_dgList.Sorted += dgList_Sorted;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.dgList = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)this.dgList).BeginInit();
			this.SuspendLayout();
			//
			//dgList
			//
			this.dgList.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.dgList.DataMember = "";
			this.dgList.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dgList.Location = new System.Drawing.Point(0, 0);
			this.dgList.Name = "dgList";
			this.dgList.Size = new System.Drawing.Size(448, 268);
			this.dgList.TabIndex = 0;
			//
			//frmList
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(444, 266);
			this.Controls.Add(this.dgList);
			this.KeyPreview = true;
			this.Name = "frmList";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmList";
			((System.ComponentModel.ISupportInitialize)this.dgList).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region "Properties"
		#region "Declarations"
		private string mKeyField;
		private int mRecordID;
		private string mRegistryKey;
			#endregion
		private bool mSorting = false;
		#endregion
		#region "Methods"
		protected void ResetGrid()
		{
			System.Drawing.Color altBackColor = Color.Snow;
			//Color.NavajoWhite  'Color.MintCream
			var _with1 = this.dgList;
			_with1.BackgroundColor = SystemColors.InactiveCaptionText;

			_with1.ResetBackColor();
			_with1.ResetForeColor();
			_with1.ResetText();

			_with1.AllowUserToAddRows = false;
			_with1.AllowUserToDeleteRows = false;
			_with1.AllowUserToOrderColumns = true;
			_with1.AllowUserToResizeColumns = true;
			_with1.AllowUserToResizeRows = true;
			_with1.BackColor = Color.GhostWhite;
			_with1.BackgroundColor = SystemColors.AppWorkspace;
			_with1.BorderStyle = BorderStyle.Fixed3D;
			_with1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			_with1.EditMode = DataGridViewEditMode.EditProgrammatically;
			//.Font = New Font("Tahoma", 8.0!)
			_with1.ForeColor = Color.MidnightBlue;
			_with1.ReadOnly = true;
			_with1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			_with1.RowsDefaultCellStyle.BackColor = Color.GhostWhite;
			_with1.AlternatingRowsDefaultCellStyle.BackColor = altBackColor;
			_with1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

			ResetColumns();

			this.dgList.Focus();
			//If Me.dgList.CurrentRow.Index > 0 Then Me.dgList.Select(Me.dgList.CurrentRowIndex) : Me.dgList.CurrentCell = New DataGridCell(Me.dgList.CurrentRowIndex, 1)
			dgList.Visible = true;
		}
		protected void ResetColumns()
		{
			DataGridViewContentAlignment Alignment = DataGridViewContentAlignment.TopLeft;
			string Format = "";
			int Order = 0;
			bool Visible = true;
			int Width = 0;

			foreach (DataGridViewColumn iColumn in this.dgList.Columns) {
				Alignment = DataGridViewContentAlignment.TopLeft;
				Format = "";
				Visible = true;
				Order = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, string.Format("ColumnOrder.{0}", iColumn.Name), iColumn.DisplayIndex);
				Width = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, string.Format("ColumnWidth.{0}", iColumn.Name), iColumn.Width);
				switch (iColumn.ValueType.Name) {
					case "Byte[]":
						Visible = false;
						break;
					case "Currency":
					case "Money":
					case "Decimal":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = "$#,##0.00";
						break;
					case "Boolean":
						Alignment = DataGridViewContentAlignment.TopCenter;
						break;
					case "Date":
					case "DateTime":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = "MM/dd/yyyy hh:mm tt";
						break;
					case "Integer":
					case "Int16":
					case "Int32":
					case "Int64":
					case "Short":
					case "Long":
					case "Double":
					case "Single":
						Alignment = DataGridViewContentAlignment.TopRight;
						Format = (iColumn.Name == "CONVERT(BIGINT,ROWID)" ? "X16" : "0");
						break;
					case "String":
					case "Text":
						if (iColumn.Name == "Notes")
							Visible = false;
						break;
				}
				iColumn.DefaultCellStyle.Alignment = Alignment;
				iColumn.DefaultCellStyle.Format = Format;
				iColumn.DisplayIndex = Order;
				iColumn.Visible = Visible;
				iColumn.Width = Width;
			}
		}
		#endregion
		#region "Event Handlers"
		private void dgList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			mSorting = false;
			//False-positive in MouseDown thinking we're sorting...
		}
		private void dgList_CurrentCellChanged(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				if (!mSorting && this.dgList.CurrentRow != null)
					mRecordID = (int)this.dgList.CurrentRow.Cells[mKeyField].Value;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void dgList_MouseDown(object sender, MouseEventArgs e)
		{
			System.Windows.Forms.DataGridView.HitTestInfo hit = dgList.HitTest(e.X, e.Y);
			if (hit.Type == DataGridViewHitTestType.ColumnHeader)
				mSorting = true;
		}
		private void dgList_Sorted(object sender, EventArgs e)
		{
			//Debug.WriteLine(String.Format("Sorted: CurrentRow.Index: {0}", Me.dgList.CurrentRow.Index))
			try {
				foreach (DataGridViewRow iRow in this.dgList.Rows) {
					if ((int)iRow.Cells[mKeyField].Value == mRecordID) {
						this.dgList.CurrentCell = this.dgList[0, iRow.Index];
						break; 
					}
				}
			} finally {
				mSorting = false;
			}
		}
		private void frmList_Load(object sender, System.EventArgs e)
		{
			int iLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Left", 0);
			int iTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Top", 0);
			int iWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Width", Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width));
			int iHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Height", Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height));
			this.SetBounds(iLeft, iTop, iWidth, iHeight);
			ResetGrid();
		}
		private void frmList_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
				mTCBase.SaveBounds(mRegistryKey, this.Left, this.Top, this.Width, this.Height);
			foreach (DataGridViewColumn iColumn in this.dgList.Columns) {
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, string.Format("ColumnOrder.{0}", iColumn.Name), Convert.ToInt32(iColumn.DisplayIndex));
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, string.Format("ColumnWidth.{0}", iColumn.Name), Convert.ToInt32(iColumn.Width));
			}
			mTCBase.ListChanged -= tcDataView_ListChanged;
			mTCBase.Move(this.dgList.CurrentRow.Index, ListChangedType.ItemChanged);
		}
		protected void Form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try {
				e.Handled = false;
				switch (e.KeyCode) {
					case Keys.Enter:
					//case Keys.Return:
					case Keys.Escape:
						e.Handled = true;
						this.Close();
						break;
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void tcDataView_ListChanged(object sender, ListChangedEventArgs e)
		{
			//Calling TraceDataView() [even when not tracing] avoids the nagging Binding exception 
			//when canceling a filter due to no remaining qualifying records...
			// - I don't know why...
			mTCBase.TraceDataView(true);
			CurrencyManager cm = mCurrencyManager;
			if (cm == null)
				cm = (CurrencyManager)this.BindingContext[mTCBase.MainDataView];
			if (cm == null)
				return;

			switch (e.ListChangedType) {
				case ListChangedType.ItemAdded:
					//An item added to the list. ListChangedEventArgs.NewIndex contains the index of the item that was added. 
					Trace("Handling {0} event from {1}; Positioning to Row #{2}", e.ListChangedType.ToString(), sender.ToString(), e.NewIndex, trcOption.trcApplication);
					if (cm.Position == e.NewIndex) {
						Trace("...but Current position is already set to NewIndex ({0});", e.NewIndex, trcOption.trcApplication);
					} else {
						if (e.NewIndex < 0) {
							Trace("...but NewIndex ({0}) is less than zero; (OldIndex:={1}); - I don't understand what's going on here...!", e.NewIndex, e.OldIndex, trcOption.trcApplication);
							return;
						} else if (e.NewIndex >= cm.List.Count) {
							Trace("...but NewIndex ({0}) is beyond our List.Count; (OldIndex:={1}); - I don't understand what's going on here...!", e.NewIndex, e.OldIndex, trcOption.trcApplication);
							return;
						}
						mTCBase.Move(e.NewIndex, e.ListChangedType);
					}
					break;
				case ListChangedType.ItemChanged:
					//An item changed in the list. ListChangedEventArgs.NewIndex contains the index of the item that was changed. 
					Trace("Handling {0} event from {1}; Row #{2} was changed (not re-positioning)", e.ListChangedType.ToString(), sender.ToString(), e.NewIndex, trcOption.trcApplication);
					break;
				case ListChangedType.ItemDeleted:
					//An item deleted from the list. ListChangedEventArgs.NewIndex contains the index of the item that was deleted. 
					Trace("Handling {0} event from {1}; Row #{2} was deleted (not re-positioning)", e.ListChangedType.ToString(), sender.ToString(), e.NewIndex, trcOption.trcApplication);
					break;
				case ListChangedType.ItemMoved:
					//An item moved within the list. ListChangedEventArgs.OldIndex contains the previous index for the item, whereas ListChangedEventArgs.NewIndex contains the new index for the item. 
					Trace("Handling " + e.ListChangedType.ToString() + " event from {0}; Positioning from Row #{1} to Row #{2}", sender.ToString(), e.OldIndex, e.NewIndex, trcOption.trcApplication);
					if (cm.Position == e.NewIndex) {
						Trace("...but Current position is already set to NewIndex ({0});", e.NewIndex, trcOption.trcApplication);
					} else {
						if (e.NewIndex < 0) {
							Trace("...but NewIndex ({0}) is less than zero; (OldIndex:={1}); - I don't understand what's going on here...!", e.NewIndex, e.OldIndex, trcOption.trcApplication);
							return;
						} else if (e.NewIndex >= cm.List.Count) {
							Trace("...but NewIndex ({0}) is beyond our List.Count; (OldIndex:={1}); - I don't understand what's going on here...!", e.NewIndex, e.OldIndex, trcOption.trcApplication);
							return;
						}
						mTCBase.Move(e.NewIndex, e.ListChangedType);
					}
					break;
				case ListChangedType.PropertyDescriptorAdded:
					//A PropertyDescriptor was added, which changed the schema. 
					Trace("Handling {0} event from {1};", e.ListChangedType.ToString(), sender.ToString(), trcOption.trcApplication);
					break;
				case ListChangedType.PropertyDescriptorChanged:
					//A PropertyDescriptor was changed, which changed the schema. 
					Trace("Handling {0} event from {1};", e.ListChangedType.ToString(), sender.ToString(), trcOption.trcApplication);
					break;
				case ListChangedType.PropertyDescriptorDeleted:
					//A PropertyDescriptor was deleted, which changed the schema. 
					Trace("Handling {0} event from {1};", e.ListChangedType.ToString(), sender.ToString(), trcOption.trcApplication);
					break;
				case ListChangedType.Reset:
					//Much of the list has changed. Any listening controls should refresh all their data from the list. 
					Trace("Handling {0} event from {1}; Refreshing CurrencyManager...", e.ListChangedType.ToString(), sender.ToString(), trcOption.trcApplication);
					if ((cm != null))
						try {
							cm.Refresh();
						} catch (Exception ex) {
						}
					break;
			}
		}
		#endregion
	}
}
