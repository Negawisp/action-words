using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface ILevel
{
    PuzzleType PuzzleType { get; }
    ReactorType ReactorType { get; }

    string[] Words { get; }
    ScorredLetter[] Letters { get; }
    
    int LevelID { get; }

    Sprite Background { get; }
    Sprite Icon { get; }
}
