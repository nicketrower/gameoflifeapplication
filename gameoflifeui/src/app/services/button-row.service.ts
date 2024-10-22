import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ButtonRowService {

  constructor() { }

  private isRunningSubj: BehaviorSubject<boolean> = new BehaviorSubject(false);
  get selectIsRunning() {
    return this.isRunningSubj.asObservable();
  }
  get getIsRunning() {
    return this.isRunningSubj.value;
  }

  toggleStartStop() {
    this.isRunningSubj.next(!this.isRunningSubj.value);
  }

  private commandSubj: BehaviorSubject<ButtonCommand> = new BehaviorSubject<ButtonCommand>('');
  get selectCommands() {
    return this.commandSubj.asObservable();
  }

  pushCommand(command: ButtonCommand) {
    this.commandSubj.next(command);
  }
}

export type ButtonCommand = 'createRandomBoard' | 'createCleanBoard' | '';
