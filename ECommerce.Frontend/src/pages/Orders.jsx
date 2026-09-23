import {
  useEffect,
  useState
} from "react";

import {
  Link
} from "react-router-dom";

import {
  getMyOrders
} from "../services/orderService";

import Loading from "../components/Loading";
import ErrorMessage from "../components/ErrorMessage";

function Orders() {
  const [orders, setOrders] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    loadOrders();
  }, []);

  async function loadOrders() {
    try {
      setLoading(true);
      setError("");

      const data = await getMyOrders();

      setOrders(
        Array.isArray(data)
          ? data
          : data?.items || []
      );
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  if (loading) {
    return (
      <Loading message="Loading orders..." />
    );
  }

  return (
    <div>

      <h1>My Orders</h1>

      <ErrorMessage
        message={error}
      />

      {orders.length === 0 ? (
        <p>
          You have no orders.
        </p>
      ) : (
        <div className="orders-list">

          {orders.map((order) => (
            <div
              className="order-card"
              key={order.id}
            >
              <h2>
                Order #{order.id}
              </h2>

              <p>
                Total: ₹
                {Number(
                  order.totalAmount
                ).toLocaleString()}
              </p>

              <p>
                Status:{" "}
                {order.orderStatus ||
                  order.status}
              </p>

              <Link
                className="button"
                to={`/orders/${order.id}`}
              >
                View Order
              </Link>
            </div>
          ))}

        </div>
      )}

    </div>
  );
}

export default Orders;