import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddExpenseCommand } from 'src/shared/models/expenses/AddExpenseCommand';
import { AddExpenseResult } from 'src/shared/models/expenses/AddExpenseResult';
import { ExpenseRow, ExpenseRows } from 'src/shared/models/expenses/ExpenseRow';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  constructor(private httpClient: HttpClient) {}

  add(command: AddExpenseCommand | null): Observable<AddExpenseResult> {
    if (!command) {
      throw new Error('command is null');
    }
    return this.httpClient.post<AddExpenseResult>('api/expense/add', command);
  }

  getAll(): Observable<ExpenseRows> {
    return this.httpClient.get<ExpenseRows>('api/expense/getAll');
  }

  getMonthly(month: Date): Observable<ExpenseRows> {
    let params = new HttpParams();
    params = params.set('month', month.toISOString());
    return this.httpClient.get<ExpenseRows>('api/expense/getMonthly', {
      params: params,
    });
  }

  delete(id: number): Observable<void> {
    return this.httpClient.post<void>(`api/expense/delete`, id);
  }
}
