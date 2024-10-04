using System;
using System.Collections.Generic;

namespace Infrastructure.TempModels;

public partial class Routine
{
    public int Correlativenumber { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public float Weight { get; set; }

    public float Height { get; set; }

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Trainer DnitrainerNavigation { get; set; } = null!;

    public virtual ICollection<Routinesexercise> Routinesexercises { get; set; } = new List<Routinesexercise>();
}
