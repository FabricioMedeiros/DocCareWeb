import { FinancialSummary } from "./financial-summary";

export interface Dashboard {
    totalPatients: number;
    appointmentsByStatus: { [key: string]: number };
    dailyFinancialSummary: FinancialSummary;
    monthlyFinancialSummary: FinancialSummary;
}