export interface CreateTransactionRequest {
  description: string;
  amount: number;
}

export interface Transaction {
  transactionId: string;
  description: string;
  amount: number;
  currency: string;
  status: number;
  timestamp: string;
}