import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PeoplePageRoutingModule } from './people-routing.module';
import { PeoplePage } from './people.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PeoplePageRoutingModule,
  ],
  declarations: [PeoplePage],
})
export class PeoplePageModule {}
