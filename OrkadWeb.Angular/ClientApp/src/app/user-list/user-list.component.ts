import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { map, Observable } from 'rxjs';
import { AuthenticationService } from '../authentication/authentication.service';
import { UserItem } from './user-item.model';

@Component({
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthenticationService
  ) {}
  connectedUserId: number | undefined;

  displayedColumns = ['id', 'name', 'email', 'role', 'actions'];
  dataSource: MatTableDataSource<UserItem>;

  ngOnInit(): void {
    this.connectedUserId = this.authService.getConnectedUser()?.id;
    this.httpClient.get<UserItem[]>('api/users').subscribe((u) => {
      this.dataSource = new MatTableDataSource<UserItem>();
      this.dataSource.data = u;
    });
  }

  displayRole(role: string) {
    if (role == 'Admin') {
      return 'Administrateur';
    }
    return '';
  }

  deleteUserDisabled(user: UserItem) {
    return this.connectedUserId === user.id;
  }

  deleteUser(user: UserItem) {}
}
