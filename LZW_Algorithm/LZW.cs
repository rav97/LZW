using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

static class LZW
{
    /*public static string Compress(string txt)
    {
        string compressed = String.Empty;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        for (int i = 0; i < txt.Length; i++)
        {
            if (!dict.ContainsKey(txt[i].ToString()))
            {
                dict.Add(txt[i].ToString(), dict.Count + 1);
            }
        }

        foreach(var e in dict)
        {
            Console.WriteLine("{0}. {1}", e.Value, e.Key);
        }

        string c = txt[0].ToString();

        for (int i = 1; i < txt.Length; i++)
        {
            string word = String.Concat(c, txt[i]);
            if (dict.ContainsKey(word))
            {
                c = String.Concat(c, txt[i]);
                //Console.WriteLine("{0} istnieje w slowniku", word);
            }
            else
            {
                compressed += dict[c];
                dict.Add(word, dict.Count + 1);
                c = String.Concat(txt[i]);
                //Console.WriteLine("do slownika dodany jest ciag {0}", word);
            }
            if (i == txt.Length - 1)
                compressed += dict[c];
        }
        return compressed;
    }*/
    public static List<UInt16> LZW_Encode(string txt)
    {
        //step 1
        Dictionary<string, UInt16> D = new Dictionary<string, UInt16>();
        UInt16 n = 0;

        for(int i = 0; i < 256; i++)
        {
            D.Add(((char)i).ToString(), n);
            n++;
        }

        //step 2
        string c = txt[0].ToString();
        List<UInt16> result = new List<UInt16>();

        //step 3
        for(int i = 1; i< txt.Length;i++)
        {
            string s = txt[i].ToString();

            if (D.ContainsKey(String.Concat(c,s)))
            {
                c = c + s;
            }
            else
            {
                result.Add(D[c]);
                D.Add(String.Concat(c, s), n);
                n++;
                c = s;
            }
        }
        //step 4
        result.Add(D[c]);

        return result;
    }
    public static string LZW_Decode(List<UInt16> data)
    {
        //step 1
        Dictionary<UInt16, string> D = new Dictionary<UInt16, string>();
        UInt16 n = 0;

        for (int i = 0; i < 256; i++)
        {
            D.Add(n, ((char)i).ToString());
            n++;
        }

        //step 2
        UInt16 pk = data[0];

        //step 3
        string result = D[pk];

        //step 4

        for(int i = 1; i < data.Count; i++)
        {
            string pc = D[pk];
            UInt16 k = data[i];
            if(D.ContainsKey(k))
            {
                D.Add(n, String.Concat(pc + D[k][0]));
                n++;
                result += D[k];
            }
            else
            {
                D.Add(n, String.Concat(pc + pc[0]));
                n++;
                result += String.Concat(pc + pc[0]);
            }
            pk = k;
        }
        return result;
    }
}
