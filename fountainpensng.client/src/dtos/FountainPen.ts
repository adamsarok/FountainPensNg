import { Ink } from "./Ink";
import { InkedUpForListDTO } from "./InkedUpForListDTO";

export interface FountainPen {
  id: number;
  maker: string;
  modelName: string;
  comment: string;
  photo: string;
  color: string;
  rating: number;
  nib: string;
  inkedUps: InkedUpForListDTO[];
  currentInk: Ink | null;
  currentInkId: number | null;
  currentInkRating: number | null;
}
