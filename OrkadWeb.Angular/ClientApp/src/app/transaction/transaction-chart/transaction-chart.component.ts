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

  lineChartData: ChartConfiguration<'line', TransactionChartPoint[]>['data'] = {
    datasets: [
      {
        data: [],
        fill: true,
      },
    ],
  };
  lineChartOptions: ChartConfiguration['options'] = {
    animation: {
      duration: 300,
    },
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
        min: moment().startOf('month').toString(),
        max: moment().endOf('month').toString(),
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

  ngOnInit(): void {
    moment.locale('fr-FR');
    this.refresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const monthChanges = changes['month'];
    if (monthChanges) {
      if (
        !moment(monthChanges.currentValue).isSame(monthChanges.previousValue)
      ) {
        this.month = monthChanges.currentValue;
        this.refresh();
      }
    }
  }

  refresh(): void {
    this.setChartData([]);
    const mMonth = moment(this.month);
    this.transactionService
      .getChartData(mMonth)
      .subscribe((points) => this.setChartData(points));
  }

  setChartData(points: TransactionChartPoint[]) {
    const scales = this.lineChartOptions?.scales;
    if (scales) {
      const scalex = scales['x'];
      if (scalex) {
        scalex.min = moment(this.month).startOf('month').toString();
        scalex.max = moment(this.month).endOf('month').toString();
      }
    }

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
