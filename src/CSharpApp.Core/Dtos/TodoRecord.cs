namespace CSharpApp.Core.Dtos;

/// <summary>
/// Record template for Todo service.
/// </summary>
/// <param name="UserId">UserId of record.</param>
/// <param name="Id">Id of record. Id is automatically incremented and should never be set/changed.</param>
/// <param name="Title">Title of record.</param>
/// <param name="Completed">If todo has been completed then mark as true.</param>
public record TodoRecord(
    [property: JsonProperty("userId")] int UserId,
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("completed")] bool Completed
);