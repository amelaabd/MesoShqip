import { apiClient } from "./client";
import type { ProgressSummary } from "../types";

export const getProgressSummary = async (): Promise<ProgressSummary> => {
  const res = await apiClient.get<ProgressSummary>("/api/v1/progress/summary");
  return res.data;
};

export const updateProgress = async (data: {
  lessonId: string;
  scorePercent: number;
}) => {
  const res = await apiClient.post("/api/v1/progress/update", data);
  return res.data;
};

export const getBadges = async () => {
  const res = await apiClient.get("/api/v1/progress/badges");
  return res.data;
};

export const awardBadges = async () => {
  const res = await apiClient.post("/api/v1/progress/award-badges");
  return res.data;
};
