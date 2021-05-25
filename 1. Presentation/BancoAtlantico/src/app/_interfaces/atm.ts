export interface ATM {
  id: number;
  name: string;
  actve: boolean;
  atmBankNotes: [{
    bankNote: string;
    count: number;
  }]
}
