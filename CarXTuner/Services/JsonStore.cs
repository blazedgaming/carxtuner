using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CarXTuner.Models;

namespace CarXTuner.Services;

public static class JsonStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private static string BaseDirectory => AppContext.BaseDirectory;
    private static string DataDirectory => Path.Combine(BaseDirectory, "Data");
    private static string PresetsDirectory => Path.Combine(DataDirectory, "Presets");
    private static string ExportsDirectory => Path.Combine(BaseDirectory, "Exports");
    private static string CarsFilePath => Path.Combine(DataDirectory, "Cars.json");

    public static void EnsureDataFoldersExist()
    {
        Directory.CreateDirectory(DataDirectory);
        Directory.CreateDirectory(PresetsDirectory);
        Directory.CreateDirectory(ExportsDirectory);
    }

    public static void SaveCars(List<CarInfo> cars)
    {
        EnsureDataFoldersExist();
        var payload = JsonSerializer.Serialize(cars, JsonOptions);
        File.WriteAllText(CarsFilePath, payload);
    }

    public static List<CarInfo> LoadCars()
    {
        try
        {
            if (!File.Exists(CarsFilePath))
            {
                return new List<CarInfo>();
            }

            var json = File.ReadAllText(CarsFilePath);
            return JsonSerializer.Deserialize<List<CarInfo>>(json, JsonOptions) ?? new List<CarInfo>();
        }
        catch
        {
            return new List<CarInfo>();
        }
    }

    public static void SavePreset(TunePreset preset)
    {
        if (preset is null)
        {
            throw new ArgumentNullException(nameof(preset));
        }

        EnsureDataFoldersExist();

        var carFolderName = SanitizeFileName(preset.CarName);
        var carFolder = Path.Combine(PresetsDirectory, carFolderName);
        Directory.CreateDirectory(carFolder);

        preset.UpdatedUtc = DateTime.UtcNow;

        var fileName = $"{SanitizeFileName(preset.DriftStyle)}_{SanitizeFileName(preset.Version)}.json";
        var filePath = Path.Combine(carFolder, fileName);

        var payload = JsonSerializer.Serialize(preset, JsonOptions);
        File.WriteAllText(filePath, payload);
    }

    public static TunePreset LoadPreset(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<TunePreset>(json, JsonOptions) ?? new TunePreset();
        }
        catch
        {
            return new TunePreset();
        }
    }

    public static List<string> ListPresetsForCar(string carName)
    {
        try
        {
            var carFolder = Path.Combine(PresetsDirectory, SanitizeFileName(carName));
            if (!Directory.Exists(carFolder))
            {
                return new List<string>();
            }

            return Directory.GetFiles(carFolder, "*.json", SearchOption.TopDirectoryOnly).ToList();
        }
        catch
        {
            return new List<string>();
        }
    }

    public static string SanitizeFileName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = value;

        foreach (var invalidChar in invalidChars)
        {
            sanitized = sanitized.Replace(invalidChar, '_');
        }

        return sanitized.Trim();
    }
}
