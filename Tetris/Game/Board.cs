﻿namespace Game
{
    using System;
    using System.Threading;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Board
    {
        public Board()
        {
            this.BoardMatrix = new int[Rows + HiddenRows, Cols];
        }

        //0 - nothing
        //1 - 7 - colors
        public int[,] BoardMatrix { get; set; }

        public static int Rows
        {
            get
            {
                return 20;
            }
        }

        public static int HiddenRows
        {
            get
            {
                return 2;
            }
        }

        public static int StartRow
        {
            get
            {
                return 5;
            }
        }

        public static int EndRow
        {
            get
            {
                return 45;
            }
        }

        public static int Cols
        {
            get
            {
                return 10;
            }
        }

        public static int StartCol
        {
            get
            {
                return 1;
            }
        }

        public static int EndCol
        {
            get
            {
                return 51;
            }
        }

        public static int BlockWidth
        {
            get
            {
                return 5;
            }
        }

        public static int BlockHeight
        {
            get
            {
                return 2;
            }
        }

        public void InsertBlock(Block block)
        {
            for (int i = 0; i < block.Coordinates.Length; i += 2)
            {
                this.BoardMatrix[block.Coordinates[i], block.Coordinates[i + 1]] = block.Type;
            }
        }

        public void RenderGameBorders()
        {
            int width = Cols * BlockWidth;
            Console.SetCursorPosition(StartCol - 1, StartRow - 1);
            Console.WriteLine("+{0}+", new string('-', width));
            int height = Rows * BlockHeight;
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine("|{0}|", new string(' ', width));
            }

            Console.WriteLine("+{0}+", new string('-', width));
        }

        public void Render()
        {
            Console.SetCursorPosition(StartCol, StartRow);
            for (int i = HiddenRows; i < HiddenRows + Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.BackgroundColor = Block.GetCorrespondingColorForType(this.BoardMatrix[i, j]);
                    for (int k = 0; k < BlockHeight; k++)
                    {
                        Console.SetCursorPosition(StartCol + j * BlockWidth, StartRow + k + (i - HiddenRows) * BlockHeight);
                        Console.Write(new string(' ', BlockWidth));
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void EmptyGameArea()
        {
            Console.SetCursorPosition(StartCol, StartRow);
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = HiddenRows; i < HiddenRows + Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    for (int k = 0; k < BlockHeight; k++)
                    {
                        Console.SetCursorPosition(StartCol + j * BlockWidth, StartRow + k + (i - HiddenRows) * BlockHeight);
                        Console.Write(new string(' ', BlockWidth));
                    }
                }
            }
        }

        public void RenderPart(int[] coordinates)
        {
            for (int i = 0; i < coordinates.Length; i += 2)
            {
                if (coordinates[i] > 1)
                {
                    Console.BackgroundColor = Block.GetCorrespondingColorForType(this.BoardMatrix[coordinates[i], coordinates[i + 1]]);
                    for (int k = 0; k < BlockHeight; k++)
                    {
                        Console.SetCursorPosition(StartCol + coordinates[i + 1] * BlockWidth, StartRow + k + (coordinates[i] - HiddenRows) * BlockHeight);
                        Console.Write(new string(' ', BlockWidth));
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void FlashRows(int[] rows, int timeInMilliseconds)
        {
            this.ChangeRowColor(rows, ConsoleColor.White);
            Thread.Sleep(timeInMilliseconds);
            this.ChangeRowColor(rows, ConsoleColor.Black);
            Thread.Sleep(timeInMilliseconds);
            this.ChangeRowColor(rows, ConsoleColor.White);
            Thread.Sleep(timeInMilliseconds);
        }

        public void ChangeRowColor(int[] rows, ConsoleColor color)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                Console.BackgroundColor = color;
                for (int k = 0; k < BlockHeight; k++)
                {
                    Console.SetCursorPosition(StartCol, StartRow + k + (rows[i] - HiddenRows) * BlockHeight);
                    Console.Write(new string(' ', BlockWidth * Cols));
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}