using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CWPGLevel : AbstractLevel, ICWLevel, IPGLevel
{
    [SerializeField] private Crossword _crossword = null;
    [SerializeField] private Pentagram[] _pentagrams = null;
    public override PuzzleType PuzzleType { get { return PuzzleType.Pentagram; } }
    public override ReactorType ReactorType { get { return ReactorType.Crossword; } }


    public override string[] Words { get { return _crossword.GetWords(); } }
    public override ScorredLetter[] Letters { get { return _pentagrams[0].Letters; } }

    
    public Crossword Crossword { get { return _crossword; } }
    public Pentagram[] Pentagrams { get { return _pentagrams; } }
}
