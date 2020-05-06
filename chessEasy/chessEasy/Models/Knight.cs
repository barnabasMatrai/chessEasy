using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chessEasy.Models
{
    class Knight : ChessPiece
    {
        public Knight(ChessBoard chessBoard, string imagePath, Point coordinates) : base(chessBoard, imagePath, coordinates)
        {

        }

        public override List<Point> GetValidMoves()
        {
            ChessPiece[,] board = ChessBoard.GetBoard;
            List<Point> validMoves = new List<Point>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (!(j == Coordinates.Y && i == Coordinates.X)
                        && (j == Coordinates.Y + 2 && i == Coordinates.X + 1
                    || j == Coordinates.Y + 2 && i == Coordinates.X - 1
                    || j == Coordinates.Y - 2 && i == Coordinates.X + 1
                    || j == Coordinates.Y - 2 && i == Coordinates.X - 1
                    || j == Coordinates.Y + 1 && i == Coordinates.X + 2
                    || j == Coordinates.Y + 1 && i == Coordinates.X - 2
                    || j == Coordinates.Y - 1 && i == Coordinates.X + 2
                    || j == Coordinates.Y - 1 && i == Coordinates.X - 2))
                    {
                        validMoves.Add(new Point(i, j));
                    }
                }
            }

            return validMoves;
        }
        protected override List<Point> RemoveInvalidMoves()
        {
            throw new NotImplementedException();
        }
    }
}
