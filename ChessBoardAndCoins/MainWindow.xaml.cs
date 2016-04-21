using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessBoardAndCoins
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double _CHESS_POLE_SIZE = 70;
        const double _COIN_RADIUS = _CHESS_POLE_SIZE - _CHESS_POLE_SIZE / 6;
        const double _COIN_POS_MODIFIER = _COIN_RADIUS / 12.1;

        const double _CHESS_BOARD_STARTING_POSITION_X = 0;
        const double _CHESS_BOARD_STARTING_POSITION_Y = 0;

        const double _DISTANCE_BETWEEN_BOARDS = _CHESS_POLE_SIZE * 1.5;

        const double _CHESS_BOARD_SIZE = _CHESS_POLE_SIZE * 8;

        const double _SECOND_BOARD_STARTING_POSITION_X = _CHESS_BOARD_SIZE + _DISTANCE_BETWEEN_BOARDS;
        const double _SECOND_BOARD_STARTING_POSITION_Y = _CHESS_BOARD_STARTING_POSITION_Y;

        public MainWindow()
        {
            InitializeComponent();

            this.Width = _SECOND_BOARD_STARTING_POSITION_X + _CHESS_BOARD_SIZE + 200;
            this.Height = _CHESS_BOARD_SIZE + _CHESS_POLE_SIZE * 1 + 150;
            
            DrawChessBoard(new Point { X = _CHESS_BOARD_STARTING_POSITION_X, Y = _CHESS_BOARD_STARTING_POSITION_Y });
            DrawChessBoard(new Point { X = _SECOND_BOARD_STARTING_POSITION_X, Y = _SECOND_BOARD_STARTING_POSITION_Y });

            

            
            DrawRandomCoins();
            SetUpUI();


            //SetUpLabels();

            DrawTargetPole(new Point { X = 0, Y = 0 });
        }


        Point BoardPoleToPoint(int poleIndex)
        {
            var x = (poleIndex % 8) * _CHESS_POLE_SIZE + _SECOND_BOARD_STARTING_POSITION_X;
            var y = (poleIndex / 8) * _CHESS_POLE_SIZE;

            return new Point
            {
                Y = y,
                X = x
            };
        }

        int coinsAdded = 0;
        int canvasShift = 0;
        void DrawRandomCoins()
        {
            canvas.Children.RemoveRange(128, coinsAdded);
            coinsAdded = 0;
            SetUpEllipsesArray();

            var rnd = new Random();

            //bool newX;
            //bool newY;

            for (int poleIndex = 0; poleIndex < 64; poleIndex++)
            {
                //newX = position.X + i * _CHESS_POLE_SIZE;
                //newY = position.Y + j * _CHESS_POLE_SIZE;

                var pos = BoardPoleToPoint(poleIndex);
                
                if(rnd.Next(2) == 1)//рисуем монету
                {
                    //if (ellipsesIndexesInCanvasArray[poleIndex] != -1)//монета уже есть, ничего не делаем
                    //    continue;

                    DrawCoin(BoardPoleToPoint(poleIndex));

                    var indexOfDrawnCoinInCanvas = canvas.Children.Count - 1;

                    ellipsesIndexesInCanvasArray[poleIndex] = indexOfDrawnCoinInCanvas;

                    coinsAdded++;
                }
                //else//убираем монету
                //{
                //    if (ellipsesIndexesInCanvasArray[poleIndex] == -1)//монеты и так нет, ничего не делаем
                //        continue;

                //    canvas.Children.RemoveAt(ellipsesIndexesInCanvasArray[poleIndex] - canvasShift);

                //    ellipsesIndexesInCanvasArray[poleIndex] = -1;

                //    //canvasShift++;
                //}
            }
        }


        Label targetBinLabel;
        Label targetDecLabel;

        Label toFlipDecLabel;
        Label toFlipBinLabel;

        Label boardStateBinLabel;

        Button clearButton;
        Button randomButton;
        void SetUpUI()
        {
            
            //Цель = 010101
            targetBinLabel = new Label();
            //targetBinLabel.Content = DecToBin(0);
            targetBinLabel.Content = DecToBin(0);
            targetBinLabel.FontSize = 60;
            targetBinLabel.Margin = new Thickness(0, _CHESS_BOARD_SIZE, 0, 0); //left, top, right, bottom

            
            MainWindow_Grid.Children.Add(targetBinLabel);

            //Цель = 42
            targetDecLabel = new Label();
            //targetDecLabel.Content = String.Format("Цель = {0}", 0);
            targetDecLabel.Content = String.Format("Цель = {0}", 0);
            targetDecLabel.Foreground = new SolidColorBrush(Colors.Red);
            targetDecLabel.FontSize = 60;
            targetDecLabel.Margin = new Thickness(_CHESS_BOARD_SIZE / 2, _CHESS_BOARD_SIZE, 0, 0); //left, top, right, bottom


            MainWindow_Grid.Children.Add(targetDecLabel);


            var boardState = GetBoardState();
            //Состояние = 101001
            boardStateBinLabel = new Label();
            boardStateBinLabel.Content = String.Format("{0} (Состояние = {1})", DecToBin(boardState), boardState);
            boardStateBinLabel.FontSize = 60;
            boardStateBinLabel.Margin = new Thickness(_SECOND_BOARD_STARTING_POSITION_X, _CHESS_BOARD_SIZE, 0, 0);//left, top, right, bottom


            MainWindow_Grid.Children.Add(boardStateBinLabel);



            toFlipBinLabel = new Label();
            toFlipBinLabel.Foreground = new SolidColorBrush(Colors.Green);
            toFlipBinLabel.FontSize = 60;
            toFlipBinLabel.Margin = new Thickness(boardStateBinLabel.Margin.Left, boardStateBinLabel.Margin.Top + 60, 0, 0);
            toFlipBinLabel.Content = String.Format("{0} (Перевернуть = {1})", DecToBin(boardState ^ currentTargetPole), boardState ^ currentTargetPole);

            MainWindow_Grid.Children.Add(toFlipBinLabel);



            //Состояние = 42
            //toFlipDecLabel = new Label();
            //toFlipDecLabel.Content = String.Format("Перевернуть = {0}", boardState ^ currentTargetPole);
            //toFlipDecLabel.Foreground = new SolidColorBrush(Colors.Green);
            //toFlipDecLabel.FontSize = 60;
            //toFlipDecLabel.Margin = new Thickness(_SECOND_BOARD_STARTING_POSITION_X + _CHESS_BOARD_SIZE / 2, _CHESS_BOARD_SIZE, 0, 0);//left, top, right, bottom


            //MainWindow_Grid.Children.Add(toFlipDecLabel);




            //TODO: кнопки "очистить" и "рандом"

            //кнопка очистить
            clearButton = new Button();
            clearButton.Content = "Очистить";
            clearButton.FontSize = 35;
            clearButton.Height = 100;
            clearButton.Margin = new Thickness(_SECOND_BOARD_STARTING_POSITION_X + _CHESS_BOARD_SIZE, 0, 0, 0);
            clearButton.Click += ClearButton_Click;

            MainWindow_Grid.Children.Add(clearButton);




            //кнопка рандом
            randomButton = new Button();
            randomButton.Content = "Случайно";
            randomButton.FontSize = 35;
            randomButton.Height = 100;
            randomButton.Margin = new Thickness(clearButton.Margin.Left, clearButton.Margin.Top - 400, 0, 0);
            randomButton.Click += RandomButton_Click;

            MainWindow_Grid.Children.Add(randomButton);

            DrawFlipPole(boardState ^ currentTargetPole);
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            DrawRandomCoins();

            RecalculateFields();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveRange(128, coinsAdded);
            coinsAdded = 0;
            SetUpEllipsesArray();

            RecalculateFields();
        }

        void FlipCoin(Point pos)
        {
            var pole = PointToBoardPole(pos);

            if(ellipsesIndexesInCanvasArray[pole.PoleIndex] == -1)//если нет монетки, то ставим
            {
                DrawCoin(BoardPoleToPoint(pole.PoleIndex));

                var indexOfDrawnCoinInCanvas = canvas.Children.Count - 1;

                ellipsesIndexesInCanvasArray[pole.PoleIndex] = indexOfDrawnCoinInCanvas;

                coinsAdded++;
            }
            else//если есть, то удаляем и сдвигаем все индексы после удалённого
            {
                var toRemove = ellipsesIndexesInCanvasArray[pole.PoleIndex];

                var test = canvas.Children.Count;

                canvas.Children.RemoveAt(toRemove);
                ellipsesIndexesInCanvasArray[pole.PoleIndex] = -1;

                for (int i = 0; i < ellipsesIndexesInCanvasArray.Count(); i++)
                {
                    if (ellipsesIndexesInCanvasArray[i] != -1) //сдвигаем индексы только если не -1
                        if(ellipsesIndexesInCanvasArray[i] > toRemove) //и только те, что идут в списке (List) после удалённого
                            ellipsesIndexesInCanvasArray[i]--;
                }

                coinsAdded--;
            }

            var boardState = GetBoardState();
            DrawFlipPole(boardState ^ currentTargetPole);
            RecalculateFields();
        }



        int GetBoardState()
        {
            var bits = "";
            var coinsCountInGroups = new int[6];
            for (int poleIndex = 1; poleIndex < 64; poleIndex++)
            {
                if (ellipsesIndexesInCanvasArray[poleIndex] != -1)//если есть монета
                {
                    for(int bit = 0; bit < coinsCountInGroups.Count(); bit++)//находим какой из битов есть в индексе
                    {
                        if(((int)Math.Pow(2, bit) & poleIndex) != 0)
                        {
                            coinsCountInGroups[bit]++;
                        }
                    }
                }
            }


            for(int bit = coinsCountInGroups.Length - 1; bit >= 0; bit--)
            {
                if (coinsCountInGroups[bit] % 2 == 0)
                    bits += "0";
                else
                    bits += "1";
            }

            return Convert.ToInt16(bits, 2);
        }


        string DecToBin(int decNum)
        {
            return Convert.ToString(decNum, 2).PadLeft(6, '0');
        }

        int oldFlipPole = -1;
        int currentFlipPole = -1;
        int oldTargetPole = -1;
        int currentTargetPole = 0;

        void DrawTargetPole(Point pos)
        {
            var pole = PointToBoardPole(pos);

            if (pole.IsTargetBoard)
            {
                if (oldTargetPole != pole.PoleIndex)//если тыкаем не в ту же клетку
                {
                    if(oldTargetPole != -1)
                        ((Rectangle)canvas.Children[oldTargetPole]).StrokeThickness = 0;
                }
                else
                {
                    return;
                }

                oldTargetPole = pole.PoleIndex;
                currentTargetPole = pole.PoleIndex;

                ((Rectangle)canvas.Children[pole.PoleIndex]).Stroke = Brushes.Red;
                ((Rectangle)canvas.Children[pole.PoleIndex]).StrokeThickness = 4;

                targetBinLabel.Content = DecToBin(pole.PoleIndex);
                targetDecLabel.Content = String.Format("Цель = {0}", pole.PoleIndex);
            }

            RecalculateFields();
            DrawFlipPole(GetBoardState() ^ currentTargetPole);
        }



        void DrawFlipPole(int poleIndex)
        {
            var pos = BoardPoleToPoint(poleIndex);
            var pole = PointToBoardPole(pos);
                
            if(oldFlipPole != -1)
                ((Rectangle)canvas.Children[oldFlipPole + 64]).StrokeThickness = 0;

            oldFlipPole = poleIndex;

            oldFlipPole = pole.PoleIndex;
            currentFlipPole = pole.PoleIndex;

            ((Rectangle)canvas.Children[pole.PoleIndex + 64]).Stroke = Brushes.Green;
            ((Rectangle)canvas.Children[pole.PoleIndex + 64]).StrokeThickness = 4;

            //targetBinLabel.Content = DecToBin(pole.PoleIndex);
            //targetDecLabel.Content = String.Format("Цель = {0}", pole.PoleIndex);
        }


        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas);

            if (!IsCursorAboveBoard(pos))
                return;

            if (pos.X < _SECOND_BOARD_STARTING_POSITION_X)
                DrawTargetPole(pos);
            else
                FlipCoin(pos);

            //var pole = PointToBoardPole(pos);

            //if(pole.IsTargetBoard)
            //{
            //    if (oldTargetPole != -1)
            //    {
            //        ((Rectangle)canvas.Children[oldTargetPole]).StrokeThickness = 0;
            //    }

            //    oldTargetPole = pole.PoleIndex;
            //    currentTargetPole = pole.PoleIndex;

            //    ((Rectangle)canvas.Children[pole.PoleIndex]).Stroke = Brushes.Red;
            //    ((Rectangle)canvas.Children[pole.PoleIndex]).StrokeThickness = 4;

            //    targetBinLabel.Content = DecToBin(pole.PoleIndex);
            //    targetDecLabel.Content = String.Format("Цель = {0}", pole.PoleIndex);
            //}
        }


        private Point startPoint;
        private Rectangle rect;
        private void DrawChessSquare(double sideLength, bool isWhite, Point position)
        {
            rect = new Rectangle
            {
                Fill = isWhite ? Brushes.White : Brushes.Black,             
            };

            rect.Width = sideLength;
            rect.Height = sideLength;

            Canvas.SetLeft(rect, position.X);
            Canvas.SetTop(rect, position.Y);
            canvas.Children.Add(rect);

            rect = null;
        }



        private void DrawChessBoard(Point position)
        {
            bool isWhite = false;

            double newX;
            double newY;

            for(int j = 0; j < 8; j++)
            {
                isWhite = !isWhite;

                for (int i = 0; i < 8; i++)
                {
                    isWhite = !isWhite;

                    newX = position.X + i * _CHESS_POLE_SIZE;
                    newY = position.Y + j * _CHESS_POLE_SIZE;

                    DrawChessSquare
                        (
                        _CHESS_POLE_SIZE,
                        isWhite,
                        new Point
                        {
                            X = newX,
                            Y = newY
                        });
                }
            }
                
        }





        void SetUpEllipsesArray()
        {
            for(int i = 0; i < ellipsesIndexesInCanvasArray.Length; i++)
            {
                ellipsesIndexesInCanvasArray[i] = -1;
            }
        }




        Ellipse ellipse;
        int[] ellipsesIndexesInCanvasArray = new int[64];
        private void DrawCoin(Point position)
        {
            ellipse = new Ellipse
            {
                Fill = Brushes.Gold,
                //Stroke = isWhite ? Brushes.White : Brushes.Black,
                //IsHitTestVisible = true,
                Stroke = Brushes.Sienna,
                StrokeThickness = 2                
            };

            ellipse.Width = _COIN_RADIUS;
            ellipse.Height = _COIN_RADIUS;

            Canvas.SetLeft(ellipse, position.X + _COIN_POS_MODIFIER);
            Canvas.SetTop(ellipse, position.Y + _COIN_POS_MODIFIER);
            canvas.Children.Add(ellipse);

            ellipse = null;
        }




        private static bool IsCursorAboveBoard(Point pos)
        {
            if (pos.X > _CHESS_BOARD_STARTING_POSITION_X + _CHESS_BOARD_SIZE
                && pos.X < _SECOND_BOARD_STARTING_POSITION_X)
            {
                return false;
            }


            if (pos.Y > _CHESS_BOARD_SIZE)
            {
                return false;
            }

            return true;
        }


        
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //var pos = e.GetPosition(canvas);
            //var pole = PointToBoardPole(pos);

            //if (!IsCursorAboveBoard(pos))
            //{
            //    this.Title = "Курсор вне досок.";
            //    return;
            //}

            //this.Title = String.Format("Мышь: X = {0}  Y= {1};  Доска = {2}  Клетка = {3}: X = {4}  Y = {5}",
            //    pos.X,
            //    pos.Y,
            //    pole.IsTargetBoard? "с указателем" : "с монетами",
            //    pole.PoleIndex,
            //    pole.X,
            //    pole.Y);
        }

        BoardPole PointToBoardPole(Point point)
        {
            double compensator;
            bool isTargetBoard;

            if(point.X >= _SECOND_BOARD_STARTING_POSITION_X)
            {
                compensator = -_SECOND_BOARD_STARTING_POSITION_X;
                isTargetBoard = false;
            }
            else
            {
                compensator = 0;
                isTargetBoard = true;
            }

            var xRounded = ((point.X + compensator) / (int)_CHESS_POLE_SIZE) * _CHESS_POLE_SIZE;
            var yRounded = (point.Y / (int)_CHESS_POLE_SIZE) * _CHESS_POLE_SIZE;

            var xCoord = xRounded / _CHESS_POLE_SIZE;
            var yCoord = (int)yRounded / _CHESS_POLE_SIZE;

            var poleIndex = (int)xCoord + 8 * (int)yCoord;

            return new BoardPole{ PoleIndex = poleIndex, IsTargetBoard = isTargetBoard, X = (int)xCoord, Y = (int)yCoord };
        }


        void RecalculateFields()
        {
            var boardState = GetBoardState();

            //toFlipDecLabel.Content = String.Format("{0}", boardState ^ currentTargetPole);//Перевернуть = 
            boardStateBinLabel.Content = String.Format("{0} (Состояние = {1})", DecToBin(boardState), boardState);
            toFlipBinLabel.Content = String.Format("{0} (Перевернуть = {1})", DecToBin(boardState ^ currentTargetPole), boardState ^ currentTargetPole);

            DrawFlipPole(boardState ^ currentTargetPole);
        }
        

        public class BoardPole
        {
            public bool IsTargetBoard { get; set; }
            public int PoleIndex { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

    }
}
