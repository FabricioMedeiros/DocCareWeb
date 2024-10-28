import { Time } from "@angular/common";
import { Doctor } from "./doctor";
import { HealthPlan } from "./healthplan";
import { Patient } from "./patient";
import { AppointmentStatus } from "./appointment-status";

export interface Appointment{
    id: number;
    date: Date;
    time: string;
    status: AppointmentStatus;
    doctor: Doctor;
    patient: Patient;
    healthPlan: HealthPlan;
    cost: number;
    notes: string;
}