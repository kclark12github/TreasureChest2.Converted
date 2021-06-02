//clsCollection.cs
//   Collection Class...
//   Copyright © 1998-2021, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/10/17    Added initial New Collection;
//   05/04/17    Created from clsFiRReCollection (making it available across all components);
//=================================================================================================================================
//Notes to Self:
//=================================================================================================================================
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCBase
{
    public class clsCollection : IEnumerable
    {
        public clsCollection() : base()
        {
			mCollection = new Collection();
        }
		public clsCollection(string[] Keys, object[] Values) : base()
		{
			if (Keys == null) throw new ArgumentException("Keys must be specified!");
			if (Values == null) throw new ArgumentException("Values must be specified!");
			if (Keys.Length != Values.Length) throw new ArgumentException("Number of elements in Keys and Values must match!");

			mCollection = new Collection();
			for (int i = 0; i < Keys.Length; i++) {
				this.Add(Keys[i], Values[i]);
			}
		}

		#region "Properties"
		private Collection mCollection;
		public clsCollection this[object Key]
		{
			get { return (clsCollection)mCollection[Key]; }
		}
		public int Count
		{
			get { return mCollection.Count; }
		}
		#endregion
		#region "Methods"
		public clsItem Add(string Key, object Value) {
			clsItem iItem = Find(Key);
			if (iItem == null) {
				iItem = new clsItem(Key, Value);
				mCollection.Add(iItem, Key);
			} else {
				iItem.Value = Value;
			}
			return iItem;
		}
		public System.Collections.IEnumerator GetEnumerator()
		{
			return mCollection.GetEnumerator();
		}
		public void Clear()
		{
			foreach (clsItem i in mCollection) {
				i.Key = null;
				i.Value = null;
				mCollection.Remove(1);
			}
		}
		public clsItem Find(string Key)
		{
			foreach (clsItem i in mCollection)
			{
				if (i.Key.Equals(Key)) return i;
			}
			return null;
		}
		public void Remove(string Key)
		{
			mCollection.Remove(Key);
		}
		#endregion
	}
	public class clsItem
	{
		public clsItem(string Key, object Value) : base() {
			mKey = Key;
			mValue = Value;
		}
		#region "Properties"
		private string mKey = "";
		private object mValue = null;
		public string Key
		{
			get { return mKey; }
			set { mKey = value; }
		}
		public object Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
        #endregion
        #region "Methods"
		//None at this time...
        #endregion
    }
}
