namespace StripComments
{
    // https://www.codewars.com/kata/51c8e37cee245da6b40000bd
    public class StripCommentsSolution
    {
        public static string StripComments(string text, string[] commentSymbols)
        {
            return string.Join("\n", text.Split("\n").Select(x =>
            {
                foreach (string commentSymbol in commentSymbols)
                {
                    int commentStart = x.IndexOf(commentSymbol);
                    if (commentStart > -1)
                    {
                        x = x.Substring(0, commentStart).TrimEnd();
                    }
                    else
                    {
                        x=x.TrimEnd();
                    }
                }

                return x;
            }));
        }

        [Test]
        public void StripComments()
        {
            Assert.AreEqual(
                "apples, pears\ngrapes\nbananas",
                StripCommentsSolution.StripComments("apples, pears # and bananas\ngrapes\nbananas !apples", new string[] { "#", "!" }));

            Assert.AreEqual("a\nc\nd", StripCommentsSolution.StripComments("a #b\nc\nd $e f g", new string[] { "#", "$" }));

        }
    }
}