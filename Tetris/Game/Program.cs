namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            ChangeConosleValues();            
            Board board = new Board();
            Engine engine = new Engine(board);
            engine.Run();
        }

        private static void ChangeConosleValues()
        {
            Console.Title = "Tetris";
            Console.CursorVisible = false;
            Console.WindowHeight = 48;
            Console.BufferHeight = 48;
            Console.WindowWidth = 80;
            Console.BufferWidth = 80;
        }
    }
}