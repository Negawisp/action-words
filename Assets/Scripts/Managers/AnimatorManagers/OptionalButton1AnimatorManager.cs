using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalButton1AnimatorManager : AbstractOptionalButtonAnimatorManager
{
    public override Sequence Appear()
    {
        if (null != _sequence)
        {
            _sequence.Complete();
        }

        _sequence = DOTween.Sequence();
        return _sequence
            .AppendCallback(() => gameObject.SetActive((true)))
            .Append(GetComponent<Image>().DOColor(Color.white, 0.2f));
    }

    public override Sequence Disappear()
    {
        if (null != _sequence)
        {
            _sequence.Complete();
        }

        _sequence = DOTween.Sequence();
        return _sequence
            .Append(GetComponent<Image>().DOColor(Color.clear, 0f))
            .AppendCallback(() => gameObject.SetActive((false)));
    }


}
