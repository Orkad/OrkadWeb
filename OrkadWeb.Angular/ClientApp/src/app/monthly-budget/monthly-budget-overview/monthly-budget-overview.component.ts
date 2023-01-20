import { Component, Input, OnInit } from '@angular/core';
import Enumerable from 'linq';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';
import { MonthlyIncome } from 'src/shared/models/MonthlyIncome';

@Component({
  selector: 'app-monthly-budget-overview[charges][incomes]', // charges & incomes required
  templateUrl: './monthly-budget-overview.component.html',
  styleUrls: ['./monthly-budget-overview.component.scss'],
})
export class MonthlyBudgetOverviewComponent implements OnInit {
  @Input() charges: MonthlyCharge[];
  @Input() incomes: MonthlyIncome[];

  constructor() {}

  ngOnInit(): void {}

  totalCharges() {
    return Enumerable.from(this.charges).sum((c) => c.amount);
  }

  totalIncomes() {
    return Enumerable.from(this.incomes).sum((i) => i.amount);
  }

  totalBudget() {
    return this.totalIncomes() - this.totalCharges();
  }
}
