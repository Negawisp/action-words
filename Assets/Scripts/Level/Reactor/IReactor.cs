using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reacts to events of a Puzzle in order to control the level progress.
/// Generates events for an ILevelManager.
/// </summary>
public interface IReactor : IDormantable
{
    /// <summary>
    /// Returns a corresponding <i><b>ReactorType</b></i>.
    /// </summary>
    ReactorType ReactorType { get; }

    /// <summary>
    /// Returns current animations <i><b>Sequence</b></i>
    /// </summary>
    Sequence AnimationsSequence { get; }

    /// <summary>
    /// Checks a type of a <i><b>level</b></i> parameter to correspond to the <i><b>Reactor</b></i> type,
    /// Checks fields of a <i><b>level</b></i> parameter,
    /// Loads Reactor prefab.
    /// </summary>
    /// <param name="level">A DTO with parameters useful for the Reactor.</param>
    void Load(ILevel level);

    /// <summary>
    /// Clears and hides the Reactor prefab.
    /// </summary>
    void LoadOut();

    /// <summary>
    /// A method called when a word was activated in a <i><b>Puzzle</b></i> part of the game.
    /// </summary>
    /// <param name="word">Word accepted from a puzzle.</param>
    /// <param name="primaryScore">Ammount of score corresponding to the word.</param>
    void ActivateWord(string word, List<ScorredLetter> scorredLetters);
    void SetWordActivatedCallback(Action<string, List<ScorredLetter>> callback);

    /// <summary>
    /// Method called as a callback from Puzzle when puzzle has performed extra action.
    /// The definition of supported extra actions should be specified in every implementation of IReactor / AbstractReactor.
    /// </summary>
    /// <param name="extraActionParameters"></param>
    void OnPuzzleExtraAction(System.Object extraActionParameters);

    void SetTaskFinishedCallback(Action callback);
    void SetDefeatCallback(Action callback);
}
