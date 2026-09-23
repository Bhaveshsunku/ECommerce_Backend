import { useEffect, useRef, useState } from "react";

import {
  getProducts
} from "../services/productService";

import ProductCard from "../components/ProductCard";
import Loading from "../components/Loading";
import ErrorMessage from "../components/ErrorMessage";

function Products() {
  const [products, setProducts] = useState([]);

  const [search, setSearch] = useState("");

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState("");
  const requestId = useRef(0);

  useEffect(() => {
    loadProducts();
  }, []);

  async function loadProducts() {
    const currentRequestId = ++requestId.current;
    setLoading(true);
    setError("");

    try {
      const data = await getProducts({
        search,
        pageNumber: 1,
        pageSize: 20
      });

      const productList =
        Array.isArray(data)
          ? data
          : data?.items ||
            data?.products ||
            [];

      if (currentRequestId === requestId.current) {
        setProducts(productList);
      }
    } catch (error) {
      if (currentRequestId === requestId.current) {
        setError(error.message || "Unable to load products.");
      }
    } finally {
      if (currentRequestId === requestId.current) {
        setLoading(false);
      }
    }
  }

  function handleSearch(event) {
    event.preventDefault();
    loadProducts();
  }

  return (
    <div>

      <div className="page-header">
        <div>
          <h1>Products</h1>
          <p>
            Browse available products.
          </p>
        </div>
      </div>

      <form
        className="search-form"
        onSubmit={handleSearch}
      >
        <input
          value={search}
          onChange={(event) =>
            setSearch(event.target.value)
          }
          placeholder="Search products..."
        />

        <button type="submit">
          Search
        </button>
      </form>

      <ErrorMessage message={error} />

      {loading ? (
        <Loading message="Loading products..." />
      ) : (
        <div className="product-grid">

          {products.length === 0 ? (
            <p>
              No products found.
            </p>
          ) : (
            products.map((product) => (
              <ProductCard
                key={product.id}
                product={product}
              />
            ))
          )}

        </div>
      )}

    </div>
  );
}

export default Products;