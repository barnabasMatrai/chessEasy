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
            SetupBoard();
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
            border.Background = Brushes.Yellow;
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
