import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MonthlyTransaction } from 'src/shared/models/MonthlyTransaction';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';

@Component({
  selector: 'app-monthly-budget',
  templateUrl: './monthly-budget.component.html',
  styleUrls: ['./monthly-budget.component.scss'],
})
export class MonthlyBudgetComponent implements OnInit {
  constructor(private dialogService: DialogService) {}

  ngOnInit(): void {}

  monthlyExpenses = new MatTableDataSource<MonthlyTransaction>([
    { id: 1, amount: 1085, name: 'loyer' } as MonthlyTransaction,
  ]);

  monthlyRevenus = new MatTableDataSource<MonthlyTransaction>([
    { id: 2, amount: 2802, name: 'salaire' } as MonthlyTransaction,
  ]);
  displayedColumns = ['name', 'amount', 'actions'];

  delete(transaction: MonthlyTransaction) {
    this.dialogService
      .confirm({
        text:
          'Confirmer la suppression de ' +
          transaction.name +
          " d'un montant de " +
          transaction.amount +
          '€ ?',
      } as ConfirmDialogData)
      .subscribe((ok) => {
        if (ok) {
          // TODO backend
          const index = this.monthlyExpenses.data.indexOf(transaction, 0);
          if (index > -1) {
            this.monthlyExpenses.data.splice(index, 1);
            this.monthlyExpenses._updateChangeSubscription();
            return;
          }
          this.monthlyRevenus.data.indexOf(transaction, 0);
          if (index > -1) {
            this.monthlyRevenus.data.splice(index, 1);
            this.monthlyRevenus._updateChangeSubscription();
            return;
          }
        }
      });
  }
}
