import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { User } from '../_interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUserSubject!: BehaviorSubject<User>;
  public currentUser!: Observable<User>;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('token') as string));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login() {
    return this.http.post<any>(`${environment.apiURL}/users/auth`, {
      'username': environment.user, 'password': environment.password
    }, this.httpOptions).subscribe(resultado => {
      localStorage.setItem('token', JSON.stringify(resultado));
      this.currentUserSubject.next(resultado);
    });
  }
}
