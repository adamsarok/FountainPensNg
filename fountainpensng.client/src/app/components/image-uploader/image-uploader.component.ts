import { Component, Input } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-image-uploader',
  standalone: true,
  imports: [],
  templateUrl: './image-uploader.component.html',
  styleUrl: './image-uploader.component.css'
})
export class ImageUploaderComponent {
  r2ApiUrl = environment.r2ApiUrl;

  @Input() 
  onUploadComplete: ((guid: string) => void) | undefined = undefined;

  @Input() 
  onError: ((error: string) => void) | undefined = undefined;

  selectedFile: File | null = null;
  
  constructor(private httpClient: HttpClient
  ) { }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      console.log(input.files);
      this.selectedFile = input.files[0]; // Get the first selected file
    }
  }

  handleUpload(file: File | null): void {
    if (!file) return;
    console.log('fake upload done!' + file);
    this.httpClient.put<string>(
      `${this.r2ApiUrl}/upload-image?fileName=${encodeURIComponent(file.name)}`, file, {  
    }).subscribe({
      next: r => {
        if (this.onUploadComplete) this.onUploadComplete(r);
      },
      error: (error: HttpErrorResponse) => { 
        if (this.onError) {
          if (error.error instanceof ErrorEvent) {
           this.onError(error.error.message);
          } else {
            this.onError(`${error.status} - ${error.message}`);
          }
        }
      }
    });
  }
}
