import { useState } from "react";
import { Link } from "react-router-dom";
import { useRegister } from "../../hooks/useAuth";

export default function RegisterPage() {
  const [form, setForm] = useState({ username: "", email: "", password: "" });
  const registerMutation = useRegister();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    registerMutation.mutate(form);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-red-50 to-white flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-black text-red-600">
            Mëso<span className="text-teal-500">Shqip</span>
          </h1>
          <p className="text-gray-500 mt-2">Krijo llogarinë tënde falas</p>
        </div>

        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-2">
                Emri i përdoruesit
              </label>
              <input
                type="text"
                value={form.username}
                onChange={(e) => setForm({ ...form, username: e.target.value })}
                placeholder="amela123"
                required
                className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 focus:ring-2 focus:ring-red-100 transition-all"
              />
            </div>

            <div>
              <label className="block text-sm font-bold text-gray-700 mb-2">
                Email
              </label>
              <input
                type="email"
                value={form.email}
                onChange={(e) => setForm({ ...form, email: e.target.value })}
                placeholder="email@shembull.com"
                required
                className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 focus:ring-2 focus:ring-red-100 transition-all"
              />
            </div>

            <div>
              <label className="block text-sm font-bold text-gray-700 mb-2">
                Fjalëkalimi
              </label>
              <input
                type="password"
                value={form.password}
                onChange={(e) => setForm({ ...form, password: e.target.value })}
                placeholder="••••••••"
                required
                minLength={6}
                className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 focus:ring-2 focus:ring-red-100 transition-all"
              />
            </div>

            <button
              type="submit"
              disabled={registerMutation.isPending}
              className="w-full py-3 bg-red-600 hover:bg-red-700 text-white font-bold rounded-xl transition-all disabled:opacity-50"
            >
              {registerMutation.isPending
                ? "Duke u regjistruar..."
                : "Regjistrohu 🦅"}
            </button>
          </form>

          <p className="text-center text-sm text-gray-500 mt-6">
            Ke llogari?{" "}
            <Link
              to="/login"
              className="text-red-600 font-bold hover:underline"
            >
              Kyçu
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
