import {Component, Input, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {Account} from '../model/account';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css']
})
export class ProfileDetailsComponent implements OnInit {
  @Input() account: Account;
  isOwner: boolean;

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.checkIfOwner();
  }

  public checkIfOwner() {
    this.isOwner = this.authService.getId() === this.account.id;
  }
}
