import { Navigate } from "react-router-dom";
import { useAuthStore } from "../../store/authStore";

export default function ProtectedRoute({
  children,
}: {
  children: React.ReactNode;
}) {
  const { isAuthenticated, onboardingCompleted, role } = useAuthStore();

  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (!onboardingCompleted && role !== "Admin")
    return <Navigate to="/onboarding" replace />;

  return <>{children}</>;
}
