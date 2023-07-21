import { Component, OnInit } from '@angular/core';
import { PeopleInterface } from 'src/app/Models/people';
import { PeopleService } from 'src/app/services/people.services';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-people',
  templateUrl: './people.page.html',
  styleUrls: ['./people.page.scss'],
})
export class PeoplePage implements OnInit {
  people: PeopleInterface[] = [];

  peopleForm = new FormGroup({
    id: new FormControl(0),
    name: new FormControl(''),
    lastName: new FormControl(''),
    age: new FormControl(0),
  });

  constructor(public peopleService: PeopleService) {}

  ngOnInit(): void {
    this.getPeople();
  }

  getPeople = () =>
    this.peopleService.getAll().subscribe(
      (data) => {
        this.people = data;
      },
      (error) => {
        console.log(error);
      }
    );

  onSave = () => {
    this.peopleForm.value.id === 0
      ? this.peopleService
          .post({
            ...this.peopleForm.value,
            id: this.people.length + 1,
          })
          .subscribe(
            (data) => {
              this.getPeople();
              this.cleanForm();
            },
            (error) => console.log(error)
          )
      : this.peopleService.put(this.peopleForm.value).subscribe(
          (data) => {
            this.getPeople();
            this.cleanForm();
          },
          (error) => console.log(error)
        );
  };

  onEdit = (item: any) => {
    this.peopleForm.setValue(item);
  };

  onDelete = (item: any) => {
    this.peopleService.delete(item.id).subscribe(
      (result) => {this.getPeople(); this.cleanForm();},
      (error) => console.log(error)
    );
  };

  cleanForm() {
    this.peopleForm.setValue({
      id: 0,
      name: '',
      lastName: '',
      age: 0,
    });
  }
}
