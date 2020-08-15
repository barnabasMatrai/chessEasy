using System;
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

            if (imagePath.Contains("black"))
            {
                this.Color = Color.Black;
            }
            else
            {
                this.Color = Color.White;
            }

            this.Coordinates = coordinates;
        }

        public abstract List<Point> GetValidMoves();
        protected abstract List<Point> RemoveInvalidMoves(List<Point> validMoves);

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
