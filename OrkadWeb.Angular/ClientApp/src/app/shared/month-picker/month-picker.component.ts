import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { UntypedFormControl, FormControlName } from '@angular/forms';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MatCalendar, MatDatepicker } from '@angular/material/datepicker';
import * as moment from 'moment';
import { Moment } from 'moment';

export const MONTH_FORMATS = {
  parse: {
    dateInput: 'MM YYYY',
  },
  display: {
    dateInput: 'MMMM YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-month-picker',
  templateUrl: './month-picker.component.html',
  styleUrls: ['./month-picker.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    { provide: MAT_DATE_FORMATS, useValue: MONTH_FORMATS },
  ],
})
export class MonthPickerComponent {
  @Input() control: UntypedFormControl = new UntypedFormControl(new Date());

  @ViewChild(MatDatepicker) datepicker: MatDatepicker<Moment>;

  constructor() {}

  pickMonth(picked: Moment) {
    const m = moment(this.control.value);
    m.year(picked.year());
    m.month(picked.month());
    this.control.setValue(m.toDate());
    this.datepicker.close();
  }

  public nextMonth() {
    const m = moment(this.control.value);
    m.add(1, 'month');
    this.control.setValue(m.toDate());
  }

  public previousMonth() {
    const m = moment(this.control.value);
    m.subtract(1, 'month');
    this.control.setValue(m.toDate());
  }
}
