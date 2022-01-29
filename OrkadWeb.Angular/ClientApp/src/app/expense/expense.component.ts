import { Component, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { date, RxwebValidators } from "@rxweb/reactive-form-validators";
import { IFormBuilder, IFormGroup } from "@rxweb/types";
import { ExpenseAddQuery } from "./expense-add-query";

@Component({
  selector: "app-expense",
  templateUrl: "./expense.component.html",
  styleUrls: ["./expense.component.scss"],
})
export class ExpenseComponent implements OnInit {
  readonly minAmount = 0.01;
  readonly maxAmount = 10000;

  test: number;
  formBuilder: IFormBuilder;
  formGroup: IFormGroup<ExpenseAddQuery>;

  constructor(formBuilder: FormBuilder) {
    this.formBuilder = formBuilder;
  }

  ngOnInit() {
    this.formGroup = this.formBuilder.group<ExpenseAddQuery>({
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
    return this.formGroup.controls.amount;
  }

  get date() {
    return this.formGroup.controls.date;
  }
}
