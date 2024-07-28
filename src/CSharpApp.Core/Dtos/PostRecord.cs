namespace CSharpApp.Core.Dtos;

/// <summary>
/// Record template for Posts service.
/// </summary>
/// <param name="UserId">UserId of record.</param>
/// <param name="Id">Id of record. Id is automatically incremented and should never be set/changed.</param>
/// <param name="Title">Title of record</param>
/// <param name="Body">Body of record</param>
public record PostRecord(
    [property: JsonProperty("userId")] int UserId,
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("body")] string Body
);