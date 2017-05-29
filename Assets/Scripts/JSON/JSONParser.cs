using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

public class JSONParser
{
    private int _lineIndex;
    private int _charIndex;

    public class SyntaxError : Exception
    {
        public SyntaxError(JSONParser parser) : base(parser._lineIndex == parser._input.Length
            ? "Unexpected end of input"
            : "Unexpected character at line " + parser._lineIndex + " character " + parser._charIndex + ": " +
              parser._input[parser._lineIndex][parser._charIndex])
        {
        }
    }

    private readonly string[] _input;
    private object _result;
    private readonly IEnumerator<char> _charEnumerator;

    private IEnumerator<char> _constructCharIterator()
    {
        for (_lineIndex = 0; _lineIndex < _input.Length; _lineIndex++)
        {
            string line = _input[_lineIndex];
            for (_charIndex = 0; _charIndex < line.Length; _charIndex++)
            {
                yield return line[_charIndex];
            }
        }
    }

    private JSONParser() : this("")
    {
    }

    private JSONParser(string input) : this(new[] {input})
    {
    }

    private JSONParser(string[] input)
    {
        _input = input;
        _charEnumerator = _constructCharIterator();
    }

    private void _parse()
    {
        _charEnumerator.MoveNext();
        _skipWhitespace();
        if (_charEnumerator.Current != '{' && _charEnumerator.Current != '[') throw new SyntaxError(this);
        _result = _identifyAndParseObject();
    }

    private object _identifyAndParseObject()
    {
        switch (_charEnumerator.Current)
        {
            case '{':
                return _parseDict();
            case '[':
                return _parseList();
            case '"':
                return _parseString();
            case 't':
            case 'f':
                return _parseBoolean();
            case 'n':
                return _parseNull();
            default:
                if (Char.IsDigit(_charEnumerator.Current))
                {
                    return _parseNumber();
                }
                throw new SyntaxError(this);
        }
    }

    private Dictionary<string, object> _parseDict()
    {
        if (_charEnumerator.Current != '{') throw new SyntaxError(this);

        Dictionary<string, object> result = new Dictionary<string, object>();

        // Skip opening bracket
        _charEnumerator.MoveNext();
        _skipWhitespace();

        // While not at closing bracket
        while (_charEnumerator.Current != '}')
        {
            // Parse key
            string key = _parseString();
            _skipWhitespace();
            if (_charEnumerator.Current != ':') throw new SyntaxError(this);
            _charEnumerator.MoveNext();

            // Parse value
            _skipWhitespace();
            result[key] = _identifyAndParseObject();
            _skipWhitespace();

            // Get ready to parse the next object, or let the while loop detect the closing bracket.
            if (_charEnumerator.Current == ',')
            {
                _charEnumerator.MoveNext();
                _skipWhitespace();
            }
        }

        return result;
    }

    private List<object> _parseList()
    {
        if (_charEnumerator.Current != '[') throw new SyntaxError(this);

        List<object> result = new List<object>();

        // Skip opening bracket
        _charEnumerator.MoveNext();
        _skipWhitespace();

        // While not at closing bracket
        while (_charEnumerator.Current != ']')
        {
            // Parse object
            result.Add(_identifyAndParseObject());
            _skipWhitespace();

            // Get ready to parse the next object, or let the while loop detect the closing bracket.
            if (_charEnumerator.Current == ',')
            {
                _charEnumerator.MoveNext();
                _skipWhitespace();
            }
        }

        return result;
    }

    private void _skipWhitespace()
    {
        while (Char.IsWhiteSpace(_charEnumerator.Current))
        {
            _charEnumerator.MoveNext();
        }
    }

    private string _parseString()
    {
        if (_charEnumerator.Current != '"') throw new SyntaxError(this);
        _charEnumerator.MoveNext();

        StringBuilder builder = new StringBuilder();
        bool escaped = false;
        // Continue building until closing quote
        while (escaped || _charEnumerator.Current != '"')
        {
            if (escaped)
            {
                escaped = false;
                // Match all escaped characters to C# characters
                switch (_charEnumerator.Current)
                {
                    case 'b':
                        builder.Append('\b');
                        break;
                    case 'f':
                        builder.Append('\f');
                        break;
                    case 'n':
                        builder.Append('\n');
                        break;
                    case 'r':
                        builder.Append('\r');
                        break;
                    case 't':
                        builder.Append('\t');
                        break;
                    case '"':
                        builder.Append('"');
                        break;
                    case '\\':
                        builder.Append('\\');
                        break;
                    default:
                        throw new SyntaxError(this);
                }
            }
            else
            {
                // Not escaped character
                if (_charEnumerator.Current == '\\')
                {
                    escaped = true;
                }
                else
                {
                    builder.Append(_charEnumerator.Current);
                }
            }

            _charEnumerator.MoveNext();
        }

        _charEnumerator.MoveNext();
        return builder.ToString();
    }

    private object _parseNumber()
    {
        StringBuilder builder = new StringBuilder();
        bool isFloat = false;

        // Scan to the end of input
        while (Char.IsDigit(_charEnumerator.Current) || _charEnumerator.Current == '.')
        {
            if (_charEnumerator.Current == '.')
            {
                if (isFloat)
                {
                    // Should not have two dots
                    throw new SyntaxError(this);
                }
                isFloat = true;
            }

            builder.Append(_charEnumerator.Current);
            _charEnumerator.MoveNext();
        }

        return isFloat ? float.Parse(builder.ToString()) : int.Parse(builder.ToString());
    }

    private bool _parseBoolean()
    {
        if (_charEnumerator.Current == 't')
        {
            _checkExpected("true");
            return true;
        }
        if (_charEnumerator.Current == 'f')
        {
            _checkExpected("false");
            return false;
        }
        throw new SyntaxError(this);
    }

    private object _parseNull()
    {
        _checkExpected("null");
        return null;
    }

    private void _checkExpected(string expected)
    {
        foreach (char c in expected)
        {
            if (_charEnumerator.Current != c)
            {
                throw new SyntaxError(this);
            }
            _charEnumerator.MoveNext();
        }
    }

    private class Tester
    {
        private static string DictionaryPrint(Dictionary<string, object> dictionary, string space = "")
        {
            string output = "";
            foreach (KeyValuePair<string, object> entry in dictionary)
            {
                output += space + entry.Key + ": ";
                if (entry.Value is Dictionary<string, object>)
                    output += "\n" + DictionaryPrint((Dictionary<string, object>) entry.Value, space + "  ");
                else if (entry.Value is List<object>)
                    output += "\n" + ListPrint((List<object>) entry.Value, space + "  ");
                else
                    output += entry.Value + "\n";
            }
            return output;
        }

        private static string ListPrint(List<object> list, string space = "")
        {
            string output = "";
            foreach (object entry in list)
            {
                if (entry is List<object>)
                    output += ListPrint((List<object>) entry, space + "  ");
                else if (entry is Dictionary<string, object>)
                    output += DictionaryPrint((Dictionary<string, object>) entry, space + "  ");
                else
                    output += entry + "\n";
            }
            return output;
        }

        [Test]
        public static void ParsingTest()
        {
            JSONParser parser = new JSONParser(new[]
            {
                "{",
                "\"key1\": \"value1\"",
                "\"key2\": \"value2\"",
                "}"
            });
            parser._parse();

            Assert.IsAssignableFrom<Dictionary<string, object>>(parser._result);
            Console.WriteLine("Hello world!");
            Console.WriteLine(DictionaryPrint((Dictionary<string, object>) parser._result));

            // Using my json config file from another project
            string input = File.OpenText(Directory.GetCurrentDirectory() + "\\config.json").ReadToEnd();
            Console.WriteLine("Input: " + input);

            parser = new JSONParser(input);
            parser._parse();

            Assert.IsAssignableFrom<Dictionary<string, object>>(parser._result);
            Console.WriteLine(DictionaryPrint((Dictionary<string, object>) parser._result));
        }
    }
}