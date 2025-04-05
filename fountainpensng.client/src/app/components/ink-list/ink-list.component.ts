import { Component, OnInit, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { InkService } from '../../services/ink.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSortModule, Sort } from '@angular/material/sort';
import { ComparerService } from '../../services/comparer.service';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-ink-list',
    imports: [MatTableModule, MatSortModule, CommonModule, FormsModule, MatButtonModule],
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
    
  dataSource = signal<InkForListDTO[]>([]);
  originalData: InkForListDTO[] = [];
  selectedColor: string = '#000000';

  constructor(private inkService: InkService, private router: Router, private comparer: ComparerService) {
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
        case 'currentPen':
          return this.comparer.compare(a.oneCurrentPenMaker + a.oneCurrentPenModelName, 
            b.oneCurrentPenMaker + b.oneCurrentPenModelName, isAsc);
        default:
          return 0;
      }
    }));
  }

  applyFilter() {
    if (!this.selectedColor) {
      this.dataSource.set(this.originalData);
      return;
    }
    //todo: filter not by exact color but by closeness in CieLAB or CieLch
    const filteredData = this.originalData.filter(ink => {
      return ink.color.toLowerCase() === this.selectedColor.toLowerCase();
    });
    this.dataSource.set(filteredData);
  }

  clearFilter() {
    console.log(this.originalData);
    this.selectedColor = '#000000';
    this.dataSource.set(this.originalData);
  }
}
