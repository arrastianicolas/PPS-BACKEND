using System;
using System.Collections.Generic;

namespace Infrastructure.TempModels;

public partial class Routinesexercise
{
    public int Correlativenumber { get; set; }

    public int Idexercise { get; set; }

    public DateOnly Breaktime { get; set; }

    public int Serie { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public virtual Exercise IdexerciseNavigation { get; set; } = null!;

    public virtual Routine Routine { get; set; } = null!;
}
