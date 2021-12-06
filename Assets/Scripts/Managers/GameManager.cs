using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Location;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Canvases that are necessary on scene
    /// </summary>
    [SerializeField] private GameObject _puzzleCanvas = null;
    [SerializeField] private GameObject _mapCanvas = null;
    
    /// <summary>
    /// Managers that are necessary on scene
    /// </summary>
    [SerializeField] private LevelManager _levelManager = null;
    [SerializeField] private CutsceneManager _cutsceneManager = null;
    [SerializeField] private BackgroundSetter _OnImageChoose = null;
    [SerializeField] private BackgroundSetter _onImangeFinishedPanelChoose = null;
    [SerializeField] private MapManager _buttonMapManager = null;
    [SerializeField] private BuyingManager purchaseManager = null;

    [SerializeField] private AfterLevelDialogueManager _afterLevelDialogueManager = null;


    private UIManager _uiManager;
    private int _locationNumber;
    private int _levelID;
    private int _cutsceneID;
    
    private int _nextLevelID;
    private int _nextCutsceneID;

    public bool CUTSCENES_ENABLED = true;

    private void Awake()
    {
    }

    public ErrorTable ErrorButton;
    void Start()
    {
        _uiManager = GetComponent<UIManager>();
        _levelManager.AddPuzzleFinishedCallback(OnPuzzleCompleted);
        _levelManager.AddDefeatCallback(OnPuzzleLost);
        _levelID = -1;


        _levelManager.gameObject.SetActive(true);
        _levelManager.HideReactorsAndPuzzles();
        _mapCanvas.gameObject.SetActive(true);
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);

        UserOptions.Instance.ErrorButton = ErrorButton;

        UserOptions.SafeIntPlayerPref.ButtonError = ErrorButton;
    }

    /// <summary>
    /// Opens up a new or RollsUp a saved level.
    /// </summary>
    /// <param name="levelNumber">Number of a level to open.</param>
    public void OnMapLevelButtonPressed (int levelNumber)
    {
        Debug.LogFormat("Launching level {0}.", levelNumber);

        _levelID = levelNumber;
        Debug.LogWarningFormat("Loading level with id {0}, levels map has such keys: {1}", _levelID, _levelManager.Levels.Keys.ToString());
        AbstractCutscene openingCutscene = _levelManager.Levels[_levelID].OpeningCutscene;
        Debug.LogFormat("Opening cutscene is null: {0}", (null == openingCutscene));

        //ToDo:
        //  Refactor the conditions. Move part of this into the LevelManager probably.
        if (null != _levelManager.CurrentLevel)
        {
            if (_levelManager.CurrentLevel.LevelID == _levelID)
            {
                Debug.Log("Loading an opened level.");
                _levelManager.RollLevelUp();
            }
            else
            {
                Debug.Log("Unloading an opened level.");
                _levelManager.LoadLevelOut();
                OnMapLevelButtonPressed(levelNumber);
                return;
            }
        }
        else
        {
            Debug.Log("No open level detected during openning a new level.");
        }

        if (null == _levelManager.CurrentLevel ||
            _levelManager.CurrentLevel.LevelID != _levelID)
        {
            if (null != openingCutscene)
            {
                _puzzleCanvas.gameObject.SetActive(false);
                _cutsceneManager.LaunchCutsene(openingCutscene, SwitchOnCurrentLevel);
            }
            else
            {
                Debug.LogWarningFormat("Opening cutscene in level {0} is required. Please, create and set a cutscene in an inspector.", _levelID);
                SwitchOnCurrentLevel();
            }
            _OnImageChoose.SetBackground(_levelManager.Levels[_levelID].Background);
            _onImangeFinishedPanelChoose.SetLevelFinishedPanelBackground(_levelManager.Levels[_levelID].Icon);
        }
        _mapCanvas.gameObject.SetActive(false);
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
    }

    public void LaunchCutscene(int cutsceneID)
    {
        _levelManager.HideReactorsAndPuzzles();
        _mapCanvas.gameObject.SetActive(false);
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
        _cutsceneManager.SetSubmanagersActive(false);

        _levelID = _cutsceneManager.DetachedCutscenes[cutsceneID].AttachedLevelID;
        if (_levelID >= 0)
        {
            _cutsceneManager.LaunchCutsene(cutsceneID, SwitchOnCurrentLevel);
        } else
        {
            _cutsceneManager.LaunchCutsene(cutsceneID, GoToMap);
        }

        if (cutsceneID > UserOptions.Instance.CompletedButtonsNumber.Value)
        {
            throw new Exception("Inconsistancy in ID tracking!!!");
        }
        if (cutsceneID == UserOptions.Instance.CompletedButtonsNumber.Value)
        {
            _buttonMapManager.UnlockButton(++UserOptions.Instance.CompletedButtonsNumber.Value);
        }
    }

    public void LaunchCurrentCutscene()
    {
        LaunchCutscene(_cutsceneID);
    }

    public void LaunchNextCutscene()
    {
        LaunchCutscene(_nextCutsceneID);
    }



    public void SwitchOnCurrentLevel()
    {
        SwitchOnLevel(_levelID);
    }

    public void ChangeAndSwitchOnLevel()
    {
        _levelManager.LoadLevelOut();
        OnMapLevelButtonPressed(_levelID);
    }

    public AbstractLevel SwitchOnLevel(int level)
    {
        _mapCanvas.gameObject.SetActive(false);
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
        _puzzleCanvas.gameObject.SetActive(true);

        _levelID = level;
        return _levelManager.LoadLevel(level);
    }

    private bool LevelWasNew()
    {
        return (UserOptions.Instance.CompletedButtonsNumber.Value - _levelID) < 1;
    }


    /// <summary>
    /// Happens when a goal of a current level has been achieved.
    /// </summary>
    private void OnPuzzleCompleted()
    {
        Debug.LogFormat("Completed level {0}", _levelID);
        AbstractCutscene endingCutscene = _levelManager.Levels[_levelID].EndingCutscene;
        int baseRewardMultiplier = 1;
        int nExtraWords = _levelManager.CurrentReactor.NewExtraWordsFound;
        int earnedGold = 0;

        if (!LevelWasNew()) { baseRewardMultiplier = 0; }
        earnedGold = 5 * baseRewardMultiplier + 2 * nExtraWords;
        purchaseManager.OnLevelFinishedMoneyOperarions(earnedGold);

        Debug.LogFormat("{0} extra words opened.", nExtraWords);
        if (null == endingCutscene)
        {
            Debug.LogWarningFormat("Ending cutscene in level {0} is required. Please, create and set a cutscene in an inspector.", _levelID + 1);
            ShowVictoryDialogue(earnedGold);
        }
        else
        {
            _cutsceneManager.LaunchCutsene(endingCutscene, () => { ShowVictoryDialogue(earnedGold); });
        }
    }

    /// <summary>
    /// Happens when a goal of a current level is unachievable.
    /// </summary>
    private void OnPuzzleLost()
    {
        Debug.LogFormat("Lost level {0}", _levelManager.Levels[_levelID]);
        ShowDefeatDialogue();
    }


    private void ShowVictoryDialogue(int earnedGold)
    {
        int variantID = _levelManager.Levels[_levelID].AfterLevelVariantID;
        AfterLevelVariantEnum afterLevelVariant = _levelManager.Levels[_levelID].AfterLevelVariant;
        Debug.LogFormat("AfterLevelVariant: {0}", afterLevelVariant);
        switch (afterLevelVariant)
        {
            case AfterLevelVariantEnum.NextLevel:
                _nextLevelID = variantID; break;
            
            case AfterLevelVariantEnum.LaunchCutscene:
                _nextCutsceneID = variantID; break;

            case AfterLevelVariantEnum.Nothing:
                { } break;
                { } break;

            case AfterLevelVariantEnum.Defeat:
                Debug.LogErrorFormat("Defeat can't be victory, can it?"); break;

            default:
                Debug.LogErrorFormat("This AfterLevelVariant is not considered: {0}.", afterLevelVariant); break;
        }
        AfterLevelDialogueScreen dialogueScreen = _afterLevelDialogueManager.GetDialogueScreen(afterLevelVariant);
        if (null == dialogueScreen) {
            Debug.LogErrorFormat("No such dialogue screen!");
            return;
        }
        else {
            Debug.LogFormat("Showing AfterLevelDialogue");
            dialogueScreen.Show(earnedGold);
        }
    }

    private void ShowDefeatDialogue()
    {
        _afterLevelDialogueManager
            .GetDialogueScreen(AfterLevelVariantEnum.Defeat)
            .Show();
    }

    public void GoToMap ()
    {
        _mapCanvas.gameObject.SetActive(true);
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
    }

    public void Win()
    {
        _levelManager.LoadLevelOut();
        //_afterLevelDialogueManager.SetAllDialogueScreensActive(false);
        Debug.LogWarningFormat("Completed level id: {0}, Completed buttons number: {1}", _levelID, UserOptions.Instance.CompletedButtonsNumber.Value);
        if (_levelID > UserOptions.Instance.CompletedButtonsNumber.Value)
        {
            throw new Exception("Inconsistancy in ID tracking!!!");
        }
        if (_levelID == UserOptions.Instance.CompletedButtonsNumber.Value)
        {
            _buttonMapManager.UnlockButton(++UserOptions.Instance.CompletedButtonsNumber.Value);
        }
    }

    public void Lose()
    {
        _levelManager.LoadLevelOut();
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
    }

    public void Retry()
    {
        Debug.LogFormat("Retrying level {0}", _levelID);
        _levelManager.LoadLevelOut();
        _afterLevelDialogueManager.SetAllDialogueScreensActive(false);
        OnMapLevelButtonPressed(_levelID);
    }

    public void NextLevel()
    {
        OnMapLevelButtonPressed(_nextLevelID);
    }

    public void BackButton ()
    {
        _levelManager.RollLevelDown();
        GoToMap();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
