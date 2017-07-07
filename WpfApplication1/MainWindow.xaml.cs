using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void FillCanvas(Canvas canvas, MazeGen.Maze m)
        {

            
            int size = 25;

            Line l;
            Line l2 = new Line();

            l2.X1 = MazeGen.Maze_Recursive_Backtracker.finishCellPoint.X * size;
            l2.X2 = MazeGen.Maze_Recursive_Backtracker.finishCellPoint.X * size + size;

            l2.Y1 = MazeGen.Maze_Recursive_Backtracker.finishCellPoint.Y * size;
            l2.Y2 = MazeGen.Maze_Recursive_Backtracker.finishCellPoint.Y * size + size;
            l2.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(l2);

            for (int x = 0; x < m.maze.GetLength(0); x++)
            {
                for (int y = 0; y < m.maze.GetLength(1); y++)
                {
                    l = new Line();
                    l.Stroke = System.Windows.Media.Brushes.Black;
                    l.StrokeThickness = 1;

                    if (m.maze[x, y].wallUp)
                    {
                        l.X1 = x * size;
                        l.Y1 = y * size;

                        l.X2 = x * size + size;
                        l.Y2 = y * size;
                        canvas.Children.Add(l);
                    }

                    l = new Line();
                    l.Stroke = System.Windows.Media.Brushes.Black;
                    l.StrokeThickness = 1;

                    if (m.maze[x, y].wallDown)
                    {
                        l.X1 = x * size;
                        l.Y1 = y * size + size;

                        l.X2 = x * size + size;
                        l.Y2 = y * size + size;
                        canvas.Children.Add(l);
                    }

                    l = new Line();
                    l.Stroke = System.Windows.Media.Brushes.Black;
                    l.StrokeThickness = 1;

                    if (m.maze[x, y].wallLeft)
                    {
                        l.X1 = x * size;
                        l.Y1 = y * size;

                        l.X2 = x * size;
                        l.Y2 = y * size + size;
                        canvas.Children.Add(l);
                    }

                    l = new Line();
                    l.Stroke = System.Windows.Media.Brushes.Black;
                    l.StrokeThickness = 1;

                    if (m.maze[x, y].wallRight)
                    {
                        l.X1 = x * size + size;
                        l.Y1 = y * size;

                        l.X2 = x * size + size;
                        l.Y2 = y * size + size;
                        canvas.Children.Add(l);
                    }
                } //for y
            }// for x
        }
        public MainWindow()
        {
            InitializeComponent();
            MazeGen.Maze_Recursive_Backtracker m = new MazeGen.Maze_Recursive_Backtracker(20, 20);
            //FillCanvas(canvas, m);
            int size = 100, thickness = size / 10 > 0 ? size / 10 : 1;
            
            MazeGen.MazeUtils.ExportToImage(@"D:/text.png", m, size, thickness);
        }

    }
}
