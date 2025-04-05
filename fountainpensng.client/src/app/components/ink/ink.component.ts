import { Component, Input, NgZone, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { InkService } from '../../services/ink.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Ink } from '../../../dtos/Ink';
import { ImageUploaderComponent } from '../image-uploader/image-uploader.component';
import { R2UploadService } from '../../services/r2-upload.service';
import { MatDialog } from '@angular/material/dialog';
import { MessageBoxComponent } from '../message-box/message-box.component';

@Component({
    selector: 'app-ink',
    imports: [
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatError,
        MatIcon,
        CommonModule,
        MatInputModule,
        MatButtonModule,
        MatAutocompleteModule,
        ImageUploaderComponent,
    ],
    templateUrl: './ink.component.html',
    styleUrl: './ink.component.css'
})
export class InkComponent implements OnInit {
  toUploadFile: File | null = null;

  @Input()
  set id(id: number) {
    if (id) this.ink$ = this.inkService.getInk(id);
  }

  ink: Ink = {
      id: 0,
      maker: '',
      inkName: '',
      comment: '',
      photo: '',
      color: '',
      rating: 0,
      ml: 0,
      inkedUps: [],
      currentPens: [],
      penDisplayName: null,
      imageObjectKey: '',
      imageUrl: '',
      cieLAB_Sort: 0
  };

  myControl = new FormControl('');
  ink$: Observable<Ink> | undefined;
  form: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private inkService: InkService,
    private snackBar: MatSnackBar,
    private zone: NgZone,
    private route: ActivatedRoute,
    private r2: R2UploadService,
    private dialog: MatDialog,
  ) {}

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      maker: ['', Validators.required],
      inkName: ['', Validators.required],
      comment: ['', Validators.required],
      color: ['', Validators.required],
      rating: ['', Validators.required],
      ml: ['', Validators.required],
    });

    if (this.ink$) {
      this.ink$.subscribe((i) => {
        //would be better in prefetch
        this.ink = i;
        i.imageUrl = this.r2.getImageUrl(i.imageObjectKey);
        this.form.patchValue({
          maker: i.maker,
          inkName: i.inkName,
          comment: i.comment,
          color: i.color,
          rating: i.rating,
          ml: i.ml,
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
            this.ink.imageObjectKey = r.guid;
            this.upsertInk();
          }
        },
        error: (err) => {
          this.showSnack('Upload failed:' + err);
        },
      });
    } else {
      this.upsertInk();
    }
  }

  upsertInk() {
    this.ink.maker = this.form.get('maker')?.value;
    this.ink.inkName = this.form.get('inkName')?.value;
    this.ink.color = this.form.get('color')?.value;
    this.ink.comment = this.form.get('comment')?.value;
    this.ink.rating = this.form.get('rating')?.value;
    this.ink.ml = this.form.get('ml')?.value;
    if (this.ink.id == 0) {
      this.inkService.createInk(this.ink).subscribe({
        next: () => {
          this.showSnack('Ink added!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    } else {
      this.inkService.updateInk(this.ink).subscribe({
        next: () => {
          this.showSnack('Ink updated!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    }
  }

  deleteInk(): void {
    if (this.ink.id) {
      const dialogRef = this.dialog.open(MessageBoxComponent, {
        width: '250px',
        data: {
          title: 'Confirm Action',
          message: 'Are you sure you want to proceed?',
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.inkService.deleteInk(this.ink.id).subscribe({
            next: () => {
              this.showSnack('Ink deleted!');
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
