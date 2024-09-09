import { Usuario } from './Usuario';

export interface ResponseLogin {
  token: string;
  data: Usuario;
  status: number;
}
