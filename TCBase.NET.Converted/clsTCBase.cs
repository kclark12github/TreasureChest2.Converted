//clsTCBase.vb
//   TreasureChest2 Base Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   08/05/18    Updated GetDataRowValue to return "Null" if the DataRow value is DBNull;
//   06/07/18    Introduced BeforeDelete Event;
//   04/21/18    Added framework support for additional custom component data modifications after the main data operations have
//               been performed;
//   04/05/18    Introduced TrimTabs to ComboBox validations;
//   12/21/17    Addressed CurrencyManager synchronization issue introduced by image processing (RemoveImage) that had caused 
//               frmList.tcDataView_ListChanged to fire after moving to the new row while cleaning up image memory used by the old 
//               row;
//               Changed History processing such that Add/Copy and Delete operations will add an entry for each field;
//   12/19/17    Reworked DataRowView property as mDRV rather than the more confusing integer-looking iRow - leaving iRow to 
//               represent a RowIndex rather than a DataRowView;
//               Improved Tracing;
//   12/02/17    Changed Move-related Trace calls to use trcAll in an effort to see why my Dates are always getting screwed-up;
//   11/19/17    Hide active form in ListCommand to avoid display of records triggered by navigation;
//               Introduced logic to avoid hangs and ArgumentExceptions within CurrencyManager if filtered data is
//               modified to no longer qualify the filter;
//               Replaced use of DataAdapter in ModifyData in-lieu of a simple SqlCommand as the built-in UpdateCommand seemed
//               ineffective when updating the last record in a filtered DataView (confirmed through SQL Profiler);
//   06/04/17    Introduced LogSQL to record actual data being inserted or modified;
//   10/28/16    Modified Initialize() method to deal with Unit Testing;
//   10/21/16    Used Collection.Contains method to avoid ArguementExceptions;
//               Introduced GetDeferredImages;
//   10/06/16    Attempting to track down issues with empty ComboBox Text on data-entry;
//   09/21/16    Corrected SQLParseColumn to handle column references enclosed in brackets;
//   09/18/16    Reworked architecture to eliminate references (i.e. controls, bindings, CurrencyManager) here and moved them into
//               the appropriate Form base class in an effort to release memory otherwise held forever;
//   12/18/14    Added F6 Support to set DateTimePicker .Value properties to 7/31/1963 which is mapped to DBNull;
//   10/19/14    Upgraded to VS2013;
//   10/15/11    Added TagContains function;
//   12/01/10    Added code in EnableControl to set a ComboBox's SelectionLength property to zero if it doesn't have focus to 
//               counteract a known bug in the native Windows ComboBox control (apparently when such a bound ComboBox with a 
//               DropDown style is resized, it's painted with its Text highlighted);
//               Google: "ComboBox text remains selected"
//   07/27/10    Added ExportRegistrySettings;
//   07/26/10    Reorganized registry settings;
//   07/25/10    Reduced maximum History.OriginalValue/Value from 4000 to 3950 (maxHistoryValue) to make room for History.Who;
//               Reset mDatabase, mUserID and mPassword from ConnectionString when it is reset;
//   07/24/10    Enhanced ParseConnectionString to handle additional ConnectionString elements;
//   10/28/09    Rewritten in VB.NET;
//   10/09/04    Redesigned error reporting;
//   07/31/03    Added SIASCurrency Support;
//   02/04/03    Added .UserID and .Password properties;
//   10/19/02    Added ShowCommand() and UnloadCommand() to restore and save the last record displayed on entry and exit;
//   10/08/02    Added DataSourceSQL to BindField();
//   10/06/02    More TreasureChest refitting;
//   08/20/02    Started History;
//=================================================================================================================================
//Notes to Self:
//TODO: Detach DataView stuff from a CurrencyManager when a host screen is closed (causing unpredictable behavior including but not 
//      limited to Cancel operations...
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
//#define DepricatedMethods
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
using static TCBase.clsString;
using static TCBase.intTCComponent;
using Microsoft.VisualBasic.Devices;

namespace TCBase
{
	/// <summary>
	/// TreasureChest2 Application Base Class
	/// </summary>
	/// <remarks>Contains all the underlying support objects and forms to support each of the specific components managed by this application.</remarks>
	public abstract class clsTCBase : clsSupportBase, intTCComponent
	{
		public clsTCBase(clsSupport objSupport) : base(objSupport, "clsTCBase")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
		}
		public clsTCBase(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName)
		{
			mMyModuleName = ModuleName;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), ModuleName);
			//Debug.WriteLine("Allocating New clsTCBase for " & mMyTraceID)
			//This call is required by the Component Designer.
			InitializeComponent();
			Initialize(objSupport);
		}
		public clsTCBase(clsSupport objSupport, string ModuleName, string Text) : base(objSupport, ModuleName)
		{
			mMyModuleName = ModuleName;
			mText = Text;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), ModuleName);
			//Debug.WriteLine("Allocating New clsTCBase for " & mMyTraceID)
			//This call is required by the Component Designer.
			InitializeComponent();
			Initialize(objSupport);
		}

		#region " Component Designer generated code "
		public clsTCBase() : base()
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			mSupport = new clsSupport();
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
		}
		public clsTCBase(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		//Component overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		protected NotifyIcon niIcon;
		//Required by the Component Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Component Designer
		//It can be modified using the Component Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(clsTCBase));
			this.niIcon = new System.Windows.Forms.NotifyIcon(this.components);
			//
			//niIcon
			//
			this.niIcon.Icon = (System.Drawing.Icon)resources.GetObject("niIcon.Icon");

		}
		#endregion
		#region "Events"
		public event ActionModeChangeEventHandler ActionModeChange;
		public delegate void ActionModeChangeEventHandler(object sender, ActionModeChangeEventArgs e);
		public event AddCompleteEventHandler AddComplete;
		public delegate void AddCompleteEventHandler(object sender, DataEventArgs e);
		public event AddInProcessEventHandler AddInProcess;
		public delegate void AddInProcessEventHandler(object sender, DataEventArgs e);
		public event AfterMoveEventHandler AfterMove;
		public delegate void AfterMoveEventHandler(object sender, RowChangeEventArgs e);
		public event BeforeDeleteEventHandler BeforeDelete;
		public delegate void BeforeDeleteEventHandler(object sender, DataEventArgs e);
		public event BeforeMoveEventHandler BeforeMove;
		public delegate void BeforeMoveEventHandler(object sender, RowChangeEventArgs e);
		public event DeleteCompleteEventHandler DeleteComplete;
		public delegate void DeleteCompleteEventHandler(object sender, DataEventArgs e);
		public event DeleteInProcessEventHandler DeleteInProcess;
		public delegate void DeleteInProcessEventHandler(object sender, DataEventArgs e);
		public event FilterCanceledEventHandler FilterCanceled;
		public delegate void FilterCanceledEventHandler(object sender, FilterChangeEventArgs e);
		public event FilterChangeEventHandler FilterChange;
		public delegate void FilterChangeEventHandler(object sender, FilterChangeEventArgs e);
		public event ListChangedEventHandler ListChanged;
		public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);
        //Public Event RowChange(ByVal sender As Object, ByVal e As RowChangeEventArgs)
        public event SortCanceledEventHandler SortCanceled;
        public delegate void SortCanceledEventHandler(object sender, SortChangeEventArgs e);
        public event SortChangeEventHandler SortChange;
        public delegate void SortChangeEventHandler(object sender, SortChangeEventArgs e);
        public event SplashEventHandler Splash;
		public delegate void SplashEventHandler(object sender, SplashEventArgs e);
		public event UnbindControlsEventHandler UnbindControls;
		public delegate void UnbindControlsEventHandler(object sender, System.EventArgs e);
		public event UpdateCompleteEventHandler UpdateComplete;
		public delegate void UpdateCompleteEventHandler(object sender, DataEventArgs e);
		public event UpdateInProcessEventHandler UpdateInProcess;
		public delegate void UpdateInProcessEventHandler(object sender, DataEventArgs e);
		public virtual void OnActionModeChange(ActionModeEnum newMode)
		{
			myActionModeChange(newMode);
		}
		public virtual void OnAddComplete(DataRowView Row)
		{
			const string EntryName = "OnAddComplete";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (AddComplete != null) {
				AddComplete(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnAddInProcess(DataRowView Row)
		{
			const string EntryName = "OnAddInProcess";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (AddInProcess != null) {
				AddInProcess(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnAfterMove(int OldRowIndex, int NewRowIndex, DataRowView Row, int TotalRows)
		{
			const string EntryName = "OnAfterMove";
			string strArgs = string.Format("OldRowIndex:={0}, NewRowIndex:={1}, Row:={2}, TotalRows:={3}", new object[] {
				OldRowIndex,
				NewRowIndex,
				Row.ToString(),
				TotalRows
			});
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New RowChangeEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (NewRowIndex >= 0 && !Information.IsDBNull(tcDataView[NewRowIndex][mTableIDColumn])) {
				mRecordID = (int)tcDataView[NewRowIndex][mTableIDColumn];
				Trace("{0}: RecordID: {1}", EntryName, mRecordID, trcOption.trcDB | trcOption.trcApplication);
			}
			if (AfterMove != null) {
				AfterMove(this, new RowChangeEventArgs(OldRowIndex, NewRowIndex, Row, TotalRows));
			}
		}
		public virtual void OnBeforeDelete(DataRowView Row)
		{
			const string EntryName = "OnBeforeDelete";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (BeforeDelete != null) {
				BeforeDelete(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnBeforeMove(int OldRowIndex, int NewRowIndex, DataRowView Row, int TotalRows)
		{
			const string EntryName = "OnBeforeMove";
			string strArgs = string.Format("OldRowIndex:={0}, NewRowIndex:={1}, Row:={2}, TotalRows:={3}", new object[] {
				OldRowIndex,
				NewRowIndex,
				Row,
				TotalRows
			});
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New RowChangeEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (NewRowIndex >= 0 && NewRowIndex <= TotalRows - 1 && !Information.IsDBNull(tcDataView[NewRowIndex][mTableIDColumn])) {
				mRecordID = (int)tcDataView[NewRowIndex][mTableIDColumn];
				Trace("{0}: RecordID: {1}", EntryName, mRecordID, trcOption.trcDB | trcOption.trcApplication);
			}
			if (BeforeMove != null) {
				BeforeMove(this, new RowChangeEventArgs(OldRowIndex, NewRowIndex, Row, TotalRows));
			}
		}
		public virtual void OnDeleteComplete(DataRowView Row)
		{
			const string EntryName = "OnDeleteComplete";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (DeleteComplete != null) {
				DeleteComplete(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnDeleteInProcess(DataRowView Row)
		{
			const string EntryName = "OnDeleteInProcess";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (DeleteInProcess != null) {
				DeleteInProcess(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnFilterCanceled(bool ShowMessage = false)
		{
			const string EntryName = "OnFilterCanceled";
			string currentFilter = mSQLfilter;
			if (ShowMessage)
				MessageBox.Show("No qualifying filtered records remain; Filter will be cleared.", "Filter Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mSQLfilter = clsSupport.bpeNullString;
            SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings\\Select Settings", mRegistryKey, mActiveForm.Name), "SQLFilter", "");
			tcDataView.RowFilter = clsSupport.bpeNullString;
			if ((mCurrencyManager != null)) {
				Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
				mCurrencyManager.Refresh();
			}
			string[] strArgs = {
				"Nothing",
				string.Format("\"{0}\"", clsSupport.bpeNullString)
			};
			if ((currentFilter != null))
				strArgs[0] = "\"" + currentFilter + "\"";
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New FilterChangeEventArgs(" + string.Format("oldFilter:={1}, newFilter:={0}", strArgs) + "))", trcOption.trcApplication);
			if (FilterCanceled != null) {
				FilterCanceled(this, new FilterChangeEventArgs(currentFilter, clsSupport.bpeNullString));
			}
		}
		readonly Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag static_OnFilterChange_oldFilter_Init = new Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag();
		string static_OnFilterChange_oldFilter;
		public virtual void OnFilterChange(string newFilter)
		{
			const string EntryName = "OnFilterChange";
			lock (static_OnFilterChange_oldFilter_Init) {
				try {
					if (InitStaticVariableHelper(static_OnFilterChange_oldFilter_Init)) {
						static_OnFilterChange_oldFilter = null;
					}
				} finally {
					static_OnFilterChange_oldFilter_Init.State = 1;
				}
			}
			if (static_OnFilterChange_oldFilter != newFilter) {
				string[] strArgs = {
					"Nothing",
					"Nothing"
				};
				if ((newFilter != null))
					strArgs[0] = "\"" + newFilter + "\"";
				Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New FilterChangeEventArgs(" + string.Format("oldFilter:={1}, newFilter:={0}", strArgs) + "))", trcOption.trcApplication);
				if (FilterChange != null) {
					FilterChange(this, new FilterChangeEventArgs(static_OnFilterChange_oldFilter, newFilter));
				}

				static_OnFilterChange_oldFilter = newFilter;
				tcDataView.RowFilter = newFilter;
				if (tcDataView.Count == 0) {
					Trace(EntryName + ": Resulting filtered DataView contains zero records - clearing filter...", trcOption.trcApplication);
					newFilter = clsSupport.bpeNullString;
					static_OnFilterChange_oldFilter = clsSupport.bpeNullString;
					OnFilterCanceled();
					MoveFirst();
				} else {
					mSQLfilter = newFilter;
					int rowIndex = FindRowByID(mRecordID);
					if (rowIndex >= 0)
						Move(rowIndex);
					else
						MoveFirst();
				}
			}
		}
        //Public Overridable Sub OnRowChange(ByVal OldRowIndex As Integer, ByVal NewRowIndex As Integer, ByVal Row As DataRowView, ByVal TotalRows As Integer)
        //    Const EntryName As String = "OnRowChange"
        //    Dim strArgs As String = String.Format("OldRowIndex:={0}, NewRowIndex:={1}, Row:={2}, TotalRows:={3}", New Object() {OldRowIndex, NewRowIndex, Row.ToString, TotalRows})
        //    Trace(EntryName & ": RaiseEvent " & EntryName.Substring("On".Length) & "(Me, New RowChangeEventArgs(" & strArgs & "))", trcOption.trcApplication)
        //    If NewRowIndex >= 0 AndAlso Not IsDBNull(tcDataView.Item(NewRowIndex)(mTableIDColumn)) Then
        //        mRecordID = tcDataView.Item(NewRowIndex)(mTableIDColumn)
        //        Trace("{0}: RecordID: {1}", EntryName, mRecordID, trcOption.trcDB + trcOption.trcApplication)
        //    End If
        //    RaiseEvent RowChange(Me, New RowChangeEventArgs(OldRowIndex, NewRowIndex, Row, TotalRows))
        //End Sub
        public virtual void OnSortCanceled(bool ShowMessage = false)
        {
            const string EntryName = "OnSortCanceled";
            string currentSort = mSQLsort;
            if (ShowMessage)
                MessageBox.Show("No qualifying filtered records remain; Filter will be cleared.", "Filter Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings\\Sort Settings", mRegistryKey, mActiveForm.Name), "SQLSort", "");
            tcDataView.RowFilter = clsSupport.bpeNullString;
            if ((mCurrencyManager != null))
            {
                Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
                mCurrencyManager.Refresh();
            }
            string[] strArgs = {
                "Nothing",
                string.Format("\"{0}\"", clsSupport.bpeNullString)
            };
            if ((currentSort != null))
                strArgs[0] = "\"" + currentSort + "\"";
            Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New SortChangeEventArgs(" + string.Format("oldSort:={1}, newSort:={0}", strArgs) + "))", trcOption.trcApplication);
            if (FilterCanceled != null)
            {
                SortCanceled(this, new SortChangeEventArgs(currentSort, clsSupport.bpeNullString));
            }
        }
        readonly Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag static_OnSortChange_oldSort_Init = new Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag();
        string static_OnSortChange_oldSort;
        public virtual void OnSortChange(string newSort)
        {
            const string EntryName = "OnSortChange";
            lock (static_OnSortChange_oldSort_Init)
            {
                try
                {
                    if (InitStaticVariableHelper(static_OnSortChange_oldSort_Init))
                    {
                        static_OnSortChange_oldSort = null;
                    }
                }
                finally
                {
                    static_OnSortChange_oldSort_Init.State = 1;
                }
            }
            if (static_OnSortChange_oldSort != newSort)
            {
                string[] strArgs = {
                    "Nothing",
                    "Nothing"
                };
                if ((newSort != null))
                    strArgs[0] = "\"" + newSort + "\"";
                Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New SortChangeEventArgs(" + string.Format("oldSort:={1}, newSort:={0}", strArgs) + "))", trcOption.trcApplication);
                if (FilterChange != null)
                {
                    SortChange(this, new SortChangeEventArgs(static_OnSortChange_oldSort, newSort));
                }

                static_OnSortChange_oldSort = newSort;
                mSQLsort = newSort;
                if (tcDataView != null)
                {
                    tcDataView.Sort = newSort;
                    int rowIndex = FindRowByID(mRecordID);
                    if (rowIndex >= 0)
                        Move(rowIndex);
                    else
                        MoveFirst();
                }
            }
        }
        public virtual void OnSplash(string Message, Icon IconImage = null)
		{
			const string EntryName = "OnSplash";
			string[] strArgs = {
				"Nothing",
				"Nothing"
			};
			if ((Message != null))
				strArgs[0] = "\"" + Message + "\"";
			if ((IconImage != null))
				strArgs[1] = "IconImage";
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New SplashEventArgs(" + string.Format("Message:={0}, IconImage={1}", strArgs) + "))", trcOption.trcApplication);
			if (Splash != null) {
				Splash(this, new SplashEventArgs(Message, IconImage));
			}
		}
		public virtual void OnUnbindControls()
		{
			const string EntryName = "OnUnbindControls";
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New EventArgs)", trcOption.trcApplication);
			if (UnbindControls != null) {
				UnbindControls(this, new System.EventArgs());
			}
		}
		public virtual void OnUpdateComplete(DataRowView Row)
		{
			const string EntryName = "OnUpdateComplete";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (UpdateComplete != null) {
				UpdateComplete(this, new DataEventArgs(Row));
			}
		}
		public virtual void OnUpdateInProcess(DataRowView Row)
		{
			const string EntryName = "OnUpdateInProcess";
			string strArgs = string.Format("Row:={0}", new object[] { Row.ToString() });
			Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New DataEventArgs(" + strArgs + "))", trcOption.trcApplication);
			if (UpdateInProcess != null) {
				UpdateInProcess(this, new DataEventArgs(Row));
			}
		}
		#endregion
		#region "Properties"
		#region "Enumerations"
		private KeyEventArgs mKeyEventArgs;
		public enum ActionModeEnum
		{
			modeDisplay = 0,
			modeAdd = 1,
			modeCopy = 2,
			modeModify = 3,
			modeDelete = 4
		}
		public enum DBEConnectionStringEnum
		{
			dbecseApplication,
			dbecseConnectTimeout,
			dbecseDatabase,
			dbecseDriver,
			dbecsePacketSize,
			dbecsePassword,
			dbecsePooling,
			dbecseServer,
			dbecseUserID,
			dbecseWorkstationID
		}
		#endregion
		#region "Declarations"
			//was 4000
		public const int maxHistoryValue = 3950;
		protected internal SqlConnection tcConnection = null;
		protected internal SqlDataAdapter tcDataAdapter = null;
		protected internal SqlTransaction tcTransaction = null;
		private DataSet tcDataSet = null;
		private DataView withEventsField_tcDataView = null;
		private DataView tcDataView {
			get { return withEventsField_tcDataView; }
			set {
				if (withEventsField_tcDataView != null) {
					withEventsField_tcDataView.ListChanged -= tcDataView_ListChanged;
				}
				withEventsField_tcDataView = value;
				if (withEventsField_tcDataView != null) {
					withEventsField_tcDataView.ListChanged += tcDataView_ListChanged;
				}
			}
		}
		private DataRowView mDRV = null;
		private int iRow = -1;
		//Private Const SQL_CONNECTION_STRING As String = "Server=GZPR141;DataBase=TreasureChest;Integrated Security=SSPI;"
		private bool fAborting = false;
		private bool fSuppressDisplay = false;
		private bool fUseDefaultConnectionString = false;
		private Form mActiveForm = null;
		private int mActiveTXLevel = 0;
		private int mCommandTimeout = 300;
        private Computer mComputer = new Computer();
        private string mConnectionString = clsSupport.bpeNullString;
		private int mConnectionTimeout = 120;
		private CurrencyManager mCurrencyManager = null;
		private string mDatabaseName = clsSupport.bpeNullString;
		private string mDatabaseServer = clsSupport.bpeNullString;
		private string mDateFormat = clsSupport.bpeNullString;
		private clsDataBaseCollection mDBCollection = new clsDataBaseCollection();
		private string mDefaultConnectionString = clsSupport.bpeNullString;
		private string mDefaultImageFileName = clsSupport.bpeNullString;
		private string mDefaultImagePath = clsSupport.bpeNullString;
        private bool mDistinct = false;
		private int mDynamicMenuHeight = 0;
		private string mFieldList = clsSupport.bpeNullString;
		private string mFileDSN = clsSupport.bpeNullString;
		private string mFullDateTimeFormat = clsSupport.bpeNullString;
		private string mImagePath = clsSupport.bpeNullString;
		private bool mIntegratedSecurity = false;
		private string mLongDateFormat = clsSupport.bpeNullString;
		private float mMainHeight;
		private float mMainWidth;
		private float mMinHeight;
		private float mMinWidth;
		private ActionModeEnum mMode = ActionModeEnum.modeDisplay;
		private string mODBCFileDSNDir = clsSupport.bpeNullString;
		private string mOrderByClause = clsSupport.bpeNullString;
		protected Form mParent = null;
		private string mPassword = clsSupport.bpeNullString;
		private System.Resources.ResourceManager mProjectResources;
		private int mRecordID;
		private string mRegistryKey = clsSupport.bpeNullString;
		private string mReportsDirectory = clsSupport.bpeNullString;
		protected string mReportFileName = clsSupport.bpeNullString;
		private string mSaveCaption = clsSupport.bpeNullString;
		private string mSaveFilter = clsSupport.bpeNullString;
		private System.Drawing.Icon mSaveIcon;
		private int mSaveLeft;
		private int mSaveRecordID;
		private int mSaveTop;
		private string mServerName = clsSupport.bpeNullString;
		private string mShortDateFormat = clsSupport.bpeNullString;
		private string mSQLfilter = clsSupport.bpeNullString;
        private string mSQLMain = clsSupport.bpeNullString;
        private string mSQLsort = clsSupport.bpeNullString;
        private string mDefaultSort = clsSupport.bpeNullString;
        protected string mTableIDColumn = clsSupport.bpeNullString;
		protected string mTableKeyColumn = clsSupport.bpeNullString;
		private string mTableList = clsSupport.bpeNullString;
		protected string mTableName = clsSupport.bpeNullString;
		protected string mText = clsSupport.bpeNullString;
		private string mTransactionName = clsSupport.bpeNullString;
		private string mUserID = clsSupport.bpeNullString;
			#endregion
		private string mWhereClause = clsSupport.bpeNullString;
		public System.Drawing.Icon Icon {
			get { return niIcon.Icon; }
		}
		public string ReportPath {
			get { return string.Format("{0}\\{1}", mReportsDirectory, mReportFileName); }
		}
		public string Text {
			get { return mText; }
			set { mText = value; }
		}
		public Form ActiveForm {
			get { return mActiveForm; }
			set {
				mActiveForm = value;
				if ((tcDataView == null)){mCurrencyManager = null;return;
}

				CurrencyManager newManager = (CurrencyManager)mActiveForm.BindingContext[tcDataView];
				if ((mCurrencyManager == null)){mCurrencyManager = newManager;return;
}

				//Now check to see if our mCurrencyManager is stale (i.e. left over from another screen)...
				if (((DataView)mCurrencyManager.List).Table.TableName != tcDataView.Table.TableName) {
					mCurrencyManager = null;
					mCurrencyManager = (CurrencyManager)mActiveForm.BindingContext[tcDataView];
				}
			}
		}
		protected internal int ActiveTXLevel {
			get { return mActiveTXLevel; }
		}
		public int CommandTimeout {
			get { return mCommandTimeout; }
			set { mCommandTimeout = value; }
		}
        public Computer MyComputer {
            get{ return mComputer; }
        }
		public bool Connected {
			get {
				bool functionReturnValue = false;
				try {
					functionReturnValue = false;
					if ((tcConnection == null))
						return functionReturnValue;
					return (tcConnection.State & ConnectionState.Open) == ConnectionState.Open;
				} catch {
				}
				return functionReturnValue;
			}
		}
		public SqlConnection Connection {
			get { return tcConnection; }
			set { tcConnection = value; }
		}
		public string ConnectionString {
			get { return mConnectionString; }
			set {
				//If mFileDSN <> bpeNullString And Value <> bpeNullString Then Throw New Exception(".FileDSN already defined.")
				if (mConnectionString != value && tcConnection != null)
					CloseConnection();
				mConnectionString = value;
				if (mConnectionString != clsSupport.bpeNullString) {
					mUserID = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseUserID);
					mPassword = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecsePassword);
					mServerName = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseServer);
					mDatabaseName = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseDatabase);
				}
			}
		}
		public string DateFormat {
			get { return mDateFormat; }
		}
		public int RecordID {
			get { return mRecordID; }
		}
		internal CurrencyManager CurrencyManager {
			get { return mCurrencyManager; }
		}
		public DataRowView CurrentRow {
			get { return mDRV; }
		}
		public string DatabaseName {
			get { return mDatabaseName; }
		}
		public string ServerName {
			get { return mServerName; }
		}
		public object DBCollection {
			get { return mDBCollection; }
		}
		public int DynamicMenuHeight {
			get { return mDynamicMenuHeight; }
			set { mDynamicMenuHeight = value; }
		}
		public string FileDSN {
			get { return mFileDSN; }
			set { mFileDSN = value; }
		}
		public string DefaultImagePath {
			get { return mDefaultImagePath; }
			set { mDefaultImagePath = value; }
		}
		public string FieldList {
			get { return mFieldList; }
		}
		public string FullDateTimeFormat {
			get { return mFullDateTimeFormat; }
		}
		public string ImagePath {
			get { return mImagePath; }
			set { mImagePath = value; }
		}
		public bool IntegratedSecurity {
			get { return mIntegratedSecurity; }
		}
		public string LongDateFormat {
			get { return mLongDateFormat; }
		}
		public DataSet MainDataSet {
			get { return tcDataSet; }
			set { tcDataSet = value; }
		}
		public DataView MainDataView {
			get { return tcDataView; }
			set { tcDataView = value; }
		}
		public float MainHeight {
			get { return mMainHeight; }
			set { mMainHeight = value; }
		}
		public float MainWidth {
			get { return mMainWidth; }
			set { mMainWidth = value; }
		}
		public float MinHeight {
			get { return mMinHeight; }
			set { mMinHeight = value; }
		}
		public float MinHeightConst {
			get { return 1440; }
		}
		public float MinWidth {
			get { return mMinWidth; }
			set { mMinWidth = value; }
		}
		public float MinWidthConst {
			get { return 2184; }
		}
		public ActionModeEnum Mode {
			get { return mMode; }
		}
		public string ODBCFileDSNDir {
			get { return mODBCFileDSNDir; }
		}
		public string OrderByClause {
			get { return mOrderByClause; }
		}
		public string Password {
			get { return mPassword; }
			set { mPassword = value; }
		}
		public System.Resources.ResourceManager ProjectResources {
			get { return mProjectResources; }
		}
		public string RegistryKey {
			get { return mRegistryKey; }
		}
		public string ReportsDirectory {
			get { return mReportsDirectory; }
			set { mReportsDirectory = value; }
		}
		public float ResizeWindowConst {
			get { return 36; }
		}
		public string SaveCaption {
			get { return mSaveCaption; }
			set { mSaveCaption = value; }
		}
		public System.Drawing.Icon SaveIcon {
			get { return mSaveIcon; }
			set { mSaveIcon = value; }
		}
		public int SaveLeft {
			get { return mSaveLeft; }
			set { mSaveLeft = value; }
		}
		public int SaveTop {
			get { return mSaveTop; }
			set { mSaveTop = value; }
		}
		public string ShortDateFormat {
			get { return mShortDateFormat; }
		}
		public string SQLFilter {
			get { return mSQLfilter; }
			set {
				if (value != clsSupport.bpeNullString & value != "0") {
					OnFilterChange(value);
				} else {
					OnFilterChange(null);
				}
			}
		}
		public string SQLMain {
			get { return mSQLMain; }
			set {
				ParseSQLSelect(value, ref mFieldList, ref mTableList, ref mWhereClause, ref mOrderByClause, ref mDistinct);
				if (mSupport.Strings.TokenCount(mTableList, ",") > 1) {
					mSQLMain = clsSupport.bpeNullString;
					mFieldList = clsSupport.bpeNullString;
					mTableList = clsSupport.bpeNullString;
					mWhereClause = clsSupport.bpeNullString;
					mOrderByClause = clsSupport.bpeNullString;
					throw new Exception("Multiple tables not supported in main SQL statement.");
				}
				mSQLMain = value;
			}
		}
        public string SQLSort
        {
            get { return mSQLsort; }
            set
            {
                if (value != clsSupport.bpeNullString)
                {
                    OnSortChange(value);
                }
                else
                {
                    OnSortChange(null);
                }
            }
        }
        public clsSupport Support {
			get { return mSupport; }
		}
		public string TableList {
			get { return mTableList; }
		}
		public string TableIDColumn {
			get { return mTableIDColumn; }
			set { mTableIDColumn = value; }
		}
		public string TableKeyColumn {
			get { return mTableKeyColumn; }
			set { mTableKeyColumn = value; }
		}
		public string TableName {
			get { return mTableName; }
			set { mTableName = value; }
		}
		public string UserID {
			get { return mUserID; }
			set { mUserID = value; }
		}
		public string WhereClause {
			get { return mWhereClause; }
		}
		#endregion
		#region "Methods"
		protected virtual void Initialize(clsSupport objSupport = null)
		{
			if (objSupport != null && !object.ReferenceEquals(objSupport, mSupport))
				mSupport = objSupport;
			mProjectResources = new System.Resources.ResourceManager("TCBase.ProjectResources", System.Reflection.Assembly.GetExecutingAssembly());

			myActionModeChange(ActionModeEnum.modeDisplay);
			fSuppressDisplay = false;

			//Initialize Date Formats...
			mShortDateFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			mLongDateFormat = mShortDateFormat;
			if (Strings.InStr(Strings.LCase(mLongDateFormat), "yyyy") == 0) {
				int iPos = Strings.InStr(Strings.LCase(mLongDateFormat), "yy");
				mLongDateFormat = Strings.Mid(mLongDateFormat, 1, iPos - 1) + "yyyy";
			}
			mFullDateTimeFormat = mLongDateFormat + " hh:nn:ss AMPM";

			//Registry Stuff...
			mRegistryKey = string.Format("Software\\KClark Software\\{0}", mSupport.ApplicationName);
			mODBCFileDSNDir = (string)GetRegistrySetting(RootKeyConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\\ODBC\\ODBC.INI\\ODBC File DSN", "DefaultDSNDir", clsSupport.bpeNullString);
			mReportsDirectory = (string)GetRegistrySetting(RegistryKey, "ReportsDirectory", clsSupport.bpeNullString);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "DefaultConnectionString", mDefaultConnectionString);
			mFileDSN = (string)GetRegistrySetting(RegistryKey, "FileDSN", clsSupport.bpeNullString);
			ConnectionString = (string)GetRegistrySetting(RegistryKey, "ConnectionString", clsSupport.bpeNullString);

			fUseDefaultConnectionString = Convert.ToBoolean(GetRegistrySetting(RegistryKey, "UseDefaultConnectionString", Convert.ToBoolean(mFileDSN == clsSupport.bpeNullString && ConnectionString == clsSupport.bpeNullString)));
			if (fUseDefaultConnectionString)
				ConnectionString = mDefaultConnectionString;

			DefaultImagePath = FileSystem.CurDir() + "\\Images";
			mImagePath = (string)GetRegistrySetting(RegistryKey, "ImagePath", DefaultImagePath + "\\" + mDefaultImageFileName);

			if (objSupport == null) {
				TraceMode = Convert.ToBoolean(GetRegistrySetting(RegistryKey, "TraceMode", false));
				TraceFile = (string)GetRegistrySetting(RegistryKey, "TraceFile", string.Format("{0}\\{1}.trace", mSupport.ApplicationPath, mSupport.ApplicationName));
				TraceOptions = (trcOption)GetRegistrySetting(RegistryKey, "TraceOptions", trcOption.trcApplication);
				if (TraceMode) {
					Trace(new string('=', 132), trcOption.trcAll);
					Trace(string.Format("{0} Start - {1}", mSupport.ApplicationName, TraceFile), trcOption.trcAll);
				}
			}

			//Form Stuff...
			MinWidth = MinWidthConst;
			MinHeight = MinHeightConst;

			//SQL Stuff...
			if (ConnectionString != clsSupport.bpeNullString) {
				mUserID = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseUserID);
				mPassword = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecsePassword);
				mServerName = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseServer);
				mDatabaseName = ParseConnectionString(ConnectionString, DBEConnectionStringEnum.dbecseDatabase);
			}
			if (mUserID == clsSupport.bpeNullString)
				mUserID = (string)GetRegistrySetting(RegistryKey, "UserID", clsSupport.bpeNullString);
			if (mPassword == clsSupport.bpeNullString)
				mPassword = (string)GetRegistrySetting(RegistryKey, "Password", clsSupport.bpeNullString);

			//ClearLastError()
		}
		public void LogSQL(SqlCommand Command)
		{
			var _with1 = Command;
			Trace(Command.CommandText, trcOption.trcApplication | trcOption.trcDB);
			//MyBase.LogMessage(Command.CommandText, 0)
			int iMaxLength = 0;
			foreach (SqlParameter iParam in _with1.Parameters) {
				if (iParam.ParameterName.Length > iMaxLength)
					iMaxLength = iParam.ParameterName.Length;
			}
			foreach (SqlParameter iParam in _with1.Parameters) {
				string Message = string.Format("{0}{1}{2}{3}{4}", new object[] {
					Constants.vbTab,
					iParam.ParameterName,
					new string(' ', iMaxLength - iParam.ParameterName.Length),
					Constants.vbTab,
					FormatData(iParam.SqlDbType, iParam.Value, true)
				});
				Trace(Message, trcOption.trcApplication | trcOption.trcDB);
				//MyBase.LogMessage(Message, 1)
			}
		}
		public abstract void Load(Form objParent, string Caption);
		public void Show()
		{
			try {
				ShowCommand((frmTCStandard)mActiveForm, true);
			} finally {
				mActiveForm.Dispose();
				mActiveForm = null;
				mDRV = null;
				tcDataView = null;
				CloseDataSet(ref tcDataSet);
				CloseConnection();
				GC.Collect();
			}
		}
		readonly Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag static_myActionModeChange_oldMode_Init = new Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag();
		ActionModeEnum static_myActionModeChange_oldMode;
		private void myActionModeChange(ActionModeEnum newMode)
		{
			const string EntryName = "myActionModeChange";
			lock (static_myActionModeChange_oldMode_Init) {
				try {
					if (InitStaticVariableHelper(static_myActionModeChange_oldMode_Init)) {
						static_myActionModeChange_oldMode = ActionModeEnum.modeDisplay;
					}
				} finally {
					static_myActionModeChange_oldMode_Init.State = 1;
				}
			}
			//Debug.WriteLine(String.Format("OnActionModeChange: oldMode:={0}; newMode:={1}", oldMode.ToString, newMode.ToString))
			if (static_myActionModeChange_oldMode != newMode) {
				string strArgs = string.Format("oldMode:={0}, newMode:={1}", static_myActionModeChange_oldMode.ToString(), newMode.ToString());
				Trace(EntryName + ": RaiseEvent " + EntryName.Substring("On".Length) + "(Me, New ActionModeChangeEventArgs(" + strArgs + "))", trcOption.trcApplication);
				if (ActionModeChange != null) {
					ActionModeChange(this, new ActionModeChangeEventArgs(static_myActionModeChange_oldMode, newMode));
				}

				static_myActionModeChange_oldMode = newMode;
				mMode = newMode;
			}
		}
		#region "Deprecated Methods"
		#if DeprecatedMethods
		<Obsolete("RecordEntry is deprecated.")>
		Public Sub RecordEntry(ByVal EntryName As String, ByVal Arguments As String, ByVal TraceOption As trcOption, Optional ByVal TrackElapsedTime As Boolean = True)
		End Sub
		<Obsolete("RecordExit is deprecated.")>
		Public Sub RecordExit(ByVal EntryName As String, Optional ByVal ErrorHandler As Boolean = False, Optional ByVal ReturnValue As String = bpeNullString)
		End Sub
		<Obsolete("RaiseError is deprecated.")>
		Public Sub RaiseError(ByVal EntryName As String, ByVal ex As System.Exception, Optional ByVal RaiseErrorSub As Object = Nothing)
		End Sub
		<Obsolete("RaiseError is deprecated.")>
		Public Sub RaiseError()
		End Sub
		<Obsolete("RaiseError is deprecated.")>
		Public Sub RaiseError(ByVal Number As Integer, ByVal Description As String)
		End Sub
		<Obsolete("RaiseError is deprecated.")>
		Public Sub RaiseError(ByVal Number As Integer, ByVal Source As String, ByVal Description As String)
		End Sub
		//#Region "Controls & Binding"
		//#Region "Converters"
		//    Private Function GetBindingInfo(ByVal sender As Binding) As String
		//        'Don't trace this guy on purpose... He's providing trace information to his callers...
		//        GetBindingInfo = bpeNullString
		//        If TraceMode Then
		//            Dim cm As CurrencyManager = CType(sender.BindingManagerBase, CurrencyManager)
		//            GetBindingInfo = String.Format("{0}.{1} bound to ", sender.Control.Name, sender.PropertyName)
		//            GetBindingInfo &= String.Format("{0}.{1} @ Row #{2} ", Me.TableName, sender.BindingMemberInfo.BindingMember, cm.Position)
		//            GetBindingInfo &= String.Format("{0}.{1}:={2}", Me.TableName, mTableIDColumn, IIf(IsDBNull(cm.Current(mTableIDColumn)), "DBNull", cm.Current(mTableIDColumn)))
		//        End If
		//    End Function
		//    'Protected Friend Sub BinaryToImage(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//    '    Dim arrPicture() As Byte
		//    '    Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//    '    If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//    '        Trace("e.Value is Null; Defaulting to TestPattern Image", trcOption.trcControls Or trcOption.trcDB)
		//    '        'Use the default image...
		//    '        arrPicture = defaultImage
		//    '    Else
		//    '        'The SQL Server Image datatype is a binary datatype. Therefore, to generate an image from it you must first create a stream object 
		//    '        'containing the binary data. Then you can generate the image by calling Image.FromStream().
		//    '        arrPicture = e.Value
		//    '    End If
		//    '    Trace("Streaming Image...", trcOption.trcControls Or trcOption.trcDB)
		//    '    Dim ms As New System.IO.MemoryStream(arrPicture)
		//    '    e.Value = System.Drawing.Image.FromStream(ms)
		//    '    'Close the stream object to release the resource.
		//    '    ms.Close()
		//    'End Sub
		//    Protected Sub BooleanToSmallInt(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)

		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            e.Value = False
		//            Trace("e.Value is Null; Converting to " & e.Value, trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//        If e.Value.GetType Is GetType(Integer) Then
		//            Trace("{0}.Value is already {1} (and isn't DBNull) - bugging out...", sender.Control.Name, e.Value.ToString, trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Select Case e.Value
		//                Case True : e.Value = 1
		//                Case Else : e.Value = 0
		//            End Select
		//            Trace("e.Value is " & e.Value, trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub DateToNull(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcApplication)
		//        Select Case Me.TableName
		//            Case "Class", "Classification", "Ships"
		//                If Date.Compare(e.Value, New Date(1775, 10, 13)) = 0 Then e.Value = DBNull.Value
		//            Case Else
		//                If Date.Compare(e.Value, New Date(1963, 7, 31)) = 0 Then e.Value = DBNull.Value
		//        End Select
		//    End Sub
		//    Protected Friend Sub NullToID(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//        'Trace(New StackTrace(True).ToString, trcOption.trcControls or trcOption.trcDB)
		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            e.Value = bpeNullString
		//            Trace("e.Value is Null; Converting to """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Trace("e.Value is """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub NullToString(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            e.Value = bpeNullString
		//            Trace("e.Value is Null; Converting to """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Trace("e.Value is """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub NullToChecked(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)

		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            e.Value = False
		//            Trace("e.Value is Null; Converting to """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Trace("e.Value is """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub NullToDate(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            Select Case Me.TableName
		//                Case "Class", "Classification", "Ships"
		//                    e.Value = New Date(1775, 10, 13)
		//                Case Else
		//                    e.Value = New Date(1963, 7, 31)
		//            End Select
		//            Trace("e.Value is Null; Converting to """ & e.Value.ToString & """", trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Trace("e.Value is """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub NullToMoney(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            Dim zero As Integer = 0
		//            e.Value = zero.ToString("c")
		//            Trace("e.Value is Null; Converting to """ & e.Value.ToString & """", trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Trace("e.Value is """ & e.Value & """", trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub MoneyToString(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)
		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            Dim zero As Integer = 0
		//            Trace("e.Value is Null; Converting to " & e.Value.ToString, trcOption.trcControls Or trcOption.trcDB)
		//            e.Value = zero.ToString("c")
		//        Else
		//            Try
		//                Trace("Converting e.Value from {0} to {1}", e.Value.ToString, CType(e.Value, Decimal).ToString("c"), trcOption.trcControls Or trcOption.trcDB)
		//                e.Value = CType(e.Value, Decimal).ToString("c")
		//            Catch ex As InvalidCastException
		//                If e.DesiredType Is e.Value.GetType Then
		//                    Trace("Caught {0}; Since e.DesiredType is already {1}, we're leaving the value alone.", ex.GetType.Name, e.DesiredType.FullName, trcOption.trcControls Or trcOption.trcDB)
		//                Else
		//                    Dim zero As Integer = 0
		//                    Trace("Caught {0}; Converting to ""{1}""", ex.GetType.Name, e.Value.ToString, trcOption.trcControls Or trcOption.trcDB)
		//                    e.Value = zero.ToString("c")
		//                End If
		//            Catch ex As Exception
		//                Throw New Exception(String.Format("Caught {0}; Attempting to convert {1} to {2}", ex.GetType.Name, e.Value, e.DesiredType.ToString), ex)
		//            End Try
		//        End If
		//    End Sub
		//    Protected Friend Sub SmallIntToBoolean(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcControls Or trcOption.trcDB)

		//        If IsDBNull(e.Value) Or e.Value.ToString.Trim.Length = 0 Then
		//            e.Value = 0
		//            Trace("e.Value is Null; Converting to " & e.Value, trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//        If e.Value.GetType Is GetType(Boolean) Then
		//            Trace("{0}.Value is already {1} (and isn't DBNull) - bugging out...", sender.Control.Name, e.Value.ToString, trcOption.trcControls Or trcOption.trcDB)
		//        Else
		//            Select Case e.Value
		//                Case 1
		//                    e.Value = True
		//                Case Else
		//                    e.Value = False
		//            End Select
		//            Trace("e.Value is " & e.Value, trcOption.trcControls Or trcOption.trcDB)
		//        End If
		//    End Sub
		//    Protected Friend Sub StringToMoney(ByVal sender As Object, ByVal e As ConvertEventArgs)
		//        Trace(GetBindingInfo(sender), trcOption.trcApplication)
		//        Try
		//            Trace("Converting e.Value from {0} to {1} ({2})", e.Value, CType(e.Value, Double).ToString, e.DesiredType.ToString, trcOption.trcControls Or trcOption.trcDB)
		//            e.Value = CType(e.Value, Double) 'Double is equivalent to a Money SQL data type.
		//        Catch ex As InvalidCastException
		//            Dim zero As Integer = 0
		//            Trace("Caught {0}; Converting to {1}", ex.GetType.Name, e.Value.ToString, trcOption.trcControls Or trcOption.trcDB)
		//            e.Value = zero.ToString("c")
		//        Catch ex As Exception
		//            Throw New Exception(String.Format("Caught {0}; Attempting to convert {1} to {2}", ex.GetType.Name, e.Value, e.DesiredType.ToString), ex)
		//        End Try
		//    End Sub
		//#End Region

		//    'TODO: Comment all this out...
		//    'Public Sub AutoComplete_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
		//    '    'No tracing on purpose (performance)...
		//    '    Dim cb As Windows.Forms.ComboBox = CType(sender, Windows.Forms.ComboBox)
		//    '    mKeyEventArgs = e
		//    '    Select Case e.KeyCode
		//    '        Case Keys.End, Keys.Home, Keys.Up, Keys.Down, Keys.PageUp, Keys.PageDown
		//    '            e.Handled = False : Exit Sub
		//    '        Case Keys.Left
		//    '            If cb.SelectionStart > 0 Then
		//    '                cb.SelectionStart -= 1
		//    '                e.Handled = True
		//    '            Else
		//    '                e.Handled = False
		//    '            End If
		//    '            Exit Sub
		//    '        Case Keys.Right
		//    '            If cb.SelectionStart < cb.SelectionLength Then
		//    '                cb.SelectionStart += 1
		//    '                e.Handled = True
		//    '            Else
		//    '                e.Handled = False
		//    '            End If
		//    '            Exit Sub
		//    '        Case Else
		//    '            If e.Control Then e.Handled = False : Exit Sub
		//    '    End Select
		//    'End Sub
		//    'Public Sub AutoComplete_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) 'Handles cbAlphaSort.KeyPress, cbLocation.KeyPress
		//    '    'No tracing on purpose (performance)...
		//    '    Dim cb As Windows.Forms.ComboBox = CType(sender, Windows.Forms.ComboBox)
		//    '    Dim searchText As String = bpeNullString
		//    '    Dim newChar As Short = 0

		//    '    'If we're a DropDownList, bail and let it do its own thing...
		//    '    If cb.DropDownStyle = ComboBoxStyle.DropDownList Then e.Handled = False : Exit Sub

		//    '    Select Case Asc(e.KeyChar)
		//    '        Case Keys.Escape
		//    '            cb.SelectedIndex = -1
		//    '            cb.Text = ""
		//    '            e.Handled = True
		//    '            Exit Sub
		//    '        Case Keys.Back
		//    '            If cb.SelectionStart <= 1 Then cb.Text = "" : Exit Sub
		//    '            searchText = cb.Text.Substring(0, IIf(cb.SelectionLength = 0, cb.Text.Length, cb.SelectionStart) - 1)
		//    '        Case Else
		//    '            If mKeyEventArgs.Control And mKeyEventArgs.Shift Then
		//    '                Select Case mKeyEventArgs.KeyCode
		//    '                    Case Keys.C : newChar = 169 '©
		//    '                    Case Keys.D : newChar = 176 '°
		//    '                    Case Keys.R : newChar = 174 '®
		//    '                    Case Keys.T : newChar = 153 '™
		//    '                    Case Else
		//    '                        If Char.IsControl(e.KeyChar) Then e.Handled = False : Exit Sub
		//    '                End Select
		//    '                If newChar <> 0 Then searchText = IIf(cb.SelectionLength = 0, cb.Text, cb.Text.Substring(0, cb.SelectionStart)) & Chr(newChar)
		//    '            Else
		//    '                searchText = IIf(cb.SelectionLength = 0, cb.Text, cb.Text.Substring(0, cb.SelectionStart)) & Char.ToUpper(e.KeyChar)
		//    '            End If
		//    '    End Select

		//    '    Dim MatchIndex As Integer = cb.FindString(searchText)
		//    '    Debug.WriteLine(String.Format("cb.FindString(""{0}"") returned: {1}", searchText, MatchIndex))
		//    '    If MatchIndex <> -1 Then
		//    '        cb.SelectedText = ""
		//    '        cb.SelectedIndex = MatchIndex
		//    '        cb.SelectionStart = searchText.Length
		//    '        cb.SelectionLength = cb.Text.Length
		//    '        e.Handled = True
		//    '    ElseIf mKeyEventArgs.Control And mKeyEventArgs.Shift And newChar <> 0 Then
		//    '        If cb.SelectionLength > 0 Then
		//    '            cb.SelectedText = Chr(newChar)
		//    '        Else
		//    '            Dim ss As Integer = cb.SelectionStart
		//    '            cb.Text = cb.Text.Substring(0, cb.SelectionStart) & Chr(newChar) & cb.Text.Substring(cb.SelectionStart)
		//    '            cb.SelectionStart = ss + 1
		//    '        End If
		//    '        e.Handled = True
		//    '    ElseIf Not Char.IsLetter(e.KeyChar) Then
		//    '        'TODO: AutoComplete: Fix Force Upper-Case logic...
		//    '        e.Handled = False
		//    '        'ElseIf Not IsNothing(cb.Tag) AndAlso CType(cb.Tag, String).ToUpper.IndexOf("UPPER") >= 0 Then
		//    '        'Dim saveText As String = cb.Text
		//    '        'Dim selectionStart As Integer = cb.SelectionStart
		//    '        'Dim selectionLength As Integer = cb.SelectionLength
		//    '        'cb.SelectedIndex = -1
		//    '        'Mid(saveText, selectionStart, selectionLength) = Char.ToUpper(e.KeyChar)
		//    '        'cb.Text = saveText
		//    '        'cb.SelectionStart = selectionStart + 1
		//    '        'cb.SelectionLength = 0
		//    '        'e.Handled = True
		//    '    Else
		//    '        e.Handled = False
		//    '    End If
		//    'End Sub
		//    'Public Sub BindControl(ByVal ctl As Control, ByVal dataSource As DataView, ByVal dataMember As String, ByVal displaySource As DataView, ByVal displayMember As String, ByVal valueMember As String)
		//    '    Dim EntryName As String = "BindControl"
		//    '    Select Case TypeName(ctl)
		//    '        Case "ComboBox"
		//    '            With CType(ctl, Windows.Forms.ComboBox)
		//    '                'Example:
		//    '                'displaySource: dvClassifications
		//    '                'displayMember: Type
		//    '                'valueMember:   ID
		//    '                'dataSource:    dvClass
		//    '                'dataMember:    ClassificationID (i.e. the field in the dataSource we want bound to the valueMember of the displaySource)
		//    '                .DataSource = displaySource
		//    '                .DisplayMember = displayMember
		//    '                .ValueMember = valueMember   'This really does need to be "valueMember" from the displaySource...
		//    '                Dim maxLength As Integer = dataSource.Table.Columns(dataMember).MaxLength
		//    '                If maxLength > 0 Then .MaxLength = maxLength
		//    '                .DataBindings.Add("SelectedValue", dataSource, dataMember)
		//    '            End With
		//    '        Case Else
		//    '            Throw New Exception(String.Format("Unexpected control type ({0}) encountered in {1}(Control,DataView,String,String). Control: {2}", TypeName(ctl), EntryName, ctl.Name))
		//    '    End Select
		//    'End Sub
		//    'Public Sub BindControl(ByVal ctl As Control, ByVal dataSource As DataView, ByVal dataMember As String)
		//    '    Dim EntryName As String = "BindControl"
		//    '    Select Case TypeName(ctl)
		//    '        Case "ComboBox"
		//    '            Dim tmpBinding As New Binding("Text", dataSource, dataMember)
		//    '            AddHandler tmpBinding.Format, AddressOf NullToString
		//    '            With CType(ctl, Windows.Forms.ComboBox)
		//    '                Dim maxLength As Integer = dataSource.Table.Columns(dataMember).MaxLength
		//    '                If maxLength > 0 Then .MaxLength = maxLength
		//    '                .DataBindings.Add(tmpBinding)
		//    '            End With
		//    '        Case "CheckBox", "RadioButton"
		//    '            Dim tmpBinding As New Binding("Checked", dataSource, dataMember)
		//    '            AddHandler tmpBinding.Format, AddressOf SmallIntToBoolean
		//    '            AddHandler tmpBinding.Parse, AddressOf BooleanToSmallInt
		//    '            CType(ctl, CheckBox).DataBindings.Add(tmpBinding)
		//    '        Case "DateTimePicker"
		//    '            Dim tmpBinding As New Binding("Value", dataSource, dataMember)
		//    '            AddHandler tmpBinding.Format, AddressOf NullToDate
		//    '            AddHandler tmpBinding.Parse, AddressOf DateToNull
		//    '            CType(ctl, DateTimePicker).DataBindings.Add(tmpBinding)
		//    '        Case "Label"
		//    '            Dim tmpBinding As New Binding("Text", dataSource, dataMember)
		//    '            AddHandler tmpBinding.Format, AddressOf NullToString
		//    '            CType(ctl, Label).DataBindings.Add(tmpBinding)
		//    '        Case "PictureBox"
		//    '            Dim tmpBinding As New Binding("Image", dataSource, dataMember)
		//    '            'AddHandler tmpBinding.Format, AddressOf BinaryToImage
		//    '            CType(ctl, PictureBox).DataBindings.Add(tmpBinding)
		//    '        Case "RichTextBox"
		//    '            Dim tmpBinding As New Binding("Rtf", dataSource, dataMember)
		//    '            AddHandler tmpBinding.Format, AddressOf NullToString
		//    '            CType(ctl, RichTextBox).DataBindings.Add(tmpBinding)
		//    '        Case "TextBox"
		//    '            Dim tmpBinding As New Binding("Text", dataSource, dataMember)
		//    '            Select Case dataMember  'Distinguish only for debugging purposes...
		//    '                Case mTableIDColumn
		//    '                    AddHandler tmpBinding.Format, AddressOf NullToID
		//    '                Case Else
		//    '                    AddHandler tmpBinding.Format, AddressOf NullToString
		//    '            End Select
		//    '            Dim strTag As String = IIf(ctl.Tag Is Nothing, "", CType(ctl.Tag, String))
		//    '            Dim tagParams() As String = strTag.Split(",".ToCharArray)
		//    '            For i As Integer = 0 To tagParams.Length - 1
		//    '                Select Case tagParams(i).ToUpper
		//    '                    Case "MONEY"
		//    '                        AddHandler tmpBinding.Format, AddressOf MoneyToString
		//    '                        AddHandler tmpBinding.Parse, AddressOf StringToMoney
		//    '                    Case "NULLS"
		//    '                End Select
		//    '            Next i
		//    '            With CType(ctl, TextBox)
		//    '                Dim maxLength As Integer = dataSource.Table.Columns(dataMember).MaxLength
		//    '                If maxLength > 0 Then .MaxLength = maxLength
		//    '                .DataBindings.Add(tmpBinding)
		//    '            End With
		//    '        Case Else
		//    '            Throw New Exception(String.Format("Unexpected control type ({0}) encountered in {1}(Control,DataView,String). Control: {2}", TypeName(ctl), EntryName, ctl.Name))
		//    '    End Select
		//    'End Sub
		//    'Public Sub Control_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
		//    '    'No tracing on purpose (performance)...
		//    '    mKeyEventArgs = e
		//    'End Sub
		//    'Public Sub Control_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
		//    '    'No tracing on purpose (performance)...
		//    '    Try
		//    '        CType(mActiveForm, frmTCBase).epBase.SetError((Control)sender, bpeNullString)
		//    '        Select Case sender.GetType.Name
		//    '            Case "TextBox"
		//    '                Dim tb As TextBox = CType(sender, TextBox)
		//    '                Dim remainingText As String = tb.Text
		//    '                If tb.SelectionLength > 0 Then remainingText = tb.Text.Replace(tb.Text.Substring(tb.SelectionStart, tb.SelectionLength), bpeNullString)
		//    '                If IIf(tb.Tag Is Nothing, "", CType(tb.Tag, String)).ToUpper.IndexOf("MONEY") >= 0 Then
		//    '                    'Note: The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events.
		//    '                    If Not (mKeyEventArgs.Control And mKeyEventArgs.KeyCode = Keys.Z) Then  '(i.e. Not attempting Undo)
		//    '                        Select Case mKeyEventArgs.KeyCode
		//    '                            Case Keys.D0 To Keys.D9
		//    '                            Case Keys.NumPad0 To Keys.NumPad9
		//    '                            Case Keys.Home, Keys.End, Keys.Left, Keys.Right 'Directional and arrow keys
		//    '                            Case Keys.Back  'Backspace
		//    '                            Case Keys.Delete
		//    '                            Case Keys.Decimal, Keys.OemPeriod 'KeyPad-Decimal, Keyboard-Period (respectively)
		//    '                                If remainingText.IndexOf(".") >= 0 Then Throw New Exception("Invalid value entered.")
		//    '                            Case Keys.Oemcomma  'Keyboard-Comma
		//    '                                If remainingText.IndexOf(".") >= 0 Then Throw New Exception("Invalid value entered.")
		//    '                            Case Else
		//    '                                'Use the following initialization to trigger the conversion exception (if any)
		//    '                                Dim strPrice As String = CType(tb.Text, Decimal).ToString("c")
		//    '                        End Select
		//    '                    End If
		//    '                ElseIf IIf(tb.Tag Is Nothing, "", CType(tb.Tag, String)).ToUpper.IndexOf("INTEGER") >= 0 Then
		//    '                    'Note: The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events.
		//    '                    If Not (mKeyEventArgs.Control And mKeyEventArgs.KeyCode = Keys.Z) Then  '(i.e. Not attempting Undo)
		//    '                        Select Case mKeyEventArgs.KeyCode
		//    '                            Case Keys.D0 To Keys.D9
		//    '                            Case Keys.NumPad0 To Keys.NumPad9
		//    '                            Case Keys.Home, Keys.End, Keys.Left, Keys.Right 'Directional and arrow keys
		//    '                            Case Keys.Back  'Backspace
		//    '                            Case Keys.Delete
		//    '                            Case Else
		//    '                                'Use the following initialization to trigger the conversion exception (if any)
		//    '                                Dim strPrice As String = CType(tb.Text, Decimal).ToString("c")
		//    '                        End Select
		//    '                    End If
		//    '                Else
		//    '                    Dim newChar As Short = 0
		//    '                    If mKeyEventArgs.Control And mKeyEventArgs.Shift Then
		//    '                        Select Case mKeyEventArgs.KeyCode   'Asc(e.KeyChar)
		//    '                            Case Keys.C : newChar = 169 '©
		//    '                            Case Keys.D : newChar = 176 '°
		//    '                            Case Keys.R : newChar = 174 '®
		//    '                            Case Keys.T : newChar = 153 '™
		//    '                            Case Else : newChar = 0
		//    '                        End Select
		//    '                    End If
		//    '                    If newChar <> 0 Then
		//    '                        If tb.SelectionLength > 0 Then
		//    '                            tb.SelectedText = Chr(newChar)
		//    '                        Else
		//    '                            Dim ss As Integer = tb.SelectionStart
		//    '                            tb.Text = tb.Text.Substring(0, tb.SelectionStart) & Chr(newChar) & tb.Text.Substring(tb.SelectionStart)
		//    '                            tb.SelectionStart = ss + 1
		//    '                        End If
		//    '                        e.Handled = True
		//    '                    End If
		//    '                End If
		//    '        End Select
		//    '    Catch ex As Exception
		//    '        CType(mActiveForm, frmTCBase).epBase.SetError((Control)sender, ex.Message)
		//    '        e.Handled = True
		//    '    End Try
		//    'End Sub
		//    'Public Sub Control_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
		//    '    'No tracing on purpose (performance)...
		//    '    mKeyEventArgs = e
		//    '    Try
		//    '        CType(mActiveForm, frmTCBase).epBase.SetError((Control)sender, bpeNullString)
		//    '        Select Case sender.GetType.Name
		//    '            Case "DateTimePicker"
		//    '                Dim dtp As DateTimePicker = CType(sender, DateTimePicker)
		//    '                Select Case mKeyEventArgs.KeyCode
		//    '                    Case Keys.F5 : dtp.Value = Now : e.Handled = True
		//    '                    Case Keys.F6 : dtp.Value = New Date(1963, 7, 31) : e.Handled = True
		//    '                End Select
		//    '        End Select
		//    '    Catch ex As Exception
		//    '        CType(mActiveForm, frmTCBase).epBase.SetError((Control)sender, ex.Message)
		//    '        e.Handled = True
		//    '    End Try
		//    'End Sub
		//    'Protected Friend Sub EnableControl(ByVal ctl As Control, ByVal Enable As Boolean)
		//    '    Const EntryName As String = "EnableControl"
		//    '    Dim strTag As String = IIf(ctl.Tag Is Nothing, "", CType(ctl.Tag, String))
		//    '    Dim tagParams() As String = strTag.Split(",".ToCharArray)
		//    '    For i As Integer = 0 To tagParams.Length - 1
		//    '        Select Case tagParams(i).ToUpper
		//    '            Case "IGNORE" : Exit Sub
		//    '        End Select
		//    '    Next i
		//    '    Select Case TypeName(ctl)
		//    '        Case "Button"
		//    '            CType(ctl, Button).Enabled = Enable
		//    '        Case "CheckBox"
		//    '            CType(ctl, CheckBox).Enabled = Enable
		//    '        Case "ComboBox"
		//    '            Dim cb As Windows.Forms.ComboBox = CType(ctl, Windows.Forms.ComboBox)
		//    '            cb.Enabled = Enable
		//    '            If Not cb.Focused Then cb.SelectionLength = 0
		//    '        Case "DateTimePicker"
		//    '            CType(ctl, DateTimePicker).Enabled = Enable
		//    '        Case "Form"
		//    '            CType(ctl, Form).Enabled = Enable
		//    '        Case "GroupBox"
		//    '            EnableControls(CType(ctl, GroupBox).Controls, Enable)
		//    '        Case "HScrollBar"
		//    '        Case "Label"
		//    '        Case "PictureBox"
		//    '            CType(ctl, PictureBox).Enabled = Enable
		//    '        Case "RichTextBox"
		//    '            With CType(ctl, RichTextBox)
		//    '                '.Enabled = Enable  'Allow scrolling, etc.
		//    '                .ReadOnly = Not Enable
		//    '                .BackColor = IIf(Enable, System.Drawing.SystemColors.Window, System.Drawing.SystemColors.Control)
		//    '            End With
		//    '        Case "StatusBar"
		//    '        Case "TabControl"
		//    '            EnableControls(CType(ctl, TabControl).Controls, Enable)
		//    '        Case "TabPage"
		//    '            EnableControls(CType(ctl, TabPage).Controls, Enable)
		//    '        Case "TextBox"
		//    '            With CType(ctl, TextBox)
		//    '                .Enabled = Enable
		//    '                .ReadOnly = Not Enable
		//    '            End With
		//    '        Case "TreeView"
		//    '            With CType(ctl, TreeView)
		//    '                .Enabled = Enable
		//    '                .BackColor = IIf(Enable, System.Drawing.SystemColors.Window, System.Drawing.SystemColors.Control)
		//    '            End With
		//    '        Case "ToolBar"
		//    '        Case "VScrollBar"
		//    '        Case Else
		//    '            Throw New Exception(String.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", TypeName(ctl), EntryName, ctl.Name))
		//    '    End Select
		//    'End Sub
		//    'Protected Friend Sub EnableControls(ByVal pControls As Control.ControlCollection, ByVal Enable As Boolean)
		//    '    For Each ctl As Control In pControls
		//    '        EnableControl(ctl, Enable)
		//    '    Next
		//    'End Sub
		//    'Protected Friend Sub EnableControls(ByVal Enable As Boolean)
		//    '    For Each iBinding As Binding In mCurrencyManager.Bindings
		//    '        EnableControl(iBinding.Control, Enable)
		//    '    Next
		//    'End Sub
		//    'Protected Friend Sub FixComboBoxes(ByVal ctl As Control)
		//    '    Dim strTag As String = IIf(ctl.Tag Is Nothing, "", CType(ctl.Tag, String))
		//    '    Select Case TypeName(ctl)
		//    '        Case "ComboBox"
		//    '            Dim cb As Windows.Forms.ComboBox = CType(ctl, Windows.Forms.ComboBox)
		//    '            If Not cb.Focused Then cb.SelectionLength = 0
		//    '        Case "GroupBox"
		//    '            FixComboBoxes(CType(ctl, GroupBox).Controls)
		//    '        Case "TabControl"
		//    '            FixComboBoxes(CType(ctl, TabControl).Controls)
		//    '        Case "TabPage"
		//    '            FixComboBoxes(CType(ctl, TabPage).Controls)
		//    '        Case Else
		//    '    End Select
		//    'End Sub
		//    'Protected Friend Sub FixComboBoxes(ByVal pControls As Control.ControlCollection)
		//    '    For Each ctl As Control In pControls
		//    '        FixComboBoxes(ctl)
		//    '    Next
		//    'End Sub
		//    'Protected Friend Overloads Function TagContains(ByVal Tag As Object, ByVal Token As String) As Boolean
		//    '    TagContains = False
		//    '    If IsNothing(Tag) OrElse IsNothing(Token) Then Throw New ApplicationException("Tag argument is required!")
		//    '    If IsNothing(Token) OrElse Token = bpeNullString Then Throw New ApplicationException("Token argument is required!")
		//    '    Dim Tokens() As String = CStr(Tag).ToUpper.Split(New Char() {",", ";"})
		//    '    For iToken As Short = 0 To Tokens.Length - 1
		//    '        If Tokens(iToken) = Token.ToUpper Then TagContains = True : Exit Function
		//    '    Next
		//    '    TagContains = False
		//    'End Function
		//#End Region
		#endif
		#endregion
		#region "Commands"
		private bool AnythingHasChanged(object originalValue, object newValue)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			if (Information.IsDBNull(originalValue) && Information.IsDBNull(newValue))
				return false;
			if (Information.IsDBNull(originalValue) && !Information.IsDBNull(newValue))
				return true;
			if (!Information.IsDBNull(originalValue) && Information.IsDBNull(newValue))
				return true;
			switch (Information.TypeName(originalValue)) {
				case "Byte()":
					if (Convert.ToByte(originalValue).CompareTo(Convert.ToByte(newValue)) != 0)
						return true;
					if ((!object.ReferenceEquals(Convert.ToByte(originalValue), Convert.ToByte(newValue))))
						return true;
					break;
				default:
					if (originalValue != newValue)
						return true;
					break;
			}
			return functionReturnValue;
		}
		private bool AnythingHasChanged(DataRow Row, string Column, ref object newValue)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			object originalValue = Row[Column, DataRowVersion.Original];
			object defaultValue = Row[Column, DataRowVersion.Default];
			//Dim currentValue As Object = Row(Column, DataRowVersion.Current)
			//Dim proposedValue As Object = Row(Column, DataRowVersion.Proposed)
			newValue = defaultValue;

			if (Information.IsDBNull(originalValue) && Information.IsDBNull(newValue))
				return false;
			if (Information.IsDBNull(originalValue) && !Information.IsDBNull(newValue))
				return true;
			if (!Information.IsDBNull(originalValue) && Information.IsDBNull(newValue))
				return true;
			switch (Information.TypeName(originalValue)) {
				case "Byte()":
					if (Convert.ToByte(originalValue).CompareTo(Convert.ToByte(newValue)) != 0)
						return true;
					if ((!object.ReferenceEquals(Convert.ToByte(originalValue), Convert.ToByte(newValue))))
						return true;
					break;
				default:
					if (originalValue != newValue)
						return true;
					break;
			}
			return functionReturnValue;
		}
		public void ConnectCommand(string UserName = clsSupport.bpeNullString, string Password = clsSupport.bpeNullString)
		{
			const string EntryName = "ConnectCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				OpenConnection(UserName, Password);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void CancelCommand()
		{
			const string EntryName = "CancelCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				try {
					switch (Mode) {
						case ActionModeEnum.modeAdd:
						case ActionModeEnum.modeCopy:
						case ActionModeEnum.modeModify:
							if (mDRV.IsEdit) {
								Trace("mDRV.Row.EndEdit()", trcOption.trcDB | trcOption.trcApplication);
								mDRV.Row.EndEdit();
							}
							//The necessary .EndEdit above always seems to trigger our real DataRowState to DataRowState.Modified. 
							//Since we're trying to see if the user actually did change a value, compare column-by-column manually...
							DataRowState myRowState = DataRowState.Unchanged;
							foreach (DataColumn iColumn in mDRV.Row.Table.Columns) {
								//Debug.WriteLine(String.Format("{0}: Original: ""{1}""; Current: ""{2}""", iColumn.ColumnName, mDRV.Row(iColumn.ColumnName, DataRowVersion.Original).ToString, mDRV.Row(iColumn.ColumnName, DataRowVersion.Default).ToString))
								if (mDRV.Row.HasVersion(DataRowVersion.Original)) {
									if (AnythingHasChanged(mDRV.Row[iColumn.ColumnName, DataRowVersion.Original], mDRV.Row[iColumn.ColumnName, DataRowVersion.Default])) {
                                        myRowState = DataRowState.Modified;
                                        break; 
                                    }
								} else {
									myRowState = mDRV.Row.RowState;
									//Assume the record's being added...
								}
							}

							if (myRowState != DataRowState.Unchanged && MessageBox.Show(this.ActiveForm, "All entries will be discarded; are you sure you want to Cancel?", "Cancel Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
								return;

							//The RejectChanges will trigger a CurrencyManager ListChanged event, so be prepared...
							if (Mode == ActionModeEnum.modeAdd || Mode == ActionModeEnum.modeCopy){mRecordID = mSaveRecordID;tcDataView.RowFilter = mSaveFilter;}

							Trace("tcDataSet.RejectChanges()", trcOption.trcDB | trcOption.trcApplication);
							tcDataSet.RejectChanges();

							Move(FindRowByID(mRecordID));
							//Trace("mCurrencyManager.Refresh()", trcOption.trcDB Or trcOption.trcApplication)
							//mCurrencyManager.Refresh()
							OnActionModeChange(ActionModeEnum.modeDisplay);
							break;
						default:
							throw new Exception(string.Format("Unexpected ActionMode ({0}) encountered in {1}().", Mode.ToString(), EntryName));
					}
					//If Mode <> ActionMode.modeDisplay Then Call BindControls(frm, pRS)
					mSaveFilter = clsSupport.bpeNullString;
				} finally {
					if (mActiveTXLevel > 0)
						AbortTrans();
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void CopyCommand()
		{
			const string EntryName = "CopyCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mSaveRecordID = RecordID;
				DataRowView saveRow = mDRV;
				int newID = GetID(this.TableName);
				//Set our Mode before we start manipulating our data to allow other methods to know what's going on. The OnActionModeChange event is intended for screen work.
				mMode = ActionModeEnum.modeCopy;
				Trace("Copying Record #{0} as new {1} row (DataRowView) #{2}", new object[] {
					mSaveRecordID,
					this.TableName,
					newID
				}, trcOption.trcDB | trcOption.trcApplication);
				Trace("{0}mDRV = tcDataView.AddNew()", Constants.vbTab, trcOption.trcDB | trcOption.trcApplication);
				mDRV = tcDataView.AddNew();
				Trace("{0}(RowState: {1}; RowVersion: {2};{3}{4})", new object[] {
					Constants.vbTab,
					mDRV.Row.RowState.ToString(),
					mDRV.RowVersion.ToString(),
					(mDRV.IsEdit ? " IsEdit; " : ""),
					(mDRV.IsNew ? " IsNew; " : "")
				}, trcOption.trcApplication);

				//Copy data from saveRow and treat as an add...
				foreach (DataColumn iColumn in mDRV.Row.Table.Columns) {
					mDRV[iColumn.ColumnName] = (iColumn.ColumnName == mTableIDColumn ? newID : saveRow[iColumn.ColumnName]);
				}
				Trace("{0}Copied {1} column data", Constants.vbTab, mDRV.Row.Table.Columns.Count, trcOption.trcApplication);
				if (TraceMode)
					Dump(mDRV);
				Trace("{0}mDRV.EndEdit()", Constants.vbTab, trcOption.trcApplication);
				mDRV.EndEdit();
				if (TraceMode)
					Dump(mDRV);

				//'Documentation says mCurrenctManager.Begin/Cancel/EndCurrentEdit should only be used when building your own complex control (like a DataGrid)...
				//Trace("mCurrencyManager.EndCurrentEdit(); mDRV:={0}, cmRow:={1}; RecordID:={2}", FindRowByID(newID), mCurrencyManager.Position, newID, trcOption.trcApplication)
				//mCurrencyManager.EndCurrentEdit()
				//...however, in practice not using the mCurrencyManager.EndCurrentEdit() caused all the existing [working] logic to fall apart - so it stays!
				//Trace("mDRV.Row.EndEdit()", trcOption.trcDB)
				//mDRV.Row.EndEdit()

				//Need to save our current RowFilter so we can add a new record that may not be included in the DataView based on the current RowFilter...
				mSaveFilter = tcDataView.RowFilter;
				tcDataView.RowFilter = string.Format("{0}={1}", mTableIDColumn, mDRV[mTableIDColumn]);
				int newRowIndex = FindRowByID(newID);
				Trace("{0}Move({1})", Constants.vbTab, newRowIndex, trcOption.trcApplication);
				Move(newRowIndex);
				Trace("mDRV.BeginEdit(); mDRV:={0}; cmRow:={1}; RecordID:={2}", newRowIndex, mCurrencyManager.Position, newID, trcOption.trcApplication);
				mDRV.BeginEdit();
				if (TraceMode)
					Dump(mDRV);
				OnActionModeChange(ActionModeEnum.modeCopy);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public void DeleteCommand()
		{
			const string EntryName = "DeleteCommand";
            try
            {
                Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
                SqlDataAdapter da = tcDataAdapter;
                DataTable dt = tcDataView.Table;
                DataSet ds = dt.DataSet;
                try
                {
                    mSaveRecordID = mRecordID;
                    int rowIndex = FindRowByID(mRecordID);
                    //Set our Mode before we start manipulating our data to allow other methods to know what's going on. The OnActionModeChange event is intended for screen work.
                    mMode = ActionModeEnum.modeDelete;
                    Trace("Deleting Record #" + mSaveRecordID, trcOption.trcDB | trcOption.trcApplication);

                    if (MessageBox.Show(this.ActiveForm, "Are you sure you want to permanently delete this record?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        throw new ExitTryException();
                    OnActionModeChange(ActionModeEnum.modeDelete);

                    BeginTrans();
                    foreach (DataColumn iColumn in mDRV.Row.Table.Columns)
                    {
                        SaveHistory(mDRV.Row, iColumn.ColumnName);
                    }
                    OnBeforeDelete(mDRV);
                    tcDataView.Delete(rowIndex);
                    var _with2 = da.DeleteCommand;
                    _with2.CommandText = string.Format("Delete From [{0}] Where [{1}]={2}", this.TableName, mTableIDColumn, mRecordID);
                    _with2.CommandType = CommandType.Text;
                    //Use the DataAdapter (through the DeleteCommand we just set up) to update the database...
                    da.Update(ds, this.TableName);
                    OnDeleteInProcess(mDRV);
                    EndTrans();
                    OnDeleteComplete(mDRV);

                    Trace("tcDataSet.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
                    tcDataSet.AcceptChanges();
                    Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
                    mCurrencyManager.Refresh();
                    OnActionModeChange(ActionModeEnum.modeDisplay);
                    Move(rowIndex);
                }
                finally
                {
                    AbortTrans();
                    OnActionModeChange(ActionModeEnum.modeDisplay);
                }
            } catch (ExitTryException ex) { 
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void FilterCommand()
		{
			const string EntryName = "FilterCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				frmSelect frm = null;
				try {
					Trace("", trcOption.trcApplication);
					frm = new frmSelect(mSupport, this, mActiveForm, string.Format("{0} Select", mActiveForm.Text));
					frm.ShowDialog();
					Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
					mCurrencyManager.Refresh();
				} finally {
					if (frm != null){frm.Dispose();frm = null;}
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void HistoryCommand(ref Boolean AllowUpdate)
        {
			//TODO: Complete HistoryCommand()
        }
		public void ListCommand(bool AllowUpdate = true)
		{
			const string EntryName = "ListCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				frmList frm = null;
				try {
					frm = new frmList(mSupport, this, mActiveForm, mActiveForm.Text + " List", tcDataView, mCurrencyManager.Position, AllowUpdate, mTableIDColumn);
					mActiveForm.Hide();
					frm.ShowDialog();
					Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
					mCurrencyManager.Refresh();
				} finally {
					if (frm != null){frm.Dispose();frm = null;}
					mActiveForm.Show();
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DataView MakeDataViewCommand(string SQLSource, string TableName, bool AddToDBCollection = true)
		{
			DataView functionReturnValue = null;
			const string EntryName = "MakeDataViewCommand";
			DataSet ds = new DataSet();
			DataView dv = null;
			string FieldList = clsSupport.bpeNullString;
			string TableList = clsSupport.bpeNullString;
			string WhereClause = clsSupport.bpeNullString;
			string OrderByClause = clsSupport.bpeNullString;
			bool Distinct = false;
			int RecordCount = 0;
			functionReturnValue = null;
			if (!Connected)
				throw new ApplicationException("Not Connected.");

			if (TokenCount(SQLSource, ";", "'") > 1)
				throw new ApplicationException("Multiple SQL Statements not supported.");
			ParseSQLSelect(SQLSource, ref FieldList, ref TableList, ref WhereClause, ref OrderByClause, ref Distinct);
			SQLSource = string.Format("Select{0} {1} From {2}", (Distinct ? " Distinct" : clsSupport.bpeNullString), FieldList, TableList);
			if (WhereClause != clsSupport.bpeNullString)
				SQLSource = string.Format("{0} Where {1}", SQLSource, WhereClause);

			Trace(EntryName + " - tcDataAdapter.SelectCommand.CommandText=\"" + SQLSource + "\"", trcOption.trcDB | trcOption.trcDB);
            tcDataAdapter.SelectCommand.CommandText = SQLSource;
            tcDataAdapter.SelectCommand.CommandType = CommandType.Text;
            tcDataAdapter.SelectCommand.Connection = tcConnection;
			if ((tcTransaction != null)) {
				if (object.ReferenceEquals(tcTransaction.Connection, tcDataAdapter.SelectCommand.Connection))
                    tcDataAdapter.SelectCommand.Transaction = tcTransaction;
			}
            tcDataAdapter.SelectCommand.CommandTimeout = mCommandTimeout;
			Trace(EntryName + " - DataAdapter.SelectCommand.CommandText=\"" + SQLSource + "\"", trcOption.trcDB | trcOption.trcDB);
            tcDataAdapter.FillSchema(ds, SchemaType.Source);
			//FillSchema doesn't seem to want to use the default constraint from the database, so roll our own default here...
			initColumnDefaults(tcDataAdapter.FillSchema(ds, SchemaType.Source)[0].Columns);
			RecordCount = tcDataAdapter.Fill(ds);

			//Before attempting to apply the sort criteria, make sure the OrderByClause corresponds to 
			//valid columns in the results table... Remember, under ADO.NET, all the query knows is the
			//list of fields returned in the DataTable... The fact that the underlying table in the 
			//database may contain such a field is irrelevant...
			string myOrderByClause = clsSupport.bpeNullString;
			for (short iSQL = 1; iSQL <= TokenCount(OrderByClause, ",", "\""); iSQL++) {
				string Token = Strings.Trim(ParseStr(OrderByClause, iSQL, ",", "\""));
				//Handle ASC/DESC that may be present...
				string testToken = ParseStr(Token, 1, " ");
				string SortOrder = ParseStr(Token, 2, " ");
				switch (SortOrder.ToUpper()) {
					case "":
					case "ASC":
					case "DESC":
						break;
					default:
						throw new ApplicationException(string.Format("Invalid Sort Order detected ({0}) near column {1}.", SortOrder, testToken));
				}
				if (Information.IsNumeric(testToken)) {
					//Support use of ordinal numbers to identify fields, but replace these in our temporary
					//myOrderByClause with the real column names otherwise the Sort will fail...
					//Note: DataTable.Columns is zero-based, while use of ordinals is not...
					if (Convert.ToInt32(testToken) > ds.Tables[0].Columns.Count) {
						throw new ApplicationException(string.Format("Order By column ({0}) exceeds the number of fields in this query.", testToken));
					} else if (Convert.ToInt32(testToken) < 1) {
						throw new ApplicationException(string.Format("Invalid Order By column ({0}).", testToken));
					} else {
						myOrderByClause += string.Format("{0}{1},", ds.Tables[0].Columns[Convert.ToInt32(testToken) - 1].ColumnName, (SortOrder != clsSupport.bpeNullString ? " " + SortOrder : clsSupport.bpeNullString));
						//Handle ASC/DESC that may be present...
					}
				} else {
					//Scan the table to ensure a column matching Token exists, fail if not...
					bool FoundColumn = false;
                    bool continueLoop = true;
                    for (int iColumn = 0; iColumn <= ds.Tables[0].Columns.Count - 1; iColumn++) {
						//If ds.Tables(0).Columns(iColumn).ColumnName = testToken Then FoundColumn = True : Exit For
						if (Strings.Replace(ds.Tables[0].Columns[iColumn].ColumnName, "\"", clsSupport.bpeNullString) == Strings.Replace(testToken, "\"", clsSupport.bpeNullString)) {
                            FoundColumn = true;
                            continueLoop = false;
                        }
                    }
					if (!FoundColumn) {
						throw new ApplicationException(string.Format("Order By column ({0}) does not appear in columns returned from query.", testToken));
					} else {
						myOrderByClause += string.Format("{0},", Strings.Replace(Token, "\"", clsSupport.bpeNullString));
					}
				}
			}
			if (myOrderByClause != clsSupport.bpeNullString)
				myOrderByClause = Strings.Left(myOrderByClause, Strings.Len(myOrderByClause) - 1);

			dv = new DataView(ds.Tables[0], null, myOrderByClause, DataViewRowState.CurrentRows);
			Trace("{0} - Complete; {1:#,##0} Records...", EntryName, dv.Count.ToString(), trcOption.trcApplication | trcOption.trcDB | trcOption.trcDB);
			if (AddToDBCollection) {
				DataTable dt = dv.Table;
				//clsDBEngine's OpenDataView would have opened its own DataSet, so move this view into our DataSet so
				//we can manage changes all in one place...
				dt.TableName = TableName;
				ds.Tables.Remove(dt);
				tcDataSet.Tables.Add(dt);
				mDBCollection.Add(TableName, SQLSource, dv);
			}
			functionReturnValue = dv;
			return functionReturnValue;
		}
		public void ModifyCommand()
		{
			const string EntryName = "ModifyCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mSaveRecordID = RecordID;
				//SaveOriginalDataValues()
				//Set our Mode before we start manipulating our data to allow other methods to know what's going on. The OnActionModeChange event is intended for screen work.
				mMode = ActionModeEnum.modeModify;
				Trace("mDRV.Row.BeginEdit()", trcOption.trcDB | trcOption.trcApplication);
				mDRV.Row.BeginEdit();
				OnActionModeChange(ActionModeEnum.modeModify);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void NewCommand()
		{
			const string EntryName = "NewCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				mSaveRecordID = RecordID;
				int newID = (int)GetID(this.TableName);
				//Set our Mode before we start manipulating our data to allow other methods to know what's going on. The OnActionModeChange event is intended for screen work.
				mMode = ActionModeEnum.modeAdd;
				Trace("Adding new {0} row (DataRowView) ID #{1}", this.TableName, newID, trcOption.trcApplication | trcOption.trcDB);
				Trace("Copying Record ID #{0} as new {1} row (DataRowView) ID #{2}", mSaveRecordID, this.TableName, newID, trcOption.trcDB | trcOption.trcApplication);
				Trace("{0}mDRV = tcDataView.AddNew()", Constants.vbTab, trcOption.trcDB | trcOption.trcApplication);
				mDRV = tcDataView.AddNew();
				Trace("{0}(RowState: {1}; RowVersion: {2};{3}{4})", new object[] {
					Constants.vbTab,
					mDRV.Row.RowState.ToString(),
					mDRV.RowVersion.ToString(),
					(mDRV.IsEdit ? " IsEdit; " : ""),
					(mDRV.IsNew ? " IsNew; " : "")
				}, trcOption.trcApplication);

				//Sanity Check - catch it before we allow the user to enter anything that we know won't be able to be posted later...
				DataView tmpDV = null;
				try {
					tmpDV = new DataView(tcDataView.Table, "", "", DataViewRowState.ModifiedOriginal);
					if (tmpDV.Count > 0)
						throw new ApplicationException(string.Format("Application Error: DataView has an uncommitted change (row #{0}; Key: \"{1}\". Process cannot complete.", tmpDV[0][mTableIDColumn], tmpDV[0][mTableKeyColumn]));
				} finally {
					tmpDV = null;
				}

				mDRV[mTableIDColumn] = newID;
				if (TraceMode)
					Dump(mDRV);

				//Need to save our current RowFilter so we can add a new record that may not be included in the DataView based on the current RowFilter...
				mSaveFilter = tcDataView.RowFilter;
				tcDataView.RowFilter = string.Format("{0}={1}", mTableIDColumn, mDRV[mTableIDColumn]);
				int newRowIndex = FindRowByID(newID);
				Trace("{0}Move({1})", Constants.vbTab, newRowIndex, trcOption.trcApplication);
				Move(newRowIndex);
				Trace("mDRV.BeginEdit(); mDRV:={0}; cmRow:={1}; RecordID:={2}", newRowIndex, mCurrencyManager.Position, newID, trcOption.trcApplication);
				mDRV.BeginEdit();
				if (TraceMode)
					Dump(mDRV);
				OnActionModeChange(ActionModeEnum.modeAdd);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void OKCommand()
		{
			const string EntryName = "OKCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				switch (Mode) {
					case ActionModeEnum.modeAdd:
					case ActionModeEnum.modeCopy:
						NewData();
						break;
					case ActionModeEnum.modeModify:
						OnUnbindControls();
						ModifyData();
						break;
					default:
						throw new Exception(string.Format("Unexpected ActionMode ({0}) encountered in {1}().", Mode.ToString(), EntryName));
				}
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void RefreshCommand()
		{
			//TODO: Determine if RefreshCommand is obsolete...
			//        Const EntryName As String = "RefreshCommand"
			//Try : Trace(trcType.trcEnter, EntryName, trcOption.trcApplication)
			//        Dim frm As System.Windows.Forms.Form
			//        Dim pRS As ADODB.Recordset
			//        Dim SaveID As Integer
			//        Dim DBinfo As clsDataBaseInfo

			//        On Error GoTo ErrorHandler
			//        frm = pForm
			//        pRS = pRecordSet

			//        SaveID = RecordID

			//        UnbindControls(frm, pRS)
			//        For Each DBinfo In mDBCollection
			//            objTrace.Trace(SIASUTL.trcType.trcBody, "Refreshing DBinfo:" & DBinfo.Key)
			//            On Error Resume Next
			//            'UPGRADE_WARNING: Couldn't resolve default property of object frm.sbStatus. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			//            frm.sbStatus.Panels("Message").Text = "Refreshing " & DBinfo.Key & "  Recordset..."
			//            On Error GoTo ErrorHandler

			//            SIASADOclsADO_definst.CloseRecordset((DBinfo.Recordset), False)
			//            mDBE.MakeVirtualRecordset((DBinfo.SQLSource), (DBinfo.Recordset))
			//        Next DBinfo
			//        BindControls(frm, pRS)
			//        FindID(pRS, SaveID)

			//        'UPGRADE_NOTE: Object frm may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        frm = Nothing
			//        'UPGRADE_NOTE: Object pRS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        pRS = Nothing
			//Finally : Trace(trcType.trcExit, EntryName, trcOption.trcApplication)
			//End Try
		}
		public void ReportCommand(string ReportPath, string Caption = null)
		{
			const string EntryName = "ReportCommand";
			Form frm = null;
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				Busy(true);
				switch (ParsePath(ReportPath, ParseParts.FileNameExt).ToLower()) {
					case ".rdlc":
						//Microsoft SSRS
						//frm = new frmViewer(mSupport, this, ReportPath, mActiveForm, ((Caption == null) ? mActiveForm.Text + " Report" : Caption));
						//Note that due to the strongly typed DataSource/DataAdapter infrastructure generated by VS2013 seemingly 
						//required to drive Microsoft's SQL Server Reporting Services reports, our component-based settings must be 
						//done within frmViewer_Load (although I'd rather we do this via parameters passed from the individual 
						//components themselves through this method and into frmViewer but I can't figure out how to generalize this 
						//strongly-typed DataSource/DataAdapter stuff)...
						frm.ShowDialog();
						break;
					case ".rpt":
						if (!new FileInfo(ReportPath).Exists)
							throw new FileNotFoundException(string.Format("\"{0}\" not found.", ReportPath));
						frm = new frmReport(mSupport, this, mActiveForm, ((Caption == null) ? mActiveForm.Text + " Report" : Caption));
						frm.Icon = mActiveForm.Icon;
						//Objects used to set the proper database connection information
						CrystalDecisions.CrystalReports.Engine.ReportDocument creReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
						CrystalDecisions.CrystalReports.Engine.Table creTable = null;
						CrystalDecisions.Shared.TableLogOnInfo crsLogonInfo = null;

						creReport.Load(ReportPath);
						//Load the report

						//Set the connection information for all the tables used in the report (Leave UserID and Password blank for trusted connection)
						foreach (CrystalDecisions.CrystalReports.Engine.Table creTable_loopVariable in creReport.Database.Tables) {
							creTable = creTable_loopVariable;
							crsLogonInfo = creTable.LogOnInfo;
                            crsLogonInfo.ConnectionInfo.ServerName = mServerName;
                            crsLogonInfo.ConnectionInfo.DatabaseName = mDatabaseName;
							if (mIntegratedSecurity) {
                                crsLogonInfo.ConnectionInfo.IntegratedSecurity = true;
                                crsLogonInfo.ConnectionInfo.UserID = clsSupport.bpeNullString;
                                crsLogonInfo.ConnectionInfo.Password = clsSupport.bpeNullString;
							} else {
                                crsLogonInfo.ConnectionInfo.IntegratedSecurity = false;
                                crsLogonInfo.ConnectionInfo.UserID = mUserID;
                                crsLogonInfo.ConnectionInfo.Password = mPassword;
							}
							creTable.ApplyLogOnInfo(crsLogonInfo);

							//TODO: Need newer version of Crystal Reports (XI R2 or 2013)
							//Dim rasReport As ReportClientDocumentWrapper = creReport.ReportClientDocument
							//Dim DDC As DataDefController = rasReport.DataDefController
							//Dim ddcDB As DataDefModel.Database = ddc.Database
							//Dim ddcDBTable As DataDefModel.CommandTable = ddcDB.Tables(0)
							//ddcDBTable.CommandText = SQL
						}

						//Set the report source for the crystal reports viewer to the report instance.
						((frmReport)frm).crvMain.ReportSource = creReport;
						//Zoom viewer to fit to the whole page so the user can see the report
						//frm.crvMain.Zoom(2)

						frm.ShowDialog();
						break;
					//Trace("mCurrencyManager.Refresh()", trcOption.trcDB + trcOption.trcApplication)
					//mCurrencyManager.Refresh()
				}
			} finally {
				if (frm != null){frm.Dispose();frm = null;}
				Busy(false);
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void SelectCommand()
		{
			const string EntryName = "SelectCommand";
			frmSelect frm = null;
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				frm = new frmSelect(mSupport, this, mActiveForm, string.Format("{0} Select", mActiveForm.Text));
				frm.Icon = mActiveForm.Icon;
				frm.ShowDialog();
				Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
				mCurrencyManager.Refresh();
			} finally {
				if (frm != null){frm.Dispose();frm = null;}
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void ShowCommand(frmTCBase frm)
		{
			const string EntryName = "ShowCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				Trace("frm:={{{0}}}:={1}", new object[] {
					frm.GetType().Name,
					frm.Name
				}, trcOption.trcApplication);
				frm.SetBounds((int)mSaveLeft + (((int)mMainWidth - frm.Width) / 2), (int)mSaveTop + (((int)mMainHeight - frm.Height) / 2), frm.Size.Width, frm.Size.Height - mDynamicMenuHeight, BoundsSpecified.All);
				this.ActiveForm = frm;
				Trace("frm.ShowDialog(mParent)", trcOption.trcApplication);
				frm.ShowDialog(mParent);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcApplication);
				MessageBox.Show(this.ActiveForm, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		public void ShowCommand(frmTCStandard frm, bool Modal)
		{
			const string EntryName = "ShowCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				SQLFilter = (string)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings\\Select Settings", mRegistryKey, frm.Name), "SQLFilter", clsSupport.bpeNullString);
				//SQLFilter = (string)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, String.Format("{0}\{1} Settings", mRegistryKey, String.Format("{0} Select", frm.Text)), "SQLFilter", bpeNullString)
				frm.SetBounds((int)mSaveLeft + (((int)mMainWidth - frm.Width) / 2), (int)mSaveTop + (((int)mMainHeight - frm.Height) / 2), frm.Size.Width, frm.Size.Height - mDynamicMenuHeight, BoundsSpecified.All);
				if (Modal) {
					this.ActiveForm = frm;
					Trace("frm.ShowDialog(mParent)", trcOption.trcApplication);
					frm.ShowDialog(mParent);
				} else {
					Trace("frm.Show()", trcOption.trcApplication);
					frm.Show();
				}
			} catch (Exception ex) {
				OnActionModeChange(ActionModeEnum.modeDisplay);
				Trace(ex.Message, trcOption.trcApplication);
				MessageBox.Show(this.ActiveForm, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
        public void SortCommand()
        {
            const string EntryName = "SortCommand";
            Form frm = null;
            try
            {
                Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
                //frm = new frmSort(mSupport, this, mActiveForm, string.Format("{0} Sort", mActiveForm.Text));
                frm.Icon = mActiveForm.Icon;
                frm.ShowDialog();
                Trace("mCurrencyManager.Refresh()", trcOption.trcDB | trcOption.trcApplication);
                mCurrencyManager.Refresh();
            }
            finally
            {
                if (frm != null) { frm.Dispose(); frm = null; }
                Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
            }
        }
        public void SQLCommand(string TableName)
		{
			const string EntryName = "SQLCommand";
			frmSQL frm = null;
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcAll);
				frm = new frmSQL(mSupport, this, mActiveForm, mActiveForm.Text + " SQL");
				frm.Icon = mActiveForm.Icon;
				frm.AccessLevel = frmSQL.dbeAccessEnum.dbaDML;
				frm.AllowStore = true;
				frm.ShowDialog();
			} finally {
				if (frm != null){frm.Dispose();frm = null;}
				Trace(trcType.trcExit, EntryName, trcOption.trcAll);
			}
		}
		public void UnloadCommand(Form frm, DataView dv)
		{
			const string EntryName = "UnloadCommand";
			try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcApplication);
				if ((dv != null)) {
                    SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", RegistryKey, frm.Name), string.Format("{0}.{1}", mTableName, mTableIDColumn), mRecordID);
					//SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, String.Format("{0}\{1} Settings", RegistryKey, frm.Name), "Filter", dv.RowFilter)
				}
				SaveBounds(string.Format("{0}\\{1} Settings", RegistryKey, frm.Name), frm.Left, frm.Top, frm.Width, frm.Height);
				mMode = ActionModeEnum.modeDisplay;
				//Just to be sure it's properly initialized for the next screen...
				mCurrencyManager = null;
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcApplication);
			}
		}
		#endregion
		#region "Data Methods"
		#region "Connection Stuff"
		/// <summary>
		/// Builds a ODBC-compliant connection string based on the given parameters
		/// </summary>
		/// <param name="DSN">File DSN representing the preconfigured connection to the desired database</param>
		/// <param name="UserName">User name credential used to make the connection</param>
		/// <param name="Password">Password credential used to make the connection</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public string BuildConnectionString(string DSN, ref string UserName, string Password)
		{
			string functionReturnValue = null;
			bool fPersistSecurityInfo = false;
			int iPacketSize = 0;
			functionReturnValue = clsSupport.bpeNullString;

			if (DSN == clsSupport.bpeNullString)
				throw new Exception("No FileDSN specified.");

			mFileDSN = DSN;
			switch (Strings.Trim(GetINIKey(DSN, "ODBC", "Driver", clsSupport.bpeNullString))) {
				case "SQL Server":
				case "SQL Native Client":
					break;
				default:
					throw new Exception("Unsupported driver: " + Strings.Trim(GetINIKey(DSN, "ODBC", "Driver", clsSupport.bpeNullString)));
			}

			mServerName = GetINIKey(DSN, "ODBC", "SERVER", clsSupport.bpeNullString);
			mDatabaseName = GetINIKey(DSN, "ODBC", "DATABASE", clsSupport.bpeNullString);
			//"Trusted_Connection" and "Integrated Security" mean the same thing, but the ODBC Administrator 
			//likes to use "Trusted_Connection" while SQL Server tools prefer "Integrated Security"...
			switch (Strings.UCase(GetINIKey(DSN, "ODBC", "TRUSTED_CONNECTION", "Unknown"))) {
				case "YES":
				case "Y":
				case "TRUE":
				case "T":
				case "SSPI":
					mIntegratedSecurity = true;
					break;
				case "NO":
				case "N":
				case "FALSE":
				case "F":
					mIntegratedSecurity = false;
					break;
				case "UNKNOWN":
					switch (Strings.UCase(GetINIKey(DSN, "ODBC", "INTEGRATEDSECURITY", "Unknown"))) {
						case "YES":
						case "Y":
						case "TRUE":
						case "T":
						case "SSPI":
							mIntegratedSecurity = true;
							break;
						default:
							mIntegratedSecurity = false;
							break;
					}
					break;
			}
			iPacketSize = Convert.ToInt32(GetINIKey(DSN, "ODBC", "PACKETSIZE", "8192"));
			//See the following link for SqlConnection.ConnectionString settings...
			//ms-help://MS.VSCC.2003/MS.MSDNQTR.2005APR.1033/cpref/html/frlrfsystemdatasqlclientsqlconnectionclassconnectionstringtopic.htm
			functionReturnValue += String.Format("Application Name={0};", mSupport.ApplicationName);
			functionReturnValue += String.Format("Workstation ID={0};", TCBase.My.MyProject.Computer.Name);
			functionReturnValue += String.Format("Data Source={0};", ServerName);
			//aka "Server" or "Address" or "Addr" or "Network Address"
			functionReturnValue += String.Format("Initial Catalog={0};", mDatabaseName);
			//aka "Database"...
			functionReturnValue += String.Format("Packet Size={0};", iPacketSize);
			//"PersistSecurityInfo=False;" will omit the "Password=<password>" element from the connection string
			//after the connection is open - meaning subsequent use of this connection string to open other connections
			//(like "master" database connections in IsInDB) will fail with invalid login information;
			//BuildConnectionString &= [String].Format("Persist Security Info={0};", fPersistSecurityInfo)
			functionReturnValue += String.Format("Connect Timeout={0};", mConnectionTimeout);
			//aka "Connection Timeout"...
			//Connection Pooling Parameters...
			functionReturnValue += String.Format("Pooling={0};", "False");
			//When true, the connection is drawn from the appropriate pool, or if necessary, created and added to the appropriate pool.
			//BuildConnectionString &= [String].Format("Connection Lifetime={0};", "1")                       'When a connection is returned to the pool, its creation time is compared with the current time, and the connection is destroyed if that time span (in seconds) exceeds the value specified by Connection Lifetime. This is useful in clustered configurations to force load balancing between a running server and a server just brought on-line. A value of zero (0) will cause pooled connections to have the maximum time-out.
			//BuildConnectionString &= [String].Format("Min Pool Size={0};", "0")                             'The minimum number of connections maintained in the pool.
			//BuildConnectionString &= [String].Format("Max Pool Size={0};", "1")                             'The maximum number of connections allowed in the pool.
			if (mIntegratedSecurity) {
				UserName = TCBase.My.MyProject.User.Name;
				functionReturnValue += String.Format("Integrated Security={0};", "SSPI");
			} else {
				functionReturnValue += String.Format("User ID={0};", UserName);
				functionReturnValue += String.Format("Password={0};", Password);
			}
			return functionReturnValue;
		}
		/// <summary>
		/// Closes the current connection
		/// </summary>
		public void CloseConnection()
		{
			try {
				//colControls.Clear()
				foreach (clsDataBaseInfo iDBInfo in mDBCollection) {
					tcDataSet.Tables.Remove(iDBInfo.DataView.Table);
					iDBInfo.DataView.Dispose();
					iDBInfo.DataView = null;
				}
				mDBCollection.Clear();
				if ((tcConnection != null)){tcConnection.Close();tcConnection.Dispose();tcConnection = null;}
			} finally {
				mTableName = clsSupport.bpeNullString;
				mTableIDColumn = clsSupport.bpeNullString;
				mTableKeyColumn = clsSupport.bpeNullString;
				mSQLMain = clsSupport.bpeNullString;
                mSQLfilter = clsSupport.bpeNullString;
                mSQLsort = clsSupport.bpeNullString;
                tcConnection = null;
				tcDataAdapter = null;
				tcTransaction = null;
				tcDataSet = null;
				tcDataView = null;
				mDRV = null;
			}
		}
		/// <summary>
		/// Opens a connection to the given database
		/// </summary>
		/// <param name="UserName">User name credential used to make the connection</param>
		/// <param name="Password">Password credential used to make the connection</param>
		private void OpenConnection(string UserName = clsSupport.bpeNullString, string Password = clsSupport.bpeNullString)
		{
			const string EntryName = "OpenConnection";
			CloseConnection();

			if (UserName == clsSupport.bpeNullString)
				UserName = mUserID;
			if (Password == clsSupport.bpeNullString)
				Password = mPassword;

			if (fUseDefaultConnectionString)
				mConnectionString = mDefaultConnectionString;
			if (mConnectionString == clsSupport.bpeNullString)
				mConnectionString = "FileDSN=" + mFileDSN;
			if (Strings.UCase(ParseStr(mConnectionString, 1, "=")) == "FILEDSN") {
				if (ParseStr(mConnectionString, 2, "=") == clsSupport.bpeNullString)
					throw new ApplicationException("ConnectionString is empty and FileDSN is not provided!");
				mConnectionString = BuildConnectionString(ParseStr(mConnectionString, 2, "="), ref UserName, Password);
			}

			//Note that ADO.NET does not alter the ConnectionString property as ADO does... It should exactly match 
			//mConnectionString after the .Open...
			tcConnection = new System.Data.SqlClient.SqlConnection(mConnectionString);
			tcDataAdapter = new SqlDataAdapter();
			var _with5 = tcDataAdapter;
			_with5.DeleteCommand = new SqlCommand();
			_with5.DeleteCommand.Connection = tcConnection;
			_with5.InsertCommand = new SqlCommand();
			_with5.InsertCommand.Connection = tcConnection;
			_with5.SelectCommand = new SqlCommand();
			_with5.SelectCommand.Connection = tcConnection;
			_with5.UpdateCommand = new SqlCommand();
			_with5.UpdateCommand.Connection = tcConnection;

			Trace("{0}: Connection.Open() - ConnectionString:=\"{1}\"", EntryName, mConnectionString, trcOption.trcDB | trcOption.trcDB);
			tcConnection.Open();
			//mProcessID = GetProcessID()
			tcDataSet = new DataSet();
		}
		#endregion
		#region "Transaction Stuff"
		public void AbortTrans()
		{
			const string EntryName = "AbortTrans";
			try {
                if (!(fAborting || tcTransaction == null))
                {
                    if ((!object.ReferenceEquals(tcConnection, tcTransaction.Connection)))
                        throw new InvalidCastException("Attempting to Rollback a transaction associated with a different connection.");

                    fAborting = true;
                    Trace(EntryName + " - .Rollback", trcOption.trcDB | trcOption.trcDB);
                    tcTransaction.Rollback();
                    tcDataAdapter.SelectCommand.Transaction = null;
                    tcDataAdapter.DeleteCommand.Transaction = null;
                    tcDataAdapter.InsertCommand.Transaction = null;
                    tcDataAdapter.UpdateCommand.Transaction = null;
                    Trace(EntryName + " - .Rollback Complete", trcOption.trcDB | trcOption.trcDB);
                    fAborting = false;
                    mActiveTXLevel = 0;
                }
			} finally {
				if ((tcTransaction != null)){tcTransaction.Dispose();tcTransaction = null;}
				mTransactionName = clsSupport.bpeNullString;
			}
		}
		public SqlTransaction BeginTrans(string Name = "Unspecified", System.Data.IsolationLevel Level = IsolationLevel.RepeatableRead)
		{
			SqlTransaction functionReturnValue = null;
			const string EntryName = "BeginTrans";
			functionReturnValue = null;
			try {
				if (mActiveTXLevel > 0)
					throw new Exception("Transaction is already active. Multiple or nested transactions are not permitted.");

				mTransactionName = Name;
				Trace(EntryName + " - BeginTransaction", trcOption.trcDB | trcOption.trcDB);
				tcTransaction = tcConnection.BeginTransaction(Level, Name);
				tcDataAdapter.SelectCommand.Transaction = tcTransaction;
				tcDataAdapter.DeleteCommand.Transaction = tcTransaction;
				tcDataAdapter.InsertCommand.Transaction = tcTransaction;
				tcDataAdapter.UpdateCommand.Transaction = tcTransaction;
				functionReturnValue = tcTransaction;
				Trace(EntryName + " - BeginTransaction Complete", trcOption.trcDB | trcOption.trcDB);
				mActiveTXLevel += 1;
			} catch (Exception ex) {
				try {
					AbortTrans();
				} catch {
				}
				throw;
			}
			return functionReturnValue;
		}
		public void EndTrans()
		{
			const string EntryName = "EndTrans";
			try {
				if ((!object.ReferenceEquals(tcConnection, tcTransaction.Connection)))
					throw new Exception("Attempting to Commit a transaction associated with a different connection.");

				Trace(EntryName + " - .Commit", trcOption.trcDB | trcOption.trcDB);
				tcTransaction.Commit();
				tcDataAdapter.SelectCommand.Transaction = null;
				tcDataAdapter.DeleteCommand.Transaction = null;
				tcDataAdapter.InsertCommand.Transaction = null;
				tcDataAdapter.UpdateCommand.Transaction = null;
				Trace(EntryName + " - .Commit Complete", trcOption.trcDB | trcOption.trcDB);
				if (mActiveTXLevel == 0)
					throw new Exception("Commit operation requires an active transaction.");
				mActiveTXLevel -= 1;
			} catch (Exception ex) {
				try {
					AbortTrans();
				} catch {
				}
				throw;
			} finally {
				tcTransaction.Dispose();
				tcTransaction = null;
				mTransactionName = clsSupport.bpeNullString;
			}
		}
		#endregion
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public void ExecuteCommand(string SQLsource, ref int RecordsAffected)
		{
			SqlCommand cmd = new SqlCommand();
			try {
                cmd.CommandText = SQLsource;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = tcConnection;
                cmd.CommandTimeout = mCommandTimeout;
                cmd.Transaction = tcTransaction;
				RecordsAffected = cmd.ExecuteNonQuery();
				if (InconsistentViewDetected(SQLsource, RecordsAffected)) {
					//Trace("SQLCommand() - Inconsistent View Detected, raising error..." & vbCrLf & vbCrLf & "SQL Statement: " & vbCrLf & SQLsource, trcOption.trcDB)
					throw new Exception(string.Format("Inconsistent View detected. SQL: {0}", SQLsource));
				}
			} catch (Exception ex) {
				try {
					AbortTrans();
				} catch {
				}
				throw;
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public void ExecuteCommand(string SQLsource, clsCollection parms, ref int RecordsAffected, Boolean PreserveParms=false)
		{
			SqlCommand cmd = new SqlCommand();
			try
			{
				cmd.CommandText = SQLsource;
				cmd.CommandType = CommandType.Text;
				cmd.Connection = tcConnection;
				cmd.CommandTimeout = mCommandTimeout;
				cmd.Transaction = tcTransaction;
				cmd.Parameters.Clear();
				foreach (clsItem i in parms) {
					cmd.Parameters.Add(GetNewSqlParameter(i.Key, i.Value));
				}
				LogSQL(cmd);
				RecordsAffected = cmd.ExecuteNonQuery();
				if (InconsistentViewDetected(SQLsource, RecordsAffected))
				{
					//Trace("SQLCommand() - Inconsistent View Detected, raising error..." & vbCrLf & vbCrLf & "SQL Statement: " & vbCrLf & SQLsource, trcOption.trcDB)
					throw new Exception(string.Format("Inconsistent View detected. SQL: {0}", SQLsource));
				}
			} catch (Exception ex) {
				try {AbortTrans();}	catch{}
				throw;
			} finally {
				if (!PreserveParms) parms = null;
				cmd.Dispose(); cmd = null;
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public object ExecuteScalarCommand(string SQLsource)
		{
			object functionReturnValue = null;
			SqlCommand cmd = new SqlCommand();
			functionReturnValue = null;
			try {
                cmd.CommandText = SQLsource;
                cmd.CommandTimeout = mCommandTimeout;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = tcConnection;
				if ((tcTransaction != null) && object.ReferenceEquals(tcTransaction.Connection, cmd.Connection))
                    cmd.Transaction = tcTransaction;
				functionReturnValue = cmd.ExecuteScalar();
			} catch (Exception ex) {
				try {
					AbortTrans();
				} catch {
				}
				throw;
			} finally {
				cmd.Dispose();
				cmd = null;
			}
			return functionReturnValue;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public object ExecuteScalarCommand(string SQLsource, clsCollection parms, Boolean PreserveParms = false)
		{
			object functionReturnValue = null;
			SqlCommand cmd = new SqlCommand();
			try
			{
				cmd.CommandText = SQLsource;
				cmd.CommandTimeout = mCommandTimeout;
				cmd.CommandType = CommandType.Text;
				cmd.Connection = tcConnection;
				if ((tcTransaction != null) && object.ReferenceEquals(tcTransaction.Connection, cmd.Connection)) cmd.Transaction = tcTransaction;
				cmd.Parameters.Clear();
				foreach (clsItem i in parms) {
					cmd.Parameters.Add(GetNewSqlParameter(i.Key, i.Value));
				}
				LogSQL(cmd);
				functionReturnValue = cmd.ExecuteScalar();
			} catch (Exception ex) {
				try {AbortTrans();} catch {	}
				throw;
			} finally {
				if (!PreserveParms) parms = null;
				cmd.Dispose(); cmd = null;
			}
			return functionReturnValue;
		}
		public int FindRowByID(int ID)
		{
			int functionReturnValue = 0;
			functionReturnValue = -1;
			switch (ID) {
				case -1:
					MoveFirst();
					functionReturnValue = 0;
					//EOF...
					break;
				case -2:
					MoveLast();
					functionReturnValue = tcDataView.Count - 1;
					//EOF...
					break;
				case -3:
					MoveFirst();
					functionReturnValue = 0;
					//Both BOF end EOF...
					break;
				default:
					if (tcDataView.Count == 0 && mSQLfilter != clsSupport.bpeNullString)
						OnFilterCanceled(true);
					if (tcDataView.Count == 0) {
						//Now what... Table must be empty, so what do we do...?
						//TODO: Troubleshoot Empty Filtered DataView Binding issue...
						throw new IndexOutOfRangeException(string.Format("{0} table is empty.", mTableIDColumn));
					}

					if (StripBraces(tcDataView.Sort, "[") == StripBraces(mTableIDColumn, "[")) {
						functionReturnValue = tcDataView.Find(ID);
						if (functionReturnValue != -1)
							return functionReturnValue;
					} else {
						for (int i = 0; i <= tcDataView.Count - 1; i++) {
							if ((int)tcDataView[i][mTableIDColumn] == ID) {
								functionReturnValue = i;
								return functionReturnValue;
							}
						}
					}
					//Didn't find it, so return the position of the next record (assuming cm.Position is that record)...
					if ((mCurrencyManager != null))
						functionReturnValue = mCurrencyManager.Position;
					break;
			}
			return functionReturnValue;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private int GetID(string TableName)
		{
			int functionReturnValue = 0;
			SqlCommand cmd = null;
			try {
				cmd = new SqlCommand(string.Format("Select Max({0})+1 From [{1}]", mTableIDColumn, TableName), tcConnection);
				functionReturnValue = (int)cmd.ExecuteScalar();
				if (functionReturnValue == null)
					functionReturnValue = 1;
			} finally {
				cmd.Dispose();
				cmd = null;
			}
			return functionReturnValue;
		}
		protected SqlParameter GetNewSqlParameter(string paramName, DataColumn iColumn, DataRow dataRow)
		{
			SqlParameter param = new SqlParameter(paramName, MapSystemToSQLDBType(iColumn.DataType));
			if (iColumn.MaxLength > 0)
				param.Size = iColumn.MaxLength;
			param.SourceColumn = iColumn.ColumnName;
			param.Value = dataRow[iColumn.ColumnName, DataRowVersion.Default];
			//Select Case dataRow.RowState
			//    Case DataRowState.Detached
			//        param.Value = dataRow(iColumn.ColumnName, DataRowVersion.Proposed)
			//    Case Else
			//        param.Value = dataRow(iColumn.ColumnName, DataRowVersion.Current)
			//End Select
			return param;
		}
		protected SqlParameter GetNewSqlParameter(string paramName, DataColumn iColumn, object Value)
		{
			SqlParameter param = new SqlParameter(paramName, MapSystemToSQLDBType(iColumn.DataType));
			if (iColumn.MaxLength > 0)
				param.Size = iColumn.MaxLength;
			param.SourceColumn = iColumn.ColumnName;
			param.Value = Value;
			//Select Case dataRow.RowState
			//    Case DataRowState.Detached
			//        param.Value = dataRow(iColumn.ColumnName, DataRowVersion.Proposed)
			//    Case Else
			//        param.Value = dataRow(iColumn.ColumnName, DataRowVersion.Current)
			//End Select
			return param;
		}
		protected SqlParameter GetNewSqlParameter(string Name, object Value)
		{
			SqlParameter parm = new SqlParameter(String.Format("@{0}", Name), Value.GetType());
			parm.SourceColumn = Name;
			parm.Value = Value;
			return parm;
		}
		public static Icon ImageToIcon(Image SourceImage)
		{
			Icon functionReturnValue = null;
			// converts an image into an icon
			Bitmap TempBitmap = new Bitmap(SourceImage);
			functionReturnValue = System.Drawing.Icon.FromHandle(TempBitmap.GetHicon());
			TempBitmap.Dispose();
			return functionReturnValue;
		}
		private bool InconsistentViewDetected(string SQLsource, int RecordsAffected)
		{
			bool functionReturnValue = false;
			int iPos = 0;
			//Gotta have a ROWID for this to work...
			functionReturnValue = false;
			if (RecordsAffected > 0)
				return functionReturnValue;
			iPos = Strings.InStr(Strings.UCase(SQLsource), " WHERE ");
			if (iPos > 0) {
				if (Strings.InStr(Strings.UCase(Strings.Mid(SQLsource, iPos)), "ROWID") > 0)
					functionReturnValue = true;
			}
			return functionReturnValue;
		}
		protected internal void initColumnDefaults(DataColumnCollection dtColumns)
		{
			foreach (DataColumn iColumn in dtColumns) {
				if (!iColumn.AllowDBNull && object.ReferenceEquals(iColumn.DefaultValue.GetType(), typeof(DBNull))) {
					switch (iColumn.DataType.Name) {
						case "Boolean":
							iColumn.DefaultValue = false;
							break;
						case "Byte":
							iColumn.DefaultValue = 0;
							break;
						case "DateTime":
							iColumn.DefaultValue = new System.DateTime(1963, 7, 31);
							break;
						case "Decimal":
							iColumn.DefaultValue = 0;
							break;
						case "Double":
							iColumn.DefaultValue = 0;
							break;
						case "Int64":
							iColumn.DefaultValue = 0;
							break;
						case "Int32":
							iColumn.DefaultValue = 0;
							break;
						case "Int16":
							iColumn.DefaultValue = 0;
							break;
						case "Single":
							iColumn.DefaultValue = 0;
							break;
						case "String":
							iColumn.DefaultValue = clsSupport.bpeNullString;
							break;
						default:
							break;
						//Leave it alone 'cause I don't know what to do...
					}
				}
			}
		}
		public SqlDbType MapSystemToSQLDBType(System.Type Type)
		{
			switch (Type.Name) {
				case "Boolean":
					return SqlDbType.Bit;
				case "Byte":
					return SqlDbType.TinyInt;
				case "Byte[]":
					//or SqlDbType.Binary
					return SqlDbType.Image;
				case "DateTime":
					return SqlDbType.DateTime;
				case "Decimal":
					//or SqlDbType.Decimal, SqlDbType.SmallMoney
					return SqlDbType.Money;
				case "Double":
					return SqlDbType.Float;
				case "Int64":
					return SqlDbType.BigInt;
				case "Int32":
					return SqlDbType.Int;
				case "Int16":
					return SqlDbType.SmallInt;
				case "Single":
					return SqlDbType.Real;
				case "String":
					// or SqlDbType.VarChar, SqlDbType.NText, SqlDbType.Text
					return SqlDbType.NVarChar;
				default:
					return SqlDbType.VarChar;
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private void ModifyData()
		{
			DataTable dt = tcDataView.Table;
			DataSet ds = dt.DataSet;
			string SQL = clsSupport.bpeNullString;
			int saveID = 0;
			string saveRowFilter = clsSupport.bpeNullString;
			bool CanceledRowFilter = false;
			SqlCommand UpdateCommand = null;

            try
            {
                Trace(trcType.trcEnter, "ModifyData", trcOption.trcApplication);
                BeginTrans();
                saveID = (int)mDRV[mTableIDColumn];
                if (mDRV.IsEdit)
                {
                    //EndCurrentEdit, on the last tcDataView.RowFilter qualifying row, changing data such that the record no longer qualified sometimes 
                    //causes an ArgumentException deep in the bowels of the CurrencyManager code. So to avoid this, check to see if we are potentially 
                    //in this scenario and preemptively loosen our RowFilter...
                    if (FindRowByID(saveID) == tcDataView.Count - 1 && tcDataView.RowFilter != clsSupport.bpeNullString)
                    {
                        saveRowFilter = tcDataView.RowFilter;
                        tcDataView.RowFilter = clsSupport.bpeNullString;
                        iRow = FindRowByID(saveID);
                        mCurrencyManager.Position = iRow;
                        CanceledRowFilter = true;
                    }
                    //Documentation says mCurrenctManager.Begin/Cancel/EndCurrentEdit should only be used when building your own complex control (like a DataGrid)...
                    Trace("mCurrencyManager.EndCurrentEdit()", trcOption.trcApplication);
                    mCurrencyManager.EndCurrentEdit();
                    //...however, in practice not using the mCurrencyManager.EndCurrentEdit() caused all the existing [working] logic to fall apart - so it stays!
                    //Trace("mDRV.Row.EndEdit()", trcOption.trcDB)
                    //mDRV.Row.EndEdit()
                }
                //First build our SQL Update Statement based on what has changed...
                foreach (DataColumn iColumn in mDRV.Row.Table.Columns)
                {
                    if (AnythingHasChanged(mDRV.Row[iColumn.ColumnName, DataRowVersion.Original], mDRV.Row[iColumn.ColumnName, DataRowVersion.Default]))
                    {
                        SQL += string.Format("[{0}]=@{1},", iColumn.ColumnName, iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString));
                        SaveHistory(mDRV.Row, iColumn.ColumnName);
                    }
                }
                //Nothing to do (from the main table anyway)...
                if (SQL != clsSupport.bpeNullString)
                {
                    SQL = SQL.Substring(0, SQL.Length - 1);
                    //trailing comma
                    SQL = string.Format("Update [{0}] Set {1} Where [{2}]=@{2}", this.TableName, SQL, mTableIDColumn);
                    UpdateCommand = new SqlCommand(SQL, tcConnection, tcTransaction);
                    //Now for each column we intend to update, create and initialize its parameter...
                    UpdateCommand.Parameters.Clear();
                    foreach (DataColumn iColumn in mDRV.Row.Table.Columns)
                    {
                        if (iColumn.ColumnName == mTableIDColumn || AnythingHasChanged(mDRV.Row[iColumn.ColumnName, DataRowVersion.Original], mDRV.Row[iColumn.ColumnName, DataRowVersion.Default]))
                        {
                            UpdateCommand.Parameters.Add(new SqlParameter("@" + iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString), mDRV.Row[iColumn.ColumnName, DataRowVersion.Default]));
                        }
                    }
                    UpdateCommand.ExecuteNonQuery();
                    //Use the UpdateCommand to update the database...
                    LogSQL(UpdateCommand);
                }
                OnUpdateInProcess(mDRV);
                EndTrans();
                OnUpdateComplete(mDRV);
                //Mark the in-memory incarnation of the data we just wrote to disk as being modified (note that this rolls all the Original DataRow version objects into Current and clears Pending)
                Trace("tcDataSet.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
                tcDataSet.AcceptChanges();
                //Check to see if we preemptively cleared our RowFilter and reestablish if appropriate...
                if (saveRowFilter != clsSupport.bpeNullString && tcDataView.RowFilter == clsSupport.bpeNullString)
                {
                    DataView dvTemp = null;
                    try
                    {
                        dvTemp = new DataView(tcDataView.Table, saveRowFilter, tcDataView.Sort, tcDataView.RowStateFilter);
                        if (dvTemp.Count > 0)
                        {
                            tcDataView.RowFilter = saveRowFilter;
                            throw new ExitTryException();
                        }
                        OnFilterCanceled(true);
                    }
                    finally
                    {
                        dvTemp.Dispose();
                        dvTemp = null;
                    }
                }
                OnActionModeChange(ActionModeEnum.modeDisplay);
                Move(FindRowByID(saveID));
            } catch (ExitTryException ex) { 
			} catch (InvalidOperationException ex) {
				Dump(mDRV);
				DataView dv = null;
				try {
					dv = new DataView(mDRV.DataView.Table, "", "", DataViewRowState.CurrentRows);
					dv.RowStateFilter = DataViewRowState.Added;
					if (dv.Count > 0){Debug.WriteLine("Added:");Dump(dv);}
					dv.RowStateFilter = DataViewRowState.Deleted;
					if (dv.Count > 0){Debug.WriteLine("Deleted:");Dump(dv);}
					dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
					if (dv.Count > 0){Debug.WriteLine("ModifiedCurrent:");Dump(dv);}
				} catch {
				} finally {
					dv.Dispose();
					dv = null;
				}
				mCurrencyManager.Refresh();
				throw;
			} finally {
				if (UpdateCommand != null){UpdateCommand.Dispose();UpdateCommand = null;}
				try {
					AbortTrans();
				} catch {
				}
				Trace(trcType.trcExit, "ModifyData", trcOption.trcApplication);
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		protected void ModifyData(DataView dvTarget, DataRowView drvSource, string IDColumn)
		{
			SqlDataAdapter da = tcDataAdapter;
			DataTable dt = dvTarget.Table;
			string SQL = clsSupport.bpeNullString;

			//First build our SQL Update Statement based on what has changed...
			foreach (DataColumn iColumn in dt.Columns) {
				if (AnythingHasChanged(drvSource.Row[iColumn.ColumnName, DataRowVersion.Original], drvSource[iColumn.ColumnName])) {
					SQL += string.Format("[{0}]=@{1},", iColumn.ColumnName, iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString));
				}
			}
			SqlCommand UpdateCommand = null;

			//Nothing to do (from the main table anyway)...
			if (SQL != clsSupport.bpeNullString) {
				SQL = SQL.Substring(0, SQL.Length - 1);
				//trailing comma
				SQL = string.Format("Update [{0}] Set {1} Where [ID]=@{2}", dt.TableName, SQL, IDColumn.Replace(" ", clsSupport.bpeNullString));
				UpdateCommand = new SqlCommand(SQL, tcConnection, tcTransaction);
                //Now for each column we intend to update, create and initialize its parameter...
                UpdateCommand.Parameters.Clear();
				foreach (DataColumn iColumn in dt.Columns) {
					if (iColumn.ColumnName == "ID") {
                        UpdateCommand.Parameters.Add(new SqlParameter("@" + IDColumn.Replace(" ", clsSupport.bpeNullString), drvSource[IDColumn]));
					} else if (AnythingHasChanged(drvSource.Row[iColumn.ColumnName, DataRowVersion.Original], drvSource[iColumn.ColumnName])) {
                        UpdateCommand.Parameters.Add(new SqlParameter("@" + iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString), drvSource[iColumn.ColumnName]));
					}
				}
				//Use the UpdateCommand to update the database...
				LogSQL(UpdateCommand);
                UpdateCommand.ExecuteNonQuery();
			}
		}
        public void Move(int RowIndex,  object objChangeType = null)
		{
			const string EntryName = "Move";
            try {
				Trace(trcType.trcEnter, EntryName, trcOption.trcDB);
				//The position of the binding context controls the "current record" Position the first record is record 0 (not 1).
				if ((objChangeType == null) && mMode != ActionModeEnum.modeDisplay)
					throw new Exception("Someone's moving our row while not in DisplayMode!");

				Trace("{0}: Moving from Row #{1} to Row #{2}", EntryName, mCurrencyManager.Position, RowIndex, trcOption.trcApplication);
				Trace("{0}: Raising BeforeMove event", EntryName, trcOption.trcDB);
				//If mDRV IsNot Nothing AndAlso mDRV.IsEdit Then mDRV.CancelEdit()    'TODO: Don't know why in Display mode our mDRV.IsEdit is true, but deal with it here...
				OnBeforeMove(mCurrencyManager.Position, RowIndex, mDRV, tcDataView.Count);
				if (RowIndex == 0 && tcDataView.Count == 0) {
					Trace("{0}: Given Row #{1} matches total number of rows (empty), so we're bugging out", EntryName, RowIndex, trcOption.trcApplication);
					return;
				} else if (RowIndex > (tcDataView.Count - 1)) {
					Trace("{0}: Given Row #{1} exceeds the total number of rows, adjusting to last row", EntryName, RowIndex, trcOption.trcApplication);
					RowIndex = (tcDataView.Count - 1);
				} else if (RowIndex < 0) {
					//TODO: What causes RowIndex (-2) during Move(Integer,ListChangeType)...?
					Trace("{0}: Given Row #{1} is invalid, adjusting to first row", EntryName, RowIndex, trcOption.trcApplication);
					RowIndex = 0;
				}
				LogMessage(string.Format("Move: Moving from Row #{0} to #{1} (RecordID #{2})", mCurrencyManager.Position, RowIndex, tcDataView[RowIndex][mTableIDColumn]), 0);
				Trace("{0}: Saving Row #{1} as mDRV (RecordID #{2})", EntryName, RowIndex.ToString(), tcDataView[RowIndex][mTableIDColumn], trcOption.trcApplication);
				mDRV = tcDataView[RowIndex];
				iRow = RowIndex;
				TraceDataView(true);
				Trace("{0}: Raising AfterMove event", EntryName, trcOption.trcDB);
				OnAfterMove(mCurrencyManager.Position, RowIndex, mDRV, tcDataView.Count);
				TraceDataView(true);
			} finally {
				Trace(trcType.trcExit, EntryName, trcOption.trcDB);
			}
		}
		public void MoveFirst()
		{
			//The position of the binding context controls the "current record" Position the first record is record 0 (not 1).
			Move(0);
		}
		public void MoveLast()
		{
			//The position of the binding context controls the "current record". Use dv.Count to figure out the total 
			//number of records.  -1 because position is zero based.
			Move(tcDataView.Count - 1);
		}
		public void MoveNext()
		{
			//The position of the binding context controls the "current record"
			Move(mCurrencyManager.Position + 1);
		}
		public void MovePrevious()
		{
			//The position of the binding context controls the "current record"
			Move(mCurrencyManager.Position - 1);
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private void NewData()
		{
			SqlDataAdapter da = tcDataAdapter;
			DataTable dt = tcDataView.Table;
			DataSet ds = dt.DataSet;
			string Columns = clsSupport.bpeNullString;
			string Values = clsSupport.bpeNullString;
			string SQL = clsSupport.bpeNullString;
			Control ctlNotes = null;
			int addID = 0;

			try {
				Trace(trcType.trcEnter, "NewData", trcOption.trcAll);
				BeginTrans();
				switch (mDRV.Row.RowState) {
					case DataRowState.Added:
					case DataRowState.Detached:
						addID = (int)mDRV[mTableIDColumn];
						break;
					default:
						throw new Exception("We're in NewData() but our RowState isn't " + DataRowState.Added.ToString() + " or " + DataRowState.Detached.ToString());
				}
				if (mDRV.IsEdit)
					mDRV.EndEdit();
				//If mDRV.IsEdit() Then
				//    'Documentation says mCurrenctManager.Begin/Cancel/EndCurrentEdit should only be used when building your own complex control (like a DataGrid)...
				//    Trace("{0}; mDRV:={1}; RecordID:={2}", "mCurrencyManager.EndCurrentEdit()", FindRowByID(addID), addID, trcOption.trcApplication)
				//    mCurrencyManager.EndCurrentEdit()
				//    '...however, in practice not using the mCurrencyManager.EndCurrentEdit() caused all the existing [working] logic to fall apart - so it stays!
				//    'Trace("mDRV.Row.EndEdit()", trcOption.trcDB)
				//    'mDRV.Row.EndEdit()
				//End If

				if (mDRV.Row.RowState != DataRowState.Added && mDRV.Row.RowState != DataRowState.Detached) {
					//EndEdit will move the row around in the DataView, so find it again before proceeding... 
					int tmpRow = FindRowByID(addID);
					//So we can use it to throw an exception without calling the routine again...
					mDRV = tcDataView[tmpRow];
					if (mDRV.Row.RowState != DataRowState.Added)
						throw new Exception(string.Format("FindRowByID({0}) returned Row #{1}, but its RowState isn't {2}.", addID, tmpRow, DataRowState.Added.ToString()));
				}

				//First build our SQL Insert Statement based screen data...
				foreach (DataColumn iColumn in mDRV.Row.Table.Columns) {
					Columns += string.Format("[{0}],", iColumn.ColumnName);
					Values += string.Format("@{0},", iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString));
					SaveHistory(mDRV.Row, iColumn.ColumnName);
				}
				Columns = Columns.Substring(0, Columns.Length - 1);
				//trailing comma
				Values = Values.Substring(0, Values.Length - 1);
				SQL = string.Format("Insert Into [{0}] ({1}) Values ({2})", this.TableName, Columns, Values);
				var _with11 = da.InsertCommand;
				_with11.CommandText = SQL;
				_with11.CommandType = CommandType.Text;
				//Now for each column we intend to update, create and initialize its parameter...
				_with11.Parameters.Clear();
				foreach (DataColumn iColumn in mDRV.Row.Table.Columns) {
					//If IsDBNull(mDRV(icolumn.ColumnName)) And icolumn.DataType Is GetType(Boolean) Then mDRV(icolumn.ColumnName) = False
					_with11.Parameters.Add(GetNewSqlParameter("@" + iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString), iColumn, mDRV.Row));
				}
				_with11.UpdatedRowSource = UpdateRowSource.None;
				//Use the DataAdapter (through the InsertCommand we just set up) to update the database...
				TraceDataView(true);
				Trace("{0}; mDRV:={1}; RecordID:={2}", "da.Update(dataTable)", FindRowByID(addID), addID, trcOption.trcApplication);
				LogSQL(da.InsertCommand);
				da.Update(dt);
				OnAddInProcess(mDRV);
				EndTrans();
				OnAddComplete(mDRV);
				//Mark the in-memory incarnation of the data we just wrote to disk as being modified (note that this rolls all the Original DataRow version objects into Current and clears Pending)
				Trace("{0}; mDRV:={1}; RecordID:={2}", "tcDataSet.AcceptChanges()", FindRowByID(addID), addID, trcOption.trcApplication);
				tcDataSet.AcceptChanges();
				tcDataView.RowFilter = mSaveFilter;
				mSaveFilter = clsSupport.bpeNullString;
				Move(FindRowByID(addID));
				//TODO: What if we don't find it?
				OnActionModeChange(ActionModeEnum.modeDisplay);
			} catch (InvalidOperationException ex) {
				Dump(mDRV);
				DataView dv = null;
				try {
					dv = new DataView(mDRV.DataView.Table, "", "", DataViewRowState.CurrentRows);
					dv.RowStateFilter = DataViewRowState.Added;
					if (dv.Count > 0){Debug.WriteLine("Added:");Dump(dv);}
					dv.RowStateFilter = DataViewRowState.Deleted;
					if (dv.Count > 0){Debug.WriteLine("Deleted:");Dump(dv);}
					dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
					if (dv.Count > 0){Debug.WriteLine("ModifiedCurrent:");Dump(dv);}
				} catch {
				} finally {
					dv = null;
				}
				Trace("{0}; mDRV:={1}; RecordID:={2}", "mCurrencyManager.Refresh()", FindRowByID(addID), addID, trcOption.trcApplication);
				mCurrencyManager.Refresh();
				throw;
			} catch (Exception ex) {
				mCurrencyManager.Refresh();
				throw;
			} finally {
				try {
					AbortTrans();
				} catch {
				}
				if (mSaveRecordID != 0)
					RemoveImages(FindRowByID(mSaveRecordID));
				if (mActiveTXLevel > 0)
					AbortTrans();
				Trace(trcType.trcExit, "NewData", trcOption.trcAll);
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		protected void NewData(string TableName, DataRowView drvSource, ref int NewID)
		{
			SqlDataAdapter da = tcDataAdapter;
			DataView dv = OpenDataView(string.Format("Select * From [{0}] Where 1=0;", TableName));
			DataTable dt = dv.Table;
			string Columns = clsSupport.bpeNullString;
			string Values = clsSupport.bpeNullString;
			NewID = (int)ExecuteScalarCommand(string.Format("Select MAX(ID)+1 From [{0}];", dt.TableName));
			foreach (DataColumn iColumn in dt.Columns) {
				Columns += string.Format("[{0}],", iColumn.ColumnName);
				Values += string.Format("@{0},", iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString));
			}
			Columns = Columns.Substring(0, Columns.Length - 1);
			//trailing comma
			Values = Values.Substring(0, Values.Length - 1);
			string SQLSource = string.Format("Insert Into [{0}] ({1}) Values ({2})", dt.TableName, Columns, Values);
            da.InsertCommand.CommandText = SQLSource;
            da.InsertCommand.CommandType = CommandType.Text;
            //Now for each column we intend to update, create and initialize its parameter...
            da.InsertCommand.Parameters.Clear();
			foreach (DataColumn iColumn in dt.Columns) {
				//If IsDBNull(mDRV(icolumn.ColumnName)) And icolumn.DataType Is GetType(Boolean) Then mDRV(icolumn.ColumnName) = False
				string paramName = "@" + iColumn.ColumnName.Replace(" ", clsSupport.bpeNullString);
				SqlParameter param = new SqlParameter(paramName, MapSystemToSQLDBType(iColumn.DataType));
				object Value = DBNull.Value;
				if (drvSource.Row.Table.Columns.Contains(iColumn.ColumnName))
					Value = drvSource[iColumn.ColumnName];
				if (iColumn.MaxLength > 0)
					param.Size = iColumn.MaxLength;
				param.SourceColumn = iColumn.ColumnName;
				param.Value = (paramName == "@ID" ? NewID : Value);
                da.InsertCommand.Parameters.Add(param);
			}
            da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
			//Use the DataAdapter (through the InsertCommand we just set up) to update the database...
			TraceDataView(true);
			LogSQL(da.InsertCommand);
            da.InsertCommand.ExecuteNonQuery();
		}
		public void CloseDataSet(ref DataSet DS)
		{
			if ((DS != null)){DS.Clear();DS.Dispose();}
			DS = null;
			GC.Collect();
		}
		public void CloseDataView(ref DataView DV)
		{
			if ((DV != null))
				DV.Dispose();
			DV = null;
			GC.Collect();
		}
		private bool CommentDetected(string SQLsource)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			//We really only care about Update or Delete statements because they will potentially destroy data if we misinterpret something like: 
			//   Update TABLE Set VALUE1--VALUE2 Where KEYVALUE=OTHER_VALUE;
			//Or
			//   Delete From TABLE Where VALUE1--VALUE2>SOME_OTHER_VALUE;
			//instead of...
			//   Update TABLE Set VALUE1-(-VALUE2) Where KEYVALUE=OTHER_VALUE;
			//Or
			//   Delete From TABLE Where VALUE1-(-VALUE2)>SOME_OTHER_VALUE;
			SQLsource = SQLsource.ToUpper().Trim();
			switch (ParseStr(SQLsource, 1, " ")) {
				case "INSERT":
					return functionReturnValue;
				case "SELECT":
					return functionReturnValue;
				case "UPDATE":
				case "DELETE":
					//Strip-off string any constants that may contain "-" characters so we don't misinterpret them as comments.
					//Note: We must assume properly structured SQL at this point (i.e. single quotes will match)...
					int openQuote = SQLsource.IndexOf("'");
					while (openQuote >= 0) {
						int closeQuote = SQLsource.IndexOf("'", openQuote + 1);
						if (closeQuote > 0)
							SQLsource = SQLsource.Substring(0, openQuote) + SQLsource.Substring(closeQuote + 1);
						openQuote = SQLsource.IndexOf("'");
					}
					//If the surviving SQLSource contains "--" then it would potentially do us harm, so flag it as a comment...
					if (SQLsource.IndexOf("--") > 0)
						functionReturnValue = true;
					break;
			}
			return functionReturnValue;
		}
		private object GetDataRowValue(DataRow dataRow, string dataMember, DataRowVersion rowVersion)
		{
			object functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if (Information.IsDBNull(dataRow[dataMember, rowVersion])){functionReturnValue = "Null";return functionReturnValue;}
			switch (MapSystemToSQLDBType(dataRow.Table.Columns[dataMember].DataType)) {
				case SqlDbType.Bit:
					functionReturnValue = string.Format("{0:True/False}", dataRow[dataMember, rowVersion].ToString());
					break;
				case SqlDbType.TinyInt:
					functionReturnValue = dataRow[dataMember, rowVersion];
					break;
				case SqlDbType.Binary:
				case SqlDbType.Image:
					functionReturnValue = "<Binary/Image>";
					break;
				case SqlDbType.DateTime:
					functionReturnValue = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dataRow[dataMember, rowVersion]);
					break;
				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					functionReturnValue = dataRow[dataMember, rowVersion];
					break;
				case SqlDbType.Float:
					functionReturnValue = dataRow[dataMember, rowVersion];
					break;
				case SqlDbType.BigInt:
				case SqlDbType.Int:
				case SqlDbType.SmallInt:
					functionReturnValue = dataRow[dataMember, rowVersion];
					break;
				case SqlDbType.Real:
					functionReturnValue = dataRow[dataMember, rowVersion];
					break;
				case SqlDbType.NVarChar:
				case SqlDbType.VarChar:
				case SqlDbType.NText:
				case SqlDbType.Text:
					functionReturnValue = dataRow[dataMember, rowVersion];
					//History.OriginalValue is defined as nvarchar(maxHistoryValue), so truncate if necessary...
					if (functionReturnValue.ToString().Length > maxHistoryValue)
						functionReturnValue = Strings.Left(functionReturnValue.ToString(), maxHistoryValue);
					break;
				default:
					functionReturnValue = dataRow[dataMember, rowVersion].ToString();
					break;
			}
			return functionReturnValue;
		}
		public void GetDeferredImages(int RowIndex)
		{
			DataView dv = null;
            try
            {
                if (RowIndex > tcDataView.Count - 1)
                    return;
                //We're in the process of adding a new row, leave for now, we'll be back...
                if (mMode == ActionModeEnum.modeAdd)
                    return;
                //We're adding a new record, so we don't need an image (yet)...
                if (mMode == ActionModeEnum.modeCopy)
                    return;
                //We're copying an existing record, so we've already got an image from our original guy, besides it will confuse the update logic...
                Trace("GetDeferredImages({0})", RowIndex, trcOption.trcApplication);
                DataRowView drv = tcDataView[RowIndex];
                //TODO: Expand using INFORMATION_SCHEMA to detect columns of data type image (but obviously not here on a per-record-visit basis)...
                string Columns = "";
                if (tcDataView.Table.Columns.Contains("Image"))
                    Columns += ",Image";
                if (tcDataView.Table.Columns.Contains("OtherImage"))
                    Columns += ",OtherImage";
                if (string.IsNullOrEmpty(Columns))
                    throw new ExitTryException();

                Columns = Columns.Substring(1);
                string SQL = string.Format("Select {0} From [{1}] Where ID={2};", Columns, mTableName, drv[mTableIDColumn]);
                dv = OpenDataView(SQL);
                if (dv.Count == 0)
                    throw new ExitTryException();
                                                //When adding new records, we would not find this guy and we cannot count on Mode as it may not yet be set...

                DataRowView drvImages = dv[0];
                for (int iToken = 1; iToken <= TokenCount(Columns, ","); iToken++)
                {
                    string Column = ParseStr(Columns, iToken, ",");
                    if (!Information.IsDBNull(drvImages[Column]))
                        drv[Column] = drvImages[Column];
                }
                Trace("GetDeferredImages: drv.EndEdit()", trcOption.trcDB | trcOption.trcApplication);
                drv.EndEdit();
                switch (mMode)
                {
                    case ActionModeEnum.modeDisplay:
                        //Accepting changes when in a Add/Modify mode will screw-up our update logic...
                        Trace("GetDeferredImages: tcDataView.Table.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
                        TraceDataView(true);
                        tcDataView.Table.AcceptChanges();
                        TraceDataView(true);
                        break;
                }
                drv = null;
                drvImages = null;
            } catch (ExitTryException ex) { 
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				if (dv != null){CloseDataView(ref dv);dv = null;}
			}
		}
		private string[] GetTableNames(string SQLsource, bool MainTableOnly, bool fAlias = true)
		{
			string[] functionReturnValue = null;
			string[] Tables = { };
			string FieldList = clsSupport.bpeNullString;
			string FromClause = clsSupport.bpeNullString;
			string WhereClause = clsSupport.bpeNullString;
			string OrderByClause = clsSupport.bpeNullString;
			bool Distinct = false;
			int RowLimit = 0;

			functionReturnValue = Tables;
			try {
				if (CommentDetected(SQLsource))
					throw new ArgumentException(string.Format("SQL comments are not supported. Detected comment in SQLSource: {0}{1}", Constants.vbCrLf, SQLsource));

				Tables = new string[TokenCount(SQLsource, ";", "'")];
				for (short iSQL = 0; iSQL <= Tables.GetUpperBound(0); iSQL++) {
					string Token = ParseStr(SQLsource, iSQL + 1, ";", "'").Trim();
					ParseSQLSelect(Token, ref FieldList, ref FromClause, ref WhereClause, ref OrderByClause, ref Distinct, ref RowLimit);
					//Before getting started, double check for ANSI SQL-92 Join syntax (i.e. "LEFT OUTER JOIN" stuff)...
					if (FromClause.ToUpper().IndexOf(" JOIN ") != -1) {
						//Debug.Assert "Found a ANSI SQL-92 Join Syntax..."
						FromClause = this.SQLReplace(FromClause, " LEFT OUTER JOIN ", ",");
						FromClause = this.SQLReplace(FromClause, " RIGHT OUTER JOIN ", ",");
						FromClause = this.SQLReplace(FromClause, " FULL OUTER JOIN ", ",");
						FromClause = this.SQLReplace(FromClause, " MERGE JOIN ", ",");
						FromClause = this.SQLReplace(FromClause, " INNER JOIN ", ",");
						//Now remove the "ON expression1 operator expression2" portion of the syntax...
						int iOnStart = FromClause.ToUpper().IndexOf(" ON ");
						while (iOnStart != -1) {
							//Need to find the end of the "ON" clause...
							int iOnEnd = FromClause.IndexOf(",", iOnStart);
							if (iOnEnd == -1) {
								FromClause = FromClause.Substring(0, iOnStart);
							} else {
								FromClause = FromClause.Substring(0, iOnStart) + FromClause.Substring(iOnEnd).Trim();
							}
							iOnStart = FromClause.ToUpper().IndexOf(" ON ");
						}
					}
					//Eliminate multiple spaces...
					while (FromClause.IndexOf("  ") != -1) {
						FromClause = FromClause.Replace("  ", " ");
					}
					//Remove any index hints that may be present...
					int iWithStart = FromClause.ToUpper().IndexOf(" WITH (");
                    bool continueLoop = false;
					if (iWithStart != -1) {
						//Skip forward through the end of this expression...
						short openParenCount = 1;
						int iWithEnd = iWithStart + " WITH (".Length;
                        continueLoop = true;
                        for (int iParen = iWithEnd; iParen <= FromClause.Length - 1 && continueLoop; iParen++) {
							switch (FromClause.Substring(iParen, 1)) {
								case "(":
									openParenCount += 1;
									break;
								case ")":
									openParenCount -= 1;
									break;
							}
							if (openParenCount == 0) {
                                iWithEnd = iParen + 1;
                                continueLoop = false;
                            }
                        }
						FromClause = FromClause.Substring(0, iWithStart) + FromClause.Substring(iWithEnd).Trim();
					}
					//Clean up additional character sequences that may trip us up...
					FromClause = FromClause.Replace("(", clsSupport.bpeNullString);
					FromClause = FromClause.Replace(")", clsSupport.bpeNullString);
					//Now FromClause should be a list of tables/aliases... (i.e. "Table1, Table2" or "Table1 T1, Table2 T2"...)
					//Grab the (Table/Alias) and build our Tables array accordingly...
					string temp = clsSupport.bpeNullString;
                    continueLoop = true;
                    for (int i = 1; i <= TokenCount(FromClause, ",") && continueLoop; i++) {
						string TableToken = ParseStr(FromClause, i, ",");
						if (TableToken.StartsWith("[") && TableToken.EndsWith("]")) {
							//Assume it needs to be enclosed in brackets - do not assume part of this is the alias...
							Tables[iSQL] = TableToken.Substring(1, TableToken.Length - 2);
						} else {
							temp = ParseStr(FromClause.Replace("[", "\"").Replace("]", "\""), i, ",").Trim();
							if (temp.IndexOf(" ") != -1)
								temp = ParseStr(temp, (fAlias ? 2 : 1), " ", "\"").Trim();
							Tables[iSQL] = temp + ",";
						}
						if (MainTableOnly)
                            continueLoop = false;
                    }
                    if (Tables[iSQL].EndsWith(","))
						Tables[iSQL] = Tables[iSQL].Substring(0, Tables[iSQL].Length - 1);
				}
				functionReturnValue = Tables;
			} catch (Exception ex) {
				throw;
			}
			return functionReturnValue;
		}
		protected DataSet OpenDataSet(DataSet ds, string SQLsource, bool SkipAsClause = false, bool NeedSchema = false)
		{
			DataSet functionReturnValue = null;
			const string EntryName = "OpenDataSet";
			short CountRetries = 0;
			short CountReconnectRetries = 0;
			bool fActiveTX = false;
			bool fConnectionLost = false;
			bool fScript = false;
			string localSQLsource = clsSupport.bpeNullString;
			int RecordCount = 0;
			string[] TableNames = new string[] { };
			int TableCount = 0;

			functionReturnValue = null;
			int myThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
			if (CommentDetected(SQLsource))
				throw new ArgumentException(string.Format("SQL comments are not supported. Detected comment in SQLSource: {0}{1}", Constants.vbCrLf, SQLsource));

			if (!Connected)
				throw new ApplicationException("Not Connected.");

			if (!SQLsource.ToUpper().Trim().StartsWith("SELECT "))
				fScript = true;
			if ((ds == null) || fScript)
				ds = new DataSet();
            bool continueLoop = true;
			while (continueLoop) {
				try {
					TryAgain:
					if (this.ActiveTXLevel > 0)
						fActiveTX = true;
					else
						fActiveTX = false;
					if (fScript || SkipAsClause) {
						localSQLsource = SQLsource;
					} else {
						for (short iSQL = 1; iSQL <= TokenCount(SQLsource, ";", "'"); iSQL++) {
							string Token = Strings.Trim(ParseStr(SQLsource, iSQL, ";", "'"));
							SQLApplyAsClause(ref Token);
							localSQLsource += Token + ";";
						}
					}

					//Remove any existing Table object(s) from the DataSet before proceeding...
					if (!fScript) {
						TableNames = GetTableNames(localSQLsource, true);
						TableCount = ds.Tables.Count;
						if (TableCount > 0) {
                            for (short iTableName = 0; iTableName <= TableNames.GetUpperBound(0); iTableName++) {
								foreach (DataTable iTable in ds.Tables) {
									if (iTable.TableName == TableNames[iTableName]) {
										ds.Tables.Remove(iTable);
										TableCount -= 1;
										//iTable.RejectChanges() : iTable.Clear() : iTable.Dispose() : iTable = Nothing
										break; 
									}
								}
							}
						}
					}
					Trace(EntryName + " - tcDataAdapter.SelectCommand.CommandText=\"" + SQLsource + "\"", trcOption.trcDB | trcOption.trcDB);
                    tcDataAdapter.SelectCommand.CommandText = localSQLsource;
                    tcDataAdapter.SelectCommand.CommandType = CommandType.Text;
                    tcDataAdapter.SelectCommand.Connection = tcConnection;
					if ((tcTransaction != null)) {
						if (object.ReferenceEquals(tcTransaction.Connection, tcDataAdapter.SelectCommand.Connection))
                            tcDataAdapter.SelectCommand.Transaction = tcTransaction;
					}
                    tcDataAdapter.SelectCommand.CommandTimeout = mCommandTimeout;
					Trace(EntryName + " - DataAdapter.SelectCommand.CommandText=\"" + SQLsource + "\"", trcOption.trcDB | trcOption.trcDB);
					if (NeedSchema)
                        tcDataAdapter.FillSchema(ds, SchemaType.Source);
					//FillSchema doesn't seem to want to use the default constraint from the database, so roll our own default here...
					initColumnDefaults(tcDataAdapter.FillSchema(ds, SchemaType.Source)[0].Columns);
					RecordCount = tcDataAdapter.Fill(ds);
					functionReturnValue = ds;
					Trace(string.Format("{0} - Complete; {1:#,##0} Records...", EntryName, RecordCount.ToString()), trcOption.trcDB | trcOption.trcDB);
					if (RecordCount > 1)
						OnSplash(string.Format("{0:#,##0} Records Read...", RecordCount));

					//Name the Table object(s) appropriately...
					if (!fScript && ds.Tables.Count > 0) {
						for (int iTable = TableCount; iTable <= ds.Tables.Count - 1; iTable++) {
							DataTable dt = ds.Tables[iTable];
							try {
								string TableName = TableNames[iTable - TableCount];
								if (!ds.Tables.Contains(TableName)) {
                                    dt.TableName = TableName;
								} else {
                                    dt.TableName = string.Format("TABLE{0}", iTable - TableCount);
								}
							} catch (System.Data.DuplicateNameException ex) {
                                //The batch must have another query using the same main Table as another - no problem, set the .TableName 
                                //to something unique and the caller will just have to use ordinal positioning rather than the string-key...
                                dt.TableName = string.Format("TABLE{0}", iTable - TableCount);
							} catch (Exception ex) {
								throw;
							}
                            dt.DefaultView.RowFilter = clsSupport.bpeNullString;
                            dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
							string OrderByClause = ParseStr(localSQLsource, iTable - TableCount + 1, ";");
							if (OrderByClause.ToUpper().IndexOf(" ORDER BY ") != -1) {
								OrderByClause = OrderByClause.Substring(OrderByClause.ToUpper().IndexOf(" ORDER BY ") + " ORDER BY ".Length);
                                dt.DefaultView.Sort = OrderByClause.Replace("\"", clsSupport.bpeNullString);
							}
						}
					}
                    continueLoop = false;
				} catch (Exception ex) {
					throw;
					//SaveLastError(ex) 'Preserve our current error in our LastError object...
					//LastError.Description = String.Format("Unexpected error encountered executing SQL statement{0}{0}{1}{0}SQL Error: {2} (#{3}){0}Underlying SQL: {0}{4}", New Object() {vbCrLf, LastError.Description, FormatDBError(LastError.Number), LastError.Number, localSQLsource})
					//mSupport.Errors.Item(mSupport.Errors.Count).Description = LastError.Description
					//Select Case LastError.Number
					//    Case dberrInconsistentView, dberrLockTimeOut, dberrDeadLock
					//        Dim TryAgain As Boolean = Me.HandleContentionError(EntryName, SQLsource, CountRetries)
					//        If Not TryAgain Then
					//            With LastError
					//                SaveLastError(.Number, .Source, .Description, .ReportedBy, .HelpFile, .HelpContext, .LastDLLError, Nothing)
					//            End With
					//            Throw LastError.Exception
					//        End If
					//    Case Else
					//        'dberrSessionTerminated, dberrSessionEndedAbnormally, dberrDBShutdown, DBEErrorCodesEnum.dberrDBEMysteryDisconnect, dberrDriverDoesNotSupportRequestedProperties
					//        Me.HandleDisconnect(Connection, EntryName, SQLsource, CountReconnectRetries)
					//End Select
				}
			}
			return functionReturnValue;
		}
		protected DataSet OpenDataSet(string SQLsource, bool SkipAsClause = false, bool NeedSchema = false)
		{
			return this.OpenDataSet(null, SQLsource, SkipAsClause, NeedSchema);
		}
		protected DataView OpenDataView(string SQLsource, bool SkipAsClause = false, bool NeedSchema = false)
		{
			DataView functionReturnValue = null;
			const string EntryName = "OpenDataView";
			DataSet ds = new DataSet();
			DataView dv = null;
			functionReturnValue = null;
			if (this.CommentDetected(SQLsource))
				throw new ArgumentException(string.Format("SQL comments are not supported. Detected comment in SQLSource: {0}{1}", Constants.vbCrLf, SQLsource));
			if (TokenCount(SQLsource, ";", "'") > 1)
				throw new ApplicationException("Multiple SQL Statements not supported.");

			ds = this.OpenDataSet(null, SQLsource, SkipAsClause, NeedSchema);
			dv = ds.Tables[0].DefaultView;
			Trace(string.Format("{0} - Complete; {1:#,##0} Records...", EntryName, dv.Count.ToString()), trcOption.trcDB | trcOption.trcDB);
			functionReturnValue = dv;
			return functionReturnValue;
		}
		private void ParseSQLSelect(string SQLSource, ref string FieldList, ref string FromClause, ref string WhereClause, ref string OrderByClause, ref bool Distinct, ref int RowLimit)
		{
			FieldList = clsSupport.bpeNullString;
			FromClause = clsSupport.bpeNullString;
			WhereClause = clsSupport.bpeNullString;
			string HavingClause = clsSupport.bpeNullString;
			string GroupByClause = clsSupport.bpeNullString;
			OrderByClause = clsSupport.bpeNullString;
			Distinct = false;
			RowLimit = 0;
			this.ParseSQLSelect(SQLSource, ref FieldList, ref FromClause, ref WhereClause, ref HavingClause, ref GroupByClause, ref OrderByClause, ref Distinct, ref RowLimit);
			//Tack-on HavingClause/GroupByClause data in WhereClause... If the calling application needs these separate, it can call the other method...
			if (HavingClause != clsSupport.bpeNullString)
				WhereClause += string.Format(" Having {0}", HavingClause);
			if (GroupByClause != clsSupport.bpeNullString)
				WhereClause += string.Format(" Group By {0}", GroupByClause);
		}
		private void ParseSQLSelect(string SQLSource, ref string FieldList, ref string FromClause, ref string WhereClause, ref string HavingClause, ref string GroupByClause, ref string OrderByClause, ref bool Distinct, ref int RowLimit)
		{
			FieldList = clsSupport.bpeNullString;
			FromClause = clsSupport.bpeNullString;
			WhereClause = clsSupport.bpeNullString;
			HavingClause = clsSupport.bpeNullString;
			GroupByClause = clsSupport.bpeNullString;
			OrderByClause = clsSupport.bpeNullString;
			Distinct = false;
			RowLimit = 0;
			string originalSQL = SQLSource;

			//Gotta have a SELECT and a FROM...
			SQLSource = SQLSource.Replace(Constants.vbCrLf, " ").Trim();
			SQLSource = SQLSource.Replace(Constants.vbCr, " ").Trim();
			SQLSource = SQLSource.Replace(Constants.vbLf, " ").Trim();
			SQLSource = SQLSource.Replace(Constants.vbTab, " ").Trim();
			if (SQLSource.EndsWith(";"))
				SQLSource = SQLSource.Substring(0, SQLSource.Length - 1);
			if (!SQLSource.ToUpper().StartsWith("SELECT"))
				return;

			//Here's a thought, we can use a DataReader to allow SQL Server to parse the columns from the query and return them in a DataTable
			//containing just the column meta-data. Unfortunately it doesn't return us any of the detail we really need to accomplish what we're 
			//trying to do here...
			//Dim idbCMD As IDbCommand = GetNewCommand()
			//With idbCMD
			//    .CommandText = SQLSource
			//    .CommandType = CommandType.Text
			//    .Connection = Connection
			//    Dim reader As IDataReader = .ExecuteReader(CommandBehavior.SchemaOnly)
			//    ADO.DumpDataTable(reader.GetSchemaTable())
			//End With

			if (SQLSource.ToUpper().StartsWith("SELECT DISTINCT")) {
				SQLSource = SQLSource.Substring("SELECT DISTINCT ".Length).Trim();
				Distinct = true;
			} else {
				SQLSource = SQLSource.Substring("SELECT ".Length).Trim();
			}
			if (SQLSource.ToUpper().StartsWith("TOP ")) {
				SQLSource = SQLSource.Substring("TOP ".Length).Trim();
				RowLimit = Convert.ToInt32(SQLSource.Substring(0, SQLSource.IndexOf(" ")));
				SQLSource = SQLSource.Substring(Convert.ToString(RowLimit).Length).Trim();
			}

			string Token = clsSupport.bpeNullString;
			//This will be our ColumnExpression...
			string ColumnAlias = clsSupport.bpeNullString;
            bool continueLoop = true;
			while (!SQLSource.ToUpper().StartsWith("FROM ") && continueLoop) {
				SQLParseColumn(ref SQLSource, ref Token, ref ColumnAlias);
				if (Token != clsSupport.bpeNullString)
					FieldList += string.Format("{0}{1}", (FieldList == clsSupport.bpeNullString ? clsSupport.bpeNullString : ","), Token);
				if (SQLSource == clsSupport.bpeNullString)
                    continueLoop = false;
			}
			if (SQLSource == clsSupport.bpeNullString)
				throw new NotSupportedException(string.Format("Problem parsing SQL (\"{0}\")", originalSQL));

			//Now our SQLSource should begin with "From ..."
			SQLSource = SQLSource.Substring("FROM ".Length).Trim();
			//Now we need to find our Where clause for the main SQL statement. Note that there may be embedded sub-queries in the From clause...
			FromClause = clsSupport.bpeNullString;
            continueLoop = true;
			for (int i = 0; i <= SQLSource.Length - 1 && continueLoop; i++) {
				switch (SQLSource.Substring(i, 1)) {
					case " ":
						//Since we've skipped forward through any paren-pairs, (assuming sub-queries need to be surrounded by parens) 
						//if we now encounter a " WHERE ", it should be the main SQL's Where clause...
						string temp = SQLSource.Substring(i + 1).Trim().ToUpper();
						if (temp.StartsWith("WHERE ") || temp.StartsWith("HAVING ") || temp.StartsWith("GROUP BY ") || temp.StartsWith("ORDER BY ")) {
                            FromClause = SQLSource.Substring(0, i);
                            SQLSource = SQLSource.Substring(i + 1);
                            continueLoop = false; 
                        }
						break;
					case "(":
						//Skip forward through the end of this expression...
						short openParenCount = 1;
						for (int iParen = i + 1; iParen <= SQLSource.Length - 1 && continueLoop; iParen++) {
							switch (SQLSource.Substring(iParen, 1)) {
								case "(":
									openParenCount += 1;
									break;
								case ")":
									openParenCount -= 1;
									break;
							}
							if (openParenCount == 0) {
                                i = iParen;
                                continueLoop = false; 
                            }
						}
						break;
				}
			}
			//If we leave the loop without finding anything else then the rest must be From clause...
			if (FromClause == clsSupport.bpeNullString){FromClause = SQLSource;return;
}
			SQLSource = SQLSource.Trim();

			//Now our SQLSource may begin with "Where ..."
			if (SQLSource.Trim().ToUpper().StartsWith("WHERE ")) {
				SQLSource = SQLSource.Substring("WHERE ".Length).Trim();
				//Now we need to find our Order By clause for the main SQL statement. Note that there may be embedded sub-queries in the Where clause...
				WhereClause = clsSupport.bpeNullString;
                continueLoop = true;
                for (int i = 0; i <= SQLSource.Length - 1 && continueLoop; i++) {
					switch (SQLSource.Substring(i, 1)) {
						case " ":
							//Since we've skipped forward through any paren-pairs, (assuming sub-queries need to be surrounded by parens) 
							//if we now encounter an " ORDER BY ", it should be the main SQL's Group/Order By clause...
							string temp = SQLSource.Substring(i + 1).Trim().ToUpper();
							if (temp.StartsWith("HAVING ") || temp.StartsWith("GROUP BY ") || temp.StartsWith("ORDER BY ")) {
                                WhereClause = SQLSource.Substring(0, i);
                                SQLSource = SQLSource.Substring(i + 1);
                                continueLoop = false; 
                            }
							break;
						case "(":
							//Skip forward through the end of this expression...
							short openParenCount = 1;
							for (int iParen = i + 1; iParen <= SQLSource.Length - 1 && continueLoop; iParen++) {
								switch (SQLSource.Substring(iParen, 1)) {
									case "(":
										openParenCount += 1;
										break;
									case ")":
										openParenCount -= 1;
										break;
								}
								if (openParenCount == 0) {
                                    i = iParen;
                                    continueLoop = false; 
                                }
							}
							break;
					}
				}
				//If we leave the loop without finding another grouping or order clause then the rest must be Where clause...
				if (WhereClause == clsSupport.bpeNullString){WhereClause = SQLSource;return;
}
				SQLSource = SQLSource.Trim();
			}

			//Now our SQLSource may begin with "Having ..."
			if (SQLSource.Trim().ToUpper().StartsWith("HAVING ")) {
				SQLSource = SQLSource.Substring("HAVING ".Length).Trim();
                //We're currently not returning a HavingClause, so spin through to see what's next...
                continueLoop = true;
                for (int i = 0; i <= SQLSource.Length - 1 && continueLoop; i++) {
					switch (SQLSource.Substring(i, 1)) {
						case " ":
							string temp = SQLSource.Substring(i + 1).Trim().ToUpper();
							if (temp.StartsWith("GROUP BY ") || temp.StartsWith("ORDER BY ")) {
                                HavingClause = SQLSource.Substring(0, i);
                                SQLSource = SQLSource.Substring(i + 1);
                                continueLoop = false; 
                            }
							break;
						case "(":
							//Skip forward through the end of this expression...
							short openParenCount = 1;
							for (int iParen = i + 1; iParen <= SQLSource.Length - 1 && continueLoop; iParen++) {
								switch (SQLSource.Substring(iParen, 1)) {
									case "(":
										openParenCount += 1;
										break;
									case ")":
										openParenCount -= 1;
										break;
								}
								if (openParenCount == 0) {
                                    i = iParen;
                                    continueLoop = false;
                                }
							}
							break;
					}
				}
				//If we leave the loop without finding another grouping or order clause then the rest must be Having clause...
				if (HavingClause == clsSupport.bpeNullString){HavingClause = SQLSource;return;
}
				SQLSource = SQLSource.Trim();
			}

			//Now our SQLSource may begin with "Group By ..."
			if (SQLSource.Trim().ToUpper().StartsWith("GROUP BY ")) {
				SQLSource = SQLSource.Substring("GROUP BY ".Length).Trim();
                //We're currently not returning a GroupByClause, so spin through to see what's next...
                continueLoop = true;
                for (int i = 0; i <= SQLSource.Length - 1 && continueLoop; i++) {
					switch (SQLSource.Substring(i, 1)) {
						case " ":
							//Since we've skipped forward through any paren-pairs, (assuming sub-queries need to be surrounded by parens) 
							//if we now encounter an " ORDER BY ", it should be the main SQL's Group/Order By clause...
							string temp = SQLSource.Substring(i + 1).Trim().ToUpper();
							if (temp.StartsWith("ORDER BY ")) {
                                GroupByClause = SQLSource.Substring(0, i);
                                SQLSource = SQLSource.Substring(i + 1);
                                continueLoop = false; 
                            }
							break;
						case "(":
							//Skip forward through the end of this expression...
							short openParenCount = 1;
							for (int iParen = i + 1; iParen <= SQLSource.Length - 1 && continueLoop; iParen++) {
								switch (SQLSource.Substring(iParen, 1)) {
									case "(":
										openParenCount += 1;
										break;
									case ")":
										openParenCount -= 1;
										break;
								}
								if (openParenCount == 0) {
                                    i = iParen;
                                    continueLoop = false; 
                                }
							}

							break;
					}
				}
				//If we leave the loop without finding another grouping or order clause then the rest must be Group By clause...
				if (GroupByClause == clsSupport.bpeNullString){GroupByClause = SQLSource;return;
}
				SQLSource = SQLSource.Trim();
			}

			//Now our SQLSource may begin with "Order By ..."
			if (SQLSource.Trim().ToUpper().StartsWith("ORDER BY ")) {
				//We expect the Order By clause to be last...
				OrderByClause = SQLSource.Substring("ORDER BY ".Length).Trim();
			}
		}
		private void RenameColumns(DataSet ds, string TableName, string TableAlias)
		{
			string TablePrefix = string.Format("{0}.", TableName);
			string AliasPrefix = clsSupport.bpeNullString;
			if (TableAlias != clsSupport.bpeNullString)
				AliasPrefix = string.Format("{0}.", TableAlias);
			foreach (DataTable iTable in ds.Tables) {
				if (iTable.TableName == TableName) {
					foreach (DataColumn iColumn in iTable.Columns) {
						if (iColumn.ColumnName.StartsWith(TablePrefix))
							iColumn.ColumnName = iColumn.ColumnName.Replace(TablePrefix, AliasPrefix);
					}
					DataView dv = ds.Tables[TableName].DefaultView;
					if ((dv != null)) {
						if (dv.RowFilter != clsSupport.bpeNullString)
							dv.RowFilter = dv.RowFilter.Replace(TablePrefix, AliasPrefix);
						if (dv.Sort != clsSupport.bpeNullString)
							dv.Sort = dv.Sort.Replace(TablePrefix, AliasPrefix);
					}
					break; 
				}
			}
		}
		public void SQLApplyAsClause(ref string SQLsource)
		{
			const string EntryName = "SQLApplyAsClause";
			const string dbparmOwner = "dbo";
			short i = 0;
			int iOnStart = 0;
			int iOnEnd = 0;
			string NewFieldList = clsSupport.bpeNullString;
			string FieldList = clsSupport.bpeNullString;
			string FromClause = clsSupport.bpeNullString;
			string SaveFromClause = clsSupport.bpeNullString;
			string WhereClause = clsSupport.bpeNullString;
			string HavingClause = clsSupport.bpeNullString;
			string GroupByClause = clsSupport.bpeNullString;
			string OrderByClause = clsSupport.bpeNullString;
			bool Distinct = false;
			int RowLimit = 0;
			string Token = clsSupport.bpeNullString;
			string ColumnAlias = clsSupport.bpeNullString;
			DataSet dsColumns = null;
			DataView dvColumns = null;
			string TableName = clsSupport.bpeNullString;
			string TBNAME = clsSupport.bpeNullString;
			string strOwner = clsSupport.bpeNullString;
			try {
				ParseSQLSelect(SQLsource, ref FieldList, ref FromClause, ref WhereClause, ref HavingClause, ref GroupByClause, ref OrderByClause, ref Distinct, ref RowLimit);
				SaveFromClause = FromClause;
				//FromClause will be chopped-up if ANSI SQL-92 Join syntax is found, so save it here to rebuild SQL statement later...

				while (FieldList.Length > 0) {
					this.SQLParseColumn(ref FieldList, ref Token, ref ColumnAlias);

					if (Token == "*") {
						NewFieldList += " " + Token;
					} else if (Strings.InStr(Token, ".*") > 0) {
						//Need to handle Table Aliases here...
						TableName = Strings.Left(Token, Strings.InStr(Token, ".*") - 1).Trim();
						TBNAME = TableName;

						//Before getting started, double check for ANSI SQL-92 Join syntax (i.e. "LEFT OUTER JOIN" stuff)...
						if (Strings.InStr(FromClause.ToUpper(), " JOIN ") > 0) {
							//Debug.Assert "Found a ANSI SQL-92 Join Syntax..."
							FromClause = FromClause.ToUpper().Replace(" LEFT OUTER JOIN ", ",");
							FromClause = FromClause.ToUpper().Replace(" RIGHT OUTER JOIN ", ",");
							FromClause = FromClause.ToUpper().Replace(" FULL OUTER JOIN ", ",");
							FromClause = FromClause.ToUpper().Replace(" MERGE JOIN ", ",");
							FromClause = FromClause.ToUpper().Replace(" INNER JOIN ", ",");
							//Now remove the "ON expression1 operator expression2" portion of the syntax...
							iOnStart = Strings.InStr(FromClause.ToUpper(), " ON ");
							while (iOnStart > 0) {
								//Need to find the end of the "ON" clause...
								iOnEnd = mSupport.Strings.InStrMX(1, FromClause, "<>=");
								//This should get us past the first expression...
								iOnEnd = mSupport.Strings.InStrMX(iOnEnd, FromClause, ",");
								//This should get us the value we want...
								if (iOnEnd == 0) {
									FromClause = FromClause.Substring(0, iOnStart - 1);
								} else {
									FromClause = string.Format("{0},{1}", FromClause.Substring(0, iOnStart - 1), FromClause.Substring(iOnEnd).Trim());
								}
								iOnStart = Strings.InStr(FromClause.ToUpper(), " ON ");
							}
						}

                        //OK, Now FromClause should be in the old format we like to work with... (i.e. "Table1, Table2" or "Table1 T1, Table2 T2"...)
                        bool continueLoop = true;
                        for (i = 1; i <= TokenCount(FromClause, ",") && continueLoop; i++) {
							Token = ParseStr(FromClause, i, ",", "\"").Trim();
							if (Token == TableName) {
								TBNAME = TableName;
                                continueLoop = false;
                            }
                            else if (TokenCount(Token, " ") == 2) {
								if (ParseStr(Token, 2, " ").Trim().ToUpper() == TableName.ToUpper()) {
									TBNAME = ParseStr(Token, 1, " ").Trim();
                                    continueLoop = false;
                                }
                            }
						}
						strOwner = dbparmOwner;
						if (Strings.InStr(TBNAME, ".") > 0) {
							strOwner = ParseStr(TBNAME, 1, ".");
							if (strOwner != dbparmOwner && strOwner == dbparmOwner.ToUpper())
								strOwner = dbparmOwner;
							TBNAME = ParseStr(TBNAME, 2, ".");
						}

						dsColumns = OpenDataSet(string.Format("Select COLUMN_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME='{0}' And TABLE_SCHEMA='{1}'", TBNAME, strOwner));
						dvColumns = new DataView(dsColumns.Tables[0], clsSupport.bpeNullString, "ORDINAL_POSITION Asc", DataViewRowState.CurrentRows);
						foreach (DataRowView mDRV in dvColumns) {
							string ColumnName = (string)mDRV["NAME"];
							string tmpSQL = string.Format(" {0}.{1} As [{0}.{2}],", TBNAME, (ColumnName.IndexOf(" ") != -1 ? "[" + ColumnName + "]" : ColumnName), ColumnName);
							string tmpAliasedSQL = string.Format(" {0}.{1} As [{0}.{1}],", TableName, ColumnName);
							if (ColumnName != "ROWID")
								NewFieldList += tmpAliasedSQL;
						}
						if ((dvColumns != null)){dvColumns.Dispose();dvColumns = null;}
						if ((dsColumns != null)){dsColumns.Dispose();dsColumns = null;}
						NewFieldList = NewFieldList.Substring(0, NewFieldList.Length - 1);
						//Remove the final comma...
					} else if (ColumnAlias != clsSupport.bpeNullString) {
						if (Strings.Left(ColumnAlias, 1) == "\"" && Strings.Right(ColumnAlias, 1) == "\"") {
							NewFieldList += string.Format(" {0} As [{1}]", Token, ColumnAlias.Replace("\"", clsSupport.bpeNullString));
						} else {
							NewFieldList += string.Format(" {0} As [{1}]", Token, ColumnAlias);
						}
					} else {
						NewFieldList += " " + Token;
					}
					if (FieldList != clsSupport.bpeNullString)
						NewFieldList += ",";
					//Debug.Print "Token #" & i & ": " & Token
				}
				NewFieldList = NewFieldList.Trim();
				SaveFromClause = SaveFromClause.Trim();

				string NewSQLsource = string.Format("Select{0}{1} {2} From {3}", new object[] {
					(Distinct ? " Distinct" : clsSupport.bpeNullString),
					(RowLimit != 0 ? string.Format(" Top {0}", RowLimit) : clsSupport.bpeNullString),
					NewFieldList,
					SaveFromClause
				});
				if (WhereClause != clsSupport.bpeNullString)
					NewSQLsource += " Where " + WhereClause;
				if (HavingClause != clsSupport.bpeNullString)
					NewSQLsource += " Having " + HavingClause;
				if (GroupByClause != clsSupport.bpeNullString)
					NewSQLsource += " Group By " + GroupByClause;
				if (OrderByClause != clsSupport.bpeNullString)
					NewSQLsource += " Order By " + OrderByClause;

				//Sanity Checks... 
				if (NewFieldList == clsSupport.bpeNullString || NewFieldList == ", *" || NewFieldList == "," || SaveFromClause == clsSupport.bpeNullString) {
					string Message = "Corrupt SQL Statement about to be returned from " + EntryName + Constants.vbCrLf;
					Message += Constants.vbTab + string.Format("{0}: {1}", "Original SQL", SQLsource) + Constants.vbCrLf;
					Message += Constants.vbTab + string.Format("{0}: {1}", "Corrupt SQL", NewSQLsource) + Constants.vbCrLf;
					Message += Constants.vbTab + new StackTrace(true).ToString() + Constants.vbCrLf;
					throw new ApplicationException(Message);
				}
				SQLsource = NewSQLsource;
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex);
			} finally {
				//iCache = Nothing
				if ((dvColumns != null)){dvColumns.Dispose();dvColumns = null;}
				if ((dsColumns != null)){dsColumns.Dispose();dsColumns = null;}
			}
		}
		public void SQLParseColumn(ref string FieldList, ref string ColumnExpression, ref string ColumnAlias)
		{
			if ((FieldList == null))
				FieldList = clsSupport.bpeNullString;
			if ((ColumnExpression == null))
				ColumnExpression = clsSupport.bpeNullString;
			if ((ColumnAlias == null))
				ColumnAlias = clsSupport.bpeNullString;
			FieldList = FieldList.Trim();
			ColumnExpression = clsSupport.bpeNullString;
			ColumnAlias = clsSupport.bpeNullString;
            bool continueLoop = true;
            for (int i = 0; i <= FieldList.Length - 1 && continueLoop; i++) {
				switch (FieldList.Substring(i, 1)) {
					case " ":
						//Since we've skipped forward through any paren-pairs, if we now encounter a " FROM ", it should be the main SQL's From clause...
						string temp = FieldList.Substring(i + 1).Trim();
						if (temp.ToUpper().StartsWith("FROM ")) {
                            ColumnExpression = FieldList.Substring(0, i);
                            continueLoop = false;
                        }
                        break;
					case ",":
						ColumnExpression = FieldList.Substring(0, i);
                        continueLoop = false;
                        break;
					case "(":
						//Skip forward through the end of this expression...
						short openParenCount = 1;
                        bool continueLoop2 = true;
                        for (int iParen = i + 1; iParen <= FieldList.Length - 1 && continueLoop2; iParen++) {
							switch (FieldList.Substring(iParen, 1)) {
								case "(":
									openParenCount += 1;
									break;
								case ")":
									openParenCount -= 1;
									break;
							}
							if (openParenCount == 0) {
                                i = iParen;
                                continueLoop2 = false;
                            }
                        }
						break;
				}
			}
			if (ColumnExpression == clsSupport.bpeNullString) {
				ColumnExpression = FieldList;
				ColumnAlias = ColumnExpression;
				FieldList = clsSupport.bpeNullString;
			}
			if (ColumnExpression.IndexOf(")") != -1 && !ColumnExpression.EndsWith(")")) {
				if (FieldList != clsSupport.bpeNullString)
					FieldList = FieldList.Substring(ColumnExpression.Length).Trim();
				//...before we change our ColumneExpression...
				string tempColumn = ParseStr(ColumnExpression.ToUpper(), 1, " AS ", "\"");
				string tempAlias = ParseStr(ColumnExpression.ToUpper(), 2, " AS ", "\"");
				if (tempColumn == clsSupport.bpeNullString || tempAlias == clsSupport.bpeNullString) {
                    //An Alias may be present but not delimited with at space between the column expression and the alias, "auto-correct" such occurrences...
                    continueLoop = true;
                    for (int i = ColumnExpression.Length - 1; i >= 0 && continueLoop; i += -1) {
						if (ColumnExpression.Substring(i, 1) == ")") {
							ColumnExpression = string.Format("{0} As [{1}]", ColumnExpression.Substring(0, i + 1), ColumnExpression.Substring(i + 1));
							ColumnAlias = clsSupport.bpeNullString;
                            //This causes the calling routine to take ColumnExpression as (i.e. already has the 'As "Alias"')...
                            continueLoop = false;
                        }
                    }
				} else if (tempAlias != clsSupport.bpeNullString) {
					ColumnAlias = clsSupport.bpeNullString;
				}
			} else {
				if (FieldList != clsSupport.bpeNullString)
					FieldList = FieldList.Substring(ColumnExpression.Length).Trim();
				//...before we change our ColumneExpression...
				if (Strings.Left(ColumnExpression, 1) == "[" && Strings.Right(ColumnExpression, 1) == "]") {
					if (ColumnExpression.IndexOf(" ") == -1) {
						//Strip the brackets, they're unnecessary...
						ColumnExpression = ColumnExpression.Substring(1, ColumnExpression.Length - 2);
					} else {
						//We still need them due to the embedded space (otherwise the space denotes separator between table name and alias)...
						//Leave ColumnExpression alone, but strip the brackets from ColumnAlias so it will be processed correctly downstream...
						ColumnAlias = ColumnExpression.Substring(1, ColumnExpression.Length - 2);
					}
				} else {
					int iPos = ColumnExpression.ToUpper().IndexOf(" AS ");
					if (iPos > -1) {
						string delim = ColumnExpression.Substring(iPos + " AS ".Length, 1);
						//Check for an acceptable delimiter (assume the statement is syntactically correct and the delimiters match)...
						if (delim == "\"" || delim == "'" || delim == "[") {
							ColumnAlias = clsSupport.bpeNullString;
							//This causes the calling routine to take ColumnExpression as (i.e. already has the 'As "Alias"')...
						//Reconstruct the ColumnExpression and its Alias (which will be delimited with double-quotes below)...
						} else {
							ColumnExpression = string.Format("{0} As [{1}]", ColumnExpression.Substring(0, iPos), ColumnExpression.Substring(iPos + " AS ".Length).Trim());
							ColumnAlias = clsSupport.bpeNullString;
							//This causes the calling routine to take ColumnExpression as (i.e. already has the 'As "Alias"')...
						}
					} else {
						ColumnAlias = ColumnExpression;
					}
				}
			}
			if (ColumnAlias != clsSupport.bpeNullString)
				ColumnAlias = string.Format("\"{0}\"", ColumnAlias);
			if (FieldList.StartsWith(","))
				FieldList = FieldList.Substring(1).Trim();
		}
		private string SQLReplace(string SQLSource, string Target, string ReplacementValue)
		{
			string functionReturnValue = null;
			functionReturnValue = SQLSource;
			int iPos = SQLSource.ToUpper().IndexOf(Target);
			if (iPos == -1)
				return functionReturnValue;
			functionReturnValue = string.Format("{0}{1}{2}", SQLSource.Substring(0, iPos), ReplacementValue, SQLSource.Substring(iPos + Target.Length));
			return functionReturnValue;
		}
		private string ParseConnectionString(string strConnection, DBEConnectionStringEnum ItemType)
		{
			string functionReturnValue = null;
			int iToken = 0;
			int maxTokens = 0;
			string Token = clsSupport.bpeNullString;
			int iSearch = 0;
			string[] SearchString = { };
			string ExtendedProperties = clsSupport.bpeNullString;
			functionReturnValue = clsSupport.bpeNullString;

			//These connection strings change with one version of ADO to the next, so we must be flexible here...
			//The idea is to build a list of property strings, the first of which is the passed connection string itself.
			//Any property named "EXTENDED PROPERTIES" will be ignored, so we can treat them separately. Those extended properties
			//will be processed as their own string if the desired property isn't otherwise found...

			SearchString = new string[2];
			SearchString[1] = strConnection;
			if (Strings.InStr(Strings.UCase(strConnection), "EXTENDED PROPERTIES") > 0) {
				ExtendedProperties = clsSupport.bpeNullString;
                bool continueLoop = true;
                for (iToken = 1; iToken <= TokenCount(strConnection, ";") && continueLoop; iToken++) {
					Token = ParseStr(strConnection, iToken, ";", "\"");
					//Debug.Print "Token #" & iToken & ": " & Token
					if (Strings.Mid(Strings.UCase(Token), 1, Strings.Len("EXTENDED PROPERTIES")) == "EXTENDED PROPERTIES")
                        continueLoop = false;
                }
                if (Strings.Mid(Strings.UCase(Token), 1, Strings.Len("EXTENDED PROPERTIES")) != "EXTENDED PROPERTIES") {
					functionReturnValue = clsSupport.bpeNullString;
					return functionReturnValue;
				}
				ExtendedProperties = Strings.Mid(Token, Strings.Len("EXTENDED PROPERTIES=") + 1);
				Array.Resize(ref SearchString, 3);
				SearchString[2] = ExtendedProperties;
			}

			for (iSearch = Information.UBound(SearchString); iSearch >= 1; iSearch += -1) {
				maxTokens = TokenCount(SearchString[iSearch], ";");
				Token = SearchString[iSearch];
				for (iToken = 1; iToken <= maxTokens; iToken++) {
					Token = ParseStr(SearchString[iSearch], iToken, ";", "\"");
					if (Token != clsSupport.bpeNullString) {
						switch (Strings.UCase(Strings.Mid(Token, 1, Strings.InStr(Token, "=") - 1))) {
							case "APPLICATION":
							case "APPLICATION NAME":
								if (ItemType == DBEConnectionStringEnum.dbecseApplication){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "CONNECT TIMEOUT":
								if (ItemType == DBEConnectionStringEnum.dbecseConnectTimeout){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "DRIVER":
							case "PROVIDER":
								if (ItemType == DBEConnectionStringEnum.dbecseDriver) {
									functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);
									if (Strings.Left(functionReturnValue, 1) == "{")
										functionReturnValue = Strings.Mid(functionReturnValue, 2, Strings.Len(functionReturnValue) - 2);
									return functionReturnValue;
								}
								break;
							case "UID":
							case "USER ID":
								if (ItemType == DBEConnectionStringEnum.dbecseUserID){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "PASSWORD":
								//SQL Server
								if (ItemType == DBEConnectionStringEnum.dbecsePassword){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "PWD":
								//SQLBase
								if (ItemType == DBEConnectionStringEnum.dbecsePassword){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "SRVR":
								//SQLBase
								if (ItemType == DBEConnectionStringEnum.dbecseServer){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "SERVER":
								//SQL Server
								if (ItemType == DBEConnectionStringEnum.dbecseServer){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "DBQ":
								//Access
								if (ItemType == DBEConnectionStringEnum.dbecseDatabase) {
									functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);
									return functionReturnValue;
								} else if (ItemType == DBEConnectionStringEnum.dbecseServer) {
									functionReturnValue = TCBase.My.MyProject.Computer.Name;
									return functionReturnValue;
								}
								break;
							case "DB":
								//SQLBase
								if (ItemType == DBEConnectionStringEnum.dbecseDatabase){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "DATABASE":
							case "INITIAL CATALOG":
								//SQL Server
								if (ItemType == DBEConnectionStringEnum.dbecseDatabase){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "DATA SOURCE":
								//SQL Server
								if (ItemType == DBEConnectionStringEnum.dbecseServer){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "WORKSTATION ID":
								if (ItemType == DBEConnectionStringEnum.dbecseWorkstationID){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "PACKET SIZE":
								if (ItemType == DBEConnectionStringEnum.dbecsePacketSize){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
							case "POOLING":
								if (ItemType == DBEConnectionStringEnum.dbecsePooling){functionReturnValue = Strings.Mid(Token, Strings.InStr(Token, "=") + 1);return functionReturnValue;}
								break;
						}
					}
				}
			}
			return functionReturnValue;
		}
		public void ParseSQLSelect(string SQLStatement, ref string FieldList, ref string TableList, ref string WhereClause, ref string OrderByClause, ref bool Distinct)
		{
			int iFrom = 0;
            int iWhere = 0;
            int iOrderBy = 0;

			FieldList = clsSupport.bpeNullString;
			TableList = clsSupport.bpeNullString;
			WhereClause = clsSupport.bpeNullString;
			OrderByClause = clsSupport.bpeNullString;

			//Gotta have a SELECT and FROM...
			SQLStatement = Strings.Trim(Strings.Replace(SQLStatement, Constants.vbCrLf, " "));
			SQLStatement = Strings.Trim(Strings.Replace(SQLStatement, Constants.vbTab, " "));
			if (Strings.UCase(Strings.Mid(SQLStatement, 1, 6)) != "SELECT")
				return;
			if (Strings.UCase(Strings.Left(SQLStatement, Strings.Len("SELECT DISTINCT"))) == "SELECT DISTINCT") {
				SQLStatement = Strings.Trim(Strings.Right(SQLStatement, Strings.Len(SQLStatement) - Strings.Len("SELECT DISTINCT")));
				Distinct = true;
			} else {
				SQLStatement = Strings.Trim(Strings.Right(SQLStatement, Strings.Len(SQLStatement) - Strings.Len("SELECT")));
				Distinct = false;
			}
			iFrom = Strings.InStr(Strings.UCase(SQLStatement), " FROM ");
			if (iFrom == 0)
				return;

			//Parse FieldList...
			FieldList = Strings.Trim(Strings.Left(SQLStatement, iFrom));
			SQLStatement = Strings.Trim(Strings.Mid(SQLStatement, iFrom + Strings.Len(" FROM ")));
			iWhere = Strings.InStr(Strings.UCase(SQLStatement), " WHERE ");

			//Parse TableList...
			if (iWhere == 0) {
				iOrderBy = Strings.InStr(Strings.UCase(SQLStatement), " ORDER BY ");
				if (iOrderBy == 0) {
					TableList = Strings.Trim(SQLStatement);
					return;
				} else {
					TableList = Strings.Trim(Strings.Left(SQLStatement, iOrderBy));
					SQLStatement = Strings.Trim(Strings.Mid(SQLStatement, iOrderBy + Strings.Len(" ORDER BY ")));
					OrderByClause = Strings.Trim(SQLStatement);
					return;
				}
			} else {
				TableList = Strings.Trim(Strings.Left(SQLStatement, iWhere));
				SQLStatement = Strings.Trim(Strings.Mid(SQLStatement, iWhere + Strings.Len(" WHERE ")));
				iOrderBy = Strings.InStr(Strings.UCase(SQLStatement), " ORDER BY ");

				if (iOrderBy == 0) {
					WhereClause = Strings.Trim(SQLStatement);
					return;
				} else {
					WhereClause = Strings.Trim(Strings.Left(SQLStatement, iOrderBy));
					SQLStatement = Strings.Trim(Strings.Mid(SQLStatement, iOrderBy + Strings.Len(" ORDER BY ")));
					OrderByClause = Strings.Trim(SQLStatement);
					return;
				}
			}
		}
		public void RemoveImages(int RowIndex)
		{
			if (RowIndex > tcDataView.Count - 1)
				return;
			//We're in the process of adding a new row, leave for now, we'll be back...
			if (mMode == ActionModeEnum.modeAdd)
				return;
			//Removing the old row's images marks it as modified, which would later cause a InvalidOperationException when posting the add...
			if (mMode == ActionModeEnum.modeCopy)
				return;
			//We're copying an existing record, so don't change our original guy or it will confuse the update logic...
			Trace("RemoveImages({0})", RowIndex, trcOption.trcApplication);
			DataRowView drv = tcDataView[RowIndex];
			if (tcDataView.Table.Columns.Contains("Image"))
				drv["Image"] = DBNull.Value;
			if (tcDataView.Table.Columns.Contains("OtherImage"))
				drv["OtherImage"] = DBNull.Value;
			Trace("RemoveImages: drv.EndEdit()", trcOption.trcDB | trcOption.trcApplication);
			drv.EndEdit();
			switch (mMode) {
				case ActionModeEnum.modeDisplay:
					Trace("RemoveImages: tcDataView.Table.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
					tcDataView.Table.AcceptChanges();
					TraceDataView(true);
					break;
			}
			drv = null;
		}
		//Private Sub SaveHistory(ByVal dataRow As DataRow)
		//    Try
		//        'We wouldn't have gotten here if something wasn't changed... We just need to rediscover the actual change...
		//        'If there's no Original version, the row must be new, so consider it a change...
		//        If dataRow.HasVersion(DataRowVersion.Original) Then Throw New Exception("SaveHistory(DataRow) is intended to be called for new records only! This record has an Original version.")
		//        Dim SQL As String = "Insert Into History([Who],[DateChanged],[TableName],[RecordID],[OriginalValue],[Value]) Values (@Who,CURRENT_TIMESTAMP,@TableName,@RecordID,@OriginalValue,@Value)"
		//        Dim HistoryCommand As New SqlCommand(SQL, tcConnection, tcTransaction)
		//        With HistoryCommand
		//            .Parameters.Add(New SqlParameter("@Who", mUserID))
		//            .Parameters.Add(New SqlParameter("@TableName", dataRow.Table.TableName))
		//            .Parameters.Add(GetNewSqlParameter("@RecordID", dataRow.Table.Columns(mTableIDColumn), dataRow))
		//            .Parameters.Add(GetNewSqlParameter("@OriginalValue", dataRow.Table.Columns(mTableKeyColumn), dataRow))
		//            .Parameters.Add(New SqlParameter("@Value", "Record Added"))
		//            .ExecuteNonQuery()
		//        End With
		//    Catch ex As SqlException
		//        Dim Message As String = String.Format("{0} ({1})", ex.Message, ex.GetType.Name)
		//        Message &= vbCrLf & "Do you want to continue without recording this History entry?"
		//        If MessageBox.Show(Me.ActiveForm, Message, ex.GetType.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Throw
		//    End Try
		//End Sub
		private void SaveHistory(DataRow dataRow, string dataMember)
		{
			SqlCommand HistoryCommand = null;
			DataRowVersion oldVersion = default(DataRowVersion);
			DataRowVersion newVersion = default(DataRowVersion);
			try {
				//We wouldn't have gotten here if something wasn't changed... We just need to rediscover the actual change...
				//If there's no Original version, the row must be new, so consider it a change...
				if (!dataRow.HasVersion(DataRowVersion.Original)) {
					//Record Added
				}
				oldVersion = DataRowVersion.Original;

				newVersion = DataRowVersion.Default;
				//According to the documentation, Proposed should only exist when BeginEdit/EndEdit/CancelEdit are being used
				//but I'm not seeing this as being the case. So until I get it straight, use it if it exists...
				//It may be that DataRowVersion.Proposed is only intended on being used in RowChanging event handlers...
				if (dataRow.HasVersion(DataRowVersion.Proposed))
					newVersion = DataRowVersion.Proposed;

				string SQL = "Insert Into History([Who],[DateChanged],[TableName],[RecordID],[Column],[OriginalValue],[Value]) Values (@Who,CURRENT_TIMESTAMP,@TableName,@RecordID,@Column,@OriginalValue,@Value)";
				string OriginalValue = clsSupport.bpeNullString;
				string Value = clsSupport.bpeNullString;

				switch (mMode) {
					case ActionModeEnum.modeAdd:
					case ActionModeEnum.modeCopy:
						OriginalValue = "Record Added";
						Value = (string)GetDataRowValue(dataRow, dataMember, newVersion);
						break;
					case ActionModeEnum.modeDelete:
						OriginalValue = (string)GetDataRowValue(dataRow, dataMember, oldVersion);
						Value = "Record Deleted";
						break;
					default:
						OriginalValue = (string)GetDataRowValue(dataRow, dataMember, oldVersion);
						Value = (string)GetDataRowValue(dataRow, dataMember, newVersion);
						break;
				}
				HistoryCommand = new SqlCommand(SQL, tcConnection, tcTransaction);
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Who", mUserID));
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TableName", dataRow.Table.TableName));
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RecordID", dataRow[mTableIDColumn]));
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Column", dataMember));
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OriginalValue", OriginalValue));
                HistoryCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Value", (mMode == ActionModeEnum.modeDelete ? "Record Deleted" : Value)));
                HistoryCommand.ExecuteNonQuery();
			} catch (SqlException ex) {
				string Message = string.Format("{0} ({1})", ex.Message, ex.GetType().Name);
				Message += Constants.vbCrLf + "Do you want to continue without recording this History entry?";
				if (MessageBox.Show(this.ActiveForm, Message, ex.GetType().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					throw;
				if (HistoryCommand != null){HistoryCommand.Dispose();HistoryCommand = null;}
			}
		}
		public void TraceDataTable()
		{
			if (!TraceMode)
				return;
			Trace("Dumping DataRows (mDRV(\"{0}\"):={1})...", mTableIDColumn, mDRV[mTableIDColumn], trcOption.trcDB);
			int i = 0;
			foreach (DataRow dr in tcDataView.Table.Rows) {
				if (dr.RowState != DataRowState.Unchanged || object.ReferenceEquals(dr, mDRV.Row)) {
					string[] args = {
						i.ToString(),
						(string)dr[mTableIDColumn],
						(string)(Information.IsDBNull(dr[mTableKeyColumn]) ? "DBNull" : dr[mTableKeyColumn]),
						dr.RowState.ToString()
					};
					Trace((object.ReferenceEquals(dr, mDRV.Row) ? "->" : "  ") + string.Format("{0,5:00000} RecordID #{1,5:00000} {2:12} (RowState: {3})", args), trcOption.trcDB);
				}
				i += 1;
			}
		}
		public void TraceDataView(bool OnlyIfInteresting = false)
		{
            //If Not TraceMode Then Exit Sub
            try
            {
                if ((mDRV == null) || (tcDataView == null))
                    throw new ExitTryException();
                Trace("Dumping DataRowViews (mDRV(\"{0}\"):={1})...", mTableIDColumn, (Information.IsDBNull(mDRV[mTableIDColumn]) ? "DBNull" : mDRV[mTableIDColumn]), trcOption.trcDB);
                if (mDRV.DataView.RowFilter != clsSupport.bpeNullString)
                    Trace("RowFilter: {0}", mDRV.DataView.RowFilter, trcOption.trcDB);
                if (mDRV.DataView.Sort != clsSupport.bpeNullString)
                    Trace("Sort: {0}", mDRV.DataView.Sort, trcOption.trcDB);
                for (int iRow = 0; iRow <= tcDataView.Count - 1; iRow++)
                {
                    DataRowView drv = tcDataView[iRow];
                    string Message = string.Format("       {0,5:00000} RecordID #{1,5:00000} {2} (RowState: {3}; RowVersion: {4};{5}{6})", new object[] {
                        iRow,
                        (Information.IsDBNull(drv[mTableIDColumn]) ? "DBNull" : drv[mTableIDColumn]),
                        (Information.IsDBNull(drv[mTableKeyColumn]) ? "DBNull" : drv[mTableKeyColumn]),
                        drv.Row.RowState.ToString(),
                        drv.RowVersion.ToString(),
                        (drv.IsEdit ? " IsEdit; " : ""),
                        (drv.IsNew ? " IsNew; " : "")
                    });
                    int iPos = mCurrencyManager.Position;
                    string tag = clsSupport.bpeNullString;

                    if (!Information.IsDBNull(mDRV[mTableIDColumn]))
                    {
                        if (Information.IsDBNull(drv[mTableIDColumn]) && Information.IsDBNull(mDRV[mTableIDColumn]))
                        {
                            tag = " mDRV->";
                        }
                        else if (!Information.IsDBNull(drv[mTableIDColumn]) && drv[mTableIDColumn] == mDRV[mTableIDColumn])
                        {
                            tag = " mDRV->";
                        }
                    }
                    if (iRow == iPos)
                        tag = (tag == clsSupport.bpeNullString ? "cmPos->" : tag.Replace("->", "=>"));
                    if (tag != clsSupport.bpeNullString)
                        Message = tag + Message.Substring(8);
                    if (drv.Row.RowState == DataRowState.Deleted)
                    {
                        Trace(Message, trcOption.trcDB);
                        Trace("             Row Deleted", trcOption.trcDB);
                    }
                    else if (!OnlyIfInteresting || drv.Row.RowState != DataRowState.Unchanged || tag != clsSupport.bpeNullString || drv.IsEdit || drv.IsNew)
                    {
                        Trace(Message, trcOption.trcDB);
                        if (drv.Row.RowState == DataRowState.Modified)
                        {
                            foreach (DataColumn iColumn in drv.Row.Table.Columns)
                            {
                                if (AnythingHasChanged(drv.Row[iColumn.ColumnName, DataRowVersion.Original], drv.Row[iColumn.ColumnName, DataRowVersion.Default]))
                                {
                                    string OriginalValue = clsSupport.bpeNullString;
                                    string CurrentValue = clsSupport.bpeNullString;
                                    WhatChanged(drv.Row, iColumn.ColumnName, ref OriginalValue, ref CurrentValue);
                                    Trace("             {0,-32} Changed From {1} to {2}", iColumn.ColumnName, (OriginalValue != clsSupport.bpeNullString ? OriginalValue.Replace(ControlChars.CrLf, "CR/LF") : "\"\"\""), (CurrentValue != clsSupport.bpeNullString ? CurrentValue.Replace(ControlChars.CrLf, "CR/LF") : "\"\"\""), trcOption.trcApplication);
                                }
                            }
                        }
                    }
                }
            } catch (ExitTryException ex) {
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcDB);
			}
		}
		private void WhatChanged(DataRow dataRow, string dataMember, ref string OriginalValue, ref string Value)
		{
			DataRowVersion oldVersion = default(DataRowVersion);
			DataRowVersion newVersion = default(DataRowVersion);

			//We wouldn't have gotten here if something wasn't changed... We just need to rediscover the actual change...
			//If there's no Original version, the row must be new, so consider it a change...
			if (!dataRow.HasVersion(DataRowVersion.Original)) {
				//Record Added
			}
			oldVersion = DataRowVersion.Original;

			newVersion = DataRowVersion.Default;
			//According to the documentation, Proposed should only exist when BeginEdit/EndEdit/CancelEdit are being used
			//but I'm not seeing this as being the case. So until I get it straight, use it if it exists...
			//It may be that DataRowVersion.Proposed is only intended on being used in RowChanging event handlers...
			if (dataRow.HasVersion(DataRowVersion.Proposed))
				newVersion = DataRowVersion.Proposed;

			switch (MapSystemToSQLDBType(dataRow.Table.Columns[dataMember].DataType)) {
				case SqlDbType.Bit:
					OriginalValue = (Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : string.Format("{0:True/False}", dataRow[dataMember, oldVersion].ToString()));
					Value = (Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : string.Format("{0:True/False}", dataRow[dataMember, newVersion].ToString()));
					break;
				case SqlDbType.TinyInt:
					OriginalValue = (string)(Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]);
					Value = (string)(Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]);
					break;
				case SqlDbType.Binary:
				case SqlDbType.Image:
					OriginalValue = (Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : "<Original Binary/Image>");
					Value = (Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : "<New Binary/Image>");
					break;
				case SqlDbType.DateTime:
					OriginalValue = (Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", dataRow[dataMember, oldVersion]));
					Value = (Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", dataRow[dataMember, newVersion]));
					break;
				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					break;
				case SqlDbType.Float:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					break;
				case SqlDbType.BigInt:
				case SqlDbType.Int:
				case SqlDbType.SmallInt:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					break;
				case SqlDbType.Real:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					break;
				case SqlDbType.NVarChar:
				case SqlDbType.VarChar:
				case SqlDbType.NText:
				case SqlDbType.Text:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					//History.OriginalValue is defined as nvarchar(maxHistoryValue), so truncate if necessary...
					if (OriginalValue.Length > maxHistoryValue)
						OriginalValue = Strings.Left(OriginalValue, maxHistoryValue);
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					//History.Value is defined as nvarchar(maxHistoryValue), so truncate if necessary...
					if (Value.Length > maxHistoryValue)
						Value = Strings.Left(Value, maxHistoryValue);
					break;
				default:
					OriginalValue = Convert.ToString((Information.IsDBNull(dataRow[dataMember, oldVersion]) ? "" : dataRow[dataMember, oldVersion]));
					Value = Convert.ToString((Information.IsDBNull(dataRow[dataMember, newVersion]) ? "" : dataRow[dataMember, newVersion]));
					break;
			}
		}
		#endregion
		#region "Registry Methods"
		private void DumpRegistryKey(Microsoft.Win32.RegistryKey Key, string SubKeyName)
		{
			Trace("{0}[{1}\\{2}]", new object[] {
				ControlChars.NewLine,
				Key.Name,
				SubKeyName
			}, trcOption.trcApplication);
			RegistryKey rKey = Key.OpenSubKey(SubKeyName, false);
			string[] ValueNames = rKey.GetValueNames();
			foreach (string v in ValueNames) {
				object o = rKey.GetValue(v);
				switch (o.GetType().Name) {
					case "String":
						Trace("\"{0}\"=\"{1}\"", new object[] {
							v,
							o.ToString().Replace("\\", "\\\\")
						}, trcOption.trcApplication);
						break;
					case "Int32":
						Trace("\"{0}\"=dword:{1:x8}", new object[] {
							v,
							o
						}, trcOption.trcApplication);
						break;
				}
			}
			string[] SubKeyNames = rKey.GetSubKeyNames();
			foreach (string s in SubKeyNames) {
				DumpRegistryKey(rKey, s);
			}
			rKey.Close();
		}
		public void DumpRegistrySettings(Microsoft.Win32.RegistryKey root, string Key)
		{
			Trace("Windows Registry Editor Version 5.00", trcOption.trcApplication);
			DumpRegistryKey(root, Key);
		}
		public object GetUserPreference(string Key, object vDefault) 
		{
			clsCollection parms = new clsCollection(new string[] { "UserID", "Key" }, new Object[] { mUserID, Key });
			string SQL = string.Format("Select [Value] From [UserPreferences] Where [UserID]=@UserID And [Key]=@Key;");

			object result = ExecuteScalarCommand(SQL, parms);

			if ((result == null) || Information.IsDBNull(result)) result = vDefault;
			return result;
		}
		private void ExportRegistryKey(Microsoft.Win32.RegistryKey Key, string SubKeyName, StreamWriter ExportWriter)
		{
			ExportWriter.WriteLine(string.Format("{0}[{1}\\{2}]", ControlChars.NewLine, Key.Name, SubKeyName));
			RegistryKey rKey = Key.OpenSubKey(SubKeyName, false);
			string[] ValueNames = rKey.GetValueNames();
			foreach (string v in ValueNames) {
				object o = rKey.GetValue(v);
				switch (o.GetType().Name) {
					case "String":
						ExportWriter.WriteLine(string.Format("\"{0}\"=\"{1}\"", v, o.ToString().Replace("\\", "\\\\").Replace("\"", "\\\"")));
						break;
					case "Int32":
						ExportWriter.WriteLine(string.Format("\"{0}\"=dword:{1:x8}", v, o));
						break;
				}
			}
			string[] SubKeyNames = rKey.GetSubKeyNames();
			foreach (string s in SubKeyNames) {
				ExportRegistryKey(rKey, s, ExportWriter);
			}
			rKey.Close();
		}
		public void ExportRegistrySettings(Microsoft.Win32.RegistryKey root, string Key, string FileName)
		{
			StreamWriter ExportWriter = new StreamWriter(FileName, false);
			ExportWriter.WriteLine("Windows Registry Editor Version 5.00");

			ExportRegistryKey(root, Key, ExportWriter);

			ExportWriter.Flush();
			ExportWriter.Close();
			ExportWriter = null;
		}
		public void SaveBounds(string Key, int iLeft, int iTop, int iWidth, int iHeight)
		{
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, Key, "Form Left", iLeft);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, Key, "Form Top", iTop);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, Key, "Form Width", iWidth);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, Key, "Form Height", iHeight);
		}
		public void SaveUserPreference(string Key, object Value)
        {
			clsCollection parms = new clsCollection(new String[] { "UserID", "Key", "Value" }, new Object[] { mUserID, Key, Value });
			string SQL = string.Empty;

			if (GetUserPreference(Key, null) == null) {
				SQL = "Insert Into [UserPreferences]([UserID],[Key],[Value]) Values(@UserID,@Key,@Value);";
			} else {
				SQL = "Update [UserPreferences] Set [Value]=@Value Where [UserID]=@UserID And [Key]=@Key;";
			}
			int recordsAffected = 0;
			ExecuteCommand(SQL, parms, ref recordsAffected);
        }
		#endregion
		#region "Utility Methods"
		public void Busy(bool IsBusy)
		{
			Cursor myCursor = (IsBusy ? Cursors.WaitCursor : Cursors.Default);
			bool fEnabled = !IsBusy;
			var _with16 = mActiveForm;
			_with16.Cursor = myCursor;
			_with16.Enabled = fEnabled;
		}
		public void cbAddItem(System.Windows.Forms.ComboBox cb, string item)
		{
			foreach (string iItem in cb.Items) {
				if (iItem.CompareTo(item) == 0)
					return;
			}
			cb.Items.Add(item);
		}
		private int cbFind(System.Windows.Forms.ComboBox cb, string item)
		{
			int functionReturnValue = 0;
			bool found = false;
            bool continueLoop = true;
			for (int i = 0; i <= cb.Items.Count - 1 && continueLoop; i++) {
				if (item == Strings.Trim((string)cb.Items[i])) {
					functionReturnValue = i;
					found = true;
                    continueLoop = false;
				}
			}
			if (!found)
				functionReturnValue = -1;
			return functionReturnValue;
		}
		public void cbValidate(System.Windows.Forms.ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...

			//Determine if we are Simple or Complex bound...
			Trace("cbValidate: sender.Text = \"{0}\"", new object[] { sender.Text }, trcOption.trcApplication);
			if (string.IsNullOrEmpty(sender.Text))
				Trace((new StackTrace(true)).ToString(), trcOption.trcApplication);
			if ((sender.DataBindings["SelectedValue"] == null)) {
				cbValidateSimple(sender, e);
			} else {
				cbValidateComplex(sender, e);
			}
		}
		private void cbValidateComplex(System.Windows.Forms.ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...
			Binding dataBinding = sender.DataBindings["SelectedValue"];
			DataView dataSource = (DataView)dataBinding.DataSource;
			string dataMember = sender.ValueMember;
			//dataBinding.BindingMemberInfo.BindingMember
			object dataValue = dataSource[sender.BindingContext[dataSource].Position][dataMember];
			DataView displaySource = (DataView)sender.DataSource;
			string displayMember = sender.DisplayMember;
			string displayValue = TrimTabs(sender.Text);

			Trace("cbValidateComplex: sender.Text = \"{0}\"", new object[] { sender.Text }, trcOption.trcApplication);
			if (string.IsNullOrEmpty(sender.Text))
				Trace((new StackTrace(true)).ToString(), trcOption.trcApplication);

			if (displayValue == clsSupport.bpeNullString && (sender.Tag == null ? "" : Convert.ToString(sender.Tag)).ToUpper().IndexOf("REQUIRED") >= 0) {
				throw new Exception("Value is required.");
			} else if ((sender.SelectedItem == null)) {
				//Something was entered, but not selected from the list... See if it matches something already in the list,
				//otherwise prompt the user if he wants it added...
				for (int i = 0; i <= displaySource.Count - 1; i++) {
					if (!Information.IsDBNull(displaySource[i][displayMember])) {
						if (displayValue == Strings.Trim((string)displaySource[i][displayMember])) {
							//We found the entry in the list, so make it look like they clicked this one...
							sender.SelectedIndex = i;
							return;
						}
					}
				}
				//OK, it's not in the list... Ask if it should be added...
				if (MessageBox.Show(this.ActiveForm, "\"" + displayValue + "\" isn't in the list... Do you want it added...?", "Confirm Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
					sender.Text = clsSupport.bpeNullString;
					throw new Exception("Select an item from the list.");
				}
				//OK, add it to the list...
				DataRowView newRow = displaySource.AddNew();
				newRow[displayMember] = displayValue;
				Trace("newRow.EndEdit()", trcOption.trcDB | trcOption.trcApplication);
				newRow.EndEdit();
				Trace("displaySource.Table.AcceptChanges()", trcOption.trcDB | trcOption.trcApplication);
				displaySource.Table.AcceptChanges();
				sender.SelectedIndex = displaySource.Find(displayValue);
			}
		}
		private void cbValidateSimple(System.Windows.Forms.ComboBox sender, CancelEventArgs e)
		{
			//Don't trace this guy on purpose... Allow our framework to deal with reporting errors properly...
			Binding dataBinding = sender.DataBindings["Text"];
			DataView displaySource = (DataView)sender.DataSource;
			string displayMember = sender.DisplayMember;
			string displayValue = TrimTabs(sender.Text);

			if (displayValue == clsSupport.bpeNullString) {
				throw new Exception("Value is required.");
			} else if ((sender.SelectedItem == null)) {
				//Something was entered, but not selected from the list... See if it matches something already in the list,
				//otherwise add it...
				sender.SelectedIndex = cbFind(sender, displayValue);
				if (sender.SelectedIndex == -1) {
					//OK, add it to the list...
					cbAddItem(sender, displayValue);
					sender.SelectedIndex = cbFind(sender, displayValue);
				}
			}
		}
		public void GenericErrorHandler(Exception exception, bool fLogToScreen, string ExtraMessage = clsSupport.bpeNullString, Control ctl = null)
		{
			//Note: This routine is intended for use by host applications. Internal error handling is done through InternalErrorHandler...
			//      The typical calling sequence is as follows...
			//       - Deepest routine calls RaiseError which registers the error in clsErrors and throws the exception to its caller...
			//       - Next Deepest routine follows suit...
			//           .
			//           .
			//           .
			//       - Eventually the error gets bubbles-up to a top-level routine responsible for UI... That routine calls 
			//         GenericErrorHandler with the exception.
			//         GenericErrorHandler then formats the top-most exception, and reports each of the underlying exceptions. 
			//         The clsErrors collection is then cleared resetting the mechanism for the next run...

			try {
				string Caption = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				string ScreenName = clsSupport.bpeNullString;
				if ((mActiveForm != null)){Caption = mActiveForm.Text;ScreenName = mActiveForm.Name;}
				string Message = string.Format("{0} ({1})", exception.Message, exception.GetType().Name);
				string Detail = string.Format("{1} raised.{0}{2}{0}{0}StackTrace:{0}{3}{0}{0}", new object[] {
					Constants.vbCrLf,
					exception.GetType().Name,
					exception.Message,
					exception.StackTrace
				});
				if ((exception.InnerException != null)) {
					Detail += string.Format("Underlying Error(s):{0}", Constants.vbCrLf);
					Exception iException = exception.InnerException;
					while ((iException != null)) {
						Detail += string.Format("{1} raised.{0}{2}{0}{0}StackTrace:{0}{3}{0}{0}", new object[] {
							Constants.vbCrLf,
							iException.GetType().Name,
							iException.Message,
							iException.StackTrace
						});
						iException = iException.InnerException;
					}
				}

				if (fLogToScreen)
					ShowError(Message, Detail, MsgBoxStyle.Exclamation, mActiveForm, string.Format("{0} ({1})", Caption, exception.GetType().Name));
				//ShowMsgBox(ex.Message, MsgBoxStyle.OKOnly, Me, ex.GetType.Name)
				if ((ctl != null) && ctl.Enabled && ctl.Visible)
					ctl.Focus();
			} catch (Exception ex) {
			} finally {
				if (mActiveTXLevel > 0)
					AbortTrans();
			}
		}
		public ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo functionReturnValue = null;
			functionReturnValue = null;
			int i = 0;
			ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageEncoders();
			for (i = 0; i <= Encoders.Length - 1; i++) {
				if (Encoders[i].MimeType == mimeType) {
					functionReturnValue = Encoders[i];
					return functionReturnValue;
				}
			}
			functionReturnValue = null;
			return functionReturnValue;
		}
		private void ReSort(ref object pRecordSet)
		{
			//TODO: Determine if ReSort is obsolete...
			//        Const EntryName As String = "ReSort"
			//        Dim pRS As ADODB.Recordset
			//        Dim SaveID As Integer
			//        '    Dim DBinfo As clsDataBaseInfo
			//        '    Dim FieldList As String
			//        '    Dim TableList As String
			//        '    Dim WhereClause As String
			//        '    Dim OrderByClause As String

			//        On Error GoTo ErrorHandler

			//        pRS = pRecordSet
			//        '    For Each DBinfo In mDBCollection
			//        '        If DBinfo.Recordset Is pRS Then
			//        '          ParseSQLSelect(DBinfo.SQLSource, FieldList, TableList, WhereClause, OrderByClause)
			//        '            pRS.Sort = OrderByClause
			//        '            Exit For
			//        '        End If
			//        '    Next
			//        SaveID = RecordID
			//        pRS.Sort = mOrderByClause
			//        FindID(pRS, SaveID)
			//        If pRS.EOF Then Call ClearCommand()

			//        'UPGRADE_NOTE: Object pRS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        pRS = Nothing
			//        Exit Sub

			//ErrorHandler:
			//        RaiseError()
		}
		public void rtfControl_KeyPress(ref object ctl, ref short KeyAscii)
		{
			//        Const EntryName As String = "rtfControl_KeyPress"
			//        Dim rtfControl As AxRichTextLib.AxRichTextBox

			//        On Error GoTo ErrorHandler
			//        rtfControl = ctl


			//        'Debug.Print KeyAscii
			//        If KeyAscii = 1 Then 'Ctrl-A/Select All...
			//            rtfControl.SelStart = 0
			//            rtfControl.SelLength = Len(rtfControl.TextRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 2 Then  'Ctrl-B/Bold...
			//            rtfControl.SelBold = Not rtfControl.SelBold
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 3 Then  'Ctrl-C/Copy...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.SetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            VB.Clipboard.SetText(rtfControl.SelRTF, vbCFRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 9 Then  'Ctrl-I/Italic...
			//            rtfControl.SelItalic = Not rtfControl.SelItalic
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 21 Then  'Ctrl-U/Underline...
			//            rtfControl.SelUnderline = Not rtfControl.SelUnderline
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 22 Then  'Ctrl-V/Paste...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.GetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            rtfControl.SelRTF = VB.Clipboard.GetText(vbCFRTF)
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 24 Then  'Ctrl-X/Cut...
			//            'UPGRADE_ISSUE: Constant vbCFRTF was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2070"'
			//            'UPGRADE_ISSUE: Clipboard method Clipboard.SetText was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2069"'
			//            VB.Clipboard.SetText(rtfControl.SelRTF, vbCFRTF)
			//            rtfControl.SelRTF = bpeNullString
			//            KeyAscii = 0
			//        ElseIf KeyAscii = 26 Then  'Ctrl-Y/Redo...

			//            KeyAscii = 0
			//        ElseIf KeyAscii = 26 Then  'Ctrl-Z/Undo...

			//            KeyAscii = 0
			//        End If

			//ExitSub:
			//        'UPGRADE_NOTE: Object rtfControl may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
			//        rtfControl = Nothing
			//        Exit Sub

			//ErrorHandler:
			//        RaiseError()
		}
		public MsgBoxResult ShowError(string Message, string Details, MsgBoxStyle MsgBoxStyle, Form Parent = null, string Caption = clsSupport.bpeNullString, System.Drawing.Icon Icon = null)
		{
			MsgBoxResult functionReturnValue = default(MsgBoxResult);
			frmError frm = null;
			functionReturnValue = MsgBoxResult.Cancel;
			try {
				frm = new frmError(this, Parent);

                frm.ShowInTaskbar = false;
                frm.lblDetails.Visible = true;
				//Default from Parent Form...
				if ((Parent != null)) {
                    frm.Icon = ((Icon != null) ? Icon : Parent.Icon);
                    frm.Text = (Caption != clsSupport.bpeNullString ? Caption : Parent.Text);
				//Default from project/assembly (if possible)...
				} else {
					if ((Icon != null)) {
                        frm.Icon = Icon;
					} else if (mSupport.EntryComponentName != clsSupport.bpeNullString) {
						Trace(mMyTraceID + " Defaulting .Icon from project/assembly...", trcOption.trcSupport);
                        frm.Icon = mSupport.Icon(string.Format("{0}\\{1}.exe", mSupport.ApplicationPath, mSupport.EntryComponentName));
					}
                    frm.Text = (Caption != clsSupport.bpeNullString ? Caption : mSupport.ApplicationName);
				}
                frm.Message = Message;
                frm.Details = Details;
                frm.MsgBoxStyle = MsgBoxStyle;
				Trace(mMyTraceID + "Calling ShowError.ShowDialog()", trcOption.trcSupport);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(Parent);
				functionReturnValue = frm.MsgBoxResult;
                frm.OKtoClose = true;
				Trace(mMyTraceID + "Calling ShowError.Close()", trcOption.trcSupport);
                frm.Close();
			} finally {
				if (frm != null){frm.Dispose();frm = null;}
			}
			return functionReturnValue;
		}
		public string StripBraces(string Source, string Delimiter = "{")
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = Source;
			if (Source.StartsWith(Delimiter))
				functionReturnValue = Source.Substring(1, Source.Length - 2);
			return functionReturnValue;
		}
		#endregion
		#region "Debugging Tools"
		private StreamWriter dumper = null;
		private short funit = 0;
		private string FormatData(DataColumn Column, object Data, bool fDelimitted = false)
		{
			return FormatData(MapSystemToSQLDBType(Column.DataType), Data, fDelimitted);
		}
		private string FormatData(SqlDbType dType, object Data, bool fDelimitted = false)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if (Information.IsDBNull(Data)){functionReturnValue = "Null";return functionReturnValue;}

			switch (dType) {
				case SqlDbType.BigInt:
				case SqlDbType.Int:
				case SqlDbType.SmallInt:
				case SqlDbType.TinyInt:
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Bit:
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					functionReturnValue = string.Format((fDelimitted ? "\"{0}\"" : "{0}"), Data);
					break;
				case SqlDbType.DateTime:
					//, SqlDbType.Date
					functionReturnValue = Data.ToString();
					break;
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Money:
				case SqlDbType.Real:
				case SqlDbType.SmallMoney:
					functionReturnValue = Data.ToString();
					break;
				default:
					functionReturnValue = string.Format("[{0}]", dType.ToString());
					break;
			}
			//Apparently with .NET 2.0 "" is now literally a Null string (one character - that being CHR(0)) - P.S. vbNullString seems now to match bpeNullString
			if (functionReturnValue.Length == 1 && Strings.Asc(functionReturnValue.Substring(0, 1)) == 0)
				functionReturnValue = clsSupport.bpeNullString;
			return functionReturnValue;
		}
		private void PrintOut(string data)
		{
			if ((dumper != null)) {
				dumper.WriteLine(data);
			} else if (funit != 0) {
				FileSystem.PrintLine(funit, data);
			} else {
				Debug.WriteLine(data);
			}
		}
		public void DumpDataTable()
		{
			StreamWriter dumper = new StreamWriter("DumpDataTable.trace", false);
			dumper.WriteLine(string.Format("Dumping DataRows (mDRV(\"{0}\"):={1})...", mTableIDColumn, mDRV[mTableIDColumn]));
			int i = 0;
			foreach (DataRow dr in tcDataView.Table.Rows) {
				if (dr.RowState != DataRowState.Unchanged || object.ReferenceEquals(dr, mDRV.Row)) {
					object[] args = {
						i,
						dr["ID"],
						(Information.IsDBNull(dr["HullNumber"]) ? "DBNull" : dr["HullNumber"]),
						dr.RowState.ToString()
					};
					dumper.WriteLine((object.ReferenceEquals(dr, mDRV.Row) ? "->" : "  ") + string.Format("{0:00000} RecordID #{1:00000} {2:12} (RowState: {3})", args));
				}
				i += 1;
			}
			dumper.Flush();
			dumper.Close();
			dumper = null;
		}
		public void DumpDataView()
		{
			StreamWriter dumper = new StreamWriter("DumpDataView.trace", false);

			dumper.WriteLine(string.Format("Dumping DataRowViews (mDRV(\"{0}\"):={1})...", mTableIDColumn, (Information.IsDBNull(mDRV[mTableIDColumn]) ? "DBNull" : mDRV[mTableIDColumn])));
			for (int i = 0; i <= tcDataView.Count - 1; i++) {
				DataRowView drv = tcDataView[i];
				object[] args = {
					i,
					(Information.IsDBNull(drv["ID"]) ? "DBNull" : drv["ID"]),
					(Information.IsDBNull(drv["HullNumber"]) ? "DBNull" : drv["HullNumber"]),
					drv.Row.RowState.ToString(),
					drv.RowVersion.ToString(),
					(drv.IsEdit ? " IsEdit; " : ""),
					(drv.IsNew ? " IsNew; " : "")
				};
				string Message = string.Format("      {0:00000} RecordID #{1:00000} {2} (RowState: {3}; RowVersion: {4};{5}{6})", args);
				int iPos = mCurrencyManager.Position;
				string tag = clsSupport.bpeNullString;

				if (!Information.IsDBNull(mDRV[mTableIDColumn])) {
					if (Information.IsDBNull(drv[mTableIDColumn]) && Information.IsDBNull(mDRV[mTableIDColumn])) {
						tag = "mDRV->";
					} else if (!Information.IsDBNull(drv[mTableIDColumn]) && drv[mTableIDColumn] == mDRV[mTableIDColumn]) {
						tag = "mDRV->";
					}
				}
				if (i == iPos)
					tag = (tag == clsSupport.bpeNullString ? "iPos->" : tag.Replace("->", "=>"));
				if (tag != clsSupport.bpeNullString)
					Message = tag + Message.Substring(7);
				if (drv.Row.RowState != DataRowState.Unchanged || tag != clsSupport.bpeNullString)
					dumper.WriteLine(Message);
			}
			dumper.Flush();
			dumper.Close();
			dumper = null;
		}
		public void Dump(DataRow dr, string FileName = null)
		{
			this.DumpDataRow(dr, FileName);
		}
		public void Dump(DataRow dr, DataRowVersion Version, string FileName = null)
		{
			this.DumpDataRow(dr, Version, FileName);
		}
		public void Dump(DataRowView drv, CurrencyManager cm = null, string FileName = null)
		{
			this.DumpDataRowView(drv, cm, FileName);
		}
		public void Dump(DataTable dt, string FileName = null)
		{
			this.DumpDataTable(dt, FileName);
		}
		public void Dump(DataView dv, CurrencyManager cm = null, string FileName = null)
		{
			this.DumpDataView(dv, cm, FileName);
		}
		private void DumpDataTable(DataTable dt, string FileName = clsSupport.bpeNullString)
		{
			int[] ColLen = null;
			try {
				if (FileName != clsSupport.bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((dt == null)) {
                    PrintOut("DataTable is Nothing");
                    throw new ExitTryException();
                }

				ColLen = new int[dt.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dt.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (int mDRV = 0; mDRV <= dt.Rows.Count - 1; mDRV++) {
					DataRow dr = dt.Rows[mDRV];
					for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
						int newLength = FormatData(dt.Columns[iColumn], dr[iColumn]).Length;
						if (newLength > ColLen[iColumn])
							ColLen[iColumn] = newLength;
					}
				}
				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

				PrintOut(string.Format("Dumping DataTable {0} (DefaultView: Count: {1:#,##0})...", dt.TableName, dt.Rows.Count));
				string strOut = "Record   RowState  ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
					string ColumnName = dt.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				for (int iRow = 0; iRow <= dt.Rows.Count - 1; iRow++) {
					DataRow dr = dt.Rows[iRow];
					strOut = string.Format("[{0:000000}] {1}{2} ", iRow, dr.RowState.ToString(), new string(' ', "Unchanged".Length - dr.RowState.ToString().Length));
					for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++) {
						string Data = FormatData(dt.Columns[iColumn], dr[iColumn]);
						strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
					}
					PrintOut(strOut);
				}
				PrintOut(string.Format("{0}{1:#,##0} Records", Constants.vbCr, dt.Rows.Count));
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
                PrintOut(ex.Message);
			} finally {
				if (FileName != clsSupport.bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataRow(DataRow dr, string FileName = clsSupport.bpeNullString)
		{
			this.DumpDataRow(dr, DataRowVersion.Default, FileName);
		}
		private void DumpDataRow(DataRow dr, DataRowVersion Version, string FileName = clsSupport.bpeNullString)
		{
			int[] ColLen = null;
			DataTable dt = null;
			int iRow = -1;
            try
            {
                if (FileName != clsSupport.bpeNullString)
                    dumper = new StreamWriter(FileName, false);
                if ((dr == null))
                {
                    PrintOut("DataRow is Nothing");
                    throw new ExitTryException();
                }
                dt = dr.Table;

                ColLen = new int[dt.Columns.Count + 1];
                //First, size the column names...
                for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++)
                {
                    ColLen[iColumn] = dt.Columns[iColumn].ColumnName.Length;
                }
                //Next, size the each data field...
                for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++)
                {
                    int newLength = FormatData(dt.Columns[iColumn], dr[iColumn]).Length;
                    if (newLength > ColLen[iColumn])
                        ColLen[iColumn] = newLength;
                }
                //Now, calculate the length of each line...
                int TotalLen = 0;
                for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++)
                {
                    TotalLen += ColLen[iColumn] + 1;
                    //Make room for a spacer...
                }

                //Find our row in the collection...
                bool continueLoop = true;
                for (int i = 0; i <= dt.Rows.Count - 1 && continueLoop; i++)
                {
                    DataRow drRow = dt.Rows[i];
                    if (object.ReferenceEquals(drRow, dr)) {
                        iRow = i;
                        continueLoop = false;
                    }
                }

                PrintOut(string.Format("Dumping DataRow {0} ({1:#,##0} of {2:#,##0})...", dt.TableName, iRow, dt.DefaultView.Count));
                PrintOut(string.Format("RowVersion {0}", Version.ToString()));
                string strOut = "Record   RowState  ";
                TotalLen += strOut.Length;
                PrintOut(new string('=', TotalLen));
                //If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
                //PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

                //Column Headers...
                for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++)
                {
                    string ColumnName = dt.Columns[iColumn].ColumnName;
                    strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
                }
                PrintOut(strOut);
                PrintOut(new string('-', TotalLen));

                //Data...
                strOut = string.Format("[{0:000000}] {1}{2} ", iRow, dr.RowState.ToString(), new string(' ', "Unchanged".Length - dr.RowState.ToString().Length));
                for (short iColumn = 0; iColumn <= dt.Columns.Count - 1; iColumn++)
                {
                    string Data = FormatData(dt.Columns[iColumn], dr[iColumn, Version]);
                    strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
                }
                PrintOut(strOut);
            } catch (ExitTryException ex) {
			} catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != clsSupport.bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataRowView(DataRowView drv, CurrencyManager cm = null, string FileName = clsSupport.bpeNullString)
		{
			int[] ColLen = null;
			DataView dv = null;
			int iRow = -1;
			try {
				int iPos = 0;
				if ((cm != null))
					iPos = cm.Position;
				if (FileName != clsSupport.bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((drv == null)) {
                    PrintOut("DataRowView is Nothing");
                    throw new ExitTryException();
                }
				dv = drv.DataView;

				ColLen = new int[dv.Table.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dv.Table.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					int newLength = FormatData(dv.Table.Columns[iColumn], drv[iColumn]).Length;
					if (newLength > ColLen[iColumn])
						ColLen[iColumn] = newLength;
				}

				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

                //Find our row in the collection...
                bool continueLoop = true;
                for (int i = 0; i <= dv.Count - 1 && continueLoop; i++) {
					DataRowView drvRowView = dv[i];
					if (object.ReferenceEquals(drvRowView, drv)) {
                        iRow = i;
                        continueLoop = false;
                    }
                }

				PrintOut(string.Format("Dumping DataRowView {0} ({1:#,##0} of {2:#,##0})...", dv.Table.TableName, iRow, dv.Count));
				PrintOut(string.Format("RowFilter:      {0}{1}", Constants.vbTab, dv.RowFilter));
				PrintOut(string.Format("Sort:           {0}{1}", Constants.vbTab, dv.Sort));
				PrintOut(string.Format("RowStateFilter: {0}{1}", Constants.vbTab, dv.RowStateFilter));
				string strOut = "Record   RowVersion ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string ColumnName = dv.Table.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				string fmt = ((cm == null) ? "[{0:000000}] {1}{2} " : "[->{0:0000}] {1}{2} ");
				strOut = string.Format(fmt, iRow, drv.RowVersion, new string(' ', "RowVersion".Length - drv.RowVersion.ToString().Length));
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string Data = FormatData(dv.Table.Columns[iColumn], drv[iColumn]);
					strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
				}
				PrintOut(string.Format("{0} (RowState: {1}; {2}{3})", new object[] {
					strOut,
					drv.Row.RowState.ToString(),
					(drv.IsEdit ? " IsEdit; " : ""),
					(drv.IsNew ? " IsNew; " : "")
				}));
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
				PrintOut(ex.Message);
			} finally {
				if (FileName != clsSupport.bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		private void DumpDataView(DataView dv, CurrencyManager cm = null, string FileName = clsSupport.bpeNullString)
		{
			int[] ColLen = null;
			try {
				int iPos = 0;
				if ((cm != null))
					iPos = cm.Position;
				if (FileName != clsSupport.bpeNullString)
					dumper = new StreamWriter(FileName, false);
				if ((dv == null)) {
                    PrintOut("DataView is Nothing");
                    throw new ExitTryException();
                }

                ColLen = new int[dv.Table.Columns.Count + 1];
				//First, size the column names...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					ColLen[iColumn] = dv.Table.Columns[iColumn].ColumnName.Length;
				}
				//Next, size the each data field...
				for (int iRow = 0; iRow <= dv.Count - 1; iRow++) {
					DataRowView drv = dv[iRow];
					for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
						int newLength = FormatData(dv.Table.Columns[iColumn], drv[iColumn]).Length;
						if (newLength > ColLen[iColumn])
							ColLen[iColumn] = newLength;
					}
				}
				//Now, calculate the length of each line...
				int TotalLen = 0;
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					TotalLen += ColLen[iColumn] + 1;
					//Make room for a spacer...
				}

				PrintOut(string.Format("Dumping DataView {0} {1:#,##0} Records...", dv.Table.TableName, dv.Count));
				PrintOut(string.Format("RowFilter:      {0}{1}", Constants.vbTab, dv.RowFilter));
				PrintOut(string.Format("Sort:           {0}{1}", Constants.vbTab, dv.Sort));
				PrintOut(string.Format("RowStateFilter: {0}{1}", Constants.vbTab, dv.RowStateFilter));
				string strOut = "Record   RowVersion ";
				TotalLen += strOut.Length;
				PrintOut(new string('=', TotalLen));
				//If vRS.Filter <> 0 Then PrintOut("Filter: " & vRS.Filter)
				//PrintOut("Record #" & BookMark & " (bookmark) of " & vRS.RecordCount & " records...")

				//Column Headers...
				for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
					string ColumnName = dv.Table.Columns[iColumn].ColumnName;
					strOut += ColumnName + new string(' ', ColLen[iColumn] - ColumnName.Length) + " ";
				}
				PrintOut(strOut);
				PrintOut(new string('-', TotalLen));

				//Data...
				for (int iRow = 0; iRow <= dv.Count - 1; iRow++) {
					DataRowView drv = dv[iRow];
					string fmt = ((cm == null) ? "[{0:000000}] {1}{2} " : "[->{0:0000}] {1}{2} ");
					strOut = string.Format(fmt, iRow, drv.RowVersion, new string(' ', "RowVersion".Length - drv.RowVersion.ToString().Length));
					for (short iColumn = 0; iColumn <= dv.Table.Columns.Count - 1; iColumn++) {
						string Data = FormatData(dv.Table.Columns[iColumn], drv[iColumn]);
						strOut += Data + new string(' ', ColLen[iColumn] - Data.Length) + " ";
					}
					PrintOut(string.Format("{0} (RowState: {1}; {2}{3})", new object[] {
						strOut,
						drv.Row.RowState.ToString(),
						(drv.IsEdit ? " IsEdit; " : ""),
						(drv.IsNew ? " IsNew; " : "")
					}));
				}
				PrintOut(string.Format("{0}{1:#,##0} Records", Constants.vbCr, dv.Count));
            } catch (ExitTryException ex) {
            } catch (Exception ex) {
                PrintOut(ex.Message);
			} finally {
				if (FileName != clsSupport.bpeNullString){dumper.Flush();dumper.Close();dumper = null;}
			}
		}
		#endregion
		#endregion
		#region "Event Handlers"
		private void tcDataView_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (ListChanged != null) {
				ListChanged(sender, e);
			}
		}
		static bool InitStaticVariableHelper(Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag flag)
		{
			if (flag.State == 0) {
				flag.State = 2;
				return true;
			} else if (flag.State == 2) {
				throw new Microsoft.VisualBasic.CompilerServices.IncompleteInitialization();
			} else {
				return false;
			}
		}
		#endregion
	}
}
	#region "Helper Classes"
	#region "EventArgs Classes"
namespace TCBase
{
	public class ActionModeChangeEventArgs : EventArgs
	{
		private clsTCBase.ActionModeEnum mOldMode = 0;
		private clsTCBase.ActionModeEnum mNewMode = 0;
		public ActionModeChangeEventArgs(clsTCBase.ActionModeEnum oldMode, clsTCBase.ActionModeEnum newMode) : base()
		{
			mOldMode = oldMode;
			mNewMode = newMode;
		}
		public clsTCBase.ActionModeEnum newMode {
			get { return mNewMode; }
		}
		public clsTCBase.ActionModeEnum oldMode {
			get { return mOldMode; }
		}
	}
}
namespace TCBase
{
	public class DataEventArgs : EventArgs
	{
		private DataRowView mRowView = null;
		public DataEventArgs(DataRowView drv) : base()
		{
			mRowView = drv;
		}
		public DataRowView RowView {
			get { return mRowView; }
		}
	}
}
namespace TCBase
{
	public class FilterChangeEventArgs : EventArgs
	{
		private string mOldFilter = null;
		private string mNewFilter = null;
		public FilterChangeEventArgs(string oldFilter, string newFilter) : base()
		{
			mOldFilter = oldFilter;
			mNewFilter = newFilter;
		}
		public string OldFilter {
			get { return mOldFilter; }
		}
		public string NewFilter {
			get { return mNewFilter; }
		}
	}
}
namespace TCBase
{
    public class SortChangeEventArgs : EventArgs
    {
        private string mOldSort = null;
        private string mNewSort = null;
        public SortChangeEventArgs(string oldSort, string newSort) : base()
        {
            mOldSort = oldSort;
            mNewSort = newSort;
        }
        public string OldSort
        {
            get { return mOldSort; }
        }
        public string NewSort
        {
            get { return mNewSort; }
        }
    }
}
namespace TCBase
{
	public class RowChangeEventArgs : DataEventArgs
	{
		private int mNewRowIndex = 0;
		private int mOldRowIndex = 0;
		private int mTotalRows = 0;
		public RowChangeEventArgs(int OldRowIndex, int NewRowIndex, DataRowView Row, int TotalRows) : base(Row)
		{
			mOldRowIndex = OldRowIndex;
			mNewRowIndex = NewRowIndex;
			mTotalRows = TotalRows;
		}
		public int RowIndex {
			get { return mNewRowIndex; }
		}
		public int OldRowIndex {
			get { return mOldRowIndex; }
		}
		public int TotalRows {
			get { return mTotalRows; }
		}
	}
}
namespace TCBase
{
	public class SplashEventArgs : EventArgs
	{
		private string mMessage = null;
		private Icon mIconImage = null;
		public SplashEventArgs(string Message, Icon IconImage) : base()
		{
			mMessage = Message;
			mIconImage = IconImage;
		}
		public string Message {
			get { return mMessage; }
		}
		public Icon IconImage {
			get { return mIconImage; }
		}
	}
}
	#endregion
	#region "Exception Classes"
namespace TCBase
{
	public class ExitTryException : Exception
	{
	}
}
#endregion
#endregion
