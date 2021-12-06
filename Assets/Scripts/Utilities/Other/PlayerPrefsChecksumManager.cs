using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsChecksumManager
{
    private string key;
    public HashSet<string> safePlayerPrefNames = new HashSet<string>();
    public Base64 Base64;

    public PlayerPrefsChecksumManager (string key, params string [] sppNames)
    {
        this.key = key;
        foreach (string sppName in sppNames)
            this.safePlayerPrefNames.Add(sppName);
        SaveChecksum();

    }

    // Вычисляем контрольную сумму
    private string GenerateChecksum ()
    {
        // Debug.LogError("Generate1");
        string hash = "";
        foreach (var property in safePlayerPrefNames)
        {
            var property1 = Base64.Encode(property);
            hash += property1 + ":";
            if (PlayerPrefs.HasKey(property1))
            {
                
                hash += (PlayerPrefs.GetInt(property1)).ToString();
            }

        }
        return Md5Sum(hash + key);
    }

    // Сохраняем контрольную сумму
    public void SaveChecksum()
    {
        string checksum = GenerateChecksum();
        PlayerPrefs.SetString("CHECKSUM" + key, checksum);
        PlayerPrefs.Save();
    }

    // Проверяем, изменялись ли данные
    public bool ChecksumIsOK ()
    {
        if (! PlayerPrefs.HasKey("CHECKSUM" + key))
            return true;

        // Debug.LogError("CheckSumisOK");
        string checksumSaved = PlayerPrefs.GetString("CHECKSUM" + key);
        string checksumReal = GenerateChecksum();

        return checksumSaved.Equals(checksumReal);
    }

    // ...
    private string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
 
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
 
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
 
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
 
        return hashString.PadLeft(32, '0');
    }
}
    
