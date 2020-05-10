using ReversiManagers;

namespace ReversiWebAPI.ResponseObjects
{
    public class ReversiBoardGameResponse
    {
        public ReversiBoardResponse ReversiBoardResponse { get; }
        public int DifficulityLevel { get; }
        public bool UserGoesFirst { get; }
        public int ReversiBoardKey { get; }
        public bool IsTerminalBoard { get; }
        public PlayerResponse PlayerWinner { get; }

        public ReversiBoardGameResponse(ReversiBoardGame game, int key, bool isTerminal)
        {
            ReversiBoardResponse = new ReversiBoardResponse(game.ReversiBoardController);
            DifficulityLevel = game.DifficulityLevel;
            UserGoesFirst = game.UserGoesFirst;
            ReversiBoardKey = key;
            IsTerminalBoard = isTerminal;
        }
        public ReversiBoardGameResponse(ReversiBoardGame game, int key, bool isTerminal, PlayerResponse playerWinner) : this(game, key, isTerminal)
        {
            PlayerWinner = playerWinner;
        }
    }
}
