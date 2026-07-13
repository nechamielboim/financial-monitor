import axios from "axios";
import type { CreateTransactionRequest } from "../types/transaction";
const api = axios.create({
  baseURL: "http://localhost:5177",
});

export const createTransaction = (transaction: CreateTransactionRequest) => {
  return api.post("/transactions", transaction);
};

export default api;