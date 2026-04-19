using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Qualificationrequest
{
    public int Requestid { get; set; }

    public int Masterid { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Master Master { get; set; } = null!;
}
