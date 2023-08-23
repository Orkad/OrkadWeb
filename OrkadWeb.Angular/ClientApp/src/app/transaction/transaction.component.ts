import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { Moment } from 'moment';
import { map } from 'rxjs';
import { TransactionService } from 'src/services/transaction.service';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';
import { ExpenseFormDialogComponent } from './expense-form-dialog/expense-form-dialog.component';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit, AfterViewInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  currentMonth = moment();
  month = new FormControl<Moment>(moment(), { nonNullable: true });

  dataSource = new MatTableDataSource<TransactionRow>();
  loaded = false;
  displayedColumns = ['date', 'name', 'amount', 'actions'];
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private expenseService: TransactionService,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.refreshExpenses();
    this.month.valueChanges.subscribe(() => this.refreshExpenses());
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    console.log(this.sort);
  }

  private refreshExpenses() {
    if (this.month.value == null) {
      return;
    }
    this.expenseService.getMonthly(this.month.value).subscribe((rows) => {
      this.dataSource.data = rows;
      this.loaded = true;
    });
  }

  canNextMonth() {
    return (
      !!this.month.value && moment(this.month.value) < moment(this.currentMonth)
    );
  }

  deleteExpense(row: TransactionRow) {
    this.dialogService
      .confirm({
        text: 'Supprimer la dépense ' + row.name + ' de ' + row.amount + '€ ?',
      } as ConfirmDialogData)
      .subscribe((ok) => {
        if (ok) {
          this.expenseService.delete(row.id).subscribe(() => {
            this.dataSource.data.remove(row);
            this.dataSource._updateChangeSubscription();
          });
        }
      });
  }

  addExpense() {
    this.openExpenseDialog().subscribe((expense: TransactionRow) => {
      if (expense) {
        if (moment(expense.date).isSame(this.month.value, 'month')) {
          this.dataSource.data.push(expense);
          this.dataSource._updateChangeSubscription();
        }
      }
    });
  }

  editExpense(expense: TransactionRow) {
    this.openExpenseDialog(expense).subscribe((expense) => {
      if (!moment(expense.date).isSame(this.month.value, 'month')) {
        this.dataSource.data.remove(expense);
        this.dataSource._updateChangeSubscription();
      }
    });
  }

  private openExpenseDialog(expense: TransactionRow | null = null) {
    return this.dialogService.dialog
      .open(ExpenseFormDialogComponent, {
        data: expense,
      })
      .afterClosed();
  }
}
