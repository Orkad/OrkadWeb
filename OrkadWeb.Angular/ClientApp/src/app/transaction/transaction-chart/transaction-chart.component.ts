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

  constructor(private transactionService: TransactionService) {}

  lineChartData: ChartConfiguration<'line', TransactionChartPoint[]>['data'];
  lineChartOptions: ChartConfiguration['options'];

  ngOnInit(): void {
    moment.locale('fr-FR');
    this.refresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
  }

  refresh(): void {
    const mMonth = moment(this.month);
    this.transactionService
      .getChartData(mMonth)
      .subscribe((points) => this.setChartData(points));
  }

  setChartData(points: TransactionChartPoint[]) {
    this.lineChartOptions = {
      locale: 'fr-FR',
      responsive: false,
      datasets: {
        line: {
          borderColor: '#000',
          pointBorderColor: '#000',
          pointBackgroundColor: '#fff',
          backgroundColor: 'rgba(63, 81, 181, 200)',
        },
      },
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
