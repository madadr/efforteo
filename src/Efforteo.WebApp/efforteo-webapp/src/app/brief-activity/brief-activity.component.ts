import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Activity} from '../model/activity';
import {Converter} from '../utils/converter';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {AccountService} from '../account.service';
import {LoadingService} from '../loading.service';

@Component({
  selector: 'app-brief-activity',
  templateUrl: './brief-activity.component.html',
  styleUrls: ['./brief-activity.component.css']
})
export class BriefActivityComponent implements OnInit, OnDestroy {
  @Input() activity: Activity;
  createdAgo: Date;
  userName: string = null;
  onCreateLoaderName = 'onInitLoader';

  public converter = Converter;

  constructor(private accountService: AccountService,
              private toggleService: LoadingService) {
  }

  ngOnInit() {
    console.log('BriefActivityComponent' + JSON.stringify(this.activity));
    this.createdAgo = new Date(this.activity.createdAt);
    this.loadUserName();
  }

  public loadUserName() {
    let account: Account;
    this.accountService.getUserData(this.activity.userId)
      .pipe(map(resp => {
          account = <Account>JSON.parse(JSON.stringify(resp.body));
          if (account != null) {
            this.userName = account.name;
            console.log('this.userName ===== ' + this.userName);
          }
        }),
        catchError(err => {
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
        }))
      .subscribe(() => {
      });
  }

  ngOnDestroy() {
    this.toggleService.remove(this.onCreateLoaderName);
  }
}
