export function parseJwt(token) {
  try {
    const payload = token.split(".")[1];

    const decodedPayload =
      atob(
        payload
          .replace(/-/g, "+")
          .replace(/_/g, "/")
      );

    return JSON.parse(decodedPayload);
  } catch {
    return null;
  }
}