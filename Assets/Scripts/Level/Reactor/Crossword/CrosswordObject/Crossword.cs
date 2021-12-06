using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;

[Serializable]
public class Crossword : ICrossword
{
    [SerializeField] private int _height = -1;
    [SerializeField] private int _width = -1;
    [SerializeField] public CWword[] _CWWords = null;

    public Dictionary<string, CWword> CWdict { get; private set; }
    private char[,]     _crosswordTable;
    private string[]    _words;


    private class StringComparer : IEqualityComparer<String>
    {
        public bool Equals(string x, string y)
        {
            Debug.Log("StringComparer!");
            Debug.Log("X: " + x + "\nY: " + y);
            return x.ToUpper().Equals(y.ToUpper());
        }
        public int GetHashCode(string obj)
        {
            return obj.Length;
        }
    }

    public Crossword() { }

    private Crossword(int x, int y)
    {
        _crosswordTable = new char[x, y];
        CWdict = new Dictionary<string, CWword>(new StringComparer());
    }

    private void SerializeFromCWWords()
    {
        //Checks
        {
            if (_CWWords == null)
            {
                Debug.LogError("Detected a trial to serialize an unprepared Crossword instance:\n" +
                               "The instance was constructed, but it did not have an array of CWWords to serialize itself from.");
                return;
            }
            if (_width <= 0 || _height <= 0)
            {
                Debug.LogError("Detected a trial to serialize an unprepared Crossword instance:\n" +
                               "The instance was constructed, but it had _width or _height of less than 1.");
                return;
            }
            if (CWdict != null)
            {
                Debug.LogWarning("Detected a trial to serialize a Crossword instance which already had \"CWdict\" property serialized.");
            }
            if (_crosswordTable != null)
            {
                Debug.LogWarning("Detected a trial to serialize a Crossword instance which already had \"_crosswordTable\" property serialized.");
            }
            if (_words != null)
            {
                Debug.LogWarning("Detected a trial to serialize a Crossword instance which already had \"_words\" property serialized.");
            }
        }
        CWdict = new Dictionary<string, CWword>(new StringComparer());
        _crosswordTable = new char[_width, _height];
        foreach (CWword cWword in _CWWords)
        {
            CWdict.Add(cWword.Word, cWword);
            AddWordToCWTable(cWword);
        }
        int nWords = CWdict.Count;
        _words = CWdict.Keys.ToArray();
    }

    /// <summary>
    /// Loads a single crossword from a given <i><b>stringReader</b></i>.
    /// </summary>
    /// <param name="stringReader">Opened instance of a StringReader.</param>
    /// <returns></returns>
    private int LoadCrossword(/*StreamReader */ StringReader stringReader)
    {
        int i = 0; string allText;

        while (!(allText = stringReader.ReadLine()).Equals("FINITE"))
        {
            Debug.Log(allText);
            string durstring = allText;
            durstring = Regex.Replace(durstring, @" ", "");
            switch (i % 4)
            {
                case 1:
                    FillCWdict(durstring, CWword.CWDirection.horizontal);
                    break;
                case 3:
                    FillCWdict(durstring, CWword.CWDirection.vertical);
                    break;
            }

            i++;
        }

        return 1;
    }

    private void FillCWdict(string str, CWword.CWDirection dir)
    {
        int j = 0; int x = 0; int y = 0; int k = 0; 
        while (str[k] != '!')
        {
            StringBuilder word = new StringBuilder();
            while (str[j] != '(')
            {
                word.Append(str[j]);
                j++;
            }
            x = str[j + 1] - '0';
            y = str[j + 3] - '0';
            j += 5;

            Debug.Log("word = " + word + " x =" + x + " y =" + y);
            CWword CWword = new CWword(word.ToString(), x, y, dir);

            CWdict.Add(word.ToString(), CWword);
            k = j;

            AddWordToCWTable(CWword);
        }
    }

    /// <summary>
    /// Fills cells of the crossword table with letters of a given CWWord.
    /// </summary>
    /// <param name="cWword"></param>
    private void AddWordToCWTable(CWword cWword)
    {
        int i = 0;
        for (i = 0; i < cWword.Word.Length; i++)
        {
            if (cWword.Direction == CWword.CWDirection.vertical)
                _crosswordTable[cWword.X + i, cWword.Y] = cWword.Word[i];
            else if(cWword.Direction == CWword.CWDirection.horizontal)
                _crosswordTable[cWword.X, cWword.Y + i] = cWword.Word[i];
        }
    }

    private void DebugLogCWTable()
    {
        for (int i = 0; i < _crosswordTable.GetLength(0); i++)
            for (int j = 0; j < _crosswordTable.GetLength(1); j++)
                    Debug.Log(_crosswordTable[i, j]);
    }

    public char[,] GetCrosswordTable()
    {
        if (_crosswordTable == null)
        {
            SerializeFromCWWords();
        }

        DebugLogCWTable();
        return _crosswordTable;
    }

    public CWword GetCWword(string word)
    {
        Debug.Log("Check word " + word);
        var en = CWdict.Keys.GetEnumerator();
        while (en.MoveNext())
        {
            Debug.Log(en.Current + "_");
            Debug.Log(word.ToUpper().Equals(en.Current.ToUpper()));
        }

        try { return CWdict[word]; }
        catch (KeyNotFoundException) { return null; }
    }

    public string[] GetWords()
    {
        if (null != _words) return _words;

        var enumerator = CWdict.Values.GetEnumerator();
        int n = CWdict.Count;
        _words = new string[n];

        for (int i = 0; i < n; i++)
        {
            enumerator.MoveNext();
            _words[i] = enumerator.Current.Word;
        }
        return _words;
    }

    public static Crossword[] LoadCrosswords(string filepath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filepath);
        StringReader MyReader = new StringReader(textAsset.text);

        int n = 0; int x_size; int y_size;


        //System.IO.StreamReader MyReader = new System.IO.StreamReader(textAsset.text);
        //n = MyReader.ReadLine()[0] - '0';
        n = Convert.ToInt32(MyReader.ReadLine());
        Debug.Log("NNNNNNN" + n );

        Crossword[] a = new Crossword[n];
        for (int i = 0; i < n; i++)
        {
            string size = MyReader.ReadLine();
            x_size = size[0] - '0';
            y_size = size[2] - '0';

            a[i] = new Crossword(x_size, y_size);
            a[i].LoadCrossword(MyReader);
        }

        return a;
    }
}

/* 7х7
 * 5, н, и, ш, м, а, ниша, шина, мина, миша, машина, наши
 * 
 * 0 0 0 0 0 м 0 
 * ш и н а 0 и 0
 * 0 0 и 0 0 н 0
 * м а ш и н а 0 
 * и 0 а 0 0 0 0
 * ш 0 0 0 0 0 0 
 * а 0 0 0 0 0 0 
 * 
 */


            /*
        System.IO.StreamReader MyReader = new System.IO.StreamReader(filepath);
        string firststr = MyReader.ReadLine();

        int x = firststr[0] - '0';
        int y = firststr[2] - '0';
        Debug.Log("x =" + x + "y =" + y);
        crosswordTable = new char[x + 1, y + 1];

        int i = 0; int j = 0;
        while ((allText = MyReader.ReadLine()) != null)
        {
            string durstring = allText;
            durstring = Regex.Replace(durstring, @" ", "");
            Debug.Log(durstring);

            for (j = 0; j < y; j++)
            {
                crosswordTable[x, j] = durstring[j];
            }
            i++;
        }*/