export const DEFAULTS = {
  REVERSI_BOARD_REQUEST: {
    spacesRequest: []
  },
  DIFFICULITY_LEVEL: 5,
  USER_GOES_FIRST: true,
  MOVE_NONE: false,
  SUCCESS: false
};

export const CONSTANTS = {
  Player: {
    PLAYER_BLACK: 0,
    PLAYER_WHITE: 1
  },
  Space: {
    EMPTY: 0,
    BLACK: 2,
    WHITE: 1
  },
  Move: {
    SPACE_IDENTIFIER: 1
  },
  Board: {
    SIZE: 8
  }
};

export const MESSAGES = {
  WIN: [
    "You have taken another step on the path of #WINNING",
    "You must surely be unmatched in any battle of the mind",
    "Way to go ol' sport!"
  ],
  LOSE: [
    "Better luck next time loser :p",
    "Surely, you will not survive the winter",
    "This proves you will be the first to die in the robot uprising"
  ]
};
