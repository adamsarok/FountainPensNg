import { InkedUp } from "./InkedUp";
import { FountainPen } from "./FountainPen";

export interface Ink {
  id: number;
  maker: string;
  inkName: string;
  comment: string;
  photo: string;
  color: string;
  ml: number;
  rating: number;
  inkedUps: InkedUp[] | null;
  currentPens: FountainPen[] | null;
  penDisplayName: string | null;
  imageObjectKey: string;
  imageUrl: string;
}
