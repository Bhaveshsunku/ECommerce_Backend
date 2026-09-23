import { Link } from "react-router-dom";

function ProductCard({ product }) {
  return (
    <article className="product-card">
      <span className="eyebrow">{product.category || "Product"}</span>
      <h2>{product.name}</h2>
      <p>{product.description || "Quality product available now."}</p>
      <strong>${Number(product.price || 0).toFixed(2)}</strong>
      <div>
        <Link className="button" to={`/products/${product.id}`}>View Product</Link>
      </div>
    </article>
  );
}

export default ProductCard;
