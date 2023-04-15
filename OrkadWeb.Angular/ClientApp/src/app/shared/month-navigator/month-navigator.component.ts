import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { Moment } from 'moment';

@Component({
  selector: 'app-month-navigator',
  templateUrl: './month-navigator.component.html',
  styleUrls: ['./month-navigator.component.scss'],
})
export class MonthNavigatorComponent {
  @Input() control = new FormControl<Moment>(moment());
  @Input() min: Moment | null;
  @Input() max: Moment | null;

  isPreviousDisabled() {
    if (!this.min) {
      return false;
    }
    return moment(this.control.value).isBefore(this.min);
  }

  isNextDisabled() {
    if (!this.max) {
      return false;
    }
    return moment(this.control.value).isAfter(this.max);
  }
}
