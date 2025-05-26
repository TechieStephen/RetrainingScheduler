using RetrainingSchedular;
using RetrainingSchedular.Models;

namespace RetrainingSchdular.Test
{
    public class SchedularTest
    {
        private const int TotalMorningMinutes = 180;
        private const int TotalAfternoonMinutes = 240;

        private List<Talk> SampleTalks => new()
        {
            new Talk("Talk A", 60),
            new Talk("Talk B", 45),
            new Talk("Talk C", 30),
            new Talk("Talk D", 60),
            new Talk("Talk E", 45)
        };

        private Schedular CreateScheduler()
        {
            // Assuming GetMorningSession and GetAfternoonSession exist in a Scheduler class
            return new Schedular(SampleTalks);
        }


        [Fact]
        public void FindBestFit_ShouldReturnTopLongestTalkEqualTo180Mins()
        {
            var scheduler = CreateScheduler();
            var result = scheduler.FindBestFit(SampleTalks, 180);

            Assert.NotNull(result);
            Assert.True(result.Sum(t => t.Duration) <= 180);
        }

        [Fact]
        public void FindBestFitBacktracking_ShouldReturnBestOptimalTalksBasedOnTheOriginalOrderOfTheTalksEqualTo180Mins()
        {
            var scheduler = CreateScheduler();
            var result = scheduler.FindBestFitBacktracking(SampleTalks, 180);

            Assert.NotNull(result);
            Assert.True(result.Sum(t => t.Duration) <= 180);
            //morning sessions should be idealy filled perfectly
            Assert.Equal(180, result.Sum(t => t.Duration)); 
        }

        [Fact]
        public void GetMorningSession_ShouldReturn180MinutesOfTalks()
        {
            var scheduler = CreateScheduler();
            var result = scheduler.GetMorningSession(SampleTalks, TotalMorningMinutes);

            Assert.NotNull(result);
            Assert.True(result.Sum(t => t.Duration) <= TotalMorningMinutes);
        }

        [Fact]
        public void GetAfternoonSession_ShouldReturn240MinutesOfTalks()
        {
            var scheduler = CreateScheduler();
            var result = scheduler.GetAfternoonSession(SampleTalks, TotalAfternoonMinutes);

            Assert.NotNull(result);
            Assert.True(result.Sum(t => t.Duration) <= TotalAfternoonMinutes);
        }

        [Fact]
        public void FindBestFitBacktracking_ShouldPickSmallerFittingCombination()
        {
            var scheduler = CreateScheduler();
            var talks = new List<Talk>
            {
                new Talk("Talk 1", 100),
                new Talk("Talk 2", 80),
                new Talk("Talk 3", 60),
                new Talk("Talk 4", 20)
            };

            var result = scheduler.FindBestFitBacktracking(talks, 180);
            Assert.True(result.Sum(t => t.Duration) <= 180);
            Assert.Equal(180, result.Sum(t => t.Duration)); // Should select 100 + 80 or similar best combo
        }
    }
}