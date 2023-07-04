namespace HumanReadableTime
{
    public class TimeFormat
    {
        public static string GetReadableTime(int seconds)
        {
            int minutes = (int)Math.Floor(seconds / 60.0);
            int hours = (int)Math.Floor(minutes / 60.0);

            seconds -= minutes * 60;
            minutes -= hours * 60;

            List<int> test=new List<int>();
            int[] testArr=new int[30];

            return string.Format("{0}:{1}:{2}",
                hours.ToString().PadLeft(2, '0'),
                minutes.ToString().PadLeft(2, '0'),
                seconds.ToString().PadLeft(2, '0')
                );
        }

        [Test]
        public void HumanReadableTest()
        {
            Assert.AreEqual("00:00:00", TimeFormat.GetReadableTime(0));
            Assert.AreEqual("00:00:05", TimeFormat.GetReadableTime(5));
            Assert.AreEqual("00:01:00", TimeFormat.GetReadableTime(60));
            Assert.AreEqual("23:59:59", TimeFormat.GetReadableTime(86399));
            Assert.AreEqual("99:59:59", TimeFormat.GetReadableTime(359999));
        }
    }
}