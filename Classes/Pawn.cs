using System;
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
            bool attack = false;
            Pawn SelectPawn = MainWindow.mainWindow.Pawns.Find(x => x.Select == true);
            Pawn SelectQueen = MainWindow.mainWindow.Queen.Find(x => x.Select == true);
            if (SelectPawn != null && SelectPawn != this)
            {
                if ((SelectPawn.Black && SelectPawn.Y - 1 == this.Y && (SelectPawn.X - 1 == this.X || SelectPawn.X + 1 == this.X)) ||
                    (!SelectPawn.Black && SelectPawn.Y + 1 == this.Y && (SelectPawn.X - 1 == this.X || SelectPawn.X + 1 == this.X)))
                {
                    if (this.IsQueen)
                    {
                        MainWindow.mainWindow.Queen.Remove(this);
                    }
                    else
                    {
                        MainWindow.mainWindow.Pawns.Remove(this);
                    }
                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    Grid.SetColumn(SelectPawn.Figure, this.X);
                    Grid.SetRow(SelectPawn.Figure, this.Y);
                    SelectPawn.X = this.X;
                    SelectPawn.Y = this.Y;
                    SelectPawn.Select = false;
                    MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
                    return;
                }
            }

            if (SelectQueen != null && SelectQueen != this)
            {
                if (this.Black != SelectQueen.Black && (this.X == SelectQueen.X || this.Y == SelectQueen.Y || Math.Abs(this.X - SelectQueen.X) == Math.Abs(this.Y - SelectQueen.Y)))
                {
                    if (this.IsQueen)
                    {
                        MainWindow.mainWindow.Queen.Remove(this);
                    }
                    else
                    {
                        MainWindow.mainWindow.Pawns.Remove(this);
                    }
                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    Grid.SetColumn(SelectQueen.Figure, this.X);
                    Grid.SetRow(SelectQueen.Figure, this.Y);
                    SelectQueen.X = this.X;
                    SelectQueen.Y = this.Y;
                    SelectQueen.Select = false;
                    MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
                    return;
                }
            }

            if (!attack)
            {
                if (!this.IsQueen)
                {
                    MainWindow.mainWindow.OnSelect(this, this);
                    if (this.Select)
                    {
                        if (this.Black)
                        {
                            this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                        }
                        else
                        {
                            this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));
                        }
                        this.Select = false;
                    }
                    else
                    {
                        this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (select).png")));
                        this.Select = true;
                    }
                }
                else
                {
                    if (this.Select)
                    {
                        if (this.Black)
                        {
                            this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (black).png")));
                        }
                        else
                        {
                            this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (white).png")));
                        }
                        this.Select = false;
                    }
                    else
                    {
                        this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Queen (select).png")));
                        this.Select = true;
                    }
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
                MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
            }
            SelectFigure(null, null);
        }

        public void TransformQueen(int X, int Y)
        {
            if ((this.Black && MainWindow.mainWindow.IsWhiteTurn) || (!this.Black && !MainWindow.mainWindow.IsWhiteTurn))
            {
                SelectFigure(null, null);
                return;
            }
            if (Math.Abs(X - this.X) == Math.Abs(Y - this.Y) || X == this.X || Y == this.Y)
            {
                Grid.SetColumn(this.Figure, X);
                Grid.SetRow(this.Figure, Y);
                this.X = X;
                this.Y = Y;
                MainWindow.mainWindow.IsWhiteTurn = !MainWindow.mainWindow.IsWhiteTurn;
            }
            SelectFigure(null, null);
        }

    }
}
