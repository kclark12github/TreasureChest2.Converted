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
//clsMain.vb
//   Main Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/17/16    Created;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TreasureChest2
{
	public class clsMain : clsTCBase
	{
		public clsMain() : base(new clsSupport(null, "TreasureChest2"), "clsMain")
		{
			try {
				Trace("clsMain.New()", trcOption.trcApplication);
			} catch (Exception ex) {
				throw;
			}
		}
		public clsMain(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName)
		{
			try {
				Trace("clsMain.New()", trcOption.trcApplication);
			} catch (Exception ex) {
				throw;
			}
		}
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		//None at this time...
		#endregion
		#region "Methods"
		#region "Console Mode Methods"
		//Public Sub DoFCTX(ByVal TXCode As FiRReAFQEnum)
		//    Const EntryName As String = "DoFCTX"
		//    Try
		//        Select Case TXCode
		//            Case FiRReAFQEnum.afqFileFunction To FiRReAFQEnum.afqFileFunctionMax : Me.DoFCTXFile(TXCode)
		//            Case FiRReAFQEnum.afqSystem To FiRReAFQEnum.afqSystemMax : Me.DoFCTXSystem(TXCode)
		//            Case FiRReAFQEnum.afqSetup To FiRReAFQEnum.afqSetupMax : Me.DoFCTXSetup(TXCode)
		//            Case FiRReAFQEnum.afqLoader To FiRReAFQEnum.afqLoaderMax : Me.DoFCTXLoader(TXCode)
		//            Case FiRReAFQEnum.afqForecast To FiRReAFQEnum.afqForecastMax : Me.DoFCTXForecast(TXCode)
		//            Case FiRReAFQEnum.afqProcessing To FiRReAFQEnum.afqProcessingMax : Me.DoFCTXProcessing(TXCode)
		//            Case FiRReAFQEnum.afqBilling To FiRReAFQEnum.afqBillingMax : Me.DoFCTXBilling(TXCode)
		//            Case FiRReAFQEnum.afqCollect To FiRReAFQEnum.afqCollectMax : Me.DoFCTXCollect(TXCode)
		//            Case FiRReAFQEnum.afqReport To FiRReAFQEnum.afqReportMax : Me.DoFCTXReport(TXCode)
		//            Case Else : Me.DoFCTXHelp(TXCode)
		//        End Select
		//    Catch ex As Exception
		//        RaiseError(EntryName, ex)
		//    End Try
		//End Sub
		//Public Sub DoFCTXSystem(ByVal TXCode As FiRReAFQEnum)
		//    Const EntryName As String = "DoFCTXSystem"
		//    Dim objFC As clsFCTXBase = Nothing
		//    Dim objFCR As clsReportBase = Nothing
		//    Try
		//        RecordEntry(EntryName, String.Format("TXCode:={0}", TXCode.ToString), trcOption.trcApplication)
		//        Select Case TXCode
		//            Case FiRReAFQEnum.afqSysAssignReports
		//                '/TXCODE=10001 /EXTENDED="/SCREENMODE=smChange;/KEYVALUE=38001;/REPORT=19001;/MENUTITLE=Access Log ListxxxXXXyy;/OVERRIDE=True;"
		//                If mUseInterop Then : objFC = New clsFCTX10001 : CType(objFC, clsFCTX10001).Connect(mFiRRe) : Else : objFC = New clsFCTX10001(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysClear : If mUseInterop Then : objFC = New clsFCTX10003 : CType(objFC, clsFCTX10003).Connect(mFiRRe) : Else : objFC = New clsFCTX10003(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysClearAll    'Run directly from frmMain...
		//            Case FiRReAFQEnum.afqSysCustom  'Menu
		//            Case FiRReAFQEnum.afqSysCustomDBUtility 'Run directly from frmMain... (not completed)
		//            Case FiRReAFQEnum.afqSysCustRep 'Menu
		//            Case FiRReAFQEnum.afqSysDefineLockout : If mUseInterop Then : objFC = New clsFCTX10015 : CType(objFC, clsFCTX10015).Connect(mFiRRe) : Else : objFC = New clsFCTX10015(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysLockoutUsers : If mUseInterop Then : objFC = New clsFCTX10013 : CType(objFC, clsFCTX10013).Connect(mFiRRe) : Else : objFC = New clsFCTX10013(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysModifyStandards : If mUseInterop Then : objFC = New clsFCTX10004 : CType(objFC, clsFCTX10004).Connect(mFiRRe) : Else : objFC = New clsFCTX10004(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysOptCodes : If mUseInterop Then : objFC = New clsFCTX10005 : CType(objFC, clsFCTX10005).Connect(mFiRRe) : Else : objFC = New clsFCTX10005(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysParams : If mUseInterop Then : objFC = New clsFCTX10006 : CType(objFC, clsFCTX10006).Connect(mFiRRe) : Else : objFC = New clsFCTX10006(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysReleaseLockout : If mUseInterop Then : objFC = New clsFCTX10014 : CType(objFC, clsFCTX10014).Connect(mFiRRe) : Else : objFC = New clsFCTX10014(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysRepActive   'Run directly from frmMain...
		//            Case FiRReAFQEnum.afqSysRepExcept : If mUseInterop Then : objFCR = New clsFCTX19003 : CType(objFCR, clsFCTX19003).Connect(mFiRRe) : Else : objFCR = New clsFCTX19003(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysRepFM : If mUseInterop Then : objFCR = New clsFCTX19004 : CType(objFCR, clsFCTX19004).Connect(mFiRRe) : Else : objFCR = New clsFCTX19004(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysRepModifyStandardsPreReport
		//            Case FiRReAFQEnum.afqSysRepNameMX : If mUseInterop Then : objFCR = New clsFCTX19005 : CType(objFCR, clsFCTX19005).Connect(mFiRRe) : Else : objFCR = New clsFCTX19005(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysRepNameMXDateRange
		//            Case FiRReAFQEnum.afqSysRepOptCodes : If mUseInterop Then : objFCR = New clsFCTX19011 : CType(objFCR, clsFCTX19011).Connect(mFiRRe) : Else : objFCR = New clsFCTX19011(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysReports 'Menu
		//            Case FiRReAFQEnum.afqSysRepParamList
		//            Case FiRReAFQEnum.afqSysRepSysLog : If mUseInterop Then : objFCR = New clsFCTX19001 : CType(objFCR, clsFCTX19001).Connect(mFiRRe) : Else : objFCR = New clsFCTX19001(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysRepTXRecap : If mUseInterop Then : objFCR = New clsFCTX19009 : CType(objFCR, clsFCTX19009).Connect(mFiRRe) : Else : objFCR = New clsFCTX19009(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysRepUser : If mUseInterop Then : objFCR = New clsFCTX19007 : CType(objFCR, clsFCTX19007).Connect(mFiRRe) : Else : objFCR = New clsFCTX19007(mFiRRe) : End If : objFCR.Run()
		//            Case FiRReAFQEnum.afqSysReset : If mUseInterop Then : objFC = New clsFCTX10008 : CType(objFC, clsFCTX10008).Connect(mFiRRe) : Else : objFC = New clsFCTX10008(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysSQLInterface    'Run directly from frmMain...
		//            Case FiRReAFQEnum.afqSysTaskMonitor 'Run directly from frmMain...
		//            Case FiRReAFQEnum.afqSystem     'Menu
		//            Case FiRReAFQEnum.afqSystemMax
		//            Case FiRReAFQEnum.afqSysTransmissionRules   'Not written
		//            Case FiRReAFQEnum.afqSysUserProfileSetup : If mUseInterop Then : objFC = New clsFCTX10007 : CType(objFC, clsFCTX10007).Connect(mFiRRe) : Else : objFC = New clsFCTX10007(mFiRRe) : End If : objFC.Run()
		//            Case FiRReAFQEnum.afqSysUserSetup : If mUseInterop Then : objFC = New clsFCTX10009 : CType(objFC, clsFCTX10009).Connect(mFiRRe) : Else : objFC = New clsFCTX10009(mFiRRe) : End If : objFC.Run()
		//            Case Else : If (TXCode \ 1000) Mod 10 = 8 Then DoFCTXCustom(TXCode)
		//        End Select
		//    Catch ex As Exception
		//        RaiseError(EntryName, ex)
		//    Finally
		//        objFC = Nothing : objFCR = Nothing
		//    End Try
		//    RecordExit(EntryName)
		//End Sub
		#endregion
		public override void Load(Form objParent, string Caption)
		{
			throw new NotImplementedException();
		}
		public void Run()
		{
			bool fUnattended = false;
			try {
				Debug.WriteLine(string.Format("{0} Initializing...", mSupport.ApplicationName));
				//If Environment.CommandLine().ToLower.IndexOf("/unattended") > -1 Then
				//    'To test Payment Data Feed use:
				//    '/U=KCLARK /P=B2SPIRIT /CONFIG="S:\FiRRe\program files\FIS\FiRRe\Config\CSTC (WSRV12)" /DSN="S:\FiRRe\program files\FIS\FiRRe\Config\CSTC (WSRV12)\WildFiRRe - WSRV12.dsn" /UNATTENDED /TXCODE=73004 /EXTENDED="/FN=S:\FTPROOT\BNYM\CSTC.rdy"
				//    fUnattended = True
				//    mFiRRe = New clsFiRRe(mSupport, Environment.CommandLine(), RunModeEnum.Console)
				//Else
				//    mFiRRe = New clsFiRRe(mSupport, Environment.CommandLine(), RunModeEnum.WindowsForms)
				//End If
				//Debug.WriteLine(String.Format("{0} Authenticating...", mSupport.ApplicationName))
				//mFiRRe.Authenticate()

				//Turn trcLogon tracing off automatically...
				if (mSupport.Trace.TraceOptions == trcOption.trcLogon)
					mSupport.Trace.TraceOptions = trcOption.trcNone;
				if (mSupport.Trace.TraceMode && mSupport.Trace.TraceOptions == trcOption.trcNone)
					mSupport.Trace.TraceMode = false;

				//If Not mFiRRe.CommandLine.CommandLineArgs("TXCODE").Present Then
				Debug.WriteLine(string.Format("{0} Invoking Main Form...", mSupport.ApplicationName));
				base.ActiveForm = new frmMain(base.Support, this, null);
				base.ActiveForm.ShowDialog();
                //    Dim form As frmMain = New frmMain(mFiRRe)
                //    mFiRRe.ShowForm(form, True)
                throw new ExitTryException();
                       //End If

                //Debug.WriteLine(String.Format("{0} Invoking Component #{1:D}...", mSupport.ApplicationName, mFiRRe.CommandLine.CommandLineArgs("TXCODE").Value))
                //mFiRRe.Mode = Microsoft.VisualBasic.Interaction.IIf(UseInterop, RunModeEnum.WindowsForms, RunModeEnum.Console)
                //Dim TXCode As FiRReAFQEnum = CType(CInt(mFiRRe.CommandLine.CommandLineArgs("TXCODE").Value), FiRReAFQEnum)
                //'If clsFCTXBase.Run (called from Me.DoFCTX below) encounters a fatal exception, it would call mFiRRe.GenericErrorHandler itself. 
                //'Doing so again here in our error handler would cause a duplicate ERROR_LOG record to be added, so suppress such an effort.
                //fLogFatalError = False
                //Me.DoFCTX(TXCode)
                //Catch ex As FiRReDatabaseVersionException
                //    If fUnattended Then mFiRRe.WriteLog(ex.Message, True) : Exit Try
                //    Dim Version() As String = mSupport.Version.Split(".")
                //    Dim Caption As String = String.Format("Database not compatible with {0} Version {1}.{2}.{3}", New String() {mFiRRe.BrandedAppName, Version(0), Version(1), Version(2)})
                //    If mFiRRe.Forms.Count > 0 Then
                //        mFiRRe.MessageBox(ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, Caption)
                //    Else
                //        Windows.Forms.MessageBox.Show(ex.Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                //    End If
                //Catch ex As FiRReNoActivityAccessException
                //    If fUnattended Then mFiRRe.WriteLog(ex.Message, True) : Exit Try
                //    If mFiRRe.Forms.Count > 0 Then
                //        mFiRRe.MessageBox(ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "Authorization Failure")
                //    Else
                //        Windows.Forms.MessageBox.Show(ex.Message, "Authorization Failure", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                //    End If
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				//mSupport.Errors.SaveLastError(ex)
				//Select Case mSupport.Errors.LastError.Number
				//Case FiRReErrorsEnum.ferrExit                    'User is merely closing the Login screen...
				//Case Else
				//If Not IsNothing(mFiRRe) Then
				//    If fLogFatalError Then mFiRRe.GenericErrorHandler(0, ex.Source, ex.Message, mMyTraceID & ".Main", FiRReErrorSeverityEnum.sevFatal, FiRReExceptionTypeEnum.SystemException)
				//Else
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
					//Try : If Not EventLog.SourceExists(MyBase.ApplicationName) Then EventLog.CreateEventSource(MyBase.ApplicationName, "Application")
					//    EventLog.WriteEntry(ApplicationName, Message, EventLogEntryType.Error)
					//Catch ex2 As Exception
					//End Try
					Console.WriteLine(Message);
				} else {
					System.Windows.Forms.MessageBox.Show(Message, ex.GetType().Name);
				}
				//End If
				//End Select
			}
			mSupport = null;
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
