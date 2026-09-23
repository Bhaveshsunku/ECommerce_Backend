import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    return {
      token: null,
      user: null,
      isLoggedIn: false,
      login: () => {},
      logout: () => {}
    };
  }

  return context;
}