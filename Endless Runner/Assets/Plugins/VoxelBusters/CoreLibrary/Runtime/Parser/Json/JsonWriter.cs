using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Globalization;
#if NETFX_CORE
using System.Reflection;
#endif

namespace VoxelBusters.CoreLibrary.Parser
{
	public class JsonWriter 
	{
		#region Constants

		private     const		int     kBufferLength	= 512;

		#endregion

		#region Fields

		private     StringBuilder       m_stringBuilder;

		#endregion

		#region Constructors

		public JsonWriter(int bufferLength = kBufferLength)
		{
			m_stringBuilder		= new StringBuilder(bufferLength);
		}

		#endregion

		#region Methods

		public string Serialise(object objectValue)
		{
			WriteObjectValue(objectValue);
			return m_stringBuilder.ToString();
		}

		public void WriteObjectValue(object objectVal)
		{	
			// null value
			if (objectVal == null)
			{
				WriteNullValue();
				return;
			}
			
			Type objectType	= objectVal.GetType();
#if !NETFX_CORE
			if (objectType.IsPrimitive)
#else
			if (objectType.GetTypeInfo().IsPrimitive)
#endif
			{
				WritePrimitive(objectVal);
				return;
			}
			// enum type
#if !NETFX_CORE
			else if (objectType.IsEnum)
#else
			else if (objectType.GetTypeInfo().IsEnum)
#endif
			{
				WriteEnum(objectVal);
				return;
			}
			// array type
			else if (objectType.IsArray)
			{
				WriteArray(objectVal as Array);
				return;
			}
			// generic list type
			else if (objectVal as IList != null)
			{
				WriteList(objectVal as IList);
				return;
			}
			// generic dictionary type
			else if (objectVal as IDictionary != null)
			{
				WriteDictionary(objectVal as IDictionary);
				return;
			}
			
			// other types
			WriteString(objectVal.ToString());
			return;
		}
		
		public void WriteDictionary(IDictionary dict)
		{
			bool                    firstEntry	    = true;
			IDictionaryEnumerator   dictEnumerator	= dict.GetEnumerator();
			
			// initialise with symbol to indicate start of hash
			m_stringBuilder.Append('{');
			
			// iterate through all keys
			while (dictEnumerator.MoveNext())
			{
                // append element seperator
                if (firstEntry)
                {
                    firstEntry  = false;
                }
                else
                {
                    m_stringBuilder.Append(',');
                }

				// key value pair is shown as key:value
				WriteString(dictEnumerator.Key.ToString());	
				m_stringBuilder.Append(':');				
				WriteObjectValue(dictEnumerator.Value);
			}
			
			// append symbol to indicate end of json string representation of dictionary
			m_stringBuilder.Append('}');
			return;
		}
		
		public void WriteArray(Array array)
		{
			// initialise with symbol to indicate start of array
			m_stringBuilder.Append('[');

			switch (array.Rank)
			{
    			case 1:
    				int arrayLength = array.Length;
    				for (int iter = 0; iter < arrayLength; iter++) 
    				{
                        if (iter != 0)
                        {
                            m_stringBuilder.Append(',');
                        }
    					WriteObjectValue(array.GetValue(iter));
    				}
    				break;
				
    			case 2:
    				int outerArrayLength    = array.GetLength(0);
    				int innerArrayLength    = array.GetLength(1);
    				for (int outerIter = 0; outerIter < outerArrayLength; outerIter++)
    				{
                        if (outerIter != 0)
                        {
                            m_stringBuilder.Append(',');
                        }

    					// append symbol to indicate start of json string representation of inner array
    					m_stringBuilder.Append('[');
    					for (int _innerIter = 0; _innerIter < innerArrayLength; _innerIter++)
    					{
                            if (_innerIter != 0)
                            {
                                m_stringBuilder.Append(',');
                            }
    						WriteObjectValue(array.GetValue(outerIter, _innerIter));
    					}
    					
    					// append symbol to indicate end of json string representation of inner array
    					m_stringBuilder.Append(']');
    				}
				    break;
			}
			
			// append symbol to indicate end of json string representation of array
			m_stringBuilder.Append(']');
			return;
		}
		
		public void WriteList(IList list)
		{
			int     totalCount      = list.Count;
			// initialise with symbol to indicate start of list
			m_stringBuilder.Append('[');

			for (int iter = 0; iter < totalCount; iter++) 
			{
                // add element seperator
                if (iter != 0)
                {
                    m_stringBuilder.Append(',');
                }

				WriteObjectValue(list[iter]);
			}
			
			// append symbol to indicate end of json string representation of array
			m_stringBuilder.Append(']');
			return;
		}

		public void WritePrimitive(object value)
		{
			if (value is bool)
			{
                if ((bool)value)
                {
                    m_stringBuilder.Append(JsonConstants.kBoolTrue);
                }
                else
                {
                    m_stringBuilder.Append(JsonConstants.kBoolFalse);
                }
			}
			else if (value is char)
			{
				m_stringBuilder.Append('"').Append((char)value).Append('"');
			}
			else
			{
				m_stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", value); //Fix for globalization
			}
		}
		
		public void WriteEnum(object value)
		{
			m_stringBuilder.Append((int)value);
		}

		public void WriteNullValue()
		{
			m_stringBuilder.Append(JsonConstants.kNull);
		}
		
		public void WriteString(string stringVal)
		{
			int charCount	= stringVal.Length;
			int charIter	= 0;
			
			// append quotes to indicate start of string
			m_stringBuilder.Append('"');
			while (charIter < charCount)
			{
				char    charValue	= stringVal[charIter++];
				if (charValue == '"') 
				{
					m_stringBuilder.Append('\\').Append('"');
				}
				else if (charValue == '\\') 
				{
					m_stringBuilder.Append('\\').Append('\\');
				}
				else if (charValue == '/') 
				{
					m_stringBuilder.Append('\\').Append('/');
				}
				else if (charValue == '\b') 
				{
					m_stringBuilder.Append('\\').Append('b');
				}
				else if (charValue == '\f') 
				{
					m_stringBuilder.Append('\\').Append('f');
				}
				else if (charValue == '\n')
				{
					m_stringBuilder.Append('\\').Append('n');
				}
				else if (charValue == '\r')
				{
					m_stringBuilder.Append('\\').Append('r');
				}
				else if (charValue == '\t')
				{
					m_stringBuilder.Append('\\').Append('t');
				}
				else if (charValue > 127) 
				{	
					string  unicode = "\\u" + ((int)charValue).ToString("x4");
					m_stringBuilder.Append(unicode);
				}
				else
				{
					m_stringBuilder.Append(charValue);
				}
			}
			
			// Append quotes to indicate end of string
			m_stringBuilder.Append('"');
		}
		
		#endregion

		#region Dictionary Methods
		
		public void WriteDictionaryStart()
		{
			m_stringBuilder.Append('{');
		}

		public void WriteKeyValuePair(string key, object value, bool appendSeperator = false)
		{
			// key value pair is shown in "key":"value" format
			WriteString(key);
			m_stringBuilder.Append(':');
			WriteObjectValue(value);

            // append seperator
            if (appendSeperator)
            {
                m_stringBuilder.Append(',');
            }
		}

		public void WriteKeyValuePairSeperator()
		{
			m_stringBuilder.Append(':');
		}
		
		public void WriteDictionaryEnd()
		{
			m_stringBuilder.Append('}');
		}

		#endregion

		#region Array / List Methods
		
		public void WriteArrayStart()
		{
			m_stringBuilder.Append('[');
		}
		
		public void WriteArrayEnd()
		{
			m_stringBuilder.Append(']');
		}

		#endregion

		#region Misc Methods

		public void WriteElementSeperator()
		{
			m_stringBuilder.Append(',');
		}

		#endregion
	}
}