import { Component, Input, NgZone, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, map, startWith } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FountainPen } from '../../../dtos/FountainPen';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { PenService } from '../../services/pen.service';
import { InkedUpForListDTO } from '../../../dtos/InkedUpForListDTO';
import { MatTableModule } from '@angular/material/table';
import { R2UploadService } from '../../services/r2-upload.service';
import { ImageUploaderComponent } from '../image-uploader/image-uploader.component';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-pen',
  standalone: true,
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
    MatTableModule,
    ImageUploaderComponent,
  ],
  templateUrl: './pen.component.html',
  styleUrl: './pen.component.css',
})
export class PenComponent implements OnInit {
  toUploadFile: File | null = null;

  @Input()
  set id(id: number) {
    if (id) this.pen$ = this.penService.getPen(id);
  }

  pen: FountainPen = {
    id: 0,
    maker: '',
    modelName: '',
    comment: '',
    photo: '',
    color: '',
    rating: 0,
    nib: '',
    inkedUps: [],
    currentInkId: 0,
    currentInkRating: 0,
    imageObjectKey: '',
    imageUrl: '',
  };

  currentInk = new FormControl('');
  filteredOptions: Observable<InkForListDTO[]> | undefined;
  pen$: Observable<FountainPen> | undefined;
  penForm: FormGroup = new FormGroup({});
  inks: InkForListDTO[] = [];
  inkedUps: InkedUpForListDTO[] = [];
  validationErrors: string[] | undefined;
  inkedUpDisplayedColumns: string[] = ['inkedAt', 'matchRating', 'ink'];

  constructor(
    private fb: FormBuilder,
    private penService: PenService,
    private snackBar: MatSnackBar,
    private zone: NgZone,
    private route: ActivatedRoute,
    private r2: R2UploadService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }
  onFileSelected(file: File | null): void {
    this.toUploadFile = file;
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.inks = data['inks'];
    });

    this.filteredOptions = this.currentInk.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value || ''))
    );

    this.penForm = this.fb.group({
      maker: ['', Validators.required],
      modelName: ['', Validators.required],
      comment: ['', Validators.required],
      color: ['', Validators.required],
      rating: ['', Validators.required],
      nib: ['', Validators.required],
      currentInk: this.currentInk,
      currentInkRating: [''],
    });
    if (this.pen$) {
      this.pen$.subscribe((p) => {
        //would be better in prefetch
        this.pen = p;
        let ink: InkForListDTO | undefined;
        if (p.currentInkId) {
          const inks = this.inks.filter((x) => x.id === p.currentInkId);
          if (inks) ink = inks[0];
        }
        p.imageUrl = this.r2.getImageUrl(p.imageObjectKey);
        console.log(p.imageUrl);
        this.inkedUps = p.inkedUps;
        this.penForm.patchValue({
          maker: p.maker,
          modelName: p.modelName,
          comment: p.comment,
          color: p.color,
          rating: p.rating,
          nib: p.nib,
          currentInk: ink,
          currentInkRating: p.currentInkRating,
        });
      });
    }
  }
  displayFn(ink: InkForListDTO): string {
    return ink ? ink.maker + ' - ' + ink.inkName : '';
  }
  private _filter(value: unknown): InkForListDTO[] {
    if (typeof value != 'string') return this.inks;
    if (!this.inks) {
      const empty: InkForListDTO[] = [];
      return empty;
    }
    const filterValue = value.toLowerCase();
    return this.inks?.filter((option) =>
      (option.maker + ' ' + option.inkName).toLowerCase().includes(filterValue)
    );
  }
  onSubmit() {
    if (this.toUploadFile) {
      this.r2.uploadFile(this.toUploadFile).subscribe({
        next: (r) => {
          if (r.errorMsg) {
            this.showSnack(r.errorMsg);
          } else if (r.guid) {
            this.showSnack('Image upload successful');
            this.pen.imageObjectKey = r.guid;
            console.log(this.pen);
            this.upsertPen();
          }
        },
        error: (err) => {
          this.showSnack('Upload failed:' + err);
        },
      });
    } else {
      this.upsertPen();
    }
  }
  upsertPen() {
    this.pen.maker = this.penForm.get('maker')?.value;
    this.pen.modelName = this.penForm.get('modelName')?.value;
    this.pen.comment = this.penForm.get('comment')?.value;
    this.pen.rating = this.penForm.get('rating')?.value;
    this.pen.color = this.penForm.get('color')?.value;
    this.pen.nib = this.penForm.get('nib')?.value;
    if (this.penForm.get('currentInkRating')?.value) {
      this.pen.currentInkRating = this.penForm.get('currentInkRating')?.value;
    } else {
      this.pen.currentInkRating = null;
    }
    const ink = this.penForm.get('currentInk')?.value;
    if (ink) {
      this.pen.currentInkId = ink.id;
    } else this.pen.currentInkId = null;
    if (this.pen.id == 0) {
      this.penService.createPen(this.pen).subscribe({
        next: () => {
          this.showSnack('Pen added!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    } else {
      this.penService.updatePen(this.pen).subscribe({
        next: () => {
          this.showSnack('Pen updated!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    }
  }
  deletePen(): void {
    if (this.pen.id) {
      const dialogRef = this.dialog.open(MessageBoxComponent, {
        width: '250px',
        data: {
          title: 'Confirm Action',
          message: 'Are you sure you want to proceed?',
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.penService.deletePen(this.pen.id).subscribe({
            next: () => {
              this.showSnack('Pen deleted!');
              this.router.navigate(['/']);
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
