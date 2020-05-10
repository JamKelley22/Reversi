import { DEFAULTS, CONSTANTS } from "./reversiConstants";

export class ReversiBoardGameRequest {
  reversiBoardRequest: ReversiBoardRequest;
  difficultyLevel: number;
  userGoesFirst: boolean;

  constructor(data: any) {
    this.reversiBoardRequest = data.reversiBoardRequest
      ? new ReversiBoardRequest(data.reversiBoardRequest)
      : DEFAULTS.REVERSI_BOARD_REQUEST;
    this.difficultyLevel = data.difficultyLevel
      ? data.difficultyLevel
      : DEFAULTS.DIFFICULITY_LEVEL;
    this.userGoesFirst = data.userGoesFirst
      ? data.userGoesFirst
      : DEFAULTS.USER_GOES_FIRST;
  }
}

export class ReversiBoardGame {
  reversiBoard: ReversiBoard;
  difficulityLevel: number;
  userGoesFirst: boolean;
  reversiBoardKey: number;
  isTerminalBoard: boolean;
  playerWinner: Player | null;

  constructor(data: any) {
    this.reversiBoard = new ReversiBoard(data.reversiBoardResponse);
    this.difficulityLevel = data.difficulityLevel;
    this.userGoesFirst = data.userGoesFirst;
    this.reversiBoardKey = data.reversiBoardKey;
    this.isTerminalBoard = data.isTerminalBoard;
    this.playerWinner = data.playerWinner
      ? new Player(data.playerWinner)
      : null;
  }
}

export class ReversiBoardRequest {
  spacesRequest: ReversiBoardSpaceRequest[];

  constructor(data: any) {
    this.spacesRequest = data.spacesRequest.map(
      (space: any) => new ReversiBoardSpaceRequest(space)
    );
  }
}

export class ReversiBoard {
  currentPlayer: Player | null;
  spaces: ReversiBoardSpace[];

  constructor(data: any) {
    this.currentPlayer = data.currentPlayer
      ? new Player(data.currentPlayer)
      : null;
    this.spaces = data.spaces.map((space: any) => new ReversiBoardSpace(space));
  }
}

export class PlayerRequest {
  playerIdentifier: number;

  constructor(data: any) {
    this.playerIdentifier = data.playerIdentifier;
  }
}

export class Player {
  playerIdentifier: number;
  playerName: string;

  constructor(data: any) {
    this.playerIdentifier = data.playerIdentifier;
    this.playerName = data.playerName;
  }
}

export class ReversiBoardSpaceRequest {
  spaceIdentifier: number;
  row: number;
  col: number;

  constructor(data: any) {
    this.spaceIdentifier = data.spaceIdentifier
      ? data.spaceIdentifier
      : CONSTANTS.Move.SPACE_IDENTIFIER;
    this.row = data.row;
    this.col = data.col;
  }
}

export class ReversiBoardSpace {
  spaceIdentifier: number;
  spaceName: string;
  row: number;
  col: number;

  constructor(data: any) {
    this.spaceIdentifier = data.spaceIdentifier;
    this.spaceName = data.spaceName ? data.spaceName : ""; //TODO
    this.row = data.row;
    this.col = data.col;
  }
}

export class MoveRequest {
  spaceRequest: ReversiBoardSpaceRequest;
  none: boolean;

  constructor(data: any) {
    this.spaceRequest = new ReversiBoardSpaceRequest(data.spaceRequest);
    this.none =
      typeof data.none !== "undefined" ? data.none : DEFAULTS.MOVE_NONE;
  }
}

export class Move {
  space: ReversiBoardSpace;
  success: boolean;

  constructor(data: any) {
    this.space = new ReversiBoardSpace(data.space);
    this.success = data.success ? data.success : DEFAULTS.SUCCESS;
  }

  moveRequest(): MoveRequest {
    return new MoveRequest({
      spaceRequest: {
        spaceIdentifier: this.space.spaceIdentifier,
        row: this.space.row,
        col: this.space.col
      },
      none: DEFAULTS.MOVE_NONE
    });
  }
}
