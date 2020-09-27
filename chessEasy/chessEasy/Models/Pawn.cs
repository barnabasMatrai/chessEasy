using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chessEasy.Models
{
    class Pawn : ChessPiece
    {
        public Pawn(ChessBoard chessBoard, string imagePath, Point coordinates) : base(chessBoard, imagePath, coordinates)
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
                    if (this.Color == Color.Black)
                    {
                        if ((j == Coordinates.Y && i <= Coordinates.X + 2 && i > Coordinates.X))
                        {
                            validMoves.Add(new Point(i, j));
                        }
                    }
                    else
                    {
                        if ((j == Coordinates.Y && i >= Coordinates.X - 2 && i < Coordinates.X))
                        {
                            validMoves.Add(new Point(i, j));
                        }
                    }
                }
            }

            validMoves = RemoveInvalidMoves(validMoves);

            List<Point> situationalSteps;

            if (this.Color == Color.Black)
            {
                situationalSteps = new List<Point>() { new Point(Coordinates.X + 1, Coordinates.Y - 1),
                                                       new Point(Coordinates.X + 1, Coordinates.Y + 1)};
            }
            else
            {
                situationalSteps = new List<Point>() { new Point(Coordinates.X - 1, Coordinates.Y - 1),
                                                       new Point(Coordinates.X - 1, Coordinates.Y + 1)};
            }

            foreach (Point situationalStep in situationalSteps)
            {
                if (situationalStep.X >= 0 && situationalStep.X < board.GetLength(1)
                    && situationalStep.Y >= 0 && situationalStep.Y < board.GetLength(0))
                {
                    ChessPiece chessPiece = board[(int)situationalStep.X, (int)situationalStep.Y];
                    if (chessPiece == null || chessPiece.GetColor == this.Color)
                    {
                        situationalSteps = situationalSteps.Where(step => step != situationalStep).ToList();
                    }
                }
                else
                {
                    situationalSteps = situationalSteps.Where(step => step != situationalStep).ToList();
                }
            }

            validMoves = validMoves.Concat(situationalSteps).ToList();

            return validMoves;
        }
        protected override List<Point> RemoveInvalidMoves(List<Point> validMoves)
        {
            ChessPiece[,] board = ChessBoard.GetBoard;

            IEnumerable<Point> obstacles = validMoves
                .Where(point => board[(int)point.X, (int)point.Y] != null);

            foreach (Point obstacle in obstacles)
            {
                if (this.Color == Color.Black)
                {
                    validMoves = validMoves.Where(move =>
                    !(move.X >= obstacle.X)).ToList();
                }
                else
                {
                    validMoves = validMoves.Where(move =>
                    !(move.X <= obstacle.X)).ToList();
                }
            }

            return validMoves;
        }

        public override List<Point> GetValidMovesIfKingIsChecked()
        {
            List<Point> validMoves = GetValidMoves();

            validMoves.RemoveAll(move => !MoveResolvesCheck(Color, move));

            return validMoves;
        }
    }
}
