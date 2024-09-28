import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-image-uploader',
  standalone: true,
  imports: [],
  templateUrl: './image-uploader.component.html',
  styleUrl: './image-uploader.component.css'
})
export class ImageUploaderComponent {
  r2ApiUrl = environment.r2ApiUrl;

  //`${R2_API_ADDRESS}/upload-image?fileName=${encodeURIComponent(fileName)}`;

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
    this.httpClient.put<unknown>(
      `${this.r2ApiUrl}/upload-image?fileName=${encodeURIComponent(file.name)}`, file, {  
      // reportProgress: true,  
      // observe: 'events'  
    }).subscribe(
      (response:any) => {
        console.log(`Ok: ${response}`); 
      },                            
    (error:any) => {
      console.log(`Error: ${error}`); 
    }); 
  }
}
