import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public books?: Book[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.onScroll();
  }

  onScroll(): void {
    this.http.get<Book[]>('/book', { params: { "page": this.page++ } }).subscribe(result => {
      this.books?.push(...result);
    }, error => console.error(error));
  }

  title = 'angularapp';
  page = 0;
}

interface Book {
  title: string;
  author: string;
  timestamp: string;
}
