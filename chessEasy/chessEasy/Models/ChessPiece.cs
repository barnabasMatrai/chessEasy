using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace chessEasy.Models
{
    public abstract class ChessPiece
    {
        protected string ImagePath;
        protected Color Color;
        protected Point Coordinates;
    }
}
