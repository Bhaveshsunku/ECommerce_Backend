import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { registerUser } from "../services/authService";

function Register() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    role: "Customer"
  });

  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  function handleChange(event) {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    setError("");
    setMessage("");
    setLoading(true);

    try {
      await registerUser(form);

      setMessage("Registration successful. You can now login.");

      setTimeout(() => {
        navigate("/login");
      }, 800);
    } catch (err) {
      setError(err.message || "Registration failed.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <div className="auth-header">
          <span className="eyebrow">Create account</span>
          <h1>Register</h1>
        </div>

        {error && <p className="error">{error}</p>}
        {message && <p className="success">{message}</p>}

        <div className="field-row two-cols">
          <label>
            First name
            <input
              name="firstName"
              placeholder="First Name"
              value={form.firstName}
              onChange={handleChange}
              required
            />
          </label>

          <label>
            Last name
            <input
              name="lastName"
              placeholder="Last Name"
              value={form.lastName}
              onChange={handleChange}
              required
            />
          </label>
        </div>

        <label>
          Email
          <input
            type="email"
            name="email"
            placeholder="you@example.com"
            value={form.email}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Password
          <input
            type="password"
            name="password"
            placeholder="Minimum 6 characters"
            value={form.password}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Role
          <select name="role" value={form.role} onChange={handleChange}>
            <option value="Customer">Customer</option>
            <option value="Vendor">Vendor</option>
          </select>
        </label>

        <button type="submit" disabled={loading}>
          {loading ? "Creating account..." : "Create Account"}
        </button>

        <p className="auth-link-row">
          Already registered? <a href="/login">Login</a>
        </p>
      </form>
    </div>
  );
}

export default Register;