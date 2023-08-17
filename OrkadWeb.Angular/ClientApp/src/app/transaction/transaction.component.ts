import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import * as moment from 'moment';
import { Moment } from 'moment';
import { map } from 'rxjs';
import { TransactionService } from 'src/services/transaction.service';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';
import { ExpenseFormDialogComponent } from './expense-form-dialog/expense-form-dialog.component';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  currentMonth = moment();
  month = new FormControl<Moment>(moment(), { nonNullable: true });

  dataSource = new MatTableDataSource<TransactionRow>();
  loaded = false;
  displayedColumns = ['date', 'name', 'amount', 'actions'];

  constructor(
    private expenseService: TransactionService,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.refreshExpenses();
    this.month.valueChanges.subscribe(() => this.refreshExpenses());
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
            const index = this.dataSource.data.indexOf(row, 0);
            if (index > -1) {
              this.dataSource.data.splice(index, 1);
              this.dataSource._updateChangeSubscription();
            }
          });
        }
      });
  }

  addExpense() {
    this.openExpenseDialog().subscribe((expense) => {
      if (expense) {
        this.dataSource.data.push(expense);
        this.dataSource._updateChangeSubscription();
      }
    });
  }

  editExpense(expense: TransactionRow) {
    this.openExpenseDialog(expense);
  }

  private openExpenseDialog(expense: TransactionRow | null = null) {
    return this.dialogService.dialog
      .open(ExpenseFormDialogComponent, {
        data: expense,
      })
      .afterClosed();
  }
}
