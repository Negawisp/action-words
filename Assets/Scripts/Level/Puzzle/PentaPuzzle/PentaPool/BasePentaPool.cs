using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to load and placehold pentagrams used in levels of the game.
/// </summary>
public abstract class BasePentaPool
{
    protected int _pentaCounter;
    protected Pentagram[] _pentagrams;

    public abstract void LoadPentagrams();
    
    public void LoadPentagrams (char[][] letterSets, int[] wordsCount)
    {
        _pentagrams = new Pentagram[letterSets.Length];
        for (int i = 0; i < _pentagrams.Length; i++)
        {
            _pentagrams[i] = new Pentagram(letterSets[i], wordsCount[i]);
        }
    }

    public Pentagram GetNextPentagram()
    {
        return _pentagrams[_pentaCounter++];
    }

    public bool OutOfPentagrams()
    {
        return (_pentaCounter >= _pentagrams.Length);
    }

    public int PentagramsLeft()
    {
        return _pentagrams.Length - _pentaCounter;
    }
}
