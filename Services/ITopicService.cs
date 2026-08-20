using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITopicService
{
Task<List<Topic>> GetTopicsAsync();
Task<Topic?>GetTopicByIdAsync(string id);
}