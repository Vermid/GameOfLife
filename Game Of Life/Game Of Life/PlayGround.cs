using System;

namespace Game_Of_Life
{
    class PlayGround
    {
        public Creature[,] gameBoard;
        private int fieldSize;
        private int generation;

        public PlayGround(int size, int creatures)
        {
            fieldSize = size;
            gameBoard = new Creature[fieldSize, fieldSize];

            int rndY;
            int rndX;
            Random rnd = new Random();

            for (int i = 0; i <= creatures; i++)
            {
                rndY = rnd.Next(0, size);
                rndX = rnd.Next(0, size);

                if (gameBoard[rndY, rndX] == null)
                {
                    Creature creature = new Creature();
                    gameBoard[rndY, rndX] = creature;
                }
            }
        }

        public PlayGround(int size)
        {
            fieldSize = size;
            gameBoard = new Creature[fieldSize, fieldSize];
        }


        /// <summary>
        /// Checks all neighbors for every field
        /// </summary>
        public void CheckNeighbor()
        {
            int creatures = 0;
            int up = -1;
            int left = -1;
            int right = 1;
            int down = 1;

            Creature[,] newGeneration = new Creature[fieldSize, fieldSize];

            for (int y = 0; y < fieldSize; y++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    //check here when y is 0 (left side)
                    if (y == 0)
                    {
                        //check here if the wanted pos is the upper left corner
                        if (x == 0)
                        {
                            //what needs too be checked  0 means dont check this 
                            creatures = CountCreatures(y, x, 0, down, right, 0);
                        }
                        //check here if the wanted pos is the down left corner
                        else if (x == fieldSize - 1)
                        {
                            creatures = CountCreatures(y, x, 0, down, 0, left);
                        }
                        //checks the rest from the upper field
                        else
                        {
                            creatures = CountCreatures(y, x, 0, down, right, left);
                        }
                    }
                    //checks here if y is on the right side of the field
                    else if (y == fieldSize - 1)
                    {
                        //upper Right
                        if (x == 0)
                        {
                            creatures = CountCreatures(y, x, up, 0, right, 0);
                        }
                        //down Right
                        else if (x == fieldSize - 1)
                        {
                            creatures = CountCreatures(y, x, up, 0, 0, left);
                        }
                        //checks the rest from the down field
                        else
                        {
                            creatures = CountCreatures(y, x, up, 0, right, left);
                        }
                    }
                    //cheks the rest of the field
                    else if (y > 0)
                    {

                        if (x == 0)
                        {
                            creatures = CountCreatures(y, x, up, down, right, 0);
                        }

                        else if (x == fieldSize - 1)
                        {
                            creatures = CountCreatures(y, x, up, down, 0, left);
                        }
                        else
                        {
                            creatures = CountCreatures(y, x, up, down, right, left);
                        }
                    }
                    //check here if on this pos is a creature or not
                    if (gameBoard[y, x] != null)
                    {
                        if (creatures == 2 || creatures == 3)
                        {
                            newGeneration[y, x] = gameBoard[y, x];
                            generation++;
                        }
                    }
                    //no creature ? check if a creature can live here
                    else
                    {
                        if (creatures == 3)
                        {
                            Creature creature = new Creature();
                            newGeneration[y, x] = creature;
                            generation++;
                        }
                    }
                }
            }
            //with Array.Clear you can clear an array very easy (What you want to clear, From what Pos, To what pos)
            Array.Clear(gameBoard, 0, gameBoard.Length);
            gameBoard = newGeneration;
        }


        public void CreateNewCreature(int y, int x)
        {
            Creature creature = new Creature();
            gameBoard[y, x] = creature;
        }

        public int GetGeneration()
        {
            return generation;
        }

        /// <summary>
        /// Counts the Creatures that are near the Y and X pos needs to know what is able to check up down left right is 0 or 1 
        /// return how much creatures are around the Y and X pos
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        private int CountCreatures(int y, int x, int up, int down, int right, int left)
        {
            int creatures = 0;
            if (up != 0)
            {
                creatures += (gameBoard[y + up, x] != null ? 1 : 0);
                if (left != 0)
                {
                    creatures += (gameBoard[y + up, x + left] != null ? 1 : 0);
                }

                if (right != 0)
                {
                    creatures += (gameBoard[y + up, x + right] != null ? 1 : 0);
                }
            }

            if (down != 0)
            {
                creatures += (gameBoard[y + down, x] != null ? 1 : 0);
                if (left != 0)
                {
                    creatures += (gameBoard[y + down, x + left] != null ? 1 : 0);
                }

                if (right != 0)
                {
                    creatures += (gameBoard[y + down, x + right] != null ? 1 : 0);
                }
            }

            if (left != 0)
            {
                creatures += (gameBoard[y, x + left] != null ? 1 : 0);
            }

            if (right != 0)
            {
                creatures += (gameBoard[y, x + right] != null ? 1 : 0);
            }
            return creatures;
        }
    }
}
