import { Component, Input, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PenService } from '../../services/pen.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FountainPen } from '../../../dtos/FountainPen';
import { InkService } from '../../services/ink.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { Observable, map, startWith } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { Ink } from '../../../dtos/Ink';

@Component({
  selector: 'app-ink',
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
  templateUrl: './ink.component.html',
  styleUrl: './ink.component.css'
})
export class InkComponent  implements OnInit {
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
    penDisplayName: null
  }

  myControl = new FormControl('');
  ink$: Observable<Ink> | undefined;
  form: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(private fb: FormBuilder, 
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
    this.upsertInk();
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
      this.ink$.subscribe(
        i => { //would be better in prefetch
          console.log(i);
          this.ink = i;
          this.form.patchValue({
            maker: i.maker,
            inkName: i.inkName,
            comment: i.comment,
            color: i.color,
            rating: i.rating,
            ml: i.ml
          });
        } 
      );
    }
  }

  upsertInk() {
    console.log(this.form.value);
    this.ink.maker = this.form.get('maker')?.value;
    this.ink.inkName = this.form.get('inkName')?.value;
    this.ink.color = this.form.get('color')?.value;
    this.ink.comment = this.form.get('comment')?.value;
    this.ink.rating = this.form.get('rating')?.value;
    this.ink.ml = this.form.get('ml')?.value;
    if (this.ink.id == 0) {
      this.inkService.createInk(this.ink).subscribe({
        next: () => {
          this.showSnack("Ink added!");
        },
        error: e => {
          this.showSnack(e);
        }
    });
  } else {
    this.inkService.updateInk(this.ink).subscribe({
      next: () => {
        this.showSnack("Ink updated!");
      },
      error: e => {
        this.showSnack(e);
      }
    });
    }
  }

}
