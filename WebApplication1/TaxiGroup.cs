using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WebApplication1;

public partial class TaxiGroup
{
    [Key]
    public int Id { get; set; }
    public int TaxiDepotId { get; set; }
    public int CarId { get; set; }
    public virtual Car Car { get; set; } = null!;

    public int Quantity { get; set; }
    //[JsonIgnore] 
    public virtual TaxiDepot TaxiDepot { get; set; } = null!;
}
