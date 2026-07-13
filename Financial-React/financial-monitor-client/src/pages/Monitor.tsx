import { useEffect, useState } from "react";
import connection from "../services/signalr";
import type { Transaction } from "../types/transaction";
import "./Montior.css";

function Monitor() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [showOnlyFailed, setShowOnlyFailed] = useState(false);

  const [newTransactionId, setNewTransactionId] = useState<string | null>(null);


  useEffect(() => {

    const loadTransactions = async () => {
      try {
        const response = await fetch(
          "http://localhost:5177/transactions"
        );

        if (!response.ok) {
          throw new Error("Failed to load transactions");
        }

        const data: Transaction[] = await response.json();

        setTransactions(data.slice(-100));

        console.log("Loaded transactions:", data);

      } catch (error) {
        console.error("Loading transactions error:", error);
      }
    };


    const handleTransaction = (transaction: Transaction) => {

      console.log(
        "Received transaction:",
        JSON.stringify(transaction, null, 2)
      );


      // מסמן את העסקה החדשה לאנימציה
      setNewTransactionId(transaction.transactionId);


      setTransactions((prev) => [
        ...prev,
        transaction
      ].slice(-100));


      // מסיר את האנימציה אחרי חצי שניה
      setTimeout(() => {
        setNewTransactionId(null);
      }, 600);
    };


    console.log("Registering TransactionCreated listener");


    connection.on(
      "TransactionCreated",
      handleTransaction
    );


    const startConnection = async () => {
      try {

        await loadTransactions();


        if (connection.state === "Disconnected") {

          await connection.start();

          console.log("SignalR connected");

        }

      } catch (error) {

        console.error(
          "SignalR connection error:",
          error
        );

      }
    };


    startConnection();


    return () => {

      connection.off(
        "TransactionCreated",
        handleTransaction
      );


      if (connection.state !== "Disconnected") {
        connection.stop();
      }

    };

  }, []);



  const displayedTransactions = showOnlyFailed
    ? transactions.filter(
        (transaction) => transaction.status === 2
      )
    : transactions;



  return (
    <div className="monitor">

      <h1>Live Dashboard</h1>


      <label>

        <input
          type="checkbox"
          checked={showOnlyFailed}
          onChange={(e) =>
            setShowOnlyFailed(e.target.checked)
          }
        />

        Show only Failed

      </label>



      <div className="transactions-grid">


        {displayedTransactions.map((transaction) => (

          <div
            key={transaction.transactionId}
            className={`transaction-card ${
              newTransactionId === transaction.transactionId
                ? "new-transaction"
                : ""
            }`}
          >


            <h3>
              {transaction.description}
            </h3>


            <p>
              Amount: {transaction.amount} {transaction.currency}
            </p>


            <p>
              Date:{" "}
              {new Date(
                transaction.timestamp
              ).toLocaleString()}
            </p>


            <span
              className={`status status-${transaction.status}`}
            >

              {transaction.status === 0 && "🟠 Pending"}

              {transaction.status === 1 && "🟢 Completed"}

              {transaction.status === 2 && "🔴 Failed"}

            </span>


          </div>

        ))}


      </div>


    </div>
  );
}

export default Monitor;