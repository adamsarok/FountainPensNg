import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { InkService } from '../../services/ink.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSortModule, Sort } from '@angular/material/sort';
import { ComparerService } from '../../services/comparer.service';

@Component({
    selector: 'app-ink-list',
    imports: [MatTableModule, MatSortModule, CommonModule],
    templateUrl: './ink-list.component.html',
    styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  displayedColumns: string[] = ['maker',
    'inkName',
    'color',
    'comment',
    'rating',
    'currentPen'];
  dataSource: InkForListDTO[] = [];
  sortedData: InkForListDTO[] = [];

  constructor(private inkService: InkService, private router: Router, private comparer: ComparerService) {

  }

  ngOnInit(): void {
    this.inkService.getInks().subscribe({
      next: r => {
        this.dataSource = r;
        this.sortedData = this.dataSource.slice();
      }
    });
  }

  openInk(id: number) {
    this.router.navigate(['/ink/' + id]);
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
        case 'inkName':
          return this.comparer.compare(a.inkName, b.inkName, isAsc);
        case 'ml':
          return this.comparer.compare(a.ml, b.ml, isAsc);
        case 'color':
          return this.comparer.compare(a.cieLch_sort, b.cieLch_sort, isAsc);
        case 'rating':
          return this.comparer.compare(a.rating, b.rating, isAsc);
        case 'currentPen':
          return this.comparer.compare(a.oneCurrentPenMaker + a.oneCurrentPenModelName, 
            b.oneCurrentPenMaker + b.oneCurrentPenModelName, isAsc);
        default:
          return 0;
      }
    });
  }
}
