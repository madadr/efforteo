export class Alert {
  public type: string;
  public value: string;
  public link: string;

  constructor(type: string, value: string, link = '') {
    this.type = type;
    this.value = value;
    this.link = link;
  }
}
