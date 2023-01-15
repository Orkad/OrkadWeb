import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MonthlyIncome } from 'src/shared/models/MonthlyIncome';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';
import { MonthlyTransactionService } from 'src/services/monthly-transaction.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-monthly-budget',
  templateUrl: './monthly-budget.component.html',
  styleUrls: ['./monthly-budget.component.scss'],
})
export class MonthlyBudgetComponent implements OnInit {
  constructor(
    private dialogService: DialogService,
    private monthlyTransactionService: MonthlyTransactionService
  ) {}

  ngOnInit(): void {
    this.monthlyTransactionService
      .incomes()
      .subscribe((data) => (this.incomes.data = data.rows));
    this.monthlyTransactionService.charges().subscribe((data) => {
      console.log(data);
      this.charges.data = data.items;
    });
  }

  charges = new MatTableDataSource<MonthlyCharge>();
  incomes = new MatTableDataSource<MonthlyIncome>();
  displayedColumns = ['name', 'amount', 'actions'];

  chargeFg = new FormGroup({
    name: new FormControl('', [Validators.required]),
    amount: new FormControl('', [Validators.required, Validators.min(0.01)]),
  });

  editedIncome: MonthlyIncome;
  editedCharge: MonthlyCharge;
  addChargeVisible: boolean;

  addCharge() {
    if (this.editedCharge) {
      return;
    }
    this.chargeFg.reset();
    this.editedCharge = <MonthlyCharge>{};
    this.charges.data.push(this.editedCharge);
    this.charges._updateChangeSubscription();
  }

  editCharge(charge: MonthlyCharge) {
    if (this.editedCharge) {
      return;
    }
    this.editedCharge = charge;
    this.chargeFg.controls.name.setValue(charge.name);
    this.chargeFg.controls.amount.setValue(charge.amount.toString());
  }

  saveCharge() {}

  deleteCharge(charge: MonthlyCharge) {
    this.dialogService
      .confirm({
        text:
          'Confirmer la suppression de ' +
          charge.name +
          " d'un montant de " +
          charge.amount +
          '€ ?',
      } as ConfirmDialogData)
      .subscribe((ok) => {
        if (ok) {
          this.charges.data = this.charges.data.filter((mc) => mc !== charge);
          this.charges._updateChangeSubscription();
          this.monthlyTransactionService.deleteCharge(charge.id).subscribe({
            error: () => this.charges.data.push(charge),
          });
        }
      });
  }

  deleteIncome(income: MonthlyIncome) {}

  beginAddCharge() {
    this.addChargeVisible = true;
  }

  beginEditCharge(charge: MonthlyCharge) {}

  beginEditIncome(income: MonthlyIncome) {}
}
