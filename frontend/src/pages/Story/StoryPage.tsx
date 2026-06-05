import { useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useMutation, useQuery } from "@tanstack/react-query";
import { generateStory } from "../../api/stories";
import { getLessonById } from "../../api/lessons";
import toast from "react-hot-toast";
import type { StoryResponse } from "../../types";

export default function StoryPage() {
  const [searchParams] = useSearchParams();
  const childId = searchParams.get("childId") ?? "";
  const lessonId = searchParams.get("lessonId") ?? "";
  const navigate = useNavigate();
  const [story, setStory] = useState<StoryResponse | null>(null);
  const [showTranslation, setShowTranslation] = useState(false);

  const { data: lesson } = useQuery({
    queryKey: ["lesson", lessonId],
    queryFn: () => getLessonById(lessonId),
    enabled: !!lessonId,
  });

  const mutation = useMutation({
    mutationFn: generateStory,
    onSuccess: (data) => {
      setStory(data);
      toast.success("Historia u gjenerua!");
    },
    onError: () => toast.error("Gabim gjatë gjenerimit. Provo sërish."),
  });

  const handleGenerate = () => {
    const words = lesson?.vocabularyItems
      ?.slice(0, 5)
      .map((v) => v.wordAlbanian) ?? ["shtëpia", "familja", "dashuria"];

    mutation.mutate({ childProfileId: childId, wordsToIntroduce: words });
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <div className="flex-1">
          <p className="font-black text-gray-800">Historia AI</p>
          <p className="text-xs text-gray-400">Gjeneruar nga Claude AI</p>
        </div>
        <span className="text-xs font-bold bg-teal-100 text-teal-700 px-3 py-1 rounded-full">
          +3 XP
        </span>
      </div>

      <div className="max-w-lg mx-auto px-4 py-6">
        {/* Generate button */}
        {!story && (
          <div className="text-center py-16">
            <div className="text-7xl mb-6">🦅</div>
            <h2 className="text-2xl font-black text-gray-800 mb-3">
              Historia e sotme
            </h2>
            <p className="text-gray-500 mb-8 max-w-xs mx-auto">
              Claude AI do të krijojë një histori të personalizuar me fjalët që
              ke mësuar.
            </p>
            <button
              onClick={handleGenerate}
              disabled={mutation.isPending}
              className="px-8 py-4 bg-teal-500 text-white font-black rounded-2xl hover:bg-teal-600 transition-all disabled:opacity-50 text-lg"
            >
              {mutation.isPending ? (
                <span className="flex items-center gap-2">
                  <svg
                    className="animate-spin h-5 w-5"
                    viewBox="0 0 24 24"
                    fill="none"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    />
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8v8H4z"
                    />
                  </svg>
                  Duke gjeneruar...
                </span>
              ) : (
                "✨ Gjenero historinë"
              )}
            </button>
          </div>
        )}

        {/* Story content */}
        {story && (
          <div className="space-y-5">
            {/* Story hero */}
            <div className="bg-teal-500 rounded-3xl p-6 text-white relative overflow-hidden">
              <div className="absolute right-4 top-1/2 -translate-y-1/2 text-6xl opacity-20">
                🦅
              </div>
              <p className="text-xs font-bold uppercase tracking-widest opacity-75 mb-2">
                ✨ Historia e ditës
              </p>
              <h2 className="text-2xl font-black leading-tight">
                {story.titleAlbanian}
              </h2>
            </div>

            {/* Story body */}
            <div className="bg-white rounded-2xl border border-gray-100 p-6">
              <div className="flex justify-between items-center mb-4">
                <p className="text-xs font-black text-gray-400 uppercase tracking-wide">
                  🇦🇱 Shqip
                </p>
                <button
                  onClick={() => setShowTranslation(!showTranslation)}
                  className="text-xs font-bold text-teal-600 hover:underline"
                >
                  {showTranslation ? "Fshih përkthimin" : "Shfaq përkthimin"}
                </button>
              </div>
              <p className="text-gray-800 leading-relaxed text-base">
                {story.bodyAlbanian}
              </p>

              {showTranslation && (
                <div className="mt-4 pt-4 border-t border-gray-100">
                  <p className="text-xs font-black text-gray-400 uppercase tracking-wide mb-3">
                    🌍 Përkthimi
                  </p>
                  <p className="text-gray-600 leading-relaxed text-sm italic">
                    {story.bodyTranslated}
                  </p>
                </div>
              )}
            </div>

            {/* New words */}
            {story.newWords?.length > 0 && (
              <div>
                <p className="font-black text-gray-800 mb-3">
                  📖 Fjalë të reja
                </p>
                <div className="grid grid-cols-2 gap-3">
                  {story.newWords.map((w, i) => (
                    <div
                      key={i}
                      className="bg-amber-50 border border-amber-200 rounded-2xl p-4"
                    >
                      <p className="font-black text-amber-800 text-base">
                        {w.albanian}
                      </p>
                      <p className="text-amber-600 text-sm mt-1">
                        {w.translated}
                      </p>
                      <p className="text-amber-400 text-xs italic mt-1">
                        [{w.phonetic}]
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {/* Actions */}
            <div className="flex gap-3 pt-2">
              <button
                onClick={() => navigate(`/quiz/${lessonId}?childId=${childId}`)}
                className="flex-1 py-4 bg-red-600 text-white font-black rounded-2xl hover:bg-red-700 transition-all"
              >
                Bëj kuizin →
              </button>
              <button
                onClick={() => {
                  setStory(null);
                  setShowTranslation(false);
                }}
                className="px-5 py-4 border-2 border-gray-200 text-gray-600 font-bold rounded-2xl hover:bg-gray-50"
              >
                ↻ E re
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
