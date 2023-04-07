import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddExpenseCommand } from 'src/shared/models/expenses/AddExpenseCommand';
import { AddExpenseResult } from 'src/shared/models/expenses/AddExpenseResult';
import { ExpenseRows } from 'src/shared/models/expenses/ExpenseRow';
import { UpdateExpenseCommand } from 'src/api/commands/UpdateExpenseCommand';
import { Moment } from 'moment';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<ExpenseRows> {
    return this.httpClient.get<ExpenseRows>('api/expense/getAll');
  }

  getMonthly(month: Moment): Observable<ExpenseRows> {
    let params = new HttpParams();
    params = params.set('month', month.toISOString());
    return this.httpClient.get<ExpenseRows>('api/expense/getMonthly', {
      params: params,
    });
  }

  add(command: AddExpenseCommand | null): Observable<AddExpenseResult> {
    if (!command) {
      throw new Error('command is null');
    }
    return this.httpClient.post<AddExpenseResult>('api/expense/add', command);
  }

  update(command: UpdateExpenseCommand): Observable<void> {
    return this.httpClient.post<void>('api/expense/update', command);
  }

  delete(id: number): Observable<void> {
    return this.httpClient.post<void>(`api/expense/delete`, id);
  }
}
