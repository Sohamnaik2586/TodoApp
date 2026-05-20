import axios from "axios";

const todoClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "http://localhost:5285/api/todo",
});

const unwrapData = (response) => response.data;

export const getTodos = async () => {
  const response = await todoClient.get("");

  return unwrapData(response);
};

export const createTodo = async (todoData) => {
  const response = await todoClient.post("", todoData);

  return unwrapData(response);
};

export const deleteTodo = async (id) => {
  const response = await todoClient.delete(`/${id}`);

  return unwrapData(response);
};

export const updateTodo = async (id, todoData) => {
  const response = await todoClient.put(`/${id}`, todoData);

  return unwrapData(response);
};

export const searchTodos = async (keyword) => {
  const response = await todoClient.get("/search", {
    params: { keyword },
  });

  return unwrapData(response);
};

export const getTodosByCategory = async (category) => {
  const response = await todoClient.get(`/category/${category}`);

  return unwrapData(response);
};

export const getTodosByPriority = async (priority) => {
  const response = await todoClient.get(`/priority/${priority}`);

  return unwrapData(response);
};
