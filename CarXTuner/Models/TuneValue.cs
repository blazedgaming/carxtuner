namespace CarXTuner.Models;

public class TuneValue
{
    public string Key { get; set; } = string.Empty;
    public double Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? Step { get; set; }
    public string Help { get; set; } = string.Empty;
}
