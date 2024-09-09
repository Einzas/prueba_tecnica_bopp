import { Component, inject } from '@angular/core';
import { AccesoService } from '../../services/acceso.service';
import { Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Login } from '../../interfaces/Login';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
})
export class LoginComponent {
  private accesoService = inject(AccesoService);

  private router = inject(Router);
  public formBuild = inject(FormBuilder);

  public formLogin: FormGroup = this.formBuild.group({
    correo: ['', Validators.required],
    contrasena: ['', Validators.required],
  });

  iniciarSesion() {
    if (this.formLogin.invalid) {
      return;
    }
    const objeto: Login = {
      correo: this.formLogin.value.correo,
      contrasena: this.formLogin.value.contrasena,
    };

    this.accesoService.login(objeto).subscribe({
      next: (data) => {
        if (data.status === 200) {
          localStorage.setItem('token', data.token);
          this.router.navigate(['/inicio']);
        } else {
          alert('Error al iniciar sesión');
        }
      },
      error: (error) => {
        console.log(error.message);
      },
    });
  }

  registro() {
    this.router.navigate(['/registro']);
  }
}
