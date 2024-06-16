import { InkedUp } from "./InkedUp";
import { FountainPen } from "./FountainPen";

export interface Ink {
  id: number;
  maker: string;
  inkName: string;
  comment: string;
  photo: string;
  color: string;
  // color_CIELAB_L: number | null;
  // color_CIELAB_a: number | null;
  // color_CIELAB_b: number | null;
  rating: number;
  inkedUps: InkedUp[] | null;
  currentPens: FountainPen[] | null;
  penDisplayName: string | null;
}
