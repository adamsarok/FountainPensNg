import { Component, Input, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
//import { FountainPen } from '../../../dtos/FountainPen';
//import { InkForListDTO } from '../../../dtos/InkForListDTO';
//import { PenService } from '../../services/pen.service';
//import { InkService } from '../../services/ink.service';
import { MatTableModule } from '@angular/material/table';
import { InkedupService } from '../../services/inkedup.service';
import { InkedUp } from '../../../dtos/InkedUp';
import { InkedUpForListDTO } from '../../../dtos/InkedUpForListDTO';
import { FountainPen } from '../../../dtos/FountainPen';
import { InkForListDTO } from '../../../dtos/InkForListDTO';

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
    MatTableModule
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
    inkedAt: '',
    matchRating: 0,
    penMaker: '',
    penName: '',
    penColor: '',
    inkMaker: '',
    inkName: '',
    inkColor: '',
    fountainPenId: 0,
    inkId: 0
  };

  constructor(private inkedUpService: InkedupService,
    private fb: FormBuilder, 
    //private penService: PenService, 
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
      ink: this.ink, //if I define a form control earlier defining a different form control here ofc doesnt work
      pen: this.pen
      //confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    }); //using builder service

    if (this.inkedUp$) {
      this.inkedUp$.subscribe(
        p => { //would be better in prefetch
          console.log(p);
          this.inkedUp = p;
          // let ink: InkForListDTO | undefined;
          // if (p.inkId) {
          //   const inks = this.inks.filter(x => x.id === p.inkId);
          //   if (inks) ink = inks[0];
          // }
          // if (p.penId) {
          //   const pens = this.pens.filter(x => x.id === p.penId);
          //   if (pens) pen = pens[0];
          // }
          const ink = this.inks.filter(x => x.id === p.inkId);
          if (!ink) return;
          const pen = this.pens.filter(x => x.id === p.fountainPenId);
          if (!pen) return;
          //console.log(pen[0]);
          //console.log(ink[0]);
          this.form.patchValue({
            ink: ink[0],
            pen: pen[0],
            matchRating: p.matchRating,
            inkedAt: p.inkedAt
          });
          console.log(this.form.value);
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

  private _inkFilter(value: any): InkForListDTO[] {
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

  private _penFilter(value: any): FountainPen[] {
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

  showSnack(msg: any): void {
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
    } else return; //error?
    const pen = this.form.get('pen')?.value;
    if (pen) { 
      this.inkedUp.fountainPenId = pen.id;
    } else return; //error?
    if (this.inkedUp.id == 0) {
      this.inkedUpService.createInkedUp(this.inkedUp).subscribe({
        next: () => {
          this.showSnack("Ink-up added!");
        },
        error: e => {
          this.showSnack(e);
        }
    });
  } else {
    this.inkedUpService.updateInkedUp(this.inkedUp).subscribe({
      next: () => {
        this.showSnack("Ink-up updated!");
      },
      error: e => {
        this.showSnack(e);
      }
    });
    }
  }
}