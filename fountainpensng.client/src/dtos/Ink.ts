import { FountainPen } from "./FountainPen";
import { InkedUpForListDTO } from "./InkedUpDTO";

export interface Ink {
  id: number;
  maker: string;
  inkName: string;
  comment: string;
  photo: string;
  color: string;
  ml: number;
  rating: number;
  inkedUps: InkedUpForListDTO[];
  currentPens: FountainPen[] | null;
  penDisplayName: string | null;
  imageObjectKey: string;
  imageUrl: string;
  cieLAB_Sort: number;
}
