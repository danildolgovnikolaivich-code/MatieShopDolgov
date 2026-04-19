using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Service
{
    public int Serviceid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime? Lastupdated { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Masterservice> Masterservices { get; set; } = new List<Masterservice>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Servicecollection> Servicecollections { get; set; } = new List<Servicecollection>();
}
