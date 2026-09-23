import {
  useState
} from "react";

import {
  useNavigate,
  useParams
} from "react-router-dom";

import {
  checkout
} from "../services/paymentService";

import ErrorMessage from "../components/ErrorMessage";

function Checkout() {
  const { orderId } = useParams();

  const navigate = useNavigate();

  const [paymentMethod, setPaymentMethod] =
    useState("MockCard");

  const [loading, setLoading] =
    useState(false);

  const [error, setError] =
    useState("");

  const [success, setSuccess] =
    useState(false);

  async function handleCheckout(event) {
    event.preventDefault();

    try {
      setLoading(true);
      setError("");

      await checkout(
        orderId,
        paymentMethod
      );

      setSuccess(true);

      setTimeout(() => {
        navigate(
          `/orders/${orderId}`
        );
      }, 1000);

    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="form-page">

      <form
        className="form-card"
        onSubmit={handleCheckout}
      >

        <h1>Checkout</h1>

        <p>
          Order #{orderId}
        </p>

        <ErrorMessage
          message={error}
        />

        {success && (
          <p className="success">
            Payment successful. Order confirmed.
          </p>
        )}

        <label>
          Payment Method
        </label>

        <select
          value={paymentMethod}
          onChange={(event) =>
            setPaymentMethod(
              event.target.value
            )
          }
        >
          <option value="MockCard">
            Mock Card
          </option>
        </select>

        <button
          type="submit"
          disabled={loading || success}
        >
          {loading
            ? "Processing..."
            : "Pay Now"}
        </button>

      </form>

    </div>
  );
}

export default Checkout;