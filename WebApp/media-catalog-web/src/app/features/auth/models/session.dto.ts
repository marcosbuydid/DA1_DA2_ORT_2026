import { UserDetailDTO } from "./user-detail.dto";

export class SessionDTO {
  token: string = '';
  loggedUser!: UserDetailDTO;
  loggedUserRoleName: string = '';
}