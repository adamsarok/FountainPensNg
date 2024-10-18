import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { InkService } from '../../services/ink.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ink-list',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './ink-list.component.html',
  styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  //todo!!!!!!!!!
  displayedColumns: string[] = ['maker', 'inkName', 'color', 'comment', 'rating', 'currentPen'];
  dataSource: InkForListDTO[] = [];

  constructor(private inkService: InkService, private router: Router) {

  }

  ngOnInit(): void {
    this.inkService.getInks().subscribe({
      next: r => {
        this.dataSource = r
      }
    });
  }

  openInk(id: number) {
    this.router.navigate(['/ink/' + id]);
  }    
}
