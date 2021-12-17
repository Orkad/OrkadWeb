import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: "app-expense",
  templateUrl: "./expense.component.html",
  styleUrls: ["./expense.component.scss"],
})
export class ExpenseComponent implements OnInit {
  addExpenseFormGroup = new FormGroup({
    amount: new FormControl(""),
    date: new FormControl(new Date()),
  });

  constructor() {}

  ngOnInit() {}
}
