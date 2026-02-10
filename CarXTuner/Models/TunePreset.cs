using System;
using System.Collections.Generic;

namespace CarXTuner.Models;

public class TunePreset
{
    public string CarName { get; set; } = string.Empty;
    public string DriftStyle { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
    public Dictionary<string, List<TuneValue>> Sections { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}
