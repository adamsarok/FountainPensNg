import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { PenService } from '../../services/pen.service';
import { FountainPen } from '../../../dtos/FountainPen';

@Component({
  selector: 'app-pen-list',
  standalone: true,
  imports: [MatTableModule, MatSortModule],
  templateUrl: './pen-list.component.html',
  styleUrl: './pen-list.component.css',
})
export class PenListComponent implements OnInit {
  displayedColumns: string[] = [
    'maker',
    'modelName',
    'color',
    'nib',
    'rating',
    'currentInk',
    'inkColor',
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
  constructor(private penService: PenService, private router: Router) { }
  openPen(id: number) {
    this.router.navigate(['/pen/' + id]);
  }
  sortData(sort: Sort) {
    console.log(sort);
    const data = this.dataSource.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'maker':
          return this.compare(a.maker, b.maker, isAsc);
        case 'modelName':
          return this.compare(a.modelName, b.modelName, isAsc);
        case 'nib':
          return this.compare(a.nib, b.nib, isAsc);
        case 'color':
          return this.compare(a.color, b.color, isAsc); //TODO: sortable color
        case 'rating':
          return this.compare(a.rating, b.rating, isAsc);
        // case 'currentInk': TODO ???
        //   return compare(a.c, b.currentInk, isAsc);
        // case 'inkColor':
        //   return compare(a.inkColor, b.inkColor, isAsc);
        default:
          return 0;
      }
    });
  }
  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
