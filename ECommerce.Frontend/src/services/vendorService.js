import { apiRequest } from "./api";

export function uploadProducts(file) {
  const formData = new FormData();

  formData.append(
    "file",
    file
  );

  return apiRequest(
    "/api/vendor/products/bulk",
    {
      method: "POST",
      body: formData
    }
  );
}