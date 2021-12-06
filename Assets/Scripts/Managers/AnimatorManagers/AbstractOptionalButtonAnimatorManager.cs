using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class AbstractOptionalButtonAnimatorManager : MonoBehaviour
{
    protected Sequence _sequence;
    public abstract Sequence Disappear();
    public abstract Sequence Appear();

    public Sequence SwitchAppear(bool appear)
    {
        if (appear)
        {
            return Appear();
        } else
        {
            return Disappear();
        }
    }
}
