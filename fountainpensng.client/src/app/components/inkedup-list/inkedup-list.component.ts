import { Component, OnInit } from '@angular/core';
import { InkedupService } from '../../services/inkedup.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpForListDTO } from '../../../dtos/InkedUpForListDTO';
import { CommonModule } from '@angular/common';

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

  constructor(private inkedUpService: InkedupService) {

  }

  ngOnInit(): void {
    this.inkedUpService.getInkedUps().subscribe({
      next: r => {
        console.log(r);
        this.dataSource = r;
      }
    });
  }
}
