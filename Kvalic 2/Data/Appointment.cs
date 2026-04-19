using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Appointment
{
    public int Appointmentid { get; set; }

    public int Userid { get; set; }

    public int Masterid { get; set; }

    public int Serviceid { get; set; }

    public DateTime Appointmentdate { get; set; }

    public int? Queuenumber { get; set; }

    public string? Status { get; set; }

    public virtual Master Master { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
