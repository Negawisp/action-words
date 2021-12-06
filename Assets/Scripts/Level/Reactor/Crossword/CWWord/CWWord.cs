using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CWword
{
    [Serializable]
    public enum CWDirection
    {
        horizontal,
        vertical
    }

    [SerializeField] private CWDirection _direction;
    [SerializeField] private string _word;
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private bool _opened;

    public int X { get { return _x; } }
    public int Y { get { return _y; } }
    public string Word { get { return _word; } }
    public CWDirection Direction { get { return _direction; } }
    public bool Opened { get { return _opened; } set { _opened = value; } }

    public CWword()
    {
        _opened = false;
    }

    public CWword(string word, int x, int y, CWDirection dir) : this()
    {
        _x = x;
        _y = y;
        _direction = dir;
        _word = word;
    }
}
