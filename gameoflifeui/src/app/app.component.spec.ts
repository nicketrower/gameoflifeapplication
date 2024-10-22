import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { RouterOutlet } from '@angular/router';
import { GameBoardComponent } from './components/game-board/game-board.component';
import { GameSettingsComponent } from './components/game-settings/game-settings.component';
import { ButtonRowComponent } from './components/button-row/button-row.component';
import { TopRowHeaderComponent } from './components/toprow/top-row-header-component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterOutlet,
        GameBoardComponent,
        GameSettingsComponent,
        ButtonRowComponent,
        TopRowHeaderComponent
      ],
      declarations: [AppComponent]
    }).compileComponents();
  });

  it('should create the app component (positive test)', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  // it('should fail to create the app component if a required dependency is missing (negative test)', () => {
  //   TestBed.resetTestingModule();
  //   TestBed.configureTestingModule({
  //     imports: [
  //       // Intentionally omitting GameBoardComponent to simulate failure
  //       RouterOutlet,
  //       GameSettingsComponent,
  //       ButtonRowComponent,
  //       TopRowHeaderComponent
  //     ],
  //     declarations: [AppComponent]
  //   });

  //   expect(() => {
  //     TestBed.createComponent(AppComponent);
  //   }).toThrowError();
  // });
});