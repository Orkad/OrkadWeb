import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddExpenseCommand } from 'src/shared/models/expenses/AddExpenseCommand';
import { AddExpenseResult } from 'src/shared/models/expenses/AddExpenseResult';
import { ExpenseRows } from 'src/shared/models/expenses/ExpenseRow';
import { UpdateExpenseCommand } from 'src/api/commands/UpdateExpenseCommand';
import { Moment } from 'moment';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  constructor(private httpClient: HttpClient) {}

  getMonthly(month: Moment): Observable<ExpenseRows> {
    let params = new HttpParams();
    params = params.set('month', month.toISOString());
    return this.httpClient.get<ExpenseRows>(this.getEndpoint('getMonthly'), {
      params: params,
    });
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

  delete(id: number): Observable<void> {
    return this.httpClient.post<void>(this.getEndpoint('delete'), id);
  }

  private getEndpoint(action: string): string {
    return 'api/transaction/' + action;
  }
}
