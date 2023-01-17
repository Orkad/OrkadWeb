import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';
import { MonthlyIncome } from 'src/shared/models/MonthlyIncome';

@Injectable({
  providedIn: 'root',
})
export class MonthlyTransactionService {
  constructor(private httpClient: HttpClient) {}

  charges(): Observable<MonthlyCharge[]> {
    return this.httpClient.get<MonthlyCharge[]>('api/monthly/charges');
  }

  addCharge(charge: MonthlyCharge) {
    return this.httpClient
      .post<number>('api/monthly/charges/', charge)
      .pipe(map((i) => (charge.id = i)));
  }

  editCharge(charge: MonthlyCharge) {
    return this.httpClient.put<void>(
      'api/monthly/charges/' + charge.id,
      charge
    );
  }

  deleteCharge(id: number): Observable<void> {
    return this.httpClient.delete<void>('api/monthly/charges/' + id);
  }

  incomes(): Observable<MonthlyIncome[]> {
    return this.httpClient.get<MonthlyIncome[]>('api/monthly/incomes');
  }

  addIncome(income: MonthlyIncome) {
    return this.httpClient
      .post<number>('api/monthly/incomes/', income)
      .pipe(map((i) => (income.id = i)));
  }

  editIncome(income: MonthlyIncome) {
    return this.httpClient.put<void>(
      'api/monthly/incomes/' + income.id,
      income
    );
  }

  deleteIncome(id: number): Observable<void> {
    return this.httpClient.delete<void>('api/monthly/incomes/' + id);
  }
}
