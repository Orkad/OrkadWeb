import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { ChartConfiguration, ChartOptions, ScatterDataPoint } from 'chart.js';
import * as moment from 'moment';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';
import 'ts-array-extensions';
import 'chartjs-adapter-moment';

@Component({
  selector: 'app-transaction-chart',
  templateUrl: './transaction-chart.component.html',
  styleUrls: ['./transaction-chart.component.scss'],
})
export class TransactionChartComponent implements OnInit, OnChanges {
  @Input() month: Date;
  @Input() transactions: TransactionRow[];

  lineChartData: ChartConfiguration<
    'line',
    { x: moment.Moment; y: number }[]
  >['data'];
  lineChartOptions: ChartConfiguration['options'];

  ngOnInit(): void {
    this.refresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    this.refresh();
  }

  refresh(): void {
    const group = this.transactions
      .sort((t1, t2) => moment(t1.date).valueOf() - moment(t2.date).valueOf())
      .groupBy(
        (t) => moment(t.date),
        (a, b) => a.isSame(b)
      );
    const sumByDay = group.map((g) => ({
      day: g.key,
      amount: g.values.map((t) => t.amount).sum(),
    }));
    const tx = sumByDay.reduce((acc, cur) => {
      acc.push({
        x: cur.day,
        y: (acc.length > 0 ? acc[acc.length - 1].y : 0) + cur.amount,
      });
      return acc;
    }, new Array<{ x: moment.Moment; y: number }>());
    this.lineChartOptions = {
      responsive: false,
      scales: {
        x: {
          min: moment(this.month).startOf('month').toString(),
          max: moment(this.month).endOf('month').toString(),
          type: 'time',
          time: {
            unit: 'day',
            displayFormats: {
              day: 'MMM D',
            },
          },
        },
        y: {
          max: 0,
          type: 'linear',
        },
      },
    };
    this.lineChartData = {
      datasets: [
        {
          data: tx,
          fill: true,
        },
      ],
    };
  }
}
