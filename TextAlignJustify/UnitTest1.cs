namespace TextAlignJustify
{
    // https://www.codewars.com/kata/537e18b6147aa838f600001b
    public class Kata
    {
        public static string Justify(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            List<string> words = str.Split(' ').ToList();
            List<string> rows = new List<string>();

            while (words.Count > 0)
            {
                List<string> rowWords = new List<string>() { words[0] };
                words.RemoveAt(0);

                if (words.Count > 0)
                {
                    string nextWord = words[0];
                    while (words.Count > 0 && RowCharactersCountWithMinSpaces(rowWords) + nextWord.Length < len)
                    {
                        words.RemoveAt(0);
                        rowWords.Add(nextWord);

                        if (words.Count > 0)
                        {
                            nextWord = words[0];
                        }
                    }

                    // Add extra spaces
                    if (words.Count > 0 && rowWords.Count > 1)
                    {
                        int extraSpacesNeeded = len - RowCharactersCountWithMinSpaces(rowWords);

                        if (rowWords.Count <= 2)
                        {
                            rowWords[0] = rowWords[0] + new String(' ', extraSpacesNeeded);
                        }
                        else
                        {
                            int spacesUsed = 0;
                            while (spacesUsed < extraSpacesNeeded)
                            {
                                for (int wordIndex = 0; wordIndex < rowWords.Count - 1 && spacesUsed < extraSpacesNeeded; wordIndex++)
                                {
                                    rowWords[wordIndex] = rowWords[wordIndex] + " ";
                                    spacesUsed++;
                                }
                            }
                        }
                    }
                }

                rows.Add(string.Join(" ", rowWords));
            }

            return string.Join("\n", rows);

            // Calculate row length with minimum 1 space
            int RowCharactersCountWithMinSpaces(List<string> rowWords)
            {
                return rowWords.Sum(x => x.Length) + rowWords.Count - 1;
            }
        }

        [Test]
        public void MyTest()
        {
            Assert.AreEqual("123  45\n6", Kata.Justify("123 45 6", 7));
        }
    }
}