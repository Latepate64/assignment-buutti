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
  page = 0;
  booksAdded = 0;
  errorMsg = '';

  constructor(private http: HttpClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getBooks();
  }

  getBooks(): void {
    this.http.get<Book[]>('/book', { params: { "page": this.page++, "offset": this.booksAdded } }).subscribe(result => {
      this.books?.push(...result);
    }, error => this.errorMsg = error.error);
  }

  addBook(): void {
    let dialogRef = this.dialog.open(AddBookComponent);
    dialogRef.afterClosed().subscribe(book => {
      if (book) {
        this.books?.unshift({
          title: book.title,
          author: book.author,
          timestamp: book.timestamp
        });
        ++this.booksAdded;
      }
    });
  }
}

interface Book {
  title: string;
  author: string;
  timestamp: string;
}
