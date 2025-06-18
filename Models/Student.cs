using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovie.Models;

public class Student
{
    [Key] // Menandakan ini adalah Primary Key
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama harus diisi.")]
    [StringLength(100, ErrorMessage = "Nama tidak boleh lebih dari 100 karakter.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email harus diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    public string Email { get; set; }

    [Range(18, 60, ErrorMessage = "Usia harus antara 18 dan 60.")]
    public int Age { get; set; }

    // Relasi ke StudentAddres
    public ICollection<StudentAddres> StudentAddreses { get; set; } = [];

    // Relasi ke Hobby
    public ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
    
    [NotMapped]
    public List<int> SelectedHobbyIds { get; set; } = [];
}
