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

using chessEasy.Models;

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
            ChessBoard board = new ChessBoard();
            //MessageBox.Show(chessBoard.ToString());
            chessBoard.Children.Add(board.ShowBoard());
            //ColorBoard();
            //SetupBoard();
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
                UnshowValidMoves(highlighted);
                UnregisterName(highlighted.Name);
            }

            border.Name = "highlighted";
            RegisterName(border.Name, border);
            border.Background = Brushes.Yellow;
            ShowValidMoves(border);
        }

        private IEnumerable<Border> GetValidMoves(Border piece)
        {
            Border highlighted = piece;

            int rowNumber = Grid.GetRow(highlighted);
            int columnNumber = Grid.GetColumn(highlighted);
            Image chessPiece = (Image)highlighted.Child;

            if (chessPiece == null)
            {
                return new List<Border>();
            }
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
                            situationalSteps = situationalSteps.Where(child => !(child.Equals(situationalStep)));
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

        private List<Border> GetPiecesCheckingKing(string color)
        {
            IEnumerable<Border> borders = chessBoard.Children.Cast<Border>().Where(child => child.Child != null);
            List<Border> chessPiecesCheckingKing = new List<Border>();

            foreach (Border border in borders)
            {
                IEnumerable<Border> validMoves = GetValidMoves(border);

                foreach (Border border1 in validMoves)
                {
                    Image image = (Image)border1.Child;

                    if (image != null && image.Source.ToString().Contains(color + "-king"))
                    {
                        chessPiecesCheckingKing.Add(border);
                    }
                }
            }

            return chessPiecesCheckingKing;
        }

        private void ShowValidMoves(Border tile)
        {
            IEnumerable<Border> borders = GetValidMoves(tile);

            if (borders != null)
            {
                foreach (Border border in borders)
                {
                    border.Background = Brushes.LightGreen;
                }
            }
        }

        private void UnshowValidMoves(Border tile)
        {
            IEnumerable<Border> borders = GetValidMoves(tile);

            if (borders != null)
            {
                foreach (Border border in borders)
                {
                    ColorTile(border);
                }
            }
        }

        private IEnumerable<Border> GetBordersTargettingBorder(Border border)
        {
            IEnumerable<Border> bordersTargettingBorder = chessBoard.Children.Cast<Border>().Where(child => GetValidMoves(child).Contains(border));
            Image borderChild = (Image)border.Child;

            foreach (Border borderTargettingBorder in bordersTargettingBorder)
            {
                Image image = (Image)borderTargettingBorder.Child;

                if (image != null && borderChild != null)
                {
                    if (image.Source.ToString().Contains("black").Equals(borderChild.Source.ToString().Contains("black")))
                    {
                        bordersTargettingBorder = bordersTargettingBorder.Where(child => !child.Equals(borderTargettingBorder));
                    }
                }
            }

            return bordersTargettingBorder;
        }

        private void MoveChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border highlighted = (Border)FindName("highlighted");
            Border stepLocation = (Border)sender;

            if (highlighted != null)
            {
                List<Border> piecesCheckingKing = GetPiecesCheckingKing("black");
                bool stepLocationIsCheckingPiece = false;

                if (piecesCheckingKing.Count > 0)
                {
                    foreach (Border pieceCheckingKing in piecesCheckingKing)
                    {
                        int checkingRow = Grid.GetRow(pieceCheckingKing);
                        int checkingColumn = Grid.GetColumn(pieceCheckingKing);

                        int stepLocationRow = Grid.GetRow(stepLocation);
                        int stepLocationColumn = Grid.GetColumn(stepLocation);

                        if (GetBordersTargettingBorder(stepLocation).Count() == 0 || checkingRow == stepLocationRow && checkingColumn == stepLocationColumn)
                        {
                            stepLocationIsCheckingPiece = true;
                        }
                    }
                }

                if ((piecesCheckingKing.Count == 0 || stepLocationIsCheckingPiece.Equals(true)) && stepLocation.Background == Brushes.LightGreen)
                {
                    ColorTile(highlighted);
                    UnshowValidMoves(highlighted);
                    UnregisterName(highlighted.Name);

                    highlighted.MouseDown -= ChooseChessPiece;
                    highlighted.MouseDown += MoveChessPiece;
                    Image chessPiece = (Image)highlighted.Child;

                    highlighted.Child = null;
                    stepLocation.Child = chessPiece;

                    stepLocation.MouseDown -= MoveChessPiece;
                    stepLocation.MouseDown += ChooseChessPiece;

                    bool chessPieceWasBlack = chessPiece.Source.ToString().Contains("black");

                    foreach (Border border in chessBoard.Children)
                    {
                        if (border.Child != null)
                        {
                            Image borderChild = (Image)border.Child;

                            if (chessPieceWasBlack)
                            {
                                if (borderChild.Source.ToString().Contains("white"))
                                {
                                    border.MouseDown -= MoveChessPiece;
                                    border.MouseDown += ChooseChessPiece;
                                }
                                else
                                {
                                    border.MouseDown -= ChooseChessPiece;
                                    border.MouseDown += MoveChessPiece;
                                }
                            }
                            else
                            {
                                if (borderChild.Source.ToString().Contains("black"))
                                {
                                    border.MouseDown -= MoveChessPiece;
                                    border.MouseDown += ChooseChessPiece;
                                }
                                else
                                {
                                    border.MouseDown -= ChooseChessPiece;
                                    border.MouseDown += MoveChessPiece;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ColorTile(highlighted);
                    UnshowValidMoves(highlighted);
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

                if (color.Equals("white"))
                {
                    border.MouseDown -= MoveChessPiece;
                    border.MouseDown += ChooseChessPiece;
                }
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

                if (color.Equals("white"))
                {
                    border.MouseDown -= MoveChessPiece;
                    border.MouseDown += ChooseChessPiece;
                }
                border.Child = chessPiece;
            }
        }
    }
}
