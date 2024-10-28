using System;
using System.Collections.Generic;


namespace Domain.Entities;
public partial class Trainer
{
    public string Dnitrainer { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public int Iduser { get; set; }

    public int Isactive { get; set; }

    public virtual User IduserNavigation { get; set; } = null!;

    public virtual ICollection<Nutritionalplan> Nutritionalplans { get; set; } = new List<Nutritionalplan>();

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
