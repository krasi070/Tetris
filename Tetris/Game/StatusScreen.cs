namespace Game
{
    using System;

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
                return 21;
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

        public static int NextBlockStartRow
        {
            get
            {
                return StartRow + 9;
            }
        }

        public static int NextBlockEndRow
        {
            get
            {
                return EndRow - 3;
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
            Console.Write("+{0}+", new string('-', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 4);
            Console.Write("|{0}SCORE|", new string(' ', EndCol - StartCol - 5));

            Console.SetCursorPosition(StartCol, StartRow + 5);
            Console.Write("|{0}|", new string('0', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 6);
            Console.Write("+{0}+", new string('-', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 7);
            Console.Write("|{0}|", new string(' ', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, StartRow + 8);
            Console.Write("|{0}NEXT{0}|", new string(' ', (EndCol - StartCol - 4) / 2));

            Console.SetCursorPosition(StartCol, StartRow + 9);
            Console.Write("|{0}|", new string(' ', EndCol - StartCol));

            this.HideNextBlock();

            Console.SetCursorPosition(StartCol, EndRow - 2);
            Console.Write("+{0}+", new string('-', EndCol - StartCol));

            Console.SetCursorPosition(StartCol, EndRow - 1);
            Console.Write("|{0}LEVEL - 00{0}|", new string(' ', (EndCol - StartCol - 10) / 2));

            Console.SetCursorPosition(StartCol, EndRow);
            Console.WriteLine("+{0}+", new string('-', EndCol - StartCol));
        }

        public void ShowNextBlock(Block block)
        {
            this.HideNextBlock();
            if (block == null)
            {
                return;
            }

            Console.BackgroundColor = block.Color;
            switch (block.Type)
            {
                case 1:
                    this.ShowBlockO();
                    break;
                case 2:
                    this.ShowBlockI();
                    break;
                case 3:
                    this.ShowBlockT();
                    break;
                case 4:
                    this.ShowBlockL();
                    break;
                case 5:
                    this.ShowBlockJ();
                    break;
                case 6:
                    this.ShowBlockS();
                    break;
                case 7:
                    this.ShowBlockZ();
                    break;
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void HideNextBlock()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = NextBlockStartRow; i <= NextBlockEndRow; i++)
            {
                Console.SetCursorPosition(StartCol, i);
                Console.Write("|{0}|", new string(' ', EndCol - StartCol));
            }
        }

        public void ChangeLinesValue(int lines)
        {
            Console.SetCursorPosition(LinesColStart, LinesRowStart);
            Console.WriteLine(lines.ToString().PadLeft(3, '0'));
        }

        public void ChangeScoreValue(long score)
        {
            Console.SetCursorPosition(StartCol + 1, StartRow + 5);
            Console.WriteLine("{0}", score.ToString().PadLeft(22, '0'));
        }

        public void ChangeHighscoreValue(long score)
        {
            Console.SetCursorPosition(StartCol + 1, StartRow + 2);
            Console.WriteLine("{0}", score.ToString().PadLeft(22, '0'));
        }

        public void ChangeLevelValue(int level)
        {
            Console.SetCursorPosition(StartCol + 15, EndRow - 1);
            Console.WriteLine(level.ToString().PadLeft(2, '0'));
        }

        private void ShowBlockO()
        {
            Console.SetCursorPosition(StartCol + 7, NextBlockStartRow + 1);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 7, NextBlockStartRow + 2);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 7, NextBlockStartRow + 3);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 7, NextBlockStartRow + 4);
            Console.Write(new string(' ', 10));
        }

        private void ShowBlockI()
        {
            Console.SetCursorPosition(StartCol + 2, NextBlockStartRow + 2);
            Console.Write(new string(' ', 20));
            Console.SetCursorPosition(StartCol + 2, NextBlockStartRow + 3);
            Console.Write(new string(' ', 20));
        }

        private void ShowBlockT()
        {
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 1);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 2);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 3);
            Console.Write(new string(' ', 5));
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 4);
            Console.Write(new string(' ', 5));
        }

        private void ShowBlockL()
        {
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 1);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 2);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 3);
            Console.Write(new string(' ', 5));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 4);
            Console.Write(new string(' ', 5));
        }

        private void ShowBlockJ()
        {
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 1);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 2);
            Console.Write(new string(' ', 15));
            Console.SetCursorPosition(StartCol + 14, NextBlockStartRow + 3);
            Console.Write(new string(' ', 5));
            Console.SetCursorPosition(StartCol + 14, NextBlockStartRow + 4);
            Console.Write(new string(' ', 5));
        }

        private void ShowBlockS()
        {
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 1);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 2);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 3);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 4);
            Console.Write(new string(' ', 10));
        }

        private void ShowBlockZ()
        {
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 1);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 4, NextBlockStartRow + 2);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 3);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(StartCol + 9, NextBlockStartRow + 4);
            Console.Write(new string(' ', 10));
        }
    }
}