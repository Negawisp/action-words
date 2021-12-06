using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScorredLetter
{
    [SerializeField] private char _letter;
    [SerializeField] private int _score;
    [SerializeField] private int _multiplier;

    public char Letter => _letter;
    public int Score => _score;
    public int Multiplier => _multiplier;

    public ScorredLetter () { }
    public ScorredLetter(char letter, int score, int multiplier)
    {
        _letter = letter;
        _score = score;
        _multiplier = multiplier;
    }
}
