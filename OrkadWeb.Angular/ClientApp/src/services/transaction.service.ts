import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AddExpenseCommand } from 'src/shared/models/expenses/AddExpenseCommand';
import { AddExpenseResult } from 'src/shared/models/expenses/AddExpenseResult';
import { UpdateExpenseCommand } from 'src/api/commands/UpdateExpenseCommand';
import { Moment } from 'moment';
import { TransactionRow } from 'src/shared/models/transactions/TransactionRow';
import { TransactionChartPoint } from 'src/shared/models/transactions/TransactionChartPoint';
import { Gain } from 'src/shared/models/transactions/Gain';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  constructor(private httpClient: HttpClient) {}

  getMonthly(month: Moment): Observable<TransactionRow[]> {
    let params = new HttpParams();
    params = params.set('month', month.toISOString());
    return this.httpClient.get<TransactionRow[]>(
      this.getEndpoint('getMonthly'),
      {
        params: params,
      }
    );
  }

  getChartData(month: Moment): Observable<TransactionChartPoint[]> {
    let params = new HttpParams();
    params = params.set('month', month.toISOString());
    return this.httpClient.get<TransactionChartPoint[]>(
      this.getEndpoint('getChartData'),
      {
        params: params,
      }
    );
  }

  addExpense(command: AddExpenseCommand | null): Observable<AddExpenseResult> {
    if (!command) {
      throw new Error('command is null');
    }
    return this.httpClient.post<AddExpenseResult>(
      this.getEndpoint('addExpense'),
      command
    );
  }

  updateExpense(command: UpdateExpenseCommand): Observable<void> {
    return this.httpClient.post<void>(
      this.getEndpoint('updateExpense'),
      command
    );
  }

  addGain(gain: Gain) {
    return this.httpClient
      .post<number>(this.getEndpoint('addGain'), gain)
      .pipe(tap((id) => (gain.id = id)));
  }

  updateGain(gain: Gain) {
    return this.httpClient.post<void>(this.getEndpoint('updateGain'), gain);
  }

  delete(id: number): Observable<void> {
    return this.httpClient.post<void>(this.getEndpoint('delete'), id);
  }

  private getEndpoint(action: string): string {
    return 'api/transactions/' + action;
  }
}
