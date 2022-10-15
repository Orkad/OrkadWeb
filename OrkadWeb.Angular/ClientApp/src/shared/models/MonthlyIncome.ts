export interface MonthlyIncomes {
  rows: MonthlyIncome[];
}

export interface MonthlyIncome {
  id: number;
  name: string;
  amount: number;
}
