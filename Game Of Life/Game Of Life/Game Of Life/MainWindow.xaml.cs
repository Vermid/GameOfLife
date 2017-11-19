using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Game_Of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int fieldSize = 1000;
        PlayGround playGround;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Inital()
        {
            fieldSize = Int32.Parse(FieldSizeTextBox.Text);
            int spawnRate = Int32.Parse(SpawnRateTextBox.Text);

            int coverRate = fieldSize * fieldSize / 100 * spawnRate;

            playGround = new PlayGround(fieldSize, coverRate);
            DrawField(fieldSize);
        }

        private void UpdateField()
        {
            //update field and creature
            playGround.CheckNeighbor();
            playGround.CheckPlayGround();
            DrawField(fieldSize);
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (playGround != null)
            {
                playGround = null;
            }
            myCanvas.Children.Clear();
            dispatcherTimer.Stop();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Inital();
            StartTimer();
        }

        private void DrawField(int size)
        {
            myCanvas.Children.Clear(); // check -> wegen memoryleak nötig

            var rectangle = new Rectangle();
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 1;

            rectangle.Height = 1000;
            rectangle.Width = 1000;
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, 0);
            myCanvas.Children.Add(rectangle);

            int x = 0;
            int y = 0;
            //draw the field here
            for (int i = 0; i < fieldSize; i++)
            {
                var line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;

                x += 10;
                line.X1 = 0;
                line.X2 = 1000;
                line.Y1 = x;
                line.Y2 = x;
                myCanvas.Children.Add(line);
            }

            for (int i = 0; i < fieldSize; i++)
            {
                var line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;

                y += 10;
                line.X1 = y;
                line.X2 = y;
                line.Y1 = 0;
                line.Y2 = 1000;
                myCanvas.Children.Add(line);
            }

            x = 0;
            for (int i = 0; i < fieldSize; i++)
            {
                y = 0;
                for (int j = 0; j < fieldSize; j++)
                {
                    if (playGround.gameBoard[i, j] != null)
                    {
                        if (playGround.gameBoard[i, j].IsAlive)
                        {
                            rectangle = new Rectangle();
                            rectangle.Stroke = Brushes.Red;
                            rectangle.StrokeThickness = 3;

                            rectangle.Height = 10;
                            rectangle.Width = 10;
                            Canvas.SetLeft(rectangle, x);
                            Canvas.SetTop(rectangle, y);
                            myCanvas.Children.Add(rectangle);
                        }

                        else
                        {
                            rectangle = new Rectangle();
                            rectangle.Stroke = Brushes.White;
                            rectangle.StrokeThickness = 3;

                            rectangle.Height = 10;
                            rectangle.Width = 10;
                            Canvas.SetLeft(rectangle, x);
                            Canvas.SetTop(rectangle, y);
                            myCanvas.Children.Add(rectangle);
                            playGround.gameBoard[i, j] = null; // check, daten dürfen beim  zeichnen nicht verändert werden, dass muss in der logik passieren
                        }

                    }
                    y += 10;
                }
                x += 10;
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateField();
        }


        private void StartTimer()
        {
            dispatcherTimer.Interval = new TimeSpan(10);
            dispatcherTimer.Start();
        }


        private void SingleStepButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            UpdateField();
        }

        private void StopPauseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
