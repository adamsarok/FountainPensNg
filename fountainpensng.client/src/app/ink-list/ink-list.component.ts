import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkService } from '../services/ink.service';
import { InkForListDTO } from '../../dtos/InkForListDTO';

@Component({
  selector: 'app-ink-list',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './ink-list.component.html',
  styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  //todo!!!!!!!!!
  displayedColumns: string[] = ['maker', 'inkName', 'color', 'comment', 'rating', 'currentPen'];
  dataSource: InkForListDTO[] = [];

  constructor(private inkService: InkService) {

  }

  ngOnInit(): void {
    this.inkService.getInks().subscribe({
      next: r => {
        this.dataSource = r
      }
    });
  }
}
