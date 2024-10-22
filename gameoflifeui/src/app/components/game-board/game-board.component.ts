import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { MatMenuTrigger, MatMenuModule } from '@angular/material/menu';
import { Subscription } from 'rxjs';
import { ButtonRowService } from '../../services/button-row.service';
import { SessionQuery } from '../../services/sessionquery.service';

import {
  allowedDimensions,
  SessionState,
} from '../../services/sessionstore.service';

import { GameBoard, GameboardArr } from './game-board-model';

@Component({
  selector: 'app-game-board',
  standalone: true,
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css'],
  imports: [MatMenuModule]
})
export class GameBoardComponent implements AfterViewInit, OnDestroy {
  @ViewChild('canvas', { static: false }) canvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild(MatMenuTrigger, { static: true }) contextMenu!: MatMenuTrigger;

  public ctx!: CanvasRenderingContext2D;

  constructor(
    private _sessionQuery: SessionQuery,
    private _buttonRowService: ButtonRowService
  ) {}

  ngAfterViewInit(): void {
    const context = this.canvas.nativeElement.getContext('2d');
    if (context) {
      this.ctx = context;
    } else {
      throw new Error('Failed to get 2D context');
    }

    const { boardHeight, boardWidth, boardResolution } =
      this._sessionQuery.getValue();

    const gameboard = new GameBoard(boardHeight, boardWidth, boardResolution);

    this._sessionQuery.select().subscribe((sessionState) => {
      this.updateBoardDimensions(
        sessionState.boardWidth,
        sessionState.boardHeight
      );

      this._sessionSettings = { ...sessionState };
      gameboard.updateSessionState(sessionState);
    });

    gameboard.selectGameBoard.subscribe((board) => this.render(board));

    this._buttonRowService.selectCommands.subscribe((command) =>
      gameboard.executeCommand(command)
    );

    this.onBoardClick = function (ev: MouseEvent) {
      const keysPressed = {
        altKey: ev.altKey,
        shiftKey: ev.shiftKey,
        ctrlKey: ev.ctrlKey,
      };

      const isRunning = this._buttonRowService.getIsRunning;
      if (!isRunning)
        gameboard.killOrAwakeCell(ev.offsetX, ev.offsetY, keysPressed);
    };

    this.onContextMenu = (ev: MouseEvent) => {
      ev.preventDefault();
      const isRunning = this._buttonRowService.getIsRunning;
      if (isRunning) return void 0;

      this.contextMenuPosition.x = ev.clientX + 'px';
      this.contextMenuPosition.y = ev.clientY + 'px';
      this.contextMenuPosition.rawEvent = ev;
      this.contextMenu.openMenu();
    };

    this.paintPredefinedObject = (objName: predefinedObject) => {
      const obj = predefinedObjectsDict[objName];
      if (!obj) return void 0;

      const rawEvent = this.contextMenuPosition.rawEvent;
      if (!rawEvent) return;

      const x = rawEvent.offsetX;
      const y = rawEvent.offsetY;

      gameboard.addPredefinedObject(x, y, obj);
    };

    this._buttonRowService.selectIsRunning.subscribe((isRunning) => {
      if (isRunning) this.loop(gameboard);
    });
  }

  loop(board: GameBoard) {
    if (this._buttonRowService.getIsRunning) {
      requestAnimationFrame(() => this.loop(board));
      board.createNextGeneration();
    }
  }

  updateBoardDimensions(
    boardWidth: allowedDimensions,
    boardHeight: allowedDimensions
  ) {
    this.canvas.nativeElement.width = boardWidth;
    this.canvas.nativeElement.height = boardHeight;
  }

  render(gameBoard: GameboardArr) {
    const c = this.ctx;
    const resolution = this._sessionSettings.boardResolution;

    for (let rowInd = 0; rowInd < gameBoard.length; rowInd++) {
      for (let colInd = 0; colInd < gameBoard[rowInd].length; colInd++) {
        const cell = gameBoard[rowInd][colInd];
        c.beginPath();
        c.rect(
          colInd * resolution,
          rowInd * resolution,
          resolution,
          resolution
        );
        c.fillStyle = cell ? 'black' : 'white';
        c.fill();
        if (this._sessionSettings.showGrid) c.stroke();
      }
    }
  }

  private _subscriptions: Subscription[] = [];
  ngOnDestroy() {
    this._subscriptions.forEach((subscriptions) => subscriptions.unsubscribe());
  }

  private _isViewInit = false;
  private _sessionSettings!: SessionState;

  onBoardClick!: (ev: MouseEvent) => void;

  contextMenuPosition = {
    x: '0px',
    y: '0px',
    rawEvent: undefined as MouseEvent | undefined,
  };

  onContextMenu!: (ev: MouseEvent) => void;

  paintPredefinedObject!: (objName: predefinedObject) => void;
}

const vertDoubleU: GameboardArr = [
  [1, 1, 1],
  [1, 0, 1],
  [1, 0, 1],
  [0, 0, 0],
  [1, 0, 1],
  [1, 0, 1],
  [1, 1, 1],
];

const horDoubleU: GameboardArr = [
  [1, 1, 1, 0, 1, 1, 1],
  [1, 0, 0, 0, 0, 0, 1],
  [1, 1, 1, 0, 1, 1, 1],
];

const octagon: GameboardArr = [
  [0, 1, 0, 0, 1, 0],
  [1, 0, 1, 1, 0, 1],
  [0, 1, 0, 0, 1, 0],
  [0, 1, 0, 0, 1, 0],
  [1, 0, 1, 1, 0, 1],
  [0, 1, 0, 0, 1, 0],
];

const glider: GameboardArr = [
  [0, 1, 0],
  [0, 0, 1],
  [1, 1, 1],
];

const heavyGlider: GameboardArr = [
  [0, 1, 1, 1, 1, 1, 1],
  [1, 0, 0, 0, 0, 0, 1],
  [0, 0, 0, 0, 0, 0, 1],
  [1, 0, 0, 0, 0, 1, 0],
  [0, 0, 1, 1, 0, 0, 0],
];

const measuringStick: GameboardArr = [
  [0, 1, 1],
  [1, 1, 0],
  [0, 1, 0],
];

type predefinedObject = keyof PredefinedObjectsDict;

const predefinedObjectsDict: PredefinedObjectsDict = {
  vertDoubleU,
  horDoubleU,
  octagon,
  glider,
  heavyGlider,
  measuringStick,
};

interface PredefinedObjectsDict {
  vertDoubleU: GameboardArr;
  horDoubleU: GameboardArr;
  octagon: GameboardArr;
  glider: GameboardArr;
  heavyGlider: GameboardArr;
  measuringStick: GameboardArr;
}
