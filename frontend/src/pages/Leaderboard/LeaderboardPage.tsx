import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { getLeaderboard } from "../../api/progress";
import { useAuthStore } from "../../store/authStore";

interface LeaderboardUser {
  username: string;
  totalPoints: number;
  currentStreak: number;
  level: string;
}

export default function LeaderboardPage() {
  const navigate = useNavigate();
  const { username } = useAuthStore();

  const { data: leaderboard, isLoading } = useQuery<LeaderboardUser[]>({
    queryKey: ["leaderboard"],
    queryFn: getLeaderboard,
  });

  const medals = ["🥇", "🥈", "🥉"];

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3 sticky top-0 z-10">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <h2 className="font-black text-gray-800">🏆 Renditja</h2>
      </div>

      <div className="max-w-lg mx-auto px-4 py-6">
        {isLoading ? (
          <div className="text-center py-12">
            <p className="text-gray-400 font-bold">Duke ngarkuar...</p>
          </div>
        ) : !leaderboard || leaderboard.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-gray-400 font-bold">
              Nuk ka përdorues në renditje
            </p>
          </div>
        ) : (
          <div className="space-y-2">
            {leaderboard.map((user: LeaderboardUser, idx: number) => (
              <div
                key={user.username}
                className={`flex items-center gap-3 p-4 rounded-2xl border ${
                  user.username === username
                    ? "bg-red-50 border-red-200"
                    : "bg-white border-gray-100"
                }`}
              >
                <div className="w-10 text-center font-black text-lg">
                  {idx < 3 ? medals[idx] : `#${idx + 1}`}
                </div>
                <div className="w-10 h-10 rounded-full bg-red-100 flex items-center justify-center font-black text-red-600 text-sm">
                  {user.username?.[0]?.toUpperCase() || "U"}
                </div>
                <div className="flex-1">
                  <p className="font-bold text-gray-800 text-sm">
                    {user.username}
                    {user.username === username && (
                      <span className="text-xs text-red-500 ml-1">(ti)</span>
                    )}
                  </p>
                  <p className="text-xs text-gray-400">
                    📚 {user.level} · 🔥 {user.currentStreak}
                  </p>
                </div>
                <div className="text-right">
                  <p className="font-black text-amber-600">
                    {user.totalPoints}
                  </p>
                  <p className="text-xs text-gray-400">pikë</p>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
