using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reversi.Controller;
using Reversi;
using Reversi.View;
using System;
using Reversi.Data;

public class UnityReversiBoardView : MonoBehaviour, IReversiBoardView
{
    [Range(1f, 100f)]
    public float maxClickDistance = 50f;
    public LayerMask boardPositionMask;
    public Transform positionsTransformParent;
    public Transform[,] boardPositions;

    ReversiBoardController controller;
    int difficultyLevel1;
    bool userMovesFirst;
    Player userPlayer, aiPlayer;
    ReversiBoardSpace userReversiBoardSpace;

    Move userMove, aiBestMove;
    int aiBestScore;

    private bool boardLocked;
    private int lastBoardHash;
    private List<Move> lastUserMoves;

    public Animator aiAnimator;
    public Manager manager;

    public bool DEBUG = false;

    void Start()
    {
        if(DEBUG)
        {
            //Use debug board file 
            string text = Resources.Load<TextAsset>("Board2").text;
            controller = new ReversiBoardController(text.Split(new[] { Environment.NewLine },StringSplitOptions.None), this);
        }
        else
        {
            //Use default starting config
            controller = new ReversiBoardController(this);
        }
        difficultyLevel1 = GetDifficulty();
        userMovesFirst = GetUserFirst();
        userPlayer = (userMovesFirst) ? Player.PlayerBlack : Player.PlayerWhite;
        aiPlayer = (userMovesFirst) ? Player.PlayerWhite : Player.PlayerBlack;
        userReversiBoardSpace = (userMovesFirst) ? ReversiBoardSpace.BLACK : ReversiBoardSpace.WHITE;

        boardPositions = new Transform[Constants.REVERSI_BOARD_LENGTH, Constants.REVERSI_BOARD_LENGTH];
        int rows = positionsTransformParent.childCount;
        for (int i = 0; i < rows; i++)
        {
            Transform row = positionsTransformParent.GetChild(i);
            int cols = row.childCount;
            for (int j = 0; j < cols; j++)
            {
                boardPositions[i, j] = row.GetChild(j);
            }
        }

        Clear();
        PrintBoard(controller.Board);
        lastBoardHash = controller.Board.GetHashCode();
        lastUserMoves = controller.GetMoves(userPlayer, false);
    }
    void Update()
    {
        if (boardLocked) return;

        List<Move> userMoves = controller.GetMoves(userPlayer, false);
        //This next bit was supposed to make it so that GetMoves would not need to be called every frame if the board state didnt change 
        //but I was having trouble getting it to work and it didnt seem to hurt preformance that much
        //if (controller.Board.GetHashCode() != lastBoardHash)
        //{
        //    userMoves = controller.GetMoves(userPlayer, false);
        //}
        if (controller.IsTerminal())
        {
            ShowWin(controller.Evaluate(userPlayer, BoardWeights.EvenWeighting) > 0 ? Player.PlayerBlack : Player.PlayerWhite);
        }

        if (userMoves.Count == 0)
        {
            //Either player cant go or game is over
            StartCoroutine(AIMakeMove(true));
            //What to do here?
        }

        foreach (Transform t in boardPositions)
        {
            t.GetComponent<MeshRenderer>().enabled = false;
            if(userMoves.Contains(
                new Move(
                    userReversiBoardSpace,
                    new System.Numerics.Vector2(
                        t.GetSiblingIndex(), 
                        t.parent.GetSiblingIndex()
                    )
                )))
            {
                MeshRenderer mr = t.GetComponent<MeshRenderer>();
                mr.material.color = new Color(0, 0, 1, .25f);
                mr.enabled = true;
            }
        }
        Move potentalUserMove = GetUserMove(userPlayer);
        if(potentalUserMove._none == false)
        {
            //Got a legit move back
            //Check to see if its valid
            bool userMoveValid = controller.IsValidMove(potentalUserMove);
            if(userMoveValid)
            {
                //Reset the animator time keeper
                aiAnimator.SetFloat("timeSinceLastUserMove", 0f);
                controller.MakeMove(potentalUserMove);
                PrintBoard(controller.Board);

                StartCoroutine(AIMakeMove(false));
            }
            else
            {
                ShowError("Invalid Move");
            }
        }
        else
        {
            //User did not move
            float timeSinceLastUserMove = aiAnimator.GetFloat("timeSinceLastUserMove");
            if(timeSinceLastUserMove > 30f)
            {
                aiAnimator.SetFloat("timeSinceLastUserMove", 0f);
            }
            else
            {
                aiAnimator.SetFloat("timeSinceLastUserMove", timeSinceLastUserMove + Time.deltaTime);
            }
        }
    }

    IEnumerator AIMakeMove(bool noUserMoves)
    {
        boardLocked = true;
        yield return null;
        // AI Turn
        List<Move> aiMoves = controller.GetMoves(aiPlayer, true);
        if (aiMoves.Count == 0)
        {
            //Game over or at least skip ai turn
            if(noUserMoves)
            {
                //Havent worked out all the bugs of isTerminal so sometimes neither side will have a move and it will keep running, this is the hack to fix it
                //Game over
                ShowWin(controller.Evaluate(userPlayer, BoardWeights.EvenWeighting) > 0 ? Player.PlayerBlack : Player.PlayerWhite);
            }
        }
        int numberOfMinimaxCalls = 0;
        (aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel1, 0, int.MinValue, int.MaxValue, ref numberOfMinimaxCalls);
        Debug.Log(aiBestMove);
        Debug.Log("here");
        controller.MakeMove(aiBestMove);
        yield return new WaitForSeconds(1f);
        PrintBoard(controller.Board);
        boardLocked = false;
    }

    public void Clear()
    {
        foreach (Transform t in boardPositions)
        {
            t.GetChild(0).GetComponent<Animator>().SetInteger("ReversiBoardSpace", 0);
        }
    }

    public int GetDifficulty()
    {
        return 3;
    }

    public void SetDiffuculty(float val)
    {
        difficultyLevel1 = (int)val;
    }

    public bool GetUserFirst()
    {
        return true;
    }

    public Move GetUserMove(Player userPlayer)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxClickDistance, boardPositionMask))
        {
            Transform positionTransformHit = hit.transform;
            MeshRenderer mr = positionTransformHit.GetComponent<MeshRenderer>();
            mr.material.color = new Color(0, 1, 0, .25f);
            mr.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                int row = positionTransformHit.parent.GetSiblingIndex();
                int col = positionTransformHit.GetSiblingIndex();
                return new Move(
                    userReversiBoardSpace,
                    new System.Numerics.Vector2(
                        positionTransformHit.GetSiblingIndex(),
                        positionTransformHit.parent.GetSiblingIndex()
                    )
                );
            }
        }
        return new Move(true);
    }

    public void PrintBoard(ReversiBoard board)
    {
        foreach (Transform t in boardPositions)
        {
            t.GetChild(0).GetComponent<Animator>().SetInteger(
                "ReversiBoardSpace", 
                (int)board.Spaces[t.GetSiblingIndex(), t.parent.GetSiblingIndex()]//might need to switch
            );
        }
    }

    public void PrintTitle()
    {
        return;
    }

    public void ShowError(string val)
    {
        Debug.LogError(val);
    }

    public void ShowWin(Player player)
    {
        string message = Enum.GetName(typeof(Player), player) + " Wins";
        aiAnimator.SetTrigger(player == aiPlayer ? "aiWins" : "userWins");
        Debug.Log(message);
        manager.OnWin(message);
    }

    public void Write(string val)
    {
        Debug.Log(val);
    }
}
