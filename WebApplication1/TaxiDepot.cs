using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class TaxiDepot
{
    [Key]
    public int Id { get; set; }
    public string? Address { get; set; }
    public virtual ICollection<TaxiGroup> TaxiGroups { get; set; } = new List<TaxiGroup>();
}
