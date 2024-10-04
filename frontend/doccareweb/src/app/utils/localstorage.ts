import { jwtDecode } from "jwt-decode";
import { JwtToken } from "../features/account/models/jtw.token";

export class LocalStorageUtils {

    public getUser() {
        const userJson = localStorage.getItem('doccareweb.user');
        return userJson ? JSON.parse(userJson) : null;
    }

    public saveLocalUserData(response: any) {
        if (response && response.token) {
            this.saveTokenUser(response.token);
    
            try {
                const decodedToken = jwtDecode<JwtToken>(response.token);
            
                this.saveUser(decodedToken.username);
                this.saveEmailUser(decodedToken.email);
            } catch (error) {
                console.error('Erro ao decodificar o token:', error);
            }
        } else {
            console.error('Token inválido:', response);
        }
    }    

    public clearLocalUserData() {
        localStorage.removeItem('doccareweb.token');
        localStorage.removeItem('doccareweb.user');
        localStorage.removeItem('doccareweb.email');
    }

    public getTokenUser(): string | null {
        return localStorage.getItem('doccareweb.token');
    }

    public getEmailUser(): string | null {
        return localStorage.getItem('doccareweb.email');
    }

    public saveTokenUser(token: string) {
        localStorage.setItem('doccareweb.token', token);
    }

    public saveUser(user: string) {
        localStorage.setItem('doccareweb.user', JSON.stringify(user));
    }

    public saveEmailUser(email: string) {
        localStorage.setItem('doccareweb.email', JSON.stringify(email));
    }

}