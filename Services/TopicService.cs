using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;


public class TopicService: ITopicService
{
    private readonly IWebHostEnvironment _env;
    private readonly JsonSerializerOptions _opts = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly string _path;

    public TopicService (IWebHostEnvironment env)
    {
        _env = env;
        _path = Path.Combine(_env.ContentRootPath, "Data", "topics.json");
    }



    public async Task<List<Topic>> GetTopicsAsync()
    {
        if (!File.Exists(_path))
        {
            return new List<Topic>();
        }

        await using var fs = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<List<Topic>>(fs, _opts) ?? new List<Topic>();
    }
    public async Task<Topic?> GetTopicByIdAsync(string id) =>
    (await GetTopicsAsync()).FirstOrDefault(t => t.Id == id);
}
