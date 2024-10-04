using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.TempModels;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Type { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    [JsonIgnore]
    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}
