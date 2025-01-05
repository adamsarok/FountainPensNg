import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { PenService } from '../../services/pen.service';
import { FountainPen } from '../../../dtos/FountainPen';
import { CommonModule } from '@angular/common';
import { ComparerService } from '../../services/comparer.service';

@Component({
    selector: 'app-pen-list',
    imports: [MatTableModule, MatSortModule, CommonModule],
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
    'currentInk',
  ];
  dataSource: FountainPen[] = [];
  sortedData: FountainPen[] = [];

  ngOnInit(): void {
    this.penService.getPens().subscribe({
      next: (r) => {
        this.dataSource = r;
        this.sortedData = this.dataSource.slice();
      },
    });
  }
  constructor(private penService: PenService, private router: Router, private comparer: ComparerService) { }
  openPen(id: number) {
    this.router.navigate(['/pen/' + id]);
  }
  sortData(sort: Sort) {
    const data = this.dataSource.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'maker':
          return this.comparer.compare(a.maker, b.maker, isAsc);
        case 'modelName':
          return this.comparer.compare(a.modelName, b.modelName, isAsc);
        case 'nib':
          return this.comparer.compare(a.nib, b.nib, isAsc);
        case 'color':
          return this.comparer.compare(a.color, b.color, isAsc); //TODO: sortable color
        case 'rating':
          return this.comparer.compare(a.rating, b.rating, isAsc);
        // case 'currentInk': TODO ???
        //   return compare(a.c, b.currentInk, isAsc);
        default:
          return 0;
      }
    });
  }
}
