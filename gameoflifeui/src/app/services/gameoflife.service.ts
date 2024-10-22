import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Subscription } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { SessionState } from './sessionstore.service';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class GameoflifeService {
  constructor(private http: HttpClient) { }

  update(session: Partial<SessionState>) {
    this.http.post(environment.api+"/GameOfLife/UpdateBoardState", session)
      .subscribe(response => {
        console.log('Server response:', response);
      });
  }
}
