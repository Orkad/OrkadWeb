import { Component, OnInit } from '@angular/core';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';

export const MONTH_PICKER_FORMAT = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: NativeDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: MONTH_PICKER_FORMAT,
    },
  ],
})
export class TransactionComponent implements OnInit {
  month: Date;

  constructor() {}

  ngOnInit(): void {
    this.month = new Date();
  }

  pickMonth(date: Date, datepicker: MatDatepicker<Date>) {
    let year = date.getFullYear();
    let month = date.getMonth();
    this.month = new Date(year, month);
    datepicker.close();
  }
}
