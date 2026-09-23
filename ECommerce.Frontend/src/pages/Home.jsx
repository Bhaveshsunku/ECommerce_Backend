import { Link } from "react-router-dom";

function Home() {
  return (
    <div className="hero">

      <h1>
        Welcome to E-Commerce
      </h1>

      <p>
        Browse products, place orders
        and complete checkout.
      </p>

      <Link
        className="button"
        to="/products"
      >
        Browse Products
      </Link>

    </div>
  );
}

export default Home;