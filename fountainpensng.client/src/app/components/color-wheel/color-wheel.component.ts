import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InkForListDTO } from '../../../dtos/InkForListDTO';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

interface RGB {
  r: number;
  g: number;
  b: number;
  err: boolean;
}

@Component({
  selector: 'app-color-wheel',
  standalone: true,
  imports: [MatTooltipModule, CommonModule],
  templateUrl: './color-wheel.component.html',
  styleUrl: './color-wheel.component.css',
})
export class ColorWheelComponent implements OnInit {
  @ViewChild('colorCanvas', { static: true })
  canvasRef!: ElementRef<HTMLCanvasElement>;

  inks: InkForListDTO[] = [];
  markers: { ink: InkForListDTO, x: number, y: number}[] = [];

  private canvas!: HTMLCanvasElement;
  private ctx!: CanvasRenderingContext2D;
  private wheelRadius = 150;
  private markerRadius = 7; //todo: make size dynamic

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.canvas = this.canvasRef.nativeElement;
    this.ctx = this.canvas.getContext('2d')!;
    this.route.data.subscribe((data) => {
      this.inks = data['inks'];
      this.plotColorGrid();
    });
  }


  plotColorGrid(): void {
    //TODO: top white row not necessary
    const numColumns = 20;
    const numRows = 20;

    const squareSize = Math.min(
      this.canvas.width / numColumns,
      this.canvas.height / numRows
    );

    for (let i = 0; i < numColumns; i++) {
      for (let j = 0; j < numRows; j++) {
        const hue = (i / numColumns) * 360; // X-axis controls hue
        const lightness = 1 - j / numRows; // Y-axis controls lightness (inverted for dark to light)

        const [r, g, b] = this.hslToRgb(hue / 360, 1, lightness);

        const x = i * squareSize;
        const y = j * squareSize;

        this.ctx.fillStyle = `rgb(${r}, ${g}, ${b})`;
        this.ctx.fillRect(x, y, squareSize, squareSize);
      }
    }

    this.inks
      .filter(x => x.rating >= 7 && x.ml > 0)
      .forEach((ink) => {
      const rgb = this.hexToRgb(ink.color);
      if (!rgb.err) {
        const [h, s, l] = this.rgbToHsl(rgb.r, rgb.g, rgb.b); //hue, saturation, lightness
        const column = Math.floor((h / 360) * numColumns); // X-axis based on hue
        const row = Math.floor((1 - l) * numRows); // Y-axis based on lightness (inverted)

        const x = column * squareSize + squareSize / 2;
        const y = row * squareSize + squareSize / 2;
        this.markers.push({ink, x, y});
        this.ctx.beginPath();
        this.ctx.arc(x, y, this.markerRadius, 0, 2 * Math.PI);
        if (ink.oneCurrentPenMaker) {
          this.ctx.fillStyle = 'white';
        } else {
          this.ctx.fillStyle = 'black';
        }
        this.ctx.fill();

       
        this.ctx.font = '12px Arial';
        this.ctx.textAlign = 'center';
        this.ctx.textBaseline = 'middle';
    
        if (ink.oneCurrentPenMaker) {
          this.ctx.fillStyle = 'black';
          this.ctx.fillText('✔', x, y);
        } else {
          this.ctx.fillStyle = 'white';
          this.ctx.fillText('✖', x, y);
        }
      }
    });
  }

  // showTooltip(x: number, y: number, ink: InkForListDTO): void {
  //   const tooltip = document.getElementById('tooltip');
  //   if (!tooltip) return;
  //   tooltip.style.left = `${x + 10}px`;  // Slight offset from the mouse pointer
  //   tooltip.style.top = `${y + 10}px`;
  //   tooltip.style.display = 'block';
  //   tooltip.innerHTML = `Ink: ${ink.inkName} ${ink.oneCurrentPenMaker ? 'Pen:' + ink.oneCurrentPenMaker + ' - ' + ink.oneCurrentPenModelName : ''}`;
  // }
  
  hideTooltip(): void {
    const tooltip = document.getElementById('tooltip');
    if (!tooltip) return;
    tooltip.style.display = 'none';
  }

  hexToRgb(hex: string): RGB {
    //todo: colorservice
    if (!hex) return { r: 0, g: 0, b: 0, err: true };
    hex = hex.replace(/^#/, '');
    if (hex.length === 3) {
      hex = hex
        .split('')
        .map((c) => c + c)
        .join('');
    }
    if (hex.length !== 6) {
      return { r: 0, g: 0, b: 0, err: true };
    }
    const r = parseInt(hex.substring(0, 2), 16);
    const g = parseInt(hex.substring(2, 4), 16);
    const b = parseInt(hex.substring(4, 6), 16);
    return { r, g, b, err: false };
  }

  // Helper function to convert HSL to RGB
  hslToRgb(h: number, s: number, l: number): [number, number, number] {
    let r: number, g: number, b: number;

    if (s == 0) {
      r = g = b = l; // achromatic
    } else {
      const hue2rgb = (p: number, q: number, t: number) => {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1 / 6) return p + (q - p) * 6 * t;
        if (t < 1 / 3) return q;
        if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
        return p;
      };

      const q = l < 0.5 ? l * (1 + s) : l + s - l * s;
      const p = 2 * l - q;

      r = hue2rgb(p, q, h + 1 / 3);
      g = hue2rgb(p, q, h);
      b = hue2rgb(p, q, h - 1 / 3);
    }

    return [Math.round(r * 255), Math.round(g * 255), Math.round(b * 255)];
  }

  // Helper function to convert RGB to HSL
  rgbToHsl(r: number, g: number, b: number): [number, number, number] {
    r /= 255;
    g /= 255;
    b /= 255;
    const max = Math.max(r, g, b),
      min = Math.min(r, g, b);
    let h: number,
      s: number,
      l: number = (max + min) / 2;

    if (max == min) {
      h = s = 0; // achromatic
    } else {
      const d = max - min;
      s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
      switch (max) {
        case r:
          h = (g - b) / d + (g < b ? 6 : 0);
          break;
        case g:
          h = (b - r) / d + 2;
          break;
        case b:
          h = (r - g) / d + 4;
          break;
        default:
          h = 0;
      }
      h *= 60;
    }
    return [h, s, l];
  }
}
