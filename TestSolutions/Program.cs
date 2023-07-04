using System.Drawing;

class TestClass
{
    static void Main(string[] args)
    {
        string[,] test = new string[,]
        {
           {"R", "Y", "Y", "Y", "R", "-",},
           {"Y", "-", "-", "-", "-", "-",},
           {"R", "Y", "R", "Y", "R", "Y",},
           {"Y", "R", "-", "-", "-", "-",},
           {"R", "R", "R", "Y", "-", "-",},
           {"R", "-", "-", "-", "-", "-",},
           {"Y", "Y", "R", "-", "-", "-", },
        };

        int column = 0;
        int row = 4;

        //for (int i = 0; i < test.GetLength(0); i++)
        //{
        //    for (int j = 0; j < test.GetLength(1); j++)
        //    {
        //        test[i, j] = "-";
        //    }
        //}


        for (int i = column, j = row; i < test.GetLength(0) && j < test.GetLength(1); i++, j++)
        {
            test[i, j] = "1";
        }

        // Values down
        for (int i = column - 1, j = row - 1; i >= 0 && j >= 0; i--, j--)
        {
            test[i, j] = "2";
        }

        for (int i = column, j = row; i < test.GetLength(0) && j >= 0; i++, j--)
        {
            test[i, j] = "3";
        }

        // Values down
        for (int i = column - 1, j = row + 1; j >= 0 && j < test.GetLength(1) && i < test.GetLength(0) && i >= 0; i--, j++)
        {
            test[i, j] = "4";
        }

        //test[column, row] = "0";

        for (int i = 0; i < test.GetLength(0); i++)
        {
            for (int j = 0; j < test.GetLength(1); j++)
            {
                Console.Write(test[i, j] + " ");
            }

            Console.WriteLine();
        }
    }
}