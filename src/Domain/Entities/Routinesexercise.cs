using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Routinesexercise
{
    public int Correlativenumber { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public int Idexercise { get; set; }

    public DateOnly Breaktime { get; set; }

    public int Serie { get; set; }

    public virtual Exercise IdexerciseNavigation { get; set; } = null!;
}
