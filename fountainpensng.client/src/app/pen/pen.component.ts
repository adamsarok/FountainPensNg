import { Component, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PenService } from '../services/pen.service';
import { Router } from '@angular/router';
import { Ink } from '../../dtos/Ink';
import { InkService } from '../services/ink.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatIcon } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-pen',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormField, MatLabel, MatError, MatSelect, MatOption, MatIcon, CommonModule, MatInputModule],
  templateUrl: './pen.component.html',
  styleUrl: './pen.component.css'
})
export class PenComponent implements OnInit {
  onSubmit() {
    this.createPen();
  }
  penForm: FormGroup = new FormGroup({});
  inks: Ink[] | undefined;
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

  ngOnInit(): void {
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
    console.log(this.penForm.value);
    this.penService.createPen(this.penForm.value).subscribe({
      next: () => {
        this.showSnack("Pen added!");
      },
      error: e => {
        this.showSnack(e);
        //console.log(e);
        //this.validationErrors = e;
      }
    });
  }

}
