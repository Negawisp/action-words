using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class AbstractLevel : MonoBehaviour, ILevel
{
    [SerializeField] private int _levelID = 0;
    [SerializeField] private AbstractCutscene _openningCutscene = null;
    [SerializeField] private AbstractCutscene _endingCutscene = null;
    [SerializeField] private AfterLevelVariantEnum _afterLevelVariant = AfterLevelVariantEnum.Nothing;
    [SerializeField] private int _afterLevelVariantID = -1;
    [SerializeField] private Sprite _background = null;
    [SerializeField] private Sprite _icon = null;

    public abstract PuzzleType PuzzleType { get; }
    public abstract ReactorType ReactorType { get; }


    public AbstractCutscene OpeningCutscene => _openningCutscene;
    public AbstractCutscene EndingCutscene => _endingCutscene;
    public AfterLevelVariantEnum AfterLevelVariant => _afterLevelVariant;
    public int AfterLevelVariantID => _afterLevelVariantID;
    public Sprite Background => _background;
    public Sprite Icon => _icon;

    public int LevelID => _levelID;
    public abstract string[] Words { get; }
    public abstract ScorredLetter[] Letters { get; }
}
