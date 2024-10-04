using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.TempModels;

public partial class Trainer
{
    public string Dnitrainer { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;
    [JsonIgnore]
    public int Iduser { get; set; }

    public int Isactive { get; set; }
    [JsonIgnore]
    public virtual User IduserNavigation { get; set; } = null!;

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
