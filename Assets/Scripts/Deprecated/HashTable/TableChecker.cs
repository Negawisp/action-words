using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableChecker : MonoBehaviour
{
    HashWordBook _table;
    
    void Start()
    {
        _table = new HashWordBook(@"C:\Users\BlindEyed\word_release.txt");

        Debug.Log ("абажур: " + _table.Contatins("абажур"));
        Debug.Log ("папа: " + _table.Contatins("папаха"));
        Debug.Log("стол: " + _table.Contatins("столец"));
        Debug.Log("артем: " + _table.Contatins("артем"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
