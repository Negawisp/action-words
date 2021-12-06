using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterLevelDialogueManager : MonoBehaviour
{
    [SerializeField] private AfterLevelDialogueScreen[] _levelEndDialogueScreens = null;

    private Dictionary<AfterLevelVariantEnum, AfterLevelDialogueScreen> _dialogueScreensMap;

    private void Awake()
    {
        _dialogueScreensMap = new Dictionary<AfterLevelVariantEnum, AfterLevelDialogueScreen>();

        Debug.LogFormat("Dialogue screens count: \"{0}\"", _levelEndDialogueScreens.Length);
        foreach (AfterLevelDialogueScreen afterLevelDialogueScreen in _levelEndDialogueScreens)
        {
            _dialogueScreensMap.Add(afterLevelDialogueScreen.AfterLevelVariant, afterLevelDialogueScreen);
            Debug.LogFormat("Loaded dialogue screen \"{0}\"", afterLevelDialogueScreen.AfterLevelVariant);
        }
    }

    public AfterLevelDialogueScreen GetDialogueScreen (AfterLevelVariantEnum afterLevelVariant)
    {
        AfterLevelDialogueScreen screen = null;
        _dialogueScreensMap.TryGetValue(afterLevelVariant, out screen);
        if (null == screen) {
            Debug.LogErrorFormat("There is no {0} attached to AfterLevelDialogueManager.", afterLevelVariant.ToString());
            return null;
        }
        return _dialogueScreensMap[afterLevelVariant];
    }

    public void SetAllDialogueScreensActive (bool setting)
    {
        foreach (BaseDialogueScreen baseDialogueScreen in _levelEndDialogueScreens)
        {
            baseDialogueScreen.gameObject.SetActive(setting);
        }
    }
}
