using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Shiftsclient
{
    public string Dniclient { get; set; } = null!;

    public int Idshift { get; set; }

    public DateOnly Date { get; set; }

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Shift IdshiftNavigation { get; set; } = null!;
}
