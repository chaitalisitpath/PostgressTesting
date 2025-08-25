using System;
using System.Collections.Generic;

namespace PostgressTesting.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? EnrollmentDate { get; set; }
}
