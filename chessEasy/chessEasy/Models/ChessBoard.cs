using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace chessEasy.Models
{
    public class ChessBoard
    {
        private ChessPiece[,] board;

        public ChessBoard()
        {
            board = SetupBoard();
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
            ChessPiece[] chessPieces = {
                new Rook("images/" + color + "-rook.png"),
                new Knight("images/" + color + "-knight.png"),
                new Bishop("images/" + color + "-bishop.png"),
                new King("images/" + color + "-king.png"),
                new Queen("images/" + color + "-queen.png"),
                new Bishop("images/" + color + "-bishop.png"),
                new Knight("images/" + color + "-knight.png"),
                new Rook("images/" + color + "-rook.png")};

            board = SetupRow(board, backRow, chessPieces);
            board = SetupRow(board, frontRow, new Pawn("images/" + color + "-pawn.png"));

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

        private ChessPiece[,] SetupRow(ChessPiece[,] board, int row, ChessPiece chessPiece)
        {
            for (int i = 0; i < 8; i++)
            {
                board[row, i] = chessPiece;
            }

            return board;
        }

        public Grid ShowBoard()
        {
            Grid board = CreateBoard();
            ColorBoard(board);

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
                            board.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.LightGray;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
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
                            board.Children.Add(border);
                        }
                        else
                        {
                            border.Background = Brushes.Transparent;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            board.Children.Add(border);
                        }
                    }
                }
        }
        //private void AddChessPiece(ChessPiece chessPiece)
        //{
        //    ChessPieces.Add(chessPiece);
        //}

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
