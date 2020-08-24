import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DailyEntry } from '../_models/dailyEntry';
import { map } from 'rxjs/operators';
import {  Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { environment} from '../../environments/environment';
import { EndDestination } from '../_models/endDestination';
import { EntryPerDayOfTheMonth } from '../_models/entryPerDayOFTheMonth';

@Injectable({
  providedIn: 'root'
})
export class UserService {
 private dailyDistancePath = environment.apiUrl + 'dailyEntry/';

public distances = [];
public endestinations = [];

constructor(private http: HttpClient, private authService: AuthService ) { }

createDistance(dailyDistance: DailyEntry) {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);

    return this.http.post(this.dailyDistancePath , dailyDistance, {headers});
  }

  getDistances(year: number): Observable<any> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `distances/${year}`, {headers})
      .pipe(
        map((data: any[]) => {
          this.distances = data;
          return data;
        }));
  }

  deleteDistance(id: number) {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.delete(this.dailyDistancePath + id, {headers});
  }

  createEndDestination(endDestination: EndDestination) {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);

    return this.http.post(this.dailyDistancePath + 'destination' , endDestination, {headers});
  }

  getEndDestination(year: number): Observable<boolean> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `destination/${year}`, {headers})
    .pipe(
      map((data: any[]) => {
        this.endestinations = data;
        return true;
      }));
  }

  getTotalDistanceOfMonth(year: number): Observable<EntryPerDayOfTheMonth> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `totalDistanceMonth/${year}`, {headers})
      .pipe(
        map((data: EntryPerDayOfTheMonth) => {
          return data;
        }));
  }

  getAverageDistanceOfMonth(year: number): Observable<EntryPerDayOfTheMonth> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `averageDistanceMonth/${year}`, {headers})
    .pipe(
      map((data: EntryPerDayOfTheMonth) => {
        return data;
      }));
  }

  getDistanceToCoverInMonth(year: number): Observable<EntryPerDayOfTheMonth> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `distanceToCoverInMonth/${year}`, {headers})
    .pipe(
      map((data: EntryPerDayOfTheMonth) => {
        return data;
      }));
  }

  getDeltaDistanceToCoverOfMonth(year: number): Observable<EntryPerDayOfTheMonth> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `deltaDistanceToCoverInMonth/${year}`, {headers})
    .pipe(
      map((data: EntryPerDayOfTheMonth) => {
        return data;
      }));
  }

  getAverageDailyDistanceOfYear(year: number): Observable<EndDestination[]> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + 'destination/' + `dailyAverageToCover/${year}`, {headers})
    .pipe(
      map((data: EndDestination[]) => {
        return data;
      }));
  }

  getAverageWeeklyDistanceOfYear(year: number): Observable<EndDestination[]> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + 'destination/'
    + `CalculateWeeklyDistanceToCover/${year}`, {headers})
    .pipe(
      map((data: EndDestination[]) => {
        return data;
      }));
  }

  getMonthlyDistanceToCover(year: number): Observable<EndDestination[]> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + 'destination/' +
    `CalculateMonthlyDistanceToCover/${year}`, {headers})
    .pipe(
      map((data: EndDestination[]) => {
        return data;
      }));
  }

  getTotalCoverdDistance(year: number): Observable<number> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `totalDistance/${year}`, {headers})
      .pipe(
        map((data: number) => {
          return data;
        }));
  }

  getTotalWalkedDays(year: number): Observable<number> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `totalWalkedDays/${year}`, {headers})
      .pipe(
        map((data: number) => {
          return data;
        }));
  }

  GetDistanceTillDestination(year: number): Observable<number> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `distanceTillDestination/${year}`, {headers})
      .pipe(
        map((data: number) => {
          return data;
        }));
  }

  GetDaysLeftOfYear(year: number): Observable<number> {
    let headers =  new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    return this.http.get(this.dailyDistancePath + `daysLeftOfYear/${year}`, {headers})
      .pipe(
        map((data: number) => {
          return data;
        }));
  }
}
