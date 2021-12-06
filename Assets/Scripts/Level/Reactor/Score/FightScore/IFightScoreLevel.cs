using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFightScoreLevel : IBaseScoreLevel
{
    Sprite OpponentSprite { get; }
    string OpponentName { get; }
}
