export const PRIORITY_OPTIONS = [
  { value: 0, label: "Low", className: "low" },
  { value: 1, label: "Medium", className: "medium" },
  { value: 2, label: "High", className: "high" },
];

export const CATEGORY_OPTIONS = [
  { value: 0, label: "Work" },
  { value: 1, label: "Personal" },
  { value: 2, label: "Shopping" },
  { value: 3, label: "Health" },
  { value: 4, label: "Finance" },
  { value: 5, label: "Education" },
  { value: 6, label: "Entertainment" },
  { value: 7, label: "Travel" },
  { value: 8, label: "Other" },
];

export const PRIORITY_LABELS = PRIORITY_OPTIONS.map(({ label }) => label);
export const CATEGORY_LABELS = CATEGORY_OPTIONS.map(({ label }) => label);

const findOptionByValue = (options, value) =>
  options.find((option) => option.value === value);

export const getPriorityLabel = (priority) =>
  findOptionByValue(PRIORITY_OPTIONS, priority)?.label ?? "Unknown";

export const getPriorityClass = (priority) =>
  findOptionByValue(PRIORITY_OPTIONS, priority)?.className ?? "low";

export const getCategoryLabel = (category) =>
  findOptionByValue(CATEGORY_OPTIONS, category)?.label ?? "Unknown";
