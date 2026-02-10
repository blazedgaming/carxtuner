using System.Collections.Generic;

namespace CarXTuner.Models;

public class CarInfo
{
    public string Name { get; set; } = string.Empty;
    public string Drive { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public List<string> Tags { get; set; } = new();
}
