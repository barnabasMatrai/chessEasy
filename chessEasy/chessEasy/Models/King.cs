using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chessEasy.Models
{
    public class King : ChessPiece
    {
        public King(ChessBoard chessBoard, string imagePath, Point coordinates) : base(chessBoard, imagePath, coordinates)
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
                        && (j < Coordinates.Y + 2
                    && j > Coordinates.Y - 2
                    && i < Coordinates.X + 2
                    && i > Coordinates.X - 2))
                    {
                        validMoves.Add(new Point(i, j));
                    }
                }
            }

            validMoves = RemoveInvalidMoves(validMoves);

            return validMoves;
        }
        protected override List<Point> RemoveInvalidMoves(List<Point> validMoves)
        {
            ChessPiece[,] board = ChessBoard.GetBoard;

            IEnumerable<Point> obstacles = validMoves
                .Where(point => board[(int)point.X, (int)point.Y] != null);

            foreach (Point obstacle in obstacles)
            {
                if (board[(int)obstacle.X, (int)obstacle.Y].GetColor == this.Color)
                {
                    validMoves = validMoves
                        .Where(move => !(move.X == obstacle.X && move.Y == obstacle.Y))
                        .ToList();
                }
            }

            return validMoves;
        }
    }
}
