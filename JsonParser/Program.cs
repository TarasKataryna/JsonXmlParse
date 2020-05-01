using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parser;

namespace JsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = "{ name:John, age:30, car:{name:MyCar, carAge:90}, city:Lviv, home:{house:2, flat:15}, array:[{item1n:name1, item1v:value1},{item2n:name2, item2v:value2}]}";
            JsonToken obj = new JsonToken(json);
            string result = obj.ParseToJson();

            Console.WriteLine(obj["array"].Array[1]["item2v"].Value);
        }
    }
}
