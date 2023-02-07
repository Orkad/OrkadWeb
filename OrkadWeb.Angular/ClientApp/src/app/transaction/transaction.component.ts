import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { Moment } from 'moment';
import { map } from 'rxjs';
import { AddExpenseCommand } from 'src/api/commands/AddExpenseCommand';
import { UpdateExpenseCommand } from 'src/api/commands/UpdateExpenseCommand';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';
import { ExpenseFormDialogComponent } from './expense-form-dialog/expense-form-dialog.component';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  currentMonth = moment();
  month = new FormControl<Moment>(moment());

  dataSource = new MatTableDataSource<ExpenseRow>();
  loaded = false;
  displayedColumns = ['date', 'name', 'amount', 'actions'];

  constructor(
    private expenseService: ExpenseService,
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
    this.expenseService.getMonthly(this.month.value).subscribe((data) => {
      this.dataSource.data = data.rows;
      this.loaded = true;
    });
  }

  canNextMonth() {
    return (
      !!this.month.value && moment(this.month.value) < moment(this.currentMonth)
    );
  }

  deleteExpense(row: ExpenseRow) {
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
        this.expenseService
          .add(expense)
          .pipe(map((r) => (expense.id = r.id)))
          .subscribe();
        this.dataSource.data.push(expense);
        this.dataSource._updateChangeSubscription();
      }
    });
  }

  editExpense(expense: ExpenseRow) {
    this.openExpenseDialog(expense).subscribe((data) => {
      if (data) {
        this.expenseService.update(expense).subscribe();
      }
    });
  }

  private openExpenseDialog(expense: ExpenseRow | null = null) {
    return this.dialogService.dialog
      .open(ExpenseFormDialogComponent, {
        data: expense,
      })
      .afterClosed();
  }
}
