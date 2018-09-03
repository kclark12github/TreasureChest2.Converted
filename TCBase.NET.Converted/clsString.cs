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
using static TCBase.clsTrace;
//clsString - clsString.vb
//   TreasureChest2 String Support Class...
//   Copyright © 1998-2018, Ken Clark
//Portions [Public Domain], taken from "The Waite Group's Visual Basic Source Library"/SAMS Publishing and...
//from Bruce McKinney's "Hardcore Visual Basic"/Microsoft Press...
//*********************************************************************************************************************************
//   Modification History:
//   Date:       Description:
//   04/05/18    Introduced TrimTabs;
//   12/01/14    Reworked ParseStr and TokenCount to accommodate encapsulated sections of strings with embedded delimiters 
//               (correcting gaps in functionality);
//   11/12/14    Removed "static" variables from ParseStr effectively eliminating "AutoMode" functionality (the ability to pass a 
//               zero Token number to get the next one) as such a feature was not used, and use of these "static" variables made 
//               the function  unreliable in a multi-threaded environment;
//               Changed access from Public to Private for those functions found unused (and moved into "Unused Methods" region;
//               Added validations (exercised in new unit tests);
//               Documented commonly used functions;
//   01/21/14    Eliminated use of RecordEntry/RecordExit for performance;
//   03/18/13    Moved AssignCSV, CSVQuote and ParseCSV from FiRRe's modLoader;
//   03/11/05    Made New() constructor "Friend" in order to accomplish PublicNotCreatable VB6 Class Instancing behavior;
//   02/21/05    Upgraded for use as a VB.NET Component;
//   04/05/03    Added CountStr();
//   10/06/01    Created from libParseStr;
//=================================================================================================================================
// ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	public class clsString : clsSupportBase
	{
		internal clsString(clsSupport objSupport) : base(objSupport, "clsString")
		{
			Trace("objSupport:={" + objSupport.GetType().ToString() + "}", trcOption.trcSupport);
			//Do more stuff, if necessary...
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		private const string sPunctuation = ",.;:!";
		private const string sWhitePunct = Constants.vbCrLf + ",.;:!";
		[DllImport("kernel32", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int lstrlen(string lpString);
		public enum EHexDump
		{
			ehdOneColumn,
			ehdTwoColumn,
			ehdEndless,
			ehdSample8,
			ehdSample16
		}
		public enum ESearchOptions
		{
			esoCaseSense = 0x1,
			esoBackward = 0x2,
			esoWholeWord = 0x4
		}
		public enum OpMode
		{
			StringBinaryCompare = CompareMethod.Binary + 1,
			StringTextCompare = CompareMethod.Text + 1,
			CharacterBinaryCompare = -(CompareMethod.Binary + 1),
			CharacterTextCompare = -(CompareMethod.Text + 1)
		}
		#endregion
		#region "Methods"
		#region "Unused Methods"
		//Public Function FilterLike(ByRef vInput As Object, ByRef sLike As String, Optional ByRef fInclude As Boolean = True) As Object
		//    'Const EntryName As String = "FilterLike"
		//    'Filter in or out strings that match a pattern based on those recognized by the Like operator
		//    Dim asRet() As String = {}
		//    Dim c, i As Integer
		//    Dim s As String
		//    FilterLike = Nothing
		//    'Try
		//    '    RecordEntry(EntryName, "vInput:={" & vInput.GetType.ToString & "}, sLike:=""" & sLike & ", fInclude:=" & fInclude.ToString, trcOption.trcSupport)
		//        While True
		//            Try
		//                For i = 0 To UBound(vInput)
		//                    s = vInput(i)
		//                    If s Like sLike Then
		//                        If fInclude Then
		//                            asRet(c) = s
		//                            c = c + 1
		//                        End If
		//                    Else
		//                        If Not fInclude Then
		//                            asRet(c) = s
		//                            c = c + 1
		//                        End If
		//                    End If
		//                Next
		//                ReDim Preserve asRet(c - 1)
		//                FilterLike = VB6.CopyArray(asRet)
		//                Exit While
		//            Catch ex As ArgumentException
		//                Const cChunk As Integer = 20
		//                ReDim Preserve asRet(c + cChunk)
		//            End Try
		//        End While
		//    'Catch ex As Exception
		//    '    RaiseError(EntryName, ex)
		//    'End Try
		//    'RecordExit(EntryName, False, FilterLike)
		//End Function
		private int FindString(ref string sTarget, ref string sFind, int iPos = 0, ESearchOptions esoOptions = 0)
		{
			int functionReturnValue = 0;
            CompareMethod ordComp = CompareMethod.Binary;
			int cFind = 0;
			bool fBack = false;
			//Get the compare method
			if (esoOptions.HasFlag(ESearchOptions.esoCaseSense)) {
				ordComp = CompareMethod.Binary;
			} else {
				ordComp = CompareMethod.Text;
			}
			//Set up first search
			cFind = Strings.Len(sFind);

			//If Len(sFind) = 1 Then iPos = iPos + 1 'cml

			if (iPos == 0)
				iPos = 1;
			if (esoOptions.HasFlag(ESearchOptions.esoBackward))
				fBack = true;
            bool continueLoop = true;
			do {
				//Find the string
				if (fBack) {
					iPos = Strings.InStrRev(sTarget, sFind, iPos, ordComp);
				} else {
					iPos = Strings.InStr(iPos, sTarget, sFind, ordComp);
				}
				//If not found, we're done
				if (iPos == 0)
					return functionReturnValue;
				//Try
				if (esoOptions.HasFlag(ESearchOptions.esoWholeWord)) {
                    //If it's supposed to be whole word and is, we're done
                    if (IsWholeWord(ref sTarget, iPos, Strings.Len(sFind)))
                    {
                        continueLoop = false;
                    }
					//Otherwise, set up next search
					else if (fBack) {
						iPos = iPos - cFind;
						if (iPos < 1)
							return functionReturnValue;
						//Try
					} else {
						iPos = iPos + cFind;
						if (iPos > Strings.Len(sTarget))
							return functionReturnValue;
						//Try
					}
				} else {
                    //If it wasn't a whole word search, we're done
                    continueLoop = false;
				}
			} while (continueLoop);
			functionReturnValue = iPos;
			return functionReturnValue;
		}
		private string FmtHex(int i, short iWidth = 8)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = Strings.Right(new string('0', iWidth) + Conversion.Hex(i), iWidth);
			return functionReturnValue;
		}
		private string FmtInt(int iVal, short iWidth, bool fRight = true)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = (fRight ? Strings.Right(Strings.Space(iWidth) + iVal, iWidth) : Strings.Left(iVal + Strings.Space(iWidth), iWidth));
			return functionReturnValue;
		}
		private string FmtStr(ref string s, short iWidth, bool fRight = true)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = (fRight ? Strings.Left(s + Strings.Space(iWidth), iWidth) : Strings.Right(Strings.Space(iWidth) + s, iWidth));
			return functionReturnValue;
		}
		string static_GetNextLine_sSave;
		int static_GetNextLine_iStart;
		int static_GetNextLine_cSave;
		private string GetNextLine(string sSource = clsSupport.bpeNullString)
		{
			string functionReturnValue = null;
			int iEnd = 0;
			functionReturnValue = clsSupport.bpeNullString;
			//Returns a line from a string, where a "line" is all characters
			//up to and including a carriage return/line feed. GetNextLine
			//works the same way as GetToken. The first call to GetNextLine
			//should pass the string to parse; subsequent calls should pass
			//an empty string. GetNextLine returns an empty string after all lines
			//have been read from the source string.

			//Initialize GetNextLine
			if ((sSource != clsSupport.bpeNullString)) {
				static_GetNextLine_iStart = 1;
				static_GetNextLine_sSave = sSource;
				static_GetNextLine_cSave = Strings.Len(static_GetNextLine_sSave);
			} else {
				if (static_GetNextLine_sSave == clsSupport.bpeNullString)
					return functionReturnValue;
				//Try
			}

			//iStart points to first character after the previous sCrLf
			iEnd = Strings.InStr(static_GetNextLine_iStart, static_GetNextLine_sSave, Constants.vbCrLf);

			if (iEnd > 0) {
				//Return line
				functionReturnValue = Strings.Mid(static_GetNextLine_sSave, static_GetNextLine_iStart, iEnd - static_GetNextLine_iStart + 2);
				static_GetNextLine_iStart = iEnd + 2;
				if (static_GetNextLine_iStart > static_GetNextLine_cSave)
					static_GetNextLine_sSave = clsSupport.bpeNullString;
			} else {
				//Return remainder of string as a line
				functionReturnValue = Strings.Mid(static_GetNextLine_sSave, static_GetNextLine_iStart) + Constants.vbCrLf;
				static_GetNextLine_sSave = clsSupport.bpeNullString;
			}
			return functionReturnValue;
		}
		private string HexDump(ref byte[] ab, EHexDump ehdFmt = EHexDump.ehdOneColumn)
		{
			string functionReturnValue = null;
			string sDump = clsSupport.bpeNullString;
			string sAscii = clsSupport.bpeNullString;
			int iColumn = 0;
			int iCur = 0;
			string sCur = clsSupport.bpeNullString;
			string sLine = clsSupport.bpeNullString;
			int i = 0;
			functionReturnValue = clsSupport.bpeNullString;
			switch (ehdFmt) {
				case EHexDump.ehdOneColumn:
				case EHexDump.ehdSample8:
					iColumn = 8;
					break;
				case EHexDump.ehdTwoColumn:
				case EHexDump.ehdSample16:
					iColumn = 16;
					break;
				case EHexDump.ehdEndless:
					iColumn = 32767;
					break;
			}
            bool continueLoop = true;
			for (i = Information.LBound(ab); i <= Information.UBound(ab) && continueLoop; i++) {
				//Get current character
				iCur = ab[i];
				sCur = Strings.Chr(iCur).ToString();

				//Append its hex value
				sLine += Strings.Right("0" + Conversion.Hex(iCur), 2) + " ";

				//Append its ASCII value or dot
				if (ehdFmt <= EHexDump.ehdTwoColumn) {
					if (iCur >= 32 && iCur < 127) {
						sAscii = sAscii + sCur;
					} else {
						sAscii = sAscii + ".";
					}
				}

				//Append ASCII to dump and wrap every paragraph
				if ((i + 1) % 8 == 0)
					sLine = sLine + " ";
				if ((i + 1) % iColumn == 0) {
                    if (ehdFmt >= EHexDump.ehdSample8) {
                        sLine = sLine + "...";
                        continueLoop = false;
                    } else {
                        sLine = sLine + " " + sAscii + Constants.vbCrLf;
                        sDump = sDump + sLine;
                        sAscii = clsSupport.bpeNullString;
                        sLine = clsSupport.bpeNullString;
                    }
				}
			}

			if (ehdFmt <= EHexDump.ehdTwoColumn) {
				if (((i + 1) % iColumn) > 0) {
					if (ehdFmt > 0) {
						sLine = Strings.Left(sLine + Strings.Space(53), 53) + sAscii;
					} else {
						sLine = Strings.Left(sLine + Strings.Space(26), 26) + sAscii;
					}
				}
				sDump = sDump + sLine;
			} else {
				sDump = sLine;
			}
			functionReturnValue = sDump;
			return functionReturnValue;
		}
		private string HexDumpB(ref string s, EHexDump ehdFmt = EHexDump.ehdOneColumn)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;

			//UPGRADE_TODO: Code was upgraded to use System.Text.UnicodeEncoding.Unicode.GetBytes() which may not have the same behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1059"'
			byte[] ab = System.Text.UnicodeEncoding.Unicode.GetBytes(s);
			functionReturnValue = HexDump(ref ab, ehdFmt);
			return functionReturnValue;
		}
		private bool IsWholeWord(ref string sTarget, int iPos, int cFind)
		{
			bool functionReturnValue = false;
			//Checks for white space and punctuation around a substring (see above)
			string sChar = null;
			//Check character before
			if (iPos > 1) {
				sChar = Strings.Mid(sTarget, iPos - 1, 1);
				if (Strings.InStr(sWhitePunct, sChar) == 0)
					return functionReturnValue;
				//Try
			}
			//Check character after
			if (iPos < Strings.Len(sTarget) - 1) {
				sChar = Strings.Mid(sTarget, iPos + cFind, 1);
				if (Strings.InStr(sWhitePunct, sChar) == 0)
					return functionReturnValue;
				//Try
			}
			functionReturnValue = true;
			return functionReturnValue;
		}
		private string RTrimLine(ref string sLine)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			//Strips off trailing carriage return/line feed
			functionReturnValue = (Strings.Right(sLine, 2) == Constants.vbCrLf ? Strings.Left(sLine, Strings.Len(sLine) - 2) : functionReturnValue = sLine);
			return functionReturnValue;
		}
		private string StrZToStr(ref string s)
		{
			string functionReturnValue = null;
			//Strip junk at end from null-terminated string
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = Strings.Left(s, lstrlen(s));
			return functionReturnValue;
		}
		#endregion
		public string AssignCSV(object Value)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if (Information.IsDBNull(Value))
				Value = clsSupport.bpeNullString;
			if ((Value == null))
				Value = clsSupport.bpeNullString;
			functionReturnValue = Convert.ToString(Value).Trim();
			if (functionReturnValue.IndexOf("\"") >= 0 || functionReturnValue.IndexOf(",") >= 0 || functionReturnValue.IndexOf(Strings.Chr(4)) >= 0 || functionReturnValue.IndexOf(Constants.vbLf) >= 0)
				functionReturnValue = string.Format("\"{0}\"", this.CSVQuote(functionReturnValue));
			return functionReturnValue;
		}
		public int CountStr(string strInput, string SearchString)
		{
			int functionReturnValue = 0;
			functionReturnValue = 0;
			if ((strInput == null) || (SearchString == null))
				return functionReturnValue;
			if (strInput == clsSupport.bpeNullString || SearchString == clsSupport.bpeNullString)
				return functionReturnValue;
			int i = strInput.IndexOf(SearchString);
			if (i == -1)
				return functionReturnValue;
			//Try
			while (i > -1) {
				functionReturnValue += 1;
				i = strInput.IndexOf(SearchString, i + 1);
			}
			return functionReturnValue;
		}
		private string CSVQuote(object objSource)
		{
			int i = 0;
            string strSource = objSource.ToString();
            //Eliminate all double - double quotes
            do {
				i = Strings.InStr(1, strSource, "\"" + "\"");
				if (i != 0) {
					strSource = Strings.Left(strSource, i - 1) + "\"" + Strings.Mid(strSource, i + 2);
				}
			} while (!(i == 0));
			//Replace all single quotes with double - single quotes
			i = Strings.InStr(1, strSource, "\"");
			while (i > 0) {
				strSource = Strings.Left(strSource, i - 1) + "\"" + "\"" + Strings.Mid(strSource, i + 1);
				i = Strings.InStr(i + 2, strSource, "\"");
			}
			return strSource;
		}
		/// <summary>This function works just like InStr() except it returns the first occurrence of any character in the SearchSet instead of the occurrence of that string as a whole</summary>
		public int InStrMX(int iStart, string strWork, string strSearchSet, CompareMethod Compare = CompareMethod.Binary)
		{
			int functionReturnValue = 0;
			functionReturnValue = 0;
			if (iStart <= 0)
				throw new ArgumentException("Start must be greater than zero.");
			if ((strWork == null) || strWork == clsSupport.bpeNullString)
				throw new ArgumentException("Work string must be specified.");
			if ((strSearchSet == null) || strSearchSet == clsSupport.bpeNullString)
				throw new ArgumentException("SearchSet must be specified.");
            bool continueLoop = true;
            for (int i = 1; i <= strSearchSet.Length && continueLoop; i++) {
				functionReturnValue = Strings.InStr(iStart, strWork, Strings.Mid(strSearchSet, i, 1), Compare);
				if (functionReturnValue != 0)
                    continueLoop = false;
			}
			return functionReturnValue;
		}
		public void ParseCSV(string strCSV, ref ArrayList TargetFields)
		{
			if ((strCSV == null))
				strCSV = clsSupport.bpeNullString;
			strCSV = strCSV.Trim();
			TargetFields = new ArrayList();
			int iField = 1;
			while (strCSV.Length > 0) {
				int iComma = strCSV.IndexOf(",");
				string Char1 = strCSV.Substring(0, 1);
				//Find the matching quote before determining the comma position...
				if (Char1 == "\"")
					iComma = strCSV.IndexOf(",", strCSV.IndexOf(Char1, 1));
				int iLen = (iComma >= 0 ? iComma : strCSV.Length);
				string TempVal = strCSV.Substring(0, iLen).Trim();
				//We'll get in here if the string has embedded quotes, or commas...
				if (Char1 == "\"") {
					//First, Strip off the outer quotes...
					TempVal = TempVal.Substring(1, TempVal.Length - 2);
                    //Next, remove any doubled quotes...
                    bool continueLoop = true;
                    for (int i = 0; i <= TempVal.Length - 1 && continueLoop; i++) {
                        int iQuote = TempVal.IndexOf("\"" + "\"", i);   // Strings.Chr(34) + Strings.Chr(34), i);
						//Note: Chr(34) = Asc("""")
						if (iQuote >= 0) {
							TempVal = TempVal.Substring(0, iQuote) + TempVal.Substring(iQuote + 2);
							i = iQuote + 1;
						} else {
                            continueLoop = false;
                        }
                    }
				}
				TargetFields.Add(TempVal);
				strCSV = (iComma >= 0 ? strCSV.Substring(iComma + 1).Trim() : clsSupport.bpeNullString);
				iField += 1;
			}
		}
		/// <summary>Parses a given string by delimiter</summary>
		/// <param name="Source">String to work on</param>
		/// <param name="Delimiter">Token delimiter</param>
		/// <param name="Encapsulator">Optional: Allows for tokens to return strings encapsulated with "Delimiter" characters</param>
		/// <returns>Returns string array</returns>
		public string[] ParseStr(string Source, string Delimiter, string Encapsulator = clsSupport.bpeNullString)
		{
			string[] delim = new string[] { Delimiter };
			if ((Source == null) || Source.Length == 0)
				throw new ArgumentException("Work string must be specified.");
			if ((Delimiter == null) || Delimiter == clsSupport.bpeNullString)
				throw new ArgumentException("Delimiter must be specified.");
			if ((Encapsulator == null))
				Encapsulator = clsSupport.bpeNullString;

			if (Delimiter.Length > 1 || (Encapsulator.Length > 0 && Source.IndexOf(Encapsulator) > -1)) {
				//Strategy: Replace all occurrences of Delimiter (not encapsulated by Encapsulator) with a
				//          substitute delimiter which can be later used in a String.Split operation.
				int cntEncap = 0;
				delim = new string[] { Strings.Chr(1).ToString() };
				int sPos = 0;
				while (sPos < Source.Length) {
					if (sPos + Encapsulator.Length < Source.Length && Encapsulator.Length > 0 && Source.Substring(sPos, Encapsulator.Length) == Encapsulator) {
						cntEncap += 1;
						sPos += Encapsulator.Length;
					} else if (sPos + Delimiter.Length < Source.Length && Source.Substring(sPos, Delimiter.Length) == Delimiter && cntEncap % 2 == 0) {
						Source = string.Format("{0}{1}{2}", Source.Substring(0, sPos), delim[0], Source.Substring(sPos + Delimiter.Length));
						sPos += delim.Length;
					} else {
						sPos += 1;
					}
				}
			}
            return Source.Split(delim, StringSplitOptions.RemoveEmptyEntries);
		}
		/// <summary>Retrieve specified token of string</summary>
		/// <param name="Source">String to work on</param>
		/// <param name="TokenNum">Returns specified token in string</param>
		/// <param name="Delimiter">Token delimiter</param>
		/// <param name="Encapsulator">Optional: Allows for tokens to return strings encapsulated with "Delimiter" characters</param>
		/// <param name="Preserve">Optional: Preserves encapsulating characters when token is encapsulated</param>
		/// <returns>Returns string token.  If none is found, will return ""</returns>
		public string ParseStr(string Source, int TokenNum, string Delimiter, string Encapsulator = clsSupport.bpeNullString, bool Preserve = false)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if (TokenNum < 1)
				throw new ArgumentException("TokenNum must be greater than zero.");
			string[] Tokens = this.ParseStr(Source, Delimiter, Encapsulator);
			if (TokenNum <= Tokens.Length)
				functionReturnValue = Tokens[TokenNum - 1];
			if (!Preserve && Encapsulator.Length > 0 && functionReturnValue.StartsWith(Encapsulator) && functionReturnValue.EndsWith(Encapsulator))
				functionReturnValue = functionReturnValue.Substring(1, functionReturnValue.Length - 2);
			return functionReturnValue;
		}
		/// <summary>Retrieve specified token of string</summary>
		/// <param name="strWork">String to work on</param>
		/// <param name="intTokenNum">Returns specified token in string</param>
		/// <param name="strDelimitChr">Token delimiter</param>
		/// <param name="strEncapChr">Optional: Allows for tokens to return strings encapsulated with "strDelimitChr" characters</param>
		/// <returns>Returns string token.  If none is found, will return ""</returns>
		public string OldParseStr(string strWork, short intTokenNum, string strDelimitChr, string strEncapChr = clsSupport.bpeNullString)
		{
			string functionReturnValue = null;
			int intSPos = 0;    //Start Position
			int intDPos = 0;    //Delimiter Position
			int intSPtr = 0;    //Start Pointer
			int intEPtr = 0;    //End Pointer

			functionReturnValue = clsSupport.bpeNullString;
			if ((strWork == null) || strWork == clsSupport.bpeNullString)
				throw new ArgumentException("Work string must be specified.");
			if (intTokenNum <= 0)
				throw new ArgumentException("TokenNum must be greater than zero.");
			if ((strDelimitChr == null) || strDelimitChr == clsSupport.bpeNullString)
				throw new ArgumentException("Delimiter must be specified.");
			if ((strEncapChr == null))
				strEncapChr = clsSupport.bpeNullString;

			short intCurrentTokenNum = 0;
			int intWorkStrLen = strWork.Length;
			bool intEncapStatus = Convert.ToBoolean(strEncapChr.Length > 0);
			int intDelimitLen = strDelimitChr.Length;
			if (intWorkStrLen == 0 || intSPos > intWorkStrLen)
				return functionReturnValue;

			string strTemp = clsSupport.bpeNullString;
            bool continueLoop = true;
			while (continueLoop) {
				strTemp = clsSupport.bpeNullString;
                if (intSPos > intWorkStrLen)
                    continueLoop = false;
                else {
				    intDPos = strWork.IndexOf(strDelimitChr, intSPos);
				    //intDPos = InStr(intSPos, strWork, strDelimitChr)
				    if (intEncapStatus) {
					    intSPtr = strWork.IndexOf(strEncapChr, intSPos);
					    //intSPtr = InStr(intSPos, strWork, strEncapChr)
					    intEPtr = strWork.IndexOf(strEncapChr, intSPtr + 1);
					    //intEPtr = InStr(intSPtr + 1, strWork, strEncapChr)
					    if (intDPos > intSPtr && intDPos < intEPtr)
						    intDPos = strWork.IndexOf(strDelimitChr, intEPtr);
					    //intDPos = InStr(intEPtr, strWork, strDelimitChr)

				        if (intDPos < intSPos)
					        intDPos = intWorkStrLen + intDelimitLen;

				        if (intDPos <= 0)
                            continueLoop = false;
                        else {
                            strTemp = strWork.Substring(intSPos, Math.Min(strWork.Length - intSPos, intDPos - intSPos));
				            //strTemp = Mid(strWork, intSPos, intDPos - intSPos)
				            intSPos = intDPos + intDelimitLen;
				            intCurrentTokenNum += 1;
				            if (intCurrentTokenNum == intTokenNum)
                                continueLoop = false;
                        }
                    }
                }
			}
			if (intEncapStatus) {
				//ParseStr = ReplaceCS(strTemp, strEncapChr, "", OpMode.StringBinaryCompare)
				if (strTemp.StartsWith(strEncapChr) && strTemp.EndsWith(strEncapChr))
					strTemp = strTemp.Substring(1, strTemp.Length - 2);
			}
			functionReturnValue = strTemp;
			return functionReturnValue;
		}
		/// <summary>Counts number of tokens in a string</summary>
		/// <param name="Source">String to work on</param>
		/// <param name="Delimiter">String Delimiter</param>
		/// <param name="Encapsulator">Optional: Allows for tokens to return strings encapsulated with "strDelimiter" characters</param>
		/// <returns>Number of tokens found</returns>
		public int TokenCount(string Source, string Delimiter, string Encapsulator = clsSupport.bpeNullString)
		{
			int functionReturnValue = 0;
			functionReturnValue = 0;
			if ((Source == null) || Source.Length == 0)
				return functionReturnValue;
			if ((Delimiter == null) || Delimiter == clsSupport.bpeNullString)
				return functionReturnValue;
			if ((Encapsulator == null))
				Encapsulator = clsSupport.bpeNullString;

			string[] Tokens = this.ParseStr(Source, Delimiter, Encapsulator);
			functionReturnValue = Tokens.Length;
            bool continueLoop = true;
            for (int i = Tokens.GetUpperBound(0); i >= 0 && continueLoop; i += -1) {
				if (Tokens[i].Length == 0)
					functionReturnValue -= 1;
				else
                    continueLoop = false;
            }
            return functionReturnValue;
		}
		public int OldTokenCount(string strWork, string strDelimiter, string strEncapChr = clsSupport.bpeNullString)
		{
			int functionReturnValue = 0;
			int intDPos = 0;
			//Delimiter Position
			int intSPtr = 0;
			//Start Pointer
			int intEPtr = 0;
			//End Pointer
			int intWorkStrLen = Strings.Len(strWork);
			int intEncapStatus = 0;
			int intSPos = 1;
			//Start Position
			string strTemp = clsSupport.bpeNullString;
			int intDelimitLen = Strings.Len(strDelimiter);
			functionReturnValue = 0;
			if (Strings.Len(strEncapChr) > 0)
				intEncapStatus = Strings.Len(strEncapChr);
			if (intWorkStrLen == 0 || intSPos > intWorkStrLen)
				return functionReturnValue;
            //Try
            bool continueLoop = true;
			while (continueLoop) {
				strTemp = clsSupport.bpeNullString;
				if (intSPos > intWorkStrLen)
                    continueLoop = false;
                else {
                    intDPos = Strings.InStr(intSPos, strWork, strDelimiter);
				    if (intEncapStatus > 0) {
					    intSPtr = Strings.InStr(intSPos, strWork, strEncapChr);
					    intEPtr = Strings.InStr(intSPtr + 1, strWork, strEncapChr);
					    if (intDPos > intSPtr && intDPos < intEPtr)
						    intDPos = Strings.InStr(intEPtr, strWork, strDelimiter);
				    }

				    if (intDPos < intSPos)
					    intDPos = intWorkStrLen + intDelimitLen;

				    if (intDPos == 0)
                        continueLoop = false;
                    else {
                        strTemp = Strings.Mid(strWork, intSPos, intDPos - intSPos);
				        intSPos = intDPos + intDelimitLen;
				        functionReturnValue += 1;
                    }
                }
			}
			return functionReturnValue;
		}
		public string TrimTabs(string Source)
		{
			string functionReturnValue = null;
			functionReturnValue = Source.Trim();
			while (functionReturnValue.StartsWith(Constants.vbTab)) {
				functionReturnValue = functionReturnValue.Substring(1);
			}
			while (functionReturnValue.EndsWith(Constants.vbTab)) {
				functionReturnValue = functionReturnValue.Substring(0, functionReturnValue.Length - 1);
			}
			return functionReturnValue;
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
