import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { date, RxwebValidators } from '@rxweb/reactive-form-validators';
import { IFormBuilder, IFormGroup } from '@rxweb/types';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';
import { AddExpenseCommand } from '../../shared/models/expenses/AddExpenseCommand';

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html',
  styleUrls: ['./expense.component.scss'],
})
export class ExpenseComponent implements OnInit, AfterViewInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  formBuilder: IFormBuilder;
  formGroup: IFormGroup<AddExpenseCommand>;

  displayedColumns = ['date', 'name', 'amount'];
  expenses = new MatTableDataSource<ExpenseRow>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    formBuilder: UntypedFormBuilder,
    private expenseService: ExpenseService
  ) {
    this.formBuilder = formBuilder;
  }

  ngOnInit() {
    this.resetForm();
    this.expenseService.getAll().subscribe((data) => {
      this.expenses.data = data.rows;
    });
  }

  ngAfterViewInit(): void {
    this.expenses.paginator = this.paginator;
    this.expenses.sort = this.sort;
  }

  resetForm() {
    this.formGroup = this.formBuilder.group<AddExpenseCommand>({
      date: [null, Validators.required],
      amount: [
        null,
        [
          Validators.required,
          Validators.min(this.minAmount),
          Validators.max(this.maxAmount),
        ],
      ],
      name: [null, Validators.required],
    });
  }

  get amount() {
    return this.formGroup.controls.amount as UntypedFormControl;
  }

  get date() {
    return this.formGroup.controls.date as UntypedFormControl;
  }

  get name() {
    return this.formGroup.controls.name as UntypedFormControl;
  }

  submitNewExpense() {
    let command = this.formGroup.value;
    this.expenseService.add(command).subscribe((r) => {
      this.resetForm();
      this.addExpense({
        id: r.id,
        amount: command?.amount,
        date: command?.date,
        name: command?.name,
      } as ExpenseRow);
    });
  }

  addExpense(expense: ExpenseRow) {
    this.expenses.data.unshift(expense);
  }
}
