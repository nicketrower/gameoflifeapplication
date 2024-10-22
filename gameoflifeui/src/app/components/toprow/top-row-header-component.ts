import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-top-row-header',
  standalone: true,
  imports: [MatToolbarModule],
  templateUrl: './top-row-header-component.html',
  styleUrl: './top-row-header-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TopRowHeaderComponent {

}
