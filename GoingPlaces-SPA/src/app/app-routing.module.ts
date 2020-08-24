import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DailyentryComponent } from './dailyentry/dailyentry.component';
import { GraphComponent } from './graph/graph.component';



const routes: Routes = [
    { path: 'home', component: HomeComponent},
    { path: 'dailyentry', component: DailyentryComponent},
    { path: 'graph', component: GraphComponent},
    { path: '**', redirectTo: 'home', pathMatch: 'full'},
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
