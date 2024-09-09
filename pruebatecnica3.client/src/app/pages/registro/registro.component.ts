import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AccesoService } from '../../services/acceso.service';
import { Router } from '@angular/router';
import { Usuario } from '../../interfaces/Usuario';
@Component({
  selector: 'app-registro',
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
})
export class RegistroComponent {
  private accesoService = inject(AccesoService);
  private router = inject(Router);

  public formBuild = inject(FormBuilder);

  public formRegistro: FormGroup = this.formBuild.group({
    nombre: ['', Validators.required],
    apellido: ['', Validators.required],
    dni: ['', Validators.required],
    fechaNacimiento: ['', Validators.required],
    email: ['', Validators.required],
    contrasena: ['', Validators.required],
  });

  registarse() {
    if (this.formRegistro.invalid) {
      return;
    }

    const objeto: Usuario = {
      nombre: this.formRegistro.value.nombre,
      apellido: this.formRegistro.value.apellido,
      dni: this.formRegistro.value.dni,
      fechaNacimiento: this.formRegistro.value.fechaNacimiento,
      email: this.formRegistro.value.email,
      contrasena: this.formRegistro.value.contrasena,
      estado: true, // O el valor correspondiente
      rol: 'user',
    };

    this.accesoService.registrarse(objeto).subscribe({
      next: (data) => {
        if (data.status === 200) {
          localStorage.setItem('token', data.token);
          this.router.navigate(['/inicio']);
        } else {
          alert('Error al registrarse');
        }
      },
      error: (error) => {
        console.log(error.message);
      },
    });
  }
  login() {
    this.router.navigate(['/login']);
  }
}
