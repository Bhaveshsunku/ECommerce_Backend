import { apiRequest } from "./api";

export function getProducts(params = {}) {
  const searchParams =
    new URLSearchParams();

  if (params.search) {
    searchParams.set(
      "Search",
      params.search
    );
  }

  if (params.category) {
    searchParams.set(
      "Category",
      params.category
    );
  }

  if (params.minPrice !== undefined &&
      params.minPrice !== "") {
    searchParams.set(
      "MinPrice",
      params.minPrice
    );
  }

  if (params.maxPrice !== undefined &&
      params.maxPrice !== "") {
    searchParams.set(
      "MaxPrice",
      params.maxPrice
    );
  }

  searchParams.set(
    "PageNumber",
    params.pageNumber || 1
  );

  searchParams.set(
    "PageSize",
    params.pageSize || 10
  );

  const query =
    searchParams.toString();

  return apiRequest(
    `/api/Products?${query}`
  );
}

export function getProductById(id) {
  return apiRequest(
    `/api/Products/${id}`
  );
}

export function createProduct(data) {
  return apiRequest(
    "/api/Products",
    {
      method: "POST",
      body: JSON.stringify(data)
    }
  );
}

export function updateProduct(id, data) {
  return apiRequest(
    `/api/Products/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(data)
    }
  );
}

export function deleteProduct(id) {
  return apiRequest(
    `/api/Products/${id}`,
    {
      method: "DELETE"
    }
  );
}