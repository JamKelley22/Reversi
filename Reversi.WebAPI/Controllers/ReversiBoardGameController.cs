using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reversi;
using Reversi.Controller;
using Reversi.Data;
using Reversi.Interfaces.Managers;
using Reversi.View;
using ReversiManagers;
using ReversiWebAPI.RequestObjects;
using ReversiWebAPI.ResponseObjects;

namespace ReversiWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReversiBoardGameController : ControllerBase
    {
        private readonly ILogger<ReversiBoardGameController> _logger;
        private readonly IReversiBoardManager _reversiBoardManager;
        //private readonly ReversiBoardController _controller;

        public ReversiBoardGameController(ILogger<ReversiBoardGameController> logger, IReversiBoardManager reversiBoardManager)
        {
            _logger = logger;
            _reversiBoardManager = reversiBoardManager;
        }

        [HttpGet]
        public ActionResult<ReversiBoardGameResponse> GetReversiBoardGame(int id = -1)
        {
            if (id == -1)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "No id paramater provided" })
                );
            }

            ReversiBoardGame boardGame = _reversiBoardManager.GetReversiBoardGame(id);
            if (boardGame == null)
            {
                return NotFound(
                    JsonConvert.SerializeObject(new Error() { Message = "Could not find board with id " + id })
                );
            }
            bool isTerminal = boardGame.ReversiBoardController.IsTerminal();
            if(isTerminal)
            {
                //Determine Who Won
                bool blackWins = boardGame.ReversiBoardController.Evaluate(Player.PlayerBlack, BoardWeights.CustomWeighting) > 0;
                return new ReversiBoardGameResponse(boardGame, id, isTerminal, new PlayerResponse(blackWins ? Player.PlayerBlack : Player.PlayerWhite));
            }
            return Ok(new ReversiBoardGameResponse(boardGame, id, isTerminal));
        }

        [Route("games")]
        [HttpGet]
        public ActionResult<IEnumerable<ReversiBoardGameResponse>> GetAllReversiBoardGames()
        {
            Dictionary<int, ReversiBoardGame> keyValuePairs = _reversiBoardManager.GetAllReversiBoardGames();
            IEnumerable<ReversiBoardGameResponse> boardGameResponses = keyValuePairs.Select(pair => new ReversiBoardGameResponse(pair.Value, pair.Key, pair.Value.ReversiBoardController.IsTerminal())).ToList();
            return Ok(boardGameResponses);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ReversiBoardGameResponse> NewReversiBoardGame([FromBody]ReversiBoardGameRequest boardRequest)
        {
            //Check for negative or zero difficulty level
            int difficultyLevel = boardRequest.DifficulityLevel;
            if (difficultyLevel < Constants.MIN_DIFFICULTY_LEVEL || difficultyLevel > Constants.MAX_DIFFICULTY_LEVEL)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "Difficulty level " + difficultyLevel + " out of range (1-10)" })
                );
            }

            ReversiBoardController reversiBoardController;
            //ReversiBoard board;
            if (boardRequest.ReversiBoard != null && boardRequest.ReversiBoard.Spaces != null)
            {
                ReversiBoardSpace[,] spaces = new ReversiBoardSpace[Constants.REVERSI_BOARD_LENGTH, Constants.REVERSI_BOARD_LENGTH];
                IEnumerable<ReversiBoardSpaceRequest> requestSpaces = boardRequest.ReversiBoard.Spaces;
                for (int i = 0; i < boardRequest.ReversiBoard.Spaces.Count(); i++)
                {
                    //int row = i / Constants.REVERSI_BOARD_LENGTH;
                    //int col = i % Constants.REVERSI_BOARD_LENGTH;
                    spaces[requestSpaces.ElementAt(i).Row, requestSpaces.ElementAt(i).Col] = (ReversiBoardSpace)boardRequest.ReversiBoard.Spaces.ElementAt(i).SpaceIdentifier;
                }
                reversiBoardController = new ReversiBoardController(spaces, new ReversiBoardView());
                //board = new ReversiBoard(spaces);
            }
            else
            {
                reversiBoardController = new ReversiBoardController(new ReversiBoardView());
            }

            ReversiBoardGame boardGame = new ReversiBoardGame()
            {
                DifficulityLevel = boardRequest.DifficulityLevel,
                ReversiBoardController = reversiBoardController,
                UserGoesFirst = boardRequest.UserGoesFirst
            };
            int key = _reversiBoardManager.AddReversiBoardGame(boardGame);

            bool isTerminal = boardGame.ReversiBoardController.IsTerminal();
            if (isTerminal)
            {
                //Determine Who Won
                bool blackWins = boardGame.ReversiBoardController.Evaluate(Player.PlayerBlack, BoardWeights.CustomWeighting) > 0;
                return new ReversiBoardGameResponse(boardGame, key, isTerminal, new PlayerResponse(blackWins ? Player.PlayerBlack : Player.PlayerWhite));
            }
            return CreatedAtAction(nameof(NewReversiBoardGame), new ReversiBoardGameResponse(boardGame, key, isTerminal));
        }

        [Route("moves")]
        public ActionResult<IEnumerable<MoveResponse>> GetValidMoves(int id = -1)
        {
            if (id == -1)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "No id paramater provided" })
                );
            }

            ReversiBoardGame boardGame = _reversiBoardManager.GetReversiBoardGame(id);
            if (boardGame == null)
            {
                return NotFound(
                    JsonConvert.SerializeObject(new Error() { Message = "Could not find board with id " + id })
                );
            }

            IEnumerable<MoveResponse> validMoves = boardGame.ReversiBoardController.GetMoves(boardGame.UserGoesFirst ? Player.PlayerBlack : Player.PlayerWhite, false).Select(move => 
                new MoveResponse(move, true)
            ).ToList();

            return Ok(validMoves);
        }

        [HttpPost]
        [Route("move")]
        public ActionResult<MoveResponse> MakeMoveOnReversiBoard([FromBody]MoveRequest moveRequest, int id = -1)
        {
            if (id == -1)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "No id paramater provided" })
                );
            }

            ReversiBoardGame boardGame = _reversiBoardManager.GetReversiBoardGame(id);
            if (boardGame == null)
            {
                return NotFound(
                    JsonConvert.SerializeObject(new Error() { Message = "Could not find board with id " + id })
                );
            }

            ReversiBoardSpace space = (ReversiBoardSpace)(moveRequest.Space.SpaceIdentifier);
            Move move = new Move(space, new Vector2(moveRequest.Space.Row, moveRequest.Space.Col));
            if (!boardGame.ReversiBoardController.IsValidMove(move))
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "Invalid Move" })
                );
            }

            bool result = boardGame.ReversiBoardController.MakeMove(move);
            return Ok(new MoveResponse(move, result));
        }

        [HttpPost]
        [Route("move/ai")]
        public ActionResult<MoveResponse> AIMakeMove(int id = -1)
        {
            if (id == -1)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(new Error() { Message = "No id paramater provided" })
                );
            }

            ReversiBoardGame boardGame = _reversiBoardManager.GetReversiBoardGame(id);
            if (boardGame == null)
            {
                return NotFound(
                    JsonConvert.SerializeObject(new Error() { Message = "Could not find board with id " + id })
                );
            }

            Player aiPlayer = boardGame.UserGoesFirst ? Player.PlayerWhite : Player.PlayerBlack;
            List<Move> aiMoves = boardGame.ReversiBoardController.GetMoves(aiPlayer, true);
            
            if (aiMoves.Count > 0)
            {
                Move aiBestMove;
                int aiBestScore;

                int numberOfMinimaxCalls = 0;
                //(aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel, 0, ref numberOfMinimaxCalls);
                (aiBestScore, aiBestMove) = boardGame.ReversiBoardController.Minimax(boardGame.ReversiBoardController.Board, aiPlayer, boardGame.DifficulityLevel, 0, int.MinValue, int.MaxValue, ref numberOfMinimaxCalls);
                boardGame.ReversiBoardController.MakeMove(aiBestMove);
                return Ok(new MoveResponse(aiBestMove, true));
            }
            else
            {
                return Ok(new MoveResponse(new Move(true), false));
            }
        }
    }
}
