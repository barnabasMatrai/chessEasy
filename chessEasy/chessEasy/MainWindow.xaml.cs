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
            SetupBoard();
        }

        private void SetupBoard()
        {
            SetupSide("black", 1, 0);
            SetupSide("white", 6, 7);
        }

        private Image CreateChessPiece(string color, string piece)
        {
            Image chessPiece = new Image();
            ImageSource chessPieceSource = new BitmapImage(new Uri("images/" + color + "-" + piece + ".png", UriKind.Relative));
            chessPiece.Source = chessPieceSource;

            return chessPiece;
        }

        private void ChooseChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            Border highlighted = (Border)FindName("highlighted");

            if (highlighted != null)
            {
                ColorTile(highlighted);
                UnshowValidMoves();
                UnregisterName(highlighted.Name);
            }

            border.Name = "highlighted";
            RegisterName(border.Name, border);
            border.Background = Brushes.Yellow;
            ShowValidMoves();
        }

        private IEnumerable<Border> GetValidMoves()
        {
            Border highlighted = (Border)FindName("highlighted");

            int rowNumber = Grid.GetRow(highlighted);
            int columnNumber = Grid.GetColumn(highlighted);
            Image chessPiece = (Image)highlighted.Child;
            string chessPieceSource = chessPiece.Source.ToString();

            IEnumerable<Border> borders = null;

            if (chessPiece.Source.ToString().Contains("pawn"))
            {
                if (chessPieceSource.Contains("black"))
                {
                    borders = chessBoard.Children
                        .Cast<Border>()
                        .Where(child => Grid.GetColumn(child) == columnNumber && Grid.GetRow(child) <= rowNumber + 2 && Grid.GetRow(child) > rowNumber);
                }
                else
                {
                    borders = chessBoard.Children
                        .Cast<Border>()
                        .Where(child => Grid.GetColumn(child) == columnNumber && Grid.GetRow(child) >= rowNumber - 2 && Grid.GetRow(child) < rowNumber);
                }

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }
            else if (chessPieceSource.Contains("rook"))
            {
                borders = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child != highlighted && (Grid.GetColumn(child) == columnNumber || Grid.GetRow(child) == rowNumber));

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }
            else if (chessPieceSource.Contains("bishop"))
            {
                borders = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child != highlighted && Math.Abs(Grid.GetColumn(child) - columnNumber) == Math.Abs(Grid.GetRow(child) - rowNumber));

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }
            else if (chessPieceSource.Contains("queen"))
            {
                borders = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child != highlighted && (Math.Abs(Grid.GetColumn(child) - columnNumber) == Math.Abs(Grid.GetRow(child) - rowNumber) || Grid.GetColumn(child) == columnNumber || Grid.GetRow(child) == rowNumber));

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }
            else if (chessPieceSource.Contains("knight"))
            {
                borders = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => Grid.GetColumn(child) == columnNumber + 2 && Grid.GetRow(child) == rowNumber + 1
                    || Grid.GetColumn(child) == columnNumber + 2 && Grid.GetRow(child) == rowNumber - 1
                    || Grid.GetColumn(child) == columnNumber - 2 && Grid.GetRow(child) == rowNumber + 1
                    || Grid.GetColumn(child) == columnNumber - 2 && Grid.GetRow(child) == rowNumber - 1
                    || Grid.GetColumn(child) == columnNumber + 1 && Grid.GetRow(child) == rowNumber + 2
                    || Grid.GetColumn(child) == columnNumber + 1 && Grid.GetRow(child) == rowNumber - 2
                    || Grid.GetColumn(child) == columnNumber - 1 && Grid.GetRow(child) == rowNumber + 2
                    || Grid.GetColumn(child) == columnNumber - 1 && Grid.GetRow(child) == rowNumber - 2);

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }
            else if (chessPieceSource.Contains("king"))
            {
                borders = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child != highlighted
                    && Grid.GetColumn(child) < columnNumber + 2
                    && Grid.GetColumn(child) > columnNumber - 2
                    && Grid.GetRow(child) < rowNumber + 2
                    && Grid.GetRow(child) > rowNumber - 2);

                borders = RemoveInvalidMoves(borders, chessPieceSource, rowNumber, columnNumber);
            }

            return borders;
        }

        private IEnumerable<Border> RemoveInvalidMoves(IEnumerable<Border> borders, string chessPieceSource, int rowNumber, int columnNumber)
        {
            IEnumerable<Border> obstacles = borders.Where(child => child.Child != null);

            if (chessPieceSource.Contains("pawn"))
            {
                IEnumerable<Border> situationalSteps = null;

                if (chessPieceSource.Contains("black"))
                {
                    foreach (Border obstacle in obstacles)
                    {
                        int obstacleRow = Grid.GetRow(obstacle);
                        int obstacleColumn = Grid.GetColumn(obstacle);

                        borders = borders.Where(child => !(Grid.GetRow(child) >= obstacleRow));

                    }

                    situationalSteps = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child.Child != null && Grid.GetRow(child) == rowNumber + 1 && (Grid.GetColumn(child) == columnNumber + 1 || Grid.GetColumn(child) == columnNumber - 1));

                    foreach (Border situationalStep in situationalSteps)
                    {
                        Image image = (Image)situationalStep.Child;

                        if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                        {
                            situationalSteps = situationalSteps.Where(child => !(child.Equals(situationalStep)));
                        }
                    }

                    borders = borders.Concat(situationalSteps);
            }
                else
                {
                    foreach (Border obstacle in obstacles)
                    {
                        int obstacleRow = Grid.GetRow(obstacle);
                        int obstacleColumn = Grid.GetColumn(obstacle);

                        borders = borders.Where(child => !(Grid.GetRow(child) <= obstacleRow));
                    }

                    situationalSteps = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => child.Child != null && Grid.GetRow(child) == rowNumber - 1 && (Grid.GetColumn(child) == columnNumber + 1 || Grid.GetColumn(child) == columnNumber - 1));

                    foreach (Border situationalStep in situationalSteps)
                    {
                        Image image = (Image)situationalStep.Child;

                        if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                        {
                            borders = borders.Where(child => !(child.Equals(situationalStep)));
                        }
                    }

                    borders = borders.Concat(situationalSteps);
                }
            }
            else if (chessPieceSource.Contains("rook"))
            {
                foreach (Border obstacle in obstacles)
                {
                    int obstacleRow = Grid.GetRow(obstacle);
                    int obstacleColumn = Grid.GetColumn(obstacle);

                    if (obstacleRow < rowNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow));
                    }
                    else if (obstacleRow > rowNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow));
                    }
                    else if (obstacleColumn < columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetColumn(child) < obstacleColumn));
                    }
                    else if (obstacleColumn > columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetColumn(child) > obstacleColumn));
                    }

                    foreach (Border border in borders)
                    {
                        Image image = (Image)border.Child;

                        if (image != null)
                        {
                            if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                            {
                                borders = borders.Where(child => !(child.Equals(border)));
                            }
                        }
                    }
                }
            }
            else if (chessPieceSource.Contains("bishop"))
            {
                foreach (Border obstacle in obstacles)
                {
                    int obstacleRow = Grid.GetRow(obstacle);
                    int obstacleColumn = Grid.GetColumn(obstacle);

                    if (obstacleRow < rowNumber && obstacleColumn < columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow && Grid.GetColumn(child) < obstacleColumn));
                    }
                    else if (obstacleRow < rowNumber && obstacleColumn > columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow && Grid.GetColumn(child) > obstacleColumn));
                    }
                    else if (obstacleRow > rowNumber && obstacleColumn < columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow && Grid.GetColumn(child) < obstacleColumn));
                    }
                    else if (obstacleRow > rowNumber && obstacleColumn > columnNumber)
                    {
                        borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow && Grid.GetColumn(child) > obstacleColumn));
                    }

                    foreach (Border border in borders)
                    {
                        Image image = (Image)border.Child;

                        if (image != null)
                        {
                            if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                            {
                                borders = borders.Where(child => !(child.Equals(border)));
                            }
                        }
                    }
                }
            }
            else if (chessPieceSource.Contains("queen"))
            {
                foreach (Border obstacle in obstacles)
                {
                    int obstacleRow = Grid.GetRow(obstacle);
                    int obstacleColumn = Grid.GetColumn(obstacle);

                    if (obstacleRow < rowNumber)
                    {
                        if (obstacleColumn < columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow && Grid.GetColumn(child) < obstacleColumn));
                        }
                        else if (obstacleColumn > columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow && Grid.GetColumn(child) > obstacleColumn));
                        }
                        else
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) < obstacleRow && Grid.GetColumn(child) == obstacleColumn));
                        }
                    }
                    else if (obstacleRow > rowNumber)
                    {
                        if (obstacleColumn < columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow && Grid.GetColumn(child) < obstacleColumn));
                        }
                        else if (obstacleColumn > columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow && Grid.GetColumn(child) > obstacleColumn));
                        }
                        else
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) > obstacleRow && Grid.GetColumn(child) == obstacleColumn));
                        }
                    }
                    else
                    {
                        if (obstacleColumn < columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) == obstacleRow && Grid.GetColumn(child) < obstacleColumn));
                        }
                        else if (obstacleColumn > columnNumber)
                        {
                            borders = borders.Where(child => !(Grid.GetRow(child) == obstacleRow && Grid.GetColumn(child) > obstacleColumn));
                        }
                    }

                    foreach (Border border in borders)
                    {
                        Image image = (Image)border.Child;

                        if (image != null)
                        {
                            if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                            {
                                borders = borders.Where(child => !(child.Equals(border)));
                            }
                        }
                    }
                }
            }
            else if (chessPieceSource.Contains("knight"))
            {
                foreach (Border border in borders)
                {
                    Image image = (Image)border.Child;

                    if (image != null)
                    {
                        if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                        {
                            borders = borders.Where(child => !(child.Equals(border)));
                        }
                    }
                }
            }
            else if (chessPieceSource.Contains("king"))
            {
                foreach (Border border in borders)
                {
                    Image image = (Image)border.Child;

                    if (image != null)
                    {
                        if (image.Source.ToString().Contains("black").Equals(chessPieceSource.Contains("black")))
                        {
                            borders = borders.Where(child => !(child.Equals(border)));
                        }
                    }
                }
            }

            return borders;
        }

        private void ShowValidMoves()
        {
            IEnumerable<Border> borders = GetValidMoves();

            if (borders != null)
            {
                foreach (Border border in borders)
                {
                    border.Background = Brushes.LightGreen;
                }
            }
        }

        private void UnshowValidMoves()
        {
            IEnumerable<Border> borders = GetValidMoves();

            if (borders != null)
            {
                foreach (Border border in borders)
                {
                    ColorTile(border);
                }
            }
        }

        private void MoveChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border highlighted = (Border)FindName("highlighted");
            Border stepLocation = (Border)sender;

            if (highlighted != null)
            {
                if (stepLocation.Background == Brushes.LightGreen)
                {
                    ColorTile(highlighted);
                    UnshowValidMoves();
                    UnregisterName(highlighted.Name);

                    highlighted.MouseDown -= ChooseChessPiece;
                    highlighted.MouseDown += MoveChessPiece;
                    Image chessPiece = (Image)highlighted.Child;

                    highlighted.Child = null;
                    stepLocation.Child = chessPiece;

                    stepLocation.MouseDown -= MoveChessPiece;
                    stepLocation.MouseDown += ChooseChessPiece;
                }
                else
                {
                    ColorTile(highlighted);
                    UnshowValidMoves();
                    UnregisterName(highlighted.Name);
                }
            }
        }

        private void ColorTile(Border border)
        {
            int rowNumber = Grid.GetRow(border);
            int columnNumber = Grid.GetColumn(border);

            if (rowNumber % 2 == 0)
            {
                if (columnNumber % 2 == 0)
                {
                    border.Background = Brushes.Transparent;
                }
                else
                {
                    border.Background = Brushes.LightGray;
                }
            }
            else
            {
                if (columnNumber % 2 == 0)
                {
                    border.Background = Brushes.LightGray;
                }
                else
                {
                    border.Background = Brushes.Transparent;
                }
            }
        }

        private void ColorBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Border border = new Border();
                    border.MouseDown += MoveChessPiece;

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
                Image chessPiece = CreateChessPiece(color, pieces[i]);
                Border border = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => Grid.GetRow(child) == rowNumber && Grid.GetColumn(child) == i)
                    .First();
                border.MouseDown -= MoveChessPiece;
                border.MouseDown += ChooseChessPiece;
                border.Child = chessPiece;
            }
        }

        private void SetupRow(string color, int rowNumber, string piece)
        {
            for (int i = 0; i < 8; i++)
            {
                Image chessPiece = CreateChessPiece(color, piece);
                Border border = chessBoard.Children
                    .Cast<Border>()
                    .Where(child => Grid.GetRow(child) == rowNumber && Grid.GetColumn(child) == i)
                    .First();
                border.MouseDown -= MoveChessPiece;
                border.MouseDown += ChooseChessPiece;
                border.Child = chessPiece;
            }
        }
    }
}
