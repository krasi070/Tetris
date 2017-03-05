namespace Game
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.IO;

    public class Engine
    {
        private const int maxLevel = 20;
        private const int defaultInterval = 30;
        private const int singlePoints = 40;
        private const int doublePoints = 100;
        private const int triplePoints = 300;
        private const int tetrisPoints = 1200;
        private readonly int[] hiddenArea = { 0, 1 };
        private const string highscorePath = "../../h-1h50043s.txt";
        private int interval = 30;
        private int level = 1;
        private int initLevel = 1;
        private int linesCleared = 0;
        private long lastUpdate = 0;
        private long score = 0;
        private long highscore = 0;

        private Board board;
        private StatusScreen statusScreen;
        private Block currBlock = null;
        private Block nextBlock = null;

        public Engine(Board board, StatusScreen statusScreen)
        {
            this.board = board;
            this.statusScreen = statusScreen;
        }

        public void Run()
        {
            while (true)
            {
                this.ExecuteGameLoop();
                if (this.score > this.highscore)
                {
                    this.WriteNewHighscore();
                }

                this.ResetValues();
            }
        }

        private void ExecuteGameLoop()
        {
            this.highscore = this.ReadHighscore();
            this.board.RenderGameBorders();
            this.statusScreen.Render();
            this.initLevel = this.ChooseInitLevel();
            this.LevelUpIfPossible();

            this.currBlock = new Block();
            this.nextBlock = new Block();
            this.Refresh();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            this.SpawnBlock(this.currBlock);
            while (true)
            {
                this.interval = defaultInterval;
                this.ProcessInput();
                long elapsedTime = timer.ElapsedMilliseconds;
                long timeForNextFall = this.lastUpdate + ((maxLevel + 1 - this.level) * interval);
                if (elapsedTime >= timeForNextFall)
                {
                    this.lastUpdate = elapsedTime;
                    if (!this.DropBlock())
                    {
                        this.CheckForFullRows();
                        if (this.CheckIfPlayerLost())
                        {
                            this.board.EmptyGameArea();
                            Console.SetCursorPosition((Board.EndCol - Board.StartCol) / 2 - 13, (Board.EndRow - Board.StartRow) / 2);
                            Console.WriteLine("YOU LOST! PLAY AGAIN? (Y / N)");

                            ConsoleKey key;
                            while (true)
                            {
                                key = Console.ReadKey(true).Key;
                                switch (key)
                                {
                                    case ConsoleKey.Y:
                                        return;
                                    case ConsoleKey.N:
                                        Console.Clear();
                                        Environment.Exit(0);
                                        break;
                                }
                            }
                        }

                        this.currBlock = new Block(this.nextBlock.Type, 0);
                        this.nextBlock = new Block();
                        this.statusScreen.ShowNextBlock(this.nextBlock);
                        this.SpawnBlock(this.currBlock);
                    }
                }
            }
        }

        private void ResetValues()
        {
            this.level = 1;
            this.initLevel = 1;
            this.linesCleared = 0;
            this.lastUpdate = 0;
            this.score = 0;
            this.currBlock = null;
            this.nextBlock = null;
            this.board.BoardMatrix = new int[Board.Rows + Board.HiddenRows, Board.Cols];
        }

        private int ChooseInitLevel()
        {
            int initLvl = 1;
            int startCol = (Board.EndCol - Board.StartCol) / 2 - 3;
            int startRow = (Board.EndRow - Board.StartRow) / 2;

            Console.SetCursorPosition(startCol - 3, startRow - 1);
            Console.Write("CHOOSE LEVEL");
            Console.SetCursorPosition(startCol, startRow);
            Console.Write("< {0} >", initLvl.ToString().PadLeft(2, '0'));

            ConsoleKey key;
            bool levelChosen = false;
            while (!levelChosen)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        initLvl = initLvl == 20 ? 1 : initLvl + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        initLvl = initLvl == 1 ? 20 : initLvl - 1;
                        break;
                    case ConsoleKey.Enter:
                        levelChosen = true;
                        break;
                }

                Console.SetCursorPosition(startCol, startRow);
                Console.Write("< {0} >", initLvl.ToString().PadLeft(2, '0'));
            }

            return initLvl;
        }

        private void SpawnBlock(Block block)
        {
            this.board.InsertBlock(block);
        }

        //true - block fell
        //false - block collided
        private bool DropBlock()
        {
            if (this.IsBlockDropable())
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
                this.currBlock.Drop();
                this.board.InsertBlock(currBlock);
                this.board.RenderPart(currBlock.Coordinates);

                return true;
            }

            return false;
        }

        private void InstantDropBlock()
        {
            bool isDropable = this.IsBlockDropable();
            if (isDropable)
            {
                this.DeleteFromBoard(currBlock.Coordinates);
                this.board.RenderPart(currBlock.Coordinates);
            }

            while (isDropable)
            {
                this.currBlock.Drop();
                isDropable = this.IsBlockDropable();
            }

            this.board.InsertBlock(currBlock);
            this.board.RenderPart(currBlock.Coordinates);
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

                this.LevelUpIfPossible();
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

        private void LevelUpIfPossible()
        {
            this.level = Math.Min(maxLevel, (this.linesCleared) / 10 + this.initLevel);
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
                        this.interval = 10;
                        this.DropBlock();
                        break;
                    case ConsoleKey.Spacebar:
                        this.InstantDropBlock();
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                    case ConsoleKey.Z:
                        this.RotateBlock();
                        break;
                    case ConsoleKey.P:
                        this.Pause();
                        break;
                    case ConsoleKey.R:
                        this.Refresh();
                        break;
                }
            }
        }

        private void Refresh()
        {
            this.board.RenderGameBorders();
            this.statusScreen.Render();
            this.board.Render();
            this.statusScreen.ChangeLinesValue(this.linesCleared);
            this.statusScreen.ChangeScoreValue(this.score);
            this.statusScreen.ChangeHighscoreValue(this.highscore);
            this.statusScreen.ShowNextBlock(nextBlock);
            this.statusScreen.ChangeLevelValue(this.level);
        }

        private void Pause()
        {
            this.board.EmptyGameArea();
            this.statusScreen.HideNextBlock();
            int startCol = (Board.EndCol - Board.StartCol) / 2 - 7;
            int startRow = (Board.EndRow - Board.StartRow) / 2 - 3;

            Console.SetCursorPosition(startCol, startRow);
            Console.WriteLine("EXIT? (Y / N)");
            this.ShowControls(startRow, startCol - 4);

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
                        this.statusScreen.ShowNextBlock(nextBlock);
                        unpause = true;
                        break;
                    case ConsoleKey.R:
                        this.Refresh();
                        this.board.EmptyGameArea();
                        this.statusScreen.HideNextBlock();
                        Console.SetCursorPosition(startCol - 4, startRow);
                        Console.WriteLine("EXIT? (Y / N)");
                        this.ShowControls(startRow, startCol);
                        break;
                }

                if (unpause)
                {
                    break;
                }

                key = Console.ReadKey(true).Key;
            }
        }

        private void ShowControls(int startRow, int startCol)
        {
            Console.SetCursorPosition(startCol - 1, startRow + 3);
            Console.WriteLine("CONTROLS:");

            Console.SetCursorPosition(startCol - 1, startRow + 4);
            Console.WriteLine("RIGHT ARROW, D - MOVE RIGHT");

            Console.SetCursorPosition(startCol - 1, startRow + 5);
            Console.WriteLine("LEFT ARROW, A - MOVE LEFT");

            Console.SetCursorPosition(startCol - 1, startRow + 6);
            Console.WriteLine("DOWN ARROW, S - DROP FASTER");

            Console.SetCursorPosition(startCol - 1, startRow + 7);
            Console.WriteLine("SPACEBAR - INSTANT DROP");

            Console.SetCursorPosition(startCol - 1, startRow + 8);
            Console.WriteLine("UP ARROW, W, Z - ROTATE");

            Console.SetCursorPosition(startCol - 1, startRow + 9);
            Console.WriteLine("P - PAUSE / UNPAUSE");

            Console.SetCursorPosition(startCol - 1, startRow + 10);
            Console.WriteLine("R - REFRESH");
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

        private long ReadHighscore()
        {
            long prevHighscore = 0;
            using (var reader = new StreamReader(highscorePath))
            {
                string scoreAsStr = reader.ReadLine();
                if (scoreAsStr == null)
                {
                    prevHighscore = 0;
                }
                else
                {
                    prevHighscore = long.Parse(scoreAsStr);
                }
            }

            return prevHighscore;
        }

        private void WriteNewHighscore()
        {
            using (var writer = new StreamWriter(highscorePath))
            {
                writer.WriteLine(this.score);
            }
        }
    }
}