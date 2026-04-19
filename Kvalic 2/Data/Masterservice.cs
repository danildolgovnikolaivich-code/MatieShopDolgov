using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Masterservice
{
    public int Id { get; set; }

    public int Masterid { get; set; }

    public int Serviceid { get; set; }

    public virtual Master Master { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
