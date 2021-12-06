using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        //[SerializeField] private RectTransform _map;
        //[SerializeField] private RectTransform _levelFinishedMenu;
        //[SerializeField] private 
    
        // Start is called before the first frame update
        void Start()
        {
            // Map.DOAnchorPos(new Vector2( 0,0), 0.25f);

        }

        
        public void BacktoMainMenu()
        {
            SceneManager.LoadScene("Scenes/MainMenu");
            // SceneManager.SetActiveScene(Scene);
        }
        
        public void LevelStartButton()
        {
            //_map.DOAnchorPos(new Vector2( 0,1700), 0.25f);
            //LevelFinishedMenu.DOAnchorPos(new Vector2( 0, 0), 0.25f);


        }

        public void LevelFinishButton()
        {
            //_map.DOAnchorPos(new Vector2(0, 0), 0.25f);
            //_levelFinishedMenu.DOAnchorPos(new Vector2(1300, 2000), 0.25f);
        }

        public void AnimationOnLevelFinished(GameObject Puzzle)//, GameObject Map,  GameObject EndScreen, GameObject BlackScreen)
        {
            //CREATE A SEQUANCE WITH DELAY 0.25f in the beggining.Ask Vlad about  


            Debug.Log("DOTweenCheck: UIManager: AnimationOnLevelFinished");
            Puzzle.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -2000), 0.5f);
            Puzzle.GetComponent<Image>().DOFade(0f, 0.25f);
            
             //EndScreen.Ge
             //EndScreen.SetActive(true);
        }
    }
}