using System.Text;
using System.Linq;

namespace WhoLikesIt
{
    public class Kata
    {
        public static string Likes(string[] name)
        {
            string text;

            switch (name.Length)
            {
                case 0:
                    text = "no one likes this";
                    break;
                case 1:
                    text = $"{name[0]} likes this";
                    break;
                case 2:
                    text = $"{string.Join(" and ", name)} like this";
                    break;
                case 3:
                    text = $"{name[0]}, {name[1]} and {name[2]} like this";
                    break;
                default:
                    text = $"{string.Join(", ", name.Take(2))} and {name.Skip(2).Count()} others like this";
                    break;
            }

            return text;
        }

        [Test, Description("It should return correct text")]
        public void SampleTest()
        {
            Assert.AreEqual("no one likes this", Kata.Likes(new string[0]));
            Assert.AreEqual("Peter likes this", Kata.Likes(new string[] { "Peter" }));
            Assert.AreEqual("Jacob and Alex like this", Kata.Likes(new string[] { "Jacob", "Alex" }));
            Assert.AreEqual("Max, John and Mark like this", Kata.Likes(new string[] { "Max", "John", "Mark" }));
            Assert.AreEqual("Alex, Jacob and 2 others like this",
                Kata.Likes(new string[] { "Alex", "Jacob", "Mark", "Max" }));
        }
    }
}