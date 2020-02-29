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

namespace chessEasy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ColorBoard();
            //SetupBoard();
        }

        private void SetupBoard()
        {
            SetupSide("black", 1, 0);
            SetupSide("white", 6, 7);
        }

        private Border CreateChessPiece(string color, string piece)
        {
            Image chessPiece = new Image();
            ImageSource chessPieceSource = new BitmapImage(new Uri("images/" + color + "-" + piece + ".png", UriKind.Relative));
            chessPiece.Source = chessPieceSource;
            chessPiece.MouseDown += ChooseChessPiece;

            Border border = new Border();
            border.Child = chessPiece;

            return border;
        }

        private void ChooseChessPiece(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Border highlighted = (Border)FindName("highlighted");

            if (highlighted != null)
            {
                highlighted.Background = Brushes.Transparent;
                UnregisterName(highlighted.Name);
            }

            border.Name = "highlighted";
            RegisterName(border.Name, border);
            border.Background = Brushes.Yellow;
        }

        private void ColorBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Border border = new Border();

                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            border.Background = Brushes.Transparent;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            chessBoard.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.LightGray;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            chessBoard.Children.Add(border);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            border.Background = Brushes.LightGray;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            chessBoard.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.Transparent;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            chessBoard.Children.Add(border);
                        }
                    }
                }
        }

        private void SetupSide(string color, int frontRow, int backRow)
        {
            string[] pieces = { "rook", "knight", "bishop", "king", "queen", "bishop", "knight", "rook" };

            SetupRow(color, backRow, pieces);
            SetupRow(color, frontRow, "pawn");
        }

        private void SetupRow(string color, int rowNumber, string[] pieces)
        {
            for (int i = 0; i < 8; i++)
            {
                Border chessPiece = CreateChessPiece(color, pieces[i]);
                Grid.SetRow(chessPiece, rowNumber);
                Grid.SetColumn(chessPiece, i);
                chessBoard.Children.Add(chessPiece);
            }
        }

        private void SetupRow(string color, int rowNumber, string piece)
        {
            for (int i = 0; i < 8; i++)
            {
                Border chessPiece = CreateChessPiece(color, piece);
                Grid.SetRow(chessPiece, rowNumber);
                Grid.SetColumn(chessPiece, i);
                chessBoard.Children.Add(chessPiece);
            }
        }
    }
}
