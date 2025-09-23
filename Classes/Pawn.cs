using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pr4.Classes
{
    public class Pawn
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public bool Select = false;
        public bool IsQueen = false;
        public bool Black = false;
        public Grid Figure {  get; set; }
        public Pawn(int X, int Y, bool Black, bool isQueen = false)
        {
            this.X = X;
            this.Y = Y;
            this.Black = Black;
            this.IsQueen = isQueen;
        }

        public void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            Pawn SelectPawn = MainWindow.mainWindow.Pawns.Find(x => x.Select == true);
            Pawn SelectQueen = MainWindow.mainWindow.Queen.Find(x => x.Select == true);

            if (SelectQueen != null && SelectQueen != this && this.Black != SelectQueen.Black)
            {
                int deltaX = Math.Abs(this.X - SelectQueen.X);
                int deltaY = Math.Abs(this.Y - SelectQueen.Y);

                if (deltaX == deltaY && deltaX > 0 || (deltaX == 0 && deltaY > 0) || (deltaY == 0 && deltaX > 0))
                {
                    if (SelectQueen.IsPathClearForCapture(this.X, this.Y))
                    {
                        if (this.IsQueen)
                            MainWindow.mainWindow.Queen.Remove(this);
                        else
                            MainWindow.mainWindow.Pawns.Remove(this);

                        MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);

                        Grid.SetColumn(SelectQueen.Figure, this.X);
                        Grid.SetRow(SelectQueen.Figure, this.Y);
                        SelectQueen.X = this.X;
                        SelectQueen.Y = this.Y;

                        SelectQueen.Select = false;
                        if (SelectQueen.Black)
                            SelectQueen.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (black).png")));
                        else
                            SelectQueen.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (white).png")));

                        MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
                        MainWindow.mainWindow.OnSelect(null, null);
                        MainWindow.mainWindow.ResetHighlights();
                        return;
                    }
                }
            }

            if (SelectPawn != null && SelectPawn != this && this.Black != SelectPawn.Black)
            {
                if ((SelectPawn.Black && SelectPawn.Y - 1 == this.Y && (SelectPawn.X - 1 == this.X || SelectPawn.X + 1 == this.X)) ||
                    (!SelectPawn.Black && SelectPawn.Y + 1 == this.Y && (SelectPawn.X - 1 == this.X || SelectPawn.X + 1 == this.X)))
                {
                    if (this.IsQueen)
                        MainWindow.mainWindow.Queen.Remove(this);
                    else
                        MainWindow.mainWindow.Pawns.Remove(this);

                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    Grid.SetColumn(SelectPawn.Figure, this.X);
                    Grid.SetRow(SelectPawn.Figure, this.Y);
                    SelectPawn.X = this.X;
                    SelectPawn.Y = this.Y;

                    SelectPawn.Select = false;
                    if (SelectPawn.Black)
                        SelectPawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                    else
                        SelectPawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));

                    MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
                    MainWindow.mainWindow.OnSelect(null, null);
                    return;
                }
            }

            if (!this.IsQueen)
            {
                if (this.Select)
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(this.Black ?
                        new Uri(@"pack://application:,,,/Images/Pawn (black).png") :
                        new Uri(@"pack://application:,,,/Images/Pawn.png")));
                    this.Select = false;
                    MainWindow.mainWindow.ResetHighlights();
                    MainWindow.mainWindow.OnSelect(null, null);
                }
                else
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (select).png")));
                    this.Select = true;
                    MainWindow.mainWindow.OnSelect(this, null);
                }
            }
            else
            {
                if (this.Select)
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(this.Black ?
                        new Uri(@"pack://application:,,,/Images/Queen (black).png") :
                        new Uri(@"pack://application:,,,/Images/Queen (white).png")));
                    this.Select = false;
                    MainWindow.mainWindow.ResetHighlights();
                    MainWindow.mainWindow.OnSelect(null, null);
                }
                else
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (select).png")));
                    this.Select = true;
                    MainWindow.mainWindow.OnSelect(null, this);
                }
            }
        }

        public void TransformPawns(int X,  int Y)
        {
            if (X != this.X || (this.Black && MainWindow.mainWindow.IsWhiteTurn) || (!this.Black && !MainWindow.mainWindow.IsWhiteTurn))
            {
                SelectFigure(null, null);
                return;
            }
            if (!Black && ((this.Y == 1 && this.Y + 2 == Y) || this.Y + 1 == Y) ||
                Black && ((this.Y == 6 && this.Y - 2 == Y) || this.Y - 1 == Y))
            {
                Grid.SetColumn(this.Figure, X);
                Grid.SetRow(this.Figure, Y);
                this.X = X;
                this.Y = Y;
                MainWindow.mainWindow.ResetHighlights();
                MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
            }
            SelectFigure(null, null);
        }

        public void TransformQueen(int X, int Y)
        {
            if ((this.Black && MainWindow.mainWindow.IsWhiteTurn) || (!this.Black && !MainWindow.mainWindow.IsWhiteTurn))
            {
                MainWindow.mainWindow.ResetHighlights();
                SelectFigure(null, null);
                return;
            }

            int deltaX = Math.Abs(X - this.X);
            int deltaY = Math.Abs(Y - this.Y);

            if (!(deltaX == 0 || deltaY == 0 || deltaX == deltaY))
            {
                MainWindow.mainWindow.ResetHighlights();
                SelectFigure(null, null);
                return;
            }

            if (!IsPathClearForCapture(X, Y))
            {
                MainWindow.mainWindow.ResetHighlights();
                SelectFigure(null, null);
                return;
            }

            if (MainWindow.mainWindow.Pawns.Any(p => p.X == X && p.Y == Y) ||
                MainWindow.mainWindow.Queen.Any(q => q.X == X && q.Y == Y))
            {
                MainWindow.mainWindow.ResetHighlights();
                SelectFigure(null, null);
                return;
            }

            Grid.SetColumn(this.Figure, X);
            Grid.SetRow(this.Figure, Y);
            this.X = X;
            this.Y = Y;

            MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
            this.Select = false;

            if (this.Black)
            {
                this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (black).png")));
            }
            else
            {
                this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (white).png")));
            }
            MainWindow.mainWindow.ResetHighlights();
        }

        private bool IsPathClearForCapture(int targetX, int targetY)
        {
            int deltaX = Math.Sign(targetX - this.X);
            int deltaY = Math.Sign(targetY - this.Y);

            int currentX = this.X + deltaX;
            int currentY = this.Y + deltaY;

            while (currentX != targetX || currentY != targetY)
            {
                if (MainWindow.mainWindow.Pawns.Any(p => p.X == currentX && p.Y == currentY))
                    return false;

                if (MainWindow.mainWindow.Queen.Any(q => q.X == currentX && q.Y == currentY))
                    return false;

                currentX += deltaX;
                currentY += deltaY;
            }

            return true;
        }

        public void HighlightQueenMoves()
        {
            // Все направления: горизонталь, вертикаль и диагонали
            int[][] directions = {
        new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 },  // Горизонталь и вертикаль
        new[] { 1, 1 }, new[] { 1, -1 }, new[] { -1, 1 }, new[] { -1, -1 }  // Диагонали
    };

            foreach (var dir in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int x = X + dir[0] * i;
                    int y = Y + dir[1] * i;

                    if (x < 0 || x > 7 || y < 0 || y > 7) break;

                    if (MainWindow.mainWindow.IsEnemy(x, y, Black))
                    {
                        MainWindow.mainWindow.HighlightCell(x, y, Brushes.LightCoral);
                        break;
                    }
                    else if (MainWindow.mainWindow.IsCellOccupied(x, y))
                    {
                        break;
                    }
                    else
                    {
                        MainWindow.mainWindow.HighlightCell(x, y, Brushes.LightGreen);
                    }
                }
            }
        }

        public void HighlightPawnMoves()
        {
            int direction = Black ? -1 : 1; // Направление движения: черные вверх, белые вниз

            // Ход на одну клетку вперед
            int x1 = X;
            int y1 = Y + direction;

            if (y1 >= 0 && y1 < 8 && !MainWindow.mainWindow.IsCellOccupied(x1, y1))
            {
                MainWindow.mainWindow.HighlightCell(x1, y1, Brushes.LightGreen);

                // Ход на две клетки вперед (только с начальной позиции)
                if ((Black && Y == 6) || (!Black && Y == 1))
                {
                    int y2 = Y + 2 * direction;
                    if (y2 >= 0 && y2 < 8 && !MainWindow.mainWindow.IsCellOccupied(x1, y2))
                    {
                        MainWindow.mainWindow.HighlightCell(x1, y2, Brushes.LightGreen);
                    }
                }
            }

            // Взятие фигур по диагонали
            int[] captureX = { X - 1, X + 1 };
            int captureY = Y + direction;

            foreach (int x in captureX)
            {
                if (x >= 0 && x < 8 && captureY >= 0 && captureY < 8)
                {
                    if (MainWindow.mainWindow.IsEnemy(x, captureY, Black))
                    {
                        MainWindow.mainWindow.HighlightCell(x, captureY, Brushes.LightCoral);
                    }
                }
            }
        }

    }
}
