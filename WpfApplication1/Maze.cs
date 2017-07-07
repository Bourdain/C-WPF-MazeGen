#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeGen
{
    public class MazeCell
    {
        public bool isVisited;
        public bool isInit;
        public bool isFinal;

        /// <summary>
        /// As paredes do maze, se for true então existe uma parede
        /// </summary>
        public bool wallUp,
                    wallDown,
                    wallLeft,
                    wallRight;

        public bool tryUp = false,//false = quer dizer que ainda nao verificou a celula
                    tryDown = false,//true = quer dizer que ja verificou a celula
                    tryLeft = false,
                    tryRight = false;

        public MazeCell()
        {
            isVisited = isInit = false;
            wallUp = wallDown = wallLeft = wallRight = true;
        }
    }






    public enum Movement { goUp = 0, goDown = 1, goLeft = 2, goRight = 3, dontMove = 4 };


    public class Maze
    {
        public MazeCell[,] maze;
        public Maze(int width, int height)
        {
            maze = new MazeCell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    maze[i, j] = new MazeCell();
                }
            }
        }
    }

    public class Maze_Recursive_Backtracker:Maze
    {
        #region Static
        public static bool isStart = true;
        public static Random r = new Random();
        public static Point initCellPoint;
        public static Point finishCellPoint;

        public static int numMaxProc = 0;
        #endregion


        public Maze_Recursive_Backtracker(int width, int height):base(width,height)
        {
            
            Movement m = (Movement)r.Next(4);

            switch (m) //Indica onde o labirinto começa, se no lado esq, cima, baixo, ou dir
            {
                case Movement.goUp: //começa em cima
                    ProcessMaze(new Point(r.Next(width), 0), Movement.goUp, 0);
                    break;
                case Movement.goDown: //começa em baixo
                    ProcessMaze(new Point(r.Next(width), height - 1), Movement.goDown, 0);
                    break;
                case Movement.goLeft://começa na esquerda
                    ProcessMaze(new Point(0, r.Next(height)), Movement.goLeft, 0);
                    break;
                case Movement.goRight://começa na direita
                    ProcessMaze(new Point(width - 1, r.Next(height)), Movement.goRight, 0);
                    break;
            }
        }

        /// <summary>
        /// Generates the maze
        /// </summary>
        /// <param name="currentPoint"></param>
        /// <param name="direction"></param>
        /// <param name="currentNumProc">Tells you the number of sequential cell processing, the largest number is the finish point</param>
        public void ProcessMaze(Point currentPoint, Movement direction, int currentNumProc)
        {
            MazeCell currentCell = maze[currentPoint.X, currentPoint.Y]; //usando a currentCell é uma forma mais facil de aceder à celula, em vez de aceder ao array, criando uma linha de codigo muito grande


            if (currentNumProc > Maze_Recursive_Backtracker.numMaxProc)
            {
                Maze_Recursive_Backtracker.numMaxProc = currentNumProc;
                Maze_Recursive_Backtracker.finishCellPoint = currentPoint;
            }

            if (Maze_Recursive_Backtracker.isStart) //quando o labirinto é chamado pela primeira vez este if é executado
            {
                Maze_Recursive_Backtracker.initCellPoint = currentPoint; //indica qual é a posicao inicial estaticamente

                currentCell.isInit = true;
                currentCell.isVisited = true;


                switch (direction) //parte a parede que server de entrada
                {
                    case Movement.goUp:
                        currentCell.wallUp = false;
                        break;
                    case Movement.goDown:
                        currentCell.wallDown = false;
                        break;
                    case Movement.goLeft:
                        currentCell.wallLeft = false;
                        break;
                    case Movement.goRight:
                        currentCell.wallRight = false;
                        break;
                }

                maze[Maze_Recursive_Backtracker.initCellPoint.X, Maze_Recursive_Backtracker.initCellPoint.Y] = currentCell;

                Maze_Recursive_Backtracker.isStart = false;

            }

            //Parte a parede para que haja ligaçao, ligando 2 celulas
            else
            {
                switch (direction)
                {
                    case Movement.goUp:
                        currentCell.wallDown = false;
                        maze[currentPoint.X, currentPoint.Y + 1].wallUp = false;
                        break;
                    case Movement.goDown:
                        currentCell.wallUp = false;
                        maze[currentPoint.X, currentPoint.Y - 1].wallDown = false;
                        break;
                    case Movement.goLeft:
                        currentCell.wallRight = false;
                        maze[currentPoint.X + 1, currentPoint.Y].wallLeft = false;
                        break;
                    case Movement.goRight:
                        currentCell.wallLeft = false;
                        maze[currentPoint.X - 1, currentPoint.Y].wallRight = false;
                        break;

                }
                currentCell.isVisited = true;
                maze[currentPoint.X, currentPoint.Y] = currentCell;
            }//else 


            #region Boundaries check
            //Vê se está perto do limite, pondo o try em true nao deixa que aceda ao elemento out of bounds
            if (currentPoint.X == 0)
            {
                currentCell.tryLeft = true;
            }
            else if (currentPoint.X == maze.GetLength(0) - 1)
            {
                currentCell.tryRight = true;
            }

            if (currentPoint.Y == 0)
            {
                currentCell.tryUp = true;
            }
            else if (currentPoint.Y == maze.GetLength(1) - 1)
            {
                currentCell.tryDown = true;
            }
            #endregion

            maze[currentPoint.X, currentPoint.Y] = currentCell;

            while (true) //cicle que vai correr ate todos os 4 lados da celula tenham sido visitadas, só depois é que volta à funçao recursiva anterior
            {

                #region Verifica Adjacentes
                if (currentPoint.X != 0) //verifica celulas adjacentes
                {
                    if (maze[currentPoint.X - 1, currentPoint.Y].isVisited)
                    {
                        currentCell.tryLeft = true;
                        maze[currentPoint.X - 1, currentPoint.Y].tryRight = true;
                    }
                }

                if (currentPoint.X != maze.GetLength(0) - 1)
                {
                    if (maze[currentPoint.X + 1, currentPoint.Y].isVisited)
                    {
                        currentCell.tryRight = true;
                        maze[currentPoint.X + 1, currentPoint.Y].tryLeft = true;
                    }
                }

                if (currentPoint.Y != 0)
                {
                    if (maze[currentPoint.X, currentPoint.Y - 1].isVisited)
                    {
                        currentCell.tryUp = true;
                        maze[currentPoint.X, currentPoint.Y - 1].tryDown = true;
                    }
                }

                if (currentPoint.Y != maze.GetLength(1) - 1)
                {
                    if (maze[currentPoint.X, currentPoint.Y + 1].isVisited)
                    {
                        currentCell.tryDown = true;
                        maze[currentPoint.X, currentPoint.Y + 1].tryUp = true;
                    }
                }
                #endregion

                maze[currentPoint.X, currentPoint.Y] = currentCell;


                if (currentCell.tryUp && currentCell.tryDown && currentCell.tryLeft && currentCell.tryRight)
                {//Se todas as células à volta foram processadas então a cecula onde está agora é a final
                    currentCell.isFinal = true;

                    maze[currentPoint.X, currentPoint.Y] = currentCell;
                    return;
                }
                else // se não for entao gera um numero e tenta verificar se a celula foi visitada ou nao
                {
                    int dir = r.Next(0, 4);
                    switch (dir)
                    {
                        case 0:
                            if (currentCell.tryUp == false)
                            {
                                currentCell.tryUp = true;
                                maze[currentPoint.X, currentPoint.Y] = currentCell;
                                ProcessMaze(new Point(currentPoint.X, currentPoint.Y - 1), Movement.goUp, currentNumProc + 1);
                            }
                            break;

                        case 1:
                            if (currentCell.tryDown == false)
                            {
                                currentCell.tryDown = true;
                                maze[currentPoint.X, currentPoint.Y] = currentCell;
                                ProcessMaze(new Point(currentPoint.X, currentPoint.Y + 1), Movement.goDown, currentNumProc + 1);
                            }
                            break;

                        case 2:
                            if (currentCell.tryLeft == false)
                            {
                                currentCell.tryLeft = true;
                                maze[currentPoint.X, currentPoint.Y] = currentCell;
                                ProcessMaze(new Point(currentPoint.X - 1, currentPoint.Y), Movement.goLeft, currentNumProc + 1);
                            }
                            break;

                        case 3:
                            if (currentCell.tryRight == false)
                            {
                                currentCell.tryRight = true;
                                maze[currentPoint.X, currentPoint.Y] = currentCell;
                                ProcessMaze(new Point(currentPoint.X + 1, currentPoint.Y), Movement.goRight, currentNumProc + 1);
                            }
                            break;

                    }//switch
                }//else
            }//while
        }//ProcessMaze
    }
}
