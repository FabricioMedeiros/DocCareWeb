import { HealthPlan } from "./healthplan";

export interface Patient {
    id: number;
    name: string;
    cpf: string;
    phone: string;
    cellPhone: string;
    healthPlan: HealthPlan;
}