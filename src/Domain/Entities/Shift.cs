using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Shift
{
    public string Dateday { get; set; } = null!;

    public TimeOnly Hour { get; set; }

    public int Idlocation { get; set; }

    public string? Dnitrainer { get; set; }

    public int? Peoplelimit { get; set; }

    public int Totaldays { get; set; }

    public virtual Trainer? DnitrainerNavigation { get; set; }

    public virtual Location IdlocationNavigation { get; set; } = null!;

    public virtual ICollection<Client> Dniclients { get; set; } = new List<Client>();
}
