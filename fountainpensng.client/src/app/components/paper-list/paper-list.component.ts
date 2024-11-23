import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { Paper } from '../../../dtos/Paper';
import { PaperService } from '../../services/paper.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-paper-list',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './paper-list.component.html',
  styleUrl: './paper-list.component.css'
})
export class PaperListComponent implements OnInit {
  displayedColumns: string[] = ['maker', 'paperName', 'comment', 'rating'];
  dataSource: Paper[] = [];

  constructor(private paperService: PaperService, private router: Router) {

  }

  ngOnInit(): void {
    this.paperService.getPapers().subscribe({
      next: r => {
        this.dataSource = r
      }
    });
  }

  openPaper(id: number) {
    this.router.navigate(['/paper/' + id]);
  }
}
