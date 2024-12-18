﻿using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Nutritionalplan
{
    public int Idnutritionalplan { get; set; }

    public string Dniclient { get; set; } = null!;

    public string Dnitrainer { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Breakfast { get; set; }

    public string? Lunch { get; set; }

    public string? Dinner { get; set; }

    public string? Brunch { get; set; }

    public string? Snack { get; set; }

    public string Status { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public string Height { get; set; } = null!;

    public virtual Client DniclientNavigation { get; set; } = null!;

    public virtual Trainer DnitrainerNavigation { get; set; } = null!;
}
