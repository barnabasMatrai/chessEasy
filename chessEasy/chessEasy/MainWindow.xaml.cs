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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = e.Source as Border;
            Color white = new Color();
            white.R = 255;
            white.G = 255;
            white.B = 255;
            border.Background = new SolidColorBrush(white);
        }

        private void SetupBoard()
        {
            Image chessPiece = CreateChessPiece("black", "queen");
            Grid.SetRow(chessPiece, 0);
            Grid.SetColumn(chessPiece, 1);
            chessBoard.Children.Add(chessPiece);
        }

        private Image CreateChessPiece(string color, string piece)
        {
            Image chessPiece = new Image();
            ImageSource chessPieceSource = new BitmapImage(new Uri("images/" + color + "-" + piece + ".png", UriKind.Relative));
            chessPiece.Source = chessPieceSource;

            return chessPiece;
        }
    }
}
