using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ChessPiece[,] GetBoard { get; }
        private Color currentTurnColor;
        private const int BOARD_LENGTH = 8;

        public ChessBoard(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GetBoard = SetupBoard();
            currentTurnColor = Color.White;
        }

        private ChessPiece[,] SetupBoard()
        {
            ChessPiece[,] board = new ChessPiece[BOARD_LENGTH, BOARD_LENGTH];

            board = SetupSide(board, Color.Black, 1, 0);
            board = SetupSide(board, Color.White, BOARD_LENGTH - 2, BOARD_LENGTH - 1);

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

            ChessPiece[] chessPieces2 = new ChessPiece[BOARD_LENGTH];

            for (int i = 0; i < BOARD_LENGTH; i++)
            {
                chessPieces2[i] = new Pawn(this, "images/" + colorString + "-pawn.png", new Point(frontRow, i));
            }

            board = SetupRow(board, backRow, chessPieces1);
            board = SetupRow(board, frontRow, chessPieces2);

            return board;
        }

        private ChessPiece[,] SetupRow(ChessPiece[,] board, int row, ChessPiece[] chessPieces)
        {
            for (int i = 0; i < BOARD_LENGTH; i++)
            {
                board[row, i] = chessPieces[i];
            }

            return board;
        }

        public Grid GenerateBoard()
        {
            Grid board = CreateBoard();
            ColorBoard(board);
            PopulateBoard(board);

            return board;
        }

        private Grid CreateBoard()
        {
            Grid board = new Grid();

            for (int i = 0; i < BOARD_LENGTH; i++)
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
            for (int i = 0; i < BOARD_LENGTH; i++)
            {
                for (int j = 0; j < BOARD_LENGTH; j++)
                {
                    Border border = new Border();

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    
                    border.MouseDown += MoveChessPiece;
                    
                    ColorTile(border);

                    board.Children.Add(border);
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
            for (int i = 0; i < BOARD_LENGTH; i++)
            {
                for (int j = 0; j < BOARD_LENGTH; j++)
                {
                    if (GetBoard[i, j] != null)
                    {
                        Image chessPieceImage = CreateImageFromChessPiece(GetBoard[i, j]);

                        Border border = chessBoard.Children
                        .Cast<Border>()
                        .Where(child => Grid.GetRow(child) == i && Grid.GetColumn(child) == j)
                        .First();

                        if (GetBoard[i, j].GetColor == currentTurnColor)
                        {
                            border.MouseDown -= MoveChessPiece;
                            chessPieceImage.MouseDown += ChooseChessPiece;
                        }

                        border.Child = chessPieceImage;
                    }
                }
            };
        }

        private void ChooseChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)((Image)sender).Parent;
            Border highlighted = (Border)mainWindow.FindName("highlighted");
            ChessPiece chessPiece = GetBoard[Grid.GetRow(border), Grid.GetColumn(border)];

            if (highlighted != null)
            {
                ChessPiece highlightedChessPiece = GetBoard[Grid.GetRow(highlighted), Grid.GetColumn(highlighted)];

                List<Point> highlightedValidMoves = highlightedChessPiece.GetValidMoves();

                ColorTile(highlighted);
                UnshowValidMoves(highlightedValidMoves);
                mainWindow.UnregisterName(highlighted.Name);
            }

            if (chessPiece.GetColor == currentTurnColor)
            {

                border.Name = "highlighted";
                mainWindow.RegisterName(border.Name, border);
                border.Background = Brushes.Yellow;

                List<Point> validMoves = chessPiece.GetValidMoves();
                ShowValidMoves(validMoves);
            }
        }

        private void ShowValidMoves(List<Point> validMoves)
        {
            foreach (Point point in validMoves)
            {
                Border border = GetBorderFromPoint(point);
                border.Background = Brushes.LightGreen;
            }
        }

        private void UnshowValidMoves(List<Point> validMoves)
        {
            foreach (Point point in validMoves)
            {
                Border border = GetBorderFromPoint(point);
                ColorTile(border);
            }
        }

        private Border GetBorderFromPoint(Point point)
        {
            Border border = ((Grid)mainWindow.chessBoard.Children[0]).Children
                        .Cast<Border>()
                        .Where(child => Grid.GetRow(child) == point.X && Grid.GetColumn(child) == point.Y)
                        .First();

            return border;
        }

        private void MoveChessPiece(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            Border highlighted = (Border)mainWindow.FindName("highlighted");

            if (highlighted != null)
            {

                int originX = Grid.GetRow(highlighted);
                int originY = Grid.GetColumn(highlighted);

                ChessPiece currentChessPiece = GetBoard[originX, originY];

                if (border.Child == null || currentChessPiece.GetColor == currentTurnColor)
                {
                    bool kingIsChecked = KingIsChecked(currentTurnColor);

                    int destinationX = Grid.GetRow(border);
                    int destinationY = Grid.GetColumn(border);

                    if (!kingIsChecked || (kingIsChecked && MoveResolvesCheck(currentTurnColor, currentChessPiece.GetCoordinates, new Point(destinationX, destinationY))))
                    {
                        List<Point> validMoves = currentChessPiece.GetValidMoves();

                        if (validMoves.Where(point => point.X == destinationX && point.Y == destinationY).Any())
                        {
                            currentChessPiece.SetCoordinates = new Point(destinationX, destinationY);

                            GetBoard[destinationX, destinationY] = GetBoard[originX, originY];
                            GetBoard[originX, originY] = null;

                            currentTurnColor = currentTurnColor == Color.White ? Color.Black : Color.White;
                        
                            UpdateBoard();

                            mainWindow.UnregisterName(highlighted.Name);

                            if (IsCheckMate(currentTurnColor))
                            {
                                MessageBox.Show(currentTurnColor.ToString() + " has lost");
                            }
                        }
                        else
                        {
                            ColorTile(highlighted);
                            List<Point> highlightedValidMoves = currentChessPiece.GetValidMoves();
                            UnshowValidMoves(highlightedValidMoves);
                            mainWindow.UnregisterName(highlighted.Name);
                        }
                    }
                    else
                    {
                        ColorTile(highlighted);
                        List<Point> highlightedValidMoves = currentChessPiece.GetValidMoves();
                        UnshowValidMoves(highlightedValidMoves);
                        mainWindow.UnregisterName(highlighted.Name);
                    }

                }
            }

        }

        private void UpdateBoard()
        {
            mainWindow.chessBoard.Children.Remove(mainWindow.chessBoard.Children[0]);
            mainWindow.chessBoard.Children.Add(GenerateBoard());
        }

        private King GetKing(Color color)
        {
            ChessPiece[,] board = GetBoard;
            King king = null;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    ChessPiece chessPiece = board[i, j];

                    if (chessPiece != null)
                    {
                        if (chessPiece.GetType().Name == "King" && chessPiece.GetColor == color)
                        {
                            king = (King)chessPiece;
                        }
                    }
                }
            }

            return king;
        }

        private bool KingIsChecked(Color color)
        {
            ChessPiece[,] board = GetBoard;
            King king = GetKing(color);
            Point kingCoordinates = king.GetCoordinates;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    ChessPiece chessPiece = board[i, j];

                    if (chessPiece != null)
                    {
                        if (chessPiece.GetValidMoves().Contains(kingCoordinates))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsCheckMate(Color color)
        {
            ChessPiece[,] board = GetBoard;
            King king = GetKing(color);

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    ChessPiece chessPiece = board[i, j];

                    if (chessPiece != null)
                    {
                        if (chessPiece.GetColor == king.GetColor)
                        {
                            foreach (Point move in chessPiece.GetValidMoves())
                            {
                                if (MoveResolvesCheck(color, chessPiece.GetCoordinates, move))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool MoveResolvesCheck(Color color, Point origin, Point move)
        {
            ChessPiece[,] board = GetBoard;

            int originX = (int) origin.X;
            int originY = (int) origin.Y;

            int moveX = (int) move.X;
            int moveY = (int) move.Y;

            ChessPiece chessPieceAtMove = board[moveX, moveY];

            bool moveResolvesCheck = false;

            if ((chessPieceAtMove != null && chessPieceAtMove.GetType().Name != "King") || chessPieceAtMove == null)
            {
                board[moveX, moveY] = board[originX, originY];
                board[originX, originY] = null;

                board[moveX, moveY].SetCoordinates = move;

                if (!KingIsChecked(color))
                {
                    moveResolvesCheck = true;
                }

                board[originX, originY] = board[moveX, moveY];
                board[moveX, moveY] = chessPieceAtMove;

                board[originX, originY].SetCoordinates = origin;
            }
            return moveResolvesCheck;
        }

        private Image CreateImageFromChessPiece(ChessPiece chessPiece)
        {
            Image chessPieceImage = new Image();
            ImageSource chessPieceSource = new BitmapImage(chessPiece.GetImagePath);
            chessPieceImage.Source = chessPieceSource;

            return chessPieceImage;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < GetBoard.GetLength(0); i++)
            {
                for (int j = 0; j < GetBoard.GetLength(1); j++)
                {
                    if (GetBoard[i, j] != null)
                    {
                        sb.Append(GetBoard[i, j].GetType().Name[0]);
                        sb.Append(GetBoard[i, j].GetType().Name[1]);
                    }
                    else
                    {
                        sb.Append("--");
                    }
                    if (j == BOARD_LENGTH - 1)
                    {
                        sb.Append("\n");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
