import { apiClient } from "./client";
import type { ProgressSummary } from "../types";

export const getProgressSummary = async (
  childId: string,
): Promise<ProgressSummary> => {
  const res = await apiClient.get<ProgressSummary>(
    `/api/v1/progress/${childId}/summary`,
  );
  return res.data;
};

export const updateProgress = async (data: {
  childProfileId: string;
  lessonId: string;
  scorePercent: number;
}) => {
  const res = await apiClient.post("/api/v1/progress/update", data);
  return res.data;
};
