using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public partial class Car
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int SitCounter { get; set; }
}
