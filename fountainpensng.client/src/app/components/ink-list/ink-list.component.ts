import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { InkService } from '../../services/ink.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSortModule, Sort } from '@angular/material/sort';

@Component({
  selector: 'app-ink-list',
  standalone: true,
  imports: [MatTableModule, MatSortModule, CommonModule],
  templateUrl: './ink-list.component.html',
  styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  //todo!!!!!!!!!
  displayedColumns: string[] = ['maker', 'inkName', 'color', 'comment', 'rating', 'currentPen'];
  dataSource: InkForListDTO[] = [];
  sortedData: InkForListDTO[] = [];

  constructor(private inkService: InkService, private router: Router) {

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
          return this.compare(a.maker, b.maker, isAsc);
        case 'inkName':
          return this.compare(a.inkName, b.inkName, isAsc);
        case 'ml':
          return this.compare(a.ml, b.ml, isAsc);
        case 'color':
          return this.compare(a.color_CIELAB_a, b.color, isAsc); //TODO: sortable color
        case 'rating':
          return this.compare(a.rating, b.rating, isAsc);
        case 'currentPen':
          return this.compare(a.oneCurrentPenMaker + a.oneCurrentPenModelName, 
            b.oneCurrentPenMaker + b.oneCurrentPenModelName, isAsc);
        default:
          return 0;
      }
    });
  }
  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
