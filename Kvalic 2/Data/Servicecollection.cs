using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Servicecollection
{
    public int Id { get; set; }

    public int Serviceid { get; set; }

    public int Collectionid { get; set; }

    public virtual Collection Collection { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
