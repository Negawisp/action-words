using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VNSprite
{
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private VNSpriteAnimationType _entranceAnimation = VNSpriteAnimationType.None;
    [SerializeField] private VNSpriteAnimationType _exitAnimation = VNSpriteAnimationType.None;

    public Sprite Sprite { get { return _sprite; } }
    public VNSpriteAnimationType EntranceAnimation { get { return _entranceAnimation; } }
    public VNSpriteAnimationType ExitAnimation { get { return _exitAnimation; } }
}
