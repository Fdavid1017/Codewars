namespace HumanReadableDuration
{
    // https://www.codewars.com/kata/52742f58faf5485cae000b9a
    public class HumanTimeFormat
    {
        public static string formatDuration(int seconds)
        {
            if (seconds == 0) return "now";

            int minutes = (int)Math.Floor(seconds / 60.0);
            seconds -= minutes * 60;

            int hours = (int)Math.Floor(minutes / 60.0);
            minutes -= hours * 60;

            int days = (int)Math.Floor(hours / 24.0);
            hours -= days * 24;

            int years = (int)Math.Floor(days / 365.0);
            days -= years * 365;

            Dictionary<string, int> times = new Dictionary<string, int>()
            {
                {"year", years},
                {"day", days},
                {"hour", hours},
                {"minute", minutes},
                {"second", seconds}
            };


            string[] timesStrings = times.Where(x => x.Value != 0)
                .Select(x => $"{x.Value} {(x.Value > 1 ? x.Key + "s" : x.Key)}")
                .ToArray();

            if (timesStrings.Length > 1)
            {
                return $"{string.Join(", ", timesStrings.Take(timesStrings.Length - 1))} and {timesStrings.Last()}";
            }
            else if (timesStrings.Length == 1)
            {
                return timesStrings[0];
            }

            return "";
        }

        [Test]
        public void basicTests()
        {
            Assert.AreEqual("now", HumanTimeFormat.formatDuration(0));
            Assert.AreEqual("1 second", HumanTimeFormat.formatDuration(1));
            Assert.AreEqual("1 minute and 2 seconds", HumanTimeFormat.formatDuration(62));
            Assert.AreEqual("2 minutes", HumanTimeFormat.formatDuration(120));
            Assert.AreEqual("1 hour, 1 minute and 2 seconds", HumanTimeFormat.formatDuration(3662));
            Assert.AreEqual("182 days, 1 hour, 44 minutes and 40 seconds", HumanTimeFormat.formatDuration(15731080));
            Assert.AreEqual("4 years, 68 days, 3 hours and 4 minutes", HumanTimeFormat.formatDuration(132030240));
            Assert.AreEqual("6 years, 192 days, 13 hours, 3 minutes and 54 seconds", HumanTimeFormat.formatDuration(205851834));
            Assert.AreEqual("8 years, 12 days, 13 hours, 41 minutes and 1 second", HumanTimeFormat.formatDuration(253374061));
            Assert.AreEqual("7 years, 246 days, 15 hours, 32 minutes and 54 seconds", HumanTimeFormat.formatDuration(242062374));
            Assert.AreEqual("3 years, 85 days, 1 hour, 9 minutes and 26 seconds", HumanTimeFormat.formatDuration(101956166));
            Assert.AreEqual("1 year, 19 days, 18 hours, 19 minutes and 46 seconds", HumanTimeFormat.formatDuration(33243586));
        }
    }
}