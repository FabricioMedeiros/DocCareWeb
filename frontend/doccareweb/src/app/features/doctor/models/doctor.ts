import { Specialty } from "./specialty";

export interface DoctorBase {
    name: string;
    crm?: string;
    email?: string;
    cellPhone?: string;
    phone?: string;
  }

  export interface Doctor extends DoctorBase {
    id: number; 
    specialtyId: number; 
  }

  export interface DoctorList extends DoctorBase {
    id: number; 
    specialty: Specialty; 
  }