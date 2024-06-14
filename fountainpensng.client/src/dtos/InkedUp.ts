import { FountainPen } from "./FountainPen";
import { Ink } from "./Ink";

export interface InkedUp {
  id: number;
  inkedAt: string;
  comment: string;
  matchRating: number;
  fountainPen: FountainPen;
  ink: Ink;
}
