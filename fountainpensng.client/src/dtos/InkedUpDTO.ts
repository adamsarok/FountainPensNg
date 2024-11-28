export interface InkedUpUploadDTO {
  id: number;
  inkedAt: Date;
  matchRating: number;
  fountainPenId: number;
  inkId: number;
  isCurrent: boolean;
}

export interface InkedUpForListDTO extends InkedUpUploadDTO {
  penMaker: string;
  penName: string;
  penColor: string;
  inkMaker: string;
  inkName: string;
  inkColor: string;
}