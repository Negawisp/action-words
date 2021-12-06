using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using JetBrains.Annotations;
using UnityEngine;

public class TetrinoPuzzle : AbstractPuzzle
{
    [CanBeNull] private Action<string, SpellEffect> _wordActivationCallback;
    [CanBeNull] private Action _tetrinoUsedCallback;
    [CanBeNull] private Action _noMovesCallback;

    public override PuzzleType PuzzleType => PuzzleType.Tetrino;

    public void Load()
    {
        gameObject.SetActive(true);
    }
    public void InitiateBoardGame(object o)
    {
        Load();
    }

    public override void LoadOut()
    {
        gameObject.SetActive(false);
        
        //OldToDo: clear board
    }

    public override void SetupAnimation(bool checker, int AnimationType)
    {
        throw new NotImplementedException();
    }

    public override void SetupRewardAnimationTwo()
    {
        throw new NotImplementedException();
    }


    public void AddWordActivationCallback(Action<string, SpellEffect> callback)
    {
        _wordActivationCallback += callback;
    }

    //OldToDo: call this method in WordInput.Activate
    public void OnWordActivation() { }
    public void CallWordActivationCallback(string word, List<Thaum> thaums)
    {
        if (_wordActivationCallback != null)
        {
            _wordActivationCallback(word, SpellEffect.None);
        }
        else
        {
            Debug.Log("_wordActivationCallback is null!");
        }
    }

    public override void ExtraAction()
    {
        throw new NotImplementedException();
    }

    public override void AddExtraActionCallback(Action<object> callback)
    {
        throw new NotImplementedException();
    }

    public override void Load(ILevel level)
    {
        throw new NotImplementedException();
    }

    public override void SetWordActivationCallback(Action<string, List<ScorredLetter>> callback)
    {
        throw new NotImplementedException();
    }
}
