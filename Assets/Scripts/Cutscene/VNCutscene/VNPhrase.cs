using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNPhrase : MonoBehaviour
{
    [SerializeField] private string _speakerName = null;
    [SerializeField] [TextArea(3, 10)] private string _text = null;
    [SerializeField] private Sprite _backgroundSprite = null;
    [SerializeField] private VNSprite _leftSprite = null;
    [SerializeField] private VNSprite _middleSprite = null;
    [SerializeField] private VNSprite _rightSprite = null;
    [SerializeField] private BubbleTypeEnum _bubbleTypeEnum = BubbleTypeEnum.Default;

    private BubbleType _bubbleType = null;

    public string SpeakerName { get { return _speakerName; } }
    public string Text { get { return _text; } }
    public Sprite BackgroundSprite { get { return _backgroundSprite; } }
    public VNSprite LeftSprite { get { return _leftSprite; } }
    public VNSprite MiddleSprite { get { return _middleSprite; } }
    public VNSprite RightSprite { get { return _rightSprite; } }
    public Sprite BubbleSprite { get => _bubbleType.BubbleSprite; }

    private void Start()
    {
        _bubbleType = FindObjectOfType<BubbleTypeHolder>()
                        .GetBubbleType(_bubbleTypeEnum);

        if (null == _leftSprite)
        {

        }
    }



}
