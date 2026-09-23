import {
  createContext,
  useState
} from "react";

import { parseJwt } from "../utils/jwt";

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(
    localStorage.getItem("token")
  );

  const user = token
    ? parseJwt(token)
    : null;

  function login(newToken) {
    localStorage.setItem(
      "token",
      newToken
    );

    setToken(newToken);
  }

  function logout() {
    localStorage.removeItem("token");
    setToken(null);
  }

  return (
    <AuthContext.Provider
      value={{
        token,
        user,
        isLoggedIn: !!token,
        login,
        logout
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}