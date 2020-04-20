using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace chessEasy.Models
{
    class Bishop : ChessPiece
    {
        protected override IEnumerable<Border> GetValidMoves()
        {
            throw new NotImplementedException();
        }
        protected override IEnumerable<Border> RemoveInvalidMoves()
        {
            throw new NotImplementedException();
        }
    }
}
