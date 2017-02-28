namespace Game
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Engine
    {
        private const int maxLevel = 20;
        private const int interval = 30;
        private const int singlePoints = 40;
        private const int doublePoints = 100;
        private const int triplePoints = 300;
        private const int tetrisPoints = 1200;
        private readonly int[] hiddenArea = { 0, 1 };
        private int level = 1;
        private int linesCleared = 0;
        private long lastUpdate = 0;
        private long score = 0;

        private Board board;
        private StatusScreen statusScreen;
        private Block currBlock;

        public Engine(Board board, StatusScreen statusScreen)
        {
            this.board = board;
            this.statusScreen = statusScreen;
        }

        public void Run()
        {
            this.board.RenderGameBorders();
            this.statusScreen.Render();
            this.board.Render();
            this.statusScreen.ChangeLinesValue(this.linesCleared);
            this.statusScreen.ChangeScoreValue(this.score);
            this.statusScreen.ChangeLevelValue(this.level);

            Stopwatch timer = new Stopwatch();
            timer.Start();
            currBlock = SpawnBlock();
            while (true)
            {
                this.ProcessInput();
                long elapsedTime = timer.ElapsedMilliseconds;
                long timeForNextFall = lastUpdate + ((maxLevel + 1 - level) * interval);
                if (elapsedTime >= timeForNextFall)
                {
                    lastUpdate = elapsedTime;
                    if (!DropBlock())
                    {
                        this.CheckForFullRows();
                        if (CheckIfPlayerLost())
                        {
                            Console.Clear();
                            Console.WriteLine("Failed!");
                            return;
                        }

                        currBlock = SpawnBlock();
                    }
                }
            }
        }

        private Block SpawnBlock()
        {
            Block newBlock = new Block();
            this.board.InsertBlock(newBlock);

            return newBlock;
        }

        //true - block fell
        //false - block collided
        private bool DropBlock()
        {
            if (this.IsBlockDropable())
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
                currBlock.Drop();
                board.InsertBlock(currBlock);
                this.board.RenderPart(currBlock.Coordinates);

                return true;
            }

            return false;
        }

        private void MoveBlockToTheRight()
        {
            if (this.IsBlockMovableToTheRight())
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
                currBlock.MoveRight();
                board.InsertBlock(currBlock);
                this.board.RenderPart(currBlock.Coordinates);
            }
        }

        private void MoveBlockToTheLeft()
        {
            if (this.IsBlockMovableToTheLeft())
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
                currBlock.MoveLeft();
                board.InsertBlock(currBlock);
                this.board.RenderPart(currBlock.Coordinates);
            }
        }

        private void RotateBlock()
        {
            if (this.IsBlockRotatable())
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
                currBlock.Rotate();
                board.InsertBlock(currBlock);
                this.board.RenderPart(currBlock.Coordinates);
            }
        }

        private void CheckForFullRows()
        {
            List<int> rowsToDelete = new List<int>();
            for (int i = 0; i < this.board.BoardMatrix.GetLength(0); i++)
            {
                int count = 0;
                for (int j = 0; j < this.board.BoardMatrix.GetLength(1); j++)
                {
                    if (this.board.BoardMatrix[i, j] > 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count == this.board.BoardMatrix.GetLength(1))
                {
                    rowsToDelete.Add(i);
                }
            }

            if (rowsToDelete.Count > 0)
            {
                var rows = rowsToDelete.ToArray();

                int timeInMilliseconds = (maxLevel + 1 - level) * (interval / 2);
                this.board.FlashRows(rows, timeInMilliseconds);

                this.RemoveFullRows(rows);

                this.linesCleared += rows.Length;
                this.statusScreen.ChangeLinesValue(this.linesCleared);

                this.IncreaseScore(rows.Length);
                this.statusScreen.ChangeScoreValue(this.score);

                this.IncreaseLevelIfPossible();
                this.statusScreen.ChangeLevelValue(this.level);
            }
        }

        private void IncreaseScore(int rowsCleared)
        {
            if (rowsCleared == 1)
            {
                this.score += this.level * singlePoints;
            }
            else if (rowsCleared == 2)
            {
                this.score += this.level * doublePoints;
            }
            else if (rowsCleared == 3)
            {
                this.score += this.level * triplePoints;
            }
            else if (rowsCleared == 4)
            {
                this.score += this.level * tetrisPoints;
            }
        }

        private void IncreaseLevelIfPossible()
        {
            this.level = Math.Min(maxLevel, (this.linesCleared + 10) / 10);
        }

        private void RemoveFullRows(int[] rows)
        {
            int[,] newBoard = new int[Board.Rows + Board.HiddenRows, Board.Cols];
            int behind = 0;
            for (int i = this.board.BoardMatrix.GetLength(0) - 1; i >= 0; i--)
            {
                bool isRowFull = false;
                for (int j = 0; j < rows.Length; j++)
                {
                    if (i == rows[j])
                    {
                        isRowFull = true;
                        behind++;
                        break;
                    }  
                }

                if (!isRowFull)
                {
                    for (int j = 0; j < this.board.BoardMatrix.GetLength(1); j++)
                    {
                        newBoard[i + behind, j] = this.board.BoardMatrix[i, j];
                    }
                }
            }

            this.board.BoardMatrix = newBoard;
            this.board.Render();
        }

        private void DeleteFromBoard(int[] coordinates)
        {
            for (int i = 0; i < coordinates.Length; i += 2)
            {
                this.board.BoardMatrix[coordinates[i], coordinates[i + 1]] = 0;
            }
        }

        private void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey pressedKey = Console.ReadKey(true).Key;
                switch (pressedKey)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        this.MoveBlockToTheRight();
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        this.MoveBlockToTheLeft();
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        this.DropBlock();
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                    case ConsoleKey.Z:
                        this.RotateBlock();
                        break;
                    case ConsoleKey.P:
                        this.Pause();
                        break;
                }
            }
        }

        private void Pause()
        {
            this.board.EmptyGameArea();
            Console.SetCursorPosition((Board.EndCol - Board.StartCol) / 2 - 7, (Board.EndRow - Board.StartRow) / 2);
            Console.WriteLine("EXIT? (Y / N)");

            ConsoleKey key = Console.ReadKey(true).Key;
            while (true)
            {
                bool unpause = false;
                switch (key)
                {
                    case ConsoleKey.Y:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.N:
                    case ConsoleKey.P:
                        this.board.Render();
                        unpause = true;
                        break;
                }

                if (unpause)
                {
                    break;
                }

                key = Console.ReadKey(true).Key;
            }
        }

        private bool IsBlockDropable()
        {
            for (int i = 0; i < currBlock.LowestPoints.Length; i += 2)
            {
                if (currBlock.LowestPoints[i] + 1 >= this.board.BoardMatrix.GetLength(0) ||
                    this.board.BoardMatrix[currBlock.LowestPoints[i] + 1, currBlock.LowestPoints[i + 1]] > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsBlockMovableToTheRight()
        {
            for (int i = 1; i < currBlock.RightmostPoints.Length; i += 2)
            {
                if (currBlock.RightmostPoints[i] + 1 >= this.board.BoardMatrix.GetLength(1) ||
                    this.board.BoardMatrix[currBlock.RightmostPoints[i - 1], currBlock.RightmostPoints[i] + 1] > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsBlockMovableToTheLeft()
        {
            for (int i = 1; i < currBlock.LeftmostPoints.Length; i += 2)
            {
                if (currBlock.LeftmostPoints[i] - 1 < 0 ||
                    this.board.BoardMatrix[currBlock.LeftmostPoints[i - 1], currBlock.LeftmostPoints[i] - 1] > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsBlockRotatable()
        {
            int[] possiblePoints = currBlock.GetCoordinatesAfterRotation((currBlock.Rotation + 1) % 4);
            int[,] tempBoard = new int[this.board.BoardMatrix.GetLength(0), this.board.BoardMatrix.GetLength(1)];
            for (int i = 0; i < tempBoard.GetLength(0); i++)
            {
                for (int j = 0; j < tempBoard.GetLength(1); j++)
                {
                    tempBoard[i, j] = this.board.BoardMatrix[i, j];
                }
            }

            for (int i = 0; i < currBlock.Coordinates.Length; i += 2)
            {
                tempBoard[currBlock.Coordinates[i], currBlock.Coordinates[i + 1]] = 0;
            }

            for (int i = 0; i < possiblePoints.Length; i += 2)
            {
                if (possiblePoints[i] < 0 || possiblePoints[i] >= Board.Rows)
                {
                    return false;
                }

                if (possiblePoints[i + 1] < 0 || possiblePoints[i + 1] >= Board.Cols)
                {
                    return false;
                }

                if (tempBoard[possiblePoints[i], possiblePoints[i + 1]] > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckIfPlayerLost()
        {
            for (int i = 0; i < currBlock.Coordinates.Length; i += 2)
            {
                for (int j = 0; j < hiddenArea.Length; j++)
                {
                    if (currBlock.Coordinates[i] == hiddenArea[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
