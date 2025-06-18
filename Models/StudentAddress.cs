using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovie.Models;

public class StudentAddress
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Address { get; set; }

    // Foreign key to Student
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public Student Student { get; set; }
}
