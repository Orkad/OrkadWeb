import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
  MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';
import * as moment from 'moment';
import { TransactionService } from 'src/services/transaction.service';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';

@Component({
  selector: 'app-expense-form-dialog',
  templateUrl: './expense-form-dialog.component.html',
  styleUrls: ['./expense-form-dialog.component.scss'],
})
export class ExpenseFormDialogComponent {
  loading: boolean;
  formGroup = new FormGroup({
    date: new FormControl(moment(), {
      nonNullable: true,
      validators: [Validators.required],
    }),
    name: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    amount: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0.01)],
    }),
  });
  get date() {
    return this.formGroup.controls.date;
  }
  get name() {
    return this.formGroup.controls.name;
  }
  get amount() {
    return this.formGroup.controls.amount;
  }
  constructor(
    public dialogRef: MatDialogRef<ExpenseFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) protected expense: TransactionRow | null,
    private transactionService: TransactionService
  ) {
    if (expense) {
      this.date.setValue(moment(expense.date));
      this.name.setValue(expense.name);
      this.amount.setValue(-expense.amount);
    }
  }

  save() {
    if (this.formGroup.invalid) {
      return;
    }
    this.loading = true;
    const formData = {
      id: this.expense?.id ?? 0,
      amount: this.amount.value,
      name: this.name.value,
      date: this.date.value.toDate(),
    };
    if (!this.expense) {
      this.expense = <TransactionRow>{};
      this.transactionService
        .addExpense(formData)
        .subscribe((data) => this.applyAndClose(data.id));
    } else {
      this.transactionService
        .updateExpense(formData)
        .subscribe(() => this.applyAndClose());
    }
  }

  applyAndClose(id: number | null = null) {
    if (this.expense) {
      if (id) this.expense.id = id;
      this.expense.date = this.date.value.toDate();
      this.expense.name = this.name.value;
      this.expense.amount = -this.amount.value;
    }
    this.dialogRef.close(this.expense);
  }
}
