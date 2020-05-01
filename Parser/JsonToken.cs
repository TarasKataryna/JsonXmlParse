using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class JsonToken
    {
        public bool IsTerminal { get; set; }

        public string Value { get; set; }

        public JsonArray Array { get; set; }

        public Dictionary<string, JsonToken> Properties { get; set; }

        private string JsonValue;

        public JsonToken this[string i]
        {
            get
            {
                return Properties[i];
            }
            set
            {
                Properties[i] = value;
            }
        }

        public JsonToken()
        {
            Properties = new Dictionary<string, JsonToken>();
        }

        public JsonToken(string json)
        {
            Properties = new Dictionary<string, JsonToken>();
            JsonValue = json;
            Parse();
        }

        public JsonToken Parse()
        {
            JsonValue = JsonValue.Replace('\"', ' ');
            JsonValue = JsonValue.Replace('\'', ' ');
            JsonValue = JsonValue.Replace(" ", string.Empty);
            ParseTokenObject(this);

            return this;
        }

        private JsonToken ParseTokenObject(JsonToken token)
        {
            if (JsonValue[0] == '{')
            {
                JsonValue = JsonValue.Remove(0, 1);

                while (JsonValue.Length != 0 && JsonValue[0] != '}')
                {
                    var newToken = new JsonToken();
                    if (JsonValue[0] == ',')
                    {
                        JsonValue = JsonValue.Remove(0, 1);
                        return token;
                    }
                    var index = JsonValue.IndexOf(':');
                    var propertyName = JsonValue.Substring(0, index);
                    JsonValue = JsonValue.Remove(0, index + 1);

                    token.Properties.Add(propertyName, ParseTokenObject(newToken));
                }

                return token;
            }
            else
            {
                if (JsonValue[0] == '[')
                {
                    Stack<int> stack = new Stack<int>();
                    for (int i = 0; i < JsonValue.Length; ++i)
                    {
                        var startIndex = 0;

                        if (JsonValue[i] == '[')
                        {
                            stack.Push(i);
                        }
                        if (JsonValue[i] == ']')
                        {
                            startIndex = stack.Pop();
                        }
                        if (stack.Count == 0)
                        {
                            token.Array = new JsonArray(JsonValue.Substring(0, i + 1));
                            JsonValue = JsonValue.Remove(0, i + 2);
                            break;
                        }
                    }
                    return token;
                }
                else
                {
                    var firstIndex = JsonValue.IndexOf(',');
                    var lastIndex = JsonValue.IndexOf('}');

                    var indexToDelete = firstIndex != -1
                        ? firstIndex < lastIndex
                            ? firstIndex
                            : lastIndex
                        : lastIndex;

                    var valueToInsert = JsonValue.Substring(0, indexToDelete);
                    JsonValue = JsonValue.Remove(0, indexToDelete + 1);

                    return new JsonToken
                    {
                        IsTerminal = true,
                        Properties = null,
                        Value = valueToInsert
                    };
                }
            }
        }

        public string ParseToJson()
        {
            return ParseTokenToJson(this);
        }

        private string ParseTokenToJson(JsonToken token)
        {
            if (token.IsTerminal == false)
            {
                if (token.Array != null)
                    return token.Array.ParseToJson();
                else
                {
                    var res = "{";
                    for (int i = 0; i < token.Properties.Count; ++i)
                    {
                        var key = token.Properties.Keys.ElementAt(i).ToString();
                        res += key + ":";


                        res += ParseTokenToJson(token.Properties[key]);

                        if (i < token.Properties.Count - 1)
                            res += ",";
                        else
                            res += "}";
                    }

                    return res;
                }
            }
            else
            {
                return token.Value;
            }
        }
    }
}
