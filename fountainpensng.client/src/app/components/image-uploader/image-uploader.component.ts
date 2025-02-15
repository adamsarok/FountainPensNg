import { Component, EventEmitter, Output } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MatButtonModule } from '@angular/material/button';

//TODO: fix ugly styling by hiding original button

@Component({
    selector: 'app-image-uploader',
    imports: [MatButtonModule],
    templateUrl: './image-uploader.component.html',
    styleUrl: './image-uploader.component.css'
})
export class ImageUploaderComponent {
  r2ApiUrl = environment.r2ApiUrl;

  @Output() selectedFile = new EventEmitter<File | null>();
  

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile.emit(input.files[0]);
    } else {
      this.selectedFile.emit(null);
    }
  }
}
