import { useState } from "react";
import { getProducts } from "../services/productService";
import { createInventory, getInventory, updateInventory } from "../services/inventoryService";
import { uploadProducts } from "../services/vendorService";

function VendorImport() {
  const [file, setFile] = useState(null);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const [manualForm, setManualForm] = useState({
    identifier: "",
    quantity: ""
  });
  const [manualLoading, setManualLoading] = useState(false);

  async function handleUpload() {
    if (!file) {
      setError("Please select an Excel file.");
      return;
    }

    try {
      setLoading(true);
      setError("");
      setSuccess("");
      setResult(null);

      const data = await uploadProducts(file);
      setResult(data);
      setSuccess("Excel import completed successfully.");
    } catch (err) {
      setError(err.message || "Bulk import failed.");
    } finally {
      setLoading(false);
    }
  }

  async function handleManualUpdate(event) {
    event.preventDefault();
    setError("");
    setSuccess("");
    setManualLoading(true);

    try {
      const identifier = manualForm.identifier.trim();
      const quantity = Number(manualForm.quantity);

      if (!identifier || Number.isNaN(quantity) || quantity < 0) {
        throw new Error("Please enter a valid product ID or name and a non-negative quantity.");
      }

      let productId = null;
      const normalized = identifier.trim();

      if (/^\d+$/.test(normalized)) {
        productId = Number(normalized);
      } else {
        const searchResult = await getProducts({
          search: normalized,
          pageNumber: 1,
          pageSize: 5
        });

        const candidates = Array.isArray(searchResult)
          ? searchResult
          : searchResult?.items || searchResult?.products || [];

        const match = candidates.find((item) =>
          item.name?.toLowerCase() === normalized.toLowerCase() ||
          item.name?.toLowerCase().includes(normalized.toLowerCase())
        );

        if (!match) {
          throw new Error("No matching product was found for that name.");
        }

        productId = match.id;
      }

      try {
        await getInventory(productId);
        await updateInventory(productId, { quantity });
      } catch (inventoryError) {
        if (!inventoryError.message.includes("404")) {
          throw inventoryError;
        }
        await createInventory({ productId, quantity });
      }

      setSuccess(`Inventory updated successfully for product ${productId}.`);
      setManualForm({ identifier: "", quantity: "" });
    } catch (err) {
      setError(err.message || "Inventory update failed.");
    } finally {
      setManualLoading(false);
    }
  }

  return (
    <div className="vendor-page">
      <div className="page-header">
        <div>
          <span className="eyebrow">Vendor tools</span>
          <h1>Vendor Inventory Dashboard</h1>
          <p>Upload Excel updates or update stock manually.</p>
        </div>
      </div>

      {error && <div className="error">{error}</div>}
      {success && <div className="success">{success}</div>}

      <div className="vendor-grid">
        <section className="panel">
          <h2>Bulk Excel Upload</h2>
          <p>Upload a spreadsheet with product rows and stock values.</p>

          <input
            type="file"
            accept=".xlsx,.xls,.csv"
            onChange={(event) => setFile(event.target.files?.[0] || null)}
          />

          <button onClick={handleUpload} disabled={loading || !file}>
            {loading ? "Uploading..." : "Upload Excel"}
          </button>

          {result && (
            <div className="result-card">
              <h3>Import Summary</h3>
              <p>Created: {result.created || 0}</p>
              <p>Updated: {result.updated || 0}</p>
              <p>Errors: {result.errors?.length || 0}</p>

              {result.errors?.length > 0 && (
                <div>
                  <h4>Issues</h4>
                  {result.errors.map((item, index) => (
                    <p key={index}>Row {item.rowNumber}: {item.error}</p>
                  ))}
                </div>
              )}
            </div>
          )}
        </section>

        <section className="panel">
          <h2>Manual Inventory Update</h2>
          <form onSubmit={handleManualUpdate} className="manual-form">
            <label>
              Product ID or Name
              <input
                type="text"
                value={manualForm.identifier}
                onChange={(event) =>
                  setManualForm((prev) => ({
                    ...prev,
                    identifier: event.target.value
                  }))
                }
                placeholder="Example: 12 or Laptop"
                required
              />
            </label>

            <label>
              Quantity
              <input
                type="number"
                min="0"
                value={manualForm.quantity}
                onChange={(event) =>
                  setManualForm((prev) => ({
                    ...prev,
                    quantity: event.target.value
                  }))
                }
                placeholder="Enter stock quantity"
                required
              />
            </label>

            <button type="submit" disabled={manualLoading}>
              {manualLoading ? "Updating..." : "Update Inventory"}
            </button>
          </form>
        </section>
      </div>
    </div>
  );
}

export default VendorImport;