using RetrainingSchedular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetrainingSchedular
{
    /// <summary>
    /// Provides helper methods for validating titles and scheduling talks.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Validates that the given title is not null, not whitespace, and contains no numeric characters.
        /// </summary>
        /// <param name="title">The title to validate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="title"/> is non-empty and contains no digits; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidTitle(string title)
        {
            // Check for null, empty, or whitespace-only strings
            if (string.IsNullOrWhiteSpace(title))
                return false;

            // Ensure title contains no digits using a regular expression
            return Regex.IsMatch(title, @"^[^0-9]*$");
        }

        /// <summary>
        /// Calculates start times for a list of talks based on an inital start time.
        /// </summary>
        /// <param name="talks">The list of <see cref="Talk"/> instances to schedule.</param>
        /// <param name="startTime">The base <see cref="TimeSpan"/> to start scheduling from.</param>
        /// <returns>
        /// A list of tuples where each tuple contains a formatted time string and the corresponding <see cref="Talk"/>.
        /// </returns>
        public static List<(string Time, Talk Talk)> CalculateStartTimes(List<Talk> talks, TimeSpan startTime)
        {
            var scheduled = new List<(string Time, Talk Talk)>();
            var current = startTime;

            // Loop through each talk and compute its starting time
            foreach (var talk in talks)
            {
                scheduled.Add((FormatTime(current), talk));
                current = current.Add(TimeSpan.FromMinutes(talk.Duration));
            }

            return scheduled;
        }

        /// <summary>
        /// Formats a <see cref="TimeSpan"/> as a 12-hour clock time string with AM/PM.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan"/> representing the time of day.</param>
        /// <returns>
        /// A string formatted as "hh:mmtt", e.g., "09:30AM".
        /// </returns>
        public static string FormatTime(TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("hh:mmtt");
        }


        /// <summary>
        /// Writes a list of strings to a 'schedule.txt' file in the user's Downloads folder.
        /// </summary>
        /// <param name="lines">The lines of text to write to the file.</param>
        public static void DownloadToTextFile(List<string> lines, string fileName = "schedule.txt")
        {
            try
            {
                var downloadsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );

                if (!Directory.Exists(downloadsPath))
                    Directory.CreateDirectory(downloadsPath);

                var filePath = Path.Combine(downloadsPath, fileName);

                File.WriteAllLines(filePath, lines);

                Console.WriteLine($"Schedule saved to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write schedule to file: {ex.Message}");
            }
        }
    }
}
