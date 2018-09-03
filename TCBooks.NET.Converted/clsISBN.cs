//clsISBN.vb
//   ISBN Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/18/16    Reworked to reflect architectural changes;
//   08/05/10    Implemented ISBN-13 Support;
//   01/05/10    Converted to VB.NET;
//   10/14/02    Added Error Handling;
//   10/08/02    Created from libISBN.bas;
//=================================================================================================================================
//Algorithm taken from web site:
//http://www.isbn.org/standards/home/isbn/international/hyphenation-instructions.asp

//http://www.isbn.spk-berlin.de/
//An ISBN always consists of ten digits preceded by the letters ISBN.
//Note: In countries where the Latin alphabet is not used, an abbreviation in the
//characters of the local script may be used in addition to the Latin letters ISBN.
//
//The ten-digit number is divided into four parts of variable length, which must be
//separated clearly by hyphens or spaces(5):
//
//ISBN 0 571 08989 5
//
//or
//
//isbn 90-70002-4-3
//
//Note: Experience suggests that hyphens are preferable to spaces.
//
//The number of digits in the first three parts of the ISBN (group identifier,
//publisher identifier, title identifier) varies. The number of digits in the group
//number and in the publisher identifier is determined by the quantity of titles
//planned to be produced by the publisher or publisher group. Publishers or publisher
//groups with large title outputs are represented by fewer digits.
//
//4.1. Group identifier
//The first part of the ISBN identifies a country, area or language area participating
//in the ISBN system. Some members form language areas (e.g. group number 3 = German
//language group) or regional units (e.g. South Pacific = group number 982). A group
//identifier may consist of up to 5 digits.
//
//Example: ISBN 90- ...
//
//All group identifiers are allocated by the International ISBN Agency in Berlin.
//
//4.2. Publisher identifier
//The second part of the ISBN identifies a particular publisher within a group. The
//publisher identifier usually indicates the exact identification of the publishing
//house and its address. If publishers exhaust their initial contingent of title
//numbers, they may be allocated an additional publisher identifier. The publisher
//identifier may comprise up to seven digits.
//
//Publisher identifiers are assigned by the ISBN group agency responsible for the
//management of the ISBN system within the country, area or language area where the
//publisher is officially based.
//
//Example: ISBN 90-70002- ...
//
//4.3. Title identifier
//The third part of the ISBN identifies a specific edition of a publication of a
//specific publisher. A title identifier may consist of up to six digits. As an ISBN
//must always have ten digits, blank digits are represented by leading zeros.
//
//Example: ISBN 90-70002-04- ...
//
//4.4. Check digit
//The check digit is the last digit of an ISBN. It is calculated on a modulus 11 with
//weights 10-2, using X in lieu of 10 where ten would occur as a check digit.
//
//This means that each of the first nine digits of the ISBN – excluding the check
//digit itself – is multiplied by a number ranging from 10 to 2 and that the resulting
//sum of the products, plus the check digit, must be divisible by 11 without a remainder.
//
//For example   ISBN 0-8436-1072-7:
//
//  Group
//Identifier Publisher
//Identifier Title
//Identifier Check
//digit
//ISBN    0 8 4 3 6   1 0 7 2 7
//Weight 10 9 8 7 6   5 4 3 2
//
//--------------------------------------------------------------------------------
//
//Products 0 + 72 + 32 + 21 + 36 + 5 + 0 + 21 + 4 + 7
//
//Total: 198
//
//As 198 can be divided by 11 without remainder 0-8436-1072-7 is a valid ISBN.
//7 is the valid check digit.
//
//4.5. Distribution of ranges
//The number of digits in each of the identifying parts 1, 2 and 3 is variable,
//although the total sum of digits contained in these parts is always 9. These
//nine digits, together with the check digit, make up the ten-digit ISBN.
//
//The number of digits in the group identifier will vary according to the output of
//books in a group. Thus, groups with an expected large output, will receive numbers
//of one or two digits and publishers with an expected large output will get numbers
//of two or three digits.
//
//For ease of reading, the four parts of the ISBN are divided by spaces or hyphens.
//
//The generation of hyphens at output by programming helps reduce work at input. It
//reduces the number of characters, eliminates manual checking of hyphenation, and
//insures accuracy of format in all ISBN listings and publications.
//
//The position of the hyphens is determined by the publisher identifier ranges
//established by each group agency in accordance with the book industry needs. The
//knowledge of the prefix ranges for each country or group of countries is necessary
//to develop the hyphenation output program.
//
//For example, the publisher identifier ranges of group number 0 in the English
//language group (Australia, English speaking Canada, Ireland, New Zealand, Puerto Rico,
//South Africa, Swaziland, United Kingdom, United States, and Zimbabwe) are as follows:
//
//     00 - 19
//    200 - 699
//   7000 - 8499
//  85000 - 89999
// 900000 - 949999
//9500000 - 9999999
//
//The following table is an example of the range distribution of publisher identifiers.
//Assuming a group identifier of one digit only, the publisher identifier ranges might
//be as shown in the left-hand column and the title identifiers as shown in the
//right-hand column.
//
//Publisher Identifier       Numbers available per publisher for
//                           Title identification
//--------------------------------------------------------------------------------
//     00 - 19               1 000 000
//    200 - 699                100 000
//   7000 - 8499                10 000
//  85000 - 89999                1 000
// 900000 - 949999                 100
//9500000 - 9999999                 10
//
//
//Example: Group identifier "0"
//If number ranges are between   Insert hyphens after
//--------------------------------------------------------------------------------
//     00 - 19       00 - 19     1st   3rd   9th digit
//    200 - 699      20 - 69      "    4th      "
//   7000 - 8499     70 - 84      "    5th      "
//  85000 - 89999    85 - 89      "    6th      "
// 900000 - 949999   90 - 94      "    7th      "
//9500000 - 9999999  95 - 99      "    8th      "
//--------------------------------------------------------------------------------
//
//Example: Group identifier "1"
//If number ranges are between   Insert hyphens after
//--------------------------------------------------------------------------------
//     00 - 19       00 - 19     1st   3rd   9th digit
//    200 - 699      20 - 69      "    4th      "
//   7000 - 8499     70 - 84      "    5th      "
//  85000 - 89999    85 - 89      "    6th      "
// 900000 - 949999   90 - 94      "    7th      "
//9500000 - 9999999  95 - 99      "    8th      "
//--------------------------------------------------------------------------------
//
//(5) For purposes of data processing the 10-digit string is used without hyphens
//    or spaces. Interpretation and human legible display is effectuated by means
//    of the tables of group numbers and publisher identifier ranges.
//
//
//Group Identifier:        Country or Area:
//--------------------------------------------------------------------------------
//0 + 1 English speaking area:
//       Australia, Canada (E.), Gibraltar, Ireland, New Zealand, Puerto Rico, South Africa, Swaziland, UK, USA, Zimbabwe
//2 French speaking area:
//       France, Belgium (Fr. sp.), Canada (Fr. sp.), Luxembourg, Switzerland (Fr. sp.)
//3 German speaking area:
//       Austria, Germany, Switzerland (Germ. sp.)
//4 Japan
//5 Russian Federation:
//       Azerbaijan, Tajikistan,
//       Turkmenistan, Uzbekistan,
//       Armenia (and 99930),
//       Belarus (and 985),
//       Estonia (and 9949, 9985),
//       Georgia (and 99928),
//       Kazakhstan (and 9965),
//       Kyrgyzstan (and 9967),
//       Latvia (and 9984),
//       Lithuania (and 9986),
//       Moldova, Republic (and 9975),
//       Ukraine (and 966)
//7 China , People 's Republic
//80 Czech Republic; Slovakia
//81 India (and 93)
//82 Norway
//83 Poland
//84 Spain
//85 Brazil
//86 Yugoslavia:
//       Bosnia and Herzegovina (and 9958),
//       Croatia (and 953),
//       Macedonia (and 9989),
//       Slovenia (and 961)
//87 Denmark
//88 Italian speaking area:
//       Italy, Switzerland (It. sp.)
//89 Korea
//90 Netherlands
//       Netherlands,
//       Belgium (Flemish)
//91 Sweden
//92 International Publishers (Unesco, EU);
//       European Community Organizations
//93 India (and 81)
//950 Argentina
//951 Finland (and 952)
//952 Finland (and 951)
//953 Croatia
//954 Bulgaria
//955 Sri Lanka
//956 Chile
//957 Taiwan, China (and 986)
//958 Colombia
//959 Cuba
//960 Greece
//961 Slovenia
//962 Hong Kong (and 988)
//963 Hungary
//964 Iran
//965 Israel
//966 Ukraine (and 5)
//967 Malaysia (and 983)
//968 Mexico (and 970)
//969 Pakistan
//970 Mexico (and 968)
//971 Philippines
//972 Portugal (and 989)
//973 Romania
//974 Thailand
//975 Turkey
//976 Caribbean Community:
//       Antigua, Bahamas, Barbados, Belize, Cayman Islands, Dominica, Grenada, Guyana, Jamaica, Montserrat, St. Kitts-Nevis, St. Lucia, St. Vincent and the Grenadines, Trinidad and Tobago, Virgin Islands (Br)
//977 Egypt
//978 Nigeria
//979 Indonesia
//980 Venezuela
//981 Singapore (and 9971)
//982 South Pacific:
//       Cook Islands, Fiji, Kiribati, Marshall Islands, Nauru, Niue, Solomon Islands, Tokelau, Tonga, Tuvalu; Vanuatu, Western Samoa
//983 Malaysia (and 967)
//984 Bangladesh
//985 Belarus (and 5)
//986 Taiwan, China (and 957)
//987 Argentina
//988 Hongkong (and 962)
//989 Portugal (and 972)
//9948 United Arab Emirates
//9949 Estonia (and 5, 9985)
//9950 Palestine
//9951 Kosova
//9952 Azerbaijan
//9953 Lebanon
//9954 Morocco (and 9981)
//9955 Lithuania (and 5, 9986)
//9956 Cameroon
//9957 Jordan
//9958 Bosnia and Herzegovina
//9959 Libya
//9960 Saudi Arabia
//9961 Algeria
//9962 Panama
//9963 Cyprus
//9964 Ghana
//9965 Kazakhstan (and 5)
//9966 Kenya
//9967 Kyrgyzstan (and 5)
//9968 Costa Rica (and 9977)
//9970 Uganda
//9971 Singapore (and 981)
//9972 Peru
//9973 Tunisia
//9974 Uruguay
//9975 Moldova (and 5)
//9976 Tanzania (and 9987)
//9977 Costa Rica (and 9968)
//9978 Ecuador
//9979 Iceland
//9980 Papua New Guinea
//9982 Zambia
//9983 Gambia
//9984 Latvia (and 5)
//9985 Estonia (and 5, 9949)
//9986 Lithuania (and 5, 9955)
//9987 Tanzania (and 9976)
//9988 Ghana
//9989 Macedonia (and 86)
//99901 Bahrain
//99903 Mauritius
//99904 Netherlands Antilles
//       Aruba, Neth. Ant.
//99905 Bolivia
//99906 Kuwait
//99908 Malawi
//99909 Malta (and 99932)
//99910 Sierra Leone
//99911 Lesotho
//99912 Botswana
//99913 Andorra (and 99920)
//99914 Suriname
//99915 Maldives
//99916 Namibia
//99917 Brunei Darussalam
//99918 Faroe Islands
//99919 Benin
//99920 Andorra (and 99913)
//99921 Qatar
//99922 Guatemala (and 99939)
//99923 El Salvador
//99924 Nicaragua
//99925 Paraguay
//99926 Honduras
//99927 Albania
//99928 Georgia
//99929 Mongolia
//99930 Armenia (and 5)
//99931 Seychelles
//99932 Malta (and 99909)
//99933 Nepal
//99934 Dominican Republic
//99935 Haiti
//99936 Bhutan
//99937 Macau
//99938 Srpska
//99939 Guatemala (and 99922)
//--------------------------------------------------------------------------------
 // ERROR: Not supported in C#: OptionDeclaration
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTrace;
namespace TCBooks
{
	public class clsISBN
	{
		public clsISBN(clsSupport objSupport) : base()
		{

			//Data obtained from http://www.isbn.spk-berlin.de/html/prefix/
			//Group                 If Number Ranges
			//Identifier "0"          are Between      Insert Hyphens After
			//----------------------------------------------------------------------------
			//00------------19            00-19        1st digit     3rd digit   9th digit
			//200-----------699           20-69              "       4th   "           "
			//7000----------8499          70-84              "       5th   "           "
			//85000---------89999         85-89              "       6th   "           "
			//900000--------949999        90-94              "       7th   "           "
			//9500000-------9999999       95-99              "       8th   "           "
			chstr[0] = "00002070859095";
			//English
			//Group                 If Number Ranges
			//Identifier "1"         are Between      Insert Hyphens After
			//--------------------------------------------------------------------------
			//00------------09            00-09       1st digit     3rd digit  9th digit
			//100-----------399           10-39           "         4th  "           "
			//4000----------5499          40-54           "         5th  "           "
			//55000---------86979       5500-8697         "         6th  "           "
			//869800--------998999      8698-9989         "         7th  "           "
			//9990000-------9999999     9990-9999         "         8th  "           "
			chstr[1] = "000010004000550086989990";
			//English

			chstr[2] = "00002070849095";
			//French
			chstr[3] = "00002070859095";
			//German
			chstr[4] = "00002070859095";
			//Japan
			chstr[5] = "000020708590@@92939598";
			//Russia
			chstr[7] = "000010508090";
			//China
			chstr[10] = "000020708590";
			//Czechoslovakia
			chstr[11] = "000020708590";
			//India - also has 93 but undefined
			chstr[12] = "000020709099";
			//Norway
			chstr[13] = "000020708590";
			//Poland
			chstr[14] = "0000207085909597";
			//Spain
			chstr[15] = "000020708590";
			//Brazil
			chstr[16] = "000030708090";
			//Yugoslavia
			chstr[17] = "000040708597";
			//Denmark
			chstr[18] = "000020708590";
			//Italian
			chstr[19] = "000025558595";
			//Korea
			chstr[20] = "000020507080";
			//Belgium, Netherlands
			chstr[21] = "002050708597";
			//Sweden
			chstr[22] = "006080909599";
			//Unesco
			chstr[50] = "0000509099";
			//Argentina
			chstr[51] = "0020558995";
			//Finland
			chstr[52] = "00002050@@89@@9599";
			//Finland
			chstr[53] = "0010156096";
			//Croatia
			chstr[54] = "000030809095";
			//Bulgaria
			chstr[55] = "0020558095";
			//Sri Lanka
			chstr[56] = "00002070";
			//Chile
			chstr[57] = "0000448297";
			//Taiwan
			chstr[58] = "0000608095";
			//Colombia
			chstr[59] = "00002070";
			//Cuba
			chstr[60] = "0000207085";
			//Greece
			chstr[61] = "0000206090";
			//Slovenia
			chstr[62] = "00002070858790";
			//Hong Kong
			chstr[63] = "000020708590";
			//Hungary
			chstr[64] = "0000305590";
			//Iran
			chstr[65] = "0000207090";
			//Israel
			chstr[66] = "0000507090";
			//Ukraine
			chstr[67] = "00609099";
			//Malaysia -- 999 makes 5 digits but this is now accounted for later
			chstr[68] = "00004050@@80";
			//Mexico -- don't know what happens before 10 and after 899
			chstr[69] = "00204080";
			//Pakistan
			chstr[70] = "0000609091";
			//Mexico
			chstr[71] = "0000508591";
			//Philippines
			chstr[72] = "0020558095";
			//Portugal
			chstr[73] = "0020558095";
			//Romania
			chstr[74] = "0000207085";
			//Thailand
			chstr[75] = "0000306092";
			//Turkey
			chstr[76] = "0040608095";
			//Caribbean
			chstr[77] = "0000205070";
			//Egypt
			chstr[78] = "00000020308090";
			//Nigeria
			chstr[79] = "0020408095";
			//Indonesia
			chstr[80] = "00002060";
			//Venezuela
			chstr[81] = "00002030";
			//Singapore
			chstr[82] = "000010@@70@@90";
			//South Pacific
			chstr[83] = "000002204050809099";
			//Malaysia
			chstr[84] = "0000408090";
			//Bangladesh
			chstr[85] = "0000406090";
			//Belarus
			chstr[87] = "0000509095";
			//Argentina
			chstr[154] = "00204080";
			//Morocco
			chstr[155] = "00004090";
			//Lithuania
			chstr[156] = "00104090";
			//Cameroun
			chstr[157] = "00004085";
			//Jordan
			chstr[158] = "00105090";
			//Bosnia
			chstr[160] = "00006090";
			//Saudi Arabia
			chstr[161] = "00508095";
			//Algeria
			chstr[162] = "00006085";
			//Panama
			chstr[163] = "00305575";
			//Cyprus
			chstr[164] = "007095";
			//Ghana
			chstr[165] = "00004090";
			//Kazakstan
			chstr[166] = "00008096";
			//Kenya
			chstr[167] = "00004090";
			//Kyrgyzstan
			chstr[168] = "00107097";
			//Costa Rica
			chstr[170] = "00004090";
			//Uganda
			chstr[171] = "00609099";
			//Singapore
			chstr[172] = "0000@@@@10406090";
			//Peru
			chstr[173] = "00107097";
			//Tunisia
			chstr[174] = "00305575";
			//Uruguay
			chstr[175] = "00509095";
			//Moldova
			chstr[176] = "006090";
			//Tanzania + 999 is 4 digits
			chstr[177] = "00009099";
			//Costa Rica
			chstr[178] = "00009599";
			//Ecuador
			chstr[179] = "00508090";
			//Iceland
			chstr[180] = "004090";
			//Papua New Guinea
			chstr[181] = "00001016208095";
			//Morocco
			chstr[182] = "00008099";
			//Zambia
			chstr[183] = "00809599";
			//Gambia -- Don't know what happens before 80
			chstr[184] = "00005090";
			//Latvia
			chstr[185] = "00508090";
			//Estonia
			chstr[186] = "000040909497";
			//Lithuania
			chstr[187] = "00004088";
			//Tanzania
			chstr[188] = "00305575";
			//Ghana
			chstr[189] = "00306095";
			//Macedonia
			chstr[203] = "002090";
			//Mauritius
			chstr[204] = "006090";
			//Netherlands Antilles
			chstr[206] = "003060";
			//Kuwait
			chstr[208] = "001090";
			//Malawi
			chstr[209] = "004095";
			//Malta
			chstr[210] = "003090";
			//Sierra Leone
			chstr[211] = "000060";
			//Lesotho
			chstr[212] = "006090";
			//Botswana
			chstr[214] = "005090";
			//Suriname
			chstr[215] = "005080";
			//Maldives
			chstr[216] = "003070";
			//Namibia
			chstr[217] = "003090";
			//Brunei Darussalam
			chstr[218] = "004090";
			//Faroes
			chstr[219] = "004090";
			//Benin
			chstr[220] = "005090";
			//Andorra
			chstr[221] = "002070";
			//Qatar
			chstr[223] = "002080";
			//El Salvador
			chstr[225] = "004080";
			//Paraguay
			chstr[226] = "001060";
			//Honduras
			chstr[227] = "003060";
			//Albania
			chstr[228] = "001080";
			//Georgia
			chstr[231] = "005080";
			//Seychelles
			chstr[232] = "001060";
			//Malta
		}
		#region "Properties"
		#region "Enumerations"
		public enum ISBNTypeEnum
		{
			isISBN10 = 0,
			isISBN13 = 1,
			isISSN = 2,
			isISMN = 3,
			isEAN = 4,
			isFinalEntry = 5
		}
		#endregion
		#region "Declarations"
		private string[] TypeName = {
			"ISBN10",
			"ISBN13",
			"ISSN",
			"ISMN",
			"EAN"
		};
		private int[] mPubPref = {
			3,
			4,
			4,
			4,
			5,
			5,
			5,
			6,
			6,
			7
		};
			#endregion
		private string[] chstr = new string[300];
		#endregion
		#region "Methods"
		public string CheckISBN(string strISBN)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			string ISBN = getISBN(strISBN);
			ISBNTypeEnum iType = GetISBNType(ISBN);
			string CheckDigit = null;
			string cd = null;
			string Message = null;
			string EAN = null;

			if (Valid(ISBN, iType)) {
				CheckDigit = GetCheckDigit(ISBN);
				Message = "This is an " + Information.TypeName(iType) + ".  The check digit is ";
				if (Strings.Len(ISBN) == GetISBNTypeLen(iType)) {
					cd = CheckDigit;
					if (!ISBN.EndsWith(cd)) {
						Message = Message + "incorrect: it should be " + CheckDigit + ".";
						Interaction.MsgBox("Incorrect check digit: " + Strings.Right(ISBN, 1) + " entered, but should be " + cd + ".", Constants.vbExclamation, "checkISBN");
					} else {
						Message = Message + "correct.";
					}
				} else {
					Message = Message + CheckDigit + ".";
				}
				Message = Message + Constants.vbCrLf;
				Message = Message + "The full " + Information.TypeName(iType) + " is " + FullNum(ISBN) + "." + Constants.vbCrLf;
				if (iType == ISBNTypeEnum.isEAN && Strings.Mid(ISBN, 1, 3) == "977")
					Message = Message + "It is for a serial publication with ISSN " + FullNum(Strings.Mid(ISBN, 4, 10)) + ".";
				if (iType == ISBNTypeEnum.isEAN && Strings.Mid(ISBN, 1, 4) == "9790")
					Message = Message + "It is for a piece of music with ISMN " + FullNum("M" + Strings.Mid(ISBN, 5, 13)) + ".";
				functionReturnValue = FullNum(ISBN);
			} else {
				if (Strings.Len(ISBN) > 13 && Strings.Mid(ISBN, 1, 3) == "977" && Valid(Strings.Mid(ISBN, 1, 13), ISBNTypeEnum.isEAN)) {
					EAN = Strings.Mid(ISBN, 1, 13);
					if (EAN == FullNum(EAN)) {
						Message = EAN + " is an EAN. The check digit is correct." + Constants.vbCrLf;
						Message = Message + "It is for a serial publication with ISSN " + FullNum(Strings.Mid(ISBN, 4, 10)) + ", issue number " + Strings.Mid(ISBN, 14, Strings.Len(ISBN)) + ".";
						functionReturnValue = FullNum(Strings.Mid(ISBN, 4, 10));
					} else {
						Message = "You have typed in too much, too little, or letters instead of numbers.";
						Interaction.MsgBox(Message, Constants.vbExclamation, "checkISBN");
						functionReturnValue = strISBN;
					}
				} else {
					Message = "You have typed in too much, too little, or letters instead of numbers.";
					Interaction.MsgBox(Message, Constants.vbExclamation, "checkISBN");
					functionReturnValue = strISBN;
				}
			}
			return functionReturnValue;
			//checkISBN = Message
		}
		private ISBNTypeEnum GetISBNType(string sNum)
		{
			ISBNTypeEnum functionReturnValue = default(ISBNTypeEnum);
			int l = Strings.Len(sNum);
			string c = Strings.Left(sNum, 1);

			if (l == 9 || l == 10){functionReturnValue = ISBNTypeEnum.isISBN10;return functionReturnValue;}
			if (l == 7 || l == 8){functionReturnValue = ISBNTypeEnum.isISSN;return functionReturnValue;}
			if (l == 12 || l == 13) {
				switch (sNum.Substring(0, 3)) {
					case "978":
					case "979":
						functionReturnValue = ISBNTypeEnum.isISBN13;
						return functionReturnValue;
					default:
						functionReturnValue = ISBNTypeEnum.isEAN;
						return functionReturnValue;
				}
			}
			if (c == "M"){functionReturnValue = ISBNTypeEnum.isISMN;return functionReturnValue;}
			return functionReturnValue;
		}
		private int GetISBNTypeLen(ISBNTypeEnum ISBNType)
		{
			int functionReturnValue = 0;
			switch (ISBNType) {
				case ISBNTypeEnum.isISBN10:
					functionReturnValue = 10;
					break;
				case ISBNTypeEnum.isISBN13:
					functionReturnValue = 13;
					break;
				case ISBNTypeEnum.isISSN:
					functionReturnValue = 8;
					break;
				case ISBNTypeEnum.isISMN:
					functionReturnValue = 10;
					break;
				case ISBNTypeEnum.isEAN:
					functionReturnValue = 13;
					break;
			}
			return functionReturnValue;
		}
		private string GetCheckDigit(string sNum)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			int x = 0;
			int Weight = 0;
			int CheckSum = 0;
			string CheckDigit = null;
			ISBNTypeEnum ISBNType = GetISBNType(sNum);
			switch (ISBNType) {
				case ISBNTypeEnum.isISBN10:
				case ISBNTypeEnum.isISSN:
					//ISBN    0  8  7  7  7   9  5  0  5  3
					//Weight 10  9  8  7  6   5  4  3  2
					//        0+72+56+49+42+ 45+20+ 0+10+3
					//For x = 1 To TypeLen(t) - 1
					//    CheckSum = CheckSum + (TypeLen(t) - x + 1) * Mid(sNum, x, 1)
					//Next x
					//CheckDigit = CStr((1100 - CheckSum) Mod 11)
					//If CheckDigit = 10 Then CheckDigit = "X"
					Weight = 2;
					for (int i = GetISBNTypeLen(ISBNType) - 1; i >= 1; i += -1) {
						CheckSum += Convert.ToInt32(sNum.Substring(i - 1, 1)) * Weight;
						Weight += 1;
					}

					CheckSum = CheckSum % 11;
					switch (CheckSum) {
						case 0:
							CheckDigit = "0";
							break;
						case 1:
							CheckDigit = "X";
							break;
						default:
							CheckDigit = Convert.ToString(11 - CheckSum);
							break;
					}
					break;
				case ISBNTypeEnum.isISBN13:
					for (int i = GetISBNTypeLen(ISBNType) - 1; i >= 1; i += -1) {
						CheckSum += Convert.ToInt32(sNum.Substring(i - 1, 1)) * ((i % 2) == 0 ? 3 : 1);
					}

					CheckSum = CheckSum % 10;
					switch (CheckSum) {
						case 0:
							CheckDigit = "0";
							break;
						default:
							CheckDigit = Convert.ToString(10 - CheckSum);
							break;
					}
					break;
				default:
					if (ISBNType == ISBNTypeEnum.isISMN)
						CheckSum = 9;
					for (x = 3 - (int)ISBNType; x <= GetISBNTypeLen(ISBNType); x++) {
						CheckSum = CheckSum + (3 - 2 * ((x + (int)ISBNType) % 2)) * Convert.ToInt32(Strings.Mid(sNum, x + 1, 1));
					}

					CheckDigit = ((1000 - CheckSum) % 10).ToString();
					break;
			}
			functionReturnValue = CheckDigit;
			return functionReturnValue;
		}
		private string FullNum(string sNum)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			string strNum = Strings.Mid(sNum, 1, GetISBNTypeLen(GetISBNType(sNum)) - 1) + GetCheckDigit(sNum);
			functionReturnValue = Hyphenate(strNum);
			return functionReturnValue;
		}
		private string Hyphenate(string str)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			string ourstr = clsSupport.bpeNullString;
			int breaker = 0;
			string Pref = null;
			string p = null;
            int groupID = 0;
			int mppl = 0;
			int i = 0;

			switch (GetISBNType(str)) {
				case ISBNTypeEnum.isISBN10:
					Pref = Prefix(str).ToString();
                    switch (Convert.ToInt32(Pref)) {
						case 0:
                            //data obtained from http://www.isbn.spk-berlin.de/html/prefix/
                            //Group                 If Number Ranges
                            //Identifier "0"          are Between      Insert Hyphens After
                            //----------------------------------------------------------------------------
                            //00------------19            00-19        1st digit     3rd digit   9th digit
                            //200-----------699           20-69              "       4th   "           "
                            //7000----------8499          70-84              "       5th   "           "
                            //85000---------89999         85-89              "       6th   "           "
                            //900000--------949999        90-94              "       7th   "           "
                            //9500000-------9999999       95-99              "       8th   "           "
                            groupID = Convert.ToInt32(Strings.Mid(str, 2, 2));
                            if (groupID >= 0 && groupID <= 19) breaker = 3;
                            else if (groupID >= 20 && groupID <= 69) breaker = 4;
                            else if (groupID >= 70 && groupID <= 84) breaker = 5;
                            else if (groupID >= 85 && groupID <= 89) breaker = 6;
                            else if (groupID >= 90 && groupID <= 94) breaker = 7;
                            else if (groupID >= 95 && groupID <= 99) breaker = 8;
                            ourstr = Pref + "-" + Substring(str, Strings.Len(Pref) + 1, breaker) + "-" + Substring(str, breaker + 1, 9) + "-" + Strings.Mid(str, 10, 1);
                            break;
						case 1:
                            //data obtained from http://www.isbn.spk-berlin.de/html/prefix/
                            //Group                 If Number Ranges
                            //Identifier "1"         are Between      Insert Hyphens After
                            //--------------------------------------------------------------------------
                            //00------------09            00-09       1st digit     3rd digit  9th digit
                            //100-----------399           10-39           "         4th  "           "
                            //4000----------5499          40-54           "         5th  "           "
                            //55000---------86979       5500-8697         "         6th  "           "
                            //869800--------998999      8698-9989         "         7th  "           "
                            //9990000-------9999999     9990-9999         "         8th  "           "
                            groupID = Convert.ToInt32(Strings.Mid(str, 2, 4));
                            if (groupID >= 0 && groupID <= 999) breaker = 3;
                            else if (groupID >= 1000 && groupID <= 3999) breaker = 4;
                            else if (groupID >= 4000 && groupID <= 5499) breaker = 5;
                            else if (groupID >= 5500 && groupID <= 8697) breaker = 6;
                            else if (groupID >= 8698 && groupID <= 9989) breaker = 7;
                            else if (groupID >= 9990 && groupID <= 9999) breaker = 8;
							ourstr = Pref + "-" + Substring(str, Strings.Len(Pref) + 1, breaker) + "-" + Substring(str, breaker + 1, 9) + "-" + Strings.Mid(str, 10, 1);
							break;
						default:
							if (Pref != "10" && chstr[shp(Pref)] != clsSupport.bpeNullString) {
								p = chstr[shp(Pref)];
								mppl = 8 - Strings.Len(Pref);
								i = Strings.Len(p) / 2;

								//Debug.Print "str", str
								//Debug.Print "chstr(shp(Pref))", p
								//Debug.Print Substring(p, (i * 2) - 1, i * 2) && " > " && Substring(str, Len(Pref) + 1, Len(Pref) + 3)

								while (Substring(p, (i * 2) - 1, i * 2).CompareTo(Substring(str, Strings.Len(Pref) + 1, Strings.Len(Pref) + 3)) > 0) {
									//Debug.Print Substring(p, (i * 2) - 1, i * 2) && " > " && Substring(str, Len(Pref) + 1, Len(Pref) + 3)
									i = i - 1;
								}

								if (i == mppl + 1 && i == Strings.Len(p) / 2)
									i = mppl - 1;
								//if it's only one over, it'll be one less
								if (i > mppl && Pref != "962")
									i = i + mppl - Strings.Len(p) / 2;
								//They get bigger again, at least for Russia
								if (i > mppl && (Pref == "84" || Pref == "962" || Pref == "978" || Pref == "9986"))
									i = 2 * mppl - i;
								//They get smaller in Hong Kong & Lithuania & Nigeria & Spain
								if (Strings.Mid(str, 1, 6) == "967999" || Strings.Mid(str, 1, 7) == "9976999")
									i = i + 1;
								//Malaysia & Tanzania have a three-digit break
								breaker = i + Strings.Len(Pref);
								ourstr = Pref + "-" + Substring(str, Strings.Len(Pref) + 1, breaker) + "-" + Substring(str, breaker + 1, 9) + "-" + Strings.Mid(str, 10, 1);
							} else {
								ourstr = string.Format("{0}-{1}", Substring(str, 1, 9), Substring(str, 10, 10));
							}
							break;
					}
					break;
				case ISBNTypeEnum.isISBN13:
					ourstr = string.Format("{0}-{1}", str.Substring(0, 3), Hyphenate(str.Substring(3)));
					break;
				case ISBNTypeEnum.isISSN:
					ourstr = Strings.Mid(str, 1, 4) + "-" + Strings.Mid(str, 5, 8);
					break;
				case ISBNTypeEnum.isISMN:
					breaker = 1 + mPubPref[Convert.ToInt32(Strings.Mid(str, 1, 1))];
					ourstr = "M-" + Strings.Mid(str, 1, breaker) + "-" + Strings.Mid(str, breaker + 1, 9) + "-" + Strings.Mid(str, 10, 1);
					break;
				case ISBNTypeEnum.isEAN:
					ourstr = str;
					break;
			}
			functionReturnValue = ourstr;
			return functionReturnValue;
		}
		private string Substring(string str, int StartPos, int EndPos)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = Strings.Mid(str, StartPos, EndPos - StartPos + 1);
			return functionReturnValue;
		}
		private int Prefix(string str)
		{
			int x = 10;
            int test = Convert.ToInt32(str.Substring(0, 1));
            if (test < 8) return test;
            test = Convert.ToInt32(str.Substring(0, 2));
            if (test > 79 && test < 94) return test;
            if (test > 94 && test < 99) return Convert.ToInt32(str.Substring(0, 3));
            test = Convert.ToInt32(str.Substring(0, 3));
            if (test > 989 && test < 999) return Convert.ToInt32(str.Substring(0, 4));
            if (test == 999) return Convert.ToInt32(str.Substring(0, 5));

            //if (Strings.Mid(str, 1, 1) < "8") x = Strings.Mid(str, 1, 1);
            //if (Strings.Mid(str, 1, 2) > "79" && Strings.Mid(str, 1, 2) < "94") x = Strings.Mid(str, 1, 2);
            //if (Strings.Mid(str, 1, 2) > "94" && Strings.Mid(str, 1, 2) < "99") x = Strings.Mid(str, 1, 3);
            //if (Strings.Mid(str, 1, 3) > "989" && Strings.Mid(str, 1, 3) < "999") x = Strings.Mid(str, 1, 4);
            //if (Strings.Mid(str, 1, 3) == "999") x = Strings.Mid(str, 1, 5);
            return x;
		}
		private int shp(string sPref)
		{
			int functionReturnValue = 0;
			functionReturnValue = 0;
			int x = 0;
			int Pref = int.Parse(sPref);
			if (Pref < 8)
				x = Pref;
			if (Pref > 7 && Pref < 99)
				x = Pref - 70;
			//80-93 -> 10-22
			if (Pref > 100 && Pref < 999)
				x = Pref - 900;
			//950-989 -> 50-89
			if (Pref > 1000 && Pref < 9999)
				x = Pref - 9800;
			//9900-9989 -> 100-189
			if (Pref > 10000)
				x = Pref - 99700;
			//99900-99999 -> 200-299
			functionReturnValue = x;
			return functionReturnValue;
		}
		private bool Valid(string sNum, ISBNTypeEnum iType)
		{
			bool v = Convert.ToBoolean(iType < ISBNTypeEnum.isFinalEntry);
			int x = 0;
			string c = null;

            bool continueLoop = true;
            for (x = 1; x <= GetISBNTypeLen(iType) && continueLoop; x++) {
				c = Strings.UCase(Strings.Mid(sNum, x, 1));
				if ((x > 1 || c != "M" || iType != ISBNTypeEnum.isISSN) && (c.CompareTo("0") < 0 || c.CompareTo("9") < 0)) {
					if (x == GetISBNTypeLen((ISBNTypeEnum)iType) && c == "X") {
					} else {
						v = false;
                        continueLoop = false;
                    }
                }
			}
			return v;
		}
		private string getISBN(string istring)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			string i = clsSupport.bpeNullString;
			string j = clsSupport.bpeNullString;
			int x = 0;

			for (x = 0; x <= istring.Length - 1; x++) {
				j = istring.Substring(x, 1).ToUpper();
				if (j.CompareTo("0") >= 0 && j.CompareTo("9") <= 0) {
					i += j;
				} else if (j == "X") {
					i += "X";
				} else if (j == "M") {
					i += "M";
				}
			}
			functionReturnValue = i;
			return functionReturnValue;
		}
		public string FormatISBN(string sISBN)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			//Group                 If Number Ranges
			//Identifier "0"          are Between      Insert Hyphens After
			//----------------------------------------------------------------------------
			//00------------19            00-19        1st digit     3rd digit   9th digit
			//200-----------699           20-69              "       4th   "           "
			//7000----------8499          70-84              "       5th   "           "
			//85000---------89999         85-89              "       6th   "           "
			//900000--------949999        90-94              "       7th   "           "
			//9500000-------9999999       95-99              "       8th   "           "
			//
			//
			//
			//
			//Group                 If Number Ranges
			//Identifier "1"         are Between      Insert Hyphens After
			//--------------------------------------------------------------------------
			//00------------09            00-09       1st digit     3rd digit  9th digit
			//100-----------399           10-39           "         4th  "           "
			//4000----------5499          40-54           "         5th  "           "
			//55000---------86979       5500-8697         "         6th  "           "
			//869800--------998999      8698-9989         "         7th  "           "
			//9990000-------9999999     9990-9999         "         8th  "           "
			//
			//
			//The following table gives the range distribution of the group identifiers:
			//
			//                           0 - 7
			//                          80 - 94
			//                         950 - 995
			//                        9960 - 9989
			//                       99900 - 99999
			//
			//
			//
			//The following table is an example of the range distribution of publisher
			//prefixes. Assuming a group identifier of one digit only, the publisher
			//identifier ranges might be as shown in the left hand column and the title
			//identifiers as shown in the right hand column.
			//
			//
			//Publisher Identifier          Numbers Available for
			//                              Title Identification
			//        00-19                    1 000 000
			//       200-699                     100 000
			//      7000-8499                     10 000
			//     85000-89999                     1 000
			//    900000-949999                      100
			//   9500000-9999999                      10
			functionReturnValue = sISBN;
			string s = Strings.Replace(sISBN, "-", clsSupport.bpeNullString);
			if (Strings.Len(s) != 10)
				return functionReturnValue;
			string Group = Strings.Left(s, 1);
            int testCase = (Convert.ToInt32(s.Substring(1, 2)));
			switch (Group) {
				case "0":
                    if (testCase >= 0 && testCase <= 19)
						functionReturnValue = Strings.Format(s, "&-&&-&&&&&&-&");
                    else if (testCase >= 20 && testCase <= 69)
						functionReturnValue = Strings.Format(s, "&-&&&-&&&&&-&");
                    else if (testCase >= 70 && testCase <= 84)
						functionReturnValue = Strings.Format(s, "&-&&&&-&&&&-&");
                    else if (testCase >= 85 && testCase <= 89)
						functionReturnValue = Strings.Format(s, "&-&&&&&-&&&-&");
                    else if (testCase >= 90 && testCase <= 94)
						functionReturnValue = Strings.Format(s, "&-&&&&&&-&&-&");
                    else if (testCase >= 95 && testCase <= 99)
						functionReturnValue = Strings.Format(s, "&-&&&&&&&-&-&");
					break;
				case "1":
                    if (testCase >= 0 && testCase <= 9)
						functionReturnValue = Strings.Format(s, "&-&&-&&&&&&-&");
                    else if (testCase >= 10 && testCase <= 39)
						functionReturnValue = Strings.Format(s, "&-&&&-&&&&&-&");
                    else if (testCase >= 40 && testCase <= 54)
						functionReturnValue = Strings.Format(s, "&-&&&&-&&&&-&");
                    else {
                        testCase = (Convert.ToInt32(s.Substring(1, 4)));
                        if (testCase >= 5500 && testCase <= 8697)
							functionReturnValue = Strings.Format(s, "&-&&&&&-&&&-&");
                        else if (testCase >= 8698 && testCase <= 9989)
							functionReturnValue = Strings.Format(s, "&-&&&&&&-&&-&");
                        else if (testCase >= 9990 && testCase <= 9999)
							functionReturnValue = Strings.Format(s, "&-&&&&&&&-&-&");
					}
					break;
			}
			return functionReturnValue;
		}
		#endregion
	}
}
