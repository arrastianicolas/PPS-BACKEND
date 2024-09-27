using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}
