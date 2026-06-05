import { apiClient } from "./client";
import type { StoryResponse } from "../types";

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

export const getStories = async (childId: string): Promise<StoryResponse[]> => {
  const res = await apiClient.get<StoryResponse[]>(
    `/api/v1/ai/stories/${childId}`,
  );
  return res.data;
};
