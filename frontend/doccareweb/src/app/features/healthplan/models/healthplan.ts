import { HealthPlanItem } from "./healthplanitem";

export interface HealthPlan {
  id: number;
  name: string;
  items: HealthPlanItem[];
}