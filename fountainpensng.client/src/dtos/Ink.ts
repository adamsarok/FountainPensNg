import { InkedUp } from "./InkedUp";
import { FountainPen } from "./FountainPen";

export interface Ink {
  id: number;
  maker: string;
  inkName: string;
  comment: string;
  photo: string;
  color: string;
  color_CIELAB_L: number | null;
  color_CIELAB_a: number | null;
  color_CIELAB_b: number | null;
  rating: number;
  inkedUps: InkedUp[];
  currentPens: FountainPen[];
  penDisplayName: string | null;
}
