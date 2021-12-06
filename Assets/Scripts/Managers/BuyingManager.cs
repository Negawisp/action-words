

using UnityEditor;
using UnityEngine;

/// <summary>
/// Class is used to work with Money Progress InApp currencies OnLevelFinished, with Tips or maybe daily tasks
/// </summary>
public class BuyingManager: MonoBehaviour
{
    [SerializeField] private int _tipPrice = -1;
    [SerializeField] private CoinsPanelManager _coinsPanelManager = null;

    //[SerializeField] private GameObject _openRandomTip;
    
    void Start()
    {
        //_openRandomTip = this.gameObject.GetComponent<OpenRandomTip>();
    }
    
    /// <summary>
    /// Add x || Subtract (-x) coins or another currency  
    /// </summary>
    private bool AddCoins(int x)
    {
  
        if (UserOptions.Instance.CoinsNumber.Value < Mathf.Abs(x) && x<0)
        {
            Debug.LogError("Not enough gold, you have " + UserOptions.Instance.CoinsNumber.Value);
            return false;                
        }
        else
            UserOptions.Instance.CoinsNumber.Value += x;

        return true;
    }

    public bool BuyRandomLetterTip()
    {
        // if (_openRandomTip.gameObject.GetComponent<OpenRandomTip>() == null)
        // {
        //     Debug.Log("Gameobject has no OpenRandomTip Component");
        //     return;
        // }
        // else
        //     _openRandomTip.gameObject.GetComponent<OpenRandomTip>().OpenRandomCells(1);

        if (AddCoins(-_tipPrice))
        {
            _coinsPanelManager.RefreshCoinsNumber();
            UserOptions.Instance.OpenRandomLettersTipsNumber.Value++;

            Debug.Log("RandomTip was successfully bought");
        }
        else
        {
            return false;
        }
        ////_moneyPanelManager.MoneyAnimation();
        return true;

    }

    public void EarnCoins(int coinsEarned)
    {
        AddCoins(coinsEarned);
        _coinsPanelManager.RefreshCoinsNumber();
    }

    
    //DO IT 25.11
    public void OnLevelFinishedMoneyOperarions(int coinsEarned)
    {
        //formula taken from economy design
        AddCoins(coinsEarned);
        _coinsPanelManager.RefreshCoinsNumber();
        
        Debug.Log("Money operation after level was successfully done");
        ////_moneyPanelManager.MoneyAnimation();
    }
    
    public void BrokePlayerPrefs()
    {
        var text = Base64.Encode("GoldCurrency");
        Debug.LogError("Edit PlayerPrefs " + text);
        PlayerPrefs.SetInt(text, 1000);
        _coinsPanelManager.RefreshCoinsNumber();
    }
}
