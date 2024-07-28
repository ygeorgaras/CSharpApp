using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Interfaces;

/// <summary>
/// Holds the interface implentation used by the PostService.
/// </summary>
public interface IPostService
{
    Task<PostRecord?> GetPostById(int id);
    Task<ReadOnlyCollection<PostRecord>> GetAllPosts();
    Task<PostRecord?> AddPostRecord(int userId, string title, string body);
    Task<HttpStatusCode> DeletePostById(int id);
}