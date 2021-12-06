using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VNCutsceneSubmanager : AbstractCSSubmanager<VNCutscene>
{
    [SerializeField] private Text _speakerName = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private Image _backgroundSprite = null;
    [SerializeField] private Image _leftSprite = null;
    [SerializeField] private Image _middleSprite = null;
    [SerializeField] private Image _rightSprite = null;
    [SerializeField] private Image _bubbleSprite = null;
    [SerializeField] private Animator _leftSpriteAnimator = null;
    [SerializeField] private Animator _middleSpriteAnimator = null;
    [SerializeField] private Animator _rightSpriteAnimator = null;

    private VNPhrase _currentPhrase;

    public override CutsceneType CutsceneType { get { return CutsceneType.VisualNovel; } }

    private void LaunchCurrentPhraseEntrance()
    {
        Debug.LogWarning("LaunchCurrentPhraseEntrance() method is not implemented!!!");
    }

    private void LaunchCurrentPhraseExit()
    {
        Debug.LogWarning("LaunchCurrentPhraseExit() method is not implemented!!!");
    }

    private void SetSprite(Image VNsprite, Sprite refferenceSprite)
    {
        if (null == VNsprite) { throw new NullReferenceException(); }

        VNsprite.sprite = refferenceSprite;
        if (null == refferenceSprite)
        {
            VNsprite.color = Color.clear;
        } else
        {
            VNsprite.color = Color.white;
        }
    }

    private void LoadCurrentPhrase()
    {
        _speakerName.text = _currentPhrase.SpeakerName;
        _text.text = _currentPhrase.Text;
        SetSprite(_leftSprite, _currentPhrase.LeftSprite.Sprite);
        SetSprite(_rightSprite, _currentPhrase.RightSprite.Sprite);
        SetSprite(_middleSprite, _currentPhrase.MiddleSprite.Sprite);
        SetSprite(_bubbleSprite, _currentPhrase.BubbleSprite);
        if (null != _currentPhrase.BackgroundSprite) { SetSprite(_backgroundSprite, _currentPhrase.BackgroundSprite); }
    }

    /// <summary>
    /// 
    /// Skips the current VNPhrase and switches on the next one, if present.
    /// If not, finishes the cutscene.
    /// 
    /// </summary>
    /// <returns>Whether there was a next phrase.</returns>
    private bool NextPhrase()
    {
        Debug.LogFormat("Current cutscene is null: {0}. Cutscene GameObj: {1}", (_currentCutscene == null), _currentCutscene.gameObject.name);
        _currentPhrase = _currentCutscene.NextPhrase();
        if (null == _currentPhrase)
        {
            return false;
        }

        LoadCurrentPhrase();
        LaunchCurrentPhraseEntrance();
        return true;
    }


    public override void LaunchCutscene(VNCutscene cutscene, Action cutsceneFinishedCallback)
    {
        Debug.Log("VNCutsceneSubmanager launching cutscene.");
        gameObject.SetActive(true);
        _currentCutscene = cutscene;
        _currentCutscene.Load();
        _cutsceneFinishedCallback = cutsceneFinishedCallback;
        if (false == NextPhrase())
        {
            FinishCutscene();
        }
    }

    public override void Continue()
    {
        LaunchCurrentPhraseExit();
        if (false == NextPhrase())
        {
            FinishCutscene();
        }
    }

    public void Skip ()
    {
        FinishCutscene();
    }

    public override void FinishCutscene()
    {
        Debug.Log("Finishing cutscene...");
        gameObject.SetActive(false);
        _cutsceneFinishedCallback();
    }
}
