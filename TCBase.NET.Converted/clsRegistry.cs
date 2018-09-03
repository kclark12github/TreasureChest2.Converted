//clsRegistry - clsRegistry.vb
//   TreasureChest2 Registry Support Class...
//   Copyright © 1998-2018, Ken Clark
//Portions [Public Domain], taken from "The Waite Group's Visual Basic Source Library"/SAMS Publishing and...
//from Bruce McKinney's "Hardcore Visual Basic"/Microsoft Press...
//*********************************************************************************************************************************
//   Modification History:
//   Date:       Description:
//   10/07/14    Reworked CreateRegistryKey to ensure all keys along the given path exist (creating as necessary);
//               Corrected logic issue in RegistryKeyExists which was always returning False;
//   09/06/06    Added RegistryKeyExists;
//   03/11/05    Made New() constructor "Friend" in order to accomplish PublicNotCreatable VB6 Class Instancing behavior;
//   02/21/05    Upgraded for use as a VB.NET Component;
//   02/07/05    Replaced GetComponentPathFromReferenceID() from VBBuild;
//   11/24/02    Removed Debug.Print messages in GetComponentPathFromReferenceID();
//               Added second ParseStr() on Language in GetComponentPathFromReferenceID();
//   11/23/02    Removed FindMostRecentEnum() reworking the logic a bit and making it in-line code in
//               GetComponentPathFromReferenceID();
//   11/19/02    Corrected FindMostRecentEnum() making sure Revision searches are limited to digits
//               (as opposed to other keys like "HELPDIR", "FLAGS", etc.);
//   11/18/02    Added FindMostRecentEnum() and GetComponentPathFromReferenceID();
//   11/16/02    Corrected GetRegistrySetting to use RootKey argument instead of incorrectly hard-coded HKEY_CURRENT_USER;
//   04/30/02    Changed GetINIKey to return szDefault if GetPrivateProfileString fails (returns zero);
//   10/08/01    Converted HKEY constants to RootKeyConstant Enum;
//   10/06/01    Created from libRegistry;
//=================================================================================================================================
// ERROR: Not supported in C#: OptionDeclaration
//#define IncludeCLSIDStuff
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
	public class clsRegistry : clsSupportBase
	{
		internal clsRegistry(clsSupport objSupport) : base(objSupport, "clsRegistry")
		{
			Trace("objSupport:={" + objSupport.GetType().ToString() + "}", trcOption.trcSupport);
			//Do more stuff, if necessary...
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		public enum RootKeyConstants : UInt32
		{
			HKEY_CLASSES_ROOT = 0x80000000,
			HKEY_CURRENT_CONFIG = 0x80000005,
			HKEY_CURRENT_USER = 0x80000001,
			HKEY_DYN_DATA = 0x80000006,
			HKEY_LOCAL_MACHINE = 0x80000002,
			HKEY_USERS = 0x80000003
		}
		#endregion
		#region "Methods"
		private RegistryKey cvtKey(clsRegistry.RootKeyConstants RootKey)
		{
			RegistryKey functionReturnValue = null;
			functionReturnValue = null;
			switch (RootKey) {
				case RootKeyConstants.HKEY_CLASSES_ROOT:
					functionReturnValue = Microsoft.Win32.Registry.ClassesRoot;
					break;
				case RootKeyConstants.HKEY_CURRENT_CONFIG:
					functionReturnValue = Microsoft.Win32.Registry.CurrentConfig;
					break;
				case RootKeyConstants.HKEY_CURRENT_USER:
					functionReturnValue = Microsoft.Win32.Registry.CurrentUser;
					break;
				case RootKeyConstants.HKEY_LOCAL_MACHINE:
					functionReturnValue = Microsoft.Win32.Registry.LocalMachine;
					break;
				case RootKeyConstants.HKEY_USERS:
					functionReturnValue = Microsoft.Win32.Registry.Users;
					break;
			}
			return functionReturnValue;
		}
		private short cmpVersions(string String1, string String2)
		{
			short functionReturnValue = 0;
			int iTokens = TokenCount(String1, ".");
			if (TokenCount(String2, ".") != iTokens)
				throw new ArgumentException("Type Mismatch");

			functionReturnValue = 0;
			for (short i = 1; i <= iTokens; i++) {
				string s1 = ParseStr(String1, i, ".");
				string s2 = ParseStr(String2, i, ".");
				int iMax = s1.Length;
				if (s2.Length > iMax)
					iMax = s2.Length;

				//Pad each string with leading zeros so we can trust string compares...
				s1 = new string('0', iMax - Strings.Len(s1)) + s1;
				s2 = new string('0', iMax - Strings.Len(s2)) + s2;
                functionReturnValue = (short)String.Compare(s1, s2);
				if (functionReturnValue != 0) return functionReturnValue;
			}
			return functionReturnValue;
		}
		public void CreateRegistryKey(clsRegistry.RootKeyConstants RootKey, string KeyName)
		{
			RegistryKey Reg = null;
			try {
				//Iterate through the KeyName making sure each sub-key exists (create as necessary)...
				string[] SubKeys = KeyName.Split('\\');
				string Key = SubKeys[0];
				for (short i = 1; i <= SubKeys.Length - 1; i++) {
					string SubKey = string.Format("{0}\\{1}", Key, SubKeys[i]);
					if (!this.RegistryKeyExists(RootKey, SubKey)) {
						Reg = cvtKey(RootKey).OpenSubKey(Key, true);
						Reg.CreateSubKey(SubKeys[i]);
						Reg.Close();
						Reg = null;
					}
					Key = SubKey;
				}
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
		}
		public void DeleteRegistryKey(clsRegistry.RootKeyConstants RootKey, string KeyName, string SubKey)
		{
			RegistryKey Reg = null;
			try {
				Reg = cvtKey(RootKey).OpenSubKey(KeyName, true);
				if ((Reg != null))
				    Reg.DeleteSubKey(SubKey);
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
		}
		public void DeleteRegistryKeyValue(clsRegistry.RootKeyConstants RootKey, string KeyName, string ValueName)
		{
			RegistryKey Reg = null;
			try {
				Reg = cvtKey(RootKey).OpenSubKey(KeyName, true);
				if ((Reg != null))
    				Reg.DeleteValue(ValueName);
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
		}
		public object GetRegistryKeyValue(clsRegistry.RootKeyConstants RootKey, string KeyName, string ValueName, object vDefault)
		{
			object functionReturnValue = null;
			RegistryKey Reg = null;
			functionReturnValue = null;
			try {
				Reg = cvtKey(RootKey).OpenSubKey(KeyName);
				if ((Reg != null))
    				functionReturnValue = Reg.GetValue(ValueName, vDefault);
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
			return functionReturnValue;
		}
		public object GetRegistrySetting(clsRegistry.RootKeyConstants RootKey, string KeyName, string ValueName, object vDefault = null)
		{
			object functionReturnValue = null;
			functionReturnValue = null;
			functionReturnValue = this.GetRegistryKeyValue(RootKey, KeyName, ValueName, vDefault);
			if (functionReturnValue == null) {
				//Assume GetRegistryKeyValue returned Nothing because the key doesn't yet exist...
				//Rather than sweat the details of the significance of the missing key, simply return the default value
				//(we'll worry about the missing key in SaveRegistrySetting if it comes to that)...
				functionReturnValue = vDefault;
			}
			return functionReturnValue;
		}
		public bool RegistryKeyExists(clsRegistry.RootKeyConstants RootKey, string KeyName)
		{
			bool functionReturnValue = false;
			RegistryKey Reg = null;
			functionReturnValue = false;
			try {
				Reg = cvtKey(RootKey).OpenSubKey(KeyName);
				if ((Reg != null))
					functionReturnValue = true;
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
			return functionReturnValue;
		}
		public void SaveRegistrySetting(clsRegistry.RootKeyConstants RootKey, string KeyName, string ValueName, object Data)
		{
			object CurrentValue = this.GetRegistrySetting(RootKey, KeyName, ValueName, clsSupport.bpeNullString);
			if (CurrentValue.ToString() == clsSupport.bpeNullString)
				this.CreateRegistryKey(RootKey, KeyName);
			if (CurrentValue.ToString() != Data.ToString())
				this.SetRegistryKeyValue(RootKey, KeyName, ValueName, Data);
		}
		public void SetRegistryKeyValue(clsRegistry.RootKeyConstants RootKey, string KeyName, string ValueName, object Data)
		{
			RegistryKey Reg = null;
			try {
				Reg = cvtKey(RootKey).OpenSubKey(KeyName, true);
				if ((Reg != null))
    				Reg.SetValue(ValueName, Data);
			} catch (System.Exception ex) {
			} finally {
				if ((Reg != null))
					Reg.Close();
			}
		}
		#region "INI-file functions"
		public string GetINIKey(string File, string Section, string Key, string defaultValue)
		{
			string Value = Strings.Space(250);
			int nLen = GetPrivateProfileString(Section, Key, defaultValue, Value, Strings.Len(Value), File);
			return (nLen == 0 ? defaultValue : Value.Substring(0, nLen).Trim());
			//Trim null character
		}
		public void SaveINIKey(string File, string Section, string Key, string Value)
		{
			int fRet = 0;
			fRet = WritePrivateProfileString(Section, Key, Value, File);
		}
		#endregion
		#region "Win32-Based Functions"
		#region "Windows Registry Constants"
		public enum RegErrorConstants : short
		{
			//Function Error Constants.
			ERROR_SUCCESS = 0,
			ERROR_REG = 1,
			ERROR_BADKEY = 2,
			ERROR_CANTOPEN = 3,
			ERROR_CANTREAD = 4,
			ERROR_CANTWRITE = 5,
			ERROR_OUTOFMEMORY = 6,
			ERROR_ARENA_TRASHED = 7,
			ERROR_ACCESS_DENIED = 8,
			ERROR_INVALID_PARAMETERS = 87,
			ERROR_NO_MORE_ITEMS = 259
		}

		//Registry Access Rights.
		//const UInt32 SYNCHRONIZE = 0x100000;
		//const UInt32 READ_CONTROL = 0x20000;
		//const UInt32 STANDARD_RIGHTS_ALL = 0x1f0000;
		//const UInt32 STANDARD_RIGHTS_READ = (READ_CONTROL);
		//const UInt32 STANDARD_RIGHTS_WRITE = (READ_CONTROL);
		//const UInt16 KEY_QUERY_VALUE = 0x01;
		//const UInt16 KEY_SET_VALUE = 0x02;
		//const UInt16 KEY_CREATE_LINK = 0x20;
		//const UInt16 KEY_CREATE_SUB_KEY = 0x04;
		//const UInt16 KEY_ENUMERATE_SUB_KEYS = 0x08;
		//const UInt16 KEY_NOTIFY = 0x10;
		//const bool KEY_READ = ((STANDARD_RIGHTS_READ | KEY_QUERY_VALUE | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY) & (~SYNCHRONIZE)) > 0;
		//const bool KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL | KEY_QUERY_VALUE | KEY_SET_VALUE | KEY_CREATE_SUB_KEY | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY | KEY_CREATE_LINK) & (~SYNCHRONIZE)) > 0;
		//const bool KEY_EXECUTE = KEY_READ && (~SYNCHRONIZE > 0);
		//const bool KEY_WRITE = ((STANDARD_RIGHTS_WRITE | KEY_SET_VALUE | KEY_CREATE_SUB_KEY) & (~SYNCHRONIZE)) > 0;
		//Windows Registry API Declarations.
		//Copies memory from pointer to string
		//void WINAPI CopyMemoryLpToStr(LPTSTR lpszDest, DWORD pvSrc, DWORD cbCopy);
		//Private Declare Sub CopyMemoryLpToStr Lib "kernel32" Alias "RtlMoveMemory" (ByVal pvDst As Integer, ByVal pvSrc As Integer, ByVal cbCopy As Integer)
		//Private Declare Function lstrlenAPtr Lib "kernel32" Alias "lstrlenA" (ByVal lpString As Integer) As Integer
		[DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);
		[DllImport("kernel32", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
		//[DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

		//Note that if you declare the lpData parameter as String, you must pass it By Value.
		//Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
		//Private Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByVal lpSecurityAttributes As Integer, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer
		//Private Declare Function RegEnumKey Lib "advapi32.dll" Alias "RegEnumKeyA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer
		//Private Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
		//Private Declare Function RegQueryValueExLong Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Integer, ByRef lpcbData As Integer) As Integer
		//Private Declare Function RegQueryValueExNULL Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer
		//Private Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpValue As String, ByVal cbData As Integer) As Integer
		//Private Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpValue As Integer, ByVal cbData As Integer) As Integer
		//Private Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
		//Private Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer
		#endregion
		#endregion
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
