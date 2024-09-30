import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    RouterModule,
    ReactiveFormsModule,
    MatMenuTrigger,
    MatMenu,
    MatMenuItem,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'fountainpensng.client';

  constructor(private matIconRegistry: MatIconRegistry, private domSanitizer: DomSanitizer) {
    this.matIconRegistry.addSvgIcon(
      'angle-right',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/angle-right.svg')
    );
    this.matIconRegistry.addSvgIcon(
      'caret-up',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/caret-up.svg')
    );
    this.matIconRegistry.addSvgIcon(
      'ban',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/ban.svg')
    );
    this.matIconRegistry.addSvgIcon(
      'pen-nib',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/icons/pen-nib.svg')
    );
  }
}
