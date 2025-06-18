using System;

namespace ApiMovie.Requests;

public class CreateStudentRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
}
