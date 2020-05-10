using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi;
using Reversi.Controller;

namespace ReversiWebAPI.ResponseObjects
{
    public class ReversiBoardResponse
    {
        public PlayerResponse CurrentPlayer { get; } 
        public IEnumerable<ReversiBoardSpaceResponse> Spaces { get; }
        public ReversiBoardResponse(ReversiBoardController boardController)
        {
            if (boardController == null)
                return;

            CurrentPlayer = new PlayerResponse(boardController.Board.CurrentPlayer);
            List<ReversiBoardSpace> spaces = boardController.Board.Spaces.Cast<ReversiBoardSpace>().ToList();
            Spaces = spaces.Select(
                (space, i) => new ReversiBoardSpaceResponse(space, i / Constants.REVERSI_BOARD_LENGTH, i % Constants.REVERSI_BOARD_LENGTH)
            );
        }
    }
}
