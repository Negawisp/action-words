using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackgroundSetter : MonoBehaviour
{
    public Sprite[] SpriteArray;

    public int NumberLevelFirstLocation;
    public int NumberLevelSecondLocation;

    public void SetBackground (Sprite bgSprite)
    {
        if (null != bgSprite)
        {
            this.gameObject.GetComponent<Image>().sprite = bgSprite;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[0];
        }
    }

    public void SetBackground(int levelNumber)
    {
        if (levelNumber > NumberLevelFirstLocation)
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[1];
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[0];
        }

    }

    public void SetLevelFinishedPanelBackground(Sprite sprite)
    {
        if (null != sprite)
        {
            this.gameObject.GetComponent<Image>().sprite = sprite;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[0];
        }
    }

    public void SetLevelFinishedPanelBackground(int levelNumber)
    {
        switch (levelNumber)
        {
            case 0:
            {
                this.gameObject.GetComponent<Image>().sprite = SpriteArray[0 % 5];
                break;
            }
            case 1:
            {
                this.gameObject.GetComponent<Image>().sprite = SpriteArray[1 % 5];
                break;
            }
            case 2:
            {
                this.gameObject.GetComponent<Image>().sprite = SpriteArray[2 % 5];
                break;
            }
            case 3:
            {
                this.gameObject.GetComponent<Image>().sprite = SpriteArray[3 % 5];
                break;
            }
            case 4:
            {
                this.gameObject.GetComponent<Image>().sprite = SpriteArray[4 % 5];
                break;
            }

        }
    }

}
