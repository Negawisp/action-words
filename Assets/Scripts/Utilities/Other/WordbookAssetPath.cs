using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to store and give path to wordbooks assets.
/// </summary>
public class WordbookAssetPath
{
    public static readonly WordbookAssetPath Russian = new WordbookAssetPath("russian_nouns", "rus");
    public static readonly WordbookAssetPath English = new WordbookAssetPath("english_nouns", "eng");

    private static Dictionary<string, WordbookAssetPath> _dictionary;
    private static Dictionary<string, WordbookAssetPath> Dictionary
    {
        get
        {
            if (null == _dictionary)
            {
                _dictionary = new Dictionary<string, WordbookAssetPath>();
                _dictionary.Add(Russian.Language, Russian);
                _dictionary.Add(English.Language, English);
            }
            return _dictionary;
        }
    }

    public readonly string Path;
    public readonly string Language;

    private WordbookAssetPath(string textAssetPath, string language)
    {
        Path = textAssetPath;
        Language = language;
    }

    public static WordbookAssetPath Get (string lang)
    {
        return Dictionary[lang];
    }
}
