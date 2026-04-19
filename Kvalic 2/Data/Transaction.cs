using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public int Userid { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Transactiondate { get; set; }

    public string? Type { get; set; }

    public virtual User User { get; set; } = null!;
}
