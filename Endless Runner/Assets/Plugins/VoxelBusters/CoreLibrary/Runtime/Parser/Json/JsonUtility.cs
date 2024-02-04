using UnityEngine;
using System.Collections;

/// <summary>
/// JSON utility provides interface to encode and decode JSON strings.
/// </summary>
/// <description>
/// Here is how we mapped JSON type to C# type
/// JSON array corresponds to type IList.
/// JSON objects corresponds to type IDictionary.
/// JSON intergers corresponds to type long.
/// JSON real numbers corresponds to type double.
/// Credits: http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
/// </description>
namespace VoxelBusters.CoreLibrary.Parser
{
	public static class JsonUtility 
	{
		#region Methods
		
		public static string ToJson(object obj)
		{
			JsonWriter  writer  = new JsonWriter();
			return writer.Serialise(obj);
		}

		public static bool IsNull(string jsonStr)
		{
			return jsonStr.Equals(JsonConstants.kNull);
		}

		public static object FromJson(string jsonStr)
		{
			JsonReader  reader  = new JsonReader(jsonStr);
			return reader.Deserialise();
		}
		
		public static object FromJson(string jsonStr, ref int errorIndex)
		{
            JsonReader  reader  = new JsonReader(jsonStr);
			return reader.Deserialise(ref errorIndex);
		}

		#endregion
	}
}