using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class HashsetSerializer
{
    public static void Serialize(string wordsAsString, out HashSet<string> output)
    {
        output = new HashSet<string>();

        StringReader reader = new StringReader(wordsAsString);
        string tmpStr = reader.ReadLine();
        while (tmpStr != null)
        {
            output.Add(tmpStr);
            tmpStr = reader.ReadLine();
        }
    }

    public static string Deserialize(in HashSet<string> input)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string s in input)
        {
            stringBuilder.Append(s).Append("\n");
        }
        return stringBuilder.ToString(0, stringBuilder.Length - 1);
    }
}