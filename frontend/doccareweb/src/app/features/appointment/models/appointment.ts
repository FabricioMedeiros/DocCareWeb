import { Doctor } from "./doctor";
import { HealthPlan } from "./healthplan";
import { Patient } from "./patient";
import { AppointmentStatus } from "./appointment-status";

export interface AppointmentBase{
    appointmentDate: Date;
    appointmentTime: string;
    status: AppointmentStatus;
    cost: number;
    notes: string;
}

export interface Appointment extends AppointmentBase{
    id: number;
    doctorId: number;
    patientId: number;
    healthPlanId: number;
}

export interface AppointmentList extends AppointmentBase{
    id: number;
    doctor: Doctor;
    patient: Patient;
    healthPlan: HealthPlan;
}