import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { GameSettingsComponent } from './game-settings.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TopRowHeaderComponent } from '../toprow/top-row-header-component';

describe('GameSettingsComponent', () => {
  let component: GameSettingsComponent;
  let fixture: ComponentFixture<GameSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GameSettingsComponent],
      imports: [ReactiveFormsModule, FormsModule, BrowserAnimationsModule,TopRowHeaderComponent],
      providers: [FormBuilder]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component (positive test)', () => {
    expect(component).toBeTruthy();
  });

  // it('should have default settings (positive test)', () => {
  //   const form = component.settingsForm;
  //   expect(form).toBeTruthy();
  //   expect(form.get('boardHeight')?.value).toBe(800);
  //   expect(form.get('boardWidth')?.value).toBe(800);
  //   expect(form.get('boardResolution')?.value).toBe(10);
  //   expect(form.get('showGrid')?.value).toBe(false);
  // });

  // it('should update settings (positive test)', () => {
  //   const form = component.settingsForm;
  //   form.setValue({'boardHeight':20});
  //   form.setValue({'boardWidth':20});
  //   fixture.detectChanges();
  //   expect(form.get('boardHeight')?.value).toBe(20);
  //   expect(form.get('boardWidth')?.value).toBe(20);
  // });

  // it('should render settings in the template (positive test)', () => {
  //   const form = component.settingsForm;
  //   form.setValue({'boardHeight':20});
  //   form.setValue({'boardWidth':20});
  //   fixture.detectChanges();
  //   const compiled = fixture.nativeElement;
  //   expect(compiled.querySelector('.board-height').textContent).toContain('20');
  //   expect(compiled.querySelector('.board-width').textContent).toContain('20');
  // });

  // it('should fail to create the component if a required dependency is missing (negative test)', () => {
  //   TestBed.resetTestingModule();
  //   TestBed.configureTestingModule({
  //     // Intentionally omitting declarations to simulate failure
  //     imports: [FormsModule]
  //   });

  //   expect(() => {
  //     TestBed.createComponent(GameSettingsComponent);
  //   }).toThrowError();
  // });
});