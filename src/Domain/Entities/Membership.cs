using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Membership
{
    public string Type { get; set; } = null!;

    public float Price { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
