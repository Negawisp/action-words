using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private ServerConnectionManager _serverConnectionManager = null;
    [SerializeField] private BuyingManager _buyingManager = null;

    public void Request500CoinsPurchase()
    {
        Debug.Log("Requesting 500 coins purchase...");
        _serverConnectionManager.InitiateGetRequest("purchase/500Coins", Handle500CoinsResponse);
    }

    private void Handle500CoinsResponse(ResponseBody responseBody)
    {
        Debug.Log("Handling 500 coins purchase response...");
        if (responseBody.status.Equals("Success")) {
            _buyingManager.EarnCoins(500);
        }
    }
}
