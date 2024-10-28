using System;
using System.Collections.Generic;


namespace Domain.Entities;
public partial class Routinesexercise
{
    public int Idroutine { get; set; }

    public int Idexercise { get; set; }

    public TimeOnly Breaktime { get; set; }

    public int Series { get; set; }

    public string Day { get; set; } = null!;

    public virtual Exercise IdexerciseNavigation { get; set; } = null!;

    public virtual Routine IdroutineNavigation { get; set; } = null!;
}
