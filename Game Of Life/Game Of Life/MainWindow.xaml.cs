using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
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
        private int pixelSize = 10;
        PlayGround playGround;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Canvas newCanvas = new Canvas();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Inital()
        {
            try
            {
                pixelSize = Int32.Parse(CreatureSize.Text);

                fieldSize = Int32.Parse(FieldSizeTextBox.Text);

                int spawnRate = Int32.Parse(SpawnRateTextBox.Text);

                int coverRate = fieldSize * fieldSize / 100 * spawnRate;

                playGround = new PlayGround(fieldSize, coverRate);
                DrawPlayeGround();
            }
            catch (Exception e)
            {
                MessageBox.Show("Please use Numbers and not Words ");
            }
        }

        private void UpdateField()
        {
            //update field and creature
            playGround.CheckNeighbor();
            DrawPlayeGround();
        }
        private void UpdateUI()
        {
            GenerationTextBox.Text = playGround.GetGeneration().ToString();
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
            Inital();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateField();
            UpdateUI();
        }

        private void StartTimer()
        {
            try
            {
                int milliseconds = Int32.Parse(SecondsTextBox.Text);
                dispatcherTimer.Interval = new TimeSpan(milliseconds);
                dispatcherTimer.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Please use Numbers no Text for Seconds");
            }
        }

        private void SingleStepButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            UpdateField();
        }

        private void StopPauseButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void DrawPlayeGround()
        {

            _y = 0;
            _x = 0;
            myCanvas.Children.Clear();

            for (int i = 0; i < fieldSize; i++)
            {
                DrawFieldMatrix();

                for (int j = 0; j < fieldSize; j++)
                {
                    if (playGround.gameBoard[i, j] != null)
                    {
                        if (playGround.gameBoard[i, j].IsAlive)
                        {
                            DrawCreatures(i * pixelSize, j * pixelSize);
                        }
                    }
                }
            }
            DrawFieldMatrix();
        }

        private void DrawCreatures(int y, int x)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = Brushes.Red;

            rectangle.Height = pixelSize;
            rectangle.Width = pixelSize;

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);

            myCanvas.Children.Add(rectangle);
        }

        private int _y;
        private int _x;

        private void DrawFieldMatrix()
        {
            Line line;
            //draw the field here
            line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 1;

            //start Pos
            line.X1 = 0;
            line.Y1 = _x;
            //end Pos
            line.X2 = fieldSize * pixelSize;
            line.Y2 = _x;
            myCanvas.Children.Add(line);
            _x += pixelSize;

            line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 1;

            //start Pos
            line.X1 = _y;
            line.Y1 = 0;
            //end Pos
            line.X2 = _y;
            line.Y2 = fieldSize * pixelSize;
            myCanvas.Children.Add(line);

            _y += pixelSize;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string myPath = $"{ Environment.GetFolderPath(Environment.SpecialFolder.Desktop) }\\GameOfLife.txt";

            if (File.Exists(myPath))
            {
                File.Delete(myPath);
            }

            StringBuilder myString = new StringBuilder();

            for (int i = 0; i < fieldSize; i++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    myString.AppendLine((playGround.gameBoard[i, x]);

                }
            }
            Console.WriteLine(playGround.gameBoard.ToString());

            //    File.WriteAllText(myPath, playGround.gameBoard.ToString());
            MessageBox.Show("Game Saved!! \n Save Path" + myPath);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
