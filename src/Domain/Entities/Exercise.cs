using System;
using System.Collections.Generic;


namespace Domain.Entities;

public partial class Exercise
{
    public int Idexercise { get; set; }

    public string Name { get; set; } = null!;

    public string Musclegroup { get; set; } = null!;

    public virtual ICollection<Routinesexercise> Routinesexercises { get; set; } = new List<Routinesexercise>();
}
