import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MonthlyCharges } from 'src/shared/models/MonthlyCharge';
import { MonthlyIncomes } from 'src/shared/models/MonthlyIncome';

@Injectable({
  providedIn: 'root',
})
export class MonthlyTransactionService {
  constructor(private httpClient: HttpClient) {}

  charges(): Observable<MonthlyCharges> {
    return this.httpClient.get<MonthlyCharges>(
      'api/monthlytransaction/charges'
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
