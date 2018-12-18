import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SignUpComponent} from './sign-up/sign-up.component';
import {HomeComponent} from './home/home.component';
import {SignInComponent} from './sign-in/sign-in.component';
import {DashboardComponent} from './dashboard/dashboard.component';
import {ActivitiesComponent} from './activities/activities.component';
import {AddActivityComponent} from './add-activity/add-activity.component';
import {ShowActivityComponent} from './show-activity/show-activity.component';
import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {ServiceNotAvailableComponent} from './service-not-available/service-not-available.component';
import {EditActivityComponent} from './edit-activity/edit-activity.component';
import {ShowProfileComponent} from './show-profile/show-profile.component';
import {RemoveActivityComponent} from './remove-activity/remove-activity.component';

const routes: Routes = [
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'home', component: HomeComponent},
  {path: 'sign-up', component: SignUpComponent},
  {path: 'sign-in', component: SignInComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'activities', component: ActivitiesComponent},
  {path: 'add-activity', component: AddActivityComponent},
  {path: 'show-activity/:id', component: ShowActivityComponent},
  {path: 'edit-activity/:id', component: EditActivityComponent},
  {path: 'remove-activity/:id', component: RemoveActivityComponent},
  {path: 'show-profile/:id', component: ShowProfileComponent},
  {path: 'not-available', component: ServiceNotAvailableComponent},
  {path: 'not-found', component: PageNotFoundComponent},
  {path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
