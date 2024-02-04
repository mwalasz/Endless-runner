using UnityEngine;
using System.Collections;

namespace VoxelBusters.CoreLibrary.Parser
{
	public class JsonString
	{
		#region Properties

		public string Value
		{	
			get;
			private set;
		}
		
		public bool IsNullOrEmpty
		{
			get;
			private set;
		}
		
		public int Length
		{
			get;
			private set;
		}

		public char this[int index]
		{
			get
			{
				return Value[index];
			}
		}

		#endregion

		#region Constructors

		public JsonString(string value)
		{
			Value			= value;
			IsNullOrEmpty	= string.IsNullOrEmpty(value);
			Length			= IsNullOrEmpty ? 0 : value.Length;
		}

		#endregion
	}
}