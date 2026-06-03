import { Navigate } from "react-router-dom";
import { useAuthStore } from "../../store/authStore";
import { jwtDecode } from "jwt-decode";

interface JwtPayload {
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
}

export default function AdminRoute({
  children,
}: {
  children: React.ReactNode;
}) {
  const { isAuthenticated, accessToken } = useAuthStore();

  if (!isAuthenticated || !accessToken) return <Navigate to="/login" replace />;

  try {
    const decoded = jwtDecode<JwtPayload>(accessToken);
    const role =
      decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    if (role !== "Admin") return <Navigate to="/dashboard" replace />;
  } catch {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
