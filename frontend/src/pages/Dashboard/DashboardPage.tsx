import { useQuery, useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { getChildren, createChild } from "../../api/children";
import { getLessons } from "../../api/lessons";
import { getProgressSummary } from "../../api/progress";
import { useAuthStore } from "../../store/authStore";
import { useLogout } from "../../hooks/useAuth";
import type { ChildProfile } from "../../types";
import toast from "react-hot-toast";

export default function DashboardPage() {
  const { username } = useAuthStore();
  const logout = useLogout();
  const navigate = useNavigate();

  const [selectedChild, setSelectedChild] = useState<ChildProfile | null>(null);
  const [showCreateChild, setShowCreateChild] = useState(false);
  const [newChild, setNewChild] = useState({
    displayName: "",
    avatarCode: "eagle",
    nativeLanguage: "en",
    startingLevel: 1,
  });

  const { data: children, refetch: refetchChildren } = useQuery({
    queryKey: ["children"],
    queryFn: getChildren,
  });

  const { data: lessons } = useQuery({
    queryKey: ["lessons"],
    queryFn: () => getLessons(),
  });

  const { data: progress } = useQuery({
    queryKey: ["progress", selectedChild?.id],
    queryFn: () => getProgressSummary(selectedChild!.id),
    enabled: !!selectedChild,
  });

  const createMutation = useMutation({
    mutationFn: createChild,
    onSuccess: () => {
      refetchChildren();
      setShowCreateChild(false);
      setNewChild({
        displayName: "",
        avatarCode: "eagle",
        nativeLanguage: "en",
        startingLevel: 1,
      });
      toast.success("Profili u krijua me sukses!");
    },
    onError: () => toast.error("Gabim gjatë krijimit. Provo sërish."),
  });

  const handleCreateChild = (e: React.FormEvent) => {
    e.preventDefault();
    createMutation.mutate(newChild);
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
            className="text-sm font-bold text-red-500 hover:text-red-700 transition-colors"
          >
            Çkyçu
          </button>
        </div>
      </nav>

      <div className="max-w-5xl mx-auto px-4 py-8">
        {/* Hero */}
        <div className="bg-red-600 rounded-3xl p-6 text-white mb-6 relative overflow-hidden">
          <div className="absolute right-6 top-1/2 -translate-y-1/2 text-6xl opacity-20">
            🦅
          </div>
          <p className="text-sm opacity-80 mb-1">Mirë se erdhe,</p>
          <h2 className="text-3xl font-black mb-2">{username}! 👋</h2>
          <p className="text-sm opacity-80">
            Zgjidh profilin e fëmijës për të vazhduar mësimet.
          </p>
        </div>

        {/* Children Profiles */}
        <div className="mb-8">
          <div className="flex justify-between items-center mb-4">
            <h3 className="text-lg font-black text-gray-800">
              Profilet e fëmijëve
            </h3>
            <button
              onClick={() => setShowCreateChild(true)}
              className="px-4 py-2 bg-red-600 text-white text-sm font-bold rounded-xl hover:bg-red-700 transition-all"
            >
              + Shto fëmijë
            </button>
          </div>

          {children?.length === 0 && (
            <div className="text-center py-12 bg-white rounded-2xl border border-gray-100">
              <p className="text-4xl mb-3">👶</p>
              <p className="text-gray-500 font-semibold mb-4">
                Nuk ke profile ende.
              </p>
              <button
                onClick={() => setShowCreateChild(true)}
                className="px-6 py-2 bg-red-600 text-white font-bold rounded-xl hover:bg-red-700"
              >
                Krijo profilin e parë
              </button>
            </div>
          )}

          <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
            {children?.map((child) => (
              <button
                key={child.id}
                onClick={() => setSelectedChild(child)}
                className={`p-5 rounded-2xl border-2 text-left transition-all hover:shadow-md ${
                  selectedChild?.id === child.id
                    ? "border-red-500 bg-red-50"
                    : "border-gray-100 bg-white"
                }`}
              >
                <div className="text-3xl mb-2">🦅</div>
                <p className="font-black text-gray-800">{child.displayName}</p>
                <p className="text-xs text-gray-400 mt-1">
                  {child.currentLevel}
                </p>
                <div className="flex items-center gap-2 mt-2">
                  <span className="text-xs font-bold text-amber-600">
                    ⭐ {child.totalPoints}
                  </span>
                  <span className="text-xs font-bold text-red-500">
                    🔥 {child.currentStreak}
                  </span>
                </div>
              </button>
            ))}
          </div>
        </div>

        {/* Progress Summary */}
        {selectedChild && progress && (
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
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
                label: "Niveli",
                value: progress.currentLevel,
                icon: "📚",
                color: "text-teal-600",
              },
              {
                label: "Mësimet",
                value: `${progress.completedLessons}/${progress.totalLessons}`,
                icon: "✅",
                color: "text-blue-600",
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

        {/* Lessons + Story */}
        {selectedChild && (
          <div>
            <h3 className="text-lg font-black text-gray-800 mb-4">
              Mësimet e disponueshme
            </h3>

            {/* Story button */}
            <button
              onClick={() =>
                navigate(
                  `/story?childId=${selectedChild.id}&lessonId=${lessons?.[0]?.id ?? ""}`,
                )
              }
              className="w-full py-4 bg-teal-500 text-white font-black rounded-2xl hover:bg-teal-600 transition-all flex items-center justify-center gap-2 mb-4"
            >
              ✨ Gjenero historinë e sotme
            </button>

            {/* Lessons list */}
            <div className="grid gap-3">
              {lessons?.map((lesson) => (
                <button
                  key={lesson.id}
                  onClick={() =>
                    navigate(`/lesson/${lesson.id}?childId=${selectedChild.id}`)
                  }
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
                        navigate(
                          `/quiz/${lesson.id}?childId=${selectedChild.id}`,
                        );
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
        )}
      </div>

      {/* Create Child Modal */}
      {showCreateChild && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center px-4 z-50">
          <div className="bg-white rounded-3xl p-8 w-full max-w-md shadow-xl">
            <div className="flex justify-between items-center mb-6">
              <h3 className="text-xl font-black text-gray-800">
                Krijo profil të ri
              </h3>
              <button
                onClick={() => setShowCreateChild(false)}
                className="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center text-gray-500 hover:bg-gray-200"
              >
                ✕
              </button>
            </div>
            <form onSubmit={handleCreateChild} className="space-y-4">
              <div>
                <label className="block text-sm font-bold text-gray-700 mb-2">
                  Emri i fëmijës
                </label>
                <input
                  type="text"
                  value={newChild.displayName}
                  onChange={(e) =>
                    setNewChild({ ...newChild, displayName: e.target.value })
                  }
                  placeholder="p.sh. Amela"
                  required
                  className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 focus:ring-2 focus:ring-red-100 transition-all"
                />
              </div>

              <div>
                <label className="block text-sm font-bold text-gray-700 mb-2">
                  Gjuha amtare
                </label>
                <select
                  value={newChild.nativeLanguage}
                  onChange={(e) =>
                    setNewChild({ ...newChild, nativeLanguage: e.target.value })
                  }
                  className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 transition-all"
                >
                  <option value="en">🇬🇧 English</option>
                  <option value="de">🇩🇪 Deutsch</option>
                  <option value="it">🇮🇹 Italiano</option>
                  <option value="fr">🇫🇷 Français</option>
                  <option value="sv">🇸🇪 Svenska</option>
                  <option value="tr">🇹🇷 Türkçe</option>
                </select>
              </div>

              <div>
                <label className="block text-sm font-bold text-gray-700 mb-2">
                  Niveli fillestar
                </label>
                <select
                  value={newChild.startingLevel}
                  onChange={(e) =>
                    setNewChild({
                      ...newChild,
                      startingLevel: Number(e.target.value),
                    })
                  }
                  className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:border-red-400 transition-all"
                >
                  <option value={1}>🌱 Fillestor</option>
                  <option value={2}>📖 Mesatar</option>
                  <option value={3}>🚀 Avancuar</option>
                </select>
              </div>

              <div className="flex gap-3 pt-2">
                <button
                  type="button"
                  onClick={() => setShowCreateChild(false)}
                  className="flex-1 py-3 rounded-xl border-2 border-gray-200 font-bold text-gray-600 hover:bg-gray-50 transition-all"
                >
                  Anulo
                </button>
                <button
                  type="submit"
                  disabled={createMutation.isPending}
                  className="flex-1 py-3 rounded-xl bg-red-600 text-white font-bold hover:bg-red-700 transition-all disabled:opacity-50"
                >
                  {createMutation.isPending ? "Duke krijuar..." : "Krijo"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
