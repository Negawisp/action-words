using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Interface defining methods to integrate a puzzle with two bodies:
 * 1) something that will react on sertain events like "word selected"
 * 2) GameManager
 */
public interface IPuzzle : IDormantable
{

    /// <summary>
    /// Returns a corresponding <i><b>ReactorType</b></i>.
    /// </summary>
    PuzzleType PuzzleType { get; }

    /// <summary>
    /// Checks a type of a <i><b>level</b></i> parameter to correspond to the <i><b>Puzzle</b></i> type,
    /// Checks fields of a <i><b>level</b></i> parameter,
    /// Loads Puzzle prefab.
    /// </summary>
    /// <param name="level">A DTO with parameters useful for the Puzzle.</param>
    void Load(ILevel level);

    /// <summary>
    /// Clears and hides the puzzle prefab.
    /// </summary>
    void LoadOut();

    /// <summary>
    /// 
    /// </summary>
    void ExtraAction ();

    /// <summary>
    /// Subscribes <i><b>callback</b></i> method to be called on an ExtraAction event.
    /// The implementation of IPuzzle / AbstractPuzzle must define the parameter of 
    /// the extra action.
    /// </summary>
    /// <param name="callback"></param>
    void AddExtraActionCallback (Action<object> callback);


    /// <summary>
    /// Used to subscribe a <b><i>callback</i></b> method to be called on an <b><i>AddWord</i></b> event.
    /// </summary>
    /// <param name="callback"></param>
    void SetWordActivationCallback(Action<string, List<ScorredLetter>> callback);
}
