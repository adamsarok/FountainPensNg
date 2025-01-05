import { Component, OnInit } from '@angular/core';
import { InkedupService } from '../../services/inkedup.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpForListDTO } from '../../../dtos/InkedUpDTO';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatSortModule, Sort } from '@angular/material/sort';
import { ComparerService } from '../../services/comparer.service';

@Component({
  selector: 'app-inkedup-list',
  imports: [MatTableModule, MatSortModule, CommonModule],
  templateUrl: './inkedup-list.component.html',
  styleUrl: './inkedup-list.component.css'
})
export class InkedupListComponent implements OnInit {

  displayedColumns: string[] = ['inkedAt', 'matchRating', 'ink', 'pen'];
  dataSource: InkedUpForListDTO[] = [];
  sortedData: InkedUpForListDTO[] = [];

  constructor(private inkedUpService: InkedupService, private router: Router,
    private comparer: ComparerService) {

  }

  ngOnInit(): void {
    this.inkedUpService.getInkedUps().subscribe({
      next: r => {
        this.dataSource = r;
        this.sortedData = this.dataSource.slice();
      }
    });
  }
  openInkedUp(id: number) {
    this.router.navigate(['/inked-up/' + id]);
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
        case 'inkedAt':
          return this.comparer.compare(a.inkedAt, b.inkedAt, isAsc);
        case 'matchRating':
          return this.comparer.compare(a.matchRating, b.matchRating, isAsc);
        case 'ink':
          return this.comparer.compare(a.inkMaker, b.inkMaker, isAsc);
        case 'pen':
          return this.comparer.compare(a.penMaker, b.penMaker, isAsc);
        default:
          return 0;
      }
    });
  }
}
