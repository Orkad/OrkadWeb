export interface MonthlyCharges {
  items: MonthlyCharge[];
}

export interface MonthlyCharge {
  id: number;
  name: string;
  amount: number;
}
