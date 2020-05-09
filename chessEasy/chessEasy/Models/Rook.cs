using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chessEasy.Models
{
    class Rook : ChessPiece
    {
        public Rook(ChessBoard chessBoard, string imagePath, Point coordinates) : base(chessBoard, imagePath, coordinates)
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
                    if ((j == Coordinates.Y || i == Coordinates.X)
                        && (!(j == Coordinates.Y && i == Coordinates.X)))
                    {
                        validMoves.Add(new Point(i, j));
                    }
                }
            }

            return validMoves;
        }
        protected override List<Point> RemoveInvalidMoves(List<Point> validMoves)
        {
            throw new NotImplementedException();
        }
    }
}
