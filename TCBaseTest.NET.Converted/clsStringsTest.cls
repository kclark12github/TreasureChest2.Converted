'clsStringsTest.vb
'   TCBase Strings Test Class...
'   Copyright © 1998-2017, Ken Clark
'*********************************************************************************************************************************
'   Modification History:
'   Date:       Description:
'   10/06/17    Created;
'=================================================================================================================================
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class clsStringsTest
    Inherits clsTCBaseTest
#Region "Test Methods"
    <TestMethod()> Public Sub Strings_AssignCSV()
        Assert.AreEqual(bpeNullString, mSupport.Strings.AssignCSV(Nothing))
        Assert.AreEqual(bpeNullString, mSupport.Strings.AssignCSV(DBNull.Value))
        Assert.AreEqual(bpeNullString, mSupport.Strings.AssignCSV(bpeNullString))
        Assert.AreEqual("fox", mSupport.Strings.AssignCSV("fox"))
        Assert.AreEqual("""NEW YORK, NY""", mSupport.Strings.AssignCSV("NEW YORK, NY"))
        Assert.AreEqual(String.Format("""187 West 152 Street{0}NEW YORK, NY""", vbCrLf), mSupport.Strings.AssignCSV(String.Format("187 West 152 Street{0}NEW YORK, NY", vbCrLf)))
        Assert.AreEqual(String.Format("""187 West 152 Street{0}NEW YORK, NY""", Chr(4)), mSupport.Strings.AssignCSV(String.Format("187 West 152 Street{0}NEW YORK, NY", Chr(4))))
    End Sub
    <TestMethod()> Public Sub Strings_CountStr()
        Assert.AreEqual(0, mSupport.Strings.CountStr(Nothing, Nothing))
        Assert.AreEqual(0, mSupport.Strings.CountStr(bpeNullString, bpeNullString))
        Assert.AreEqual(8, mSupport.Strings.CountStr("The quick brown fox jumps over the lazy dog.", " "))
        Assert.AreEqual(5, mSupport.Strings.CountStr("C:\program files\SunGard\FiRRe\Config\Database.dsn", "\"))
        Assert.AreEqual(23, mSupport.Strings.CountStr("2#26395,,,#1546,10/31/2014,0,Description,Job_ID,Y,N,10/31/2014,0,Description,CHRISTINE CONWAY,EPISCOPAL HEALTH SERVICES,""NEW YORK, NY"",Record 1,MiscData05,MiscData06,MiscData07,7/19/2014,117707,0", ","))
        Assert.AreEqual(9, mSupport.Strings.CountStr("Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour", " "))
    End Sub
    <TestMethod()> Public Sub Strings_InStrMX()
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(0, " ", " "))
            Assert.Fail("InStrMX did not throw exception when passed zero as Start")
        Catch ex As Exception When ex.Message = "Start must be greater than zero."
        End Try
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(-1, " ", " "))
            Assert.Fail("InStrMX did not throw exception when passed -1 as Start")
        Catch ex As Exception When ex.Message = "Start must be greater than zero."
        End Try
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(1, Nothing, " "))
            Assert.Fail("InStrMX did not throw exception when passed Nothing as strWork")
        Catch ex As Exception When ex.Message = "Work string must be specified."
        End Try
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(1, bpeNullString, " "))
            Assert.Fail("InStrMX did not throw exception when passed """" as strWork")
        Catch ex As Exception When ex.Message = "Work string must be specified."
        End Try
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(1, " ", Nothing))
            Assert.Fail("InStrMX did not throw exception when passed Nothing as strSearchSet")
        Catch ex As Exception When ex.Message = "SearchSet must be specified."
        End Try
        Try
            Assert.AreEqual(0, mSupport.Strings.InStrMX(1, " ", bpeNullString))
            Assert.Fail("InStrMX did not throw exception when passed """" as strSearchSet")
        Catch ex As Exception When ex.Message = "SearchSet must be specified."
        End Try

        Assert.AreEqual(4, mSupport.Strings.InStrMX(1, "The quick brown fox jumps over the lazy dog.", " ."))
        Assert.AreEqual(17, mSupport.Strings.InStrMX(17, "C:\program files\SunGard\FiRRe\Config\Database.dsn", "\:."))
        Assert.AreEqual(8, mSupport.Strings.InStrMX(2, "2#26395,,,#1546,10/31/2014,0,Description,Job_ID,Y,N,10/31/2014,0,Description,CHRISTINE CONWAY,EPISCOPAL HEALTH SERVICES,""NEW YORK, NY"",Record 1,MiscData05,MiscData06,MiscData07,7/19/2014,117707,0", ",#/."))
        Assert.AreEqual(10, mSupport.Strings.InStrMX(1, "Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour", "'"))
    End Sub
    <TestMethod()> Public Sub Strings_ParseCSV()
        Dim CSV As String = "#26395,,,#1546,10/31/2014,0,Description,Job_ID,Y,N,10/31/2014,0,Description,CHRISTINE CONWAY,EPISCOPAL HEALTH SERVICES,""NEW YORK, NY"",Record 1,MiscData05,MiscData06,MiscData07,7/19/2014,117707,0"
        Dim List As ArrayList = Nothing
        mSupport.Strings.ParseCSV(Nothing, List)
        Assert.AreEqual(0, List.Count, "Expected an empty list, but found {0} items", List.Count)
        mSupport.Strings.ParseCSV(CSV, List)
        Assert.AreEqual(23, List.Count, "Expected {0:#,##0} items, but found {1:#,##0} items", New Object() {23, List.Count})

        Assert.AreEqual("#26395", CStr(List(0)), False)
        Assert.AreEqual("", CStr(List(1)), False)
        Assert.AreEqual("", CStr(List(2)), False)
        Assert.AreEqual("#1546", CStr(List(3)), False)
        Assert.AreEqual("10/31/2014", CStr(List(4)), False)
        Assert.AreEqual("0", CStr(List(5)), False)
        Assert.AreEqual("Description", CStr(List(6)), False)
        Assert.AreEqual("Job_ID", CStr(List(7)), False)
        Assert.AreEqual("Y", CStr(List(8)), False)
        Assert.AreEqual("N", CStr(List(9)), False)
        Assert.AreEqual("10/31/2014", CStr(List(10)), False)
        Assert.AreEqual("0", CStr(List(11)), False)
        Assert.AreEqual("Description", CStr(List(12)), False)
        Assert.AreEqual("CHRISTINE CONWAY", CStr(List(13)), False)
        Assert.AreEqual("EPISCOPAL HEALTH SERVICES", CStr(List(14)), False)
        Assert.AreEqual("NEW YORK, NY", CStr(List(15)), False)
        Assert.AreEqual("Record 1", CStr(List(16)), False)
        Assert.AreEqual("MiscData05", CStr(List(17)), False)
        Assert.AreEqual("MiscData06", CStr(List(18)), False)
        Assert.AreEqual("MiscData07", CStr(List(19)), False)
        Assert.AreEqual("7/19/2014", CStr(List(20)), False)
        Assert.AreEqual("117707", CStr(List(21)), False)
        Assert.AreEqual("0", CStr(List(22)), False)
    End Sub
    <TestMethod()> Public Sub Strings_ParseStr()
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(Nothing, 4, " "), False)
            Assert.Fail("ParseStr did not throw exception when passed Nothing as strWork")
        Catch ex As Exception When ex.Message = "Work string must be specified."
        End Try
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(bpeNullString, 4, " "), False)
            Assert.Fail("ParseStr did not throw exception when passed """" as strWork")
        Catch ex As Exception When ex.Message = "Work string must be specified."
        End Try
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(" ", 4, Nothing), False)
            Assert.Fail("ParseStr did not throw exception when passed Nothing as strDelimiter")
        Catch ex As Exception When ex.Message = "Delimiter must be specified."
        End Try
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(" ", 4, bpeNullString), False)
            Assert.Fail("ParseStr did not throw exception when passed """" as strDelimiter")
        Catch ex As Exception When ex.Message = "Delimiter must be specified."
        End Try
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(" ", 0, " "), False)
            Assert.Fail("ParseStr did not throw exception when passed zero as TokenNum")
        Catch ex As Exception When ex.Message = "TokenNum must be greater than zero."
        End Try
        Try
            Assert.AreEqual(bpeNullString, MyBase.ParseStr(" ", -1, " "), False)
            Assert.Fail("ParseStr did not throw exception when passed -1 as TokenNum")
        Catch ex As Exception When ex.Message = "TokenNum must be greater than zero."
        End Try

        Assert.AreEqual("fox", MyBase.ParseStr("The quick brown fox jumps over the lazy dog.", 4, " "), False)
        Assert.AreEqual("FiRRe", MyBase.ParseStr("C:\program files\SunGard\FiRRe\Config\Database.dsn", 4, "\"), False)

        Assert.AreEqual("NEW YORK, NY", MyBase.ParseStr("2#26395,,,#1546,10/31/2014,0,Description,Job_ID,Y,N,10/31/2014,0,Description,CHRISTINE CONWAY,EPISCOPAL HEALTH SERVICES,""NEW YORK, NY"",Record 1,MiscData05,MiscData06,MiscData07,7/19/2014,117707,0", 16, ",", """"), False)
        Assert.AreEqual("O'Neil's", MyBase.ParseStr("Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour", 3, " ", """"), False)
        Assert.AreEqual("\\WWS004\SunGard\FiRRe\program files\SunGard\FiRRe\Config\BNYM (Trusted)\WildFiRRe - WWS060.dsn", MyBase.ParseStr("FileDSN=\\WWS004\SunGard\FiRRe\program files\SunGard\FiRRe\Config\BNYM (Trusted)\WildFiRRe - WWS060.dsn", 2, "="), False)

        Dim test As String = "CONVERT(VARCHAR(11),""ACCOUNT"".""NEXT_BILL_DATE"",101) as ""Next Bill Date"""
        Assert.AreEqual("NEXT BILL DATE", mSupport.Strings.OldParseStr(test.ToUpper, 2, " AS ", """"), False, "Using "" AS "" as delimiter")
        Assert.AreEqual("NEXT BILL DATE", MyBase.ParseStr(test.ToUpper, 2, " AS ", """"), False, "Using "" AS "" as delimiter")


        test = "(Select Count(*) From COMPONENT_RECIPIENT    Where COMPONENT_LINK.COMPONENT_NUMBER=COMPONENT_RECIPIENT.COMPONENT_NUMBER And            COMPONENT_LINK.ACCOUNT_NUMBER=COMPONENT_RECIPIENT.ACCOUNT_NUMBER)SPLIT"
        'Dim TestCase() As String = mSupport.Strings.ParseStr(test.ToUpper, " AS ", """")
        Assert.AreEqual(test.ToUpper, mSupport.Strings.OldParseStr(test.ToUpper, 1, " AS ", """"), False, "Using "" AS "" as delimiter")
        Assert.AreEqual(test.ToUpper, MyBase.ParseStr(test.ToUpper, 1, " AS ", """"), False, "Using "" AS "" as delimiter")

        test = """Assembly""=""Microsoft.Office.Interop.Outlook, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C"""
        Assert.AreEqual("""Assembly""", MyBase.ParseStr(test, 1, "=", """", True), False, "Embedded double-quotes (1) [preserve]")
        Assert.AreEqual("Assembly", MyBase.ParseStr(test, 1, "=", """"), False, "Embedded double-quotes (1)")
        Assert.AreEqual("""Microsoft.Office.Interop.Outlook, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C""", MyBase.ParseStr(test, 2, "=", """", True), False, "Embedded double-quotes (2) [preserve]")
        Assert.AreEqual("Microsoft.Office.Interop.Outlook, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C", MyBase.ParseStr(test, 2, "=", """"), False, "Embedded double-quotes (2)")
        Assert.AreEqual(bpeNullString, MyBase.ParseStr(test, 3, "=", """"), False, "Embedded double-quotes (3)")

        test = """Extensibility,version=\""7.0.3300.0\"",publicKeyToken=\""B03F5F7F11D50A3A\"",fileVersion=\""7.0.9466.1\"",culture=\""neutral\""""=hex(7):78,\"""
        Assert.AreEqual("""Extensibility,version=\""7.0.3300.0\"",publicKeyToken=\""B03F5F7F11D50A3A\"",fileVersion=\""7.0.9466.1\"",culture=\""neutral\""""", MyBase.ParseStr(test, 1, "=", """", True), False, "Monster embedded double-quotes (1)")
        Assert.AreEqual("hex(7):78,\""", MyBase.ParseStr(test, 2, "=", """", True), False, "Monster embedded double-quotes (2)")
        Assert.AreEqual(bpeNullString, MyBase.ParseStr(test, 3, "=", """", True), False, "Monster embedded double-quotes (3)")
    End Sub
    <TestMethod()> Public Sub Strings_TokenCount()
        Assert.AreEqual(0, MyBase.TokenCount(Nothing, Nothing))
        Assert.AreEqual(0, MyBase.TokenCount(bpeNullString, bpeNullString))
        Assert.AreEqual(0, MyBase.TokenCount(bpeNullString, ","))
        Assert.AreEqual(0, MyBase.TokenCount("TEST", bpeNullString))
        Assert.AreEqual(1, MyBase.TokenCount("TEST", ","))
        Assert.AreEqual(10, mSupport.Strings.OldTokenCount("The quick brown  fox jumps over the lazy dog.", " "), "Double-space")
        Assert.AreEqual(10, MyBase.TokenCount("The quick brown  fox jumps over the lazy dog.", " "), "Double-space")
        Assert.AreEqual(9, MyBase.TokenCount("The quick brown fox jumps over the lazy dog.", " "))
        Assert.AreEqual(6, MyBase.TokenCount("C:\program files\SunGard\FiRRe\Config\Database.dsn", "\"))
        Assert.AreEqual(23, MyBase.TokenCount("2#26395,,,#1546,10/31/2014,0,Description,Job_ID,Y,N,10/31/2014,0,Description,CHRISTINE CONWAY,EPISCOPAL HEALTH SERVICES,""NEW YORK, NY"",Record 1,MiscData05,MiscData06,MiscData07,7/19/2014,117707,0", ",", """"))
        Assert.AreEqual(10, MyBase.TokenCount("Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour", " ", """"))
        Assert.AreEqual(1, mSupport.Strings.OldTokenCount("Select NAME As [NAME], COLNO As [COLNO] From XSYSCOLUMNS Where TBNAME='COMPONENT_LINK' And TBOWNER='dbo';", ";", "'"))
        Assert.AreEqual(2, mSupport.Strings.OldTokenCount("CONVERT(VARCHAR(11),""ACCOUNT"".""NEXT_BILL_DATE"",101) as ""Next Bill Date""".ToUpper, " AS ", """"), "Using "" AS "" as delimiter")
        Assert.AreEqual(2, MyBase.TokenCount("CONVERT(VARCHAR(11),""ACCOUNT"".""NEXT_BILL_DATE"",101) as ""Next Bill Date""".ToUpper, " AS ", """"), "Using "" AS "" as delimiter")
        Assert.AreEqual(1, MyBase.TokenCount("Select NAME As [NAME], COLNO As [COLNO] From XSYSCOLUMNS Where TBNAME='COMPONENT_LINK' And TBOWNER='dbo';", ";", "'"))
        Assert.AreEqual(2, MyBase.TokenCount("""Assembly""=""Microsoft.Office.Interop.Outlook, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C""", "=", """"))
        Assert.AreEqual(2, MyBase.TokenCount("""Extensibility,version=\""7.0.3300.0\"",publicKeyToken=\""B03F5F7F11D50A3A\"",fileVersion=\""7.0.9466.1\"",culture=\""neutral\""""=hex(7):78,\""", "=", """"))
        Assert.AreEqual(1, mSupport.Strings.OldTokenCount("Format <> 'VHS'", " AND "))
        Assert.AreEqual(1, MyBase.TokenCount("Format <> 'VHS'", " AND "))
        Assert.AreEqual(2, mSupport.Strings.OldTokenCount("TYPE = 'HOT WHEELS®' AND SERIES LIKE '2013 %'", " AND "))
        Assert.AreEqual(2, MyBase.TokenCount("TYPE = 'HOT WHEELS®' AND SERIES LIKE '2013 %'", " AND "))
    End Sub
#End Region
End Class