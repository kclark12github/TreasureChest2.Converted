'clsFieldEditsTest.vb
'   TCBase FieldEdits Class...
'   Copyright © 1998-2017, Ken Clark
'*********************************************************************************************************************************
'   Modification History:
'   Date:       Description:
'   10/06/17    Created;
'=================================================================================================================================
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class clsFieldEditsTest
    Inherits clsTCBaseTest
#Region "Test Methods"
    <TestMethod> Public Sub FieldEdits_ChkHiNumber()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        Dim ImmeasurableeventArgs As KeyPressEventArgs = New KeyPressEventArgs("I"c)

        txtTest.Text = bpeNullString
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkHiNumber(txtTest, ImmeasurableeventArgs), "Return value (""I"" - Immeasurable)") : Assert.AreEqual("Immeasurable", txtTest.Text, False, "txtTest.Text (""I"" - Immeasurable)")

        txtTest.Text = bpeNullString
        Assert.AreEqual(CShort(Asc("9"c)), mSupport.FieldEdits.ChkHiNumber(txtTest, New KeyPressEventArgs("9"c)), "Return value (""9"")")

        txtTest.Text = bpeNullString
        Try : Assert.AreEqual(CShort(Asc("A"c)), mSupport.FieldEdits.ChkHiNumber(txtTest, New KeyPressEventArgs("A"c)), "Return value (""A"")")
            Assert.Fail("ChkHiNumber should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered"
        End Try

        txtTest.Text = "0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc("."c)), mSupport.FieldEdits.ChkHiNumber(txtTest, New KeyPressEventArgs("."c)), "Return value (""."")")
            Assert.Fail("ChkHiNumber should have thrown an exception when passed multiple ""."".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (multiple ""."")"
        End Try

        txtTest.Text = "0. 1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc(" "c)), mSupport.FieldEdits.ChkHiNumber(txtTest, New KeyPressEventArgs(" "c)), "Return value ("" "")")
            Assert.Fail("ChkHiNumber should have thrown an exception when passed "" "".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (embedded space)"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_ChkKey()
        Assert.AreEqual(CShort(Asc("a")), mSupport.FieldEdits.ChkKey(Nothing, New KeyPressEventArgs("a"c)), "ChkKey(Nothing, New KeyPressEventArgs(""a""c)")
        Dim cNum As Short = Asc("a"c) : mSupport.FieldEdits.ChkKey(cNum, True)
        Assert.AreEqual(CShort(Asc("A")), cNum, "ChkKey(cNum, True")

        Dim expected As Short = CShort(vbKeyEnum.vbKeyBell)
        cNum = CShort(Keys.Decimal) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, "Keys.Decimal")
        cNum = CShort(Keys.Divide) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, "Keys.Divide")
        cNum = CShort(Keys.Multiply) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, "Keys.Multiply")
        cNum = CShort(Keys.Space) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, "Keys.Space")
        cNum = Asc("."c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """.""")
        cNum = Asc("/"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """/""")
        cNum = Asc(" "c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """ """)
        cNum = Asc("*"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """*""")
        cNum = Asc("!"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """!""")
        cNum = Asc("@"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """@""")
        cNum = Asc("#"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """#""")
        cNum = Asc("$"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """$""")
        cNum = Asc("^"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """^""")
        cNum = Asc("&"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """&""")
        cNum = Asc("*"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """*""")
        cNum = Asc("`"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """`""")
        cNum = Asc("|"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """|""")
        cNum = Asc("?"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """?""")
        cNum = Asc(","c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """,""")
        cNum = Asc("="c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """=""")
        cNum = Asc(""""c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """")
        cNum = Asc("{"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """{""")
        cNum = Asc("}"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """}""")
        cNum = Asc("["c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """[""")
        cNum = Asc("]"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """]""")
        cNum = Asc("\"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """\""")
        cNum = Asc("<"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """<""")
        cNum = Asc(">"c) : mSupport.FieldEdits.ChkKey(cNum, False) : Assert.AreEqual(expected, cNum, """>""")
        '... and just for good measure, repeat one of the above through ChkKey(Object,KeyPressEventArgs) to be sure exception is thrown...
        Try : Assert.AreEqual(CShort(Asc("@")), mSupport.FieldEdits.ChkKey(Nothing, New KeyPressEventArgs("@"c)), "ChkKey(Nothing, New KeyPressEventArgs(""a""c)")
            Assert.Fail("ChkKey should have thrown an exception when passed ""@"".")
        Catch ex As Exception When ex.Message = "Invalid data entered (@)"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_ChkINumber()
        Assert.AreEqual(CShort(Asc("0"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("0"c)), "Return value (""0"")")
        Assert.AreEqual(CShort(Asc("1"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("1"c)), "Return value (""1"")")
        Assert.AreEqual(CShort(Asc("2"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("2"c)), "Return value (""2"")")
        Assert.AreEqual(CShort(Asc("3"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("3"c)), "Return value (""3"")")
        Assert.AreEqual(CShort(Asc("4"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("4"c)), "Return value (""4"")")
        Assert.AreEqual(CShort(Asc("5"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("5"c)), "Return value (""5"")")
        Assert.AreEqual(CShort(Asc("6"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("6"c)), "Return value (""6"")")
        Assert.AreEqual(CShort(Asc("7"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("7"c)), "Return value (""7"")")
        Assert.AreEqual(CShort(Asc("8"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("8"c)), "Return value (""8"")")
        Assert.AreEqual(CShort(Asc("9"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("9"c)), "Return value (""9"")")

        Assert.AreEqual(CShort(Asc("-"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("-"c)), "Return value (""-"")")
        Try : Assert.AreEqual(CShort(Asc("-"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("-"c), True), "Return value (""-"")")
            Assert.Fail("ChkINumber should have thrown an exception when passed ""-"" when not allowing negative numbers.")
        Catch ex As Exception When ex.Message = "Invalid integer data entered"
        End Try
        Assert.AreEqual(CShort(Keys.Subtract), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Subtract))), "Return value (Keys.Subtract)")
        Try : Assert.AreEqual(CShort(Keys.Subtract), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Subtract)), True), "Return value (Keys.Subtract)")
            Assert.Fail("ChkINumber should have thrown an exception when passed Keys.Subtract when not allowing negative numbers.")
        Catch ex As Exception When ex.Message = "Invalid integer data entered"
        End Try

        Try : Assert.AreEqual(CShort(Asc("A"c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("A"c)), "Return value (""A"")")
            Assert.Fail("ChkINumber should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = "Invalid integer data entered"
        End Try

        Try : Assert.AreEqual(CShort(Asc("."c)), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs("."c)), "Return value (""."")")
            Assert.Fail("ChkINumber should have thrown an exception when passed ""."".")
        Catch ex As Exception When ex.Message = "Invalid integer data entered"
        End Try

        'TODO: There's a long-standing note in ChkINumber questioning why a space is legal...
        'Try : Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Space))), "Return value (Keys.Space)")
        '    Assert.Fail("ChkINumber should have thrown an exception when passed "" "".")
        'Catch ex As Exception When ex.Message = "Invalid integer data entered"
        'End Try
        Assert.AreEqual(CShort(Keys.Space), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Space))), "Keys.Space")
        Assert.AreEqual(CShort(Keys.Back), mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Back))), "Keys.Back")
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Return))), "Keys.Return")
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(Keys.Enter))), "Keys.Enter")

        'Exercise clipboard stuff...
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyCutX))), "vbKeyCutX")
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyCopyC))), "vbKeyCopyC")
        System.Windows.Forms.Clipboard.SetDataObject("12345") : Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyPasteV))), "vbKeyPasteV (""12345"")")
        System.Windows.Forms.Clipboard.SetDataObject("-12345") : Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyPasteV))), "vbKeyPasteV (""-12345"")")
        Try : System.Windows.Forms.Clipboard.SetDataObject("The quick brown fox jumps over the lazy dog.") : Assert.AreEqual(0S, mSupport.FieldEdits.ChkINumber(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyPasteV))), "vbKeyPasteV (""The quick brown fox jumps over the lazy dog."")")
            Assert.Fail("ChkINumber should have thrown an exception when pasting a non-numeric text string")
        Catch ex As Exception When ex.Message = "Invalid integer data entered"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_ChkMaxNumber()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        Dim UnlimitedeventArgs As KeyPressEventArgs = New KeyPressEventArgs("U"c)

        txtTest.Text = bpeNullString
        Assert.AreEqual(0S, mSupport.FieldEdits.ChkMaxNumber(txtTest, UnlimitedeventArgs), "Return value (""U"" - Unlimited)") : Assert.AreEqual("Unlimited.", txtTest.Text, False, "txtTest.Text (""U"" - Unlimited)")

        txtTest.Text = bpeNullString
        Assert.AreEqual(CShort(Asc("9"c)), mSupport.FieldEdits.ChkMaxNumber(txtTest, New KeyPressEventArgs("9"c)), "Return value (""9"")")

        txtTest.Text = bpeNullString
        Try : Assert.AreEqual(CShort(Asc("A"c)), mSupport.FieldEdits.ChkMaxNumber(txtTest, New KeyPressEventArgs("A"c)), "Return value (""A"")")
            Assert.Fail("ChkMaxNumber should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered"
        End Try

        txtTest.Text = "0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc("."c)), mSupport.FieldEdits.ChkMaxNumber(txtTest, New KeyPressEventArgs("."c)), "Return value (""."")")
            Assert.Fail("ChkMaxNumber should have thrown an exception when passed multiple ""."".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (multiple ""."")"
        End Try

        txtTest.Text = "0. 1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc(" "c)), mSupport.FieldEdits.ChkMaxNumber(txtTest, New KeyPressEventArgs(" "c)), "Return value ("" "")")
            Assert.Fail("ChkMaxNumber should have thrown an exception when passed "" "".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (embedded space)"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_ChkRNumber()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"

        txtTest.Text = bpeNullString
        Assert.AreEqual(CShort(Asc("9"c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("9"c)), "Return value (""9"")")

        txtTest.Text = bpeNullString
        Try : Assert.AreEqual(CShort(Asc("A"c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("A"c)), "Return value (""A"")")
            Assert.Fail("ChkRNumber should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered"
        End Try

        txtTest.Text = "0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 3
        Assert.AreEqual(CShort(Asc("."c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("."c)), "Return value (""."")")
        txtTest.Text = "0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc("."c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("."c)), "Return value (""."")")
            Assert.Fail("ChkRNumber should have thrown an exception when passed multiple ""."".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (multiple ""."")"
        End Try

        txtTest.Text = "-0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 4
        Assert.AreEqual(CShort(Asc("-"c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("-"c)), "Return value (""-"")")
        txtTest.Text = "-0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc("-"c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs("-"c)), "Return value (""-"")")
            Assert.Fail("ChkRNumber should have thrown an exception when passed multiple ""-"".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (multiple ""-"")"
        End Try

        txtTest.Text = "0.1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 3
        Assert.AreEqual(CShort(Asc(" "c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs(" "c)), "Return value ("" "")")
        txtTest.Text = "0. 1" : txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Try : Assert.AreEqual(CShort(Asc(" "c)), mSupport.FieldEdits.ChkRNumber(txtTest, New KeyPressEventArgs(" "c)), "Return value ("" "")")
            Assert.Fail("ChkRNumber should have thrown an exception when passed "" "".")
        Catch ex As Exception When ex.Message = "Invalid real number data entered (embedded space)"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_CheckMark()
        Dim chkTest As CheckBox = New CheckBox : chkTest.Name = "chkTest" : chkTest.CheckState = CheckState.Checked
        Assert.AreEqual(CheckState.Checked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(Chr(System.Windows.Forms.Keys.Space))))
        Assert.AreEqual(CheckState.Checked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(Chr(System.Windows.Forms.Keys.X))))
        Assert.AreEqual(CheckState.Checked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(Chr(System.Windows.Forms.Keys.Y))))
        Assert.AreEqual(CheckState.Unchecked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(Chr(System.Windows.Forms.Keys.Back))))
        Assert.AreEqual(CheckState.Unchecked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(Chr(System.Windows.Forms.Keys.N))))
        Assert.AreEqual(CheckState.Unchecked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(CChar(vbCr))))
        chkTest.CheckState = CheckState.Unchecked
        Assert.AreEqual(CheckState.Checked, mSupport.FieldEdits.CheckMark(chkTest, New KeyPressEventArgs(CChar(vbCr))))
    End Sub
    <TestMethod()> Public Sub FieldEdits_ConvertToCSV()
        Assert.AreEqual("", mSupport.FieldEdits.ConvertToCSV(DBNull.Value), False, "DBNull")
        Assert.AreEqual("", mSupport.FieldEdits.ConvertToCSV(Nothing), False, "Nothing")
        Assert.AreEqual("", mSupport.FieldEdits.ConvertToCSV(bpeNullString), False, "bpeNullString")
        Assert.AreEqual("", mSupport.FieldEdits.ConvertToCSV(" "), False, "Space")
        Assert.AreEqual("""Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour, Open Late Fridays""", mSupport.FieldEdits.ConvertToCSV("Stop by O'Neil's Bar & Grill Wednesdays for Happy Hour, Open Late Fridays"), False, "Embedded commas")
    End Sub
    <TestMethod()> Public Sub FieldEdits_ConvertToFlat()
        Assert.AreEqual(New String(" "c, 20), mSupport.FieldEdits.ConvertToFlat(DBNull.Value, 20), False, "DBNull")
        Assert.AreEqual(New String(" "c, 20), mSupport.FieldEdits.ConvertToFlat(Nothing, 20), False, "Nothing")
        Assert.AreEqual(New String(" "c, 20), mSupport.FieldEdits.ConvertToFlat(bpeNullString, 20), False, "bpeNullString")
        Assert.AreEqual(New String(" "c, 20), mSupport.FieldEdits.ConvertToFlat(" ", 20), False, "Space")
        'TODO: The following test was disabled due to the ambiguity represented by the block/line functionality implemented for BNYM's HSP process.
        'Try : Dim tmp As String = mSupport.FieldEdits.ConvertToFlat("The quick brown fox jumps over the lazy dog.", 20)
        '    Assert.Fail("ConvertToFlat failed to throw ArgumentException because string is too long.")
        'Catch ex As ArgumentException When ex.Message.IndexOf("exceeds fill length") > -1
        'End Try
        Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", mSupport.FieldEdits.ConvertToFlat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 26), False, "Full String")
        Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ      ", mSupport.FieldEdits.ConvertToFlat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 32), False, "Partial String")
    End Sub
    <TestMethod> Public Sub FieldEdits_CSVQuote()
        Assert.AreEqual("", mSupport.FieldEdits.CSVQuote(Nothing), False, "Nothing")
        Assert.AreEqual("", mSupport.FieldEdits.CSVQuote(bpeNullString), False, "bpeNullString")
        Assert.AreEqual(" ", mSupport.FieldEdits.CSVQuote(" "), False, "Space")
        Assert.AreEqual("The quick brown fox jumps over the lazy dog.", mSupport.FieldEdits.CSVQuote("The quick brown fox jumps over the lazy dog."), False, "Normal String")
        Assert.AreEqual("""Stop by O'Neil's Bar & Grill Wednesdays for """"Happy Hour"""", Open Late Fridays""", mSupport.FieldEdits.ConvertToCSV("Stop by O'Neil's Bar & Grill Wednesdays for ""Happy Hour"", Open Late Fridays"), False, "Embedded commas, double-quotes and apostrophes")
    End Sub
    <TestMethod()> Public Sub FieldEdits_DecodePercentage()
        'Assert.AreEqual(0.0R, mSupport.FieldEdits.DecodePercentage(DBNull.Value), "DBNull")
        Assert.AreEqual(0.0R, mSupport.FieldEdits.DecodePercentage(Nothing), "Nothing")
        Assert.AreEqual(0.0R, mSupport.FieldEdits.DecodePercentage(bpeNullString), "bpeNullString")
        Assert.AreEqual(0.0R, mSupport.FieldEdits.DecodePercentage(" "), "Space")
        Assert.AreEqual(0.2R, mSupport.FieldEdits.DecodePercentage("20%"), "Includes %")
        Assert.AreEqual(0.255R, mSupport.FieldEdits.DecodePercentage("25.5"), "No %")
    End Sub
    <TestMethod()> Public Sub FieldEdits_ElapsedTime()
        Assert.AreEqual("00:00:00", mSupport.FieldEdits.ElapsedTime(Today, Today), False, "Today/Today")
        Assert.AreEqual("01:00:00", mSupport.FieldEdits.ElapsedTime(CDate("01/01/2014 3:00 PM"), CDate("01/01/2014 4:00 PM")), False, "3PM/4PM")
        Assert.AreEqual("02:45:02", mSupport.FieldEdits.ElapsedTime(CDate("01/01/2014 3:00 PM"), CDate("01/01/2014 5:45:02 PM")), False, "3PM/5:45:02PM")

        Assert.AreEqual("0 Seconds", mSupport.FieldEdits.ElapsedTime(Today, Today, 1), False, "Today/Today")
        Assert.AreEqual("1 Hours, 0 Seconds", mSupport.FieldEdits.ElapsedTime(CDate("01/01/2014 3:00 PM"), CDate("01/01/2014 4:00 PM"), 1), False, "3PM/4PM")
        Assert.AreEqual("2 Hours, 45 Minutes, 2 Seconds", mSupport.FieldEdits.ElapsedTime(CDate("01/01/2014 3:00 PM"), CDate("01/01/2014 5:45:02 PM"), 1), False, "3PM/5:45:02PM")
    End Sub
    <TestMethod()> Public Sub FieldEdits_ExtractPhone()
        Assert.AreEqual("6109753000", mSupport.FieldEdits.ExtractPhone("(610) 975-3000"), False, "(xxx) xxx-xxxx")
    End Sub
    <TestMethod()> Public Sub FieldEdits_FixMoney()
        Assert.AreEqual(Math.Round(1234D, 2), mSupport.FieldEdits.FixMoney(1234D, 2, clsFieldEdits.rrRoundingRule.rrNone), "FixMoney(1234D, 2, rrNone)")
        Assert.AreEqual(Math.Round(1234D, 2), mSupport.FieldEdits.FixMoney(1234D, 2, clsFieldEdits.rrRoundingRule.rrRoundDown), "FixMoney(1234D, 2, rrRoundDown)")
        Assert.AreEqual(Math.Round(1234D, 2), mSupport.FieldEdits.FixMoney(1234D, 2, clsFieldEdits.rrRoundingRule.rrRoundUp), "FixMoney(1234D, 2, rrRoundUp)")
        Assert.AreEqual(1234D, mSupport.FieldEdits.FixMoney(1234D, 2, clsFieldEdits.rrRoundingRule.rrStandard), "FixMoney(1234D, 2, rrStandard)")

        Assert.AreEqual(Math.Round(1234.567D, 2), mSupport.FieldEdits.FixMoney(1234.567D, 2, clsFieldEdits.rrRoundingRule.rrNone), "FixMoney(1234.567D, 2, rrNone)")
        Assert.AreEqual(Math.Round(1234.567D, 2), mSupport.FieldEdits.FixMoney(1234.567D, 2, clsFieldEdits.rrRoundingRule.rrRoundDown), "FixMoney(1234.567D, 2, rrRoundDown)")
        Assert.AreEqual(Math.Round(1234.567D, 2), mSupport.FieldEdits.FixMoney(1234.567D, 2, clsFieldEdits.rrRoundingRule.rrRoundUp), "FixMoney(1234.567D, 2, rrRoundUp)")
        Assert.AreEqual(1234.57D, mSupport.FieldEdits.FixMoney(1234.567D, 2, clsFieldEdits.rrRoundingRule.rrStandard), "FixMoney(1234.567D, 2, rrStandard)")

        Assert.AreEqual(Math.Round(-1234.567D, 2), mSupport.FieldEdits.FixMoney(-1234.567D, 2, clsFieldEdits.rrRoundingRule.rrNone), "FixMoney(-1234.567D, 2, rrNone)")
        Assert.AreEqual(Math.Round(-1234.567D, 2), mSupport.FieldEdits.FixMoney(-1234.567D, 2, clsFieldEdits.rrRoundingRule.rrRoundDown), "FixMoney(-1234.567D, 2, rrRoundDown)")
        Assert.AreEqual(Math.Round(-1234.567D, 2), mSupport.FieldEdits.FixMoney(-1234.567D, 2, clsFieldEdits.rrRoundingRule.rrRoundUp), "FixMoney(-1234.567D, 2, rrRoundUp)")
        Assert.AreEqual(-1234.57D, mSupport.FieldEdits.FixMoney(-1234.567D, 2, clsFieldEdits.rrRoundingRule.rrStandard), "FixMoney(-1234.567D, 2, rrStandard)")

        Assert.AreEqual(Math.Round(CDec(1.0R / 3.0R), 2), mSupport.FieldEdits.FixMoney(1D / 3D, 2, clsFieldEdits.rrRoundingRule.rrNone), "FixMoney(1D / 3D, 2, rrNone)")
        Assert.AreEqual(Math.Round(CDec(1.0R / 3.0R), 2), mSupport.FieldEdits.FixMoney(1D / 3D, 2, clsFieldEdits.rrRoundingRule.rrRoundDown), "FixMoney(1D / 3D, 2, rrRoundDown)")
        Assert.AreEqual(Math.Round(CDec(1.0R / 3.0R), 2), mSupport.FieldEdits.FixMoney(1D / 3D, 2, clsFieldEdits.rrRoundingRule.rrRoundUp), "FixMoney(1D / 3D, 2, rrRoundUp)")
        Assert.AreEqual(0.33D, mSupport.FieldEdits.FixMoney(1D / 3D, 2, clsFieldEdits.rrRoundingRule.rrStandard), "FixMoney(1D / 3D, 2, rrStandard)")
    End Sub
    <TestMethod()> Public Sub FieldEdits_FixQuotes()
        Assert.AreEqual(" ", MyBase.FixQuotes(DBNull.Value), False, "DBNull")
        Assert.AreEqual(" ", MyBase.FixQuotes(Nothing), False, "Nothing")
        Assert.AreEqual(" ", MyBase.FixQuotes(bpeNullString), False, "bpeNullString")
        Assert.AreEqual(" ", MyBase.FixQuotes(" "), False, "Space")
        Assert.AreEqual("The quick brown fox jumps over the lazy dog.", MyBase.FixQuotes("The quick brown fox jumps over the lazy dog."), False, "No quotes")
        Assert.AreEqual("Stop by O''Neil''s Bar & Grill Wednesdays for ""Happy Hour""", MyBase.FixQuotes("Stop by O'Neil's Bar & Grill Wednesdays for ""Happy Hour"""), False, "Multiple single-quotes")
    End Sub
    <TestMethod> Public Sub FieldEdits_FormatBytes()
        Assert.AreEqual("1,023 Bytes", mSupport.FieldEdits.FormatBytes(1023), False, "Bytes")
        Assert.AreEqual("64.00 KB", mSupport.FieldEdits.FormatBytes(1024.0R * 64.0R), False, "KB")
        Assert.AreEqual("64.50 MB", mSupport.FieldEdits.FormatBytes((1024.0R * 1024.0R) * 64.5R), False, "MB")
        Assert.AreEqual("64.50 GB", mSupport.FieldEdits.FormatBytes((1024.0R * 1024.0R * 1024.0R) * 64.5R), False, "GB")
        Assert.AreEqual("4.25 TB", mSupport.FieldEdits.FormatBytes((1024.0R * 1024.0R * 1024.0R * 1024.0R) * 4.25R), False, "TB")
    End Sub
    <TestMethod> Public Sub FieldEdits_FormatElapsed()
        Assert.AreEqual("14 Days, 6 Hours, 56 Minutes, 7.890 Seconds", mSupport.FieldEdits.FormatElapsed(1234567890, False), False, "123456789")
        Assert.AreEqual("2 Minutes, 3.456 Seconds", mSupport.FieldEdits.FormatElapsed(123456, False), False, "123456")
        Assert.AreEqual("1.234 Seconds", mSupport.FieldEdits.FormatElapsed(1234, False), False, "1234")

        Assert.AreEqual("14.06:56:07.890", mSupport.FieldEdits.FormatElapsed(1234567890, True), False, "123456789")
        Assert.AreEqual("00:02:03.456", mSupport.FieldEdits.FormatElapsed(123456, True), False, "123456")
        Assert.AreEqual("00:00:01.234", mSupport.FieldEdits.FormatElapsed(1234, True), False, "1234")
    End Sub
    <TestMethod> Public Sub FieldEdits_FormatMoney()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest" : txtTest.MaxLength = 0
        txtTest.Text = "15.5" : mSupport.FieldEdits.FormatMoney(txtTest) : Assert.AreEqual(System.Drawing.Color.Black, txtTest.ForeColor, "Positive")
        txtTest.Text = "-15.5" : mSupport.FieldEdits.FormatMoney(txtTest) : Assert.AreEqual(System.Drawing.Color.Red, txtTest.ForeColor, "Negative")

        txtTest.Text = "15.5" : mSupport.FieldEdits.FormatMoney(txtTest, System.Drawing.Color.Red, System.Drawing.Color.Black) : Assert.AreEqual(System.Drawing.Color.Black, txtTest.ForeColor, "Positive")
        txtTest.Text = "-15.5" : mSupport.FieldEdits.FormatMoney(txtTest, System.Drawing.Color.Red, System.Drawing.Color.Black) : Assert.AreEqual(System.Drawing.Color.Red, txtTest.ForeColor, "Negative")

        Dim originalText As String
        originalText = "12345.678" : txtTest.Text = originalText : mSupport.FieldEdits.FormatMoney(txtTest) : Assert.AreEqual("$12,345.68", txtTest.Text, False, originalText)
        'MaxLength Overflow...
        Try : txtTest.MaxLength = 12
            originalText = "1234567890123.99" : txtTest.Text = originalText : mSupport.FieldEdits.FormatMoney(txtTest) : Assert.AreEqual("$1,234,567,890,123.99", txtTest.Text, False, originalText)
        Catch ex As Exception When ex.Message.EndsWith("Value cannot exceed 999,999")
        End Try

        'OverflowException...
        Try : txtTest.Text = "999999999999999999999999999999.98888" : mSupport.FieldEdits.FormatMoney(txtTest, System.Drawing.Color.Red, System.Drawing.Color.Black)
            Assert.Fail("FormatMoney should have thrown an OverflowException.")
        Catch ex As OverflowException
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_FormatPercentage()
        Dim data As String = Nothing : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("0.00 %", data, False, "Nothing")
        data = bpeNullString : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("0.00 %", data, False, "bpeNullString")
        data = "0.15" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("0.15 %", data, False, "0.15")
        data = "15%" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("15.00 %", data, False, "15%")
        data = "15 %" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("15.00 %", data, False, "15 %")
        data = "0.155" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("0.16 %", data, False, "0.155")
        data = "15.5%" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("15.50 %", data, False, "15.5%")
        data = "15.5 %" : mSupport.FieldEdits.FormatPercentage(data) : Assert.AreEqual("15.50 %", data, False, "15.5 %")

        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        txtTest.Text = "15.5 %" : mSupport.FieldEdits.FormatPercentage(txtTest) : Assert.AreEqual(System.Drawing.Color.Black, txtTest.ForeColor, "Positive %")
        txtTest.Text = "-15.5 %" : mSupport.FieldEdits.FormatPercentage(txtTest) : Assert.AreEqual(System.Drawing.Color.Red, txtTest.ForeColor, "Negative %")

        txtTest.Text = "15.5 %" : mSupport.FieldEdits.FormatPercentage(txtTest, System.Drawing.Color.Red, System.Drawing.Color.Black) : Assert.AreEqual(System.Drawing.Color.Black, txtTest.ForeColor, "Positive %")
        txtTest.Text = "-15.5 %" : mSupport.FieldEdits.FormatPercentage(txtTest, System.Drawing.Color.Red, System.Drawing.Color.Black) : Assert.AreEqual(System.Drawing.Color.Red, txtTest.ForeColor, "Negative %")
    End Sub
    <TestMethod> Public Sub FieldEdits_FormatSeconds()
        Assert.AreEqual("123456789.00000", mSupport.FieldEdits.FormatSeconds(123456789.0R), False, "123456789.0R")
        Assert.AreEqual("123456.78900", mSupport.FieldEdits.FormatSeconds(123456.789R), False, "123456.789R")

        Assert.AreEqual("34293 Hours, 33 Minutes, 9 Seconds", mSupport.FieldEdits.FormatSeconds(123456789.0R, 1), False, "123456789.0R")
        Assert.AreEqual("34 Hours, 17 Minutes, 37 Seconds", mSupport.FieldEdits.FormatSeconds(123456.789R, 1), False, "123456.789R")
        Assert.AreEqual("20 Minutes, 35 Seconds", mSupport.FieldEdits.FormatSeconds(1234.567R, 1), False, "1234.567R")
    End Sub
    <TestMethod> Public Sub FieldEdits_GetUnselectedText()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"

        txtTest.Text = bpeNullString
        Assert.AreEqual(bpeNullString, mSupport.FieldEdits.GetUnselectedText(txtTest), "bpeNullString")

        txtTest.Text = "The quick brown fox jumps over the lazy dog."
        txtTest.SelectionStart = 0 : txtTest.SelectionLength = 0
        Assert.AreEqual(txtTest.Text, mSupport.FieldEdits.GetUnselectedText(txtTest), "Nothing Selected")
        txtTest.SelectionStart = 0 : txtTest.SelectionLength = txtTest.Text.Length
        Assert.AreEqual(bpeNullString, mSupport.FieldEdits.GetUnselectedText(txtTest), "Everything Selected")
        txtTest.SelectionStart = 0 : txtTest.SelectionLength = txtTest.Text.Length - 1
        Assert.AreEqual(".", mSupport.FieldEdits.GetUnselectedText(txtTest), "All but last")
        txtTest.SelectionStart = 4 : txtTest.SelectionLength = 35
        Assert.AreEqual("The lazy dog.", mSupport.FieldEdits.GetUnselectedText(txtTest), "Middle Selected")
    End Sub
    <TestMethod> Public Sub FieldEdits_KeyPressUcase()
        Dim cNum As Short = Asc("a"c) : mSupport.FieldEdits.KeyPressUcase(cNum) : Assert.AreEqual(CShort(Asc("A"c)), cNum, """a""")
    End Sub
    <TestMethod> Public Sub FieldEdits_KeyPressInteger()
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("0"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("1"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("2"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("3"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("4"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("5"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("6"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("7"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("8"c))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("9"c))

        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs(Chr(Keys.Subtract)))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs(Chr(Keys.Space)))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs(Chr(Keys.Back)))
        mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyMinus)))

        Try : mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("A"c))
            Assert.Fail("KeyPressInteger should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = """A"" is not a valid integer digit"
        End Try

        Try : mSupport.FieldEdits.KeyPressInteger(Nothing, New KeyPressEventArgs("."c))
            Assert.Fail("KeyPressInteger should have thrown an exception when passed ""."".")
        Catch ex As Exception When ex.Message = """."" is not a valid integer digit"
        End Try
    End Sub
    <TestMethod> Public Sub FieldEdits_KeyPressReal()
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("0"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("1"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("2"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("3"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("4"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("5"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("6"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("7"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("8"c))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("9"c))

        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs(Chr(Keys.Subtract)))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs(Chr(Keys.Space)))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs(Chr(Keys.Back)))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyPeriod)))
        mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs(Chr(vbKeyEnum.vbKeyMinus)))

        Try : mSupport.FieldEdits.KeyPressReal(Nothing, New KeyPressEventArgs("A"c))
            Assert.Fail("KeyPressReal should have thrown an exception when passed ""A"".")
        Catch ex As Exception When ex.Message = """A"" is not a valid decimal-number digit"
        End Try
    End Sub
    <TestMethod()> Public Sub FieldEdits_LeftFill()
        Assert.AreEqual(New String(" "c, 80), MyBase.LeftFill(DBNull.Value, 80), False, "DBNull")
        Assert.AreEqual(New String(" "c, 80), MyBase.LeftFill(Nothing, 80), False, "Nothing")
        Assert.AreEqual(New String(" "c, 80), MyBase.LeftFill(bpeNullString, 80), False, "bpeNullString")
        Assert.AreEqual(New String(" "c, 80), MyBase.LeftFill(" ", 80), False, "Space")
        Try : MyBase.LeftFill("12345678901234567890", 10)
            Assert.Fail("LeftFill failed to throw ArgumentException because string is too long.")
        Catch ex As ArgumentException When ex.Message.IndexOf("exceeds fill length") > -1
        End Try
        Assert.AreEqual("12345678901234567890", MyBase.LeftFill("12345678901234567890", 20), False, "Full String")
        Assert.AreEqual("12345", MyBase.LeftFill(12345, 5), False, "Full Positive Integer (Space)")
        Assert.AreEqual("-12345", MyBase.LeftFill(-12345, 6), False, "Full Negative Integer (Space)")
        Assert.AreEqual("12345     ", MyBase.LeftFill(12345, 10), False, "Full Positive Integer (Space)")
        Assert.AreEqual("-12345    ", MyBase.LeftFill(-12345, 10), False, "Full Negative Integer (Space)")
        Assert.AreEqual("1234567890.123456789", MyBase.LeftFill(1234567890.123456789D, 20), False, "Full Positive Decimal (Space)")
        Assert.AreEqual("-1234567890.12345678", MyBase.LeftFill(-1234567890.12345678D, 20), False, "Full Negative Decimal (Space)")
        Assert.AreEqual("1234567890.123456   ", MyBase.LeftFill(1234567890.123456D, 20), False, "Positive Decimal (Space)")
        Assert.AreEqual("-1234567890.123456  ", MyBase.LeftFill(-1234567890.123456D, 20), False, "Negative Decimal (Space)")
        Assert.AreEqual("1234567890.123456000", MyBase.LeftFill(1234567890.123456D, 20, "0"), False, "Positive Decimal (Zero)")
        Assert.AreEqual("-1234567890.12345600", MyBase.LeftFill(-1234567890.123456D, 20, "0"), False, "Negative Decimal (Zero)")
    End Sub
    <TestMethod()> Public Sub FieldEdits_RightFill()
        Assert.AreEqual(New String(" "c, 80), MyBase.RightFill(DBNull.Value, 80), False, "DBNull")
        Assert.AreEqual(New String(" "c, 80), MyBase.RightFill(Nothing, 80), False, "Nothing")
        Assert.AreEqual(New String(" "c, 80), MyBase.RightFill(bpeNullString, 80), False, "bpeNullString")
        Assert.AreEqual(New String(" "c, 80), MyBase.RightFill(" ", 80), False, "Space")
        Try : MyBase.RightFill("12345678901234567890", 10)
            Assert.Fail("RightFill failed to throw ArgumentException because string is too long.")
        Catch ex As ArgumentException When ex.Message.IndexOf("exceeds fill length") > -1
        End Try
        Assert.AreEqual("12345678901234567890", MyBase.RightFill("12345678901234567890", 20), False, "Full String")
        Assert.AreEqual("12345", MyBase.RightFill(12345, 5), False, "Full Positive Integer (Space)")
        Assert.AreEqual("-12345", MyBase.RightFill(-12345, 6), False, "Full Negative Integer (Space)")
        Assert.AreEqual("     12345", MyBase.RightFill(12345, 10), False, "Positive Integer (Space)")
        Assert.AreEqual("    -12345", MyBase.RightFill(-12345, 10), False, "Negative Integer (Space)")
        Assert.AreEqual("1234567890.123456789", MyBase.RightFill(1234567890.123456789D, 20), False, "Full Positive Decimal (Space)")
        Assert.AreEqual("-1234567890.12345678", MyBase.RightFill(-1234567890.12345678D, 20), False, "Full Negative Decimal (Space)")
        Assert.AreEqual("   1234567890.123456", MyBase.RightFill(1234567890.123456D, 20), False, "Positive Decimal (Space)")
        Assert.AreEqual("  -1234567890.123456", MyBase.RightFill(-1234567890.123456D, 20), False, "Negative Decimal (Space)")
        Assert.AreEqual("0001234567890.123456", MyBase.RightFill(1234567890.123456D, 20, "0"), False, "Positive Decimal (Zero)")
        Assert.AreEqual("-001234567890.123456", MyBase.RightFill(-1234567890.123456D, 20, "0"), False, "Negative Decimal (Zero)")
    End Sub
    <TestMethod()> Public Sub FieldEdits_RndCents()
        Assert.AreEqual(1234D, mSupport.FieldEdits.RndCents(1234D), "RndCents(1234.0D)")
        Assert.AreEqual(1234.57D, mSupport.FieldEdits.RndCents(1234.567D), "RndCents(1234.567D)")
        Assert.AreEqual(-1234.57D, mSupport.FieldEdits.RndCents(-1234.567D), "RndCents(-1234.567D)")
        Assert.AreEqual(0.33D, mSupport.FieldEdits.RndCents(1D / 3D), "RndCents(1.0D / 3.0D)")
    End Sub
    <TestMethod> Public Sub FieldEdits_SetTitleCase()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        txtTest.Text = "The quick brown fox jumps over the lazy dog." : mSupport.FieldEdits.SetTitleCase(txtTest)
        Assert.AreEqual("The Quick Brown Fox Jumps Over The Lazy Dog.", txtTest.Text, False)
    End Sub
    <TestMethod> Public Sub FieldEdits_SetUpperCase()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        txtTest.Text = "The quick brown fox jumps over the lazy dog." : mSupport.FieldEdits.SetUpperCase(txtTest)
        Assert.AreEqual("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.", txtTest.Text, False)
    End Sub
    <TestMethod()> Public Sub FieldEdits_szStringToString()
        Assert.AreEqual(bpeNullString, mSupport.FieldEdits.szStringToString(Nothing), False, "szStringToString(Nothing)")
        Assert.AreEqual(bpeNullString, mSupport.FieldEdits.szStringToString(bpeNullString), False, "szStringToString(bpeNullString)")
        Assert.AreEqual("$1,234.56- USD", mSupport.FieldEdits.szStringToString("$1,234.56- USD"), False, "szStringToString(""$1,234.56-"")")
        Assert.AreEqual("$1,234.56- USD", mSupport.FieldEdits.szStringToString(String.Format("$1,234.56- USD{0}", Chr(0))), False, "szStringToString(""$1,234.56-{{0}}"")")
        Assert.AreEqual("$1,234", mSupport.FieldEdits.szStringToString(String.Format("$1,234{0}.56- USD", Chr(0))), False, "szStringToString(""$1,234{{0}}.56-"")")
        Assert.AreEqual(bpeNullString, mSupport.FieldEdits.szStringToString(String.Format("{0}$1,234.56- USD", Chr(0))), False, "szStringToString(""{{0}}$1,234.56-"")")
    End Sub
    <TestMethod> Public Sub FieldEdits_Tags()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        mSupport.FieldEdits.TagSet(txtTest, "Ignore")
        mSupport.FieldEdits.TagSet(txtTest, "IGNORE")
        mSupport.FieldEdits.TagSet(txtTest, "IGN")
        mSupport.FieldEdits.TagSet(txtTest, "READ-ONLY")
        Assert.AreEqual(True, mSupport.FieldEdits.TagContains(txtTest, "ignore"), """ignore""")
        Assert.AreEqual(False, mSupport.FieldEdits.TagContains(txtTest, "WRITE-ONLY"), """WRITE-ONLY""")
        mSupport.FieldEdits.TagRemove(txtTest, "ign")
        mSupport.FieldEdits.TagRemove(txtTest, "ignore")
        mSupport.FieldEdits.TagRemove(txtTest, "ign")
        Assert.AreEqual(False, mSupport.FieldEdits.TagContains(txtTest, "ign"), """ign""")
        mSupport.FieldEdits.TagRemove(txtTest, "read-only")
    End Sub
    <TestMethod> Public Sub FieldEdits_TextKeyDown()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest"
        'F10 (or any other code) - Unchanged...
        txtTest.Text = "The quick brown fox jumps over the lazy dog." : MyBase.TextKeyDown(txtTest, CShort(Keys.F10), 0)
        Assert.AreEqual("The quick brown fox jumps over the lazy dog.", txtTest.Text, False)
        'F11 - TitleCase
        MyBase.TextKeyDown(txtTest, CShort(Keys.F11), 0)
        Assert.AreEqual("The Quick Brown Fox Jumps Over The Lazy Dog.", txtTest.Text, False)
        'F12 - UpperCase
        MyBase.TextKeyDown(txtTest, CShort(Keys.F12), 0)
        Assert.AreEqual("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.", txtTest.Text, False)
    End Sub
    <TestMethod> Public Sub FieldEdits_TextSelected()
        Dim txtTest As TextBox = New TextBox : txtTest.Name = "txtTest" : txtTest.Text = "12345"
        MyBase.TextSelected(txtTest) : Assert.AreEqual("12345", txtTest.SelectedText, "txtTest.Text = ""12345""")

        'TODO: This ComboBox functionality is problematic...
        'Dim cbTest As SIASSupport.ComboBox = New SIASSupport.ComboBox() : cbTest.Name = "cbTest" : cbTest.DropDownStyle = ComboBoxStyle.DropDown
        'cbTest.Items.AddRange(New String() {"Vanilla", "Chocolate", "Strawberry", "Blueberry", "Banana"}) : cbTest.SelectedIndex = 3
        'Assert.AreEqual(cbTest.SelectedItem, cbTest.Text, False, "cbTest.SelectedItem = ""{0}"" but cbTest.Text = ""{1}""", cbTest.SelectedItem, cbTest.Text)
        'MyBase.TextSelected(cbTest) : Assert.AreEqual(cbTest.SelectedItem, cbTest.SelectedText, False, "cbTest.SelectedItem = ""{0}"" but cbTest.SelectedText = ""{1}""", cbTest.SelectedItem, cbTest.SelectedText)
    End Sub
    <TestMethod> Public Sub FieldEdits_UpCase()
        Assert.AreEqual(CShort(Asc("A"c)), MyBase.UpCase(Asc("a"c)), """a""")
    End Sub
    <TestMethod()> Public Sub FieldEdits_Verify()
        Assert.AreEqual(12S, MyBase.Verify("$1,234.56- USD", "0123456789$,.- "), "Verify(""$1,234.56-"",""0123456789$,.- "")")
        Assert.AreEqual(1S, MyBase.Verify("$1,234.56- USD", "-0123456789"), "Verify(""$1,234.56-"",""-0123456789"")")
    End Sub
#End Region
End Class
