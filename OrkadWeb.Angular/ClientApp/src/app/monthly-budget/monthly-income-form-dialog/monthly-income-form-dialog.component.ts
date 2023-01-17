import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MonthlyIncome } from 'src/shared/models/MonthlyIncome';

@Component({
  selector: 'app-monthly-income-form-dialog',
  templateUrl: './monthly-income-form-dialog.component.html',
  styleUrls: ['./monthly-income-form-dialog.component.scss'],
})
export class MonthlyIncomeFormDialogComponent {
  formGroup = new FormGroup({
    name: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    amount: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0.01)],
    }),
  });

  get name() {
    return this.formGroup.controls.name;
  }
  get amount() {
    return this.formGroup.controls.amount;
  }

  constructor(
    public dialogRef: MatDialogRef<MonthlyIncomeFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public income: MonthlyIncome | null
  ) {
    if (income) {
      this.name.setValue(income.name);
      this.amount.setValue(income.amount);
    }
  }

  save() {
    if (this.formGroup.invalid) {
      return;
    }
    if (!this.income) {
      this.income = <MonthlyIncome>{};
    }
    this.income.name = this.name.value;
    this.income.amount = this.amount.value;
    this.dialogRef.close(this.income);
  }
}
