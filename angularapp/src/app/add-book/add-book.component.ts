import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UntypedFormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent {
  titleControl = new UntypedFormControl('');
  authorControl = new UntypedFormControl('');
  errorMsg = '';

  constructor(private http: HttpClient, private dialogRef: MatDialogRef<AddBookComponent>) { }

  addBook(): void {
    const book: Book = {
      Title: this.titleControl.value,
      Author: this.authorControl.value
    };
    this.http.post<Book>('/book', book).subscribe(result => {
      this.dialogRef.close(result);
    }, error => this.errorMsg = error.error);
  }
}

interface Book {
  Title: string;
  Author: string;
}
