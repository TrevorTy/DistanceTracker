import { Component, OnInit } from '@angular/core';
import { Chart } from 'node_modules/chart.js';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css']
})
export class GraphComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    this.renderCanvas();
    this.renderLineChart();
  }
  renderCanvas() {
    let myChart = new Chart('barChart', {
      type: 'bar',
      data: {
          labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange', 
          'Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
          datasets: [{
              label: 'Distances',
              data: [12, 19, 3, 5, 2, 3, 12, 19, 3, 5, 2, 3],
              backgroundColor: [
                  'rgba(255, 99, 132, 0.2)',
                  'rgba(54, 162, 235, 0.2)',
                  'rgba(255, 206, 86, 0.2)',
                  'rgba(75, 192, 192, 0.2)',
                  'rgba(153, 102, 255, 0.2)',
                  'rgba(255, 159, 64, 0.2)'
              ],
              borderColor: [
                  'rgba(255, 99, 132, 1)',
                  'rgba(54, 162, 235, 1)',
                  'rgba(255, 206, 86, 1)',
                  'rgba(75, 192, 192, 1)',
                  'rgba(153, 102, 255, 1)',
                  'rgba(255, 159, 64, 1)'
              ],
              borderWidth: 3
          }]
      },
      options: {
          scales: {
              yAxes: [{
                  ticks: {
                      beginAtZero: true
                  }
              }]
          }
      }
  });
  }
//  "data":{"labels":["January","February","March","April","May","June","July"],
//  "datasets":[{"label":"My First Dataset","data":[65,59,80,81,56,55,40],"fill":false,
//  "borderColor":"rgb(75, 192, 192)","lineTension":0.1}]},"options":{}});
  renderLineChart() {
    let myLineChart = new Chart('lineChart', {
      type: 'line',
      data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September',
                  'October', 'November', 'December'],
        datasets : [ {
          label: 'Distances',
          data: [ 5, 9.5, 8, 6, 3, 4, 6, 8, 2, 4, 5, 7],
          borderColor: 'rgb(75, 192, 192)',
          fill: false,
          borderWidth: 1,
                    },
                  ],
            },
      options: {
        scales: {
            yAxes: [{
                stacked: true
            }]
        }
    }
  });
  }
}
