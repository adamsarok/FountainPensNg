import { Component, OnInit, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { Paper } from '../../../dtos/Paper';
import { PaperService } from '../../services/paper.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSortModule, Sort } from '@angular/material/sort';
import { ComparerService } from '../../services/comparer.service';

@Component({
  selector: 'app-paper-list',
  imports: [MatTableModule, MatSortModule, CommonModule],
  templateUrl: './paper-list.component.html',
  styleUrl: './paper-list.component.css'
})
export class PaperListComponent implements OnInit {
  displayedColumns: string[] = ['maker', 'paperName', 'comment', 'rating'];
  dataSource = signal<Paper[]>([]);

  constructor(private paperService: PaperService, private router: Router, private comparer: ComparerService) {

  }

  ngOnInit(): void {
    this.paperService.getPapers().subscribe({
      next: r => {
        this.dataSource.set(r);
      }
    });
  }

  openPaper(id: number) {
    this.router.navigate(['/paper/' + id]);
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
        case 'paperName':
          return this.comparer.compare(a.paperName, b.paperName, isAsc);
        case 'comment':
          return this.comparer.compare(a.comment, b.comment, isAsc);
        case 'rating':
          return this.comparer.compare(a.rating, b.rating, isAsc);
        default:
          return 0;
      }
    }));
  }
}
