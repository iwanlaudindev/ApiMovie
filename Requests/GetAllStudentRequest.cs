using ApiMovie.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovie.Requests;

public class GetAllStudentRequest
{
    public static readonly string[] OrderAbleColumn = [
        nameof(StudentResponse.Name),
        nameof(StudentResponse.Email),
        nameof(StudentResponse.Age)
    ];

    [FromQuery(Name = "s")] public string? Search { get; set; }
    [FromQuery] public string? OrderBy { get; set; }
    [FromQuery] public string? OrderType { get; set; }
    [FromQuery] public int Page { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}
