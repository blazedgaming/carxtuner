using System;
using System.Collections.Generic;
using CarXTuner.Models;
using CarXTuner.Services;

namespace CarXTuner;

internal static class Program
{
    private static void Main()
    {
        JsonStore.EnsureDataFoldersExist();

        var cars = new List<CarInfo>
        {
            new()
            {
                Name = "AE86",
                Drive = "RWD",
                IsFavorite = true,
                Tags = new List<string> { "JDM", "Lightweight" }
            },
            new()
            {
                Name = "Laurel",
                Drive = "RWD",
                IsFavorite = false,
                Tags = new List<string> { "Sedan", "Street" }
            }
        };

        JsonStore.SaveCars(cars);

        var preset = new TunePreset
        {
            CarName = "AE86",
            DriftStyle = "Standard",
            Version = "v1.0",
            Author = "CarXTuner",
            CreatedUtc = DateTime.UtcNow,
            Notes = "Baseline drift tune."
        };

        preset.Sections["Alignment"] = new List<TuneValue>
        {
            new() { Key = "Front Camber", Value = -4.0, Unit = "°", Min = -10, Max = 0, Step = 0.1 },
            new() { Key = "Rear Camber", Value = -1.5, Unit = "°", Min = -10, Max = 0, Step = 0.1 }
        };
        preset.Sections["Suspension"] = new List<TuneValue>
        {
            new() { Key = "Front Spring Rate", Value = 6.5, Unit = "kN/m", Min = 2, Max = 12, Step = 0.1 },
            new() { Key = "Rear Spring Rate", Value = 7.2, Unit = "kN/m", Min = 2, Max = 12, Step = 0.1 }
        };

        JsonStore.SavePreset(preset);

        var presetFiles = JsonStore.ListPresetsForCar("AE86");
        if (presetFiles.Count > 0)
        {
            var loadedPreset = JsonStore.LoadPreset(presetFiles[0]);
            Console.WriteLine($"Loaded preset for {loadedPreset.CarName} - {loadedPreset.DriftStyle} {loadedPreset.Version}.");
        }
    }
}
