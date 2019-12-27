using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LZW
{
    public static List<UInt16> LZW_Encode(string txt)
    {
        //step 1
        Dictionary<string, UInt16> D = fillEncodeDict();
        UInt16 n = (UInt16)D.Count;


        //step 2
        string c = txt[0].ToString();
        List<UInt16> result = new List<UInt16>();

        //step 3
        for (int i = 1; i < txt.Length; i++)
        {
            string s = txt[i].ToString();

            if (D.ContainsKey(String.Concat(c, s)))
            {
                c = c + s;
            }
            else
            {
                result.Add(D[c]);
                D.Add(String.Concat(c, s), n);
                if (n == UInt16.MaxValue)
                {
                    D = fillEncodeDict();
                    n = (UInt16)D.Count;
                }
                else
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
        Dictionary<UInt16, string> D = fillDecodeDict();
        UInt16 n = (UInt16)D.Count;

        //step 2
        UInt16 pk = data[0];

        //step 3
        string result = D[pk];

        //step 4

        for (int i = 1; i < data.Count; i++)
        {
            string pc = D[pk];
            UInt16 k = data[i];
            if (D.ContainsKey(k))
            {
                D.Add(n, String.Concat(pc + D[k][0]));
                result += D[k];
            }
            else
            {
                D.Add(n, String.Concat(pc + pc[0]));
                result += String.Concat(pc + pc[0]);
            }
            if (n == UInt16.MaxValue)
            {
                D = fillDecodeDict();
                n = (UInt16)D.Count;
            }
            else
                n++;
            pk = k;
        }
        return result;
    }
    private static Dictionary<string, UInt16> fillEncodeDict()
    {
        Dictionary<string, UInt16> D = new Dictionary<string, UInt16>();

        for (int i = 0; i < 256; i++)
        {
            D.Add(((char)i).ToString(), (UInt16)i);
        }
        return D;
    }
    private static Dictionary<UInt16, string> fillDecodeDict()
    {
        Dictionary<UInt16, string> D = new Dictionary<UInt16, string>();

        for (int i = 0; i < 256; i++)
        {
            D.Add((UInt16)i, ((char)i).ToString());
        }

        return D;
    }
}