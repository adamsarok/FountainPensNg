import { Component, OnInit, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { PenService } from '../../services/pen.service';
import { FountainPen } from '../../../dtos/FountainPen';
import { CommonModule } from '@angular/common';
import { ComparerService } from '../../services/comparer.service';
import { ColorFilterComponent, ColorFilterEvent } from '../color-filter/color-filter.component';

@Component({
    selector: 'app-pen-list',
    imports: [
      MatTableModule, 
      MatSortModule, 
      CommonModule,
      ColorFilterComponent
    ],
    templateUrl: './pen-list.component.html',
    styleUrl: './pen-list.component.css'
})
export class PenListComponent implements OnInit {
  displayedColumns: string[] = [
    'maker',
    'modelName',
    'color',
    'nib',
    'rating',
    'lastInkedAt',
    'currentInk',
  ];
  dataSource = signal<FountainPen[]>([]);
  originalData: FountainPen[] = [];

  ngOnInit(): void {
    this.penService.getPens().subscribe({
      next: (r) => {
        this.dataSource.set(r);
        this.originalData = r;
      },
    });
  }
  constructor(private penService: PenService, private router: Router, private comparer: ComparerService) { }
  openPen(id: number) {
    this.router.navigate(['/pen/' + id]);
  }
  sortData(sort: Sort) {
    if (!sort.active || sort.direction === '') {
      return;
    }
    const data = this.dataSource().slice();
    this.dataSource.set(data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'maker':
          return this.comparer.compare(a.maker, b.maker, isAsc);
        case 'modelName':
          return this.comparer.compare(a.modelName, b.modelName, isAsc);
        case 'nib':
          return this.comparer.compare(a.nib, b.nib, isAsc);
        case 'color':
          return this.comparer.compare(a.cieLch_sort, b.cieLch_sort, isAsc);
        case 'rating':
          return this.comparer.compare(a.rating, b.rating, isAsc);
        case 'lastInkedAt':
          return this.comparer.compare(a.lastInkedAt, b.lastInkedAt, isAsc);
        // case 'currentInk': TODO ???
        //   return compare(a.c, b.currentInk, isAsc);
        default:
          return 0;
      }
    }));
  }
  handleApplyFilter(event: ColorFilterEvent) {
    const filteredData = this.originalData.filter(pen => {
      return Math.abs(pen.cieLch_sort - event.cieLchDistance) < event.threshold;
    });
    this.dataSource.set(filteredData);
  }

  handleClearFilter() {
    this.dataSource.set(this.originalData);
  }
}
