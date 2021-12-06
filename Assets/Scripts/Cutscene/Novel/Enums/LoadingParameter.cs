using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enum serves as a parameter for cutscene-loading methods.
/// 
/// Also you can use this enum to customise what will happen after
/// a certain event in a level.
/// </summary>
public enum LoadingParameter
{
    AUTO,
    MAP,
    NEXT_LEVEL,
    START,
    END
}
