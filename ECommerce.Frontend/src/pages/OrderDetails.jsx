import {
  useEffect,
  useState
} from "react";

import {
  Link,
  useNavigate,
  useParams
} from "react-router-dom";

import {
  getOrderById,
  cancelOrder
} from "../services/orderService";

import Loading from "../components/Loading";
import ErrorMessage from "../components/ErrorMessage";

function OrderDetails() {
  const { id } = useParams();

  const navigate = useNavigate();

  const [order, setOrder] = useState(null);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [cancelling, setCancelling] =
    useState(false);

  useEffect(() => {
    loadOrder();
  }, [id]);

  async function loadOrder() {
    try {
      setLoading(true);
      setError("");

      const data =
        await getOrderById(id);

      setOrder(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  async function handleCancel() {
    try {
      setCancelling(true);
      setError("");

      await cancelOrder(id);

      await loadOrder();
    } catch (error) {
      setError(error.message);
    } finally {
      setCancelling(false);
    }
  }

  if (loading) {
    return (
      <Loading message="Loading order..." />
    );
  }

  if (!order) {
    return (
      <ErrorMessage
        message={
          error || "Order not found."
        }
      />
    );
  }

  const status =
    order.orderStatus ||
    order.status;

  return (
    <div className="details-card">

      <h1>
        Order #{order.id}
      </h1>

      <p>
        Status: <strong>{status}</strong>
      </p>

      <p>
        Total: ₹
        {Number(
          order.totalAmount
        ).toLocaleString()}
      </p>

      <h2>Items</h2>

      <div className="order-items">

        {order.items?.map(
          (item, index) => (
            <div
              className="order-item"
              key={index}
            >
              <p>
                {item.productName}
              </p>

              <p>
                Quantity: {item.quantity}
              </p>

              <p>
                Unit Price: ₹
                {Number(
                  item.unitPrice
                ).toLocaleString()}
              </p>

              <p>
                Subtotal: ₹
                {Number(
                  item.subTotal
                ).toLocaleString()}
              </p>
            </div>
          )
        )}

      </div>

      {status === "Pending" && (
        <button
          onClick={handleCancel}
          disabled={cancelling}
        >
          {cancelling
            ? "Cancelling..."
            : "Cancel Order"}
        </button>
      )}

      {status === "Pending" && (
        <Link
          className="button secondary"
          to={`/checkout/${order.id}`}
        >
          Continue to Checkout
        </Link>
      )}

    </div>
  );
}

export default OrderDetails;