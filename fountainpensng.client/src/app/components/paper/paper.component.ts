import { Component, Input, NgZone, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PaperService } from '../../services/paper.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Paper } from '../../../dtos/Paper';
import { ImageUploaderComponent } from '../image-uploader/image-uploader.component';
import { R2UploadService } from '../../services/r2-upload.service';
import { MatDialog } from '@angular/material/dialog';
import { MessageBoxComponent } from '../message-box/message-box.component';


@Component({
    selector: 'app-paper',
    imports: [
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatError,
        MatSelect,
        MatOption,
        MatIcon,
        CommonModule,
        MatInputModule,
        MatButtonModule,
        MatAutocompleteModule,
        ImageUploaderComponent,
    ],
    templateUrl: './paper.component.html',
    styleUrl: './paper.component.css'
})
export class PaperComponent implements OnInit {
  toUploadFile: File | null = null;

  @Input()
  set id(id: number) {
    if (id) this.paper$ = this.paperService.getPaper(id);
  }

  paper: Paper = {
    id: 0,
    maker: '',
    paperName: '',
    comment: '',
    photo: '',
    rating: 0,
    imageObjectKey: '',
    imageUrl: '',
  };

  myControl = new FormControl('');
  paper$: Observable<Paper> | undefined;
  form: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private paperService: PaperService,
    private snackBar: MatSnackBar,
    private zone: NgZone,
    private route: ActivatedRoute,
    private r2: R2UploadService,
    private dialog: MatDialog
  ) { }

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      maker: ['', Validators.required],
      paperName: ['', Validators.required],
      comment: ['', Validators.required],
      color: ['', Validators.required],
      rating: ['', Validators.required],
      ml: ['', Validators.required],
    });

    if (this.paper$) {
      this.paper$.subscribe((i) => {
        //would be better in prefetch
        this.paper = i;
        i.imageUrl = this.r2.getImageUrl(i.imageObjectKey);
        this.form.patchValue({
          maker: i.maker,
          paperName: i.paperName,
          comment: i.comment,
          rating: i.rating,
        });
      });
    }
  }

  onFileSelected(file: File | null): void {
    this.toUploadFile = file;
  }

  onSubmit() {
    if (this.toUploadFile) {
      this.r2.uploadFile(this.toUploadFile).subscribe({
        next: (r) => {
          if (r.errorMsg) {
            this.showSnack(r.errorMsg);
          } else if (r.guid) {
            this.showSnack('Image upload successful');
            this.paper.imageObjectKey = r.guid;
            this.upsertPaper();
          }
        },
        error: (err) => {
          this.showSnack('Upload failed:' + err);
        },
      });
    } else {
      this.upsertPaper();
    }
  }

  upsertPaper() {
    this.paper.maker = this.form.get('maker')?.value;
    this.paper.paperName = this.form.get('paperName')?.value;
    this.paper.comment = this.form.get('comment')?.value;
    this.paper.rating = this.form.get('rating')?.value;
    if (this.paper.id == 0) {
      this.paperService.createPaper(this.paper).subscribe({
        next: () => {
          this.showSnack('Paper added!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    } else {
      this.paperService.updatePaper(this.paper).subscribe({
        next: () => {
          this.showSnack('Paper updated!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    }
  }

  deletePaper(): void {
    if (this.paper.id) {
      const dialogRef = this.dialog.open(MessageBoxComponent, {
        width: '250px',
        data: {
          title: 'Confirm Action',
          message: 'Are you sure you want to proceed?',
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.paperService.deletePaper(this.paper.id).subscribe({
            next: () => {
              this.showSnack('paper deleted!');
              this.router.navigate(['/']);
              //go back to main
            },
            error: (e) => {
              this.showSnack(e);
            },
          });
        }
      });
    }
  }
}
