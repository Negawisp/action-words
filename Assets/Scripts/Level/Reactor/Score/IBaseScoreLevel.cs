using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseScoreLevel
{
    int NumberOfTurns { get; }
    int ScoreGoal { get; }
}
