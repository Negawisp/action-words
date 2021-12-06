using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPGameobjectsPool : MonoBehaviour
{
    [SerializeField] private List<BaseScrollLetter> _letters = null;
    [SerializeField] private Transform _lettersHolder = null;

    protected void Start()
    {
        if (_letters.Capacity == 0)
        { Debug.LogError("Capacity of Letters pool is 0."); }
    }

    public BaseScrollLetter GetLetter()
    {
        BaseScrollLetter item = _letters[0];
        if (null == item)
        {
            Debug.Log("Letter pool is empty!!");
        }
        Debug.Log("Getting PentaLetter \"" + item.name + "\"");
        _letters.RemoveAt(0);
        return item;
    }

    private void Store (BaseScrollLetter letter)
    {
        Debug.Log("Returning PentaLetter \"" + letter.name + "\" to pool.");
        _letters.Add(letter);
        letter.transform.SetParent(_lettersHolder);
        letter.transform.localPosition = Vector2.zero;
        //letter.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        //letter.GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }

    public void Store (BaseScrollLetter[] letters)
    {
        int n = letters.Length;
        for (int i = 0; i < n; i++) Store(letters[i]);
    }
}
