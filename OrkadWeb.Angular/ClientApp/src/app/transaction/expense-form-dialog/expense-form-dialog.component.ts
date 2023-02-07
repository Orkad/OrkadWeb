import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';

@Component({
  selector: 'app-expense-form-dialog',
  templateUrl: './expense-form-dialog.component.html',
  styleUrls: ['./expense-form-dialog.component.scss'],
})
export class ExpenseFormDialogComponent {
  formGroup = new FormGroup({
    date: new FormControl(new Date(), {
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
    @Inject(MAT_DIALOG_DATA) public expense: ExpenseRow | null
  ) {
    if (expense) {
      this.date.setValue(expense.date);
      this.name.setValue(expense.name);
      this.amount.setValue(expense.amount);
    }
  }

  save() {
    if (this.formGroup.invalid) {
      return;
    }
    if (!this.expense) {
      this.expense = <ExpenseRow>{};
    }
    this.expense.date = this.date.value;
    this.expense.name = this.name.value;
    this.expense.amount = this.amount.value;
    this.dialogRef.close(this.expense);
  }
}
