import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import {
  getAdminStats,
  getAdminUsers,
  getAdminChildren,
  getAdminLessons,
  deleteUser,
  toggleLessonPublish,
  changeUserRole,
} from "../../api/admin";
import { useLogout } from "../../hooks/useAuth";
import toast from "react-hot-toast";

type Tab = "stats" | "users" | "children" | "lessons";

export default function AdminDashboard() {
  const [tab, setTab] = useState<Tab>("stats");
  const logout = useLogout();
  const qc = useQueryClient();

  const { data: stats } = useQuery({
    queryKey: ["admin-stats"],
    queryFn: getAdminStats,
  });
  const { data: users } = useQuery({
    queryKey: ["admin-users"],
    queryFn: getAdminUsers,
    enabled: tab === "users",
  });
  const { data: children } = useQuery({
    queryKey: ["admin-children"],
    queryFn: getAdminChildren,
    enabled: tab === "children",
  });
  const { data: lessons } = useQuery({
    queryKey: ["admin-lessons"],
    queryFn: getAdminLessons,
    enabled: tab === "lessons",
  });

  const deleteMutation = useMutation({
    mutationFn: deleteUser,
    onSuccess: () => {
      toast.success("Useri u fshi.");
      qc.invalidateQueries({ queryKey: ["admin-users"] });
    },
  });

  const toggleMutation = useMutation({
    mutationFn: toggleLessonPublish,
    onSuccess: () => {
      toast.success("Statusi u ndryshua.");
      qc.invalidateQueries({ queryKey: ["admin-lessons"] });
    },
  });

  const roleMutation = useMutation({
    mutationFn: ({ userId, role }: { userId: string; role: string }) =>
      changeUserRole(userId, role),
    onSuccess: () => {
      toast.success("Roli u ndryshua.");
      qc.invalidateQueries({ queryKey: ["admin-users"] });
    },
  });

  const chartData = stats
    ? [
        { name: "Userë", value: stats.totalUsers },
        { name: "Fëmijë", value: stats.totalChildren },
        { name: "Mësime", value: stats.totalLessons },
        { name: "Kuize", value: stats.totalQuizzes },
        { name: "Histori", value: stats.totalStories },
      ]
    : [];

  const tabs: { key: Tab; label: string; icon: string }[] = [
    { key: "stats", label: "Statistika", icon: "📊" },
    { key: "users", label: "Userët", icon: "👥" },
    { key: "children", label: "Fëmijët", icon: "👶" },
    { key: "lessons", label: "Mësimet", icon: "📚" },
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Navbar */}
      <nav className="bg-white border-b border-gray-100 px-6 py-4 flex justify-between items-center">
        <div className="flex items-center gap-3">
          <h1 className="text-2xl font-black text-red-600">
            Mëso<span className="text-teal-500">Shqip</span>
          </h1>
          <span className="px-3 py-1 bg-red-100 text-red-700 text-xs font-black rounded-full">
            ADMIN
          </span>
        </div>
        <button
          onClick={logout}
          className="text-sm font-bold text-red-500 hover:text-red-700"
        >
          Çkyçu
        </button>
      </nav>

      <div className="max-w-6xl mx-auto px-4 py-8">
        {/* Tabs */}
        <div className="flex gap-2 mb-8 bg-white rounded-2xl border border-gray-100 p-2 w-fit">
          {tabs.map((t) => (
            <button
              key={t.key}
              onClick={() => setTab(t.key)}
              className={`px-5 py-2 rounded-xl text-sm font-bold transition-all ${
                tab === t.key
                  ? "bg-red-600 text-white"
                  : "text-gray-500 hover:bg-gray-50"
              }`}
            >
              {t.icon} {t.label}
            </button>
          ))}
        </div>

        {/* Stats Tab */}
        {tab === "stats" && stats && (
          <div>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
              {[
                {
                  label: "Userë total",
                  value: stats.totalUsers,
                  icon: "👥",
                  color: "text-blue-600",
                },
                {
                  label: "Fëmijë",
                  value: stats.totalChildren,
                  icon: "👶",
                  color: "text-teal-600",
                },
                {
                  label: "Aktiv sot",
                  value: stats.activeToday,
                  icon: "🔥",
                  color: "text-red-600",
                },
                {
                  label: "Mësime kryer",
                  value: stats.completedLessons,
                  icon: "✅",
                  color: "text-green-600",
                },
              ].map((s) => (
                <div
                  key={s.label}
                  className="bg-white rounded-2xl border border-gray-100 p-5 text-center"
                >
                  <p className="text-3xl mb-2">{s.icon}</p>
                  <p className={`text-3xl font-black ${s.color}`}>{s.value}</p>
                  <p className="text-xs text-gray-400 font-semibold uppercase tracking-wide mt-1">
                    {s.label}
                  </p>
                </div>
              ))}
            </div>

            <div className="bg-white rounded-2xl border border-gray-100 p-6">
              <h3 className="font-black text-gray-800 mb-4">
                Pamje e përgjithshme
              </h3>
              <ResponsiveContainer width="100%" height={280}>
                <BarChart data={chartData}>
                  <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                  <XAxis
                    dataKey="name"
                    tick={{ fontSize: 12, fontWeight: 700 }}
                  />
                  <YAxis tick={{ fontSize: 12 }} />
                  <Tooltip />
                  <Bar dataKey="value" fill="#E8294A" radius={[8, 8, 0, 0]} />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        )}

        {/* Users Tab */}
        {tab === "users" && (
          <div className="bg-white rounded-2xl border border-gray-100 overflow-hidden">
            <table className="w-full">
              <thead className="bg-gray-50 border-b border-gray-100">
                <tr>
                  {[
                    "Username",
                    "Email",
                    "Roli",
                    "Fëmijë",
                    "Krijuar",
                    "Veprime",
                  ].map((h) => (
                    <th
                      key={h}
                      className="px-4 py-3 text-left text-xs font-black text-gray-500 uppercase tracking-wide"
                    >
                      {h}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {users?.map((user: any) => (
                  <tr
                    key={user.id}
                    className="hover:bg-gray-50 transition-colors"
                  >
                    <td className="px-4 py-3 font-bold text-gray-800">
                      {user.username}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-500">
                      {user.email}
                    </td>
                    <td className="px-4 py-3">
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-black ${
                          user.role === "Admin"
                            ? "bg-red-100 text-red-700"
                            : "bg-teal-100 text-teal-700"
                        }`}
                      >
                        {user.role}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-600">
                      {user.childCount}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-400">
                      {new Date(user.createdAt).toLocaleDateString("sq-AL")}
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex gap-2">
                        <button
                          onClick={() =>
                            roleMutation.mutate({
                              userId: user.id,
                              role: user.role === "Admin" ? "Parent" : "Admin",
                            })
                          }
                          className="px-3 py-1 text-xs font-bold bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100"
                        >
                          {user.role === "Admin" ? "Bëj User" : "Bëj Admin"}
                        </button>
                        <button
                          onClick={() => {
                            if (confirm("A je i sigurt?"))
                              deleteMutation.mutate(user.id);
                          }}
                          className="px-3 py-1 text-xs font-bold bg-red-50 text-red-600 rounded-lg hover:bg-red-100"
                        >
                          Fshi
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* Children Tab */}
        {tab === "children" && (
          <div className="bg-white rounded-2xl border border-gray-100 overflow-hidden">
            <table className="w-full">
              <thead className="bg-gray-50 border-b border-gray-100">
                <tr>
                  {[
                    "Emri",
                    "Prindi",
                    "Niveli",
                    "Pikë",
                    "Streak",
                    "Gjuha",
                    "Aktiv",
                  ].map((h) => (
                    <th
                      key={h}
                      className="px-4 py-3 text-left text-xs font-black text-gray-500 uppercase tracking-wide"
                    >
                      {h}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {children?.map((child: any) => (
                  <tr
                    key={child.id}
                    className="hover:bg-gray-50 transition-colors"
                  >
                    <td className="px-4 py-3 font-bold text-gray-800">
                      🦅 {child.displayName}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-500">
                      {child.parentUsername}
                    </td>
                    <td className="px-4 py-3">
                      <span className="px-2 py-1 bg-teal-50 text-teal-700 text-xs font-black rounded-full">
                        {child.currentLevel}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-sm font-bold text-amber-600">
                      ⭐ {child.totalPoints}
                    </td>
                    <td className="px-4 py-3 text-sm font-bold text-red-500">
                      🔥 {child.currentStreak}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-500">
                      {child.nativeLanguage.toUpperCase()}
                    </td>
                    <td className="px-4 py-3 text-xs text-gray-400">
                      {child.lastActivityDate
                        ? new Date(child.lastActivityDate).toLocaleDateString(
                            "sq-AL",
                          )
                        : "—"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* Lessons Tab */}
        {tab === "lessons" && (
          <div className="bg-white rounded-2xl border border-gray-100 overflow-hidden">
            <table className="w-full">
              <thead className="bg-gray-50 border-b border-gray-100">
                <tr>
                  {[
                    "Titulli",
                    "Niveli",
                    "Tipi",
                    "Fjalë",
                    "Statusi",
                    "Veprime",
                  ].map((h) => (
                    <th
                      key={h}
                      className="px-4 py-3 text-left text-xs font-black text-gray-500 uppercase tracking-wide"
                    >
                      {h}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {lessons?.map((lesson: any) => (
                  <tr
                    key={lesson.id}
                    className="hover:bg-gray-50 transition-colors"
                  >
                    <td className="px-4 py-3 font-bold text-gray-800">
                      {lesson.titleAlbanian}
                    </td>
                    <td className="px-4 py-3">
                      <span className="px-2 py-1 bg-blue-50 text-blue-700 text-xs font-black rounded-full">
                        {lesson.level}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-500">
                      {lesson.lessonType}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-600">
                      {lesson.vocabCount}
                    </td>
                    <td className="px-4 py-3">
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-black ${
                          lesson.isPublished
                            ? "bg-green-100 text-green-700"
                            : "bg-gray-100 text-gray-500"
                        }`}
                      >
                        {lesson.isPublished ? "Publikuar" : "Draft"}
                      </span>
                    </td>
                    <td className="px-4 py-3">
                      <button
                        onClick={() => toggleMutation.mutate(lesson.id)}
                        className={`px-3 py-1 text-xs font-bold rounded-lg transition-all ${
                          lesson.isPublished
                            ? "bg-gray-50 text-gray-600 hover:bg-gray-100"
                            : "bg-green-50 text-green-600 hover:bg-green-100"
                        }`}
                      >
                        {lesson.isPublished ? "Çpubliko" : "Publiko"}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
