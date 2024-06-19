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
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { FountainPen } from '../../../dtos/FountainPen';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { PenService } from '../../services/pen.service';
import { InkService } from '../../services/ink.service';

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
    MatAutocompleteModule
  ],
  templateUrl: './pen.component.html',
  styleUrl: './pen.component.css'
})
export class PenComponent implements OnInit {
  @Input()
  set id(id: number) {
    if (id) this.pen$ = this.penService.getPen(id);
  }

  //id: number | undefined;
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
    currentInk: null,
    currentInkId: 0,
    currentInkRating: 0
  };
  
  currentInk = new FormControl('');
  filteredOptions: Observable<InkForListDTO[]> | undefined;
  pen$: Observable<FountainPen> | undefined;
  penForm: FormGroup = new FormGroup({});
  inks: InkForListDTO[] = [];
  validationErrors: string[] | undefined;
  //currentInk: InkForListDTO | undefined;
  constructor(private fb: FormBuilder, 
    private penService: PenService, 
    private router: Router, 
    private inkService: InkService,
    private snackBar: MatSnackBar,
    private zone: NgZone,
    private route: ActivatedRoute
  ) { }

  showSnack(msg: any): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }
  onSubmit() {
    this.upsertPen();
  }
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // console.log('checking route data');
      // console.log(data['inks']);
      this.inks = data['inks'];
    });

    this.filteredOptions = this.currentInk.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );

    this.penForm = this.fb.group({
      maker: ['', Validators.required],
      modelName: ['', Validators.required],
      comment: ['', Validators.required],
      color: ['', Validators.required],
      rating: ['', Validators.required],
      nib: ['', Validators.required],
      currentInk: this.currentInk, //if I define a form control earlier defining a different form control here ofc doesnt work
      currentInkRating: ['']
      //confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    }); //using builder service

    if (this.pen$) {
      this.pen$.subscribe(
        p => { //would be better in prefetch
          console.log(p);
          this.pen = p;
          let ink: InkForListDTO | undefined;
          if (p.currentInkId) {
            const inks = this.inks.filter(x => x.id === p.currentInkId);
            if (inks) ink = inks[0];
            console.log(ink);
          }
          this.penForm.patchValue({
            maker: p.maker,
            modelName: p.modelName,
            comment: p.comment,
            color: p.color,
            rating: p.rating,
            nib: p.nib,
            currentInk: ink,
            currentInkRating: p.currentInkRating
          });
        } 
      );
    }
  }
  displayFn(ink: InkForListDTO): string {
    //TODO: something is not right, remove ngModel and check if solved
    // let reallyInk: InkForListDTO | undefined;
    // if (typeof ink === 'number') {  //TODO: something is not right, remove ngModel and check if solved
    //   console.log(this.inks);
    //   const filtered = this.inks && this.inks.filter(x => x.id == ink).slice(1);
    //   if (filtered && filtered[0]) reallyInk = filtered[0];
    // } else reallyInk = ink;
    // console.log(reallyInk);
    return ink ? ink.maker + " - " + ink.inkName : '';
  }
  private _filter(value: any): InkForListDTO[] {
    if (value as InkForListDTO) return this.inks;
    console.log('nem mukodo value:');
    console.log(typeof value);
    console.log(value as InkForListDTO);
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

  upsertPen() {
    this.pen.maker = this.penForm.get('maker')?.value;
    this.pen.modelName = this.penForm.get('modelName')?.value;
    this.pen.comment = this.penForm.get('comment')?.value;
    this.pen.rating = this.penForm.get('rating')?.value;
    this.pen.color = this.penForm.get('color')?.value;
    this.pen.nib = this.penForm.get('nib')?.value;
    if (this.penForm.get('currentInkRating')?.value) { //why doesn't a null coalesce work here?
      this.pen.currentInkRating = this.penForm.get('currentInkRating')?.value;
    } else {
      this.pen.currentInkRating = null;
    }
    const ink = this.penForm.get('currentInk')?.value;
    if (ink) { 
      this.pen.currentInk = ink;
      this.pen.currentInkId = ink.id;
    }
    else this.pen.currentInkId = null;
    console.log(this.pen);
    if (this.pen.id == 0) {
      this.penService.createPen(this.pen).subscribe({
        next: () => {
          this.showSnack("Pen added!");
        },
        error: e => {
          this.showSnack(e);
        }
    });
  } else {
    this.penService.updatePen(this.pen).subscribe({
      next: () => {
        this.showSnack("Pen updated!");
      },
      error: e => {
        this.showSnack(e);
      }
    });
    }
  }

}
