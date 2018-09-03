//clsTrace.vb
//   TreasureChest2 Trace Module...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   12/19/17    Implemented additional Trace Support;
//   10/21/16    Used Collection.Contains method to avoid ArguementExceptions;
//   09/18/16    Updated trcOptions;
//   01/24/14    Changed forceFullCollection argument to GC.GetTotalMemory() from True to False to avoid deadlock caused when 
//               tracing object destruction (SyncLocked on "TraceLock");
//   01/21/14    Eliminated use of RecordEntry/RecordExit for performance;
//   10/23/12    Added CleanUpTraceEnabled/FinalizeTraceEnabled to reduce output window clutter;
//   03/30/12    Added COM Interop-friendly overload for Trace method;
//   09/18/09    Added trcFTP;
//   10/19/06    Updated FormatTimer to use 24-hour clock in time specification (i.e. "HH" instead of "hh" in format);
//   10/03/05    Added support for new trcMemory TraceOption;
//   08/31/05    Created LogMessage from code previously contained in Trace to be used more generically throughout application;
//   08/24/05    Added new TraceOptions:
//                   trcServer = 2 ^ 17
//                   trcFileWatcher = 2 ^ 18
//                   trcMSMQWatcher = 2 ^ 19
//                   trcTaskWatcher = 2 ^ 20
//                   trcTCPIPWatcher = 2 ^ 21
//   04/20/05    Added trcWinsock support;
//   04/07/05    Added trcCL, trcControls and trcScheduler;
//   03/21/05    Updated "winroot" reference with the correct keyword "%WinDir%";
//   03/11/05    Made New() constructor "Friend" in order to accomplish PublicNotCreatable VB6 Class Instancing behavior;
//   10/09/04    Added trcNone;
//   08/05/04    Added TraceObject property;
//   06/24/04    Changed FormatTimer to consistently format fractional seconds for proper trace log display;
//   04/12/04    Added FormatTimer to get fractional seconds using (Timer function) for trace messages;
//   02/24/04    Added trcRPC;
//   12/01/03    Added logic to handle lone vbCr and vbLf in Trace messages;
//   11/24/03    Added trcADO and code to handle new Cancel button on frmTraceOptions;
//   11/22/02    Cleaned-up TraceMode Property Let routine;
//   11/14/01    Implemented ShowTraceOptions;
//               Moved ParseTraceOptions from clsFiRReApplication;
//   10/08/01    Created from FiRRe's Trace code;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
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
using VB = Microsoft.VisualBasic;
namespace TCBase
{
	public class clsTrace : IDisposable
	{
		internal clsTrace(clsSupport objSupport, string TracePath = null, clsTrace.trcOption toTraceOptions = trcOption.trcNone, bool bTraceMode = false) : base()
		{
			const string EntryName = "New";
			mSupport = objSupport;
			mMyModuleName = "clsTrace";
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, mSupport.ExecutingComponentName, mMyModuleName);

			TraceIndent = 0;
			mTraceOptions = toTraceOptions;
			mTraceUnit = 0;
            mTraceFile = (string)Microsoft.VisualBasic.Interaction.IIf((TracePath == null), bpeNullString, TracePath);
			mInitialTraceMessage = bpeNullString;
            mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            TraceMode = bTraceMode;
			Trace(string.Format("{0}.{1}(objSupport:={{clsSupport}}, TracePath:=\"{2}\", toTraceOptions:={3}, bTraceMode:={4})", new object[] {
				mMyTraceID,
				EntryName,
				mTraceFile,
				toTraceOptions,
				bTraceMode
			}), trcOption.trcSupport);
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Declarations"
		const string bpeNullString = "";
		private bool bDisposed = false;
		private bool fTraceMode;
		static bool CleanUpTraceEnabled = false;
		static bool FinalizeTraceEnabled = false;
		private trcOption mDefaultTraceOption = trcOption.trcSupport;
		private string mInitialTraceMessage;
		protected clsSupport mSupport;
		private string mTraceFile;
		private trcOption mTraceOptions;
		private int mTraceUnit;
		protected string mMyModuleName;
		protected string mMyTraceID;
		private Dictionary<string,clsIndentByThread> ThreadIndents = new Dictionary<string,clsIndentByThread>();
		private object LogMessageLock = new object();
        private int mThreadID = -1;
			#endregion
		private object TraceLock = new object();
		#region "Enumerations"
		public enum trcType
		{
			trcEnter = -1,
			trcBody = 0,
			trcExit = 1
		}
		[FlagsAttribute()]
		public enum trcOption
		{
			trcAll = -1,
			trcNone = 0,
			trcApplication = 1,
			trcBinding = 2,
			trcCL = 4,
			trcControls = 8,
			trcDB = 16,
			trcLogon = 32,
			trcMemory = 64,
			trcReports = 128,
			trcSupport = 256,
			trcEverythingButMemory = trcApplication | trcBinding | trcReports | trcDB | trcSupport | trcCL | trcControls | trcLogon
		}
		#endregion
		public string DefaultTraceFile {
			get { return string.Format("{0}\\{1}.trace", mSupport.ApplicationPath, mSupport.ApplicationName); }
		}
		public trcOption DefaultTraceOption {
			get { return mDefaultTraceOption; }
			set { mDefaultTraceOption = value; }
		}
		public string InitialMessage {
			get { return mInitialTraceMessage; }
			set { mInitialTraceMessage = InitialMessage; }
		}
		public string MyModuleName {
			get { return mMyModuleName; }
		}
		public string MyTraceID {
			get { return mMyTraceID; }
		}
		public string TraceFile {
			get { return mTraceFile; }
			set { mTraceFile = value; }
		}
        private int ThreadID
        {
            get { return mThreadID; }
            set { mThreadID = value; }
        }
        private short TraceIndent {
			get {
				short functionReturnValue = 0;
				clsIndentByThread objIBT = null;
				try {
					if (mThreadID == -1)
						mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
					if (!ThreadIndents.ContainsKey(Convert.ToString(mThreadID)))
						ThreadIndents.Add(Convert.ToString(mThreadID), new clsIndentByThread(mThreadID, 0));
					objIBT = ThreadIndents[Convert.ToString(mThreadID)];
					functionReturnValue = objIBT.TraceIndent;
				} catch (System.ArgumentException ex) {
					ThreadIndents.Add(Convert.ToString(mThreadID), new clsIndentByThread(mThreadID, 0));
				}
				return functionReturnValue;
			}
			set {
				clsIndentByThread objIBT = null;
				try {
					if (mThreadID == -1)
						mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
					if (!ThreadIndents.ContainsKey(Convert.ToString(mThreadID)))
						ThreadIndents.Add(Convert.ToString(ThreadID), new clsIndentByThread(mThreadID, value));
					objIBT = ThreadIndents[Convert.ToString(mThreadID)];
					objIBT.TraceIndent = value;
				} catch (System.ArgumentException ex) {
					ThreadIndents.Add(Convert.ToString(ThreadID), new clsIndentByThread(ThreadID, value));
				}
			}
		}
		public trcOption TraceOptions {
			get { return mTraceOptions; }
			set { mTraceOptions = value; }
		}
		public bool TraceMode {
			get { return fTraceMode; }
			set {
				string strTrace = bpeNullString;
				if (!fTraceMode && value) {
					//We're turning Tracing on...
					if (mTraceFile == bpeNullString)
						mTraceFile = DefaultTraceFile;
					if (mTraceOptions == 0)
						mTraceOptions = trcOption.trcEverythingButMemory;
					//(changed from trcAll as trcMemory causes the system routine to get memory stats and it hangs)
					if (Strings.InStr(mTraceFile, "\\") == 0)
						mTraceFile = string.Format("{0}\\{1}", Path.GetDirectoryName(mSupport.ApplicationPath), mTraceFile);
					mTraceUnit = FileSystem.FreeFile();
					FileSystem.FileOpen(mTraceUnit, mTraceFile, OpenMode.Append);
					Debug.WriteLine(new string('=', 132));
					FileSystem.PrintLine(mTraceUnit, new string('=', 132));
					FileSystem.FileClose(mTraceUnit);
					mTraceUnit = 0;

					fTraceMode = value;
					strTrace = string.Format("Trace started for {0}; User {1} on {2}{3}", new object[] {
						mSupport.ApplicationName,
						TCBase.My.MyProject.User.Name,
						TCBase.My.MyProject.Computer.Name,
						Constants.vbCrLf
					});
					strTrace += string.Format("System Information:{0}", Constants.vbCrLf);
					strTrace += string.Format("{0}CPU Processor                          {0}{1}{2}", Constants.vbTab, TCBase.My.MyProject.Computer.Info.OSPlatform, Constants.vbCrLf);
					strTrace += string.Format("{0}Memory: Total Physical Memory          {0}{1:#,##0.00} MB{2}", Constants.vbTab, TCBase.My.MyProject.Computer.Info.TotalPhysicalMemory / 1024, Constants.vbCrLf);
					strTrace += string.Format("{0}Memory: Total Virtual Memory           {0}{1:#,##0.00} MB{2}", Constants.vbTab, TCBase.My.MyProject.Computer.Info.TotalVirtualMemory / 1024, Constants.vbCrLf);
					strTrace += string.Format("{0}Operating System                       {0}{1} {2}{3}", new object[] {
						Constants.vbTab,
						TCBase.My.MyProject.Computer.Info.OSFullName,
						TCBase.My.MyProject.Computer.Info.OSVersion,
						Constants.vbCrLf
					});
					strTrace += string.Format("{0}Video Resolution                       {0}{1} x {2} pixels{3}", new object[] {
						Constants.vbTab,
						TCBase.My.MyProject.Computer.Screen.Bounds.Width,
						TCBase.My.MyProject.Computer.Screen.Bounds.Height,
						Constants.vbCrLf
					});
					strTrace += string.Format("{0}Video Color Depth                      {0}{1:#,##0} colors{2}", Constants.vbTab, Convert.ToString(TCBase.My.MyProject.Computer.Screen.BitsPerPixel), Constants.vbCrLf);
					strTrace += mInitialTraceMessage;
					Trace(strTrace + new string('-', 108), trcOption.trcEverythingButMemory);

					//We can't control if the user turns this off, so avoid the resulting indentation issue by
					//not tracing this operation...
					//Call Trace(trcEnter, "FiRReApplication.TraceMode()", trcApplication)
				} else if (fTraceMode && !value) {
					//We're turning Tracing off...
					Debug.WriteLine(new string('=', 132));
					mTraceUnit = FileSystem.FreeFile();
					FileSystem.FileOpen(mTraceUnit, mTraceFile, OpenMode.Append);
					FileSystem.PrintLine(mTraceUnit, new string('=', 132));
					FileSystem.FileClose(mTraceUnit);
					//We can't control if the user turns this off, so avoid the resulting indentation issue by
					//not tracing this operation...
					//Call Trace(trcExit, "FiRReApplication.TraceMode()", trcApplication)
				}
				fTraceMode = value;
			}
		}
		#endregion
		#region "Methods"
		#region "Destructor"
		public void Dispose()
		{
			//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "]" & vbTab & TraceID & ".Dispose()")
			Dispose(true);
			//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "] " & mMyTraceID & ".Dispose(): GC.SuppressingFinalize(Me)")
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			try {
				if (bDisposed)
					return;
				if (Disposing) {
					//Object is being disposed, not finalized.
					//It is safe to access other objects (other than the base object) only from inside this block.
					if ((mSupport != null))
						Trace("Disposing: " + mMyTraceID, trcOption.trcMemory);
				}
			} finally {
				mSupport = null;
				bDisposed = true;
				//MyBase.Dispose(Disposing)
			}
		}
		#endregion
		public static void CleanUpTrace(string TraceID)
		{
			if (!CleanUpTraceEnabled)
				return;
			string strNow = DateAndTime.Today.ToShortDateString() + " " + FormatTimer();
			Debug.WriteLine(string.Format("[{0:0000}] {1}; {2}CleanUp: {3}{4}", new object[] {
				System.Threading.Thread.CurrentThread.ManagedThreadId,
				strNow,
				Constants.vbTab,
				TraceID,
				MemoryStats()
			}));
		}
		public static void FinalizeTrace(string TraceID)
		{
			if (!FinalizeTraceEnabled)
				return;
			string strNow = DateAndTime.Today.ToShortDateString() + " " + FormatTimer();
			Debug.WriteLine(string.Format("[{0:0000}] {1}; {2}Finalize: {3}{4}", new object[] {
				System.Threading.Thread.CurrentThread.ManagedThreadId,
				strNow,
				Constants.vbTab,
				TraceID,
				MemoryStats()
			}));
		}
		public static string FormatMemoryDump(long GCTotalMemory, long NonpagedSystemMemory, long PagedMemory, long PagedSystemMemory, long PrivateMemory, long VirtualMemory, long WorkingSet)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			functionReturnValue += Constants.vbTab + string.Format("GC.GetTotalMemory: {0:#,##0.00} KB;", GCTotalMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Non-paged System Memory: {0:#,##0.00} KB;", NonpagedSystemMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Paged Memory: {0:#,##0.00} KB;", PagedMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Paged System Memory: {0:#,##0.00} KB;", PagedSystemMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Private Memory: {0:#,##0.00} KB;", PrivateMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Virtual Memory: {0:#,##0.00} KB;", VirtualMemory / (long)ByteEnum.KB);
			functionReturnValue += Constants.vbTab + string.Format("Working Set: {0:#,##0.00} KB;", WorkingSet / (long)ByteEnum.KB);
			return functionReturnValue;
		}
		public static string FormatTimer()
		{
			return string.Format("{0:HH:mm:ss.ffff}", DateAndTime.Now);
		}
		public string LogMessage(string FileName, string Message, short Indent, bool fSkipFormat = false)
		{
			string functionReturnValue = null;
			mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
			string strMessagePrefix = bpeNullString;
			if ((FileName == null))
				FileName = bpeNullString;
			if (!fSkipFormat) {
				string strNow = DateAndTime.Today.ToShortDateString() + " " + FormatTimer();
				string strTabs = Strings.StrDup(Indent, Constants.vbTab);
				strMessagePrefix = string.Format("[{0:0000}] {1}; {2}", ThreadID, strNow, strTabs);
			}

			lock (LogMessageLock) {
				string strLogMessage = strMessagePrefix + Message;
				strLogMessage = Strings.Replace(Strings.Replace(Strings.Replace(strLogMessage, Constants.vbCrLf, Constants.vbCrLf + strMessagePrefix), Constants.vbLf, Constants.vbCrLf), Constants.vbCr + Constants.vbCrLf, Constants.vbCrLf);
				functionReturnValue = strLogMessage;

				//Write the message to the VB Immediate Window...
				try {
					Debug.WriteLine(strLogMessage);
				} catch (Exception ex) {
				}
				if (FileName == bpeNullString)
					return functionReturnValue;

				//See if our file is too big to handle... If so, we'll rename it accordingly, and open a new one...
				FileInfo LogFileInfo = new FileInfo(FileName);
				if (LogFileInfo.Exists) {
					if (LogFileInfo.Length > (mSupport.LogMaxSizeMB * (int)ByteEnum.MB)) {
						System.DateTime dtModified = LogFileInfo.LastWriteTime;
						string NewFileName = Path.GetFileNameWithoutExtension(LogFileInfo.Name) + "." + Strings.Format(dtModified.Year, "0000") + Strings.Format(dtModified.Month, "00") + Strings.Format(dtModified.Day, "00") + Strings.Format(dtModified.Hour, "00") + Strings.Format(dtModified.Minute, "00") + Strings.Format(dtModified.Second, "00") + LogFileInfo.Extension;
						FileSystem.Rename(FileName, string.Format("{0}\\{1}", LogFileInfo.DirectoryName, NewFileName));

						//If we successfully renamed our existing file, now police any older files that need to be deleted...
						DirectoryInfo LogDirInfo = new DirectoryInfo(LogFileInfo.DirectoryName);
						FileInfo[] LogFileList = LogDirInfo.GetFiles(string.Format("{0}.*{1}", Path.GetFileNameWithoutExtension(LogFileInfo.Name), LogFileInfo.Extension));
						foreach (FileInfo iFileInfo in LogFileList) {
							if (DateAndTime.DateDiff(DateInterval.DayOfYear, iFileInfo.LastWriteTime, DateAndTime.Now) > mSupport.LogRetentionDays)
								iFileInfo.Delete();
						}
					}
				}
				//Write the message to the file...
				StreamWriter mLogFileWriter = new StreamWriter(FileName, true);
				mLogFileWriter.WriteLine(strLogMessage);
				mLogFileWriter.Flush();
				mLogFileWriter.Close();
				mLogFileWriter = null;
				LogFileInfo = null;
			}
			return functionReturnValue;
		}
		public static string MemoryStats(long GCTotalMemory = 0, System.Diagnostics.Process enterProcess = null)
		{
			string functionReturnValue = null;
			try {
				if (GCTotalMemory == 0)
					GCTotalMemory = GC.GetTotalMemory(false);
				if ((enterProcess == null))
					enterProcess = Process.GetCurrentProcess();
				var _with1 = enterProcess;
				functionReturnValue = Constants.vbTab + "[Current Memory Statistics:" + FormatMemoryDump(GCTotalMemory, _with1.NonpagedSystemMemorySize64, _with1.PagedMemorySize64, _with1.PagedSystemMemorySize64, _with1.PrivateMemorySize64, _with1.VirtualMemorySize64, _with1.WorkingSet64) + "]";
			} finally {
				enterProcess = null;
			}
			return functionReturnValue;
		}
		public static string MemoryConsumed(long GCTotalMemory, System.Diagnostics.Process enterProcess, System.Diagnostics.Process exitProcess)
		{
			string functionReturnValue = null;
			var _with2 = exitProcess;
			functionReturnValue = Constants.vbTab + "[Net Memory Consumed:" + FormatMemoryDump((GC.GetTotalMemory(false) - GCTotalMemory), (_with2.NonpagedSystemMemorySize64 - enterProcess.NonpagedSystemMemorySize64), (_with2.PagedMemorySize64 - enterProcess.PagedMemorySize64), (_with2.PagedSystemMemorySize64 - enterProcess.PagedSystemMemorySize64), (_with2.PrivateMemorySize64 - enterProcess.PrivateMemorySize64), (_with2.VirtualMemorySize64 - enterProcess.VirtualMemorySize64), (_with2.WorkingSet64 - enterProcess.WorkingSet64)) + "]";
			return functionReturnValue;
		}
		public clsTrace.trcOption ParseTraceOptions(string strTraceOptions)
		{
			clsTrace.trcOption functionReturnValue = default(clsTrace.trcOption);
			int i = 0;
			string Options = null;

			functionReturnValue = 0;
			while (Strings.Len(strTraceOptions) > 0) {
				i = Strings.InStr(strTraceOptions, "+");
				if (i == 0) {
					Options = Strings.Trim(Strings.UCase(strTraceOptions));
					strTraceOptions = bpeNullString;
				} else {
					Options = Strings.Trim(Strings.UCase(Strings.Left(strTraceOptions, i - 1)));
					strTraceOptions = Strings.Trim(Strings.Mid(strTraceOptions, i + 1));
				}

				switch (Options) {
					case "TRCALL":
						functionReturnValue = trcOption.trcAll;
						break;
					case "TRCAPPLICATION":
						functionReturnValue &= trcOption.trcApplication;
						break;
					case "TRCREPORTS":
						functionReturnValue &= trcOption.trcReports;
						break;
					case "TRCSQL":
						functionReturnValue &= trcOption.trcDB;
						break;
					case "TRCADO":
						functionReturnValue &= trcOption.trcDB;
						break;
					case "TRCSUPPORT":
						functionReturnValue &= trcOption.trcSupport;
						break;
					case "TRCCL":
						functionReturnValue &= trcOption.trcCL;
						break;
					case "TRCCONTROLS":
						functionReturnValue &= trcOption.trcControls;
						break;
					case "TRCMEMORY":
						functionReturnValue &= trcOption.trcMemory;
						break;
					case "TRCEVERYTHINGBUTMEMORY":
						functionReturnValue &= trcOption.trcEverythingButMemory;
						break;
				}
			}
			return functionReturnValue;
		}
		public void ShowTraceOptions()
		{
			frmTraceOptions frmTraceOptions = new frmTraceOptions(ref mSupport);
            frmTraceOptions.iTraceOptions = mTraceOptions;
            frmTraceOptions.txtTraceFile.Text = mTraceFile;
            frmTraceOptions.ShowDialog();
			if (!frmTraceOptions.fCancelClicked) {
				mTraceOptions = frmTraceOptions.iTraceOptions;
				mTraceFile = frmTraceOptions.txtTraceFile.Text;
				fTraceMode = Convert.ToBoolean(mTraceOptions != trcOption.trcNone);
			}
            frmTraceOptions.fOKtoUnload = true;
            frmTraceOptions.Close();
		}
		public void Trace(string Message, clsTrace.trcOption trcTraceOption)
		{
			Trace(trcType.trcBody, Message, trcTraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Message, clsTrace.trcOption trcTraceOptions = trcOption.trcNone, System.Diagnostics.Process ceProcess = null)
		{
			string TraceFile = bpeNullString;
			lock (TraceLock) {
				//Override Trace Parameters if trcAll...
				if (trcTraceOptions == trcOption.trcAll && !fTraceMode) {
					TraceFile = mSupport.LogFile;
				} else {
					if (!fTraceMode || trcTraceOptions == 0)
						return;
					if (((mTraceOptions & trcTraceOptions) == 0))
						return;
					TraceFile = mTraceFile;
				}

				mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                //Debug.WriteLine("Trace Options: " & CType(trcTraceOptions, trcOption).ToString)

                //Make sure we allocate a new TraceIndent for this ThreadID...
                clsIndentByThread objIBT = null;
                objIBT = ThreadIndents[Convert.ToString(mThreadID)];
                if (trcTraceType == trcType.trcExit && objIBT.TraceIndent > 0)
                    objIBT.TraceIndent -= 1;
				switch (trcTraceType) {
					case trcType.trcEnter:
						Message = "Entering " + Message;
						break;
					case trcType.trcExit:
						Message = "Exiting " + Message;
						break;
				}

				if ((mTraceOptions & trcOption.trcMemory) == trcOption.trcMemory) {
					long TotalMemory = GC.GetTotalMemory(false);
					Message += MemoryStats(TotalMemory, Process.GetCurrentProcess());
					switch (trcTraceType) {
						case trcType.trcExit:
							if ((ceProcess != null))
								Message += MemoryConsumed(TotalMemory, ceProcess, Process.GetCurrentProcess());
							break;
					}
				}
				this.LogMessage(TraceFile, Message, objIBT.TraceIndent, false);

				if (trcTraceType == trcType.trcEnter)
                    objIBT.TraceIndent += 1;
			}
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
namespace TCBase
{
	#region "Support Classes"
	internal class clsIndentByThread
	{
		public int ThreadID;
		public short TraceIndent;
		internal clsIndentByThread(int iThreadID, short iTraceIndent) : base()
		{
			ThreadID = iThreadID;
			TraceIndent = iTraceIndent;
		}
	}
}
#endregion
