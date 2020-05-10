import * as React from "react";
import { Button, Spin, message, notification } from "antd";

import { ReversiHandler, IReversiHandler } from "./reversi.api.handler";
import {
  ReversiBoardGame,
  ReversiBoardGameRequest,
  Move,
  MoveRequest,
  ReversiBoardSpaceRequest,
  ReversiBoardSpace
} from "./reversi.api.types";
import { CONSTANTS, MESSAGES } from "./reversiConstants";

export interface IReversiProps {
  handler?: IReversiHandler;
  gameId: number;
  difficulty: number;
}

export interface IReversiState {
  game: ReversiBoardGame | null;
  validMoves: Move[] | null;
  moveLock: boolean;
}

export default class Reversi extends React.Component<
  IReversiProps,
  IReversiState
> {
  static defaultProps = {
    handler: new ReversiHandler()
  };
  state: IReversiState = {
    game: null,
    validMoves: null,
    moveLock: false
  };

  componentDidMount = async () => {
    const game: ReversiBoardGame = await this.createGame(
      new ReversiBoardGameRequest({
        difficultyLevel: this.props.difficulty,
        userGoesFirst: true
      })
    );

    const validMoves: Move[] = await this.getValidMoves(game.reversiBoardKey);

    this.setState({
      game: game,
      validMoves: validMoves
    });
  };

  //   shouldComponentUpdate = (
  //     nextProps: IReversiProps,
  //     nextState: IReversiState
  //   ): boolean => {
  //     if (this.state.game) {
  //       const everySpaceSame = this.state.game.reversiBoard.spaces.every(
  //         (space: ReversiBoardSpace, i: number) =>
  //           nextState.game!.reversiBoard.spaces[i].spaceIdentifier ===
  //           space.spaceIdentifier
  //       );
  //       return !everySpaceSame;
  //     } else {
  //       if (nextState.game) {
  //         return true;
  //       }
  //       return false;
  //     }
  //   };

  //   componentDidUpdate = (prevProps: IReversiProps, prevState: IReversiState) => {
  //     if (this.state.game !== null && prevState.game !== null) {
  //       const prevSpaces: ReversiBoardSpace[] = prevState.game!.reversiBoard
  //         .spaces;
  //       const currSpaces: ReversiBoardSpace[] = this.state.game.reversiBoard
  //         .spaces;
  //       for (let i = 0; i < this.state.game.reversiBoard.spaces.length; i++) {
  //         if (
  //           currSpaces[i].spaceIdentifier !== prevSpaces[i].spaceIdentifier ||
  //           currSpaces[i].col !== prevSpaces[i].col ||
  //           currSpaces[i].row !== prevSpaces[i].col
  //         ) {
  //           this.checkGameOver(this.state.game!);
  //           return;
  //         }
  //       }
  //       this.state.game.reversiBoard.spaces.forEach(
  //         (space: ReversiBoardSpace, i: number) => {}
  //       );
  //     }
  //   };

  createGame = async (
    gameRequest: ReversiBoardGameRequest
  ): Promise<ReversiBoardGame> => {
    const { handler } = this.props;

    const game: ReversiBoardGame = await handler!.create(gameRequest);
    return game;
  };

  reloadBoard = async () => {
    const { handler } = this.props;
    const { game } = this.state;

    const newGameState: ReversiBoardGame = await handler!.get(
      game!.reversiBoardKey
    );

    const validMoves: Move[] = await this.getValidMoves(
      newGameState.reversiBoardKey
    );

    let gameCopy: any = { ...this.state.game };
    gameCopy.reversiBoard.spaces = newGameState.reversiBoard.spaces;
    this.setState({ game: gameCopy, validMoves: validMoves });
    this.checkGameOver(game!);
  };

  checkGameOver = (game: ReversiBoardGame) => {
    console.log(game.isTerminalBoard);

    if (game.isTerminalBoard) {
      const btn = (
        <>
          <Button type="danger">Close</Button>{" "}
          <Button
            type="primary"
            onClick={() =>
              this.createGame(
                new ReversiBoardGameRequest({
                  difficultyLevel: this.props.difficulty,
                  userGoesFirst: true
                })
              )
            }
          >
            Play Again
          </Button>
        </>
      );
      const playerWon: boolean =
        game.userGoesFirst &&
        game.playerWinner!.playerIdentifier === CONSTANTS.Player.PLAYER_BLACK;

      const notificationType: "success" | "error" = playerWon
        ? "success"
        : "error";
      notification[notificationType]({
        message: playerWon ? "You Won!" : "You Lost",
        description: playerWon
          ? MESSAGES.WIN[Math.random() * Math.floor(MESSAGES.WIN.length)]
          : MESSAGES.LOSE[Math.random() * Math.floor(MESSAGES.LOSE.length)],
        duration: 0,
        btn
      });
    }
  };

  getValidMoves = async (gameId: number): Promise<Move[]> => {
    const { handler } = this.props;
    const validMoves: Move[] = await handler!.getValidMoves(gameId);
    return validMoves;
  };

  makeMove = async (move: Move) => {
    const { handler } = this.props;
    const { game } = this.state;
    let moveResult: Move = await handler!.makeMove(
      move.moveRequest(),
      game!.reversiBoardKey
    );
    if (!moveResult.success) {
      message.error("Move Not Made");
    } else {
      //message.success("Move Made");
      await this.reloadBoard();
      this.setState({
        moveLock: true
      });
      message.info("AI Thinking...");
      const aiMove: Move = await this.aiMakeMove();
      //message.info("AI Made Move: " + aiMove.toString());
      await this.reloadBoard();
      this.setState({
        moveLock: false
      });
    }
  };

  aiMakeMove = async (): Promise<Move> => {
    const { handler } = this.props;
    const { game } = this.state;
    let aiMove: Move = await handler!.aiMakeMove(game!.reversiBoardKey);
    return aiMove;
  };

  render() {
    if (this.state.game === null) {
      return <Spin />;
    }

    const { game, validMoves, moveLock } = this.state;

    return (
      <div>
        {game?.reversiBoard.spaces.map(
          (space: ReversiBoardSpace, i: number) => {
            let lineBreak = false;
            if (i !== 0 && i % CONSTANTS.Board.SIZE === 0) {
              lineBreak = true;
            }

            let isValidMove = false;
            if (validMoves && validMoves.length > 0) {
              isValidMove = validMoves.some(
                (move: Move) =>
                  move.space.col === space.col &&
                  move.space.row === space.row &&
                  space.spaceIdentifier === CONSTANTS.Space.EMPTY
              );
            }

            const row = Math.floor(i / CONSTANTS.Board.SIZE);
            const col = i % CONSTANTS.Board.SIZE;

            switch (space.spaceIdentifier) {
              case CONSTANTS.Space.BLACK:
                return (
                  <>
                    {lineBreak ? <br /> : <></>}
                    <Button
                      type="primary"
                      shape="circle"
                      onClick={() =>
                        !moveLock ? message.error("Invalid Move") : {}
                      }
                      key={i}
                    />
                  </>
                );
              case CONSTANTS.Space.WHITE:
                return (
                  <>
                    {lineBreak ? <br /> : <></>}
                    <Button
                      type="danger"
                      shape="circle"
                      onClick={() =>
                        !moveLock ? message.error("Invalid Move") : {}
                      }
                      key={i}
                    />
                  </>
                );
              default:
                if (isValidMove)
                  return (
                    <>
                      {lineBreak ? <br /> : <></>}
                      <Button
                        type="default"
                        style={!moveLock ? { borderColor: "green" } : {}}
                        shape="circle"
                        key={i}
                        onClick={() =>
                          !moveLock
                            ? this.makeMove(
                                new Move({
                                  space: {
                                    spaceIdentifier: game.userGoesFirst
                                      ? CONSTANTS.Space.BLACK
                                      : CONSTANTS.Space.WHITE,
                                    row: row,
                                    col: col
                                  }
                                })
                              )
                            : {}
                        }
                        disabled={moveLock}
                      />
                    </>
                  );
                else
                  return (
                    <>
                      {lineBreak ? <br /> : <></>}
                      <Button
                        type="default"
                        shape="circle"
                        key={i}
                        onClick={() =>
                          !moveLock ? message.error("Invalid Move") : {}
                        }
                        disabled={moveLock}
                      />
                    </>
                  );
            }
          }
        )}
      </div>
    );
  }
}
