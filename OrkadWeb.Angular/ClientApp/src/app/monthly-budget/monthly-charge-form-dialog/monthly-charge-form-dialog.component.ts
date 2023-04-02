import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatLegacyDialogRef as MatDialogRef, MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA } from '@angular/material/legacy-dialog';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';

@Component({
  selector: 'app-monthly-charge-form-dialog',
  templateUrl: './monthly-charge-form-dialog.component.html',
  styleUrls: ['./monthly-charge-form-dialog.component.scss'],
})
export class MonthlyChargeFormDialogComponent {
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
    public dialogRef: MatDialogRef<MonthlyChargeFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public charge: MonthlyCharge | null
  ) {
    if (charge) {
      this.name.setValue(charge.name);
      this.amount.setValue(charge.amount);
    }
  }

  save() {
    if (this.formGroup.invalid) {
      return;
    }
    if (!this.charge) {
      this.charge = <MonthlyCharge>{};
    }
    this.charge.name = this.name.value;
    this.charge.amount = this.amount.value;
    this.dialogRef.close(this.charge);
  }
}
