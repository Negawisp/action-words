using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTypeHolder : MonoBehaviour
{
    [SerializeField]
    private BubbleType[] _bubbleTypes = null;

    private Dictionary<BubbleTypeEnum, BubbleType> _bubbleTypesMap = null;


    private void Awake()
    {
        _bubbleTypesMap = new Dictionary<BubbleTypeEnum, BubbleType>(_bubbleTypes.Length);
        foreach (BubbleType bt in _bubbleTypes)
        {
            _bubbleTypesMap.Add(bt.BubbleTypeEnum, bt);
        }
    }

    public BubbleType GetBubbleType(BubbleTypeEnum bubbleTypeEnum)
    {
        BubbleType retVal = null;
        _bubbleTypesMap.TryGetValue(bubbleTypeEnum, out retVal);
        if (null == retVal)
        {
            Debug.LogErrorFormat(
                "{0} is not implemented! Please, create this BubbleType in scene instance declaring this error.",
                bubbleTypeEnum);
        }
        return retVal;
    }
}
