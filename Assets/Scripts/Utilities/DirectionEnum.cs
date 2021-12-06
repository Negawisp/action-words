using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DirectionEnum
{
    Default = 0,
    
    ToTheRight = 1,
    ToTheLeft = -ToTheRight,

    Downwards = 2,
    Upwards = -Downwards
}
