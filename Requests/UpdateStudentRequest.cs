using System;

namespace ApiMovie.Requests;

public class UpdateStudentRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
}
