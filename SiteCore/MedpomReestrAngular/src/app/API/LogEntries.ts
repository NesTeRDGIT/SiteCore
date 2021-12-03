 export class LogEntries
{
  public TimeGenerated:Date = null;
  public Message:string = "";
  public Type:TypeEntries = TypeEntries.message;
  constructor(obj:any){
      if(obj!=null){
            if(obj.TimeGenerated!=null)
                this.TimeGenerated = new Date(obj.TimeGenerated);
            this.Message = obj.Message;
            this.Type = obj.Type;
      }
  }
}

export enum TypeEntries
  {
    message = 0,
    error = 1,
    warning = 2
  }