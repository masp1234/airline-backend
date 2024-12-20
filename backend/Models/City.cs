﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace backend.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int StateId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    public virtual State State { get; set; } = null!;
}
