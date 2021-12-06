using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICWLevel : ILevel
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Crossword of the level.</returns>
    Crossword Crossword { get; }
}
