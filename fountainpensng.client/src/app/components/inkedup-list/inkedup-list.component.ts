import { Component, OnInit } from '@angular/core';
import { InkedupService } from '../../services/inkedup.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpForListDTO } from '../../../dtos/InkedUpDTO';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inkedup-list',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './inkedup-list.component.html',
  styleUrl: './inkedup-list.component.css'
})
export class InkedupListComponent implements OnInit {

  displayedColumns: string[] = ['inkedAt', 'matchRating', 'ink', 'pen'];
  dataSource: InkedUpForListDTO[] = [];

  constructor(private inkedUpService: InkedupService, private router: Router) {

  }

  ngOnInit(): void {
    this.inkedUpService.getInkedUps().subscribe({
      next: r => {
        this.dataSource = r;
      }
    });
  }
  openInkedUp(id: number) {
    this.router.navigate(['/inked-up/' + id]);
  }
}
