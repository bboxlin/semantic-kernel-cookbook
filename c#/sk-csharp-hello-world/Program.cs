using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using skills.Json;

var kernelSettings = KernelSettings.LoadSettings();

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning)
        .AddConsole()
        .AddDebug();
});

IKernel kernel = new KernelBuilder()
    .WithLogger(loggerFactory.CreateLogger<IKernel>())
    .WithCompletionService(kernelSettings)
    .Build();

if (kernelSettings.EndpointType == EndpointTypes.TextCompletion)
{
    // note: using skills from the repo
    var skillsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "skills");
    var skill = kernel.ImportSemanticSkillFromDirectory(skillsDirectory, "CitySkill");

    kernel.ImportSkill(new ExtractJson(), nameof(ExtractJson));
    var jsonSkillFunc = kernel.Skills.GetFunction(nameof(ExtractJson), "ExtractInformation");

    var context = new ContextVariables();
    context.Set("input", "I am plan to go Austin tomorrow");

    var result = await kernel.RunAsync(context, skill["history"], jsonSkillFunc);
    Console.WriteLine(result["input"]);
    Console.WriteLine(result["city"]);
    Console.WriteLine(result["history"]);
}
//-----------------------------//
else if (kernelSettings.EndpointType == EndpointTypes.ChatCompletion)
{
    var chatCompletionService = kernel.GetService<IChatCompletion>();

    var chat = chatCompletionService.CreateNewChat("You are an AI assistant that helps people find information.");
    chat.AddMessage(AuthorRole.User, "Hi, what information can you provide for me?");

    string response = await chatCompletionService.GenerateMessageAsync(chat, new ChatRequestSettings());
    Console.WriteLine(response);
}


