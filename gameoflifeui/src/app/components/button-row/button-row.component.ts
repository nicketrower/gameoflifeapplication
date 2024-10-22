import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { HelperComponent } from '../helper/helper.component';
import { ButtonRowService } from '../../services/button-row.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-button-row',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, CommonModule],
  templateUrl: './button-row.component.html',
  styleUrl: './button-row.component.css'
})
export class ButtonRowComponent implements OnInit {
  constructor(
    private _bRowService: ButtonRowService,
    private _dialog: MatDialog
  ) {}

  isRunning$!: Observable<boolean>;

  ngOnInit() {
    this.isRunning$ = this._bRowService.selectIsRunning;
  }

  onStartStop() {
    this._bRowService.toggleStartStop();
  }

  onRandomBoard() {
    this._bRowService.pushCommand('createRandomBoard');
  }
  onClearBoard() {
    this._bRowService.pushCommand('createCleanBoard');
  }

  openDialog() {
    const dialogRef = this._dialog.open(HelperComponent);
  }

}
