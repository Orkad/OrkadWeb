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
import { TransactionService } from 'src/services/transaction.service';
import { TransactionChartPoint } from 'src/shared/models/transactions/TransactionChartPoint';

@Component({
  selector: 'app-transaction-chart',
  templateUrl: './transaction-chart.component.html',
  styleUrls: ['./transaction-chart.component.scss'],
})
export class TransactionChartComponent implements OnInit, OnChanges {
  @Input() month: Date;
  @Input() transactions: TransactionRow[];

  constructor(private transactionService: TransactionService) {}

  lineChartData: ChartConfiguration<'line', TransactionChartPoint[]>['data'];
  lineChartOptions: ChartConfiguration['options'];

  ngOnInit(): void {
    moment.locale('fr-FR');
    this.refresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['transactions']) {
      this.refresh();
    }
  }

  refresh(): void {
    const mMonth = moment(this.month);
    this.transactionService
      .getChartData(mMonth)
      .subscribe((points) => this.setChartData(points));
    // const group = this.transactions
    //   .sort((t1, t2) => moment(t1.date).valueOf() - moment(t2.date).valueOf())
    //   .groupBy(
    //     (t) => moment(t.date),
    //     (a, b) => a.isSame(b)
    //   );
    // const sumByDay = group.map((g) => ({
    //   day: g.key,
    //   amount: g.values.map((t) => t.amount).sum(),
    // }));
    // const tx = sumByDay.reduce((acc, cur) => {
    //   acc.push({
    //     x: cur.day,
    //     y: (acc.length > 0 ? acc[acc.length - 1].y : 0) + cur.amount,
    //   });
    //   return acc;
    // }, new Array<{ x: moment.Moment; y: number }>());
  }

  setChartData(points: TransactionChartPoint[]) {
    this.lineChartOptions = {
      locale: 'fr-FR',
      responsive: false,
      scales: {
        x: {
          min: moment(this.month).startOf('month').toString(),
          max: moment(this.month).endOf('month').toString(),
          type: 'time',
          time: {
            unit: 'day',
            displayFormats: {
              day: 'D MMM',
              hour: '',
            },
            tooltipFormat: 'DD/MM/YYYY',
          },
        },
        y: {
          suggestedMin: 0,
          type: 'linear',
          ticks: {
            callback: this.euroSuffix,
          },
        },
      },
      plugins: {
        tooltip: {
          callbacks: {
            label: function (context) {
              return context.parsed.y + ' \u20AC';
            },
          },
        },
      },
    };
    this.lineChartData = {
      datasets: [
        {
          data: points,
          fill: true,
        },
      ],
    };
  }

  private euroSuffix(amount: number | string) {
    return amount + ' \u20AC';
  }
}
