import { Component, EventEmitter, NgZone, OnInit, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSliderModule } from '@angular/material/slider';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ColorService } from '../../services/color.service';
import { FormsModule } from '@angular/forms';

export interface ColorFilterEvent {
  cieLchDistance: number,
  threshold: number
}

@Component({
  selector: 'app-color-filter',
  imports: [
    MatButtonModule,
    MatSliderModule,
    MatCardModule,
    FormsModule
  ],
  templateUrl: './color-filter.component.html',
  styleUrl: './color-filter.component.css'
})
export class ColorFilterComponent  {
  @Output() applyFilterEvent = new EventEmitter<ColorFilterEvent>();
  @Output() clearFilterEvent = new EventEmitter();

  selectedColor: string = '#000000';
  distanceThreshold: number = 50;

  constructor(private colorService: ColorService,
    private snackBar: MatSnackBar,
    private zone: NgZone) {
  }
  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }

  applyFilter() {
    if (!this.selectedColor) return;
    this.colorService.getCieLchDistance(this.selectedColor).subscribe({
      next: result => {
        this.applyFilterEvent.emit({ cieLchDistance: result, threshold: this.distanceThreshold });
      },
      error: (err) => {
        this.showSnack('Error querying CieLch distance to reference: ' + err);
      }
    });
  }

  clearFilter() {
    this.distanceThreshold = 50;
    this.clearFilterEvent.emit();
  }
}
