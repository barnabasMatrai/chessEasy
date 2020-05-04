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
            throw new NotImplementedException();
        }
        protected override List<Point> RemoveInvalidMoves()
        {
            throw new NotImplementedException();
        }
    }
}
