import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddBookComponent } from './add-book/add-book.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public books?: Book[] = [];
  title = 'angularapp';
  page = 0;

  constructor(private http: HttpClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.onScroll();
  }

  onScroll(): void {
    this.http.get<Book[]>('/book', { params: { "page": this.page++ } }).subscribe(result => {
      this.books?.push(...result);
    }, error => console.error(error));
  }

  addBook(): void {
    this.dialog.open(AddBookComponent);
  }
}

interface Book {
  title: string;
  author: string;
  timestamp: string;
}
