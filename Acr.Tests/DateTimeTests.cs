using System;
using Xunit;


namespace Acr.Tests {
    
    public class DateTimeTests {

        [Fact]
        public void EndOfToday() {
            var dt = DateTime.Now.EndOfDay();
            Assert.True(
                dt.Hour == 23 &&
                dt.Minute == 59
            );
        }


        [Fact]
        public void BeginningOfDay() {
            var dt = new DateTime(12, 1, 8, 1, 0, 0);
            Assert.Equal(dt.BeginningOfDay().Hour, 0);
        }


        [Fact]
        public void AddWeekdays() {
            var dt = new DateTime(2011, 4, 3); // Sunday
            dt = dt.AddWeekdays(7);
            Assert.Equal(dt.Day, 12);
        }

        [Fact]
        public void BeginningOfMonth() {
            var dt = new DateTime(2000, 12, 15);
            Assert.Equal(dt.BeginningOfMonth().Day, 1);
            //dt.AddWeekdays()
            //dt.AddWorkdays()
            //dt.LastDayOfMonth()
            
        }


        [Fact]
        public void LastDayOfMonth() {
            var dt = new DateTime(2000, 3, 1);
            Assert.Equal(dt.LastDayOfMonth().Day, 31);
        }


        [Fact]
        public void IsWeekday() {
            Assert.True(DayOfWeek.Tuesday.IsWeekday());
            Assert.False(DayOfWeek.Tuesday.IsWeekend());
            Assert.True(DayOfWeek.Saturday.IsWeekend());
        }


        [Fact]
        public void DaysDifference() {
            var dt1 = new DateTime(2000, 1, 1);
            var dt2 = new DateTime(2000, 1, 2);

            Assert.Equal(dt1.DifferenceInDays(dt2), 1);
        }
    }
}
