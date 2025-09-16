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
        public List<Classes.Pawn> Pawns = new List<Classes.Pawn>();

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.mainWindow = this;

            Pawns.Add(new Classes.Pawn(0, 1, false));
            Pawns.Add(new Classes.Pawn(1, 1, false));
            Pawns.Add(new Classes.Pawn(2, 1, false));
            Pawns.Add(new Classes.Pawn(3, 1, false));
            Pawns.Add(new Classes.Pawn(4, 1, false));
            Pawns.Add(new Classes.Pawn(5, 1, false));
            Pawns.Add(new Classes.Pawn(6, 1, false));
            Pawns.Add(new Classes.Pawn(7, 1, false));

            Pawns.Add(new Classes.Pawn(0, 6, false));
            Pawns.Add(new Classes.Pawn(1, 6, false));
            Pawns.Add(new Classes.Pawn(2, 6, false));
            Pawns.Add(new Classes.Pawn(3, 6, false));
            Pawns.Add(new Classes.Pawn(4, 6, false));
            Pawns.Add(new Classes.Pawn(5, 6, false));
            Pawns.Add(new Classes.Pawn(6, 6, false));
            Pawns.Add(new Classes.Pawn(7, 6, false));
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
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/Pawn (black).png")));
                }
                else
                {
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/Pawn (white).png")));
                }
                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }
        }

        public void OnSelect(Classes.Pawn SelectPawn)
        {
            foreach (Classes.Pawn Pawn in Pawns)
            {
                if (Pawn != SelectPawn)
                {
                    if (Pawn.Select)
                    {
                        Pawn.SelectFigure(null, null);
                    }
                }
            }
        }

        private void SelectTile(object sender , MouseButtonEventArgs e)
        {
            Grid Tile = sender as Grid;
            int X = Grid.GetColumn(Tile);
            int Y = Grid.GetRow(Tile);
            Classes.Pawn SelectPawn = Pawns.Find(x => x.Select == true);
            if (SelectPawn != null)
            {
                SelectPawn.Transform(X, Y);
            }
        }
    }
}
