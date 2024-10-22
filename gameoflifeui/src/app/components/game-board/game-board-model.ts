import { BehaviorSubject, Subject } from 'rxjs';
import { ButtonCommand } from '../../services/button-row.service';
import {
  allowedDimensions,
  allowedResolutions,
  SessionState,
} from '../../services/sessionstore.service';

type Bit = 0 | 1;
type BitArray = Bit[];
export type GameboardArr = BitArray[];

const oneOrZero = () => (Math.random() >= 0.5 ? 1 : 0);

interface keysPressed {
  altKey: boolean;
  shiftKey: boolean;
  ctrlKey: boolean;
}

export class GameBoard {
  private _boardWidht: allowedDimensions;
  set boardWidth(val: allowedDimensions) {
    this._boardWidht = val;
  }

  private _boardHeight: allowedDimensions;
  set boardHeight(val: allowedDimensions) {
    this._boardHeight = val;
  }

  private _boardResolution: allowedResolutions;
  set boardResolution(val: allowedResolutions) {
    this._boardResolution = val;
  }

  constructor(
    boardWidht: allowedDimensions,
    boardHeight: allowedDimensions,
    boardResolution: allowedResolutions
  ) {
    this._boardWidht = boardWidht;
    this._boardHeight = boardHeight;
    this._boardResolution = boardResolution;

    this.createInitialRandomGameboard();
    this._gameBoardSubj = new BehaviorSubject(this._gameBoard);
  }

  public _gameBoard: GameboardArr = [];

  public createInitialRandomGameboard() {
    this._gameBoard = this.createGameboard();
  }

  public _gameBoardSubj: BehaviorSubject<GameboardArr>;
  get selectGameBoard() {
    return this._gameBoardSubj.asObservable();
  }

  updateSessionState(sessionState: SessionState) {
    const { boardHeight, boardResolution, boardWidth } = sessionState;

    const onlyshowGridChanged =
      boardWidth === this._boardWidht &&
      boardHeight === this._boardHeight &&
      boardResolution === this._boardResolution;

    if (!onlyshowGridChanged) {
      this.boardHeight = boardHeight;
      this.boardResolution = boardResolution;
      this.boardWidth = boardWidth;

      this._gameBoard = this.createGameboard();
    }
    this.updateBoard();
  }

  updateBoard() {
    this._gameBoardSubj.next(this._gameBoard);
  }

  executeCommand(command: ButtonCommand) {
    switch (command) {
      case 'createCleanBoard':
        this._gameBoard = this.createGameboard(() => 0);
        break;
      case 'createRandomBoard':
        this._gameBoard = this.createGameboard();
    }
    this.updateBoard();
  }

  private createGameboard(fillfunction: () => Bit = oneOrZero): GameboardArr {
    const width = this._boardWidht;
    const height = this._boardHeight;
    const resolution = this._boardResolution;

    const numOfRows = height / resolution;
    const numOfCols = width / resolution;

    const boardArr: GameboardArr = new Array(numOfRows)
      .fill(undefined)
      .map(() => new Array(numOfCols).fill(undefined).map(fillfunction));

      console.log('boardArr', boardArr);

    return boardArr;
  }

  private copyBoard(board: GameboardArr): GameboardArr {
    return board.map((row) => [...row]);
  }

  killOrAwakeCell(x: number, y: number, keysPressed: keysPressed) {
    const resolution = this._boardResolution;
    const gameBoard = this._gameBoard;

    const row = Math.floor(y / resolution);
    const col = Math.floor(x / resolution);

    const boardCopy = this.copyBoard(this._gameBoard);

    const indexViolatesLowerBorder = col < 0 || row < 0;

    const maxRowIndex = gameBoard.length - 1;
    const maxColIndex = gameBoard[0].length - 1;

    const indexViolatesUpperBorder = row > maxRowIndex || col > maxColIndex;

    const indexInBounds =
      !indexViolatesLowerBorder && !indexViolatesUpperBorder;

    if (indexInBounds) {
      //altKey => make row besides alive
      //altKey + shift => make row besides dead

      //ctrl => make col below alive
      //ctrl  + shift => make col below dead

      const { altKey, shiftKey, ctrlKey } = keysPressed;
      if (!altKey && !ctrlKey) {
        boardCopy[row][col] = boardCopy[row][col] ? 0 : 1;
      } else if (altKey && !ctrlKey) {
        const filler = shiftKey ? 0 : 1;
        for (let cIndex = col; cIndex < maxColIndex + 1; cIndex++) {
          boardCopy[row][cIndex] = filler;
        }
      } else if (!altKey && ctrlKey) {
        const filler = shiftKey ? 0 : 1;
        for (let rIndex = row; rIndex < maxRowIndex + 1; rIndex++) {
          boardCopy[rIndex][col] = filler;
        }
      } else if (altKey && ctrlKey) {
        const filler = shiftKey ? 0 : 1;
        for (let cIndex = col; cIndex < maxColIndex + 1; cIndex++) {
          boardCopy[row][cIndex] = filler;
        }
        for (let rIndex = row + 1; rIndex < maxRowIndex + 1; rIndex++) {
          boardCopy[rIndex][col] = filler;
        }
      }
    }

    this._gameBoard = boardCopy;
    this.updateBoard();
  }

  addPredefinedObject(x: number, y: number, obj: GameboardArr) {
    const resolution = this._boardResolution;
    const gameBoard = this._gameBoard;

    const startRow = Math.floor(y / resolution);
    const startCol = Math.floor(x / resolution);

    const boardCopy = this.copyBoard(this._gameBoard);

    const indexViolatesLowerBorder = startCol < 0 || startRow < 0;

    const maxRowIndex = gameBoard.length - 1;
    const maxColIndex = gameBoard[0].length - 1;

    const indexViolatesUpperBorder = (row: number, col: number) =>
      row > maxRowIndex || col > maxColIndex;

    if (!indexViolatesLowerBorder) {
      obj.forEach((objRow, rIndex) => {
        objRow.forEach((cell, cIndex) => {
          const row = startRow + rIndex;
          const col = startCol + cIndex;
          if (indexViolatesUpperBorder(row, col)) return;
          boardCopy[row][col] = cell;
        });
      });
    }
    this._gameBoard = boardCopy;
    this.updateBoard();
  }

  createNextGeneration() {
    const board = this._gameBoard;
    const boardLength = board.length;
    const nextGeneration: GameboardArr = this.copyBoard(this._gameBoard);

    for (let rowIndex = 0; rowIndex < boardLength; rowIndex++) {
      const row = board[rowIndex];
      const rowLength = row.length;
      for (let colIndex = 0; colIndex < rowLength; colIndex++) {
        const cell = row[colIndex];

        const rowAbove = board[rowIndex - 1] || [];
        const rowBelow = board[rowIndex + 1] || [];
        const fieldBefore = board[rowIndex][colIndex - 1];
        const fieldAfter = board[rowIndex][colIndex + 1];

        const countOfLivingNeighbors = [
          rowAbove[colIndex - 1],
          rowAbove[colIndex],
          rowAbove[colIndex + 1],
          rowBelow[colIndex - 1],
          rowBelow[colIndex],
          rowBelow[colIndex + 1],
          fieldBefore,
          fieldAfter,
        ].reduce<number>((pv, cv) => (cv != null ? pv + cv : pv), 0);

        const becomeLivingCellInAnyCase = countOfLivingNeighbors === 3 as number;
        const keepAlive = cell && countOfLivingNeighbors === 2 as number;

        const newCellValue = becomeLivingCellInAnyCase || keepAlive ? 1 : 0;

        nextGeneration[rowIndex][colIndex] = newCellValue;
      }
    }

    console.log('nextGeneration', nextGeneration);
    this._gameBoard = nextGeneration;
    this.updateBoard();
  } //end of createNextGeneration()
} // end of class
