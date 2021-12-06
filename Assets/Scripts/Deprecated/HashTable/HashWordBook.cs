using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;

public class HashWordBook
{
    private HashSet<string> _data;

    public HashWordBook (string sourcePath)
    {
        _data = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        Load (sourcePath);
    }
    
    
    public HashWordBook (TextAsset textAsset)
    {
        _data = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        Load (textAsset);
    }

    private void Add (string word)
    {
        _data.Add(word);
    }

    public bool Contatins (string word)
    {
        return _data.Contains(word);
    }

    public void Load(string path)
    {
        StreamReader stream = new StreamReader(path);

        if (stream == null)
            Debug.LogError("Couldn't find file " + path);

        string tmpStr = stream.ReadLine();
        while (!stream.EndOfStream)
        {
            Add(tmpStr);
            tmpStr = stream.ReadLine();
        }

        stream.Close();
    }
    
    
    public void Load(TextAsset textAsset)
    {
        StringReader stream = new StringReader(textAsset.text);
        
        string tmpStr = stream.ReadLine();
        while (tmpStr != null)
        {
            Add(tmpStr);
            tmpStr = stream.ReadLine();
        }
    }
}