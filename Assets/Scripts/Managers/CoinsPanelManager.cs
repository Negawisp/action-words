



using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinsPanelManager: MonoBehaviour
{
    [SerializeField] private GameObject[] _moneyPanels;
    
    
    
    void Start()
    {
        RefreshCoinsNumber();
    }

    public void RefreshCoinsNumber()
    {
        foreach (var moneyPanel in _moneyPanels)
        {
            foreach (Transform child in moneyPanel.transform)
            {
                if (child.GetComponent<Text>() != null)
                    child.gameObject.GetComponent<Text>().text = "" + UserOptions.Instance.CoinsNumber.Value;
            }
        }
    }

    public void MoneyAnimation()
    {}
    
    /// <summary>
    /// 
    /// </summary>
    public void Load()
    {}
    
 
    
}
