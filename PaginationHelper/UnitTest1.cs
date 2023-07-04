namespace PaginationHelper
{
    public class Tests
    {

        public class PagnationHelper<T>
        {
            private IList<T> collection;
            private int itemsPerPage;

            /// <summary>
            /// Constructor, takes in a list of items and the number of items that fit within a single page
            /// </summary>
            /// <param name="collection">A list of items</param>
            /// <param name="itemsPerPage">The number of items that fit within a single page</param>
            public PagnationHelper(IList<T> collection, int itemsPerPage)
            {
                this.collection = collection;
                this.itemsPerPage = itemsPerPage;
            }

            /// <summary>
            /// The number of items within the collection
            /// </summary>
            public int ItemCount
            {
                get
                {
                    return collection.Count;
                }
            }

            /// <summary>
            /// The number of pages
            /// </summary>
            public int PageCount
            {
                get
                {
                    return (int)Math.Ceiling((float)ItemCount / (float)itemsPerPage);
                }
            }

            /// <summary>
            /// Returns the number of items in the page at the given page index 
            /// </summary>
            /// <param name="pageIndex">The zero-based page index to get the number of items for</param>
            /// <returns>The number of items on the specified page or -1 for pageIndex values that are out of range</returns>
            public int PageItemCount(int pageIndex)
            {
                if (pageIndex >= PageCount || pageIndex < 0)
                {
                    return -1;
                }

                if (pageIndex < PageCount - 1)
                {
                    return itemsPerPage;
                }

                if (pageIndex == 1)
                {
                    return ItemCount - itemsPerPage;
                }

                return ItemCount - pageIndex * itemsPerPage;
            }

            /// <summary>
            /// Returns the page index of the page containing the item at the given item index.
            /// </summary>
            /// <param name="itemIndex">The zero-based index of the item to get the pageIndex for</param>
            /// <returns>The zero-based page index of the page containing the item at the given item index or -1 if the item index is out of range</returns>
            public int PageIndex(int itemIndex)
            {
                if (itemIndex < 0 || itemIndex >= ItemCount)
                {
                    return -1;
                }

                return (int)Math.Floor((double)itemIndex / (double)itemsPerPage);
            }
        }

        [TestFixture]
        public class SolutionTest
        {
            private readonly IList<int> collection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            private PagnationHelper<int> helper;

            [SetUp]
            public void SetUp()
            {
                helper = new PagnationHelper<int>(collection, 10);
            }

            [Test]
            [TestCase(-1, ExpectedResult = -1)]
            [TestCase(1, ExpectedResult = 10)]
            [TestCase(3, ExpectedResult = -1)]
            public int PageItemCountTest(int pageIndex)
            {
                return helper.PageItemCount(pageIndex);
            }

            [Test]
            [TestCase(-1, ExpectedResult = -1)]
            [TestCase(12, ExpectedResult = 1)]
            [TestCase(24, ExpectedResult = -1)]
            public int PageIndexTest(int itemIndex)
            {
                return helper.PageIndex(itemIndex);
            }

            [Test]
            public void ItemCountTest()
            {
                Assert.AreEqual(24, helper.ItemCount);
            }

            [Test]
            public void PageCountTest()
            {
                Assert.AreEqual(3, helper.PageCount);
            }
        }
    }
}