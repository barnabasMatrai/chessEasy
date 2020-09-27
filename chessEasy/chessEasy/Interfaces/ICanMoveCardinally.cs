using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace chessEasy.Interfaces
{
    interface ICanMoveCardinally
    {
        List<Point> GetValidCardinalMoves(List<Point> validMoves);
    }
}
