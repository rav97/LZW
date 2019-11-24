using System;
using System.Collections.Generic;

namespace FileStats.LZW
{
	public static class LZW
	{
		public static List<ushort> Encode(string txt)
		{
			//step 1
			var dictionary = new Dictionary<string, ushort>();
			ushort n = 0;

			for (var i = 0; i < 256; i++)
			{
				dictionary.Add(((char)i).ToString(), n);
				n++;
			}

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
					n++;
					c = s;
				}
			}
			//step 4
			result.Add(dictionary[c]);

			return result;
		}
		public static string Decode(List<ushort> data)
		{
			//step 1
			var dictionary = new Dictionary<ushort, string>();
			ushort n = 0;

			for (var i = 0; i < 256; i++)
			{
				dictionary.Add(n, ((char)i).ToString());
				n++;
			}

			//step 2
			var pk = data[0];

			//step 3
			var result = dictionary[pk];

			//step 4

			for (var i = 1; i < data.Count; i++)
			{
				var pc = dictionary[pk];
				var k = data[i];
				if (dictionary.ContainsKey(k))
				{
					dictionary.Add(n, string.Concat(pc + dictionary[k][0]));
					n++;
					result += dictionary[k];
				}
				else
				{
					dictionary.Add(n, string.Concat(pc + pc[0]));
					n++;
					result += string.Concat(pc + pc[0]);
				}
				pk = k;
			}
			return result;
		}
	}
}