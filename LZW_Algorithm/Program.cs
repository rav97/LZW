using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        string inputFilepath = @"..\..\inputfile.txt";
        string outputFilepath = @"..\..\compressed.bin";
        string outputDecompressed = @"..\..\decompressed.txt";

        string inputText = File.ReadAllText(inputFilepath, Encoding.ASCII);
        List<UInt16> compressed = LZW.LZW_Encode(inputText);

        byte[] bytes = ListToBytes(compressed);
        File.WriteAllBytes(outputFilepath, bytes);

        byte[] readBytes = File.ReadAllBytes(outputFilepath);
        List<UInt16> readList = BytesToList(readBytes);

        string decoded = LZW.LZW_Decode(readList);

        float l1 = (float)inputText.Length, l2 = (float)bytes.Length;
        Console.WriteLine("Plain text length: {0}", l1);
        Entropy(inputText);
        Console.WriteLine("Compressed text length: {0}", l2);
        Entropy(compressed);
        float compression = 100.0f * ((l1 - l2) / l1);
        Console.WriteLine("Compression rate: {0}", compression);

        File.WriteAllText(outputDecompressed, decoded);
    }
    public static byte[] ListToBytes(List<UInt16> list)
    {
        byte[] bytes = new byte[list.Count * 2];

        int i = 0;

        foreach (UInt16 e in list)
        {
            byte[] b = BitConverter.GetBytes(e);
            bytes[i] = b[0];
            bytes[i + 1] = b[1];

            i += 2;
        }
        return bytes;
    }
    public static List<UInt16> BytesToList(byte[] bytes)
    {
        List<UInt16> list = new List<UInt16>(bytes.Length / 2);

        for(int i = 0; i < bytes.Length; i+=2)
        {
            list.Add(BitConverter.ToUInt16(bytes, i));
        }

        return list;
    }
    public static void Entropy(string txt)
    {
        Dictionary<char, int> charCount = new Dictionary<char, int>();

        for(int i=0;i<txt.Length;i++)
        {
            if(charCount.ContainsKey(txt[i]))
            {
                charCount[txt[i]] += 1;
            }
            else
            {
                charCount[txt[i]] = 1;
            }
        }
        print(charCount, (float)txt.Length);
    }
    public static void Entropy(List<UInt16> list)
    {
        Dictionary<UInt16, int> charCount = new Dictionary<UInt16, int>();

        for (int i = 0; i < list.Count; i++)
        {
            if (charCount.ContainsKey(list[i]))
            {
                charCount[list[i]] += 1;
            }
            else
            {
                charCount[list[i]] = 1;
            }
        }
        print(charCount, (float)list.Count);
    }
    public static void print(Dictionary<char, int> dict, float size)
    {
        float percent;
        double entropy = 0;
        dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        foreach(KeyValuePair<char, int> kvp in dict)
        {
            percent = ((float)dict[kvp.Key] / size);
            float p1 = 1 / percent;
            entropy += percent * Math.Log(p1, 2.0d);
            int byteCount = Encoding.ASCII.GetByteCount(kvp.Key.ToString());
            //Console.WriteLine("{0}\t{1}\t{2}\t{3}", kvp.Key, byteCount, dict[kvp.Key], Math.Round(percent * 100, 2));
        }
        Console.WriteLine("Entropy: {0}", entropy);
    }
    public static void print(Dictionary<UInt16, int> dict, float size)
    {
        float percent;
        double entropy = 0;
        dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        foreach (KeyValuePair<UInt16, int> kvp in dict)
        {
            percent = ((float)dict[kvp.Key] / size);
            float p1 = 1 / percent;
            entropy += percent * Math.Log(p1, 2.0d);
            int byteCount = 8;
            //Console.WriteLine("{0}\t{1}\t{2}\t{3}", kvp.Key, byteCount, dict[kvp.Key], Math.Round(percent * 100, 2));
        }
        Console.WriteLine("Entropy: {0}", entropy);
    }
}
