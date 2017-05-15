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

    protected override void _init()
    {
        _text = Content.text;
        Content.text = "";
        base._init();
    }

    protected override void _reinit()
    {
        StopAllCoroutines();
        _currentIndex = 0;
        Continue.enabled = false;
        Content.enabled = false;
        CharacterPortrait.enabled = false;
    }

    protected override IEnumerator _run()
    {
        CharacterPortrait.enabled = true;
        Content.enabled = true;
        yield return StartCoroutine(_print());
        // This wait here is to prevent the immediate activation of the last WaitUntil when the player clicks to skip animation.
        yield return new WaitForEndOfFrame();
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

    private void OnDisable()
    {
        Continue.enabled = false;
    }
}