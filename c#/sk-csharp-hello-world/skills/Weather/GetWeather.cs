using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skills.Weather
{
    public class GetWeather
    {
        [SKFunction, Description("Get weather information based on the location")]
        public string? GetWeatherAsync(SKContext? context)
        {
            var cityname = context["input"];
            return null;
        }
    }
}
