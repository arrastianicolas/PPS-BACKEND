using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Client
{
    public string Dniclient { get; set; } = null!;

    public string Typememberships { get; set; } = null!;

    public DateTime Startdatemembership { get; set; }

    public DateOnly Birthdate { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public int Iduser { get; set; }

    public int Isactive { get; set; }

    public string Genre { get; set; } = null!;

    public DateTime Actualdatemembership { get; set; }

    public virtual User IduserNavigation { get; set; } = null!;

    public virtual ICollection<Nutritionalplan> Nutritionalplans { get; set; } = new List<Nutritionalplan>();

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

    public virtual Membership TypemembershipsNavigation { get; set; } = null!;

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
