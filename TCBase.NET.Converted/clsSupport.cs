//clsSupport.vb
//   TreasureChest2 Support Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//   Modification History:
//   Date:       Description:
//   06/04/17    Introduced LogFile property;
//   10/21/16    Used Collection.Contains method to avoid ArguementExceptions;
//   12/04/14    Added ThreadName to SetupNewThread;
//   12/12/05    Added a CleanupThread() method to reclaim memory for finished threads within our framework;
//   12/02/05    Extended Strings property to be a collection clsString objects by ThreadID instead of a common object due to 
//               multi-threaded coordination issues (static variables in ParseStr() designed to be called to iteratively parse 
//               through a string - preemption from other threads caused an interruption in this logic which manifested itself as 
//               strange errors in routines like clsCommandLine.ParseCommandLine and clsDBEngine.SQLApplyAsClause);
//   09/19/05    Added LogMaxSizeMB and LogRetentionDays properties;
//   07/07/05    Added EntryComponent, and ExecutingComponent;
//               An attempt was made to implement a CallingComponent, but the way the .NET framework
//               manages Assemblies through Reflection.Assembly.GetCallingAssembly does not seem to 
//               support working our way back up the stack;
//   05/17/05    Added AssemblyInfo class to AssemblyInfo.vb and added appropriate references here to 
//               leverage these new definitions;
//   04/06/05    Added Multi-Thread support for CallStack property;
//               Eliminated LastError and related methods as these were relocated into clsErrors;
//   02/21/05    Created;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
#define UseFileInfo
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
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
	public class clsSupport : IDisposable
	{
		public clsSupport(System.Reflection.Assembly objAssembly = null, string OverrideApplicationName = bpeNullString, string TracePath = bpeNullString, clsTrace.trcOption toTraceOptions = trcOption.trcNone, bool bTraceMode = false) : base()
		{
			const string EntryName = "New";
			System.Diagnostics.Process enterProcess = null;
			//Try
			if ((TracePath == null))
				TracePath = bpeNullString;
			string strAssembly = null;
			if ((objAssembly == null))
				strAssembly = "Nothing";
			else
				strAssembly = "{" + objAssembly.GetType().ToString() + "}";
			string Arguments = string.Format("objAssembly:={0},OverrideApplicationName:=\"{1}\",TracePath:=\"{2}\",toTraceOptions:={3},TraceMode:={4}", new object[] {
				strAssembly,
				OverrideApplicationName,
				TracePath,
				toTraceOptions.ToString(),
				bTraceMode.ToString()
			});
			mOverrideApplicationName = OverrideApplicationName;
			mThread = System.Threading.Thread.CurrentThread;
			mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
			mAssemblyInfo = new AssemblyInfo(objAssembly);
			mTraceID = mAssemblyInfo.Product + "." + mAssemblyInfo.Name + "." + ModuleName;
			mTrace = new clsTrace(this, TracePath, toTraceOptions, bTraceMode);
			//Must be first so we can trace the rest...

			mRegionalTimeFormat = DateTimeFormatInfo.CurrentInfo.LongTimePattern;
			//GetRegionalSetting(Win.WinNLS.LOCALE_STIMEFORMAT)
			//Changing from tt to AMPM screws up grid formatting relying on fmrShortDateTime...
			//If Right(strRegionalTimeFormat, 3) = " tt" Then strRegionalTimeFormat = Mid(strRegionalTimeFormat, 1, Len(strRegionalTimeFormat) - 2) + "AMPM"
			mShortDate = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			//GetRegionalSetting(Win.WinNLS.LOCALE_SSHORTDATE)
			mLongDate = DateTimeFormatInfo.CurrentInfo.LongDatePattern;
			//GetRegionalSetting(Win.WinNLS.LOCALE_SLONGDATE)

			string strMessage = mTraceID + "." + EntryName + "(" + Arguments + ")";
			if ((mTrace.TraceOptions & trcOption.trcMemory) == trcOption.trcMemory)
				enterProcess = Process.GetCurrentProcess();
			mTrace.Trace(trcType.trcEnter, strMessage, trcOption.trcSupport);

			//mErrors = New clsErrors(Me)
			//'Force an allocation of a new clsError object for this thread...
			//Dim i As Integer = mErrors.LastError(Threading.Thread.CurrentThread.ManagedThreadId).Number
			//'Allocate a CallStack for this (main) thread... Others will be created as necessary in SetupNewThread()...
			//mCallStackCollection.Add(New clsCallStack(Me), CStr(Threading.Thread.CurrentThread.ManagedThreadId))
			//'Allocate a String for this (main) thread... Others will be created as necessary in SetupNewThread()...
			//mStringCollection.Add(New clsString(Me), CStr(Threading.Thread.CurrentThread.ManagedThreadId))

			//Do more stuff, if necessary...
			//Catch ex As Exception
			//    If Not IsNothing(mErrors) Then mErrors.RaiseError(Threading.Thread.CurrentThread.ManagedThreadId, mTraceID, EntryName, ex, Nothing) Else Throw ex
			//End Try
			if ((mTrace != null))
				mTrace.Trace(trcType.trcExit, mTraceID + "." + EntryName, trcOption.trcSupport | trcOption.trcMemory, enterProcess);
			enterProcess = null;
			mLogFile = string.Format("{0}\\{1}.log", this.ApplicationPath, this.ApplicationName);
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Trace Support"
		public string ModuleName {
			get { return "clsSupport"; }
		}
		public string TraceID {
			get { return mTraceID; }
		}
			#endregion
		private string mTraceID;
		#region "Enumerations"
		public enum ByteEnum : long
		{
			KB = 1024,
			MB = KB * 1024,
			GB = MB * 1024,
			TB = GB * 1024
		}
		public enum ParseParts
		{
			DrvOnly = 1,
			DirOnly = 2,
			DirOnlyNoSlash = -2,
			DrvDir = 3,
			DrvDirNoSlash = -3,
			FileNameBase = 4,
			FileNameExt = 5,
			FileNameExtNoDot = -5,
			FileNameBaseExt = 6,
			DrvDirFileNameBase = 7,
			Protocol = 8,
			ServerOnly = 9,
			ServerShare = 10,
			ShareOnly = 11
		}
		public enum RunModeEnum
		{
			Console = 0,
			WindowsForms = 1,
			Scheduler = 2
		}
		public enum vbRGBColorConstants : UInt32
		{
			vbUndefinedColor = 0xffffffff,
			//Supplemented VB Color Constants
			vbMaroon = 0x80,
			//RGB(128, 0, 0)
			vbGreen = 0x8000,
			//RGB(0, 128, 0)
			vbOlive = 0x8080,
			//RGB(128, 128, 0)
			vbNavy = 0x800000,
			//RGB(0, 0, 128)
			vbPurple = 0x800080,
			//RGB(128, 0, 128)
			vbTeal = 0x808000,
			//RGB(0, 128, 128)
			vbGray = 0x808080,
			//RGB(128, 128, 128)
			vbRed = 0xff,
			//RGB(255, 0, 0)
			vbLime = 0xff00,
			//RGB(0, 255, 0)
			vbYellow = 0xffff,
			//RGB(255, 255, 0)
			vbBlue = 0xff0000,
			//RGB(0, 0, 255)
			vbMagenta = 0xff00ff,
			//RGB(255, 0, 255)
			vbCyan = 0xffff00,
			//RGB(0, 255, 255)
			vbSilver = 0xc0c0c0,
			//RGB(192, 192, 192)
			vbWhite = 0xffffff,
			//RGB(255, 255, 255)
			vbBlack = 0x0,
			//RGB(0, 0, 0)
			//System Color Constants
			vbScrollBars = 0x80000000,
			//Scroll bar color
			vbDesktop = 0x80000001,
			//Desktop color
			vbActiveTitleBar = 0x80000002,
			//Color of the title bar for the active window
			vbInactiveTitleBar = 0x80000003,
			//Color of the title bar for the inactive window
			vbMenuBar = 0x80000004,
			//Menu background color
			vbWindowBackground = 0x80000005,
			//Window background color
			vbWindowFrame = 0x80000006,
			//Window frame color
			vbMenuText = 0x80000007,
			//Color of text on menus
			vbWindowText = 0x80000008,
			//Color of text in windows
			vbTitleBarText = 0x80000009,
			//Color of text in caption, size box, and scroll arrow
			vbActiveBorder = 0x8000000a,
			//Border color of active window
			vbInactiveBorder = 0x8000000b,
			//Border color of inactive window
			vbApplicationWorkspace = 0x8000000c,
			//Background color of multiple-document interface (MDI) applications
			vbHighlight = 0x8000000d,
			//Background color of items selected in a control
			vbHighlightText = 0x8000000e,
			//Text color of items selected in a control
			vbButtonFace = 0x8000000f,
			//Color of shading on the face of command buttons
			vbButtonShadow = 0x80000010,
			//Color of shading on the edge of command buttons
			vbGrayText = 0x80000011,
			//Grayed (disabled) text
			vbButtonText = 0x80000012,
			//Text color on push buttons
			vbInactiveCaptionText = 0x80000013,
			//Color of text in an inactive caption
			vb3DHighlight = 0x80000014,
			//Highlight color for 3D display elements
			vb3DDKShadow = 0x80000015,
			//Darkest shadow color for 3D display elements
			vb3DLight = 0x80000016,
			//Second lightest of the 3D colors after vb3Dhighlight
			vb3DFace = 0x8000000f,
			//Color of text face
			vb3DShadow = 0x80000010,
			//Color of text shadow
			vbInfoText = 0x80000017,
			//Color of text in ToolTips
			vbInfoBackground = 0x80000018
			//Background color of ToolTips
		}
		#endregion
		#region "Declarations"
		public const string bpeNullString = "";
		private string mApplicationName;
		private string mApplicationPath;
		private AssemblyInfo mAssemblyInfo;
        //Private mBytes As clsBytes
        //private Dictionary<string,clsCallStack> mCallStackCollection = new Dictionary<string, clsCallStack>();
		private Dictionary<string, clsString> mStringCollection = new Dictionary<string, clsString>();
		//Private mDates As clsDates
		//Private mErrors As clsErrors
		private clsFieldEdits mFieldEdits;
		//Private mFileSystem As clsFileSystem
		private string mLogFile;
		private int mLogMaxSizeMB = 10;
		private int mLogRetentionDays = 30;
		private string mLongDate;
		private string mOverrideApplicationName;
		private string mRegionalTimeFormat;
		private clsRegistry mRegistry;
		//Private mSecurity As clsSecurity
		private string mShortDate;
		//Private mSystem As clsSystem
		private System.Threading.Thread mThread;
		private int mThreadID = -1;
		private clsTrace mTrace;
		private clsUI mUI;
		//Private mWin32Error As clsWin32Error
		private object FieldEditsLock = new object();
		private object RegistryLock = new object();
		private object StringLock = new object();
			#endregion
		private object UILock = new object();
		public string ApplicationName {
			get {
				if (mOverrideApplicationName != bpeNullString)
					return mOverrideApplicationName;
				return this.EntryComponentName;
			}
		}
		public string ApplicationPath {
			get {
				System.Reflection.Assembly EntryAssembly = System.Reflection.Assembly.GetEntryAssembly();
				if ((EntryAssembly == null))
					return bpeNullString;
				string path = EntryAssembly.Location;
				//FileInfo: "URI formats are not supported."
				if (ParsePath(path, ParseParts.Protocol) == "file://")
					path = path.Substring("file:///".Length);
				return new FileInfo(path).DirectoryName;
			}
		}
		public string ApplicationVersion {
			get {
				System.Reflection.Assembly EntryAssembly = System.Reflection.Assembly.GetEntryAssembly();
				if ((EntryAssembly == null))
					return bpeNullString;
				//TODO: Revisit
				return EntryAssembly.GetName().Version.ToString();
			}
		}
		//Public ReadOnly Property Dates(Optional ByVal AutoAllocate As Boolean = True) As clsDates
		//    Get
		//        SyncLock GetType(clsDates)
		//            If IsNothing(mDates) AndAlso AutoAllocate Then mDates = New clsDates(Me) 'Must have Strings instance...
		//            Return mDates
		//        End SyncLock
		//    End Get
		//End Property
		public string EntryComponentName {
			get {
				System.Reflection.Assembly EntryAssembly = System.Reflection.Assembly.GetEntryAssembly();
				if ((EntryAssembly == null))
					return "";
				return EntryAssembly.GetName().Name.ToString();
			}
		}
		//Public ReadOnly Property Errors(Optional ByVal AutoAllocate As Boolean = True) As clsErrors
		//    Get
		//        SyncLock GetType(clsErrors)
		//            If IsNothing(mErrors) AndAlso AutoAllocate Then mErrors = New clsErrors(Me)
		//            Return mErrors
		//        End SyncLock
		//    End Get
		//End Property
		public string ExecutingComponentName {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString(); }
		}
		public clsFieldEdits FieldEdits {
			get {
				lock (FieldEditsLock) {
					if (mFieldEdits == null)
						mFieldEdits = new clsFieldEdits(this);
					return mFieldEdits;
				}
			}
		}
		//Public ReadOnly Property FileSystem(Optional ByVal AutoAllocate As Boolean = True) As clsFileSystem
		//    Get
		//        SyncLock GetType(clsFileSystem)
		//            If IsNothing(mFileSystem) AndAlso AutoAllocate Then mFileSystem = New clsFileSystem(Me)
		//            Return mFileSystem
		//        End SyncLock
		//    End Get
		//End Property
		public string fmtFormatDateTime {
			get { return fmtShortDateTime; }
		}
		public string fmtLongDate {
			get { return mLongDate; }
		}
		public string fmtLongDateTime {
			get { return string.Format("{0} {1}", fmtLongDateYYYY, fmtTime); }
		}
		public string fmtLongDateYYYY {
			get {
				string functionReturnValue = null;
				functionReturnValue = mLongDate;
				if (functionReturnValue.ToLower().IndexOf("yyyy") == -1)
					functionReturnValue = functionReturnValue.Replace("yy", "yyyy");
				return functionReturnValue;
			}
		}
		public string fmtShortDate {
			get { return mShortDate; }
		}
		public string fmtShortDateTime {
			get { return string.Format("{0} {1}", fmtShortDateYYYY, fmtTime); }
		}
		public string fmtShortDateYYYY {
			get {
				string functionReturnValue = null;
				functionReturnValue = mShortDate;
				if (functionReturnValue.ToLower().IndexOf("yyyy") == -1)
					functionReturnValue = functionReturnValue.Replace("yy", "yyyy");
				return functionReturnValue;
			}
		}
		public string fmtTime {
			get { return mRegionalTimeFormat; }
		}
		public string LogFile {
			get { return mLogFile; }
			set { mLogFile = value; }
		}
		public int LogMaxSizeMB {
			get { return mLogMaxSizeMB; }
			set {
				if (value > 30)
					throw new ArgumentException("LogMaxSizeMB cannot be larger than 30MB or file will not be able to be edited using available Windows tools.");
				mLogMaxSizeMB = value;
			}
		}
		public int LogRetentionDays {
			get { return mLogRetentionDays; }
			set { mLogRetentionDays = value; }
		}
		public string OverrideApplicationName {
			get { return mOverrideApplicationName; }
		}
        private int ThreadID
        {
            get { return mThreadID; }
            set { mThreadID = value; }
        }
        public clsRegistry Registry {
			get {
				lock (RegistryLock) {
					if (mRegistry == null)
						mRegistry = new clsRegistry(this);
					return mRegistry;
				}
			}
		}
		//Public ReadOnly Property Security(Optional ByVal AutoAllocate As Boolean = True) As clsSecurity
		//    Get
		//        SyncLock GetType(clsSecurity)
		//            If IsNothing(mSecurity) AndAlso AutoAllocate Then mSecurity = New clsSecurity(Me)
		//            Return mSecurity
		//        End SyncLock
		//    End Get
		//End Property
		public clsString Strings {
			get {
				clsString functionReturnValue = null;
				const string EntryName = "get_Strings";
				string strMessage = bpeNullString;
				functionReturnValue = null;
				lock (StringLock) {
					try {
						if ((mTrace != null)) {
							if (mTrace.TraceOptions == trcOption.trcAll) {
								strMessage = mTraceID + "." + EntryName + "(ThreadID:=" + ThreadID.ToString() + ")";
								mTrace.Trace(trcType.trcEnter, strMessage, trcOption.trcAll);
							}
						}
						if (!mStringCollection.ContainsKey(ThreadID.ToString())) {
							//If we don't have a String for this thread, create one...
							if ((mTrace != null) && mTrace.TraceOptions == trcOption.trcAll)
								mTrace.Trace(trcType.trcBody, "Creating New clsString for ThreadID:=" + ThreadID.ToString(), trcOption.trcAll);
							functionReturnValue = new clsString(this);
							mStringCollection.Add(ThreadID.ToString(), functionReturnValue);
						}
						functionReturnValue = mStringCollection[ThreadID.ToString()];
					} catch (System.ArgumentException ex) {
						//If we don't have a String for this thread, create one...
						if ((mTrace != null) && mTrace.TraceOptions == trcOption.trcAll)
							mTrace.Trace(trcType.trcBody, "Creating New clsString for ThreadID:=" + ThreadID.ToString(), trcOption.trcAll);
						functionReturnValue = new clsString(this);
                        mStringCollection.Add(ThreadID.ToString(), functionReturnValue);
                    }
                    finally {
						if ((mTrace != null)) {
							if (mTrace.TraceOptions == trcOption.trcAll) {
								strMessage = mTraceID + "." + EntryName;
								strMessage += Constants.vbTab + "[String Collection contains " + mStringCollection.Count.ToString() + " entries]";
								mTrace.Trace(trcType.trcExit, strMessage, trcOption.trcAll);
							}
						}
					}
				}
				return functionReturnValue;
			}
		}
		//Public ReadOnly Property System(Optional ByVal AutoAllocate As Boolean = True) As clsSystem
		//    Get
		//        SyncLock GetType(clsSystem)
		//            If IsNothing(mSystem) AndAlso AutoAllocate Then mSystem = New clsSystem(Me)
		//            Return mSystem
		//        End SyncLock
		//    End Get
		//End Property
		public clsTrace Trace {
			get { return mTrace; }
		}
		public clsUI UI {
			get {
				lock (UILock) {
					if (mUI == null)
						mUI = new clsUI(this);
					return mUI;
				}
			}
		}
		//Public ReadOnly Property Win32Error(Optional ByVal AutoAllocate As Boolean = True) As clsWin32Error
		//    Get
		//        SyncLock GetType(clsWin32Error)
		//            If IsNothing(mWin32Error) AndAlso AutoAllocate Then mWin32Error = New clsWin32Error(Me)
		//            Return mWin32Error
		//        End SyncLock
		//    End Get
		//End Property
		#endregion
		#region "Methods"
		#region "Destructor"
		private bool bDisposed = false;
		public void Dispose()
		{
			try {
				//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "]" & vbTab & TraceID & ".Dispose()")
				Dispose(true);
				//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "] " & TraceID & ".Dispose(): GC.SuppressingFinalize(Me)")
				GC.SuppressFinalize(this);
			} catch (Exception ex) {
				throw;
			}
		}
		protected void Dispose(bool Disposing)
		{
			try {
				if (bDisposed)
					return;
				if (Disposing) {
					//Object is being disposed, not finalized.
					//It is safe to access other objects (other than the base object) only from inside this block.
					mTrace.Trace("Disposing: " + TraceID, trcOption.trcMemory);
					//If Not IsNothing(mBytes) Then mBytes.Dispose() : mBytes = Nothing
					//If Not IsNothing(mDates) Then mDates.Dispose() : mDates = Nothing
					//If Not IsNothing(mErrors) Then mErrors.Dispose() : mErrors = Nothing
					if ((mFieldEdits != null)){mFieldEdits.Dispose();mFieldEdits = null;}
					//If Not IsNothing(mFileSystem) Then mFileSystem.Dispose() : mFileSystem = Nothing
					if ((mRegistry != null)){mRegistry.Dispose();mRegistry = null;}
					//If Not IsNothing(mSecurity) Then mSecurity.Dispose() : mSecurity = Nothing
					//If Not IsNothing(mSystem) Then mSystem.Dispose() : mSystem = Nothing
					if ((mUI != null)){mUI.Dispose();mUI = null;}
					//If Not IsNothing(mWin32Error) Then mWin32Error.Dispose() : mWin32Error = Nothing

					//For Each iCallStack As clsCallStack In mCallStackCollection
					//    mCallStackCollection.Remove(1)
					//    iCallStack.Dispose() : iCallStack = Nothing
					//Next iCallStack
					//mCallStackCollection = null;
					//For Each iString As clsString In mStringCollection
					//    mStringCollection.Remove(1)
					//    iString.Dispose() : iString = Nothing
					//Next iString
					mStringCollection = null;

					mTrace.Dispose();
					mTrace = null;
					//Must be last so the rest can be traced...
					//Console.WriteLine(String.Format("Exiting {0}.Dispose{1}{2}", TraceID, vbTab, MemoryStats(GC.GetTotalMemory(True), Process.GetCurrentProcess())))
				}
			} finally {
				//MyBase.Dispose(Disposing)
				bDisposed = true;
			}
		}
		#endregion
		#region "FileSystem"
		#if UseSHGetFileInfo
		public enum IconSize
		{
			LargeIcon = 0,
			SmallIcon = 1
		}
		protected struct SHFILEINFO
		{
				// : icon
			public IntPtr hIcon;
				// : icondex
			public int iIcon;
				// : SFGAO_ flags
			public int dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}
		[DllImport("shell32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);
		public System.Drawing.Icon Icon(string FilePath, IconSize Size)
		{
			System.Drawing.Icon functionReturnValue = null;
			const int SHGFI_ICON = 0x100;
			const int SHGFI_SMALLICON = 0x1;
			const int SHGFI_LARGEICON = 0x0;
			SHFILEINFO shinfo = default(SHFILEINFO);
			IntPtr hIcon = default(IntPtr);
			//The handle to the system image list.
			int iSize = 0;

			shinfo = new SHFILEINFO();
			shinfo.szDisplayName = new string(Strings.Chr(0), 260);
			shinfo.szTypeName = new string(Strings.Chr(0), 80);
			if (Size == IconSize.LargeIcon)
				iSize = SHGFI_LARGEICON;
			else
				iSize = SHGFI_SMALLICON;
			hIcon = SHGetFileInfo(FilePath, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | iSize);
			functionReturnValue = System.Drawing.Icon.FromHandle(shinfo.hIcon);
			//The icon is returned in the hIcon member of the shinfo struct.
			shinfo = null;
			return functionReturnValue;
		}
		#endif
		public System.Drawing.Icon Icon(string FilePath)
		{
			return System.Drawing.Icon.ExtractAssociatedIcon(FilePath);
		}
        public string ParsePath(string strPath, ParseParts intPart)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			if ((strPath == null))
				strPath = bpeNullString;
			if (strPath.Trim() == bpeNullString)
				return functionReturnValue;
			#if UseFileInfo
			FileInfo fi = null;
			string Protocol = bpeNullString;
			string Drv = bpeNullString;
			string ServerShare = bpeNullString;
			string ServerOnly = bpeNullString;
			string ShareOnly = bpeNullString;
			bool WildCardFound = false;
			bool isPath = true;
			bool isUNC = false;
			bool isURI = false;
			try {
				//First check for URI specification...
				if (strPath.ToLower().StartsWith("file://")) {
					isURI = true;
					Protocol = "file://";
					if (intPart == ParseParts.Protocol)
						return Protocol;
					//Path will be in any of the following formats:
					//   file:///S:/FiRRe/... (note the extra "/" before the drive letter)...
					//   file://\\WSRV08/SunGard/FiRRe/...
					//   file://WWS004/SunGard/FiRRe/...
					if (strPath.ToLower().StartsWith("file:///") && strPath.Substring(9, 1) == ":") {
						strPath = strPath.Substring("file:///".Length).Replace("/", "\\");
					} else if (strPath.ToLower().StartsWith("file://\\\\")) {
						strPath = strPath.Substring("file://".Length).Replace("/", "\\");
					} else if (strPath.ToLower().StartsWith("file://") && strPath.IndexOf(":", "file://".Length) == -1) {
						strPath = strPath.Substring("file:".Length).Replace("/", "\\");
					} else {
						throw new NotSupportedException(string.Format("FILE protocol pattern ({0}) not recognized by ParsePath", strPath));
					}
				} else if (strPath.ToLower().StartsWith("http://")) {
					isURI = true;
					throw new NotSupportedException(string.Format("HTTP protocol pattern ({0}) not supported by ParsePath", strPath));
				} else if (strPath.ToLower().StartsWith("https://")) {
					isURI = true;
					throw new NotSupportedException(string.Format("HTTPS protocol pattern ({0}) not supported by ParsePath", strPath));
				}

				//OK, not that any file:// has been stripped off, see if we have a UNC or traditional pathname provided...
				if (strPath.StartsWith("\\\\")) {
					isUNC = true;
					ServerShare = strPath.Substring(2, strPath.IndexOf("\\", strPath.IndexOf("\\", 2) + 1) - 2);
					ServerOnly = ServerShare.Substring(0, ServerShare.IndexOf("\\"));
					ShareOnly = ServerShare.Substring(ServerShare.IndexOf("\\") + 1);
				} else {
					if (strPath.Length < "C:\\".Length)
						throw new NotSupportedException(string.Format("Invalid path ({0})", strPath));
					if (strPath.Substring(1, 1) == ":") {
						Drv = strPath.Substring(0, 2);
					//We weren't given a path at all, but a simple filename...
					} else {
						isPath = false;
					}
				}
				//If our would-be path contains any wild-cards, FileInfo will throw an exception, so deal with that here...
				if (strPath.IndexOf("*") > -1){WildCardFound = true;strPath = strPath.Replace("*", "XXXXXXXX");}

				fi = new FileInfo(strPath);
				switch (intPart) {
					case ParseParts.DrvOnly:
						return Drv;
					case ParseParts.ServerShare:
						return ServerShare;
					case ParseParts.ServerOnly:
						return ServerOnly;
					case ParseParts.ShareOnly:
						return ShareOnly;
					case ParseParts.DirOnly:
						if (!isPath)
							return bpeNullString;
						if (isUNC)
							return string.Format("{0}\\", fi.DirectoryName.Substring("\\\\".Length + ServerShare.Length));
						return string.Format("{0}\\", fi.DirectoryName.Substring(Drv.Length));
					case ParseParts.DirOnlyNoSlash:
						if (!isPath)
							return bpeNullString;
						if (isUNC)
							return fi.DirectoryName.Substring("\\\\".Length + ServerShare.Length);
						return fi.DirectoryName.Substring(Drv.Length);
					case ParseParts.DrvDir:
						if (isPath)
							return string.Format("{0}\\", fi.DirectoryName);
						else
							return bpeNullString;
						break;
					case ParseParts.DrvDirNoSlash:
						if (isPath)
							return fi.DirectoryName;
						else
							return bpeNullString;
						break;
					case ParseParts.DrvDirFileNameBase:
						if (isPath)
							return fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length);
						else
							return bpeNullString;
						break;
					case ParseParts.FileNameBaseExt:
						return fi.Name;
					case ParseParts.FileNameBase:
						return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
					case ParseParts.FileNameExt:
						return fi.Extension;
					case ParseParts.FileNameExtNoDot:
						return fi.Extension.Substring(1);
				}
			} finally {
				if (WildCardFound)
					functionReturnValue = functionReturnValue.Replace("XXXXXXXX", "*");
				fi = null;
			}
			#else
			short intCPos = 0;
			short intLPos = 0;
			short intTemp = 0;
			short intPathStart = 0;
			short intPathLen = 0;
			string strPart1 = bpeNullString;
			string strPart2 = bpeNullString;
			string strPart4 = bpeNullString;
			string strPart5 = bpeNullString;
			functionReturnValue = bpeNullString;

			intPathLen = strPath.Length;

			//Get the Protocol (if any)...
			intCPos = strPath.IndexOf("://");
			if (intCPos > 0) {
				if (intPart == clsFileSystem.ParseParts.Protocol){strPath = Strings.Left(strPath, intCPos + "://".Length);return functionReturnValue;}
				//If we don't want the Protocol, strip it off so it won't interfere with any logic below...
				switch (Strings.LCase(ParseStr(strPath, 1, "://"))) {
					case "file":
						//Path will be in any of the following formats:
						//   file:///S:/FiRRe/... (note the extra "/" before the drive letter)...
						//   file://\\WSRV08/SunGard/FiRRe/...
						//   file://WWS004/SunGard/FiRRe/...
						if (strPath.ToLower().StartsWith("file:///") && strPath.Substring(9, 1) == ":") {
							strPath = strPath.Substring("file:///".Length).Replace("/", "\\");
						} else if (strPath.ToLower().StartsWith("file://\\\\")) {
							strPath = strPath.Substring("file://".Length).Replace("/", "\\");
						} else if (strPath.ToLower().StartsWith("file://") && strPath.IndexOf(":", "file://".Length) == -1) {
							strPath = strPath.Substring("file:".Length).Replace("/", "\\");
						} else {
							throw new NotSupportedException(string.Format("FILE protocol pattern ({0}) not not recognized by ParsePath", strPath));
						}
						intPathLen = strPath.Length;
						break;
					case "http":
						throw new NotSupportedException("HTTP protocol not currently supported by ParsePath");
				}
			}

			//Get drive portion.
			intCPos = Strings.InStr(strPath, ":");
			if (intCPos)
				strPart1 = Strings.Left(strPath, intCPos);

			//Get path portion.
			intLPos = Strings.InStr(1, strPath, "\\");
			if (Strings.Right(strPath, 1) == "\\") {
				//strPath contains no filename.
				if (intPathLen > intLPos) {
					if (intPart < 0) {
						strPart2 = Strings.Mid(strPath, intLPos, intPathLen - intLPos);
					} else {
						strPart2 = Strings.Mid(strPath, intLPos);
					}
				} else {
					strPart2 = "\\";
				}

			} else {
				if (intLPos) {
					//strPath must contain a filename.
					intPathStart = intLPos;
					intLPos = intLPos + 1;

					do {
						intCPos = Strings.InStr(intLPos, strPath, "\\");
						if (intCPos) {
							intLPos = intCPos + 1;
						}
					} while (intCPos);

					if (intPart < 0) {
						strPart2 = Strings.Mid(strPath, intPathStart, intLPos - intPathStart - 1);
					} else {
						strPart2 = Strings.Mid(strPath, intPathStart, intLPos - intPathStart);
					}
				} else {
					//No path was found.
					if (Strings.Len(strPart1)) {
						//If drive spec, start at position 3 when getting filename portion.
						intLPos = 3;
					} else {
						intLPos = 1;
					}
				}

				strPart4 = Strings.Mid(strPath, intLPos);

				//Check if filename base has extension.
				intCPos = 1;
				do {
					intTemp = Strings.InStr(intCPos + 1, strPart4, ".");
					if (intTemp)
						intCPos = intTemp;
				} while (intTemp);

				if (intCPos > 1) {
					//Get filename extension portion.
					// Check if it's "negative".
					if (Strings.InStr(Convert.ToString(intPart), "-")) {
						strPart5 = Strings.Mid(strPart4, intCPos + 1);
					} else {
						strPart5 = Strings.Mid(strPart4, intCPos);
					}
					strPart4 = Strings.Left(strPart4, intCPos - 1);
					//Get filename base portion.
				}
			}

			switch (intPart) {
				case ParseParts.DrvOnly:
					functionReturnValue = strPart1;
					break;
				case ParseParts.DirOnly:
				case ParseParts.DirOnlyNoSlash:
					functionReturnValue = strPart2;
					break;
				case ParseParts.DrvDir:
				case ParseParts.DrvDirNoSlash:
					functionReturnValue = strPart1 + strPart2;
					break;
				case ParseParts.FileNameBase:
					functionReturnValue = strPart4;
					break;
				case ParseParts.FileNameExt:
				case ParseParts.FileNameExtNoDot:
					functionReturnValue = strPart5;
					break;
				case ParseParts.FileNameBaseExt:
					functionReturnValue = strPart4 + strPart5;
					break;
				case ParseParts.DrvDirFileNameBase:
					functionReturnValue = strPart1 + strPart2 + strPart4;
					break;
				case ParseParts.Protocol:
					functionReturnValue = strPart1;
					break;
			}
			#endif
			return functionReturnValue;
		}
		#endregion
		//Public Sub SetupNewThread()
		//    If Threading.Thread.CurrentThread.ManagedThreadId = mThreadID Then Exit Sub
		//    Dim iCount As Integer = CallStack.Count     'Force an allocation of a new clsCallStack object for this thread if necessary...
		//    Dim iStrings As clsString = Strings(True)   'Force an allocation of a new clsString object for this thread if necessary...
		//    Dim i As Integer = mErrors.LastError.Number 'Force an allocation of a new clsError object for this thread if necessary...
		//End Sub
		//Public Sub CleanupThread()
		//    Dim ThreadID As Integer = Threading.Thread.CurrentThread.ManagedThreadId
		//    If ThreadID = mThreadID Then Exit Sub
		//    Try
		//        mCallStackCollection.Remove(ThreadID.ToString)
		//        mStringCollection.Remove(ThreadID.ToString)
		//        mErrors.RemoveLastError(ThreadID.ToString)
		//    Catch ex As Exception
		//        'This should never happen...
		//    End Try
		//End Sub
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
