using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Former ButtonDisabler
public class GameObjectDisabler : MonoBehaviour
{
    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
