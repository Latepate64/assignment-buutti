import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public books?: Book[];

  constructor(http: HttpClient) {
    http.get<Book[]>('/book').subscribe(result => {
      this.books = result;
    }, error => console.error(error));
  }

  title = 'angularapp';
}

interface Book {
  title: string;
  author: string;
  timestamp: string;
}
