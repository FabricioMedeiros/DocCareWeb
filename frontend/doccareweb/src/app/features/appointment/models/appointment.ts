import { Doctor } from "./doctor";
import { HealthPlan } from "./healthplan";
import { Patient } from "./patient";
import { AppointmentStatus } from "./appointment-status";
import { AppointmentItem } from "./appointment-item";

export interface AppointmentBase{
    appointmentDate: Date;
    startTime: string;
    endTime: string;
    status: AppointmentStatus;
    cost: number;
    notes: string;
    items: AppointmentItem[];
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