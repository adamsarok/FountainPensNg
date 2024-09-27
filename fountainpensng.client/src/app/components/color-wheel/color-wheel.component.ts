import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FountainPen } from '../../../dtos/FountainPen';
import { ActivatedRoute } from '@angular/router';

interface RGB {
  r: number, g:number, b: number, err: boolean
}

@Component({
  selector: 'app-color-wheel',
  standalone: true,
  imports: [],
  templateUrl: './color-wheel.component.html',
  styleUrl: './color-wheel.component.css'
})
export class ColorWheelComponent implements OnInit {
  @ViewChild('colorCanvas', { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>;

  //@Input() 
  inkColors: { r: number, g: number, b: number }[] = [{r:128, g:0, b:128},{r:0, g:0, b:128},];
  pens: FountainPen[] = [];

  private canvas!: HTMLCanvasElement;
  private ctx!: CanvasRenderingContext2D;
  private wheelRadius = 150; 

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.canvas = this.canvasRef.nativeElement;
    this.ctx = this.canvas.getContext('2d')!;
    this.drawColorWheel();
    this.route.data.subscribe(data => {
      this.pens = data['pens'];
      console.log(this.pens);
      this.plotInkColors();
    });
  }

  //TODO: we don't actually want a color wheel but a component that can show lightness as well!
  drawColorWheel(): void {
    //TODO fix weird gradient
    const numSegments = 360;
    const step = (2 * Math.PI) / numSegments;
    for (let i = 0; i < numSegments; i++) {
      const angle = i * step;
      const hue = i;
      const [r, g, b] = this.hslToRgb(hue / 360, 1, 0.5);
      this.ctx.beginPath();
      this.ctx.moveTo(this.canvas.width / 2, this.canvas.height / 2);
      this.ctx.arc(this.canvas.width / 2, this.canvas.height / 2, this.wheelRadius, angle, angle + step);
      this.ctx.fillStyle = `rgb(${r},${g},${b})`;
      this.ctx.fill();
    }
  }

  hexToRgb(hex: string): RGB { //todo: colorservice
    if (!hex) return { r: 0, g: 0, b: 0, err: true };
    hex = hex.replace(/^#/, '');
    if (hex.length === 3) {
      hex = hex.split('').map(c => c + c).join('');
    }
    if (hex.length !== 6) {
      return { r: 0, g: 0, b: 0, err: true };
    }
    const r = parseInt(hex.substring(0, 2), 16);
    const g = parseInt(hex.substring(2, 4), 16);
    const b = parseInt(hex.substring(4, 6), 16);
    return { r, g, b, err: false };
  }

  plotInkColors(): void {
    //console.log(this.inkColors);
    //console.log(this.canvas);
    this.pens.forEach(pen => { //todo: only current ink, not all inkups
      if (pen.inkedUps && pen.inkedUps.length > 0) {
        const ink = pen.inkedUps[0];
        const rgb = this.hexToRgb(ink.inkColor);
        if (!rgb.err) {
          console.log(rgb);
          const [h, s, l] = this.rgbToHsl(rgb.r, rgb.g, rgb.b); //hue, saturation, lightness
          const angle = (h * Math.PI) / 180;
          const radius = this.wheelRadius * s;
          const x = this.canvas.width / 2 + radius * Math.cos(angle);
          const y = this.canvas.height / 2 + radius * Math.sin(angle);
          console.log(`x:${x} y:${y}`);
          this.ctx.beginPath();
          this.ctx.arc(x, y, 5, 0, 2 * Math.PI); // Plot a small circle for the ink color
          this.ctx.fillStyle = `rgb(${rgb.r},${rgb.g},${rgb.b})`;
          this.ctx.fill();
        }
      }
    });
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
    const max = Math.max(r, g, b), min = Math.min(r, g, b);
    let h: number, s: number, l: number = (max + min) / 2;

    if (max == min) {
      h = s = 0; // achromatic
    } else {
      const d = max - min;
      s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
      switch (max) {
        case r: h = (g - b) / d + (g < b ? 6 : 0); break;
        case g: h = (b - r) / d + 2; break;
        case b: h = (r - g) / d + 4; break;
        default: h = 0;
      }
      h *= 60;
    }
    return [h, s, l];
  }
}
