export interface Usuario {
  dni: string;
  nombre: string;
  apellido: string;
  email: string;
  contrasena: string;
  fechaNacimiento: Date;
  estado: boolean;
  rol: string;
}
