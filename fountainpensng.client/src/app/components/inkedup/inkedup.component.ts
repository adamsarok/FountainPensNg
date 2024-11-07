import { Component, Input, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, map, startWith } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTableModule } from '@angular/material/table';
import { InkedupService } from '../../services/inkedup.service';
import { InkedUpForListDTO } from '../../../dtos/InkedUpDTO';
import { FountainPen } from '../../../dtos/FountainPen';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-inkedup',
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
    MatDatepickerModule,
    MatNativeDateModule 
  ],
  templateUrl: './inkedup.component.html',
  styleUrl: './inkedup.component.css'
})
export class InkedupComponent implements OnInit {
  @Input()
  set id(id: number) {
    if (id) this.inkedUp$ = this.inkedUpService.getInkedUp(id);
  }
  inkedUp$: Observable<InkedUpForListDTO> | undefined;
  inks: InkForListDTO[] = [];
  pens: FountainPen[] = [];
  ink = new FormControl('');
  pen = new FormControl('');
  inkFilteredOptions$: Observable<InkForListDTO[]> | undefined;
  penFilteredOptions$: Observable<FountainPen[]> | undefined;
  form: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  inkedUp: InkedUpForListDTO = {
    id: 0,
    inkedAt: new Date(),
    matchRating: 0,
    penMaker: '',
    penName: '',
    penColor: '',
    inkMaker: '',
    inkName: '',
    inkColor: '',
    fountainPenId: 0,
    inkId: 0,
    isCurrent: false
  };

  constructor(private inkedUpService: InkedupService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private zone: NgZone,
    private route: ActivatedRoute
  ) {

  }
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.inks = data['inks'];
      this.pens = data['pens'];
    });

    this.inkFilteredOptions$ = this.ink.valueChanges.pipe(
      startWith(''),
      map(value => this._inkFilter(value || '')),
    );
    this.penFilteredOptions$ = this.pen.valueChanges.pipe(
      startWith(''),
      map(value => this._penFilter(value || '')),
    );

    this.form = this.fb.group({
      inkedAt: [''],
      matchRating: ['', Validators.required],
      comment: ['', Validators.required],
      ink: this.ink,
      pen: this.pen
    });

    if (this.inkedUp$) {
      this.inkedUp$.subscribe(
        p => { //would be better in prefetch
          this.inkedUp = p;
          const ink = this.inks.filter(x => x.id === p.inkId);
          if (!ink) return;
          const pen = this.pens.filter(x => x.id === p.fountainPenId);
          if (!pen) return;
          this.form.patchValue({
            ink: ink[0],
            pen: pen[0],
            matchRating: p.matchRating,
            inkedAt: p.inkedAt
          });
        }
      );
    }
  }
  displayInk(ink: InkForListDTO): string {
    return ink ? ink.maker + " - " + ink.inkName : '';
  }
  displayPen(pen: FountainPen): string {
    return pen ? pen.maker + " - " + pen.modelName : '';
  }

  private _inkFilter(value: unknown): InkForListDTO[] {
    if (typeof value != "string") return this.inks;
    if (!this.inks) {
      const empty: InkForListDTO[] = [];
      return empty;
    }
    const filterValue = value.toLowerCase();
    return this.inks?.filter(
      option => (option.maker + " " + option.inkName)
        .toLowerCase()
        .includes(filterValue));
  }

  private _penFilter(value: unknown): FountainPen[] {
    if (typeof value != "string") return this.pens;
    if (!this.pens) {
      const empty: FountainPen[] = [];
      return empty;
    }
    const filterValue = value.toLowerCase();
    return this.pens?.filter(
      option => (option.maker + " " + option.modelName)
        .toLowerCase()
        .includes(filterValue));
  }

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }
  onSubmit() {
    this.upsertInkUp();
  }

  upsertInkUp() {
    this.inkedUp.matchRating = this.form.get('matchRating')?.value;
    const ink = this.form.get('ink')?.value;
    if (ink) {
      this.inkedUp.inkId = ink.id;
    } else return; //TODO: error
    const pen = this.form.get('pen')?.value;
    if (pen) {
      this.inkedUp.fountainPenId = pen.id;
    } else return; //TODO: error
    this.inkedUp.inkedAt = this.form.get('inkedAt')?.value;
    if (this.inkedUp.id == 0) {
      this.inkedUpService.createInkedUp(this.inkedUp).subscribe({
        next: () => {
          this.showSnack("Ink-up added!");
        },
        error: e => {
          console.log(e);
          this.showSnack(e);
        }
      });
    } else {
      this.inkedUpService.updateInkedUp(this.inkedUp).subscribe({
        next: () => {
          this.showSnack("Ink-up updated!");
        },
        error: e => {
          console.log(e);
          this.showSnack(e);
        }
      });
    }
  }
}
