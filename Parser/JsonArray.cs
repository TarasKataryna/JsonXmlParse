using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class JsonArray
    {
        public bool IsArray { get; set; }

        public List<JsonToken> Tokens { get; set; }

        public string JsonValue { get; set; }

        public JsonToken this[int i]
        {
            get
            {
                return Tokens[i];
            }
        }

        public JsonArray(string json)
        {
            Tokens = new List<JsonToken>();
            JsonValue = json;
            Parse();
        }

        public JsonArray Parse()
        {
            JsonValue = JsonValue.Replace('\"', ' ');
            JsonValue = JsonValue.Replace('\'', ' ');
            JsonValue = JsonValue.Replace(" ", string.Empty);
            ParseToken();

            return this;
        }

        private void ParseToken()
        {
            if (JsonValue[0] == '[')
            {
                JsonValue = JsonValue.Remove(0, 1);
                JsonValue = JsonValue.Remove(JsonValue.Length - 1, 1);

                Stack<int> stack = new Stack<int>();

                for (int i = 0; i < JsonValue.Length; ++i)
                {
                    var startIndex = 0;

                    if (JsonValue[i] == '{')
                    {
                        stack.Push(i);
                    }
                    if (JsonValue[i] == '}')
                    {
                        startIndex = stack.Pop();
                    }
                    if (JsonValue[i] == ',')
                    {
                        continue;
                    }
                    if (stack.Count == 0)
                    {
                        Tokens.Add(new JsonToken(JsonValue.Substring(startIndex, i - startIndex + 1)));
                    }
                }
            }
        }

        public string ParseToJson()
        {
            var result = "[";
            for(int i = 0; i < Tokens.Count; ++i)
            {
                result += Tokens[i].ParseToJson();
            }
            result += "]";
            return result;
        }
    }
}
