'clsDataTest.vb
'   TCBase Data Test Class...
'   Copyright © 1998-2017, Ken Clark
'*********************************************************************************************************************************
'   Modification History:
'   Date:       Description:
'   10/06/17    Created;
'=================================================================================================================================
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class clsDataTest
    Inherits clsTCBaseTest
#Region "Declarations"
    Private mDatabase As String = "TreasureChest"
    Private mPassword As String = "b2spirit"
    Private mServer As String = "GGGSCP1"
    Private mUser As String = "KCLARK"
    Private mSQLSource As String
    Private mSQLExpected As String
#End Region
#Region "Private Methods"
    Protected Overloads Sub Initialize()
        'mSQLSource = ""
        'mSQLExpected = ""
        Dim objSupport As clsSupport = New clsSupport(Nothing, "TreasureChest2")
        Dim RegistryKey As String = String.Format("Software\KClark Software\{0}", objSupport.ApplicationName)
        objSupport.Trace.TraceMode = objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceMode", False)
        objSupport.Trace.TraceFile = objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceFile", String.Format("{0}\{1}.trace", objSupport.ApplicationPath, objSupport.ApplicationName))
        objSupport.Trace.TraceOptions = objSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, RegistryKey, "TraceOptions", trcOption.trcApplication)
        If objSupport.Trace.TraceMode Then
            objSupport.Trace.Trace(trcType.trcBody, New String("=", 132))
            objSupport.Trace.Trace(trcType.trcBody, String.Format("{0} Start - {1}", objSupport.ApplicationName, objSupport.Trace.TraceFile))
        End If
        MyBase.Initialize(objSupport)
        MyBase.FileDSN = Me.MakeDSN(mServer, mDatabase, False)  'TODO: Need to figure out how to configure NETWORK SERVICE database access
        MyBase.ConnectionString = BuildConnectionString(MyBase.FileDSN, mUser, mPassword)
    End Sub
    Protected Sub Cleanup()
        Try
            If ActiveTXLevel > 0 Then AbortTrans()
            CloseConnection()
            If MyBase.FileDSN <> bpeNullString Then File.Delete(MyBase.FileDSN)
        Catch ex As Exception : TestContext.WriteLine(ex.Message)
        End Try
    End Sub
    Protected Function MakeDSN(ByVal Server As String, ByVal Database As String, Trusted As Boolean) As String
        MakeDSN = bpeNullString
        Dim FileUnit As Integer = 0
        Try
            MakeDSN = Path.GetTempFileName()
            FileUnit = FreeFile()
            FileOpen(FileUnit, MakeDSN, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            PrintLine(FileUnit, "[ODBC]")
            PrintLine(FileUnit, "DRIVER=SQL Native Client")
            PrintLine(FileUnit, String.Format("Server={0}", Server))
            PrintLine(FileUnit, String.Format("Database={0}", Database))
            PrintLine(FileUnit, String.Format("Trusted_Connection={0}", IIf(Trusted, "Yes", "No")))
            FileClose(FileUnit) : FileUnit = 0
        Catch ex As Exception
            If FileUnit > 0 Then FileClose(FileUnit)
        End Try
    End Function
#End Region
#Region "Test Methods"
#Region "DataSet Operations"
#Region "SQL Statement Processing"
    <TestMethod()> Public Sub DBESQL_RowNumber()
        Dim dvRD As DataView = Nothing
        Try
            Me.Initialize()
            ConnectCommand()
            CommandTimeout = 120 'Provide the query some extra time as it seems to need it during some builds (network traffic?)
            Dim Offset As Integer = 1
            Dim PageSize As Integer = 5000
            mSQLSource = String.Format("Select ROW_NUMBER() Over(Order By {0}) As [RowNumber],{0},Title,Author From {1}", "ID", "Books")
            mSQLSource = String.Format("Select * From ({0}) As [RD] Where [RowNumber] Between {1} And {2} Order By [RowNumber];", mSQLSource, Offset, Offset + PageSize - 1)
            dvRD = OpenDataView(mSQLSource) : Offset += PageSize
        Finally : Me.Cleanup()
            If Not IsNothing(dvRD) Then CloseDataView(dvRD) : dvRD = Nothing
        End Try
    End Sub
#End Region
#End Region
#Region "Connections"
    <TestMethod()> Public Sub DBE_BuildConnectionString()
        Dim DSN As String = bpeNullString
        Try
            Me.Initialize()
            Assert.AreEqual(String.Format("Application Name=TreasureChest2;Workstation ID={0};Data Source={2};Initial Catalog={1};Packet Size=8192;Connect Timeout=120;Pooling=False;User ID={3};Password={4};", New Object() {Environment.MachineName, mDatabase, mServer, mUser, mPassword}), BuildConnectionString(MyBase.FileDSN, mUser, mPassword), False, "Untrusted Connection")

            DSN = Me.MakeDSN(mServer, mDatabase, True) 'Use a local DSN as our default is Untrusted...
            Assert.AreEqual(String.Format("Application Name=TreasureChest2;Workstation ID={0};Data Source={2};Initial Catalog={1};Packet Size=8192;Connect Timeout=120;Pooling=False;Integrated Security=SSPI;", Environment.MachineName, mDatabase, mServer), BuildConnectionString(DSN, mUser, mPassword), False, "Trusted Connection")
            File.Delete(DSN) : DSN = bpeNullString
        Finally : Me.Cleanup()
            If DSN <> bpeNullString Then File.Delete(DSN)
        End Try
    End Sub
    <TestMethod()> Public Sub DBE_Connection()
        Dim DSN As String = bpeNullString
        Try
            Me.Initialize()
            ConnectCommand()

            'TODO: Need to figure out how to configure NETWORK SERVICE database access
            'DSN = MakeDSN(True) 'Use a local DSN as our default is Untrusted...
            'mDBE.Connect(String.Format("FileDSN={0}", DSN), mUserName, mPassword)
            'File.Delete(DSN) : DSN = bpeNullString
        Finally : Me.Cleanup()
        End Try
    End Sub
    <TestMethod> Public Sub DBESQL_DataTables()
        Dim ds As DataSet = Nothing
        Dim SQLSource As String = bpeNullString
        Try
            Me.Initialize()
            ConnectCommand()

            Dim cntKits As Integer = 0 : SQLSource = "Select Count(*) From Kits Where [WishList]=0;" : cntKits = CInt(ExecuteScalarCommand(SQLSource)) : mSQLSource = SQLSource
            Dim cntCollectables As Integer = 0 : SQLSource = "Select Count(*) From Collectables Where [WishList]=0;" : cntCollectables = CInt(ExecuteScalarCommand(SQLSource)) : mSQLSource &= SQLSource
            Dim cntWishKits As Integer = 0 : SQLSource = "Select Count(*) From Kits wlKits Where [WishList]=1;" : cntWishKits = CInt(ExecuteScalarCommand(SQLSource)) : mSQLSource &= SQLSource
            ds = OpenDataSet(mSQLSource)
            Assert.AreEqual(3, ds.Tables.Count, "ds.Tables.Count")
            Assert.AreEqual("Kits", ds.Tables(0).TableName, False, "ds.Tables(0)") : Assert.AreEqual(cntKits, ds.Tables(0).Rows(0).Item(0), "cntKits vs. ds.Tables(0).Rows(0).Item(0)")
            Assert.AreEqual("Collectables", ds.Tables(1).TableName, False, "ds.Tables(1)") : Assert.AreEqual(cntCollectables, ds.Tables(1).Rows(0).Item(0), "cntCollectables vs. ds.Tables(1).Rows(0).Item(0)")
            Assert.AreEqual("wlKits", ds.Tables(2).TableName, False, "ds.Tables(2)") : Assert.AreEqual(cntWishKits, ds.Tables(2).Rows(0).Item(0), "cntWishKits vs. ds.Tables(2).Rows(0).Item(0)")

            Dim cntShipKits As Integer = 0 : SQLSource = "Select Count(*) From Kits shipKits Where [Type]='Ship';" : cntShipKits = CInt(ExecuteScalarCommand(SQLSource)) : mSQLSource = SQLSource
            'Should replace Tables(0)...
            Dim maxKitID As Integer = 0 : SQLSource = "Select Max(ID) From Kits;" : maxKitID = CInt(ExecuteScalarCommand(SQLSource)) : mSQLSource &= SQLSource
            ds = OpenDataSet(ds, mSQLSource)
            Assert.AreEqual(4, ds.Tables.Count, "ds.Tables.Count")
            Assert.AreEqual("Collectables", ds.Tables(0).TableName, False, "ds.Tables(0)") : Assert.AreEqual(cntCollectables, ds.Tables(0).Rows(0).Item(0), "cntCollectables vs. ds.Tables(0).Rows(0).Item(0)")
            Assert.AreEqual("wlKits", ds.Tables(1).TableName, False, "ds.Tables(1)") : Assert.AreEqual(cntWishKits, ds.Tables(1).Rows(0).Item(0), "cntWishKits vs. ds.Tables(1).Rows(0).Item(0)")
            Assert.AreEqual("shipKits", ds.Tables(2).TableName, False, "ds.Tables(2)") : Assert.AreEqual(cntShipKits, ds.Tables(2).Rows(0).Item(0), "cntShipKits vs. ds.Tables(2).Rows(0).Item(0)")
            Assert.AreEqual("Kits", ds.Tables(3).TableName, False, "ds.Tables(3)") : Assert.AreEqual(maxKitID, ds.Tables(3).Rows(0).Item(0), "maxKitID vs. ds.Tables(3).Rows(0).Item(0)")
        Finally : Me.Cleanup()
            If Not IsNothing(ds) Then CloseDataSet(ds) : ds = Nothing
        End Try
    End Sub
#End Region
#Region "Transactions"
    <TestMethod()> Public Sub DBE_BeginEnd()
        Try
            Me.Initialize()
            ConnectCommand()
            BeginTrans()
            EndTrans()
        Finally : Me.Cleanup()
        End Try
    End Sub
#End Region
#End Region
End Class