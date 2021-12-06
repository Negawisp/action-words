using System;
using UnityEngine;

public abstract class AbstractCutscene : MonoBehaviour, ICutscene
{
    [SerializeField] private int _id = 0;
    [SerializeField] private string _name = null;
    /// <summary>
    /// Level to switch on after a cutscene. If < 0, then no level is switched on.
    /// </summary>
    [SerializeField] private int _attachedLevelID = -1;

    public int ID { get => _id; }
    public string Name { get => _name; }
    public int AttachedLevelID { get => _attachedLevelID; }
    public abstract CutsceneType CutsceneType { get; }

    public abstract void Load();
    public abstract void Unload();
}
