using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A stuct to placehold "pentagrams": sets of letters to show on levels.
/// </summary>
[Serializable]
public class Pentagram: ISerializationCallbackReceiver
{
    [SerializeField] private int _wordsCount;
    [SerializeField] protected ScorredLetter[] _letters;

    public ScorredLetter[] Letters { get { return _letters; } }

    /// <summary>
    /// 
    /// </summary>
    private ScorredLetter[] _lettersPlaceholder;
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        _lettersPlaceholder = new ScorredLetter[_letters.Length];
    }

    public int GetWordsCount ()
    {
        return _wordsCount;
    }

    public Pentagram() { }
    public Pentagram (char[] letters, int wordsCount)
    {
        _wordsCount = wordsCount;
        _letters = new ScorredLetter[letters.Length];
        for (int i = 0; i < _letters.Length; i++)
        {
            _letters[i] = new ScorredLetter(letters[i], 1, 1);
        }
    }

    public void ChangeLettersOrder(int[] order)
    {
        if (order.Length != _letters.Length)
        {
            Debug.LogErrorFormat(
                "Mismatch in lengths of pentagram letters array and new-order array:\n" +
                "Letters:\n" +
                "{0}\n" +
                "\n" +
                "Order:\n" +
                "{1}", _letters, order);
        }

        for (int i = 0; i < order.Length; i++)
        {
            _lettersPlaceholder[i] = _letters[order[i]];
        }
        ScorredLetter[] sl = _lettersPlaceholder;
        _lettersPlaceholder = _letters;
        _letters = sl;
    }
}
