using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Fushigi.param
{
    public class ParamLoader
    {
        public static void Load()
        {
            ParamHolder areaParams = [];
            mParams = [];

            string areaParamText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "AreaParam.json"));
            var jsonNode = JsonNode.Parse(areaParamText);
            if (jsonNode == null)
                return;

            var nodes = jsonNode.AsObject();

            foreach (KeyValuePair<string, JsonNode?> obj in nodes)
            {
                // todo -- support other things
                if (obj.Value is JsonValue)
                    areaParams.Add(obj.Key, (string)obj.Value);
            }

            mParams.Add("AreaParam", areaParams);
        }

        public static ParamHolder GetHolder(string name) => mParams[name];

        static Dictionary<string, ParamHolder> mParams = [];
    }

    public class ParamHolder : Dictionary<string, string>
    {

    }
}
