using UnityEngine;
using System.Collections;

namespace VoxelBusters.CoreLibrary.Parser
{
	public static class JsonExtensions 
	{
		#region Methods

		public static string ToJson(this IDictionary dictionary)
		{
			string  jsonStr = JsonUtility.ToJson(dictionary);
			return JsonUtility.IsNull(jsonStr) ? null : jsonStr;
		}

		public static string ToJson(this IList list)
		{
			string  jsonStr	= JsonUtility.ToJson(list);
			return JsonUtility.IsNull(jsonStr) ? null : jsonStr;
		}

		#endregion
	}
}