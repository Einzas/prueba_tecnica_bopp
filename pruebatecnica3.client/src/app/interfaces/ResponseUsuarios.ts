import { Usuario } from './Usuario';

export interface ResponseUsuarios {
  data: Usuario[];
  status: number;
  mensaje: string;
}
