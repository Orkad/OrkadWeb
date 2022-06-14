import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { map, Observable } from 'rxjs';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseRow } from 'src/shared/models/expenses/ExpenseRow';

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
  month: UntypedFormControl = new UntypedFormControl(new Date());

  dataSource = new MatTableDataSource<ExpenseRow>([
    { amount: 1, name: 'test' } as ExpenseRow,
  ]);
  displayedColumns = ['date', 'name', 'amount'];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    this.expenseService.getAll().subscribe((rows) => {
      console.log(rows);
      this.dataSource.data = rows;
    });
    //this.refreshExpenses();
    //this.month.valueChanges.subscribe(() => this.refreshExpenses());
  }

  // private refreshExpenses() {
  //   this.expenseService.getMonthly(this.month.value).subscribe((rows) => {
  //     this.expenses.data = rows;
  //   });
  // }
}
