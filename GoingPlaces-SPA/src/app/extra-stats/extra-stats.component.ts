import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-extra-stats',
  templateUrl: './extra-stats.component.html',
  styleUrls: ['./extra-stats.component.css']
})
export class ExtraStatsComponent implements OnInit {

  constructor(private userService: UserService) { }
  totalWalkedDistance: number;
  totalWalkedDays: number;
  distanceTillDestination: number;
  daysLeftOfYear: number;
  public currentYear  = 2020;

  ngOnInit() {
    this.userService.getTotalCoverdDistance(this.currentYear)
      .subscribe(result => {
        this.totalWalkedDistance = result;
    });
    this.userService.getTotalWalkedDays(this.currentYear)
      .subscribe(result => {
        this.totalWalkedDays = result;
    });
    this.userService.GetDistanceTillDestination(this.currentYear)
      .subscribe(result => {
        this.distanceTillDestination = result;
      });
    this.userService.GetDaysLeftOfYear(this.currentYear)
        .subscribe(result => {
          this.daysLeftOfYear = result;
      });
  }


  
}
