import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { PenService } from '../services/pen.service';
import { FountainPen } from '../../dtos/FountainPen';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pen-list',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './pen-list.component.html',
  styleUrl: './pen-list.component.css'
})
export class PenListComponent implements OnInit {
  displayedColumns: string[] = ['maker', 'modelName', 'color', 'nib', 'rating', 'currentInk', 'inkColor'];
  dataSource: FountainPen[] = [];

  ngOnInit(): void {
    this.penService.getPens().subscribe({
      next: r => {
        this.dataSource = r
      }
    });
  }
  constructor(private penService: PenService, private router: Router) {

  }
  openPen(id: number) {
    this.router.navigate(['/pen/' + id]);
  }    
}
