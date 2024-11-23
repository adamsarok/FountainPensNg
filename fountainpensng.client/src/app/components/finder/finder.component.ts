import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { SearchResult } from '../../../dtos/SearchResult';
import { FinderService } from '../../services/finder.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-finder',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './finder.component.html',
  styleUrl: './finder.component.css'
})
export class FinderComponent implements OnInit {
  displayedColumns: string[] = ['searchResultType', 'maker', 'model', 'color', 'comment', 'rating'];
  dataSource: SearchResult[] = [];

  constructor(private finderService: FinderService, private router: Router) {

  }

  ngOnInit(): void {
    //todo: provide fulltext param
    this.finderService.getSearchResults("").subscribe({
      next: r => {
        this.dataSource = r
      }
    });
  }

  openResult(id: number, searhResultType: string) {
    //TODO!!!
    //this.router.navigate(['/ink/' + id]);
  }
}
