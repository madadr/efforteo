export class ActivityDetailedStats {
  public id: string;
  public category: string;
  public time: number;
  public distance: number;
  public speed: number;
  public pace: number;
  public createdAt: string;
  public predecessor: Predecessor;
}

export class Predecessor {
  public id: string;
  public deltaSpeed: number;
  public deltaPace: number;
}
