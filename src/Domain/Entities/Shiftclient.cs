using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Shiftclient
{
    public int Idshift { get; set; }

    public string Dniclient { get; set; } = null!;

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Shift IdshiftNavigation { get; set; } = null!;
}
