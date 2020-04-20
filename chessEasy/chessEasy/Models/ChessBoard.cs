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
        private Grid _Board;
        private List<ChessPiece> ChessPieces = new List<ChessPiece>();

        public ChessBoard()
        {
            this._Board = CreateBoard();
            ColorBoard(_Board);
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
        private void AddChessPiece(ChessPiece chessPiece)
        {
            ChessPieces.Add(chessPiece);
        }

        public Grid GetBoard
        {
            get { return _Board; }
        }
    }
}
