using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Twotwo.Agent.Services
{
    public static class PromptLoader
    {
        private static Dictionary<string, string> _prompts = new();
        private static bool _isLoaded = false;
        private static readonly object _lock = new();

        public static void Load(string filePath)
        {
            lock (_lock)
            {
                // Resolve possible locations: provided path, or base output dir, or parent dir
                string path = filePath;
                if (!File.Exists(path))
                {
                    var baseDir = AppContext.BaseDirectory;
                    var p1 = Path.Combine(baseDir, filePath);
                    if (File.Exists(p1)) path = p1;
                    else
                    {
                        var p2 = Path.Combine(baseDir, "..", filePath);
                        if (File.Exists(p2)) path = p2;
                    }
                }

                if (!File.Exists(path))
                {
                    // Try loading from the library directory (where Prompts.json may be packaged)
                    var libDir = Path.GetDirectoryName(typeof(PromptLoader).Assembly.Location);
                    if (!string.IsNullOrEmpty(libDir)
                        )
                    {
                        var libPath = Path.Combine(libDir!, "Prompts.json");
                        if (File.Exists(libPath)) path = libPath;
                    }
                }
                if (!File.Exists(path)) throw new FileNotFoundException($"Prompt file not found: {path}");

                var json = File.ReadAllText(path);
                _prompts = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
                _isLoaded = true;
            }
        }

        public static string Get(string key)
        {
            if (!_isLoaded) throw new InvalidOperationException("Prompts not loaded. Call PromptLoader.Load() first.");
            return _prompts.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}
