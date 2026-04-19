using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Review
{
    public int Reviewid { get; set; }

    public int Userid { get; set; }

    public int? Masterid { get; set; }

    public int? Serviceid { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Master? Master { get; set; }

    public virtual Service? Service { get; set; }

    public virtual User User { get; set; } = null!;
}
