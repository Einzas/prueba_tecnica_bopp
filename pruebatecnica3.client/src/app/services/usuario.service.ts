import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { appsettings } from '../settings/appsettings';
import { Observable } from 'rxjs';
import { Usuario } from '../interfaces/Usuario';
import { ResponseUsuarios } from '../interfaces/ResponseUsuarios';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private http = inject(HttpClient);
  private baseUrl: string = appsettings.apiUrl;
  constructor() {}

  listar(): Observable<ResponseUsuarios> {
    return this.http.get<ResponseUsuarios>(`${this.baseUrl}Usuarios/listar`);
  }
}
