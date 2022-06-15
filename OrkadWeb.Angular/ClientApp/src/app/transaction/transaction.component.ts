import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, UntypedFormControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  currentMonth = new Date();
  month = new FormControl<Date>(new Date());

  dataSource = new MatTableDataSource<ExpenseRow>([
    { amount: 1, name: 'test', date: new Date() } as ExpenseRow,
  ]);
  displayedColumns = ['date', 'name', 'amount', 'actions'];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    this.refreshExpenses();
    this.month.valueChanges.subscribe(() => this.refreshExpenses());
  }

  private refreshExpenses() {
    this.dataSource.data = [];
    if (this.month.value == null) {
      return;
    }
    this.expenseService.getMonthly(this.month.value).subscribe((data) => {
      this.dataSource.data = data.rows;
    });
  }

  canNextMonth() {
    return (
      !!this.month.value && moment(this.month.value) < moment(this.currentMonth)
    );
  }
}
