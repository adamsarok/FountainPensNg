import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { InkService } from '../services/ink.service';
import { Ink } from '../../dtos/Ink';

@Component({
  selector: 'app-ink-list',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './ink-list.component.html',
  styleUrl: './ink-list.component.css'
})
export class InkListComponent implements OnInit {
  //todo!!!!!!!!!
  displayedColumns: string[] = ['maker', 'modelName', 'color'];
  dataSource: Ink[] = [];

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
