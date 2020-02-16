using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FileStats.LZW
{
	public static class LZW
	{
		public static List<ushort> Encode(string txt)
		{
			//step 1
			var dictionary = InitializeEncodeDictionary();
			var n = Convert.ToUInt16(dictionary.Count);

			//step 2
			var c = txt[0].ToString();
			var result = new List<ushort>();

			//step 3
			for (var i = 1; i < txt.Length; i++)
			{
				var s = txt[i].ToString();

				if (dictionary.ContainsKey(string.Concat(c, s)))
				{
					c += s;
				}
				else
				{
					result.Add(dictionary[c]);
					dictionary.Add(string.Concat(c, s), n);

					if (n == ushort.MaxValue)
					{
						dictionary = InitializeEncodeDictionary();
						n = Convert.ToUInt16(dictionary.Count);
					}
					else
						n++;

					c = s;
				}
			}
			//step 4
			result.Add(dictionary[c]);

			return result;
		}
		public static string Decode(IEnumerable<ushort> data)
		{
			var builder = new StringBuilder();
			//step 1
			var dictionary = InitializeDecodeDictionary();
			var n = Convert.ToUInt16(dictionary.Count);

			//step 2
			var pk = data.FirstOrDefault();

			//step 3
			builder.Append(dictionary[pk]);

			//step 4

			foreach (var k in data.Skip(1))
			{
				var pc = dictionary[pk];

				if (dictionary.ContainsKey(k))
				{
					dictionary.Add(n, string.Concat(pc, dictionary[k][0])); 
					builder.Append(dictionary[k]);
				}
				else
				{
					dictionary.Add(n, string.Concat(pc, pc[0]));
					builder.Append(string.Concat(pc, pc[0]));
				}
				if (n == ushort.MaxValue)
				{
					dictionary = InitializeDecodeDictionary();
					n = Convert.ToUInt16(dictionary.Count);
				}
				else
					n++;

				pk = k;
			}

			return builder.ToString();
		}

		private static IDictionary<string, ushort> InitializeEncodeDictionary()
		{
			var dictionary = new Dictionary<string, ushort>();
			for (var i = 0; i < 256; i++)
			{
				dictionary.Add(((char)i).ToString(), Convert.ToUInt16(i));
			}

			return dictionary;
		}

		private static IDictionary<ushort, string> InitializeDecodeDictionary()
		{
			var dictionary = new Dictionary<ushort, string>();

			for (var i = 0; i < 256; i++)
			{
				dictionary.Add(Convert.ToUInt16(i), ((char)i).ToString());
			}

			return dictionary;
		}
	}
}