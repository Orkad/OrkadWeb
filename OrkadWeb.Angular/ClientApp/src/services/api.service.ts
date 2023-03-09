import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private httpClient: HttpClient) {}

  post<T>(endPoint: string, body: any) {
    return this.httpClient.post<T>('api/' + endPoint, body);
  }
}
