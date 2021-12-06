using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _wand = null;
    [SerializeField] private Liner _liner = null;

    private Animator _animator;

    private bool _selectingLetters;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _selectingLetters = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_selectingLetters)
        {
            Vector3 wandPosition = _wand.transform.position;
            _liner.OnLettersBeingHighlighted(_wand.transform.position);
        }
    }

    public void ShowTutorial ()
    {
        _animator.SetTrigger("tutorial");
    }

    public void EndTutorial ()
    {
        _selectingLetters = false;
        _liner.Clear(null);
    }

    public void SelectLetter ()
    {
        _selectingLetters = true;
        _liner.AddLetter(_wand.transform.position);
    }
}
