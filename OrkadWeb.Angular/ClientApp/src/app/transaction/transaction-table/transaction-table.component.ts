import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { ConfirmDialogData } from 'src/app/shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from 'src/app/shared/dialog/dialog.service';
import { TransactionService } from 'src/services/transaction.service';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';
import { ExpenseFormDialogComponent } from '../expense-form-dialog/expense-form-dialog.component';
import { GainFormDialogComponent } from '../gain-form-dialog/gain-form-dialog.component';
import { Gain } from 'src/shared/models/transactions/Gain';

@Component({
  selector: 'app-transaction-table',
  templateUrl: './transaction-table.component.html',
  styleUrls: ['./transaction-table.component.scss'],
})
export class TransactionTableComponent
  implements OnInit, AfterViewInit, OnChanges
{
  // ### Binding properties ###
  /** the date representing the targeted month for displaying data */
  @Input() month = new Date();
  /** triggered when a transaction is added/updated/deleted */
  @Output() transactionChange = new EventEmitter<TransactionRow>();

  // ### UI properties ###
  dataSource = new MatTableDataSource<TransactionRow>();
  displayedColumns = ['date', 'name', 'amount', 'actions'];
  @ViewChild(MatSort) sort: MatSort;

  // ### Dependency injection ###
  constructor(
    private transactionService: TransactionService,
    private dialogService: DialogService
  ) {}

  // ### Lifecycle hooks ###
  ngOnInit(): void {
    this.refresh();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges): void {
    const monthChanges = changes['month'];
    if (monthChanges) {
      if (
        !moment(monthChanges.currentValue).isSame(monthChanges.previousValue)
      ) {
        this.refresh();
      }
    }
  }

  // ### UI Functions ###
  refresh() {
    this.transactionService.getMonthly(moment(this.month)).subscribe((rows) => {
      this.dataSource.data = rows;
    });
  }

  deleteExpense(row: TransactionRow) {
    this.dialogService
      .confirm({
        text: 'Supprimer la dépense ' + row.name + ' de ' + row.amount + '€ ?',
      } as ConfirmDialogData)
      .subscribe((ok) => {
        if (ok) {
          this.transactionService.delete(row.id).subscribe(() => {
            this.dataSource.data.remove(row);
            this.dataSource._updateChangeSubscription();
            this.transactionChange.emit(row);
          });
        }
      });
  }

  editExpense(expense: TransactionRow) {
    this.dialogService.dialog
      .open(ExpenseFormDialogComponent, {
        data: expense,
      })
      .afterClosed()
      .subscribe((expense) => {
        if (expense) {
          if (!moment(expense.date).isSame(moment(this.month), 'month')) {
            this.dataSource.data.remove(expense);
            this.dataSource._updateChangeSubscription();
          }
          this.transactionChange.emit(expense);
        }
      });
  }

  addExpense() {
    this.dialogService.dialog
      .open(ExpenseFormDialogComponent, {
        data: null,
      })
      .afterClosed()
      .subscribe((expense: TransactionRow) => {
        if (expense) {
          if (moment(expense.date).isSame(moment(this.month), 'month')) {
            this.dataSource.data.push(expense);
            this.dataSource._updateChangeSubscription();
            this.transactionChange.emit(expense);
          }
        }
      });
  }

  addGain() {
    this.dialogService.dialog
      .open(GainFormDialogComponent, {
        data: null,
      })
      .afterClosed()
      .subscribe((gain: Gain) => {
        if (gain && moment(gain.date).isSame(moment(this.month), 'month')) {
          const row = <TransactionRow>{
            id: gain.id,
            amount: gain.amount,
            date: gain.date,
            name: gain.name,
          };
          this.dataSource.data.push(row);
          this.dataSource._updateChangeSubscription();
          this.transactionChange.emit(row);
        }
      });
  }
}
