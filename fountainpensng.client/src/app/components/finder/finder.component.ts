import { Component, NgZone, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { SearchResult } from '../../../dtos/SearchResult';
import { FinderService } from '../../services/finder.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'app-finder',
    imports: [MatTableModule,
        CommonModule,
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatInputModule,
        MatButtonModule,],
    templateUrl: './finder.component.html',
    styleUrl: './finder.component.css'
})
export class FinderComponent implements OnInit {
  displayedColumns: string[] = ['searchResultType', 'maker', 'model', 'color', 'comment', 'rating'];
  dataSource: SearchResult[] = [];
  form: FormGroup = new FormGroup({});
  constructor(private finderService: FinderService,
    private router: Router,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private zone: NgZone) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      fulltext: ['', Validators.nullValidator],
    });
  }

  onSubmit() {
    const fulltext = this.form.get('fulltext')?.value;
    this.finderService.getSearchResults(fulltext).subscribe({
      next: r => {
        this.showSnack('Search successful');
        console.log(r)
        this.dataSource = r;
      },
      error: (err) => {
        this.showSnack('Search failed:' + err);
      },
    });
  }

  showSnack(msg: string): void {
    this.zone.run(() => {
      this.snackBar.open(msg, 'Close', { duration: 3000 });
    });
  }

  openResult(id: number, searchResultType: string) {
    //TODO!!!
    //this.router.navigate(['/ink/' + id]);
  }
}
