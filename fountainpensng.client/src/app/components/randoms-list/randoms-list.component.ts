import { Component, OnInit, signal } from '@angular/core';
import { RandomsService } from '../../services/randoms.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpSuggestionDTO } from '../../../dtos/InkedUpSuggestionDTO';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-randoms-list',
  imports: [MatTableModule, CommonModule, MatButtonModule, MatFormFieldModule, MatInputModule, FormsModule],
  templateUrl: './randoms-list.component.html',
  styleUrl: './randoms-list.component.css'
})
export class RandomsListComponent implements OnInit {

  displayedColumns: string[] = ['pen', 'penColor', 'ink', 'inkColor'];
  dataSource = signal<InkedUpSuggestionDTO[]>([]);
  count: number = 5;

  constructor(private randomsService: RandomsService) { }

  ngOnInit(): void {
    this.loadRandoms();
  }

  loadRandoms(): void {
    this.randomsService.getRandoms(this.count).subscribe({
      next: r => {
        this.dataSource.set(r);
      }
    });
  }

  refreshRandoms(): void {
    this.loadRandoms();
  }
}
