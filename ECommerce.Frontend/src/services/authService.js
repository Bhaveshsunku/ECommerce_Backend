import { apiRequest } from "./api";

export function registerUser(data) {
  return apiRequest(
    "/api/Auth/register",
    {
      method: "POST",
      body: JSON.stringify(data),
      skipAuth: true
    }
  );
}

export function loginUser(data) {
  return apiRequest(
    "/api/Auth/login",
    {
      method: "POST",
      body: JSON.stringify(data),
      skipAuth: true
    }
  );
}