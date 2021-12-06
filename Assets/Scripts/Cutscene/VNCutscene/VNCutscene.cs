using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNCutscene : AbstractCutscene
{
    [SerializeField] private VNPhrase[] _phrases = null;
    private int _curentPhraseNumber;

    public override CutsceneType CutsceneType { get { return CutsceneType.VisualNovel; } }

    public VNPhrase NextPhrase()
    {
        if (_curentPhraseNumber >= _phrases.Length)
        {
            return null;
        }
        else
        {
            return _phrases[_curentPhraseNumber++];
        }
    }

    public override void Load()
    {
        _curentPhraseNumber = 0;
    }

    public override void Unload()
    {
        throw new System.NotImplementedException();
    }
}
