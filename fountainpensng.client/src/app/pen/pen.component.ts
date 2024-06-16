import { Component, Input, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PenService } from '../services/pen.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FountainPen } from '../../dtos/FountainPen';
import { InkService } from '../services/ink.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { Observable, map, startWith } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import {MatAutocompleteModule} from '@angular/material/autocomplete';

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
    this.pen$ = this.penService.getPen(id);
    console.log(id);
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
    currentInkId: 0
  };
  
  myControl = new FormControl('');
  filteredOptions: Observable<InkForListDTO[]> | undefined;
  pen$: Observable<FountainPen> | undefined;
  penForm: FormGroup = new FormGroup({});
  inks: InkForListDTO[] = [];
  validationErrors: string[] | undefined;
  currentInk: InkForListDTO | undefined;
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
    this.createPen();
  }
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // console.log('checking route data');
      // console.log(data['inks']);
      this.inks = data['inks'];
    });

    this.filteredOptions = this.myControl.valueChanges.pipe(
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
      currentInk: [''],
      //confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    }); //using builder service

    if (this.pen$) {
      this.pen$.subscribe(
        x => { //would be better in prefetch
          console.log(x);
          this.pen = x;
          this.penForm.patchValue({
            maker: x.maker,
            modelName: x.modelName,
            comment: x.comment,
            color: x.color,
            rating: x.rating,
            nib: x.nib
            //todo get ink
            //this.penForm.value.currentInk = x.currentInkId;
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
  private _filter(value: string): InkForListDTO[] {
    console.log(this.inks);
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

  createPen() {
    console.log(this.pen);
    console.log(this.currentInk);
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
