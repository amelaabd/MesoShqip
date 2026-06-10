import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { getLessons } from "../../api/lessons";
import { getProgressSummary } from "../../api/progress";
import { useAuthStore } from "../../store/authStore";
import { useLogout } from "../../hooks/useAuth";

export default function DashboardPage() {
  const { username, nativeLanguage } = useAuthStore();
  const logout = useLogout();
  const navigate = useNavigate();

  const { data: progress } = useQuery({
    queryKey: ["progress"],
    queryFn: getProgressSummary,
  });

  const { data: lessons } = useQuery({
    queryKey: ["lessons"],
    queryFn: () => getLessons(),
  });

  const langFlags: Record<string, string> = {
    en: "🇬🇧",
    de: "🇩🇪",
    it: "🇮🇹",
    fr: "🇫🇷",
    sv: "🇸🇪",
    tr: "🇹🇷",
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Navbar */}
      <nav className="bg-white border-b border-gray-100 px-6 py-4 flex justify-between items-center sticky top-0 z-10">
        <h1 className="text-2xl font-black text-red-600">
          Mëso<span className="text-teal-500">Shqip</span>
        </h1>
        <div className="flex items-center gap-4">
          <span className="text-sm font-semibold text-gray-600">
            👋 {username}
          </span>
          <button
            onClick={logout}
            className="text-sm font-bold text-red-500 hover:text-red-700"
          >
            Çkyçu
          </button>
        </div>
      </nav>

      <div className="max-w-2xl mx-auto px-4 py-8">
        {/* Hero */}
        <div className="bg-red-600 rounded-3xl p-6 text-white mb-6 relative overflow-hidden">
          <div className="absolute right-6 top-1/2 -translate-y-1/2 text-6xl opacity-20">
            🦅
          </div>
          <p className="text-sm opacity-80 mb-1">Mirë se erdhe,</p>
          <h2 className="text-3xl font-black mb-3">{username}! 👋</h2>
          <div className="flex items-center gap-2">
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
          <div className="grid grid-cols-3 gap-4 mb-8">
            {[
              {
                label: "Pikë",
                value: progress.totalPoints,
                icon: "⭐",
                color: "text-amber-600",
              },
              {
                label: "Streak",
                value: progress.currentStreak,
                icon: "🔥",
                color: "text-red-500",
              },
              {
                label: "Mësimet",
                value: `${progress.completedLessons}/${progress.totalLessons}`,
                icon: "✅",
                color: "text-teal-600",
              },
            ].map((stat) => (
              <div
                key={stat.label}
                className="bg-white rounded-2xl border border-gray-100 p-4 text-center"
              >
                <p className="text-2xl mb-1">{stat.icon}</p>
                <p className={`text-xl font-black ${stat.color}`}>
                  {stat.value}
                </p>
                <p className="text-xs text-gray-400 font-semibold uppercase tracking-wide mt-1">
                  {stat.label}
                </p>
              </div>
            ))}
          </div>
        )}

        {/* Historia */}
        <button
          onClick={() => navigate("/story")}
          className="w-full py-4 bg-teal-500 text-white font-black rounded-2xl hover:bg-teal-600 transition-all flex items-center justify-center gap-2 mb-6"
        >
          ✨ Gjenero historinë e sotme
        </button>

        {/* Lessons */}
        <div>
          <h3 className="text-lg font-black text-gray-800 mb-4">Mësimet</h3>
          <div className="grid gap-3">
            {lessons?.map((lesson) => (
              <button
                key={lesson.id}
                onClick={() => navigate(`/lesson/${lesson.id}`)}
                className="bg-white rounded-2xl border border-gray-100 p-4 flex items-center gap-4 hover:shadow-md transition-all text-left"
              >
                <div className="w-12 h-12 rounded-xl bg-red-50 flex items-center justify-center text-xl flex-shrink-0">
                  {lesson.lessonType === "Vocabulary" ? "📚" : "🧠"}
                </div>
                <div className="flex-1">
                  <p className="font-black text-gray-800">
                    {lesson.titleAlbanian}
                  </p>
                  <p className="text-sm text-gray-400">
                    {lesson.level} · {lesson.vocabularyCount} fjalë
                  </p>
                </div>
                <div className="flex items-center gap-2">
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(`/quiz/${lesson.id}`);
                    }}
                    className="px-3 py-1 bg-purple-100 text-purple-700 text-xs font-bold rounded-full hover:bg-purple-200"
                  >
                    Quiz
                  </button>
                  <span className="text-gray-400 text-xl">›</span>
                </div>
              </button>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
