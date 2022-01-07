import { Role } from ".";

export class User {

    id: number;
    name: string;
    password: string;
    email: string;
    role: Role;
    token?: string;

}
