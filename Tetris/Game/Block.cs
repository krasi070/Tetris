namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Block
    {
        public Block()
            : this(GetBlockType(), 0)
        {
        }

        public Block(int type, int rotation)
        {
            this.Type = type;
            this.Color = GetCorrespondingColorForType(this.Type);
            this.Rotation = rotation;
            this.Coordinates = GetStartingCoordinates(this.Type);
            this.LowestPoints = this.GetLowestPoints();
            this.RightmostPoints = this.GetRightmostPoints();
            this.LeftmostPoints = this.GetLeftmostPoints();
        }

        // 0 - nothing
        // 1 - O
        // 2 - I
        // 3 - T
        // 4 - L
        // 5 - J
        // 6 - S
        // 7 - Z
        public int Type { get; private set; }

        public ConsoleColor Color { get; private set; }

        public int Rotation { get; set; }

        public int[] Coordinates { get; set; }

        public int[] LowestPoints { get; set; }

        public int[] RightmostPoints { get; set; }

        public int[] LeftmostPoints { get; set; }

        public void Drop()
        {
            for (int i = 0; i < this.Coordinates.Length; i += 2)
            {
                this.Coordinates[i]++;
            }

            for (int i = 0; i < this.LowestPoints.Length; i += 2)
            {
                this.LowestPoints[i]++;
            }

            for (int i = 0; i < this.RightmostPoints.Length; i += 2)
            {
                this.RightmostPoints[i]++;
            }

            for (int i = 0; i < this.LeftmostPoints.Length; i += 2)
            {
                this.LeftmostPoints[i]++;
            }
        }

        public void MoveLeft()
        {
            for (int i = 1; i < this.Coordinates.Length; i += 2)
            {
                this.Coordinates[i]--;
            }

            for (int i = 1; i < this.LowestPoints.Length; i += 2)
            {
                this.LowestPoints[i]--;
            }

            for (int i = 1; i < this.RightmostPoints.Length; i += 2)
            {
                this.RightmostPoints[i]--;
            }

            for (int i = 1; i < this.LeftmostPoints.Length; i += 2)
            {
                this.LeftmostPoints[i]--;
            }
        }

        public void MoveRight()
        {
            for (int i = 1; i < this.Coordinates.Length; i += 2)
            {
                this.Coordinates[i]++;
            }

            for (int i = 1; i < this.LowestPoints.Length; i += 2)
            {
                this.LowestPoints[i]++;
            }

            for (int i = 1; i < this.RightmostPoints.Length; i += 2)
            {
                this.RightmostPoints[i]++;
            }

            for (int i = 1; i < this.LeftmostPoints.Length; i += 2)
            {
                this.LeftmostPoints[i]++;
            }
        }

        public void Rotate()
        {
            this.Rotation = (this.Rotation + 1) % 4;
            this.Coordinates = this.GetCoordinatesAfterRotation(this.Rotation);
            this.LowestPoints = this.GetLowestPoints();
            this.RightmostPoints = this.GetRightmostPoints();
            this.LeftmostPoints = this.GetLeftmostPoints();
        }

        public int[] GetCoordinatesAfterRotation(int rotation)
        {
            switch (this.Type)
            {
                case 1:
                    return this.Coordinates;
                case 2:
                    return this.GetCoordinatesAfterRotationForI(rotation);
                case 3:
                    return this.GetCoordinatesAfterRotationForT(rotation);
                case 4:
                    return this.GetCoordinatesAfterRotationForL(rotation);
                case 5:
                    return this.GetCoordinatesAfterRotationForJ(rotation);
                case 6:
                    return this.GetCoordinatesAfterRotationForS(rotation);
                case 7:
                    return this.GetCoordinatesAfterRotationForZ(rotation);
                default:
                    return null;
            }
        }

        private int[] GetCoordinatesAfterRotationForI(int rotation)
        {
            if (rotation % 2 == 1)
            {
                return new int[]
                {
                            this.Coordinates[0] - 2, this.Coordinates[1] + 2,
                            this.Coordinates[2] - 1, this.Coordinates[3] + 1,
                            this.Coordinates[4], this.Coordinates[5],
                            this.Coordinates[6] + 1, this.Coordinates[7] - 1
                };
            }
            else
            {
                return new int[]
                {
                            this.Coordinates[0] + 2, this.Coordinates[1] - 2,
                            this.Coordinates[2] + 1, this.Coordinates[3] - 1,
                            this.Coordinates[4], this.Coordinates[5],
                            this.Coordinates[6] - 1, this.Coordinates[7] + 1
                };
            }
        }

        private int[] GetCoordinatesAfterRotationForT(int rotation)
        {
            switch (rotation)
            {
                case 1:
                    return new int[]
                    {
                                this.Coordinates[4] - 1, this.Coordinates[5] - 1,
                                this.Coordinates[0], this.Coordinates[1],
                                this.Coordinates[2], this.Coordinates[3],
                                this.Coordinates[6], this.Coordinates[7]
                    };
                case 2:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1],
                                this.Coordinates[2], this.Coordinates[3],
                                this.Coordinates[4], this.Coordinates[5],
                                this.Coordinates[6] - 1, this.Coordinates[7] + 1
                    };
                case 3:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1],
                                this.Coordinates[4], this.Coordinates[5],
                                this.Coordinates[6], this.Coordinates[7],
                                this.Coordinates[2] + 1, this.Coordinates[3] + 1
                    };
                default:
                    return new int[]
                    {
                                this.Coordinates[0] + 1, this.Coordinates[1] - 1,
                                this.Coordinates[2], this.Coordinates[3],
                                this.Coordinates[4], this.Coordinates[5],
                                this.Coordinates[6], this.Coordinates[7]
                    };
            }
        }

        private int[] GetCoordinatesAfterRotationForL(int rotation)
        {
            switch (rotation)
            {
                case 1:
                    return new int[]
                    {
                                this.Coordinates[0] - 1, this.Coordinates[1],
                                this.Coordinates[2] - 1, this.Coordinates[3],
                                this.Coordinates[4], this.Coordinates[5] - 1,
                                this.Coordinates[6], this.Coordinates[7] + 1
                    };
                case 2:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1] + 2,
                                this.Coordinates[2] + 1, this.Coordinates[3] - 1,
                                this.Coordinates[4], this.Coordinates[5],
                                this.Coordinates[6] - 1, this.Coordinates[7] + 1
                    };
                case 3:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1] - 1,
                                this.Coordinates[2], this.Coordinates[3] + 1,
                                this.Coordinates[4] + 1, this.Coordinates[5],
                                this.Coordinates[6] + 1, this.Coordinates[7]
                    };
                default:
                    return new int[]
                    {
                                this.Coordinates[0] + 1, this.Coordinates[1] - 1,
                                this.Coordinates[2], this.Coordinates[3],
                                this.Coordinates[4] - 1, this.Coordinates[5] + 1,
                                this.Coordinates[6], this.Coordinates[7] - 2
                    };
            }
        }

        private int[] GetCoordinatesAfterRotationForJ(int rotation)
        {
            switch (rotation)
            {
                case 1:
                    return new int[]
                    {
                                this.Coordinates[0] - 1, this.Coordinates[1] + 1,
                                this.Coordinates[2], this.Coordinates[3],
                                this.Coordinates[4] + 1, this.Coordinates[5] - 2,
                                this.Coordinates[6], this.Coordinates[7] - 1
                    };
                case 2:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1] - 1,
                                this.Coordinates[2], this.Coordinates[3] - 1,
                                this.Coordinates[4] - 1, this.Coordinates[5] + 1,
                                this.Coordinates[6] - 1, this.Coordinates[7] + 1
                    };
                case 3:
                    return new int[]
                    {
                                this.Coordinates[0], this.Coordinates[1] + 1,
                                this.Coordinates[2] - 1, this.Coordinates[3] + 2,
                                this.Coordinates[4], this.Coordinates[5],
                                this.Coordinates[6] + 1, this.Coordinates[7] - 1
                    };
                default:
                    return new int[]
                    {
                                this.Coordinates[0] + 1, this.Coordinates[1] - 1,
                                this.Coordinates[2] + 1, this.Coordinates[3] - 1,
                                this.Coordinates[4], this.Coordinates[5] + 1,
                                this.Coordinates[6], this.Coordinates[7] + 1
                    };
            }
        }

        private int[] GetCoordinatesAfterRotationForS(int rotation)
        {
            if (rotation % 2 == 1)
            {
                return new int[]
                    {
                                this.Coordinates[0] - 1, this.Coordinates[1],
                                this.Coordinates[2], this.Coordinates[3] - 1,
                                this.Coordinates[4] - 1, this.Coordinates[5] + 2,
                                this.Coordinates[6], this.Coordinates[7] + 1
                    };
            }
            else
            {
                return new int[]
                    {
                                this.Coordinates[0] + 1, this.Coordinates[1],
                                this.Coordinates[2], this.Coordinates[3] + 1,
                                this.Coordinates[4] + 1, this.Coordinates[5] - 2,
                                this.Coordinates[6], this.Coordinates[7] - 1
                    };
            }
        }

        private int[] GetCoordinatesAfterRotationForZ(int rotation)
        {
            if (rotation % 2 == 1)
            {
                return new int[]
                {
                            this.Coordinates[0] - 1, this.Coordinates[1] + 2,
                            this.Coordinates[2], this.Coordinates[3],
                            this.Coordinates[4] - 1, this.Coordinates[5] + 1,
                            this.Coordinates[6], this.Coordinates[7] - 1
                };
            }
            else
            {
                return new int[]
                {
                            this.Coordinates[0] + 1, this.Coordinates[1] - 2,
                            this.Coordinates[2], this.Coordinates[3],
                            this.Coordinates[4] + 1, this.Coordinates[5] - 1,
                            this.Coordinates[6], this.Coordinates[7] + 1
                };
            }
        }

        public static ConsoleColor GetCorrespondingColorForType(int type)
        {
            switch (type)
            {
                case 1:
                    return ConsoleColor.Yellow;
                case 2:
                    return ConsoleColor.Cyan;
                case 3:
                    return ConsoleColor.DarkMagenta;
                case 4:
                    return ConsoleColor.Gray;
                case 5:
                    return ConsoleColor.Blue;
                case 6:
                    return ConsoleColor.Green;
                case 7:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Black;
            }
        }

        private static byte GetBlockType()
        {
            Random rand = new Random();

            return (byte)rand.Next(1, 8);
        }        

        private static int[] GetStartingCoordinates(int type)
        {
            switch (type)
            {
                case 1:
                    return new int[] { 0, 4, 0, 5, 1, 4, 1, 5};
                case 2:
                    return new int[] { 0, 3, 0, 4, 0, 5, 0, 6 };
                case 3:
                    return new int[] { 0, 3, 0, 4, 0, 5, 1, 4 };
                case 4:
                    return new int[] { 0, 3, 0, 4, 0, 5, 1, 3 };
                case 5:
                    return new int[] { 0, 3, 0, 4, 0, 5, 1, 5 };
                case 6:
                    return new int[] { 0, 4, 0, 5, 1, 3, 1, 4 };
                case 7:
                    return new int[] { 0, 3, 0, 4, 1, 4, 1, 5 };
                default:
                    return null;
            }
        }

        private int[] GetLowestPoints()
        {
            Dictionary<int, int> lowestPoints = new Dictionary<int, int>();
            for (int i = 0; i < this.Coordinates.Length; i += 2)
            {
                if (!lowestPoints.ContainsKey(this.Coordinates[i + 1]))
                {
                    lowestPoints.Add(this.Coordinates[i + 1], this.Coordinates[i]);
                }

                if (lowestPoints[this.Coordinates[i + 1]] < this.Coordinates[i])
                {
                    lowestPoints[this.Coordinates[i + 1]] = this.Coordinates[i];
                }
            }

            int[] result = new int[lowestPoints.Count * 2];
            int index = 0;
            foreach (var pair in lowestPoints)
            {
                result[index] = pair.Value;
                result[index + 1] = pair.Key;
                index += 2;
            }

            return result;
        }

        private int[] GetRightmostPoints()
        {
            Dictionary<int, int> rightmostPoints = new Dictionary<int, int>();
            for (int i = 0; i < this.Coordinates.Length; i += 2)
            {
                if (!rightmostPoints.ContainsKey(this.Coordinates[i]))
                {
                    rightmostPoints.Add(this.Coordinates[i], this.Coordinates[i + 1]);
                }

                if (rightmostPoints[this.Coordinates[i]] < this.Coordinates[i + 1])
                {
                    rightmostPoints[this.Coordinates[i]] = this.Coordinates[i + 1];
                }
            }

            int[] result = new int[rightmostPoints.Count * 2];
            int index = 0;
            foreach (var pair in rightmostPoints)
            {
                result[index] = pair.Key;
                result[index + 1] = pair.Value;
                index += 2;
            }

            return result;
        }

        private int[] GetLeftmostPoints()
        {
            Dictionary<int, int> leftmostPoints = new Dictionary<int, int>();
            for (int i = 0; i < this.Coordinates.Length; i += 2)
            {
                if (!leftmostPoints.ContainsKey(this.Coordinates[i]))
                {
                    leftmostPoints.Add(this.Coordinates[i], this.Coordinates[i + 1]);
                }

                if (leftmostPoints[this.Coordinates[i]] > this.Coordinates[i + 1])
                {
                    leftmostPoints[this.Coordinates[i]] = this.Coordinates[i + 1];
                }
            }

            int[] result = new int[leftmostPoints.Count * 2];
            int index = 0;
            foreach (var pair in leftmostPoints)
            {
                result[index] = pair.Key;
                result[index + 1] = pair.Value;
                index += 2;
            }

            return result;
        }
    }
}
