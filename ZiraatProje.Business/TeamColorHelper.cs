using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public static class TeamColorHelper
    {
        private static readonly ConcurrentDictionary<string, string> _registeredTeamColors = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterTeamColor(string? teamName, string? colorHex)
        {
            if (!string.IsNullOrWhiteSpace(teamName) && !string.IsNullOrWhiteSpace(colorHex))
            {
                _registeredTeamColors[teamName.Trim()] = colorHex.Trim();
            }
        }

        public static void RegisterTeams(IEnumerable<Team>? teams)
        {
            if (teams == null) return;
            foreach (var t in teams)
            {
                RegisterTeamColor(t.TeamName, t.Color);
            }
        }

        public static string GetDefaultColorForTeam(string? teamName)
        {
            if (string.IsNullOrWhiteSpace(teamName)) return "#7c3aed";

            var trimmed = teamName.Trim();
            if (_registeredTeamColors.TryGetValue(trimmed, out var registeredHex) && !string.IsNullOrWhiteSpace(registeredHex))
            {
                return registeredHex;
            }

            return trimmed.ToLower() switch
            {
                "takip" => "#7c3aed",
                "tahsis" => "#059669",
                "teminat" => "#2563eb",
                "departman yönetimi" or "yönetim" => "#bc171d",
                _ => "#7c3aed"
            };
        }

        public static string GetHeaderColor(string? colorHex, string? teamName = null)
        {
            if (!string.IsNullOrWhiteSpace(colorHex) && colorHex.StartsWith("#") && (colorHex.Length == 7 || colorHex.Length == 9))
            {
                return colorHex;
            }
            return GetDefaultColorForTeam(teamName);
        }

        public static string GetBgColor(string? colorHex, string? teamName = null)
        {
            var primary = GetHeaderColor(colorHex, teamName);
            try
            {
                var color = ColorTranslator.FromHtml(primary);
                // Mix 92% white (255,255,255) with 8% primary color for a soft pastel background
                int r = (int)(color.R * 0.08 + 255 * 0.92);
                int g = (int)(color.G * 0.08 + 255 * 0.92);
                int b = (int)(color.B * 0.08 + 255 * 0.92);
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return "#f5f3ff";
            }
        }

        public static string GetBorderColor(string? colorHex, string? teamName = null)
        {
            var primary = GetHeaderColor(colorHex, teamName);
            try
            {
                var color = ColorTranslator.FromHtml(primary);
                // Mix 70% white with 30% primary color for a soft border
                int r = (int)(color.R * 0.30 + 255 * 0.70);
                int g = (int)(color.G * 0.30 + 255 * 0.70);
                int b = (int)(color.B * 0.30 + 255 * 0.70);
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return "#ddd6fe";
            }
        }

        public static string GetTextColor(string? colorHex, string? teamName = null)
        {
            var primary = GetHeaderColor(colorHex, teamName);
            try
            {
                var color = ColorTranslator.FromHtml(primary);
                // Darken by 25% for high-contrast readable text
                int r = (int)(color.R * 0.75);
                int g = (int)(color.G * 0.75);
                int b = (int)(color.B * 0.75);
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return "#5b21b6";
            }
        }
    }
}
