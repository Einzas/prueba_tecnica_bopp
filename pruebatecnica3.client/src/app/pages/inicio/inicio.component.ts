import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from '../../interfaces/Usuario';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css',
  standalone: true,
  imports: [MatCardModule, MatTableModule],
})
export class InicioComponent {
  private usuariosService = inject(UsuarioService);
  public usuarios: Usuario[] = [];
  public displayedColumns: string[] = [
    'nombre',
    'apellido',
    'dni',
    'fechaNacimiento',
    'email',
    'rol',
    'estado',
  ];

  constructor() {
    this.usuariosService.listar().subscribe({
      next: (data) => {
        if (data.data.length > 0) {
          this.usuarios = data.data;
        }
      },
      error: (error) => {
        console.log(error.message);
      },
    });
  }
}
