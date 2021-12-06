using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Characters : MonoBehaviour
{
    public GameObject Aliana;
    public GameObject Mother;
    public GameObject FishMan;

    [SerializeField] Sprite[] Sprites = null;

    private void Start()
    {
        Image AlianaImage = Aliana.GetComponent<Image>();
        AlianaImage.sprite = Sprites[0];
    }
 
}
