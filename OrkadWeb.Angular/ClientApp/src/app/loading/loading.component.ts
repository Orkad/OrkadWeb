import { Component, Input } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss'],
})
export class LoadingComponent {
  @Input() offlineDate = new Date();

  getOfflineTimeSpan() {
    return moment(this.offlineDate).format('HH:mm');
  }
}
