export interface InkedUpSuggestionDTO {
  fountainPenId: number;
  penMaker: string;
  penName: string;
  inkId: number;
  inkMaker: string;
  inkName: string;
  penColor: string;
  inkColor: string;
  penNib: string;
  inkLastInkedAt: Date | null;
  strategy: 'favorites' | 'least-used' | 'new-combinations' | 'neglected-pens';
}
