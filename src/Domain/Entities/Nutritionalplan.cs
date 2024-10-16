using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Nutritionalplan
{
    public int Idnutritionalplan { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Breakfast { get; set; }

    public string? Luch { get; set; }

    public string? Dinner { get; set; }

    public string? Brunch { get; set; }

    public string? Snack { get; set; }

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Trainer DnitrainerNavigation { get; set; } = null!;
}
