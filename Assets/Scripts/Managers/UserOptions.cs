using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Location;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A singleton existing globally between scenes and containing all the user's game data and settings.
/// 
/// <b>One and only one instance of this must exist through all the gametime, so every scene must
/// have a GameObject with this script in order for Awake() to check the singleton's existence.</b>
/// </summary>
public class UserOptions : MonoBehaviour
{
    public static UserOptions Instance;

    public HashWordBook WordBook { get; private set; }
    public string UserLanguage { get; private set; }

    public Base64 Base64;

    public ErrorTable ErrorButton;

    public string ShopIp
    {
        get
        {
            return PlayerPrefs.GetString("shopIp");
        }
        set
        {
            PlayerPrefs.SetString("shopIp", value);
        }
    }
    
    
    
    public class IntPlayerPref
    {
        public readonly string PrefName;
        private int _value;
        //public Encoder
        public virtual int Value
        {
            get => _value;
            internal set
            {
                _value = value;
                PlayerPrefs.SetInt(PrefName, value);
            }
        }        

        internal IntPlayerPref(string prefName)
        {
            string encodedPrefName = Base64.Encode(prefName);
            PrefName = encodedPrefName;
            Value = PlayerPrefs.GetInt(PrefName);
            
        }
    }
    
    public class SafeIntPlayerPref : IntPlayerPref
    {
        private static PlayerPrefsChecksumManager _checksumManager;

        internal static ErrorTable ButtonError;
        internal static void InitializeChecksumManager(string key, string[] prefNames)
        {
            _checksumManager = new PlayerPrefsChecksumManager(key, prefNames);
        }

        public SafeIntPlayerPref(string prefName) : base(prefName)
        {
            if (null == _checksumManager)
            {
                Debug.LogWarning("SafeIntPlayerPref class did not have static field \"_checksumManager\" initialized.");
            }
        }

        private void OnChecksumNotOk()
        {
            ButtonError.gameObject.SetActive(true);
        }

        

        public override int Value
        {
            get
            {
                if (_checksumManager.ChecksumIsOK())
                {
                    return base.Value;
                }
                else
                {
                    OnChecksumNotOk();
                    return base.Value;;
                }
                
            }
            internal set
            {
                if (_checksumManager.ChecksumIsOK())
                {
                    base.Value = value;
                    _checksumManager.SaveChecksum();
                }
                else
                {
                    OnChecksumNotOk();
                }
            }
        }
    }

    private IntPlayerPref AppWasNeverLaunched = null;

    public IntPlayerPref OpenRandomLetterTipIsUnlocked = null;
    public IntPlayerPref OpenRandomWordTipIsUnlocked = null;
    public IntPlayerPref OpenChosenCellTipIsUnlocked = null;

    public IntPlayerPref CompletedButtonsNumber = null;
    public IntPlayerPref OpenRandomLettersTipsNumber = null;
    public IntPlayerPref OpenRandomWordTipsNumber = null;
    public IntPlayerPref OpenChosenLetterTipsNumber = null;
    public SafeIntPlayerPref CoinsNumber = null;
    /// <summary>
    /// On scene loading:
    /// If the Instance doesn't exist, loads it;
    /// Else destroys this excessive instance.
    /// </summary>
    private void Awake()
    {
        if (null == Instance)
        {
            Load();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(this);
        }
    }

    private void Load()
    {
        UserLanguage = GetUserLanguage();
        string wordbookAssetPath = WordbookAssetPath.Get(UserLanguage).Path;
        TextAsset textAsset = Resources.Load<TextAsset>(wordbookAssetPath);
        if (null == textAsset) { Debug.LogErrorFormat("Couldn't find text asset {0}", wordbookAssetPath); }
        WordBook = new HashWordBook(textAsset);


        string[] sppNames = {"GoldCurrency"};//, "OpenRandomLettersTipsNumber", "OpenRandomWordTipsNumber", "OpenChosenLetterTipsNumber" };
        SafeIntPlayerPref.InitializeChecksumManager("a", sppNames);
        SafeIntPlayerPref.ButtonError = ErrorButton;

            CoinsNumber = new SafeIntPlayerPref("GoldCurrency");
        OpenRandomLettersTipsNumber = new SafeIntPlayerPref("OpenRandomLettersTipsNumber");
        OpenRandomWordTipsNumber = new SafeIntPlayerPref("OpenRandomWordTipsNumber");
        OpenChosenLetterTipsNumber = new SafeIntPlayerPref("OpenChosenLetterTipsNumber");

        AppWasNeverLaunched = new IntPlayerPref("WasNeverLaunched");

        OpenRandomLetterTipIsUnlocked = new IntPlayerPref("OpenRandomLetterTipUnlocked");
        OpenRandomWordTipIsUnlocked = new IntPlayerPref("OpenRandomWordTipUnlocked");
        OpenChosenCellTipIsUnlocked = new IntPlayerPref("OpenChosenCellTipUnlocked");
        CompletedButtonsNumber = new IntPlayerPref("CompletedLevelsNumber");

        //ToDo: This is the place with hardcoded player prefs
        AppWasNeverLaunched.Value = 0;
        if (AppWasNeverLaunched.Value >= 0)
        {
            PlayerPrefs.DeleteAll();
            SetInitialPlayerPrefs();
        }
        //SetDebugPlayerPrefs();
        //end ToDo
        
        
        
    }

    private void SetInitialPlayerPrefs()
    {
        OpenRandomLetterTipIsUnlocked.Value = -1;
        OpenRandomWordTipIsUnlocked.Value = -1;
        OpenChosenCellTipIsUnlocked.Value = -1;

        CompletedButtonsNumber.Value = 22;
        OpenRandomLettersTipsNumber.Value = 5;
        OpenRandomWordTipsNumber.Value = 2;
        OpenChosenLetterTipsNumber.Value = 3;
        CoinsNumber.Value = 100;

        AppWasNeverLaunched.Value = -1;

        ShopIp = "10.0.0.5";
    }

    private void SetDebugPlayerPrefs()
    {
        OpenRandomLetterTipIsUnlocked.Value = -1;
        OpenRandomWordTipIsUnlocked.Value = -1;
        OpenChosenCellTipIsUnlocked.Value = -1;

        CompletedButtonsNumber.Value = 22;
        OpenRandomLettersTipsNumber.Value = 0;
        OpenRandomWordTipsNumber.Value = 2;
        OpenChosenLetterTipsNumber.Value = 3;
        CoinsNumber.Value = 100;
    }

    private string GetUserLanguage()
    {
        return "rus";
    }

    private string GetLevelExtraWordsPrefName(ILevel level)
    {
        return string.Format("LVL_TYPE{0}__ID{1}", level.GetType().FullName, level.LevelID);
    }

    public void SaveExtraWords(in ILevel level, HashSet<string> words)
    {
        string wordsAsString = HashsetSerializer.Deserialize(words);
        PlayerPrefs.SetString(GetLevelExtraWordsPrefName(level), wordsAsString);
    }

    public void LoadExtraWords(in ILevel level, out HashSet<string> words)
    {
        string wordsAsString = PlayerPrefs.GetString(GetLevelExtraWordsPrefName(level));
        HashsetSerializer.Serialize(wordsAsString, out words);
    }

    public void TakeShopIpInput()
    {
        string shopIp = FindObjectOfType<InputField>().textComponent.text;
        ShopIp = shopIp;
    }
}
