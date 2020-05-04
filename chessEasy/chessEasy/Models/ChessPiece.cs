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
        protected abstract List<Point> RemoveInvalidMoves();
        protected void ShowValidMoves()
        {
            List<Point> borders = GetValidMoves();

            //if (borders != null)
            //{
            //    foreach (Border border in borders)
            //    {
            //        border.Background = Brushes.LightGreen;
            //    }
            //}
        }

        public Uri GetImagePath
        {
            get { return this.ImagePath; }
        }
    }
}
