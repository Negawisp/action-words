using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Extension of BasePentaPool able to load levels from resouce files.
/// </summary>
public class PentaPool : BasePentaPool
{
    private string _pentagramsFilePath = "levels";

    void Awake()
    {
        _pentaCounter = 0;
    }


    override
    public void LoadPentagrams()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(_pentagramsFilePath);
        StringReader sr = new StringReader(textAsset.text);

        string[] lineSeparators = { "\n\r", "\n" }; // UNNECESSARY READING OF THE WHOLE FILE HERE.
        string[] wordSeparators = { ", " };         // THINK OF ANOTHER SOLUTION!
        string[] lines = sr.ReadToEnd().Split(lineSeparators, System.StringSplitOptions.RemoveEmptyEntries);

        int n = lines.Length;
        _pentagrams = new SmartPentagram[n];

        string[] row;
        int      nLetters;
        char[]   letters;
        string[] words;
        for (int i = 0; i < n; i++)
        {
            // LINE IS SPLIT IN TOO MANY STRINGS. SOLUTION CAN BE WAY MORE ELEGANT!
            row = lines[i].Split(wordSeparators, System.StringSplitOptions.RemoveEmptyEntries);

            nLetters = int.Parse(row[0]);
            letters = new char[nLetters];
            for (int j = 0; j < nLetters; j++)  { letters[j] = row[j + 1][0]; }

            int nWords = row.Length - nLetters - 1;
            words = new string[nWords];
            for (int j = 0; j < nWords; j++)    { words[j] = row[j + nLetters + 1]; }

            _pentagrams[i] = new SmartPentagram(letters, words);
        }
    }

    public void LoadPentagrams(char[][] letterSets, string[][] words)
    {
        _pentagrams = new SmartPentagram[letterSets.Length];
        for (int i = 0; i < _pentagrams.Length; i++)
        {
            _pentagrams[i] = new SmartPentagram(letterSets[i], words[i]);
        }
    }

    public void LoadPentagrams(IPGLevel pgLevel)
    {
        _pentaCounter = 0;
        _pentagrams = pgLevel.Pentagrams;
    }

    public new Pentagram GetNextPentagram()
    {
        return _pentagrams[_pentaCounter++];
    }

    public Pentagram GetPentagram (int number)
    {
        return _pentagrams[number];
    }
}
