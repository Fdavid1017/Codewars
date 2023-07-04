namespace RankingPokerHands
{
    // https://www.codewars.com/kata/5739174624fc28e188000465

    public class Tests
    {
        public enum Result
        {
            Win,
            Loss,
            Tie
        }

        public enum CardType
        {
            Spade, Heart, Diamond, Club
        }

        public class Card
        {
            public int value;
            public CardType cardType;

            public Card(string type)
            {
                char valueChar = type[0];
                char typeChar = type[1];

                switch (valueChar)
                {
                    case 'T':
                        this.value = 10;
                        break;
                    case 'J':
                        this.value = 11;
                        break;
                    case 'Q':
                        this.value = 12;
                        break;
                    case 'K':
                        this.value = 13;
                        break;
                    case 'A':
                        this.value = 14;
                        break;
                    default:
                        this.value = int.Parse(valueChar.ToString());
                        break;
                }

                switch (typeChar)
                {
                    case 'S':
                        this.cardType = CardType.Spade;
                        break;
                    case 'H':
                        this.cardType = CardType.Heart;
                        break;
                    case 'D':
                        this.cardType = CardType.Diamond;
                        break;
                    case 'C':
                        this.cardType = CardType.Club;
                        break;
                }
            }
        }

        public class PokerHand
        {
            Card[] cards;

            public PokerHand(string hand)
            {
                string[] cardsString = hand.Split(' ');

                cards = new[]
                {
                    new Card(cardsString[0]),
                    new Card(cardsString[1]),
                    new Card(cardsString[2]),
                    new Card(cardsString[3]),
                    new Card(cardsString[4]),
                };
            }

            public Result CompareWith(PokerHand hand)
            {
                int currentHandValue = this.GetHandValue();
                int comparedHandValue = hand.GetHandValue();

                if (currentHandValue > comparedHandValue)
                {
                    return Result.Win;
                }

                if (currentHandValue < comparedHandValue)
                {
                    return Result.Loss;
                }

                return Result.Tie;
            }

            public int GetHandValue()
            {
                int value = 0;

                // ===== High card (simple card values) =====
                foreach (Card card in cards)
                {
                    value += card.value;
                }



                // ===== Pair & Two pairs (+1 * pair) =====
                int pairs = this.cards.GroupBy(x => x.value)
                    .Select(x => x.Count())
                    .Count(x => x == 2);
                value += pairs;

                // ===== Three of kings (+3) =====
                bool withThreeKind = this.cards.GroupBy(x => x.value)
                    .Select(x => x.Count())
                    .Any(x => x == 3);
                value += withThreeKind ? 3 : 0;

                // ===== Straight (+4) =====
                Card[] orderedCards = this.cards.OrderBy(x => x.value).ToArray();
                bool inOrder = true;

                List<Card> tempStraightCards = orderedCards.ToList();

                // First is 2 and last is Ace
                if (tempStraightCards[0].value == 2 && tempStraightCards[4].value == 14)
                {
                    tempStraightCards.RemoveAt(4);
                }

                for (int i = 0; i < orderedCards.Length - 1 && inOrder; i++)
                {
                    if (orderedCards[i + 1].value - orderedCards[i].value != 1)
                    {
                        inOrder = false;
                    }
                }

                if (inOrder)
                {
                    value += 4;
                }

                // ===== Flush (+5) =====
                bool allSameType = cards.GroupBy(x => x.cardType).Count() == 1;
                if (allSameType)
                {
                    value += 5;
                }

                // ===== Full house (+6) =====
                if (pairs == 1)
                {
                    var cardsPair = this.cards.GroupBy(x => x.value).First(x => x.Count() == 2).ToArray();
                    Card[] fullHouseCards = cards.Except(cardsPair).ToArray();
                    int fullHouseCardTypes = fullHouseCards.GroupBy(x => x.cardType).Count();

                    if (fullHouseCardTypes == 3)
                    {
                        value += 6;
                    }
                }

                // ===== Four of a kind (+7) =====
                Card[] fourWithSameValue = cards.GroupBy(x => x.value).FirstOrDefault(x => x.Count() == 4)?.ToArray() ?? null;

                if (fourWithSameValue != null)
                {
                    value += 7;
                }

                return value;
            }
        }

        [Test]
        public void MyTest()
        {
            Assert.AreEqual(Result.Tie, new PokerHand("6S 6D 6H 2C KS").CompareWith(new PokerHand("2H 3H 4H 5H 6H")));
        }

        [TestFixture]
        public class PokerTests
        {
            [TestCase("Highest straight flush wins", Result.Loss, "2H 3H 4H 5H 6H", "KS AS TS QS JS")]
            [TestCase("Straight flush wins of 4 of a kind", Result.Win, "2H 3H 4H 5H 6H", "AS AD AC AH JD")]
            [TestCase("Highest 4 of a kind wins", Result.Win, "AS AH 2H AD AC", "JS JD JC JH 3D")]
            [TestCase("4 Of a kind wins of full house", Result.Loss, "2S AH 2H AS AC", "JS JD JC JH AD")]
            [TestCase("Full house wins of flush", Result.Win, "2S AH 2H AS AC", "2H 3H 5H 6H 7H")]
            [TestCase("Highest flush wins", Result.Win, "AS 3S 4S 8S 2S", "2H 3H 5H 6H 7H")]
            [TestCase("Flush wins of straight", Result.Win, "2H 3H 5H 6H 7H", "2S 3H 4H 5S 6C")]
            [TestCase("Equal straight is tie", Result.Tie, "2S 3H 4H 5S 6C", "3D 4C 5H 6H 2S")]
            [TestCase("Straight wins of three of a kind", Result.Win, "2S 3H 4H 5S 6C", "AH AC 5H 6H AS")]
            [TestCase("3 Of a kind wins of two pair", Result.Loss, "2S 2H 4H 5S 4C", "AH AC 5H 6H AS")]
            [TestCase("2 Pair wins of pair", Result.Win, "2S 2H 4H 5S 4C", "AH AC 5H 6H 7S")]
            [TestCase("Highest pair wins", Result.Loss, "6S AD 7H 4S AS", "AH AC 5H 6H 7S")]
            [TestCase("Pair wins of nothing", Result.Loss, "2S AH 4H 5S KC", "AH AC 5H 6H 7S")]
            [TestCase("Highest card loses", Result.Loss, "2S 3H 6H 7S 9C", "7H 3C TH 6H 9S")]
            [TestCase("Highest card wins", Result.Win, "4S 5H 6H TS AC", "3S 5H 6H TS AC")]
            [TestCase("Equal cards is tie", Result.Tie, "2S AH 4H 5S 6C", "AD 4C 5H 6H 2C")]
            public void PokerHandTest(string description, Result expected, string hand, string opponentHand)
            {
                Assert.AreEqual(expected, new PokerHand(hand).CompareWith(new PokerHand(opponentHand)), description);
            }
        }
    }
}