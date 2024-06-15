import { Component, Input, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PenService } from '../services/pen.service';
import { Router } from '@angular/router';
import { FountainPen } from '../../dtos/FountainPen';
import { InkService } from '../services/ink.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-pen',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormField, MatLabel, MatError, MatSelect, MatOption, MatIcon, CommonModule, MatInputModule],
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

  pen$: Observable<FountainPen> | undefined;
  penForm: FormGroup = new FormGroup({});
  inks: InkForListDTO[] | undefined;
  validationErrors: string[] | undefined;
  constructor(private fb: FormBuilder, 
    private penService: PenService, 
    private router: Router, 
    private inkService: InkService,
    private snackBar: MatSnackBar,
    private zone: NgZone) { }

  showSnack(msg: any): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }
  onSubmit() {
    this.createPen();
  }
  ngOnInit(): void {
    if (this.pen$) {
      this.pen$.subscribe(
        x => this.pen = x
      );
    }
    console.log(this.pen);
    this.inkService.getInks().subscribe(x => {
      this.inks = x;
    })

    this.penForm = this.fb.group({
      maker: ['', Validators.required],
      modelName: ['', Validators.required],
      comment: ['', Validators.required],
      color: ['', Validators.required],
      rating: ['', Validators.required],
      nib: ['', Validators.required],
      currentInkId: [''],
      //confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    }); //using builder service
  }

  createPen() {
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
