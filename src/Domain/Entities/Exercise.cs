using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public partial class Exercise
{
    public int Idexercise { get; set; }

    public string Name { get; set; } = null!;

    public string Musclegroup { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Routinesexercise> Routinesexercises { get; set; } = new List<Routinesexercise>();
}
