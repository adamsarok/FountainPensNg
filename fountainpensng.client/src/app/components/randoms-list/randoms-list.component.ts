import { Component, OnInit, inject, signal } from '@angular/core';
import { RandomsService } from '../../services/randoms.service';
import { MatTableModule } from '@angular/material/table';
import { InkedUpSuggestionDTO } from '../../../dtos/InkedUpSuggestionDTO';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { InkedupService } from '../../services/inkedup.service';
import { InkedUpUploadDTO } from '../../../dtos/InkedUpDTO';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-randoms-list',
  imports: [MatTableModule, CommonModule, MatButtonModule, MatIconModule, MatSnackBarModule, MatTooltipModule],
  templateUrl: './randoms-list.component.html',
  styleUrl: './randoms-list.component.css'
})
export class RandomsListComponent implements OnInit {

  displayedColumns: string[] = ['strategy', 'pen', 'penNib', 'penColor', 'ink', 'inkLastInkedAt', 'inkColor', 'actions'];
  strategies = {
    'favorites': { order: 0, icon: 'favorite', label: 'Favorites - The pen and ink pairings you keep coming back to.' },
    'new-combinations': { order: 1, icon: 'auto_awesome', label: 'Discovery - Highly rated pens and inks in new combinations.' },
    'least-used': { order: 2, icon: 'history', label: 'Hidden ink - A forgotten ink with a favorite pen.' },
    'neglected-pens': { order: 3, icon: 'hourglass_empty', label: 'Hidden pen - A forgotten pen with a favorite ink' }
  };

  strategyDetails(strategy: InkedUpSuggestionDTO['strategy']) {
    return this.strategies[strategy];
  }
  dataSource = signal<InkedUpSuggestionDTO[]>([]);

  private randomsService = inject(RandomsService);
  private inkedupService = inject(InkedupService);
  private snackBar = inject(MatSnackBar);

  ngOnInit(): void {
    this.loadRandoms();
  }

  loadRandoms(): void {
    this.randomsService.getRandoms(8).subscribe({
      next: r => {
        this.dataSource.set([...r].sort((a, b) =>
          this.strategies[a.strategy].order - this.strategies[b.strategy].order));
      },
      error: () => this.snackBar.open('Could not load suggestions. Please try again.', 'Close', { duration: 5000 })
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
