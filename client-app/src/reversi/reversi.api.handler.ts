import axios from "axios";

import {
  ReversiBoardGameRequest,
  ReversiBoardGame,
  Move,
  MoveRequest
} from "./reversi.api.types";

export interface IReversiHandler {
  get(gameId: number): Promise<ReversiBoardGame>;
  getAll(): Promise<ReversiBoardGame[]>;
  create(request: ReversiBoardGameRequest): Promise<ReversiBoardGame>;
  delete(gameId: number): Promise<boolean>;
  getValidMoves(gameId: number): Promise<Move[]>;
  makeMove(moveRequest: MoveRequest, id: number): Promise<Move>;
  aiMakeMove(gameId: number): Promise<Move>;
}

export class Error {
  message: any;
  error: any;
  constructor(data: any) {
    this.message = data.message;
    this.error = data.error;
  }
}

let BASE_URI = "";
const PROD_URL: string = process.env.REACT_APP_PROD_URL
  ? process.env.REACT_APP_PROD_URL
  : "";
const DEV_URL: string = process.env.REACT_APP_DEV_URL
  ? process.env.REACT_APP_DEV_URL
  : "";
if (process.env.NODE_ENV === "production") {
  BASE_URI = PROD_URL;
} else {
  BASE_URI = DEV_URL;
}

export class ReversiHandler implements IReversiHandler {
  async get(gameId: number): Promise<ReversiBoardGame> {
    try {
      const response = await axios.get(BASE_URI, {
        params: {
          id: gameId
        }
      });
      return new ReversiBoardGame(response.data);
    } catch (e) {
      return Promise.reject(e);
    }
  }
  async getAll(): Promise<ReversiBoardGame[]> {
    try {
      const response = await axios.get(BASE_URI + "/games");
      return response.data.map((game: any) => new ReversiBoardGame(game));
    } catch (e) {
      return Promise.reject(e);
    }
  }
  async create(request: ReversiBoardGameRequest): Promise<ReversiBoardGame> {
    try {
      const response = await axios.post(BASE_URI, {
        reversiBoard: request.reversiBoardRequest,
        difficulityLevel: request.difficultyLevel,
        userGoesFirst: request.userGoesFirst
      });
      return new ReversiBoardGame(response.data);
    } catch (e) {
      return Promise.reject(e);
    }
  }
  async delete(gameId: number): Promise<boolean> {
    throw new Error("Method not implemented.");
  }
  async getValidMoves(gameId: number): Promise<Move[]> {
    try {
      const response = await axios.get(BASE_URI + "/moves", {
        params: {
          id: gameId
        }
      });

      return response.data.map((move: any) => new Move(move));
    } catch (e) {
      return Promise.reject(e);
    }
  }
  async makeMove(moveRequest: MoveRequest, gameId: number): Promise<Move> {
    try {
      const response = await axios.post(
        BASE_URI + "/move",
        {
          space: moveRequest.spaceRequest,
          none: moveRequest.none
        },
        {
          params: {
            id: gameId
          }
        }
      );
      return new Move(response.data);
    } catch (e) {
      return Promise.reject(e);
    }
  }
  async aiMakeMove(gameId: number): Promise<Move> {
    try {
      const response = await axios.post(
        BASE_URI + "/move/ai",
        {},
        {
          params: {
            id: gameId
          }
        }
      );
      return new Move(response.data);
    } catch (e) {
      return Promise.reject(e);
    }
  }
}
