using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseDialogueScreen : MonoBehaviour, IDialoguePanel
{
    [SerializeField] private float _animationsDelayTime = 0.5f;
    [SerializeField] private GameObject _background = null;
    [SerializeField] private GameObject _dialoguePanel = null;

    private void AnimateBackgroundFadeIn()
    {
        Debug.Log("DOTweenCheck: BaseDialogueScreen: PlayAnimationLevelFinish");
        _background.GetComponent<Image>().color = Color.clear;
        _background.GetComponent<Image>().DOFade(0.5f, _animationsDelayTime);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void HideInstantly()
    {
        gameObject.SetActive(false);
    }


    public virtual void Show()
    {
        Debug.Log("DOTweenCheck: BaseDialogueScreen: Show");
        Sequence sequence = DOTween.Sequence();

        gameObject.SetActive(true);
        _dialoguePanel.SetActive(false);
        _background.SetActive(true);

        sequence.AppendCallback(AnimateBackgroundFadeIn)
                .AppendInterval(_animationsDelayTime)
                .AppendCallback(() => _dialoguePanel.gameObject.SetActive(true));
    }

    public virtual void ShowInstantly()
    {
        gameObject.SetActive(true);
        _dialoguePanel.SetActive(true);
        _background.SetActive(true);
        AnimateBackgroundFadeIn();
    }
}
