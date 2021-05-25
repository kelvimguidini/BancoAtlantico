import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as signalR from '@aspnet/signalr';
import { ATM } from '../_interfaces/atm';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AtmService {

  constructor(private http: HttpClient) { }
  public atms!: ATM[];

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  private hubConnection!: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiURL}/hub` )
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('iniciando conexão'))
      .catch(err => console.log('Error ao iniciar conexão: ' + err))
  }

  public addTransferATMDataListener = () => {
    this.hubConnection.on('transfernotesalertdata', (data) => {
      this.atms = data;
      console.log(data);
    });
  }

  public getATMs(): Observable<ATM[]> {
    return this.http.get<ATM[]>(`${environment.apiURL}/maneger/getactveatm`, this.httpOptions);
  }

  public turnOff(id: number): Observable<boolean> {
    return this.http.post<boolean>(`${environment.apiURL}/maneger/turnoffatm`, id, this.httpOptions);
  }
}
