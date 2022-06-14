import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css'],
})
export class TransactionComponent implements OnInit {
  month: UntypedFormControl = new UntypedFormControl(new Date());

  dataSource = new MatTableDataSource<ExpenseRow>([
    { amount: 1, name: 'test', date: new Date() } as ExpenseRow,
  ]);
  displayedColumns = ['date', 'name', 'amount'];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    this.refreshExpenses();
    this.month.valueChanges.subscribe(() => this.refreshExpenses());
  }

  private refreshExpenses() {
    this.expenseService.getMonthly(this.month.value).subscribe((data) => {
      this.dataSource.data = data.rows;
    });
  }
}
