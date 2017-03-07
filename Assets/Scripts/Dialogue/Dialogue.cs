using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : AbstractStoryElement
{
    public Image CharacterPortrait;
    public Text Content;

    public const float PrintRate = 0.02F;
    private String _text;
    private int _currentIndex = 0;
    private bool _printingFinished;

    private void Start()
    {
        // For future reference: may read from files here.
        _text = Content.text;
        Content.text = "";
    }

    public override void OnActivate()
    {
        CharacterPortrait.enabled = true;
        Content.enabled = true;
        StartPrint();
    }

    private void StartPrint()
    {
        _printingFinished = false;
        InvokeRepeating("Advance", 0, PrintRate);
    }

    private void Advance()
    {
        // Check if finished printing
        if (_currentIndex >= _text.Length)
        {
            Content.text = _text.Substring(0, _currentIndex);
            _printingFinished = true;
            CancelInvoke("Advance");
            return;
        }

        _currentIndex++;
        Content.text = _text.Substring(0, _currentIndex);
    }

    private void JumpToFinish()
    {
        // Finish printing
        _currentIndex = _text.Length;
    }

    public override void OnClick()
    {
        if (!_printingFinished)
        {
            JumpToFinish();
        }
        else
        {
            OnFinished();
        }
    }

    public override void OnDeactivate()
    {
        CharacterPortrait.enabled = false;
        Content.enabled = false;
    }
}