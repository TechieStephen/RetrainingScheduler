using RetrainingSchedular;
using RetrainingSchedular.Models;

namespace RetrainingSchdular.Test
{
    public class HelpersTest
    {
        [Theory]
        [InlineData("Talk 101", false)]
        [InlineData("Teaching Innovation", true)]
        [InlineData("Data Science 2025", false)]
        [InlineData("123", false)]
        [InlineData("", false)] // empty strings are not allowed
        public void IsvalidTitle_CheckForNumbers(string title, bool expected)
        {
            var result = Helpers.IsValidTitle(title);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateStartTimes_ShouldReturnCorrectSchedule()
        {
            // Arrange
            var talks = new List<Talk>
            {
                new Talk("Talk A", 60),
                new Talk("Talk B", 45)
            };

            var startTime = new TimeSpan(9, 0, 0); // 09:00 AM

            // Act
            var schedule = Helpers.CalculateStartTimes(talks, startTime);

            // Assert
            Assert.Equal(2, schedule.Count);
            Assert.Equal("09:00AM", schedule[0].Time);
            Assert.Equal("Talk A", schedule[0].Talk.Title);

            Assert.Equal("10:00AM", schedule[1].Time);
            Assert.Equal("Talk B", schedule[1].Talk.Title);
        }


        [Theory]
        [InlineData(9, 0, "09:00AM")]
        [InlineData(13, 15, "01:15PM")]
        [InlineData(0, 5, "12:05AM")]
        [InlineData(23, 59, "11:59PM")]
        public void FormatTime_ShouldReturnFormattedString(int hour, int minute, string expected)
        {
            // Arrange
            var time = new TimeSpan(hour, minute, 0);

            // Act
            var result = Helpers.FormatTime(time);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
