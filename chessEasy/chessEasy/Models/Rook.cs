﻿using chessEasy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chessEasy.Models
{
    class Rook : ChessPiece, ICanMoveCardinally
    {
        public Rook(ChessBoard chessBoard, string imagePath, Point coordinates) : base(chessBoard, imagePath, coordinates)
        {

        }

        public override List<Point> GetValidMoves()
        {
            List<Point> validMoves = new List<Point>();
            validMoves = GetValidCardinalMoves(validMoves);
            validMoves = RemoveInvalidMoves(validMoves);

            return validMoves;
        }

        public List<Point> GetValidCardinalMoves(List<Point> validMoves)
        {
            ChessPiece[,] board = ChessBoard.GetBoard;

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
                if (obstacle.X < this.Coordinates.X)
                {
                    validMoves = validMoves.Where(move => !(move.X < obstacle.X)).ToList();
                }
                else if (obstacle.X > this.Coordinates.X)
                {
                    validMoves = validMoves.Where(move => !(move.X > obstacle.X)).ToList();
                }
                else if (obstacle.Y < this.Coordinates.Y)
                {
                    validMoves = validMoves.Where(move => !(move.Y < obstacle.Y)).ToList();
                }
                else if (obstacle.Y > this.Coordinates.Y)
                {
                    validMoves = validMoves.Where(move => !(move.Y > obstacle.Y)).ToList();
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
