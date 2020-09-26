﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace chessEasy.Models
{
    public abstract class ChessPiece
    {
        protected ChessBoard ChessBoard;
        protected Uri ImagePath;
        protected Color Color;
        protected Point Coordinates;

        protected ChessPiece(ChessBoard chessBoard, string imagePath, Point coordinates)
        {
            this.ChessBoard = chessBoard;
            this.ImagePath = new Uri(imagePath, UriKind.Relative);
            this.Color = imagePath.Contains("black") ? Color.Black : Color.White;
            this.Coordinates = coordinates;
        }

        public abstract List<Point> GetValidMoves();
        protected abstract List<Point> RemoveInvalidMoves(List<Point> validMoves);

        public bool MoveResolvesCheck(Color color, Point move)
        {
            ChessPiece[,] board = ChessBoard.GetBoard;

            int originX = (int)Coordinates.X;
            int originY = (int)Coordinates.Y;

            int moveX = (int)move.X;
            int moveY = (int)move.Y;

            ChessPiece chessPieceAtMove = board[moveX, moveY];

            bool moveResolvesCheck = false;

            if ((chessPieceAtMove != null && chessPieceAtMove.GetType().Name != "King") || chessPieceAtMove == null)
            {
                board[moveX, moveY] = board[originX, originY];
                board[originX, originY] = null;

                board[moveX, moveY].SetCoordinates = move;

                if (!ChessBoard.KingIsChecked(color))
                {
                    moveResolvesCheck = true;
                }

                board[originX, originY] = board[moveX, moveY];
                board[moveX, moveY] = chessPieceAtMove;

                board[originX, originY].SetCoordinates = new Point(originX, originY);
            }
            return moveResolvesCheck;
        }

        //protected King GetKing(Color color)
        //{
        //    ChessPiece[,] board = ChessBoard.GetBoard;
        //    King king = null;

        //    for (int i = 0; i < board.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < board.GetLength(1); j++)
        //        {
        //            ChessPiece chessPiece = board[i, j];

        //            if (chessPiece != null)
        //            {
        //                if (chessPiece.GetType().Name == "King" && chessPiece.GetColor == color)
        //                {
        //                    king = (King)chessPiece;
        //                }
        //            }
        //        }
        //    }

        //    return king;
        //}

        //public bool KingIsChecked(Color color)
        //{
        //    ChessPiece[,] board = ChessBoard.GetBoard;
        //    King king = GetKing(color);
        //    Point kingCoordinates = king.GetCoordinates;

        //    for (int i = 0; i < board.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < board.GetLength(1); j++)
        //        {
        //            ChessPiece chessPiece = board[i, j];

        //            if (chessPiece != null)
        //            {
        //                if (chessPiece.GetValidMoves().Contains(kingCoordinates))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        public Uri GetImagePath
        {
            get { return this.ImagePath; }
        }

        public Color GetColor
        {
            get { return this.Color; }
        }

        public Point GetCoordinates
        {
            get { return this.Coordinates; }
        }

        public Point SetCoordinates
        {
            set { this.Coordinates = value; }
        }
    }
}
