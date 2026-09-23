import { apiRequest } from "./api";

export function getInventory(productId) {
  return apiRequest(
    `/api/Inventory/product/${productId}`
  );
}

export function createInventory(data) {
  return apiRequest(
    "/api/Inventory",
    {
      method: "POST",
      body: JSON.stringify(data)
    }
  );
}

export function updateInventory(
  productId,
  data
) {
  return apiRequest(
    `/api/Inventory/product/${productId}`,
    {
      method: "PUT",
      body: JSON.stringify(data)
    }
  );
}