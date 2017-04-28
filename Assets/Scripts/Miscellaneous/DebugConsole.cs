using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public GameObject Console;
    public InputField Command;
    public Text Output;

    private delegate void CommandHandler(string[] arguments);

    private SortedDictionary<string, CommandHandler> _commandHandlers;

    private void Awake()
    {
        Console.SetActive(false);
        DontDestroyOnLoad(gameObject);

        _initCommandHandlers();
    }

    private void _initCommandHandlers()
    {
        _commandHandlers = new SortedDictionary<string, CommandHandler>();
        _commandHandlers["help"] = arguments =>
        {
            _consolePrint("Available commands:");
            foreach (string command in _commandHandlers.Keys)
            {
                _consolePrint(command);
            }
        };
        _commandHandlers["load"] = arguments => { LevelManager.Instance.ToLevelScene(int.Parse(arguments[0])); };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Console.SetActive(!Console.activeSelf);
        }
    }

    public void OnCommand()
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            return;
        }

        print("Command!");
        string[] tokens = Command.text.Split(' ');
        Command.text = "";

        try
        {
            CommandHandler handler = _commandHandlers[tokens[0]];
            handler(tokens.Skip(1).ToArray());
        }
        catch (KeyNotFoundException)
        {
            _consolePrint("<color=#ff0000ff>Invalid command.</color>");
        }
        catch (Exception ex)
        {
            _consolePrint("<color=#ff0000ff>Error:" + ex.StackTrace + "</color>");
        }
    }

    private void _consolePrint(string text)
    {
        Output.text += "\n";
        Output.text += text;
    }
}