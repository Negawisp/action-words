using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class OnlineBattleManager : MonoBehaviour, IBattleManager
    {
        [SerializeField] private Animator _bubbleAnimator = null;
        [SerializeField] private Text _bubbleText = null;

        [SerializeField] private PentaPuzzle _boardGame = null;
        [SerializeField] private CanvasGroup _boardCanvas = null;
        [SerializeField] private Character _player = null;
        [SerializeField] private Character _enemy = null;

        [SerializeField] private Text _playerActivatedWords = null;
        [SerializeField] private Text _enemyActivatedWords = null;

        [SerializeField] private TurnLabel[] _turnLabels = null;
        [SerializeField] private GameObject _changeScrollButton = null;

        private bool _isPlayerTurn = true;
        private bool _enemyDead;

        private string _lastWord;
        private bool _enemyAttackReady = false;
        private bool _enemyChangeScrollReady = false;

        [CanBeNull] private Action<string> _endTurnCallback;
        [CanBeNull] private Action<bool> _endBattleCallback;


        private void OnEnable()
        {
            _boardGame.SetWordActivationCallback(OnWordActivation);
            _boardGame.AddExtraActionCallback(OnSkipScroll);
            _player.SetDeadCallback(OnCharacterDead);
            _player.SetDeadAnimEndCallback(OnDeadAnimationEnd);
            _player.SetEndTurnCallback(OnEndTurn);
            _player.GetDamageDealer().SetTarget(_enemy);
            _enemy.SetDeadCallback(OnCharacterDead);
            _enemy.SetDeadAnimEndCallback(OnDeadAnimationEnd);
            _enemy.SetEndTurnCallback(OnEndTurn);

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setBossIcon(_enemy.GetTurnIcon());
            }
        }


        public void FixedUpdate()
        {
            if (_enemyAttackReady)
            {
                _enemyAttackReady = false;
                enemyAction(_lastWord);
            }

            if (_enemyChangeScrollReady)
            {
                _enemyChangeScrollReady = false;
                EnemyChangeScroll();
            }
        }

        public void StartBattle(bool isPlayerTurn, string bossName)
        {
            Debug.Log("StartBattle");
            if (!isPlayerTurn)
                ConnectionManager.GetMsg(SetEnemyActionReady);

            _isPlayerTurn = isPlayerTurn;
            _boardCanvas.blocksRaycasts = _isPlayerTurn;

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setTurn(_isPlayerTurn);
            }

            _enemy = gameObject.transform.Find(bossName)?.GetComponent<Character>();
            if (_enemy == null)
            {
                Debug.LogError("Bad boss name");
            }
            _enemyActivatedWords = _enemy.transform
                                         .Find("ActivatedWordsPlaceholder")
                                         .Find("ActivatedWords")
                                         .GetComponent<Text>();

            gameObject.SetActive(true);
            _player.gameObject.SetActive(true);
            _enemy.gameObject.SetActive(true);
            _boardGame.gameObject.SetActive(true);
            _boardGame.Load();
        }

        public void SetEndTurnCallback(Action<string> callback)
        {
            _endTurnCallback += callback;
        }

        public void SetBattleEndCallback(Action<bool> callback)
        {
            _endBattleCallback += callback;
        }



        public void SetEnemyActionReady(string word)
        {
            if (word[0] == '-')
            {
                _enemyChangeScrollReady = true;
                return;
            }

            if (word.Length > 1)
            {
                _lastWord = word;
                _enemyAttackReady = true;
            }
        }
        //OldToDo: move to character, AI should depend on boss.
        public void enemyAction(string word)
        {
            /*
            if (_enemy.IsDead()) return;
            
            List<string> words = _boardGame.GetSelectableWords();
            StartCoroutine(_boardGame.EmulateWordActivation(word));
            */
        }
        
        public void EnemyChangeScroll()
        {
            Debug.Log("EnemyChangeScroll");
            _boardGame.ExtraAction();
            ConnectionManager.GetMsg(SetEnemyActionReady);
        }


        private void OnWordActivation(string word, List<ScorredLetter> scorredLetters)
        {
            if (_isPlayerTurn)
            {
                _player.Attack(word.Length);
                _bubbleText.text = word.ToUpper() + "!";
                _bubbleAnimator.SetTrigger("Show");
                _player.Attack(word.Length);
                _playerActivatedWords.text = _playerActivatedWords.text + word + "\n";
                _endTurnCallback(word);
            }
            else
            {
                _enemy.Attack(word.Length);
                _enemyActivatedWords.text = _enemyActivatedWords.text + word + "\n";
                _changeScrollButton.SetActive(true);
            }
        }

        private void OnCharacterDead(string deadName)
        {
            Debug.Log(deadName + " is dead!");
            if (_endBattleCallback != null)
            {
                _boardGame.LoadOut();
                _enemyDead = !(deadName.Equals(_player.Name));
            }
            else
            {
                Debug.Log("_endBattleCallback is null!");
            }
        }

        private void OnDeadAnimationEnd()
        {
            _endBattleCallback(_enemyDead);
            gameObject.SetActive(false);
        }


        private void OnEndTurn()
        {
            _isPlayerTurn = !_isPlayerTurn;
            _boardCanvas.blocksRaycasts = _isPlayerTurn;

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setTurn(_isPlayerTurn);
            }

            // Enemy deals damage to player
            if (!_isPlayerTurn)
            {
                Debug.Log("Boss turn");
                //enemyAction();
            }
            else
            {
                Debug.Log("Player turn");
            }
        }

        private void OnSkipScroll(object newScroll)
        {
            _playerActivatedWords.text = "";
            _enemyActivatedWords.text = "";
        }
    }
}
