using Pr4.Classes;
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

namespace Pr4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public bool IsWhiteTurn = true;
        public List<Classes.Pawn> Pawns = new List<Classes.Pawn>();
        public List<Classes.Pawn> Queen = new List<Classes.Pawn>();

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.mainWindow = this;

            Pawns.Add(new Classes.Pawn(0, 1, false, false));
            Pawns.Add(new Classes.Pawn(1, 1, false, false));
            Pawns.Add(new Classes.Pawn(2, 1, false, false));
            Pawns.Add(new Classes.Pawn(3, 1, false, false));
            Pawns.Add(new Classes.Pawn(4, 1, false, false));
            Pawns.Add(new Classes.Pawn(5, 1, false, false));
            Pawns.Add(new Classes.Pawn(6, 1, false, false));
            Pawns.Add(new Classes.Pawn(7, 1, false, false));

            Pawns.Add(new Classes.Pawn(0, 6, true, false));
            Pawns.Add(new Classes.Pawn(1, 6, true, false));
            Pawns.Add(new Classes.Pawn(2, 6, true, false));
            Pawns.Add(new Classes.Pawn(3, 6, true, false));
            Pawns.Add(new Classes.Pawn(4, 6, true, false));
            Pawns.Add(new Classes.Pawn(5, 6, true, false));
            Pawns.Add(new Classes.Pawn(6, 6, true, false));
            Pawns.Add(new Classes.Pawn(7, 6, true, false));

            Queen.Add(new Classes.Pawn(3, 0, false, true));

            Queen.Add(new Classes.Pawn(3, 7, true, true));

            CreateFigure();
        }

        public void CreateFigure()
        {
            foreach (Classes.Pawn Pawn in Pawns)
            {
                Pawn.Figure = new Grid()
                {
                    Width = 50,
                    Height = 50
                };
                if (Pawn.Black)
                {
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                }
                else
                {
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));
                }
                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }
            foreach (Classes.Pawn Pawn in Queen)
            {
                Pawn.Figure = new Grid()
                {
                    Width = 50,
                    Height = 50
                };
                if (Pawn.Black)
                {
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (black).png")));
                }
                else
                {
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (white).png")));
                }
                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }
        }

        public void OnSelect(Classes.Pawn SelectPawn, Classes.Pawn SelectQueen)
        {
            ResetHighlights();

            foreach (Classes.Pawn Pawn in Pawns)
            {
                if (Pawn != SelectPawn && Pawn.Select)
                {
                    Pawn.Select = false;
                    if (Pawn.Black)
                        Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                    else
                        Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));
                }
            }

            foreach (Classes.Pawn Pawn in Queen)
            {
                if (Pawn != SelectQueen && Pawn.Select)
                {
                    Pawn.Select = false;
                    if (Pawn.Black)
                        Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (black).png")));
                    else
                        Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (white).png")));
                }
            }

            if (SelectQueen != null && SelectQueen.Select)
            {
                SelectQueen.HighlightQueenMoves();
            }
            else if (SelectPawn != null && SelectPawn.Select)
            {
                SelectPawn.HighlightPawnMoves();
            }
        }

        private void SelectTile(object sender , MouseButtonEventArgs e)
        {
            Grid Tile = sender as Grid;
            int X = Grid.GetColumn(Tile);
            int Y = Grid.GetRow(Tile);
            Classes.Pawn SelectPawn = Pawns.Find(x => x.Select == true);
            Classes.Pawn SelectQueen = Queen.Find(x => x.Select == true);
            if (SelectPawn != null)
            {
                SelectPawn.TransformPawns(X, Y);
            }
            else if (SelectQueen != null)
            {
                SelectQueen.TransformQueen(X, Y);
            }
            else
            {
                ResetHighlights();
            }
        }

        // Сброс подсветки
        public void ResetHighlights()
        {
            var highlights = gameBoard.Children.OfType<Border>().Where(b => b.Name == "Highlight").ToList();
            foreach (var highlight in highlights)
            {
                gameBoard.Children.Remove(highlight);
            }
        }

        // Проверка вражеской фигуры
        public bool IsEnemy(int x, int y, bool isBlack)
        {
            return Pawns.Any(p => p.X == x && p.Y == y && p.Black != isBlack) ||
                   Queen.Any(q => q.X == x && q.Y == y && q.Black != isBlack);
        }

        public bool IsCellOccupied(int x, int y)
        {
            return Pawns.Any(p => p.X == x && p.Y == y) ||
                   Queen.Any(q => q.X == x && q.Y == y);
        }

        public void HighlightCell(int x, int y, SolidColorBrush color)
        {
            var highlight = new Border()
            {
                Name = "Highlight",
                Background = color,
                Width = 50,
                Height = 50,
                IsHitTestVisible = false
            };

            Grid.SetColumn(highlight, x);
            Grid.SetRow(highlight, y);
            gameBoard.Children.Add(highlight);
        }


    }
}
