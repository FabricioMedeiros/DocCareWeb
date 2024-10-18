import { Address } from './address';
import { Gender } from "./gender.enum";
import { HealthPlan } from './healthplan';

export interface PatientBase {
    name: string;
    cpf: string;
    rg: string;
    gender: Gender;
    birthDate: Date;
    email: string;
    phone: string;
    cellPhone: string;
    notes: string;
    Address: Address;
}

export interface Patient extends PatientBase {
    id: number;
    healthPlanId: number;
}

export interface PatientList extends PatientBase {
    id: number;
    healthPlan: HealthPlan;
}