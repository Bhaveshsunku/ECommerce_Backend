import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

function Navbar() {
  const { isLoggedIn, user, logout } = useAuth();
  const navigate = useNavigate();
  const role = user?.role || user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <nav className="navbar">
      <div className="nav-content">
        <Link className="brand" to="/">E-Commerce</Link>
        <div className="nav-links">
          <Link to="/products">Products</Link>
          {isLoggedIn && <Link to="/orders">My Orders</Link>}
          {isLoggedIn && role === "Vendor" && <Link to="/vendor/import">Vendor Tools</Link>}
          {!isLoggedIn ? (
            <>
              <Link to="/login">Login</Link>
              <Link to="/register">Register</Link>
            </>
          ) : (
            <>
              <span>{user?.email}</span>
              <button className="nav-button" onClick={handleLogout}>Logout</button>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
