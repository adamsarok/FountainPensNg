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
import { MatOption } from '@angular/material/select';
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
import { InkedUpForListDTO } from '../../../dtos/InkedUpDTO';
import { MatTableModule } from '@angular/material/table';
import { R2UploadService } from '../../services/r2-upload.service';
import { ImageUploaderComponent } from '../image-uploader/image-uploader.component';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { MatDialog } from '@angular/material/dialog';
import { InkedupService } from '../../services/inkedup.service';

@Component({
    selector: 'app-pen',
    imports: [
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatError,
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
    styleUrl: './pen.component.css'
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
    currentInkComment: '',
    imageObjectKey: '',
    imageUrl: '',
    cieLch_sort: 0,
    lastInkedAt: new Date(),
  };

  currentInk = new FormControl('');
  filteredOptions: Observable<InkForListDTO[]> | undefined;
  pen$: Observable<FountainPen> | undefined;
  penForm: FormGroup = new FormGroup({});
  inkupForm: FormGroup = new FormGroup({});
  inks: InkForListDTO[] = [];
  inkedUps: InkedUpForListDTO[] = [];
  validationErrors: string[] | undefined;
  inkedUpDisplayedColumns: string[] = ['inkedAt', 'matchRating', 'ink'];

  constructor(
    private fb: FormBuilder,
    private penService: PenService,
    private inkedUpService: InkedupService,
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
    });

    this.inkupForm = this.fb.group({
      currentInk: this.currentInk,
      currentInkRating: [''],
      currentInkComment: ['']
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
        this.inkedUps = p.inkedUps.sort((a, b) => new Date(b.inkedAt).getTime() - new Date(a.inkedAt).getTime());
        this.penForm.patchValue({
          maker: p.maker,
          modelName: p.modelName,
          comment: p.comment,
          color: p.color,
          rating: p.rating,
          nib: p.nib,
        });
        this.inkupForm.patchValue({
          currentInk: ink,
          currentInkRating: p.currentInkRating,
          currentInkComment: p.currentInkComment
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
            this.upsertPen();
            this.r2.getImageUrl(r.guid).subscribe({
              next: (r) => {
                this.pen.imageUrl = r;
              }
            });
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
  cleanPen() {
    if (this.pen.id != 0) {
      this.penService.emptyPen(this.pen.id).subscribe({
        next: () => {
          this.showSnack('Pen is empty!');
        },
        error: (e) => {
          this.showSnack(e);
        },
      });
    }
  }
  upsertPen() {
    this.pen.maker = this.penForm.get('maker')?.value;
    this.pen.modelName = this.penForm.get('modelName')?.value;
    this.pen.comment = this.penForm.get('comment')?.value;
    this.pen.rating = this.penForm.get('rating')?.value;
    this.pen.color = this.penForm.get('color')?.value;
    this.pen.nib = this.penForm.get('nib')?.value;
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
  onInkUp() {
    const inkComment = this.inkupForm.get('currentInkComment')?.value ?? '';
    const inkRating = this.inkupForm.get('currentInkRating')?.value;
    const ink = this.inkupForm.get('currentInk')?.value;
    if (!inkRating || !ink || !ink.id)
      this.showSnack('Select an ink and a match rating');
    else {
      this.inkedUpService
        .createInkedUp({
          id: 0,
          inkedAt: new Date(),
          matchRating: inkRating,
          fountainPenId: this.pen.id,
          inkId: ink.id,
          isCurrent: true,
          comment: inkComment
        })
        .subscribe({
          next: () => {
            this.showSnack('Inked up!');
          },
          error: (e) => {
            this.showSnack(e);
            console.log(e);
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
