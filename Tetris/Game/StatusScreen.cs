namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatusScreen
    { 
        public static int StartRow
        {
            get
            {
                return 4;
            }
        }

        public static int EndRow
        {
            get
            {
                return 30;
            }
        }

        public static int StartCol
        {
            get
            {
                return 54;
            }
        }

        public static int EndCol
        {
            get
            {
                return 76;
            }
        }

        public static int LinesColStart
        {
            get
            {
                return 28;
            }
        }

        public static int LinesRowStart
        {
            get
            {
                return 2;
            }
        }

        public void Render()
        {
            int width = Board.Cols * Board.BlockWidth;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("+{0}+", new string('-', width));
            Console.WriteLine("|{0}|", new string(' ', width));
            string lines = "LINES - 000";
            Console.WriteLine("|{0}{1}{2}|", new string(' ', (width - lines.Length) / 2), lines, new string(' ', (width - lines.Length) / 2 + 1));
            Console.WriteLine("|{0}|", new string(' ', width));

            Console.SetCursorPosition(StartCol, StartRow);
            Console.Write("+{0}+", new string('-', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 1);
            Console.Write("|{0}HIGHSCORE|", new string(' ', EndCol - StartCol - 9));

            Console.SetCursorPosition(StartCol, StartRow + 2);
            Console.Write("|{0}|", new string('0', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 3);
            Console.Write("|{0}|", new string(' ', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 4);
            Console.Write("|{0}|", new string(' ', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 5);
            Console.Write("|{0}SCORE|", new string(' ', EndCol - StartCol - 5));

            Console.SetCursorPosition(StartCol, StartRow + 6);
            Console.Write("|{0}|", new string('0', EndCol - StartCol));

            for (int i = StartRow + 7; i < EndRow - 2; i++)
            {
                Console.SetCursorPosition(StartCol, i);
                Console.WriteLine("|{0}|", new string(' ', EndCol - StartCol));
            }

            Console.SetCursorPosition(StartCol, EndRow - 2);
            Console.Write("+{0}+", new string('-', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, EndRow - 1);
            Console.Write("|{0}LEVEL - 00{0}|", new string(' ', (EndCol - StartCol - 10) / 2));

            Console.SetCursorPosition(StartCol, EndRow);
            Console.WriteLine("+{0}+", new string('-', EndCol - StartCol));
        }

        public void ChangeLinesValue(int lines)
        {
            Console.SetCursorPosition(LinesColStart, LinesRowStart);
            Console.WriteLine(lines.ToString().PadLeft(3, '0'));
        }

        public void ChangeScoreValue(long score)
        {
            Console.SetCursorPosition(StartCol + 1, StartRow + 6);
            Console.WriteLine(score.ToString().PadLeft(22, '0'));
        }

        public void ChangeLevelValue(int level)
        {
            Console.SetCursorPosition(StartCol + 15, EndRow - 1);
            Console.WriteLine(level.ToString().PadLeft(2, '0'));
        }
    }
}