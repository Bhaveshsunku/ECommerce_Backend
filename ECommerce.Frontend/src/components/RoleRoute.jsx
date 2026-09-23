import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

function RoleRoute({ role, children }) {
  const { isLoggedIn, user } = useAuth();
  const currentRole = user?.role || user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  if (!isLoggedIn) {
    return <Navigate to="/login" replace />;
  }

  return currentRole === role ? children : <Navigate to="/products" replace />;
}

export default RoleRoute;
