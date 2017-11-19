using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game_Of_Life

// check: warum so oft alle schleifen -> geht das nicht mit weniger schleifen?
// --warum checkneighbor + checkplayground
// veränderungen an der datenstruktur DÜRFEN nicht beim zeichnen/rendern passieren
// CountCreatures -> sehr schwer zu verstehen, dieser part könnte +bersichtlicher programmiert sein (wäre aber vermutlich dann etwas langsamer)
// low: wegen performance -> canvas -> selbst mit GDI zeichen, nicht mit Canvas Children

{
    class PlayGround
    {
        public Creature[,] gameBoard;
        private int fieldSize;

        public PlayGround(int size, int creatures)
        {
            gameBoard = new Creature[size, size];
            fieldSize = size;

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

        public void CheckNeighbor()
        {
            int neighbors = 0;
            bool isAlive;

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (gameBoard[x, y] != null && gameBoard[x, y].IsAlive)
                    {
                        neighbors = CountCreatures(x, y);

                        switch (neighbors)
                        {
                            case 2:
                                isAlive = true;
                                break;
                            case 3:
                                isAlive = true;
                                break;
                            default:
                                isAlive = false;
                                break;
                        }

                        gameBoard[x, y].IsAlive = isAlive;
                    }
                }
            }
        }

        public void CheckPlayGround()
        {
            int creatures = 0;
            Creature[,] newGeneration = new Creature[fieldSize, fieldSize];

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    creatures = CountCreatures(x, y);

                    if (creatures == 3)
                    {
                        Creature creature = new Creature();
                        newGeneration[x, y] = creature;
                    }
                }
            }

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (newGeneration[x, y] != null)
                    {
                        gameBoard[x, y] = newGeneration[x, y];
                    }
                }
            }
        }

        /// <summary>
        /// Count all Creatures arround the passed x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CountCreatures(int x, int y)
        {
            int creatures = 0;

            if (x < fieldSize - 1 && y < fieldSize - 1)
            {
                if (gameBoard[x + 1, y + 1] != null)
                {
                    creatures++;
                }
                if (gameBoard[x + 1, y] != null)
                {
                    creatures++;
                }
            }

            if (x > 0 && y > 0)
            {
                if (gameBoard[x - 1, y - 1] != null)
                {
                    creatures++;
                }
            }

            if (x > 0 && y < fieldSize - 1)
            {
                if (gameBoard[x - 1, y + 1] != null)
                {
                    creatures++;
                }
                if (gameBoard[x, y + 1] != null)
                {
                    creatures++;
                }
                if (gameBoard[x - 1, y] != null)
                {
                    creatures++;
                }
            }

            if (x < fieldSize - 1 && y > 0)
            {
                if (gameBoard[x + 1, y - 1] != null)
                {
                    creatures++;
                }
                if (gameBoard[x, y - 1] != null)
                {
                    creatures++;
                }
            }

            return creatures;
        }
    }
}
