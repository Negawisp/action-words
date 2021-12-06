using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnectionManager : MonoBehaviour
{
    static readonly HttpClient client = new HttpClient();

    [SerializeField] private string _purchaseServiceIp = "10.0.0.5";
    [SerializeField] private string _purchaseServicePort = "8080";
    
    private string responseBodyJSON;


    // Start is called before the first frame update
    void Start()
    {
        _purchaseServiceIp = UserOptions.Instance.ShopIp;
        responseBodyJSON = "Nothing to see here.";
    }

    public void InitiateGetRequest(string request, Action<ResponseBody> onResponseReceived)
    {
      //StartCoroutine(GetRequest(url, onResponseReceived));
        GetRequest(string.Format("http://{0}:{1}/{2}", _purchaseServiceIp, _purchaseServicePort, request), onResponseReceived);
    }


    private async void GetRequest(string url, Action<ResponseBody> onResponseReceived)
    {
        try
        {
            //HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            Debug.LogFormat("Sending a request to {0}...", url);
            string responseBody = await client.GetStringAsync(url);
            Debug.Log(responseBody);
            ResponseBody parsedBody = new ResponseBody();
            JsonUtility.FromJsonOverwrite(responseBody, parsedBody);
            onResponseReceived(parsedBody);
        }
        catch (HttpRequestException e)
        {
            Debug.LogErrorFormat("Message :{0} ", e.Message);
        }
    }

    /*
    private IEnumerator GetRequest(string url, Action<ResponseBody> onResponseReceived)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log("Sending a request...");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            // Show results as text
            responseBodyJSON = www.downloadHandler.text;
            Debug.Log(responseBodyJSON);

            ResponseBody parsedBody = JsonUtility.FromJson<ResponseBody>(responseBodyJSON);
            onResponseReceived(parsedBody);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }*/
}
