using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Routine
{
    public int Idroutine { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public string Height { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Days { get; set; }

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Trainer DnitrainerNavigation { get; set; } = null!;

    public virtual ICollection<Routinesexercise> Routinesexercises { get; set; } = new List<Routinesexercise>();
}
