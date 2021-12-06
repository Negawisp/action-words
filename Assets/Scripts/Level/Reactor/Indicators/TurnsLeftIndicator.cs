using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsLeftIndicator : MonoBehaviour
{
    [SerializeField] Text _turnsLeftText = null;


    public void SetTurnsNumber(int turnsNumber)
    {
        _turnsLeftText.text = string.Format("{0} ходов осталось.", turnsNumber);
    }
}
