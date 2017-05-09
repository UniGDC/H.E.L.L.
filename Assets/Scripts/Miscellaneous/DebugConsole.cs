using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : SingletonMonoBehaviour<DebugConsole>
{
    public GameObject Console;
    public InputField Command;
    public Text Output;

    private delegate void CommandHandler(string[] arguments);

    private SortedDictionary<string, CommandHandler> _commandHandlers;
    private Dictionary<string, string> _commandHelp;

    private void Awake()
    {
        Instance = this;
        Console.SetActive(false);

        _initCommandHandlers();
    }

    private void _initCommandHandlers()
    {
        _commandHandlers = new SortedDictionary<string, CommandHandler>();
        _commandHelp = new Dictionary<string, string>();

        _commandHandlers["help"] = arguments =>
        {
            if (arguments.Length == 0)
            {
                _consolePrint("Available commands:");
                foreach (string command in _commandHandlers.Keys)
                {
                    _consolePrint(command);
                }

                _consolePrint("Use \"help <command>\" to view usage for a specific command.");
            }
            else
            {
                try
                {
                    string help = _commandHelp[arguments[0]];
                    _consolePrint(help);
                }
                catch (KeyNotFoundException)
                {
                    _consolePrint("Use \"help\" to see a list of available commands.");
                }
                catch (Exception ex)
                {
                    _consolePrint("<color=#ff0000ff>Error:" + ex.Source + "</color>");
                }
            }
        };

        _commandHandlers["load"] = arguments => { LevelManager.Instance.ToLevelScene(int.Parse(arguments[0])); };
        _commandHelp["load"] = "Use \"load <index>\" to load the specified level. 0-indexed.";

        _commandHandlers["mainMenu"] = arguments => { LevelManager.Instance.ToMainMenu(); };
        _commandHelp["mainMenu"] = "Use to return to main menu.";

        _commandHandlers["stage"] = arguments =>
        {
            foreach (Transform child in Spawned.Instance.transform)
            {
                Destroy(child.gameObject);
            }
            LevelController.Instance.SkipToStage(int.Parse(arguments[0]));
        };
        _commandHelp["stage"] = "Use \"stage <index>\" to skip to the indicated stage of the current level. 0-indexed.";

        _commandHandlers["clear"] = arguments => { Output.text = ""; };
        _commandHelp["clear"] = "Use to clear the console messages.";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Console.SetActive(!Console.activeSelf);
            if (Console.activeSelf)
            {
                Command.Select();
                Command.ActivateInputField();
            }
        }
    }

    public void OnCommand()
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            return;
        }

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
            _consolePrint("Use \"help\" to see a list of available commands.");
        }
        catch (Exception ex)
        {
            _consolePrint("<color=#ff0000ff>Error:" + ex.Message + "</color>");
        }

        Command.Select();
        Command.ActivateInputField();
    }

    private void _consolePrint(string text)
    {
        Output.text += "\n";
        Output.text += text;
    }
}