using Microsoft.AspNetCore.Mvc;

namespace ApiMovie.Requests;

public class StudentDetailRequest
{
    [FromRoute(Name = "id")] public int Id { get; set; }
    [FromQuery(Name ="hobbyId")] public int HobbyId { get; set; }
}
