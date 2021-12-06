using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A level with Turn-Limited Scored Penta Puzzle (<i><b>TLSPPuzzle</b></i>) and
/// <i><b>FightScoreReactor</b></i>.
/// </summary>
public class TLSFightLevel : AbstractLevel, ITLSPLevel, IFightScoreLevel
{
    [SerializeField] private int _scoreGoal = 9000;
    [SerializeField] private int _numberOfTurns = 9000;
    [SerializeField] private string _opponentName = "Enter name";
    [SerializeField] private Sprite _opponentSprite = null;
    [SerializeField] private Pentagram[] _pentagrams = null;


    public override PuzzleType PuzzleType => PuzzleType.TLSP;

    public override ReactorType ReactorType => ReactorType.FightScore;

    public override string[] Words => throw new System.NotImplementedException();

    public override ScorredLetter[] Letters { get { return _pentagrams[0].Letters; } }
    public int ScoreGoal => _scoreGoal;
    public int NumberOfTurns => _numberOfTurns;
    public Pentagram[] Pentagrams => _pentagrams;

    public Sprite OpponentSprite => _opponentSprite;

    public string OpponentName => _opponentName;
}
