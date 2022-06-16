import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  UntypedFormControl,
  Validators,
} from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { AddExpenseCommand } from 'src/api/commands/AddExpenseCommand';
import { UpdateExpenseCommand } from 'src/api/commands/UpdateExpenseCommand';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';
import { ConfirmDialogData } from '../shared/dialog/confirm-dialog/confirm-dialog.data';
import { DialogService } from '../shared/dialog/dialog.service';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  currentMonth = new Date();
  month = new FormControl<Date>(new Date());

  dataSource = new MatTableDataSource<ExpenseRow>([
    { amount: 1, name: 'test', date: new Date() } as ExpenseRow,
  ]);
  displayedColumns = ['date', 'name', 'amount', 'actions'];
  editedRow: ExpenseRow | null;
  addExpenseFormVisible = false;
  addExpenseFormGroup = new FormGroup<AddExpenseFormGroup>({
    amount: new FormControl<number>(0, {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.min(this.minAmount),
        Validators.max(this.maxAmount),
      ],
    }),
    date: new FormControl<Date>(new Date(), {
      nonNullable: true,
      validators: [Validators.required],
    }),
    name: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  get addExpenseControls() {
    return this.addExpenseFormGroup.controls;
  }

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
    });
  }

  canNextMonth() {
    return (
      !!this.month.value && moment(this.month.value) < moment(this.currentMonth)
    );
  }

  beginAddExpense() {
    this.addExpenseFormGroup.reset();
    this.addExpenseFormVisible = true;
  }

  beginEditExpense(row: ExpenseRow) {
    this.addExpenseFormGroup.controls.amount.setValue(row.amount);
    this.addExpenseFormGroup.controls.date.setValue(row.date);
    this.addExpenseFormGroup.controls.name.setValue(row.name);
    this.editedRow = row;
    this.addExpenseFormVisible = true;
  }

  endAddOrEditExpense() {
    this.editedRow = null;
    this.addExpenseFormVisible = false;
  }

  saveExpense() {
    if (!this.editedRow) {
      let command = this.addExpenseFormGroup.value as AddExpenseCommand;
      this.expenseService.add(command).subscribe(() => {
        this.refreshExpenses();
      });
    } else {
      let command = this.addExpenseFormGroup.value as UpdateExpenseCommand;
      command.id = this.editedRow.id;
      this.expenseService.update(command).subscribe(() => {
        this.refreshExpenses();
      });
    }
    this.endAddOrEditExpense();
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
}

export interface AddExpenseFormGroup {
  date: FormControl<Date>;
  amount: FormControl<number>;
  name: FormControl<string>;
}
