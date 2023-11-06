using Microsoft.SemanticKernel.Orchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.SemanticKernel.SkillDefinition;
using System.ComponentModel;

namespace skills.Json
{
    public class ExtractJson
    {
        [SKFunction, Description("Extract information from the JSON")]
        public SKContext? ExtractInformation(SKContext? context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            JObject jsonObject = JObject.Parse(context["input"]);

            if (jsonObject.TryGetValue("cityname", out var city))
            {
                context["city"] = city.ToString();
            }


            if (jsonObject.TryGetValue("history", out var history))
            {
                context["history"] = history.ToString();
            }
            return context;
        }
    }
}
