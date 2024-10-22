import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GameBoardComponent } from './game-board.component';
import { SessionQuery } from '../../services/sessionquery.service';
import { ButtonRowService } from '../../services/button-row.service';
import { of } from 'rxjs';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { GameboardArr } from './game-board-model'; // Adjust the path as necessary

interface PredefinedObjectsDict {
  vertDoubleU: GameboardArr;
  horDoubleU: GameboardArr;
  octagon: GameboardArr;
  glider: GameboardArr;
  heavyGlider: GameboardArr;
  measuringStick: GameboardArr;
}


describe('GameBoardComponent', () => {
  let component: GameBoardComponent;
  let fixture: ComponentFixture<GameBoardComponent>;
  let sessionQueryMock: any;
  let buttonRowServiceMock: any;
  let gameboardMock: any;
  let mockPredefinedObjectsDict: PredefinedObjectsDict;
  let mockGameboardArr: GameboardArr = [[0, 1, 0],[1, 1, 1],[0, 1, 0]];

  beforeEach(async () => {
    mockPredefinedObjectsDict = {
      vertDoubleU: mockGameboardArr,
      horDoubleU: mockGameboardArr,
      octagon: mockGameboardArr,
      glider: mockGameboardArr,
      heavyGlider: mockGameboardArr,
      measuringStick: mockGameboardArr,
    };

    gameboardMock = {
      executeCommand: jasmine.createSpy('executeCommand'),
      boardWidth: 1200,
      boardHeight: 1200,
      boardResolution: 1,
      selectGameBoard: jasmine.createSpy('selectGameBoard'),
      createNextGeneration: jasmine.createSpy('createNextGeneration'),
      // Add other required properties here
    };

    sessionQueryMock = {
      getValue: jasmine.createSpy('getValue').and.returnValue({
        boardHeight: 10,
        boardWidth: 10,
        boardResolution: 1,
      }),
      select: jasmine.createSpy('select').and.returnValue(of({
        boardHeight: 10,
        boardWidth: 10,
        boardResolution: 1,
      })),
    };

    buttonRowServiceMock = {
      selectCommands: of('someCommand'),
      selectIsRunning: of(false),
      getIsRunning: false,
    };

    await TestBed.configureTestingModule({
      declarations: [GameBoardComponent],
      providers: [
        { provide: SessionQuery, useValue: sessionQueryMock },
        { provide: ButtonRowService, useValue: buttonRowServiceMock },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component (positive test)', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the canvas context (positive test)', () => {
    const canvas = fixture.nativeElement.querySelector('canvas');
    const context = canvas.getContext('2d');
    spyOn(canvas, 'getContext').and.returnValue(context);
    component.ngAfterViewInit();
    expect(component.ctx).toBe(context);
  });

  it('should throw an error if the canvas context is not available (negative test)', () => {
    const canvas = fixture.nativeElement.querySelector('canvas');
    spyOn(canvas, 'getContext').and.returnValue(null);
    expect(() => component.ngAfterViewInit()).toThrowError('Failed to get 2D context');
  });

  it('should subscribe to sessionQuery and update board dimensions (positive test)', () => {
    spyOn(component, 'updateBoardDimensions');
    component.ngAfterViewInit();
    expect(sessionQueryMock.select).toHaveBeenCalled();
    expect(component.updateBoardDimensions).toHaveBeenCalledWith(1200, 1200);
  });

  it('should subscribe to buttonRowService commands and execute them (positive test)', () => {
    gameboardMock = {
      executeCommand: jasmine.createSpy('executeCommand'),
      boardWidth: 1200,
      boardHeight: 1200,
      boardResolution: 1,
      selectGameBoard: jasmine.createSpy('selectGameBoard'),
      createNextGeneration: jasmine.createSpy('createNextGeneration'),
      // Add other required properties here
    };
    spyOn<any>(component, 'createGameBoard').and.returnValue(gameboardMock);
    component.ngAfterViewInit();
    expect(gameboardMock.executeCommand).toHaveBeenCalledWith('someCommand');
  });

  it('should handle board click events (positive test)', () => {
    gameboardMock = {
      killOrAwakeCell: jasmine.createSpy('killOrAwakeCell'),
    };
    spyOn(component as any, 'createGameBoard').and.returnValue(gameboardMock);
    component.ngAfterViewInit();
    const event = new MouseEvent('click', { altKey: true });
    Object.defineProperty(event, 'offsetX', { value: 5 });
    Object.defineProperty(event, 'offsetY', { value: 5 });
    component.onBoardClick(event);
    expect(gameboardMock.killOrAwakeCell).toHaveBeenCalledWith(5, 5, { altKey: true, shiftKey: false, ctrlKey: false });
  });

  it('should handle context menu events (positive test)', () => {
    const event = new MouseEvent('contextmenu', { clientX: 100, clientY: 100 });
    spyOn(event, 'preventDefault');
    component.ngAfterViewInit();
    component.onContextMenu(event);
    expect(event.preventDefault).toHaveBeenCalled();
    expect(component.contextMenuPosition.x).toBe('100px');
    expect(component.contextMenuPosition.y).toBe('100px');
  });

  it('should paint predefined object (positive test)', () => {
    component.contextMenuPosition = {
      x: '5px',
      y: '5px',
      rawEvent: new MouseEvent('click'),
    };

    Object.defineProperty(component.contextMenuPosition.rawEvent, 'offsetX', { value: 5 });
    Object.defineProperty(component.contextMenuPosition.rawEvent, 'offsetY', { value: 5 });
    const objName = 'glider'; // or any predefined object name you want to use
    component.paintPredefinedObject(objName);

    expect(gameboardMock.addPredefinedObject).toHaveBeenCalledWith(5, 5, mockPredefinedObjectsDict);
  });

  it('should start the game loop when isRunning is true (positive test)', () => {

    spyOn(component as any, 'createGameBoard').and.returnValue(gameboardMock);
    spyOn(component, 'loop');
    buttonRowServiceMock.selectIsRunning = of(true);
    component.ngAfterViewInit();
    expect(component.loop).toHaveBeenCalledWith(gameboardMock);
  });
});