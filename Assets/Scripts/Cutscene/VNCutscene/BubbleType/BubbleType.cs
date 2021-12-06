using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A custom complex enumerator 
/// </summary>
[Serializable]
public class BubbleType
{
    [SerializeField]
    public BubbleTypeEnum BubbleTypeEnum;
    [SerializeField]
    public  Sprite BubbleSprite;

    private BubbleType() { }
}
