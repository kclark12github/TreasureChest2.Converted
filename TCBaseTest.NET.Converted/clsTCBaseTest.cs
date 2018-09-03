using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using TCBase.clsSupportBase;
using static TCBase.clsTrace;
//clsTCBaseTest.vb
//   TCBase Test Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//   Modification History:
//   Date:       Description:
//   12/06/14    Created;
//=================================================================================================================================
using System.Text;
namespace TCBaseTest
{

	[TestClass()]
	public class clsTCBaseTest : clsTCBase
	{
		#region "Properties"
		#region "Declarations"
			#endregion
		private TestContext mTestContext;
		public TestContext TestContext {
			get { return mTestContext; }
			set { mTestContext = value; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region "Support"
		[TestMethod()]
		public void Support_ParsePath()
		{
			string Path = clsSupport.bpeNullString;
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("", base.ParsePath[null, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");

			Path = "C:\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("C:\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("C:\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("C:\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("C:", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			Path = "S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("S:", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			Path = "\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("WWS004", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("WWS004\\SunGard", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("SunGard", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			Path = "file://\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("file://", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("WWS004", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("WWS004\\SunGard", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("SunGard", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			Path = "file://WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("\\\\WWS004\\SunGard\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("file://", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("WWS004", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("WWS004\\SunGard", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("SunGard", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			//TODO: Research why there's this extra slash before the drive letter - Windows Explorer handles either form (i.e. with or without extra slash)
			try {
				Path = "file://S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
				Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
				Assert.Fail("ParsePath did not throw exception when passed file protocol (without extra slash)");
			} catch (Exception ex) {
			}

			Path = "file:///S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DirOnly], false, "DirOnly");
			Assert.AreEqual("\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DirOnlyNoSlash], false, "DirOnlyNoSlash");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\", base.ParsePath[Path, ParseParts.DrvDir], false, "DrvDir");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config", base.ParsePath[Path, ParseParts.DrvDirNoSlash], false, "DrvDirNoSlash");
			Assert.AreEqual("S:\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database", base.ParsePath[Path, ParseParts.DrvDirFileNameBase], false, "DrvDirFileNameBase");
			Assert.AreEqual("S:", base.ParsePath[Path, ParseParts.DrvOnly], false, "DrvOnly");
			Assert.AreEqual("Database", base.ParsePath[Path, ParseParts.FileNameBase], false, "FileNameBase");
			Assert.AreEqual("Database.dsn", base.ParsePath[Path, ParseParts.FileNameBaseExt], false, "FileNameBaseExt");
			Assert.AreEqual(".dsn", base.ParsePath[Path, ParseParts.FileNameExt], false, "FileNameExt");
			Assert.AreEqual("dsn", base.ParsePath[Path, ParseParts.FileNameExtNoDot], false, "FileNameExtNoDot");
			Assert.AreEqual("file://", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerOnly], false, "ServerOnly");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ServerShare], false, "ServerShare");
			Assert.AreEqual("", base.ParsePath[Path, ParseParts.ShareOnly], false, "ShareOnly");

			try {
				Path = "http://WSRV04\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
				Assert.AreEqual("http://", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
				Assert.Fail("ParsePath did not throw exception when passed http protocol");
			} catch (Exception ex) {
			}

			try {
				Path = "https://WSRV04\\FiRRe\\program files\\SunGard\\FiRRe\\Config\\Database.dsn";
				Assert.AreEqual("https://", base.ParsePath[Path, ParseParts.Protocol], false, "Protocol");
				Assert.Fail("ParsePath did not throw exception when passed https protocol");
			} catch (Exception ex) {
			}
		}
		#endregion
	}
}
