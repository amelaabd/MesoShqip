import { apiClient } from "./client";
import type { StoryResponse } from "../types";

export const generateStory = async (data: {
  wordsToIntroduce: string[];
}): Promise<StoryResponse> => {
  const res = await apiClient.post<StoryResponse>(
    "/api/v1/ai/stories/generate",
    data,
  );
  return res.data;
};

export const getStories = async (): Promise<StoryResponse[]> => {
  const res = await apiClient.get<StoryResponse[]>("/api/v1/ai/stories");
  return res.data;
};
