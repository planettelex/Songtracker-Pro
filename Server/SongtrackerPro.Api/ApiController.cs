using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Api
{
    public class ApiController : ControllerBase
    {
        protected JsonSerializerOptions SerializerOptions =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = !ApplicationSettings.MinifyJson
            };
    }
}
