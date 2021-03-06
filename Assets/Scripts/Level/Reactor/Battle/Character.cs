using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private string _name = null;
        [SerializeField] private Sprite _TurnIcon = null;
        [SerializeField] private int _maxHealth = -1;
        [SerializeField] private int _health = -1;
        [SerializeField] private int _damage = -1;
        [SerializeField] private string _attackType = null;
      //  [SerializeField] private AudioClip _soundattackType;

        public string Name => _name;
        public int Damage => _damage;

        [SerializeField] private  Text _healthText = null;
        [SerializeField] private  Image _progressBar = null;
        [SerializeField] private  Image _avatar = null;
        private Color _initColor;
        private  Text _charInfo;

        [SerializeField] private float _shakeTime = 0.1f;
        [SerializeField] private float _deadShakeTime = 0.5f;
        [SerializeField] private float _shakeSpeed = 30;
        [SerializeField] private float _shakeAmp = 0.8f;
        private float _shakeEndTime = 0;

        [CanBeNull] private Action<string> _deadCallback = null;
        [CanBeNull] private Action _deadAnimEndCallback = null;
        
        [SerializeField] private Animator _attackAnimator = null;
        [SerializeField] private Animator _avatarAnimator = null;

        [SerializeField] private DamageDealer _damageDealer = null;

        [SerializeField] private BattleSoundManager _theSoundManager = null;

        public void SetDeadCallback(Action<string> callback)
        {
            _deadCallback = null;
            _deadCallback += callback;
        }

        public void SetDeadAnimEndCallback(Action callback)
        {
            _deadAnimEndCallback = null;
            _deadAnimEndCallback += callback;
        }

        public void SetEndTurnCallback(Action callback)
        {   if (_damageDealer == null){
             _damageDealer = _attackAnimator.gameObject.GetComponent<DamageDealer>();
            }
            _damageDealer.SetEndTurnCallback(callback);
        }

        public DamageDealer GetDamageDealer(){
            return _damageDealer;
        }

        public Sprite GetTurnIcon()
        {
            return _TurnIcon;
        }

        void Start()
        {
            _theSoundManager = FindObjectOfType<BattleSoundManager>();
            UpdateProgressBar();
            _charInfo = GetComponentInChildren<Text>();
            _charInfo.text = _name;
            _damageDealer = _attackAnimator.gameObject.GetComponent<DamageDealer>();

        }
        
        public void DealDamage(int damage)
        {
            Debug.Log("Damage is being dealt.");
            if (name.Equals("Player"))
                _theSoundManager.PlaySound("Pain");
            
            _health -= damage;
            AnimateDamage();
            UpdateProgressBar();
            
        }


        private void AnimateDamage(){
            if (IsDead())
            {
                _theSoundManager.PlaySound("Death");

                _health = 0;
                StartCoroutine(Shake(_deadShakeTime));
                _deadCallback(_name);
            }
            else
            {         
                StartCoroutine(Shake(_shakeTime));
            }
        }


        public void Attack(int prescaler){
            _attackAnimator.SetTrigger(_attackType);
            Debug.Log("Enemy is attacking with damage of " + prescaler * _damage + ": " + prescaler + " letters and " + _damage + " for each.");
            _damageDealer.SetDamage(prescaler * _damage);

            _theSoundManager.PlaySound(_attackType);
            return;
        }

        public bool IsDead()
        {
            return _health <= 0;
        }

        private void UpdateProgressBar()
        {
            _progressBar.fillAmount = _health / (float) _maxHealth;
            _healthText.text = _health + " / " + _maxHealth;
        }


        private  IEnumerator Shake(float shakeTime)
        {
            if (_avatarAnimator != null){
            _avatarAnimator.SetTrigger("Damaged");
            }
            _shakeEndTime = Time.time + shakeTime;
            Vector2 initPos = _avatar.rectTransform.anchoredPosition;
            int timer = 0;
            while (_shakeEndTime > Time.time)
            {
                timer++;
                Color c = _avatar.color;
                //fade out
                if (IsDead())
                {
                    float a = (_shakeEndTime - Time.time) / _deadShakeTime;
                    c.a = a;
                    _avatar.color = c;
                }
                else
                {
                    float gb =  Mathf.Sin(timer * Time.deltaTime);
                    c.g = gb;
                    c.b = gb;
                    _avatar.color = c;
                }

                Vector2 pos = initPos;
                pos.x += Mathf.Sin(Time.time * _shakeSpeed) * _shakeAmp * Screen.width;
                _avatar.rectTransform.anchoredPosition = pos;
                yield return 1;
            }
            _avatar.rectTransform.anchoredPosition = initPos;
            _avatar.color = _initColor;

            if (IsDead())
            {
                gameObject.SetActive(false);
                if (_deadCallback != null)
                {
                    _deadAnimEndCallback();
                }
                else
                {
                    Debug.Log("_deadCallback is null!");
                }
            }
        }

        private void OnEnable()
        {
            _initColor = _avatar.color;
            _health = _maxHealth;
            UpdateProgressBar();
        }

        private void OnDisable()
        {
            _avatar.color = _initColor;
        }
    }
}
