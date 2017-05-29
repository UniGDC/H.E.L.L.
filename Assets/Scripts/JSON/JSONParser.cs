using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


public class JSONParser
{
    // Cache parsed files to improve performance.
    private static Dictionary<string, JSONParser> _parsed = new Dictionary<string, JSONParser>();

    public static JSONParser ParseFile(string filePath)
    {
        if (_parsed.ContainsKey(filePath))
        {
            return _parsed[filePath];
        }

        // Read lines to allow better error messages.
        StreamReader reader = File.OpenText(filePath);
        LinkedList<string> lines = new LinkedList<string>();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            lines.AddLast(line);
        }

        JSONParser parser = new JSONParser(lines.ToArray());
        parser._parse();
        _parsed[filePath] = parser;
        return parser;
    }

    public static JSONParser ParseString(string[] json)
    {
        return new JSONParser(json);
    }

    private int _lineIndex;
    private int _charIndex;

    private readonly string[] _input;
    public object Result { get; private set; }
    private readonly IEnumerator<char> _charEnumerator;

    public class SyntaxError : Exception
    {
        public SyntaxError(JSONParser parser) : base(parser._lineIndex == parser._input.Length
            ? "Unexpected end of input"
            : "Unexpected character at line " + parser._lineIndex + " character " + parser._charIndex + ": " +
              parser._input[parser._lineIndex][parser._charIndex])
        {
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

    private void _parse()
    {
        // Move through the start of the file
        _charEnumerator.MoveNext();
        _skipWhitespace();

        if (_charEnumerator.Current != '{' && _charEnumerator.Current != '[') throw new SyntaxError(this);
        Result = _identifyAndParseObject();
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

        // Move past the closing bracket
        _charEnumerator.MoveNext();

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

        // Move past the closing bracket
        _charEnumerator.MoveNext();

        return result;
    }

    private void _skipWhitespace()
    {
        while (Char.IsWhiteSpace(_charEnumerator.Current) || _checkComment())
        {
            _charEnumerator.MoveNext();
        }
    }

    private bool _checkComment()
    {
        if (_charEnumerator.Current == '/' && _charIndex < _input[_lineIndex].Length - 1 && _input[_lineIndex][_charIndex + 1] == '/')
        {
            // Skip to end of the line
            _charIndex = _input[_lineIndex].Length - 1;
            return true;
        }
        return false;
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
}