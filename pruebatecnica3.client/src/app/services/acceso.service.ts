import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { appsettings } from '../settings/appsettings';
import { Usuario } from '../interfaces/Usuario';
import { Observable } from 'rxjs';
import { ResponseLogin } from '../interfaces/ResponseLogin';
import { Login } from '../interfaces/Login';

@Injectable({
  providedIn: 'root',
})
export class AccesoService {
  private http = inject(HttpClient);
  private baseUrl: string = appsettings.apiUrl;
  constructor() {}

  registrarse(objeto: Usuario): Observable<ResponseLogin> {
    return this.http.post<ResponseLogin>(
      `${this.baseUrl}Usuarios/registro`,
      objeto
    );
  }

  login(objeto: Login): Observable<ResponseLogin> {
    return this.http.post<ResponseLogin>(
      `${this.baseUrl}Usuarios/login`,
      objeto
    );
  }
}
