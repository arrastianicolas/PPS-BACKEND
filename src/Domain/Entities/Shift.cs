using System;
using System.Collections.Generic;

namespace Infrastructure.TempModels;

public partial class Shift
{
    public int Idshift { get; set; }

    public DateTime Date { get; set; }

    public int Idlocation { get; set; }

    public string? Dnitrainer { get; set; }

    public int Peoplelimit { get; set; }

    public virtual Trainer? DnitrainerNavigation { get; set; }

    public virtual Location IdlocationNavigation { get; set; } = null!;

    public virtual ICollection<Shiftsclient> Shiftsclients { get; set; } = new List<Shiftsclient>();
}
