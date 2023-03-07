import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { map, Observable } from 'rxjs';
import { UserItem } from './user-item.model';

@Component({
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  constructor(private httpClient: HttpClient) {}

  dataSource$ = this.httpClient.get<UserItem[]>('api/users').pipe(
    map((u) => {
      const dataSource = new MatTableDataSource<UserItem>();
      dataSource.data = u;
      return dataSource;
    })
  );

  ngOnInit(): void {}
}
