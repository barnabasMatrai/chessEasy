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
        protected string ImagePath;
        protected Color Color;
        protected Point Coordinates;

        protected abstract void MoveChesspiece();
        protected abstract IEnumerable<Border> GetValidMoves();
        protected abstract IEnumerable<Border> RemoveInvalidMoves();
        protected void ShowValidMoves()
        {
            IEnumerable<Border> borders = GetValidMoves();

            if (borders != null)
            {
                foreach (Border border in borders)
                {
                    border.Background = Brushes.LightGreen;
                }
            }
        }
    }
}
