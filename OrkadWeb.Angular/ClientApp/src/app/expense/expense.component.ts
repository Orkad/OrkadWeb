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
  test: number;
  formBuilder: IFormBuilder;
  formGroup: IFormGroup<ExpenseAddQuery>;

  constructor(formBuilder: FormBuilder) {
    this.formBuilder = formBuilder;
  }

  ngOnInit() {
    this.formGroup = this.formBuilder.group<ExpenseAddQuery>({
      date: [new Date()],
      amount: [null, [Validators.min(0.01), Validators.max(10000)]],
    });
  }
}
