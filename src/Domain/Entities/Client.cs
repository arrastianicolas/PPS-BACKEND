
using System.Text.Json.Serialization;

namespace Infrastructure.TempModels;

public partial class Client
{
    public string Dniclient { get; set; } = null!;

    public string Typememberships { get; set; } = null!;

    public DateTime Startdatemembership { get; set; }

    public DateOnly Birthdate { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;
    [JsonIgnore]
    public int Iduser { get; set; }
    public int Isactive { get; set; }

    public string Genre { get; set; } = null!;
    [JsonIgnore]
    public virtual User IduserNavigation { get; set; } = null!;

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

    public virtual ICollection<Shiftsclient> Shiftsclients { get; set; } = new List<Shiftsclient>();
    [JsonIgnore]
    public virtual Membership TypemembershipsNavigation { get; set; } = null!;
}
