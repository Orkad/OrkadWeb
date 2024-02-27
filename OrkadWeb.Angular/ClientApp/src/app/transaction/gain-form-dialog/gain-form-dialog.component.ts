import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
  MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';
import * as moment from 'moment';
import {
  AddTransactionGainCommand,
  TransactionClient,
} from 'src/app/web-api-client';
import { TransactionService } from 'src/services/transaction.service';
import { Gain } from 'src/shared/models/transactions/Gain';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';

@Component({
  selector: 'app-gain-form-dialog',
  templateUrl: './gain-form-dialog.component.html',
  styleUrls: ['./gain-form-dialog.component.scss'],
})
export class GainFormDialogComponent {
  transactionId: number | null;
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

  constructor(
    public dialogRef: MatDialogRef<GainFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) gain: Gain | null,
    private transactionService: TransactionService,
    private transactionClient: TransactionClient
  ) {
    if (gain) {
      this.formGroup.patchValue({
        amount: gain.amount,
        date: moment(gain.date),
        name: gain.name,
      });
      this.transactionId = gain.id;
    }
  }

  private formToGain() {
    const controls = this.formGroup.controls;
    return <Gain>{
      id: this.transactionId,
      date: controls.date.value.toDate(),
      name: controls.name.value,
      amount: controls.amount.value,
    };
  }

  save() {
    if (this.formGroup.invalid) {
      return;
    }
    this.loading = true;
    const gain = this.formToGain();
    if (this.transactionId) {
      this.transactionService
        .updateGain(gain)
        .subscribe(() => this.close(gain));
    } else {
      const command = new AddTransactionGainCommand({
        name: gain.name,
        amount: gain.amount,
        date: gain.date,
      });
      this.transactionClient.addGain(command).subscribe((id) => {
        gain.id = id;
        this.close(gain);
      });
      this.transactionService.addGain(gain).subscribe((id) => {
        gain.id = id;
        this.close(gain);
      });
    }
  }

  close(gain: Gain) {
    this.dialogRef.close(gain);
  }
}
