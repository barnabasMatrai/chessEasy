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
        public Pawn(string imagePath) : base(imagePath)
        {

        }

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
