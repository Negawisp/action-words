using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A parameter of AbstractLevel defining an action of a GameManager on level being finished.
/// </summary>
public enum AfterLevelVariantEnum
{
    Nothing,
    Defeat,
    LaunchCutscene,
    NextLevel
}
