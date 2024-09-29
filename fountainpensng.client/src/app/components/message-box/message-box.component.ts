import { Component, Inject } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-message-box',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './message-box.component.html',
  styleUrl: './message-box.component.css',
})
export class MessageBoxComponent {
  constructor(
    public dialogRef: MatDialogRef<MessageBoxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string; message: string }
  ) {}

  // onNoClick(): void {
  //   this.dialogRef.close(false); // Return false if "No" is clicked
  // }
}
