using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Domain.Entities;

public partial class Shift
{
    public int Idshift { get; set; }

    public string Dateday { get; set; } = null!;

    public TimeOnly Hour { get; set; }

    public int Idlocation { get; set; }

    public string? Dnitrainer { get; set; }

    public int? Peoplelimit { get; set; }

    public int? Actualpeople { get; set; }

    public int IsActive { get; set; }

    public virtual Trainer? DnitrainerNavigation { get; set; }

    public virtual Location IdlocationNavigation { get; set; } = null!;

    public virtual Shiftclient? Shiftclient { get; set; }
}
