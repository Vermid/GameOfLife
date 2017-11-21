using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        string myPath = $"{ Environment.GetFolderPath(Environment.SpecialFolder.Desktop) }\\GameOfLife.txt";

        //dont like this try to change it
        private int _y;
        private int _x;

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
                MessageBox.Show("Please use Numbers nothing else");
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
            StopTimer();
            Inital();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

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

        private void StopTimer()
        {
            dispatcherTimer.Stop();
        }

        private void SingleStepButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            UpdateField();
        }

        private void StopPauseButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }

        private void DrawPlayeGround()
        {
            //you need this or you matrix want be drawn
            _x = 0;
            _y = 0;

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
            StopTimer();

            if (File.Exists(myPath))
            {
                File.Delete(myPath);
            }

            StringBuilder myString = new StringBuilder();

            myString.AppendLine($"pixelSize:{pixelSize} | fieldSize:{fieldSize}");

            for (int y = 0; y < fieldSize; y++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    if (playGround.gameBoard[y, x] != null)
                    {
                        myString.AppendLine($"PosY:{y} | PosX:{x}");
                    }
                }
            }

            File.WriteAllText(myPath, myString.ToString());
            MessageBox.Show("Game Saved!! \n Save Path" + myPath);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();

            //maybe you will need here a exception because of the save path.
            // if there is no file to load 
            string[] lines = File.ReadAllLines(myPath);
            string[] poslines;

            if (playGround != null)
            {
                // clear the current field and array 
                myCanvas.Children.Clear();
                //clear the field before you load a old one
                Array.Clear(playGround.gameBoard, 0, playGround.gameBoard.Length);
            }

            foreach (string line in lines)
            {
                poslines = line.Split('|');

                if (line.Contains("PosX"))
                {
                    if (poslines.Length > 1)
                    {
                        try
                        {
                            int x = Convert.ToInt32(poslines[0].Remove(0, 5));
                            int y = Convert.ToInt32(poslines[1].Remove(0, 6));
                            playGround.CreateNewCreature(y, x);
                        }
                        catch (Exception ex)
                        {
                            // save the exception info into a logfile to check what happens 
                            // databse should not allow to change this settings
                        }
                    }
                }
                else
                {
                    try
                    {
                        int pixel = Convert.ToInt32(poslines[0].Remove(0, 10));
                        int field = Convert.ToInt32(poslines[1].Remove(0, 11));

                        pixelSize = pixel;
                        fieldSize = field;
                        playGround = new PlayGround(fieldSize);
                    }
                    catch (Exception ex)
                    {
                        // save the exception info into a logfile to check what happens 
                        // databse should not allow to change this settings
                        pixelSize = 10;
                        fieldSize = 100;
                    }
                }
            }
            DrawPlayeGround();
        }
    }
}
