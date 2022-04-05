using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private Sprite _storyicon;
    [SerializeField] private Sprite _blockicon;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Completed levels: " + UserOptions.Instance.CompletedButtonsNumber.Value);
        SetButtonsActive(UserOptions.Instance.CompletedButtonsNumber.Value);
        //HighlightButtonAsNew(UserOptions.Instance.CompletedButtonsNumber.Value);
    }

    private void SetButtonsActive(int completedButtonsNumber)
    {
        for (int i = 0; i < completedButtonsNumber; i++)
        {
            UnlockButton(i);
        }
        for (int i = completedButtonsNumber; i < _buttons.Length; i++)
        {
            LockButton(i);
        }

        if (completedButtonsNumber < _buttons.Length)
        {
            UnlockButton(completedButtonsNumber);
        }
    }

    private void HighlightButtonAsNew(int buttonNumber)
    {
        GameObject button_current = _buttons[buttonNumber];
        GameObject button_previous = null;
        if (buttonNumber > 0)
            button_previous = _buttons[buttonNumber - 1];
        
        if (button_current.gameObject.GetComponent<Image>() != null &&
            button_previous.gameObject.GetComponent<Image>() != null)
        {
            button_previous.gameObject.GetComponent<Image>().color = Color.white;
            button_current.gameObject.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void UnlockButton(int buttonNumber)
    {
        if (buttonNumber >= _buttons.Length)
        {
            return;
        }
        if(buttonNumber>0)
            HighlightButtonAsNew(buttonNumber);
        //////////////////////////
        //_buttons[buttonNumber].SetActive(true);
        //////////////////////////

        ///////////////////////
        GameObject button = _buttons[buttonNumber];
        button.GetComponent<Button>().enabled = true;
        switch (button.tag)
        {
            case "Fight":
            {
                // foreach (Transform child in button.transform)
                // {
                //     child.gameObject.SetActive(true);
                // }
                break;
            }
            case "Level":
            {
                foreach (Transform child in button.transform)
                {
                    if (child.gameObject.GetComponent<Text>() != null)
                    {
                        child.gameObject.SetActive(true);
                    }
                    if (child.gameObject.GetComponent<Image>() != null && child.gameObject.GetComponent<Image>().sprite == _blockicon) 
                        child.gameObject.SetActive(false);            
                }
                break;
            }
            case "Novella":
            {
                foreach (Transform child in button.transform)
                {
                    child.gameObject.GetComponent<Image>().sprite = _storyicon;
                    child.gameObject.GetComponent<Image>().SetNativeSize();
                    child.gameObject.SetActive(true);
                }
                break;
            }

        }
         
        // int flag = 0;
        // foreach (Transform child in button.transform)
        // {
        //     if (child != button.transform)
        //     {
        //         //levels Buttons
        //         if (child.gameObject.GetComponent<Text>() != null)
        //         {
        //             child.gameObject.SetActive(true);
        //             flag = 1; 
        //         }
        //         if (child.gameObject.GetComponent<Image>()!=null && child.gameObject.GetComponent<Image>().sprite == _blockicon) 
        //             child.gameObject.SetActive(false);
        //         
        //         //Novellas buttons
        //         if (child.gameObject.GetComponent<Text>() == null && flag != 1 )
        //         {
        //         }
        //         //if (child.gameObject.tag.Equals("Fight"))
        //             
        //
        //
        //
        //
        //
        //     }
        // }
        //////////////////////
    }

    public void LockButton(int buttonNumber)
    {
        if (buttonNumber >= _buttons.Length)
        {
            return;
        }
        /////////////////////////
        //_buttons[buttonNumber].SetActive(false);
        /////////////////////////
        
        ///////////////////////
        GameObject button = _buttons[buttonNumber];
        button.GetComponent<Button>().enabled = false;

        foreach (Transform child in button.transform)
        {
            if (child != button.transform)
            {
                switch (button.tag)
                {
                    case "Level":
                    {
                        if (child.gameObject.GetComponent<Text>()!=null)
                            child.gameObject.SetActive(false);
                        break;
                    }
                    case "Novella":
                    {
                        child.gameObject.GetComponent<Image>().sprite = _blockicon;
                        child.gameObject.GetComponent<Image>().SetNativeSize();
                        break;
                    }
                    case "Fight":
                        break;
                }
            }
        }
        //////////////////////
    }


    
}
