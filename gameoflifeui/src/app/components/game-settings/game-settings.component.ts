import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { NumberFormatPipe } from '../../pipes/number-format.pipe';
import { distinctUntilChanged } from 'rxjs/operators';
import { ButtonRowService } from '../../services/button-row.service';
import { SessionService } from '../../services/session.service';
import {
  DEFAULT_BOARDID,
  DEFAULT_BOARDNAME,
  DEFAULT_HEIGHT,
  DEFAULT_RESOLUTION,
  DEFAULT_SHOW_GRID,
  DEFAULT_WIDTH,
  dimensionsSelection,
  resolutionsSelection,
  SessionState,
} from '../../services/sessionstore.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import {MatInputModule} from '@angular/material/input';

@Component({
  selector: 'app-game-settings',
  standalone: true,
  imports: [MatSlideToggleModule, MatFormFieldModule, MatSelectModule, MatOptionModule, ReactiveFormsModule, NumberFormatPipe,CommonModule, MatInputModule],
  templateUrl: './game-settings.component.html',
  styleUrl: './game-settings.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GameSettingsComponent {
  constructor(
    private _fb: FormBuilder,
    private _sessionService: SessionService,
    private _buttonRowService: ButtonRowService) {}
    ngOnInit() {
      this.settingsForm = this._fb.group({
        boardId: [DEFAULT_BOARDID],
        boardName: [DEFAULT_BOARDNAME],
        boardHeight: [DEFAULT_HEIGHT],
        boardWidth: [DEFAULT_WIDTH],
        boardResolution: [DEFAULT_RESOLUTION],
        showGrid: [DEFAULT_SHOW_GRID],
      });
  
      const formsChangeSubscription = this.settingsForm.valueChanges
        .pipe(distinctUntilChanged(comparer))
        .subscribe((formValues) => {
          this._sessionService.update(formValues);
        });
  
      const isStartStopSubscription =
        this._buttonRowService.selectIsRunning.subscribe((isRunning) => {
          isRunning ? this.settingsForm.disable() : this.settingsForm.enable();
        });
  
      this._subscriptions.push(formsChangeSubscription, isStartStopSubscription);
    }
  
    settingsForm!: FormGroup;
  
    dimensionsSelection = dimensionsSelection;
    resolutionSelection = resolutionsSelection;
  
    private _subscriptions: Subscription[] = [];
    ngOnDestroy() {
      this._subscriptions.forEach((subscription) => subscription.unsubscribe());
    }
  }
  
  const stateCombinerFn = (state: SessionState) => {
    const { boardHeight, boardResolution, boardWidth, showGrid } = state;
    return [boardHeight, boardResolution, boardWidth, showGrid]
      .map((item) => item.toString())
      .join('');
  };
  
  const comparer = (state: SessionState, state1: SessionState) =>
    stateCombinerFn(state) === stateCombinerFn(state1);
