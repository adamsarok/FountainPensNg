import { Component, NgZone, OnInit, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { InkService } from '../../services/ink.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSortModule, Sort } from '@angular/material/sort';
import { ComparerService } from '../../services/comparer.service';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { ColorService } from '../../services/color.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSliderModule } from '@angular/material/slider';
import { MatCardModule } from '@angular/material/card';
import { ColorFilterComponent, ColorFilterEvent } from '../color-filter/color-filter.component';
@Component({
  selector: 'app-ink-list',
  imports: [MatTableModule,
    MatSortModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatSliderModule,
    MatCardModule,
    ColorFilterComponent
  ],
  templateUrl: './ink-list.component.html',
  styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  displayedColumns: string[] = ['maker',
    'inkName',
    'color',
    'comment',
    'rating',
    'lastInkedAt',
    'currentPen'];

  dataSource = signal<InkForListDTO[]>([]);
  originalData: InkForListDTO[] = [];

  constructor(private inkService: InkService,
    private router: Router,
    private comparer: ComparerService,
    private colorService: ColorService,
    private snackBar: MatSnackBar,
    private zone: NgZone) {
  }

  ngOnInit(): void {
    this.inkService.getInks().subscribe({
      next: r => {
        this.dataSource.set(r);
        this.originalData = r;
      }
    });
  }

  openInk(id: number) {
    this.router.navigate(['/ink/' + id]);
  }

  sortData(sort: Sort) {
    if (!sort.active || sort.direction === '') return;
    const data = this.dataSource().slice();
    this.dataSource.set(data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'maker':
          return this.comparer.compare(a.maker, b.maker, isAsc);
        case 'inkName':
          return this.comparer.compare(a.inkName, b.inkName, isAsc);
        case 'ml':
          return this.comparer.compare(a.ml, b.ml, isAsc);
        case 'color':
          return this.comparer.compare(a.cieLch_sort, b.cieLch_sort, isAsc);
        case 'rating':
          return this.comparer.compare(a.rating, b.rating, isAsc);
        case 'comment':
          return this.comparer.compare(a.comment, b.comment, isAsc);
        case 'lastInkedAt':
          return this.comparer.compare(a.lastInkedAt, b.lastInkedAt, isAsc);
        case 'currentPen':
          return this.comparer.compare(a.oneCurrentPenMaker + a.oneCurrentPenModelName,
            b.oneCurrentPenMaker + b.oneCurrentPenModelName, isAsc);
        default:
          return 0;
      }
    }));
  }

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }

  handleApplyFilter(event: ColorFilterEvent) {
    const filteredData = this.originalData.filter(ink => {
      return Math.abs(ink.cieLch_sort - event.cieLchDistance) < event.threshold;
    });
    this.dataSource.set(filteredData);
  }

  handleClearFilter() {
    this.dataSource.set(this.originalData);
  }
}
