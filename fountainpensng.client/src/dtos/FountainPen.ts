import { InkedUp } from "./InkedUp";
import { Ink } from "./Ink";

export interface FountainPen {
  id: number;
  maker: string;
  modelName: string;
  comment: string;
  photo: string;
  color: string;
  rating: number;
  nib: string;
  inkedUps: InkedUp[];
  currentInk: Ink | null;
  currentInkId: number;
  currentInkRating: number;
}
