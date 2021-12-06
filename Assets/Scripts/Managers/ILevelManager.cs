using System;
using System.Collections.Generic;

public interface ILevelManager
{
    //AbstractLevel[] Levels { get; }
    Dictionary<int, AbstractLevel> Levels { get; }
    AbstractLevel CurrentLevel { get; }

    /// <summary>
    /// When a player finishes a puzzle of a level, some event may occur preceeding the
    /// end of the level.
    /// 
    /// To call such an event, use this method!
    /// </summary>
    /// <param name="onPuzzleFinished">Action to activate when the puzzle is finished</param>
    void AddPuzzleFinishedCallback(Action onPuzzleFinished);

    /// <summary>
    /// Prepares the scene elements accordingly to the levelNumber.
    /// </summary>
    /// <param name="levelNumber">Level number to load</param>
    void LoadLevel(int levelNumber);

    /// <summary>
    /// Prepares the scene to change the level:
    /// e.g. returns scene items to pool or unloads unnecessary resources.
    /// </summary>
    void LoadLevelOut();

    /// <summary>
    /// Closes current level saving its progress.
    /// Level should be able to be opened later at the same state it was RolledDown.
    /// 
    /// Only one level at a time can be RolledDown: on another level being RolledDown,
    /// the previous RolledDown level's progress is no longer saved.
    /// 
    /// Furthermore, on another level opened the RolledDown level's progress is lost as well.
    /// </summary>
    void RollLevelDown();

    /// <summary>
    /// Opens a RolledDown level.
    /// </summary>
    void RollLevelUp();
}
