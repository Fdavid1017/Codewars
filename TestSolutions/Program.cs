using System.Drawing;

class TestClass
{
    public enum Direction { Up = 8, Down = 2, Left = 4, Right = 6 }

    static void Main(string[] args)
    {
        PriorityQueue<Direction, DateTime> queue = new PriorityQueue<Direction, DateTime>();

        queue.Enqueue(Direction.Up, DateTime.Now);
        queue.Enqueue(Direction.Down, DateTime.Now);
        queue.Enqueue(Direction.Left, DateTime.Now);

        queue.Dequeue();

        

        Console.WriteLine("END");
    }
}