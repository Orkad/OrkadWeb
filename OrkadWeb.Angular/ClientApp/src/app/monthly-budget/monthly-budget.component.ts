import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MonthlyIncome } from 'src/shared/models/MonthlyIncome';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';
import { MonthlyTransactionService } from 'src/services/monthly-transaction.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MonthlyChargeFormDialogComponent } from './monthly-charge-form-dialog/monthly-charge-form-dialog.component';
import { MonthlyIncomeFormDialogComponent } from './monthly-income-form-dialog/monthly-income-form-dialog.component';

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
    this.monthlyTransactionService.charges().subscribe((data) => {
      this.charges.data = data;
      this.chargesLoaded = true;
    });
    this.monthlyTransactionService.incomes().subscribe((data) => {
      this.incomes.data = data;
      this.incomesLoaded = true;
    });
  }

  charges = new MatTableDataSource<MonthlyCharge>();
  chargesLoaded = false;
  incomes = new MatTableDataSource<MonthlyIncome>();
  incomesLoaded = false;
  displayedColumns = ['name', 'amount', 'actions'];

  chargeFg = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number | null>(null, [
      Validators.required,
      Validators.min(0.01),
    ]),
  });

  editedIncome: MonthlyIncome;
  addChargeVisible: boolean;

  addCharge() {
    this.openChargeDialog().subscribe((data) => {
      if (data) {
        this.monthlyTransactionService.addCharge(data).subscribe();
        this.charges.data.push(data);
        this.charges._updateChangeSubscription();
      }
    });
  }

  editCharge(charge: MonthlyCharge) {
    this.openChargeDialog(charge).subscribe((data) => {
      if (data) {
        this.monthlyTransactionService.editCharge(data).subscribe();
      }
    });
  }

  deleteCharge(charge: MonthlyCharge) {
    this.dialogService
      .confirm({
        text: `Confirmer la suppression de ${charge.name} ?`,
      })
      .subscribe((ok) => {
        if (ok) {
          this.charges.data.remove(charge);
          this.charges._updateChangeSubscription();
          this.monthlyTransactionService.deleteCharge(charge.id).subscribe({
            error: () => this.charges.data.push(charge),
          });
        }
      });
  }
  private openChargeDialog(charge: MonthlyCharge | null = null) {
    return this.dialogService.dialog
      .open(MonthlyChargeFormDialogComponent, {
        data: charge,
      })
      .afterClosed();
  }

  addIncome() {
    this.openIncomeDialog().subscribe((data) => {
      if (data) {
        this.monthlyTransactionService.addIncome(data).subscribe();
        this.incomes.data.push(data);
        this.incomes._updateChangeSubscription();
      }
    });
  }
  editIncome(income: MonthlyIncome) {
    const old = structuredClone(income);
    this.openIncomeDialog(income).subscribe((data) => {
      if (data) {
        this.monthlyTransactionService.editIncome(data).subscribe({
          error: () => {
            this.incomes.data.replace(data, old);
            this.incomes._updateChangeSubscription();
          },
        });
      }
    });
  }

  deleteIncome(income: MonthlyIncome) {
    this.dialogService
      .confirm({
        text: `Confirmer la suppression de ${income.name} ?`,
      })
      .subscribe((ok) => {
        if (ok) {
          this.incomes.data.remove(income);
          this.incomes._updateChangeSubscription();
          this.monthlyTransactionService.deleteIncome(income.id).subscribe({
            error: () => this.charges.data.push(income),
          });
        }
      });
  }
  private openIncomeDialog(income: MonthlyIncome | null = null) {
    return this.dialogService.dialog
      .open(MonthlyIncomeFormDialogComponent, {
        data: income,
      })
      .afterClosed();
  }
}
