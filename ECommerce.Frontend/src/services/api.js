const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL;

export async function apiRequest(
  endpoint,
  options = {}
) {
  const { skipAuth = false, ...requestOptions } = options;
  const token = localStorage.getItem("token");

  const headers = {
    ...(requestOptions.headers || {})
  };

  if (!(requestOptions.body instanceof FormData)) {
    headers["Content-Type"] = "application/json";
  }

  if (token && !skipAuth) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(
    `${API_BASE_URL}${endpoint}`,
    {
      ...requestOptions,
      headers
    }
  );

  if (!response.ok) {
    let message = `Request failed with status ${response.status}`;

    try {
      const contentType = response.headers.get("content-type") || "";
      const errorData = contentType.includes("application/json")
        ? await response.json()
        : await response.text();

      if (typeof errorData === "string" && errorData) {
        message = errorData;
      } else if (errorData.message) {
        message = errorData.message;
      }
    } catch {
      // Response did not contain a readable error body.
    }

    throw new Error(message);
  }

  if (response.status === 204) {
    return null;
  }

  const contentType =
    response.headers.get("content-type");

  if (
    contentType &&
    contentType.includes("application/json")
  ) {
    return response.json();
  }

  return null;
}