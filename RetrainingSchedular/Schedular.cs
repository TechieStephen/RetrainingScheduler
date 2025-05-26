using RetrainingSchedular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrainingSchedular
{
    /// <summary>
    /// Responsible for scheduling talks into tracks with morning and afternoon sessions.
    /// Uses fitting algorithms to optimize talk distribution within the available time.
    /// </summary>
    public class Schedular
    {
        /// <summary>
        /// List of all talks to be scheduled.
        /// </summary>
        public List<Talk>? AllTalks { get; set; }

        /// <summary>
        /// Constructor to initialize the Schedular with a list of talks.
        /// </summary>
        public Schedular(List<Talk> talks)
        {
            AllTalks = talks;
        }


        private readonly int TotalMorningMinutes = 180;
        private readonly int TotalAfternoonMinutes = 240;
        private readonly TimeSpan MorningSessionStart = new(9,0,0);
        private readonly TimeSpan AfternoonSessionStart = new(13, 0, 0);


        /// <summary>
        /// Schedules talks into multiple tracks with morning and afternoon sessions.
        /// Calculates the sharing start time based on afternoon session length.
        /// </summary>
        /// <returns>A list of scheduled <see cref="Track"/> instances.</returns>
        public List<Track> ScheduleTracks()
        {
            List<Track> Tracks = new();

            do
            {
                Track track = new();

                // Get morning talks
                var morningTalks = GetMorningSession(AllTalks, TotalMorningMinutes);
                track.MorningTalks = morningTalks;
                AllTalks.RemoveAll(t => morningTalks.Contains(t));


                // Get afternoon talks
                var afternoonTalks = GetAfternoonSession(AllTalks, TotalAfternoonMinutes);
                track.AfternoonTalks = afternoonTalks;
                AllTalks.RemoveAll(t => afternoonTalks.Contains(t));


                //Set the sharing start time
                
                //find total afternoon minutes
                var afternoonTotal = afternoonTalks.Sum(t => t.Duration);

                track.SharingTimeStart = AfternoonSessionStart.Add(TimeSpan.FromMinutes(afternoonTotal));

                //Check if the sharing time starts earlier than 4PM, if so set it to 4PM
                //The fitting algorithm already makes sure sharing time is no later than 5 PM by ensuring afternoon session never ecceeds 240Mins is about 4 PM - 5 PM
                if (track.SharingTimeStart < new TimeSpan(16, 0, 0))
                    track.SharingTimeStart = new TimeSpan(16, 0, 0);

                Tracks.Add(track);
            }
            while (AllTalks.Count > 0);


            return Tracks;
        }


        /// <summary>
        /// Retrieves a best-fit list of talks for the morning session.
        /// </summary>
        /// <param name="availableTalks">List of available talks.</param>
        /// <param name="totalMinutes">Maximum duration of the morning session.</param>
        /// <returns>List of talks that best fit the morning session time.</returns>
        public List<Talk> GetMorningSession(List<Talk> availableTalks, int totalMinutes)
        {
            return FindBestFitBacktracking(availableTalks, TotalMorningMinutes);
        }


        /// <summary>
        /// Retrieves a best-fit list of talks for the afternoon session.
        /// </summary>
        /// <param name="availableTalks">List of available talks.</param>
        /// <param name="totalMinutes">Maximum duration of the afternoon session.</param>
        /// <returns>List of talks that best fit the afternoon session time.</returns>
        public List<Talk> GetAfternoonSession(List<Talk> availableTalks, int totalMinutes)
        {
            return FindBestFitBacktracking(availableTalks, TotalAfternoonMinutes);
        }

        /// <summary>
        /// Uses greedy algoritm to pick longer (best-fit) list of talks by sorting in descending order of duration.
        /// </summary>
        /// <param name="availableTalks">List of talks to choose from.</param>
        /// <param name="maxMinutes">Maximum total duration allowed.</param>
        /// <returns>A list of talks that best fit within the allowed time.</returns>
        public List<Talk> FindBestFit(List<Talk> availableTalks, int maxMinutes)
        {
            // Sort by descending duration (uses greedy algoritm to pick longer talks first)
            var sorted = availableTalks.OrderByDescending(t => t.Duration).ToList();
            var result = new List<Talk>();
            int totalTime = 0;

            foreach (var talk in sorted)
            {
                if (totalTime + talk.Duration <= maxMinutes)
                {
                    result.Add(talk);
                    totalTime += talk.Duration;
                    //AllTalks.Remove(talk);
                }
            }

            // Remove the selected talks from the original list
            //AllTalks = AllTalks.Except(result).ToList();

            return result;
        }

        /// <summary>
        /// Uses backtracking (a BruteForce Approuch) to find the optimal combination of talks that best fits the available time.
        /// </summary>
        /// <param name="availableTalks">List of talks to evaluate.</param>
        /// <param name="maxMinutes">Maximum total duration allowed.</param>
        /// <returns>A list of talks that result in the highest total duration without exceeding the limit.</returns>
        public List<Talk> FindBestFitBacktracking(List<Talk> availableTalks, int maxMinutes)
        {
            List<Talk> bestFit = new();
            int bestSum = 0;

            void Backtrack(List<Talk> current, int index, int currentSum)
            {
                if (currentSum > maxMinutes) return;
                if (currentSum > bestSum)
                {
                    bestSum = currentSum;
                    bestFit = new List<Talk>(current);
                }

                for (int i = index; i < availableTalks.Count; i++)
                {
                    current.Add(availableTalks[i]);
                    Backtrack(current, i + 1, currentSum + availableTalks[i].Duration);
                    current.RemoveAt(current.Count - 1);
                }
            }

            Backtrack(new List<Talk>(), 0, 0);

            //foreach (var t in bestFit)
            //    AllTalks.Remove(t);

            return bestFit;
        }
    }
}
