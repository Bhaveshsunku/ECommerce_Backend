import { apiRequest } from "./api";

export function checkout(
  orderId,
  paymentMethod
) {
  return apiRequest(
    `/api/Payments/checkout/${orderId}`,
    {
      method: "POST",
      body: JSON.stringify({
        paymentMethod
      })
    }
  );
}