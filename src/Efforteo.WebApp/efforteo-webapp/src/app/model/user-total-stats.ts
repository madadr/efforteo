export class UserTotalStats {
  public category: string;
  public amount: string;
  public time: number;
  public distance: number;
  public averageSpeed: number;
  public averagePace: number;
  public longestDistance: ActivityPointer;
  public longestTime: ActivityPointer;
}

export class ActivityPointer {
  public id: string;
  public time: number;
  public distance: number;
}
