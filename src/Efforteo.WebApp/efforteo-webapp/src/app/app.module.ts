import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NavbarComponent} from './navbar/navbar.component';
import {SignUpComponent} from './sign-up/sign-up.component';
import {HomeComponent} from './home/home.component';
import {SignInComponent} from './sign-in/sign-in.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {AppInterceptor} from './http-interceptors/app-interceptor';
import {AlertsComponent} from './alerts/alerts.component';
import {AuthInterceptor} from './http-interceptors/auth-interceptor';
import {MatProgressBarModule} from '@angular/material';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {DashboardComponent} from './dashboard/dashboard.component';
import {ActivitiesComponent} from './activities/activities.component';
import {AddActivityComponent} from './add-activity/add-activity.component';
import {Router} from '@angular/router';
import {ShowActivityComponent} from './show-activity/show-activity.component';
import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {ServiceNotAvailableComponent} from './service-not-available/service-not-available.component';
import {LoaderComponent} from './loader/loader.component';
import {LoadingService} from './loading.service';
import {TimeAgoPipe} from 'time-ago-pipe';
import { EditActivityComponent } from './edit-activity/edit-activity.component';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SignUpComponent,
    HomeComponent,
    SignInComponent,
    AlertsComponent,
    DashboardComponent,
    ActivitiesComponent,
    AddActivityComponent,
    LoaderComponent,
    ShowActivityComponent,
    PageNotFoundComponent,
    ServiceNotAvailableComponent,
    TimeAgoPipe,
    EditActivityComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatProgressBarModule,
    MatProgressSpinnerModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AppInterceptor, multi: true},
    {
      provide: HTTP_INTERCEPTORS, useFactory: function (router: Router) {
        return new AuthInterceptor(router);
      },
      multi: true,
      deps: [Router]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
