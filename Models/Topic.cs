
using System.Collections.Generic;
public class Topic
{
    public string Id { get; set; } = "";

    public string Title { get; set; } = "";

    public List<WordItem> Words { get; set; } = new List<WordItem>();
}