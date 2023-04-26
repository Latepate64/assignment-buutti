import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UntypedFormControl } from '@angular/forms';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent {
  titleControl = new UntypedFormControl('');
  authorControl = new UntypedFormControl('');

  constructor(private http: HttpClient) { }

  addBook(): void {
    const book: Book = {
      Title: this.titleControl.value,
      Author: this.authorControl.value
    };
    this.http.post<Book>('/book', book).subscribe(result => {
      //TODO close form
    }, error => console.error(error));
  }
}

interface Book {
  Title: string;
  Author: string;
}
