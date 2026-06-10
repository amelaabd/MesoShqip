import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { login, register, logout, completeOnboarding } from "../api/auth";
import { useAuthStore } from "../store/authStore";
import toast from "react-hot-toast";

export function useLogin() {
  const setAuth = useAuthStore((s) => s.setAuth);
  const navigate = useNavigate();

  return useMutation({
    mutationFn: login,
    onSuccess: (data) => {
      setAuth(data.accessToken, data.username, data.role);
      toast.success("Mirë se erdhe!");
      if (data.role === "Admin") navigate("/admin");
      else navigate("/dashboard");
    },
    onError: () => toast.error("Email ose fjalëkalim i gabuar."),
  });
}

export function useRegister() {
  const navigate = useNavigate();

  return useMutation({
    mutationFn: register,
    onSuccess: () => {
      toast.success("Regjistrimi u krye! Kyçu tani.");
      navigate("/login");
    },
    onError: () => toast.error("Regjistrimi dështoi. Provo sërish."),
  });
}

export function useCompleteOnboarding() {
  const setOnboarding = useAuthStore((s) => s.setOnboarding);
  const navigate = useNavigate();

  return useMutation({
    mutationFn: completeOnboarding,
    onSuccess: (_, variables) => {
      setOnboarding(variables.nativeLanguage, true);
      toast.success("Mirë se vjen! Fillojmë mësimet.");
      navigate("/dashboard");
    },
    onError: () => toast.error("Gabim. Provo sërish."),
  });
}

export function useLogout() {
  const clearAuth = useAuthStore((s) => s.clearAuth);
  const navigate = useNavigate();

  return () => {
    logout();
    clearAuth();
    navigate("/login");
    toast.success("U çkyçe me sukses.");
  };
}
