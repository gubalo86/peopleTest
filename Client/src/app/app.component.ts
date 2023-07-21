import { Component, OnInit } from '@angular/core';
import { PeopleService } from './services/people.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'People Test';

  constructor(public peopleService: PeopleService) {}

  ngOnInit(): void {
    
  }
}