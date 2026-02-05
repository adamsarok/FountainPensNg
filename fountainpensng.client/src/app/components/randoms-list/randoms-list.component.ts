import { Component, OnInit, signal } from '@angular/core';
import { RandomsService } from '../../services/randoms.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpSuggestionDTO } from '../../../dtos/InkedUpSuggestionDTO';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { InkedupService } from '../../services/inkedup.service';
import { InkedUpUploadDTO } from '../../../dtos/InkedUpDTO';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-randoms-list',
  imports: [MatTableModule, CommonModule, MatButtonModule, MatFormFieldModule, MatInputModule, FormsModule, MatIconModule, MatSnackBarModule, MatTooltipModule],
  templateUrl: './randoms-list.component.html',
  styleUrl: './randoms-list.component.css'
})
export class RandomsListComponent implements OnInit {

  displayedColumns: string[] = ['pen', 'penNib', 'penColor', 'ink', 'inkLastInkedAt', 'inkColor', 'actions'];
  dataSource = signal<InkedUpSuggestionDTO[]>([]);
  count: number = 10;

  constructor(
    private randomsService: RandomsService,
    private inkedupService: InkedupService,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

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

  createInkedUp(suggestion: InkedUpSuggestionDTO): void {
    const newInkedUp: InkedUpUploadDTO = {
      id: 0,
      inkedAt: new Date(),
      matchRating: 0,
      fountainPenId: suggestion.fountainPenId,
      inkId: suggestion.inkId,
      isCurrent: true,
      comment: ''
    };

    this.inkedupService.createInkedUp(newInkedUp).subscribe({
      next: () => {
        this.snackBar.open(`Created Inked-Up: ${suggestion.penMaker} ${suggestion.penName} with ${suggestion.inkMaker} ${suggestion.inkName}`, 'Close', {
          duration: 3000
        });
        this.loadRandoms(); // Refresh the list to get new suggestions
      },
      error: (err) => {
        this.snackBar.open('Error creating Inked-Up: ' + err.message, 'Close', {
          duration: 5000
        });
      }
    });
  }
}
