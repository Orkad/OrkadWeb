import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { date, RxwebValidators } from '@rxweb/reactive-form-validators';
import { IFormBuilder, IFormGroup } from '@rxweb/types';
import { ExpenseService } from 'src/services/expense.service';
import { AddExpenseCommand } from '../../shared/models/expenses/AddExpenseCommand';

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html',
  styleUrls: ['./expense.component.scss'],
})
export class ExpenseComponent implements OnInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  formBuilder: IFormBuilder;
  formGroup: IFormGroup<AddExpenseCommand>;

  constructor(
    formBuilder: FormBuilder,
    private expenseService: ExpenseService
  ) {
    this.formBuilder = formBuilder;
  }

  ngOnInit() {
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
    });
  }

  get amount() {
    return this.formGroup.controls.amount as FormControl;
  }

  get date() {
    return this.formGroup.controls.date as FormControl;
  }

  submitNewExpense() {
    this.expenseService
      .add(this.formGroup.value)
      .subscribe((data) => console.log(data));
  }
}
