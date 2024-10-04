using System;
using System.Collections.Generic;

namespace Infrastructure.TempModels;

public partial class Location
{
    public int Idlocation { get; set; }

    public string Adress { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Isactive { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
