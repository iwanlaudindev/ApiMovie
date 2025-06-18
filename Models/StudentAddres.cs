using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovie.Models;

public class StudentAddres
{
    [Key]
    public int Id { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    // public int NewPending { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
}
