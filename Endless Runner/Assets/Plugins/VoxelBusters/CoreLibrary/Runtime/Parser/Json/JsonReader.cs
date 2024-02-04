using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Parser
{
	public class JsonReader
    {
        #region Properties

        internal JsonString JSONString { get; private set; }

        #endregion

        #region Constructors

        private JsonReader()
		{}

		public JsonReader(string stringValue)
		{
            JSONString = new JsonString(stringValue);
		}

		#endregion

		#region Methods

		public object Deserialise()
		{
            // check if input string is null
            if (JSONString.IsNullOrEmpty)
            {
                return null;
            }

			int readErrorIndex	= 0;
			return ReadValue(ref readErrorIndex);
		}

        public object Deserialise(ref int errorIndex)
		{
            // check if input string is null
            if (JSONString.IsNullOrEmpty)
            {
                return null;
            }

			// read JSON string 
			int     index   = 0;
			object  value   = ReadValue(ref index);
            if (index != JSONString.Length)
            {
                errorIndex  = index;
            }
            else
            {
                errorIndex  = -1;
            }
			return value;
		}

        #endregion

        #region Parse value methods

		private object ReadValue(ref int index)
		{
			// remove white spaces
			RemoveWhiteSpace(ref index);

			// read token ahead
			JsonToken	token	= LookAhead(index);
			switch (token)
			{
    			case JsonToken.CurlyOpenBracket: 
    				return ReadObject(ref index);
    				
    			case JsonToken.SquareOpenBracket:
    				return ReadArray(ref index);
    				
    			case JsonToken.String:
    				return ReadString(ref index);
    				
    			case JsonToken.Number:
    				return ReadNumber(ref index);
    				
    			case JsonToken.Null:
    				index   += 4;
    				return null;
    				
    			case JsonToken.True:
    				index   += 4;
    				return true;
    				
    			case JsonToken.False:
    				index   += 5;
    				return false;
    				
    			default:
    				Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
    				break;
			}
			
			return null;
		}
		
		private object ReadObject(ref int index)
		{
			IDictionary     dictionary 	= new Dictionary<string, object>();
			bool 			isDone 	    = false;
			
			// skip curls
			index++;
			
			while (!isDone) 
			{
				JsonToken 	token       = LookAhead(index);
				if (token == JsonToken.None) 
				{
					Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
					return null;
				} 
				else if (token == JsonToken.CurlyCloseBracket) 
				{
					NextToken(ref index);
					
					// mark read dictionary object as finished
					isDone      = true;
				}
				else 
				{
					string  key;
					object  value;

					int	    readStatus		= ReadKeyValuePair(ref index, out key, out value);
					if (readStatus != -1)
					{
						// add dictionary entry
						dictionary[key]	    = value;

						// read next token
						JsonToken   nextToken	= LookAhead(index);
						if (nextToken == JsonToken.Comma) 
						{
							NextToken(ref index);
						} 
						else if (nextToken == JsonToken.CurlyCloseBracket) 
						{
							NextToken(ref index);

							// mark read dictionary object as finished
							isDone  = true;
						}
						else
						{
							Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
							return null;
						}
					}
				}
			}
			
			return dictionary;
		}

		private int ReadKeyValuePair(ref int index, out string key, out object value)
		{
			// default values
			key	    = null;
			value	= null;

			// read key
			key	    = ReadValue(ref index) as string;
			if (key == null) 
			{
				Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
				return -1;
			}

			// skip seperator
			if (NextToken(ref index) != JsonToken.Colon) 
			{
				Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
				return -1;
			}

			// read value
			value	= ReadValue(ref index);

			return 0;
		}
		
		private object ReadArray(ref int index)
		{
			IList   list    = new List<object>();
			bool    isDone  = false;
			
			// skip square bracket
			index++;
			
			while (!isDone) 
			{
				JsonToken   token   = LookAhead(index);
				if (token == JsonToken.None) 
				{
					Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
					return null;
				}
				else if (token == JsonToken.SquareCloseBracket) 
				{
					NextToken(ref index);
					
					// mark read array object as finished
					isDone	        = true;
				} 
				else 
				{
					// read element
					object	    arrayElement    = ReadValue(ref index);
					list.Add(arrayElement);

					// read next token
					JsonToken	nextToken	    = LookAhead(index);
					if (nextToken == JsonToken.Comma) 
					{
						NextToken(ref index);
					} 
					else if (nextToken == JsonToken.SquareCloseBracket) 
					{
						NextToken(ref index);

						// mark read array object as finished
						isDone	    = true;
					} 
					else
					{
						Debug.LogError(string.Format("[JSON] Parse error at index ={0}", index));
						return null;
					}
				}
			}
			
			return list;
		}
		
		private string ReadString(ref int index)
		{
			StringBuilder   stringBuilder	= new StringBuilder();
			bool            isDone		    = false;
			
			// skip double quotes
			index++;
			
			while (!isDone)
			{
				// check end of string condition
				if (index == JSONString.Length)
					break;
				
				// get current character and increment pointer index
				char    charValue	= JSONString[index++];
				if (charValue == '"')
				{
					isDone	= true;
				}
				else if (charValue == '\\')
				{
					// We are done with the json string
					if (index == JSONString.Length)
						break;
					
					// Get current character and increment pointer index
					charValue	= JSONString[index++];
                    if (charValue == '"')
                    {
                        stringBuilder.Append('"');
                    }
                    else if (charValue == '\\')
                    {
                        stringBuilder.Append('\\');
                    }
                    else if (charValue == '/')
                    {
                        stringBuilder.Append('/');
                    }
                    else if (charValue == 'b')
                    {
                        stringBuilder.Append('\b');
                    }
                    else if (charValue == 'f')
                    {
                        stringBuilder.Append('\f');
                    }
                    else if (charValue == 'n')
                    {
                        stringBuilder.Append('\n');
                    }
                    else if (charValue == 'r')
                    {
                        stringBuilder.Append('\r');
                    }
                    else if (charValue == 't')
                    {
                        stringBuilder.Append('\t');
                    }
                    else if (charValue == 'u')
                    {
                        int remLength = JSONString.Length - index;
                        if (remLength >= 4)
                        {
                            string  unicodeStr  = JSONString.Value.Substring(index, 4);
                            char    unicodeChar = (char)int.Parse(unicodeStr, System.Globalization.NumberStyles.HexNumber);

                            stringBuilder.Append(unicodeChar);

                            // skip next 4 characters
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }
				}
				else 
				{
					stringBuilder.Append(charValue);
				}
			}

            if (!isDone)
            {
                return null;
            }
			return stringBuilder.ToString();
		}
		
		private object ReadNumber(ref int index)
		{
			int     numIndex	= index;
			bool    isDone		= false;
			
			while (!isDone)
			{
				if (JsonConstants.kNumericLiterals.IndexOf(JSONString[numIndex]) != -1)
				{
					numIndex++;
                    if (numIndex >= JSONString.Length)
                    {
                        isDone  = true;
                    }
				}
				else
				{
					isDone	    = true;
				}
			}
			
			// get number sequence
			int     stringLength    = numIndex - index;
			string  numberStr	    = JSONString.Value.Substring(index, stringLength);
			
			// update look ahead index, point it to next char after end of number string
			index					= numIndex;
			
			// first try to parse an number as long int
			long longValue;
            if (long.TryParse(numberStr, out longValue))
            {
                return longValue;
            }
			
			// convert to double
			double doubleValue;
            if (double.TryParse(numberStr, out doubleValue))
            {
                return doubleValue;
            }
			
			return null;
		}

		#endregion

		#region Helper Methods
		
		private JsonToken LookAhead(int index)
		{
			int indexCopy	= index;
			return NextToken(ref indexCopy);
		}
		
		private JsonToken NextToken(ref int index)
		{
            // check if exceeded json string length
            if (index == JSONString.Length)
            {
                return JsonToken.None;
            }
			
			// remove spacing
			RemoveWhiteSpace(ref index);
			
			// cache current character
			char    charValue   = JSONString[index++];
			switch (charValue)
			{
    			case '{': 
    				return JsonToken.CurlyOpenBracket;
    				
    			case '}': 
    				return JsonToken.CurlyCloseBracket;
    				
    			case '[': 
    				return JsonToken.SquareOpenBracket;
    				
    			case ']': 
    				return JsonToken.SquareCloseBracket;
    				
    			case ':': 
    				return JsonToken.Colon;
    				
    			case ',': 
    				return JsonToken.Comma;
    				
    			case '"': 
    				return JsonToken.String;
    				
    			case '0': case '1': case '2': case '3': case '4': 
    			case '5': case '6': case '7': case '8': case '9':
    			case '-': 
    				return JsonToken.Number;
			}
			
			// reverting post increment which was done after reading character
			index--;
			
			// check for constants
			if ((index + 4) < JSONString.Length)
			{
				if (('n' == JSONString[index]) &&
				    ('u' == JSONString[index + 1]) &&
				    ('l' == JSONString[index + 2]) &&
				    ('l' == JSONString[index + 3]))
				{
					index   += 4;
					return JsonToken.Null;
				}
			}
			if ((index + 4) < JSONString.Length)
			{
				if (('t' == JSONString[index]) &&
				    ('r' == JSONString[index + 1]) &&
				    ('u' == JSONString[index + 2]) &&
				    ('e' == JSONString[index + 3]))
				{
					index   += 4;
					return JsonToken.True;
				}
			}
			if ((index + 5) < JSONString.Length)
			{
				if (('f' == JSONString[index]) &&
				    ('a' == JSONString[index + 1]) &&
				    ('l' == JSONString[index + 2]) &&
				    ('s' == JSONString[index + 3]) &&
				    ('e' == JSONString[index + 4]))
				{
					index += 5;
					return JsonToken.False;
				}
			}
			
			return JsonToken.None;
		}
		
		private void RemoveWhiteSpace(ref int index)
		{
			int     charCount   = JSONString.Length;
			while (index < charCount)
			{
				char charValue	= JSONString[index];
				if (JsonConstants.kWhiteSpaceLiterals.IndexOf(charValue) != -1)
				{
					index++;
				}
				else
				{
					break;
				}
			}
		}

		#endregion
	}
}
