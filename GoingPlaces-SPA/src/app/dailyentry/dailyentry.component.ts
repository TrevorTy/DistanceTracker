import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/ngx-bootstrap-datepicker';
import { UserService } from '../_services/user.service';
import { DailyEntry } from '../_models/dailyEntry';
import { map } from 'rxjs/operators';
import { EntryPerDayOfTheMonth } from '../_models/entryPerDayOFTheMonth';
import { EndDestination } from '../_models/endDestination';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-dailyentry',
  templateUrl: './dailyentry.component.html',
  styleUrls: ['./dailyentry.component.css']
})
export class DailyentryComponent implements OnInit {
  dailyEntryForm: FormGroup;
  endDestinationForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  dailyEntry: DailyEntry;
  myEndDestination: EndDestination;
  public endDestination: EndDestination[];

  constructor(private fb: FormBuilder, private userService: UserService, private toastr: ToastrService) { }
  public currentYear  = 2020; // The user has to decide this
  public dailyEntries: DailyEntry[] = [];
  public totalPerMonth: EntryPerDayOfTheMonth;
  public averageDistance: EntryPerDayOfTheMonth;
  public distanceToCoverInMonth: EntryPerDayOfTheMonth;
  public deltaDistanceToCoverInMonth: EntryPerDayOfTheMonth;
  // _
  public distancesOverView: EntryPerDayOfTheMonth[] = [];
  public averageDailyDistanceEndDestination: EndDestination[];
  public averageWeeklyDistanceEndDestination: EndDestination[];
  public averageMonthlyDistanceEndDestination: EndDestination[];
  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-blue',
      isAnimated: true
    };

    this.createDailyEntryForm();
    this.createEndDestinationForm();
  
    this.refreshAllTotals();
    this.userService.getDistances(this.currentYear)
    .subscribe(result => {
      this.dailyEntries =  result;
      this.getEntryPerDayOfTheMonth();
    });

    this.userService.getEndDestination(this.currentYear)
    .subscribe(success => {
      if (success) {
        this.endDestination =  this.userService.endestinations;
      }
    });

    this.userService.getAverageDailyDistanceOfYear(this.currentYear)
      .subscribe(result => {
          this.averageDailyDistanceEndDestination = result;
      });

    this.userService.getAverageWeeklyDistanceOfYear(this.currentYear)
      .subscribe(result => {
        this.averageWeeklyDistanceEndDestination = result;
      });

    this.userService.getMonthlyDistanceToCover(this.currentYear)
      .subscribe(result => {
        this.averageMonthlyDistanceEndDestination = result;
      });
  }

  private refreshAllTotals() {
    this.userService.getAverageDistanceOfMonth(this.currentYear)
      .subscribe(res => {
        this.averageDistance = res;
        this.getEntryPerDayOfTheMonth();
      });
    this.userService.getTotalDistanceOfMonth(this.currentYear)
      .subscribe(res => {
        this.totalPerMonth = res;
        this.getEntryPerDayOfTheMonth();
      });
    this.userService.getDistanceToCoverInMonth(this.currentYear)
      .subscribe(res => {
        this.distanceToCoverInMonth = res;
        this.getEntryPerDayOfTheMonth();
      });
    this.userService.getDeltaDistanceToCoverOfMonth(this.currentYear)
      .subscribe(res => {
        this.deltaDistanceToCoverInMonth = res;
        this.getEntryPerDayOfTheMonth();
      });
  }

  createDailyEntryForm() {
    this.dailyEntryForm = this.fb.group({
      // currentDay: [null],
      dailyDistance: ['', Validators.required],
      month: ['', Validators.required],
      year: ['', Validators.required],
      day: ['', Validators.required]
    });
  }

  createEndDestinationForm() {
    this.endDestinationForm = this.fb.group({
      // currentDay: [null],
      goal: ['', Validators.required],
      distanceOfGoal: ['', Validators.required],
      year: ['', Validators.required]
    });
  }



    createDailyEntry() {
      if (this.dailyEntryForm.valid) {
        this.dailyEntry = Object.assign({}, this.dailyEntryForm.value);
        this.userService.createDistance(this.dailyEntry).subscribe(
          () => {
            this.dailyEntries.push(this.dailyEntry);
            this.refreshAllTotals();
            this.dailyEntryForm.reset();
            this.toastr.success('Distance has been added');
            console.log('create entry is successful');
          },
          error => {
            console.log(error);
          }
        );
        }
      }
  

      createEndDestination() {
        if (this.endDestinationForm.valid) {
          this.myEndDestination = Object.assign({}, this.endDestinationForm.value);
          this.userService.createEndDestination(this.myEndDestination).subscribe(
            () => {
              this.endDestination.push(this.myEndDestination);
              this.toastr.success('Goal has been added');
              this.refreshAllTotals();
            },
            error => {
              console.log(error);
            }
          );
          }
      }


      deleteDistance(id: number) {
        this.userService.deleteDistance(id).subscribe( () => {
          this.dailyEntries.splice(this.dailyEntries.findIndex(distance => distance.id === id), 1);
          this.toastr.success('Distance has been deleted');
          this.getEntryPerDayOfTheMonth();
        });
      }

      public getEntryPerDayOfTheMonth(): void {
        let result: EntryPerDayOfTheMonth[] = [{day: 1, year: 2020}, {day: 2, year: 2020}, {day: 3, year: 2020},
          {day: 4, year: 2020}, {day: 5, year: 2020}, {day: 6, year: 2020}, {day: 7, year: 2020}, {day: 8, year: 2020},
          {day: 9, year: 2020}, {day: 10, year: 2020}, {day: 11, year: 2020}, {day: 12, year: 2020}, {day: 13, year: 2020},
          {day: 14, year: 2020}, {day: 15, year: 2020}, {day: 16, year: 2020}, {day: 17, year: 2020}, {day: 18, year: 2020},
          {day: 19, year: 2020}, {day: 20, year: 2020}, {day: 21, year: 2020}, {day: 22, year: 2020}, {day: 23, year: 2020},
          {day: 24, year: 2020}, {day: 25, year: 2020}, {day: 26, year: 2020}, {day: 27, year: 2020}, {day: 28, year: 2020},
          {day: 29, year: 2020}, {day: 30, year: 2020}, {day: 31, year: 2020}
          // , {day: 98, year: 2020, january: this.getTotalDistanceOfMonth('may')}
        ];

        this.dailyEntries.forEach((entry) => {
          let entryPerDay: EntryPerDayOfTheMonth = result.find(r => r.day === entry.day &&
              r.year === entry.year);
          if (entryPerDay === undefined) {
            entryPerDay = {title: entry.day.toString(), year: entry.year };
            result.push(entryPerDay);
          }
          switch (entry.month) {
            case 'january':
              entryPerDay.january = entry.dailyDistance;
              entryPerDay.januaryId = entry.id;
              break;
            case 'february':
              entryPerDay.february = entry.dailyDistance;
              entryPerDay.februaryId = entry.id;
              break;
            case 'march':
                entryPerDay.march = entry.dailyDistance;
                entryPerDay.marchId = entry.id;
                break;
            case 'april':
                entryPerDay.april = entry.dailyDistance;
                entryPerDay.aprilId = entry.id;
                break;
            case 'may':
                  entryPerDay.may = entry.dailyDistance;
                  entryPerDay.mayId = entry.id;
                  break;
            case 'june':
                  entryPerDay.june = entry.dailyDistance;
                  entryPerDay.juneId = entry.id;
                  break;
            case 'july':
                  entryPerDay.july = entry.dailyDistance;
                  entryPerDay.julyId = entry.id;
                  break;
            case 'august':
                  entryPerDay.august = entry.dailyDistance;
                  entryPerDay.augustId = entry.id;
                  break;
            case 'september':
                  entryPerDay.september = entry.dailyDistance;
                  entryPerDay.septemberId = entry.id;
                  break;
            case 'october':
                  entryPerDay.october = entry.dailyDistance;
                  entryPerDay.octoberId = entry.id;
                  break;
            case 'november':
                  entryPerDay.november = entry.dailyDistance;
                  entryPerDay.novemberId = entry.id;
                  break;
            case 'december':
                  entryPerDay.december = entry.dailyDistance;
                  entryPerDay.decemberId = entry.id;
                  break;
              default:
              break;
          }
        });

        this.distancesOverView = result.sort((a, b) => a.day - b.day); // sort or orderBy
        if (this.totalPerMonth.title) {
           this.distancesOverView.push(this.totalPerMonth);
        }
        if (this.averageDistance.title) {
         this.distancesOverView.push(this.averageDistance);
        }
        if (this.distanceToCoverInMonth.title) {
        this.distancesOverView.push(this.distanceToCoverInMonth);
        }
        if (this.deltaDistanceToCoverInMonth.title) {
        this.distancesOverView.push(this.deltaDistanceToCoverInMonth);
        }
      }

      getDailyEntries() {
          return this.userService.getDistances(this.currentYear);
      }

    getEndDestination() {
      return this.userService.getEndDestination(this.currentYear);
     }
}

