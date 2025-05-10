export interface InkForListDTO {
  id: number;
  maker: string;
  inkName: string;
  comment: string;
  photo: string;
  color: string;
  rating: number;
  ml: number;
  oneCurrentPenMaker: string;
  oneCurrentPenModelName: string;
  oneCurrentPenModelColor: string;
  cieLch_sort: number;
  lastInkedAt: Date;
}
