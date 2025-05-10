import { InkedUpForListDTO } from "./InkedUpDTO";

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
  currentInkId: number | null;
  currentInkRating: number | null;
  currentInkComment: string | null;
  imageObjectKey: string;
  imageUrl: string;
  cieLch_sort: number;
  lastInkedAt: Date;
}
