﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RangHo.DialogueScript;

public class Dialogue : AbstractGameplayStage
{
    

    public Image CharacterPortrait;
    public Text Content;
    
    [NonSerialized]
    public Interpreter DialogueInterpreter;

    public const float PrintRate = 0.02F;
    private string _text;
    private int _currentIndex = 0;
    private bool _printingFinished;

    public Text Continue;

    protected override void Awake()
    {
        base.Awake();
        // For future reference: may read from files here.
        _text = Content.text;
        Content.text = "";
    }

    public override void Begin()
    {
        base.Begin();
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
            Continue.enabled = true;

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

    public void OnClick()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (!_printingFinished)
        {
            JumpToFinish();
        }
        else
        {
            End();
        }
    }

    public override void End()
    {
        CharacterPortrait.enabled = false;
        Content.enabled = false;
        Continue.enabled = false;
        base.End();
    }
}