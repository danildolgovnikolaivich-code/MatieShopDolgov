using System;
using System.Collections.Generic;

namespace Kvalic_2.Data;

public partial class Collection
{
    public int Collectionid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Servicecollection> Servicecollections { get; set; } = new List<Servicecollection>();
}
