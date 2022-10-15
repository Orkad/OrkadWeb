export interface MonthlyCharges {
  rows: MonthlyCharge[];
}

export interface MonthlyCharge {
  id: number;
  name: string;
  amount: number;
}
