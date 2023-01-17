import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { MonthlyCharge } from 'src/shared/models/MonthlyCharge';
import { MonthlyIncomes } from 'src/shared/models/MonthlyIncome';

@Injectable({
  providedIn: 'root',
})
export class MonthlyTransactionService {
  constructor(private httpClient: HttpClient) {}

  charges(): Observable<MonthlyCharge[]> {
    return this.httpClient.get<MonthlyCharge[]>(
      'api/monthlytransaction/charges'
    );
  }

  addCharge(charge: MonthlyCharge) {
    return this.httpClient
      .post<number>('api/monthlytransaction/charges/', charge)
      .pipe(map((i) => (charge.id = i)));
  }

  editCharge(charge: MonthlyCharge) {
    return this.httpClient.put<void>(
      'api/monthlytransaction/charges/' + charge.id,
      charge
    );
  }

  deleteCharge(id: number): Observable<void> {
    return this.httpClient.delete<void>('api/monthlytransaction/charges/' + id);
  }

  incomes(): Observable<MonthlyIncomes> {
    return this.httpClient.get<MonthlyIncomes>(
      'api/monthlytransaction/incomes'
    );
  }
}
