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
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';

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

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    this.refreshExpenses();
    this.month.valueChanges.subscribe(() => this.refreshExpenses());
  }

  private refreshExpenses() {
    this.dataSource.data = [];
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

  addExpense() {
    this.addExpenseFormVisible = false;
    let command = this.addExpenseFormGroup.value as AddExpenseCommand;
    this.expenseService.add(command).subscribe(() => {
      this.refreshExpenses();
    });
  }
}

export interface AddExpenseFormGroup {
  date: FormControl<Date>;
  amount: FormControl<number>;
  name: FormControl<string>;
}
