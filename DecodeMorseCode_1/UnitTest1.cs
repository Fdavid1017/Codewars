namespace DecodeMorseCode_1
{
    public class MorseCodeDecoder
    {
        public static string Decode(string morseCode)
        {
            throw new System.NotImplementedException("Please provide some code.");
        }

        [Test]
        public void MorseCodeDecoderBasicTest_1()
        {
            try
            {
                string input = ".... . -.--   .--- ..- -.. .";
                string expected = "HEY JUDE";

                string actual = MorseCodeDecoder.Decode(input);

                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
            }
        }
    }
}