using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : AbstractStage
{
    public Image CharacterPortrait;
    public Text Content;

    public const float PrintRate = 0.02F;
    private String _text;
    private int _currentIndex;
    private bool _printingFinished;

    public Text Continue;

    protected override void Awake()
    {
        base.Awake();
        // For future reference: may read from files here.
        _text = Content.text;
        Content.text = "";
    }

    public override IEnumerator Run()
    {
        _currentIndex = 0;
        CharacterPortrait.enabled = true;
        Content.enabled = true;
        yield return StartCoroutine(_print());
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }

    private IEnumerator _print()
    {
        _printingFinished = false;
        for (; _currentIndex < _text.Length; _currentIndex++)
        {
            Content.text = _text.Substring(0, _currentIndex);
            yield return new WaitForSeconds(PrintRate);
        }
        _printingFinished = true;
        Content.text = _text;
        Continue.enabled = true;
    }

//    private void Advance()
//    {
//        // Check if finished printing
//        if (_currentIndex >= _text.Length)
//        {
//            Content.text = _text.Substring(0, _currentIndex);
//            _printingFinished = true;
//            Continue.enabled = true;
//
//            CancelInvoke("Advance");
//            return;
//        }
//
//        _currentIndex++;
//        Content.text = _text.Substring(0, _currentIndex);
//    }

    private void _jumpToFinish()
    {
        // Finish printing
        _currentIndex = _text.Length;
    }

    public void OnClick()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (!_printingFinished)
        {
            _jumpToFinish();
        }
    }

    public override void Kill()
    {
        StopAllCoroutines();
        Continue.enabled = false;
        Content.enabled = false;
        CharacterPortrait.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Continue.enabled = false;
    }
}