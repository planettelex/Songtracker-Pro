using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.ViewModels;
using SongtrackerPro.Tasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Api
{
    public class ApiControllerBase : ControllerBase
    {
        protected JsonSerializerOptions SerializerOptions =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = !ApplicationSettings.Api.MinifyJson
            };

        protected string JsonSerialize<T>(TaskResult<T> taskResults)
        {
            return taskResults.Success ?
                JsonSerializer.Serialize(taskResults.Data, SerializerOptions) : 
                JsonSerializer.Serialize(new ServerError(taskResults.Exception), SerializerOptions);
        }
    }
}
