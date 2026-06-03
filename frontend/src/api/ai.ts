import { apiClient } from "./client";
import type { StoryResponse, QuizQuestion } from "../types";

export const generateStory = async (data: {
  childProfileId: string;
  wordsToIntroduce: string[];
}): Promise<StoryResponse> => {
  const res = await apiClient.post<StoryResponse>(
    "/api/v1/ai/stories/generate",
    data,
  );
  return res.data;
};

export const generateQuiz = async (data: {
  childProfileId: string;
  weakWords: string[];
}): Promise<{ sessionId: string; questions: QuizQuestion[] }> => {
  const res = await apiClient.post("/api/v1/ai/quizzes/generate", data);
  return res.data;
};
