using NUnit.Framework.Internal.Execution;
using static System.Net.WebRequestMethods;

namespace BreadcrumbGenerator
{
    // https://www.codewars.com/kata/563fbac924106b8bf7000046

    public class Kata
    {
        public static string GenerateBC(string url, string separator)
        {
            if (!url.StartsWith("http"))
            {
                url = "https://" + url;
            }


            if (Path.HasExtension(url))
            {
                string extension = Path.GetExtension(url);
                int place = url.LastIndexOf(extension);
                url = url.Remove(place, extension.Length);
            }

            Uri uri = new Uri(url);
            string[] segments = uri.Segments.Where(x => !x.ToUpper().StartsWith("INDEX")).ToArray();

            string previousPath = "";

            List<string> breadcrumbSections = segments.Select((segment, i) =>
            {
                string s = segment.Length - 1 > 30 ? AcronymizeSegment(segment) : segment;

                if (i == segments.Length - 1)
                {
                    return GetBreadcrumbLastSection(s);
                }

                string sectionPath = $"{previousPath}{segment}";
                previousPath = sectionPath;

                return GetBreadcrumbMiddleSection(s, sectionPath);
            }).ToList();

            return string.Join(separator, breadcrumbSections);



            /* ===== LOCAL FUNCTIONS ===== */

            string GetBreadcrumbMiddleSection(string sectionName, string sectionPath)
            {
                sectionName = FormatSectionName(sectionName);
                return $"<a href=\"{sectionPath}\">{sectionName}</a>";
            }

            string GetBreadcrumbLastSection(string sectionName)
            {
                return $"<span class=\"active\">{FormatSectionName(sectionName)}</span>";
            }

            string FormatSectionName(string sectionName)
            {
                return sectionName == "/" ? "HOME" : sectionName.Replace("-", " ").Replace("/", "").ToUpper();
            }

            string AcronymizeSegment(string segment)
            {
                string[] ignoredWords = { "the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a" };

                string[] words = segment.Replace("/", "").Split('-').Where(x => !ignoredWords.Contains(x.ToLower())).ToArray();
                return string.Join("", words.Select(x => x[0]));
            }
        }

        [TestFixture]
        public class SolutionTest
        {
            private string[] urls = new string[] {
                    "http://linkedin.it/surfer-bed-at-kamehameha-by-or-skin-of/",
                    "mysite.com/pictures/holidays.html",
                    "www.codewars.com/users/GiacomoSorbi?ref=CodeWars",
                    "www.microsoft.com/docs/index.htm#top",
                    "mysite.com/very-long-url-to-make-a-silly-yet-meaningful-example/example.asp",
                    "www.very-long-site_name-to-make-a-silly-yet-meaningful-example.com/users/giacomo-sorbi",
                    "https://www.linkedin.com/in/giacomosorbi",
                    "www.agcpartners.co.uk/",
                    "www.agcpartners.co.uk",
                    "https://www.agcpartners.co.uk/index.html",
                    "http://www.agcpartners.co.uk"
            };

            private string[] seps = new string[] { " + ", " : ", " / ", " * ", " > ", " + ", " * ", " * ", " # ", " >>> ", " % " };


            private string[] anss = new string[] {
                    "<a href=\"/\">HOME</a> + <span class=\"active\">SBKS</span>",
                    "<a href=\"/\">HOME</a> : <a href=\"/pictures/\">PICTURES</a> : <span class=\"active\">HOLIDAYS</span>",
                    "<a href=\"/\">HOME</a> / <a href=\"/users/\">USERS</a> / <span class=\"active\">GIACOMOSORBI</span>",
                    "<a href=\"/\">HOME</a> * <span class=\"active\">DOCS</span>",
                    "<a href=\"/\">HOME</a> > <a href=\"/very-long-url-to-make-a-silly-yet-meaningful-example/\">VLUMSYME</a> > <span class=\"active\">EXAMPLE</span>",
                    "<a href=\"/\">HOME</a> + <a href=\"/users/\">USERS</a> + <span class=\"active\">GIACOMO SORBI</span>",
                    "<a href=\"/\">HOME</a> * <a href=\"/in/\">IN</a> * <span class=\"active\">GIACOMOSORBI</span>",
                    "<span class=\"active\">HOME</span>",
                    "<span class=\"active\">HOME</span>",
                    "<span class=\"active\">HOME</span>",
                    "<span class=\"active\">HOME</span>"
            };
            [Test]
            public void ExampleTests()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"\nTest With: {urls[i]}");
                    if (i == 5) Console.WriteLine("\nThe one used in the above test was my LinkedIn Profile; if you solved the kata this far and manage to get it, feel free to add me as a contact, message me about the language that you used and I will be glad to endorse you in that skill and possibly many others :)\n\n ");

                    Assert.AreEqual(anss[i], Kata.GenerateBC(urls[i], seps[i]));
                }
            }
        }
    }
}