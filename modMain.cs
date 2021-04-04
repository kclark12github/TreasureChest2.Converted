	//modMain.vb
	//   Main TreasureChest Program...
	//   Copyright © 1998-2018, Ken Clark
	//*********************************************************************************************************************************
	//
	//   Modification History:
	//   Date:       Description:
	//   09/17/16    Created;
	//=================================================================================================================================
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
namespace TreasureChest2
{
	static class modMain
	{
		public static string getApplicationName()
		{
			//Dim at As Type = GetType(AssemblyProductAttribute)
			//Dim r() As Object = [Assembly].GetEntryAssembly.GetCustomAttributes(at, False)
			//Dim pt As AssemblyProductAttribute = CType(r(0), AssemblyProductAttribute)
			//Return pt.Product
			System.Reflection.Assembly EntryAssembly = System.Reflection.Assembly.GetEntryAssembly();
			if ((EntryAssembly == null))
				return clsSupport.bpeNullString;
			//TODO: Revisit
			return EntryAssembly.GetName().Name.ToString();
		}
		//VB6-INTEROP BenchTest
		//Include the following on your project command-line (Project \ Properties \ Configuration properties \ Debugging \ Start Options
		//   /CONFIG="<config-folder-path>" /DSN="<dsn-file-path>" /TXCODE=##### [if untrusted then] /User=<FiRRe-username> /Password=<FiRRe-password> 
		//   /CONFIG="S:\FiRRe\program files\FIS\FiRRe\Config\BNYM (Trusted)" /DSN="S:\FiRRe\program files\FIS\FiRRe\Config\BNYM (Trusted)\WildFiRRe - WSRV12.dsn" /TXCODE=73003
		//Other supported options that may be useful:
		//   /TRACEFILE="V:\FiRRe\SIASFiRRe.log" /TraceOptions=trcSQL+trcApplicationDetail+trcADO /TrustedConnection /ConfirmCredentials=True /DBSecurity=False
		//Use /EXTENDED options when driving a component via the command-line:
		//   /CONFIG="<config-folder-path>" /DSN="<dsn-file-path>" /TXCODE=73003 /EXTENDED="/PostingDate=02/08/2013;/FileName=V:\FTProot\WINNAR.rdy"
		//   Note the lack of spaces within the EXTENDED argument. Semicolon is the delimiter, not space, so embedded spaces will be taken literally 
		//   (no space trimming is done).
		[STAThread()]
		public static void Main()
		{
			string ApplicationName = getApplicationName();
			Debug.WriteLine(string.Format("{0} Started (allocating support infrastructure)...", ApplicationName));
			clsSupport objSupport = null;
			clsMain objMain = null;
			bool fUnattended = false;
			try {
				objSupport = new clsSupport(null, "TreasureChest2");
				string RegistryKey = string.Format("Software\\KClark Software\\{0}", objSupport.ApplicationName);
				objSupport.Trace.TraceMode = Convert.ToBoolean(objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceMode", false));
				objSupport.Trace.TraceFile = (string)objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceFile", string.Format("{0}\\{1}.trace", objSupport.ApplicationPath, objSupport.ApplicationName));
				objSupport.Trace.TraceOptions = (trcOption)objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceOptions", trcOption.trcApplication);
				if (objSupport.Trace.TraceMode) {
					objSupport.Trace.Trace(trcType.trcBody, new string('=', 132));
					objSupport.Trace.Trace(trcType.trcBody, string.Format("{0} Start - {1}", objSupport.ApplicationName, objSupport.Trace.TraceFile));
				}

				if (Environment.CommandLine.ToLower().IndexOf("/unattended") > -1)
					fUnattended = true;
#if TRACESTARTUP
				FileInfo fi = new FileInfo(Environment.CommandLine.Split()[0].Replace("\"", ""));
				objMain = new clsMain(new clsSupport(null, string.Format("{0}.trace", fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length)), trcOption.trcEverythingButMemory, true), "Main");
				fi = null;
#else
				objMain = new clsMain(objSupport, "modMain");
#endif
				Application.EnableVisualStyles();
				Debug.WriteLine(string.Format("{0} Run()...", ApplicationName));
				objMain.Run();
			} catch (Exception ex) {
				string Message = string.Format("{1}{0}{0}StackTrace:{0}{2}{0}", Constants.vbCrLf, ex.Message, ex.StackTrace);
				Exception iException = ex.InnerException;
				if ((iException != null)) {
					Message += string.Format("{0}Underlying Exception(s):{0}", Constants.vbCrLf);
					while ((iException != null)) {
						Message += string.Format("{1}{0}{2}{0}{0}StackTrace:{0}{3}{0}", new object[] {
							Constants.vbCrLf,
							iException.GetType().Name,
							iException.Message,
							iException.StackTrace
						});
						iException = iException.InnerException;
					}
				}
				if (fUnattended) {
					try {
						if (!EventLog.SourceExists(ApplicationName))
							EventLog.CreateEventSource(ApplicationName, "Application");
						EventLog.WriteEntry(ApplicationName, Message, EventLogEntryType.Error);
					} catch (Exception ex2) {
					}
					Console.WriteLine(Message);
				} else {
					System.Windows.Forms.MessageBox.Show(Message, ex.GetType().Name);
				}
			} finally {
				objMain = null;
			}
		}
	}
}
