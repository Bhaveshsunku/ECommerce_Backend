import {
  useEffect,
  useState
} from "react";

import {
  useNavigate,
  useParams
} from "react-router-dom";

import {
  getProductById
} from "../services/productService";

import {
  getInventory
} from "../services/inventoryService";

import {
  createOrder
} from "../services/orderService";

import { useAuth } from "../hooks/useAuth";

import Loading from "../components/Loading";
import ErrorMessage from "../components/ErrorMessage";

function ProductDetails() {
  const { id } = useParams();

  const navigate = useNavigate();

  const { isLoggedIn } = useAuth();

  const [product, setProduct] = useState(null);
  const [inventory, setInventory] = useState(null);

  const [quantity, setQuantity] = useState(1);

  const [loading, setLoading] = useState(true);
  const [ordering, setOrdering] = useState(false);

  const [error, setError] = useState("");

  useEffect(() => {
    loadProduct();
  }, [id]);

  async function loadProduct() {
    try {
      setLoading(true);
      setError("");

      const [
        productData,
        inventoryData
      ] = await Promise.all([
        getProductById(id),
        getInventory(id)
      ]);

      setProduct(productData);
      setInventory(inventoryData);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  async function handleBuy() {
    if (!isLoggedIn) {
      navigate("/login");
      return;
    }

    if (
      !inventory ||
      quantity > inventory.quantity
    ) {
      setError("Insufficient stock.");
      return;
    }

    try {
      setOrdering(true);
      setError("");

      const order = await createOrder({
        items: [
          {
            productId: Number(id),
            quantity: Number(quantity)
          }
        ]
      });

      navigate(
        `/checkout/${order.id}`
      );
    } catch (error) {
      setError(error.message);
    } finally {
      setOrdering(false);
    }
  }

  if (loading) {
    return (
      <Loading message="Loading product..." />
    );
  }

  if (error && !product) {
    return (
      <ErrorMessage message={error} />
    );
  }

  if (!product) {
    return <p>Product not found.</p>;
  }

  return (
    <div className="details-card">

      <h1>{product.name}</h1>

      <p>
        {product.description}
      </p>

      <h2>
        ₹{Number(product.price).toLocaleString()}
      </h2>

      <p>
        Category: {product.category}
      </p>

      <p>
        Available Stock:{" "}
        {inventory?.quantity ?? 0}
      </p>

      {error && (
        <ErrorMessage message={error} />
      )}

      <label>
        Quantity
      </label>

      <input
        className="quantity-input"
        type="number"
        min="1"
        max={inventory?.quantity || 1}
        value={quantity}
        onChange={(event) =>
          setQuantity(event.target.value)
        }
      />

      <br />

      <button
        onClick={handleBuy}
        disabled={
          ordering ||
          !inventory ||
          inventory.quantity === 0
        }
      >
        {ordering
          ? "Creating Order..."
          : "Buy Now"}
      </button>

    </div>
  );
}

export default ProductDetails;