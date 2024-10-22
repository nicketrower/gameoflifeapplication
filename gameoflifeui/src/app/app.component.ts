import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GameBoardComponent } from './components/game-board/game-board.component';
import { GameSettingsComponent } from './components/game-settings/game-settings.component';
import { ButtonRowComponent } from './components/button-row/button-row.component';
import { TopRowHeaderComponent } from './components/toprow/top-row-header-component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GameBoardComponent, GameSettingsComponent, ButtonRowComponent,TopRowHeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {
}
