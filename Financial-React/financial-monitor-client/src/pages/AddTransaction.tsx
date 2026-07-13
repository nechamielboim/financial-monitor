import { useState } from "react";
import { createTransaction } from "../services/api";

function AddTransaction() {
  const [description, setDescription] = useState("");
  const [amount, setAmount] = useState(0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await createTransaction({
        description,
        amount,
      });

      alert("Transaction created!");

      setDescription("");
      setAmount(0);
    } catch (error) {
      console.error(error);
      alert("Failed to create transaction");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h1>Transaction Simulator</h1>

      <form onSubmit={handleSubmit}>
        <div>
          <label>Description</label>
          <br />
          <input
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <br />

        <div>
          <label>Amount</label>
          <br />
          <input
            type="number"
            value={amount}
            onChange={(e) => setAmount(Number(e.target.value))}
          />
        </div>

        <br />

        <button type="submit">
          Send Transaction
        </button>
      </form>
    </div>
  );
}

export default AddTransaction;