import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Products from "./pages/Products";
import ProductDetails from "./pages/ProductDetails";
import Orders from "./pages/Orders";
import OrderDetails from "./pages/OrderDetails";
import Checkout from "./pages/Checkout";
import VendorImport from "./pages/VendorImport";
import NotFound from "./pages/NotFound";

import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";
import RoleRoute from "./components/RoleRoute";

function App() {
  return (
    <BrowserRouter>

      {/* Navigation bar shown on all pages */}
      <Navbar />

      <main className="page-container">

        <Routes>

          {/* Public Routes */}
          <Route
            path="/"
            element={<Home />}
          />

          <Route
            path="/login"
            element={<Login />}
          />

          <Route
            path="/register"
            element={<Register />}
          />

          <Route
            path="/products"
            element={<Products />}
          />

          <Route
            path="/products/:id"
            element={<ProductDetails />}
          />

          {/* Protected Customer/User Routes */}
          <Route
            path="/orders"
            element={
              <ProtectedRoute>
                <Orders />
              </ProtectedRoute>
            }
          />

          <Route
            path="/orders/:id"
            element={
              <ProtectedRoute>
                <OrderDetails />
              </ProtectedRoute>
            }
          />

          <Route
            path="/checkout/:orderId"
            element={
              <ProtectedRoute>
                <Checkout />
              </ProtectedRoute>
            }
          />

          {/* Vendor-only Route */}
          <Route
            path="/vendor/import"
            element={
              <RoleRoute role="Vendor">
                <VendorImport />
              </RoleRoute>
            }
          />

          {/* Unknown URL */}
          <Route
            path="*"
            element={<NotFound />}
          />

        </Routes>

      </main>

    </BrowserRouter>
  );
}

export default App;