import { BrowserRouter, Routes, Route, Navigate, Link } from "react-router-dom";
import AddTransaction from "./pages/AddTransaction";
import Monitor from "./pages/Monitor";

function App() {
  return (
    <BrowserRouter>
      <nav style={{ marginBottom: "20px" }}>
        <Link to="/add">Add Transaction</Link>
        {" | "}
        <Link to="/monitor">Live Dashboard</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Navigate to="/add" replace />} />
        <Route path="/add" element={<AddTransaction />} />
        <Route path="/monitor" element={<Monitor />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;