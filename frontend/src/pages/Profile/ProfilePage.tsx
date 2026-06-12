import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getProgressSummary, getBadges } from "../../api/progress";
import { completeOnboarding } from "../../api/auth";
import { useAuthStore } from "../../store/authStore";
import { useLogout } from "../../hooks/useAuth";
import { useTranslation } from "../../hooks/useTranslation";
import type { SupportedLanguage } from "../../i18n/translations";
import toast from "react-hot-toast";

const LANGUAGES = [
  { code: "en", flag: "🇬🇧", name: "English" },
  { code: "de", flag: "🇩🇪", name: "Deutsch" },
  { code: "it", flag: "🇮🇹", name: "Italiano" },
  { code: "fr", flag: "🇫🇷", name: "Français" },
  { code: "sv", flag: "🇸🇪", name: "Svenska" },
  { code: "tr", flag: "🇹🇷", name: "Türkçe" },
];

const LEVELS = [
  { value: 1, label: "Fillestor", emoji: "🌱" },
  { value: 2, label: "Mesatar", emoji: "📖" },
  { value: 3, label: "Avancuar", emoji: "🚀" },
];

export default function ProfilePage() {
  const { username, nativeLanguage, setOnboarding } = useAuthStore();
  const { t } = useTranslation();
  const logout = useLogout();
  const navigate = useNavigate();
  const qc = useQueryClient();

  const [editLang, setEditLang] = useState(false);
  const [editLevel, setEditLevel] = useState(false);
  const [selLang, setSelLang] = useState(nativeLanguage as SupportedLanguage);
  const [selLevel, setSelLevel] = useState(1);

  const { data: progress } = useQuery({
    queryKey: ["progress"],
    queryFn: getProgressSummary,
  });

  const { data: badges } = useQuery({
    queryKey: ["badges"],
    queryFn: getBadges,
  });

  const updateMutation = useMutation({
    mutationFn: completeOnboarding,
    onSuccess: (_, variables) => {
      setOnboarding(variables.nativeLanguage, true);
      qc.invalidateQueries({ queryKey: ["progress"] });
      toast.success(t("saveChanges") + " ✓");
      setEditLang(false);
      setEditLevel(false);
    },
    onError: () => toast.error("Gabim. Provo sërish."),
  });

  const initials = username?.[0]?.toUpperCase() ?? "U";

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3 sticky top-0 z-10">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <h2 className="font-black text-gray-800 flex-1">{t("profile")}</h2>
        <button onClick={logout} className="text-sm font-bold text-red-500">
          {t("logout")}
        </button>
      </div>

      <div className="max-w-lg mx-auto px-4 py-6 space-y-4">
        {/* Avatar card */}
        <div className="bg-white rounded-3xl border border-gray-100 p-6 flex items-center gap-5">
          <div className="w-20 h-20 rounded-full bg-red-500 flex items-center justify-center font-black text-white text-3xl flex-shrink-0">
            {initials}
          </div>
          <div>
            <p className="font-black text-xl text-gray-800">{username}</p>
            <p className="text-sm text-gray-400 mt-1">
              {LANGUAGES.find((l) => l.code === nativeLanguage)?.flag}{" "}
              {LANGUAGES.find((l) => l.code === nativeLanguage)?.name}
            </p>
            <p className="text-sm text-gray-400">
              📚 {progress?.currentLevel ?? "Fillestor"}
            </p>
          </div>
        </div>

        {/* Stats */}
        {progress && (
          <div className="grid grid-cols-2 gap-3">
            {[
              {
                label: t("totalPoints"),
                value: progress.totalPoints,
                icon: "⭐",
                color: "text-amber-600",
              },
              {
                label: t("currentStreak"),
                value: `${progress.currentStreak} ${t("days")}`,
                icon: "🔥",
                color: "text-red-500",
              },
              {
                label: t("completedLessons"),
                value: `${progress.completedLessons}/${progress.totalLessons}`,
                icon: "✅",
                color: "text-teal-600",
              },
              {
                label: t("changeLevel"),
                value: progress.currentLevel,
                icon: "📚",
                color: "text-blue-600",
              },
            ].map((s) => (
              <div
                key={s.label}
                className="bg-white rounded-2xl border border-gray-100 p-4"
              >
                <p className="text-2xl mb-1">{s.icon}</p>
                <p className={`text-lg font-black ${s.color}`}>{s.value}</p>
                <p className="text-xs text-gray-400 font-semibold mt-1">
                  {s.label}
                </p>
              </div>
            ))}
          </div>
        )}

        {/* Badges */}
        {badges && (
          <div className="bg-white rounded-3xl border border-gray-100 overflow-hidden">
            <div className="px-5 py-4 border-b border-gray-50">
              <p className="font-black text-gray-800 text-sm uppercase tracking-wide">
                {t("badges")}
              </p>
            </div>
            <div className="p-4 grid grid-cols-3 gap-3">
              {badges.map((badge: any) => (
                <div
                  key={badge.id}
                  className={`flex flex-col items-center gap-2 p-3 rounded-2xl border transition-all ${
                    badge.isEarned
                      ? "border-amber-200 bg-amber-50"
                      : "border-gray-100 bg-gray-50 opacity-40"
                  }`}
                >
                  <span className="text-2xl">{badge.iconUrl}</span>
                  <span className="text-xs font-bold text-gray-700 text-center leading-tight">
                    {badge.name}
                  </span>
                  {badge.isEarned && (
                    <span className="text-xs text-amber-600 font-bold">✓</span>
                  )}
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Settings */}
        <div className="bg-white rounded-3xl border border-gray-100 overflow-hidden">
          <div className="px-5 py-4 border-b border-gray-50">
            <p className="font-black text-gray-800 text-sm uppercase tracking-wide">
              {t("settings")}
            </p>
          </div>

          {/* Change language */}
          <div className="px-5 py-4 border-b border-gray-50">
            <div className="flex justify-between items-center mb-2">
              <p className="font-bold text-gray-700 text-sm">
                {t("changeLanguage")}
              </p>
              <button
                onClick={() => {
                  setEditLang(!editLang);
                  setSelLang(nativeLanguage as SupportedLanguage);
                }}
                className="text-xs font-bold text-red-500"
              >
                {editLang ? "Anulo" : t("editProfile")}
              </button>
            </div>
            {!editLang ? (
              <p className="text-gray-500 text-sm">
                {LANGUAGES.find((l) => l.code === nativeLanguage)?.flag}{" "}
                {LANGUAGES.find((l) => l.code === nativeLanguage)?.name}
              </p>
            ) : (
              <div className="grid grid-cols-3 gap-2 mt-3">
                {LANGUAGES.map((lang) => (
                  <button
                    key={lang.code}
                    onClick={() => setSelLang(lang.code as SupportedLanguage)}
                    className={`p-2 rounded-xl border text-center text-xs font-bold transition-all ${
                      selLang === lang.code
                        ? "border-red-400 bg-red-50"
                        : "border-gray-100"
                    }`}
                  >
                    {lang.flag} {lang.name}
                  </button>
                ))}
                <button
                  onClick={() =>
                    updateMutation.mutate({
                      nativeLanguage: selLang,
                      level: selLevel || 1,
                    })
                  }
                  className="col-span-3 py-2 bg-red-600 text-white text-xs font-black rounded-xl mt-1"
                >
                  {t("saveChanges")}
                </button>
              </div>
            )}
          </div>

          {/* Change level */}
          <div className="px-5 py-4">
            <div className="flex justify-between items-center mb-2">
              <p className="font-bold text-gray-700 text-sm">
                {t("changeLevel")}
              </p>
              <button
                onClick={() => setEditLevel(!editLevel)}
                className="text-xs font-bold text-red-500"
              >
                {editLevel ? "Anulo" : t("editProfile")}
              </button>
            </div>
            {!editLevel ? (
              <p className="text-gray-500 text-sm">
                📚 {progress?.currentLevel}
              </p>
            ) : (
              <div className="flex flex-col gap-2 mt-3">
                {LEVELS.map((lv) => (
                  <button
                    key={lv.value}
                    onClick={() => setSelLevel(lv.value)}
                    className={`p-3 rounded-xl border text-left text-sm font-bold transition-all ${
                      selLevel === lv.value
                        ? "border-red-400 bg-red-50"
                        : "border-gray-100"
                    }`}
                  >
                    {lv.emoji} {lv.label}
                  </button>
                ))}
                <button
                  onClick={() =>
                    updateMutation.mutate({
                      nativeLanguage: selLang || nativeLanguage,
                      level: selLevel,
                    })
                  }
                  className="py-2 bg-red-600 text-white text-xs font-black rounded-xl mt-1"
                >
                  {t("saveChanges")}
                </button>
              </div>
            )}
          </div>
        </div>

        {/* Logout */}
        <button
          onClick={logout}
          className="w-full py-4 border-2 border-red-200 text-red-600 font-black rounded-2xl hover:bg-red-50 transition-all"
        >
          {t("logout")}
        </button>
      </div>
    </div>
  );
}
