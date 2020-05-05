using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chessEasy.Models
{
    public class ChessBoard
    {
        private MainWindow mainWindow;
        private ChessPiece[,] board;
        private int turnsPassed;

        public ChessBoard(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            board = SetupBoard();
            turnsPassed = 0;
        }

        private ChessPiece[,] SetupBoard()
        {
            ChessPiece[,] board = new ChessPiece[8, 8];

            board = SetupSide(board, Color.Black, 1, 0);
            board = SetupSide(board, Color.White, 6, 7);

            return board;
        }

        private ChessPiece[,] SetupSide(ChessPiece[,] board, Color color, int frontRow, int backRow)
        {
            string colorString = color.ToString().ToLower();

            ChessPiece[] chessPieces1 = {
                new Rook(this, "images/" + colorString + "-rook.png", new Point(backRow, 0)),
                new Knight(this, "images/" + colorString + "-knight.png", new Point(backRow, 1)),
                new Bishop(this, "images/" + colorString + "-bishop.png", new Point(backRow, 2)),
                new King(this, "images/" + colorString + "-king.png", new Point(backRow, 3)),
                new Queen(this, "images/" + colorString + "-queen.png", new Point(backRow, 4)),
                new Bishop(this, "images/" + colorString + "-bishop.png", new Point(backRow, 5)),
                new Knight(this, "images/" + colorString + "-knight.png", new Point(backRow, 6)),
                new Rook(this, "images/" + colorString + "-rook.png", new Point(backRow, 7))};

            ChessPiece[] chessPieces2 = {
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 0)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 1)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 2)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 3)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 4)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 5)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 6)),
                new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, 7))};

            board = SetupRow(board, backRow, chessPieces1);
            board = SetupRow(board, frontRow, chessPieces2);

            return board;
        }

        private ChessPiece[,] SetupRow(ChessPiece[,] board, int row, ChessPiece[] chessPieces)
        {
            for (int i = 0; i < 8; i++)
            {
                board[row, i] = chessPieces[i];
            }

            return board;
        }

        public Grid ShowBoard()
        {
            Grid board = CreateBoard();
            ColorBoard(board);
            PopulateBoard(board);

            return board;
        }

        private Grid CreateBoard()
        {
            Grid board = new Grid();

            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                board.ColumnDefinitions.Add(columnDefinition);

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                board.RowDefinitions.Add(rowDefinition);
            }

            return board;
        }
        private void ColorBoard(Grid board)
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
                            border.MouseDown += MoveChessPiece;
                            board.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.LightGray;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            border.MouseDown += MoveChessPiece;
                            board.Children.Add(border);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            border.Background = Brushes.LightGray;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            border.MouseDown += MoveChessPiece;
                            board.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.Transparent;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            border.MouseDown += MoveChessPiece;
                            board.Children.Add(border);
                        }
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

        private void PopulateBoard(Grid chessBoard)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                    {
                        Image chessPieceImage = CreateImageFromChessPiece(board[i, j]);

                        Border border = chessBoard.Children
                        .Cast<Border>()
                        .Where(child => Grid.GetRow(child) == i && Grid.GetColumn(child) == j)
                        .First();

                        chessPieceImage.MouseDown += ChooseChessPiece;

                        border.Child = chessPieceImage;
                    }
                }
            };
        }

        private void ChooseChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)((Image)sender).Parent;
            Border highlighted = (Border)mainWindow.FindName("highlighted");

            if (border.Child != null)
            {
                ChessPiece chessPiece = board[Grid.GetRow(border), Grid.GetColumn(border)];

                if ((chessPiece.GetColor == Color.Black
                    && turnsPassed % 2 == 1)
                    || (chessPiece.GetColor == Color.White
                    && turnsPassed % 2 == 0))
                {
                    if (highlighted != null)
                    {
                        ChessPiece highlightedChessPiece = board[Grid.GetRow(highlighted), Grid.GetColumn(highlighted)];
                        
                        ColorTile(highlighted);
                        List<Point> highlightedValidMoves = highlightedChessPiece.GetValidMoves();
                        UnshowValidMoves(highlightedValidMoves);
                        mainWindow.UnregisterName(highlighted.Name);
                    }

                    border.Name = "highlighted";
                    mainWindow.RegisterName(border.Name, border);
                    border.Background = Brushes.Yellow;

                    List<Point> validMoves = chessPiece.GetValidMoves();
                    ShowValidMoves(validMoves);
                }
            }
        }

        private void ShowValidMoves(List<Point> validMoves)
        {
            foreach (Point point in validMoves)
            {
                Border border = ((Grid)mainWindow.chessBoard.Children[0]).Children
                        .Cast<Border>()
                        .Where(child => Grid.GetRow(child) == point.X && Grid.GetColumn(child) == point.Y)
                        .First();
                border.Background = Brushes.LightGreen;
            }
        }

        private void UnshowValidMoves(List<Point> validMoves)
        {
            foreach (Point point in validMoves)
            {
                Border border = ((Grid)mainWindow.chessBoard.Children[0]).Children
                        .Cast<Border>()
                        .Where(child => Grid.GetRow(child) == point.X && Grid.GetColumn(child) == point.Y)
                        .First();
                ColorTile(border);
            }
        }

        private void MoveChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            Border highlighted = (Border)mainWindow.FindName("highlighted");

            if (border.Child == null && highlighted != null)
            {
                int originX = Grid.GetRow(highlighted);
                int originY = Grid.GetColumn(highlighted);

                ChessPiece currentChessPiece = board[originX, originY];

                int destinationX = Grid.GetRow(border);
                int destinationY = Grid.GetColumn(border);

                List<Point> validMoves = currentChessPiece.GetValidMoves();

                if (validMoves.Where(point => point.X == destinationX && point.Y == destinationY).Any())
                {
                    currentChessPiece.SetCoordinates = new Point(destinationX, destinationY);

                    board[destinationX, destinationY] = board[originX, originY];
                    board[originX, originY] = null;

                    mainWindow.chessBoard.Children.Remove(mainWindow.chessBoard.Children[0]);
                    mainWindow.chessBoard.Children.Add(ShowBoard());

                    turnsPassed++;
                }
            }
        }

        private Image CreateImageFromChessPiece(ChessPiece chessPiece)
        {
            Image chessPieceImage = new Image();
            ImageSource chessPieceSource = new BitmapImage(chessPiece.GetImagePath);
            chessPieceImage.Source = chessPieceSource;

            return chessPieceImage;
        }

        public ChessPiece[,] GetBoard
        {
            get { return board; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        sb.Append(board[i, j].GetType().Name[0]);
                        sb.Append(board[i, j].GetType().Name[1]);
                    }
                    else
                    {
                        sb.Append("--");
                    }
                    if (j == 7)
                    {
                        sb.Append("\n");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
