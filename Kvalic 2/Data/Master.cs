using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Master
{
    public int Masterid { get; set; }

    public int Userid { get; set; }

    public int? Qualificationlevel { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Masterservice> Masterservices { get; set; } = new List<Masterservice>();

    public virtual ICollection<Qualificationrequest> Qualificationrequests { get; set; } = new List<Qualificationrequest>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
