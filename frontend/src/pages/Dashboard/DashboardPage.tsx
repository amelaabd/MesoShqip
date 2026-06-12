import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { getLessons } from "../../api/lessons";
import { getProgressSummary } from "../../api/progress";
import { useAuthStore } from "../../store/authStore";
import { useLogout } from "../../hooks/useAuth";
import { useTranslation } from "../../hooks/useTranslation";

const langFlags: Record<string, string> = {
  en: "🇬🇧",
  de: "🇩🇪",
  it: "🇮🇹",
  fr: "🇫🇷",
  sv: "🇸🇪",
  tr: "🇹🇷",
};

export default function DashboardPage() {
  const { username, nativeLanguage } = useAuthStore();
  const { t } = useTranslation();
  const logout = useLogout();
  const navigate = useNavigate();

  const { data: progress } = useQuery({
    queryKey: ["progress"],
    queryFn: getProgressSummary,
  });

  const { data: lessons, isLoading: lessonsLoading } = useQuery({
    queryKey: ["lessons"],
    queryFn: () => getLessons(),
  });

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Navbar */}
      <nav className="bg-white border-b border-gray-100 px-4 md:px-6 py-4 flex justify-between items-center sticky top-0 z-10">
        <h1 className="text-xl md:text-2xl font-black text-red-600">
          Mëso<span className="text-teal-500">Shqip</span>
        </h1>
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate("/profile")}
            className="w-9 h-9 rounded-full bg-red-100 flex items-center justify-center font-black text-red-600 text-sm hover:bg-red-200 transition-all"
          >
            {username?.[0]?.toUpperCase() ?? "U"}
          </button>
          <button
            onClick={logout}
            className="text-sm font-bold text-gray-400 hover:text-red-500 hidden md:block"
          >
            {t("logout")}
          </button>
        </div>
      </nav>

      <div className="max-w-2xl mx-auto px-4 py-6">
        {/* Hero */}
        <div className="bg-red-600 rounded-3xl p-5 md:p-6 text-white mb-5 relative overflow-hidden">
          <div className="absolute right-4 top-1/2 -translate-y-1/2 text-5xl md:text-6xl opacity-20">
            🦅
          </div>
          <p className="text-sm opacity-80 mb-1">{t("welcomeBack")}</p>
          <h2 className="text-2xl md:text-3xl font-black mb-3">
            {username}! 👋
          </h2>
          <div className="flex items-center gap-2 flex-wrap">
            <span className="bg-white bg-opacity-20 px-3 py-1 rounded-full text-xs font-bold">
              {langFlags[nativeLanguage] ?? "🌍"} {nativeLanguage.toUpperCase()}
            </span>
            <span className="bg-white bg-opacity-20 px-3 py-1 rounded-full text-xs font-bold">
              📚 {progress?.currentLevel ?? "Fillestor"}
            </span>
          </div>
        </div>

        {/* Stats */}
        {progress && (
          <div className="grid grid-cols-3 gap-3 mb-5">
            {[
              {
                label: t("points"),
                value: progress.totalPoints,
                icon: "⭐",
                color: "text-amber-600",
              },
              {
                label: t("streak"),
                value: progress.currentStreak,
                icon: "🔥",
                color: "text-red-500",
              },
              {
                label: t("completedLessons"),
                value: `${progress.completedLessons}/${progress.totalLessons}`,
                icon: "✅",
                color: "text-teal-600",
              },
            ].map((stat) => (
              <div
                key={stat.label}
                className="bg-white rounded-2xl border border-gray-100 p-3 md:p-4 text-center"
              >
                <p className="text-xl md:text-2xl mb-1">{stat.icon}</p>
                <p className={`text-lg md:text-xl font-black ${stat.color}`}>
                  {stat.value}
                </p>
                <p className="text-xs text-gray-400 font-semibold uppercase tracking-wide mt-1 hidden md:block">
                  {stat.label}
                </p>
              </div>
            ))}
          </div>
        )}

        {/* Story button */}
        <button
          onClick={() => navigate("/story")}
          className="w-full py-4 bg-teal-500 text-white font-black rounded-2xl hover:bg-teal-600 transition-all flex items-center justify-center gap-2 mb-5 text-sm md:text-base"
        >
          {t("todayStory")}
        </button>

        {/* Lessons */}
        <h3 className="text-base md:text-lg font-black text-gray-800 mb-3">
          {t("lessons")}
        </h3>
        <div className="grid gap-3">
          {lessonsLoading ? (
            <div className="bg-white rounded-2xl border border-gray-100 p-8 text-center">
              <p className="text-3xl mb-2 animate-pulse">📚</p>
              <p className="text-gray-400 font-semibold text-sm">
                {t("loading")}
              </p>
            </div>
          ) : !lessons || lessons.length === 0 ? (
            <div className="bg-white rounded-2xl border border-gray-100 p-8 text-center">
              <p className="text-3xl mb-2">📚</p>
              <p className="text-gray-400 font-semibold text-sm">
                {t("noLessons")}
              </p>
            </div>
          ) : (
            lessons.map((lesson) => (
              <div
                key={lesson.id}
                className="bg-white rounded-2xl border border-gray-100 p-4 flex items-center gap-3 hover:shadow-md transition-all cursor-pointer active:scale-[0.98]"
                onClick={() => navigate(`/lesson/${lesson.id}`)}
              >
                <div className="w-10 h-10 md:w-12 md:h-12 rounded-xl bg-red-50 flex items-center justify-center text-lg md:text-xl flex-shrink-0">
                  {lesson.lessonType === "Vocabulary" ? "📚" : "🧠"}
                </div>
                <div className="flex-1 min-w-0">
                  <p className="font-black text-gray-800 text-sm md:text-base truncate">
                    {lesson.titleAlbanian}
                  </p>
                  <p className="text-xs md:text-sm text-gray-400">
                    {lesson.level} · {lesson.vocabularyCount} {t("words")}
                  </p>
                </div>
                <div
                  className="flex items-center gap-2 flex-shrink-0"
                  onClick={(e) => e.stopPropagation()}
                >
                  <button
                    onClick={() => navigate(`/quiz/${lesson.id}`)}
                    className="px-2 md:px-3 py-1 bg-purple-100 text-purple-700 text-xs font-bold rounded-full hover:bg-purple-200 active:scale-95"
                  >
                    {t("quiz")}
                  </button>
                  <span className="text-gray-400 text-lg">›</span>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
