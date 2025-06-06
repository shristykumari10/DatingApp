import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {



  registerMode = false;

  http = inject(HttpClient);



  registerToggle() {
    this.registerMode = !this.registerMode
  }



  cancelRegisterMode(event: boolean) {
    this.registerMode = false;
  }

}
