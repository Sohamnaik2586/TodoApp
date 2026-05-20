import { useEffect, useRef, useState } from "react";

import Navbar from "../components/Navbar/Navbar";
import TodoForm from "../components/TodoForm/TodoForm";
import TodoCard from "../components/TodoCard/TodoCard";

import {
  getTodos,
  searchTodos,
  getTodosByCategory,
  getTodosByPriority,
} from "../services/todoService";

import "../styles/layout.css";

const ALL_TODOS_VIEW = { type: "all", value: "" };

const isSameView = (currentView, nextView) =>
  currentView.type === nextView.type && currentView.value === nextView.value;

const getRequestForView = ({ type, value }) => {
  switch (type) {
    case "search":
      return {
        request: () => searchTodos(value),
        errorMessage: "Failed to search todos",
      };
    case "category":
      return {
        request: () => getTodosByCategory(value),
        errorMessage: "Failed to filter todos",
      };
    case "priority":
      return {
        request: () => getTodosByPriority(value),
        errorMessage: "Failed to filter todos",
      };
    default:
      return {
        request: getTodos,
        errorMessage: "Failed to fetch todos",
      };
  }
};

function TodoPage() {
  const [todos, setTodos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [activeView, setActiveView] = useState(ALL_TODOS_VIEW);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const latestRequestId = useRef(0);

  const markPendingRequestsStale = () => {
    latestRequestId.current += 1;
  };

  const showView = (nextView) => {
    markPendingRequestsStale();
    setLoading(true);

    if (isSameView(activeView, nextView)) {
      setRefreshCounter((currentCount) => currentCount + 1);
      return;
    }

    setActiveView(nextView);
  };

  const refreshActiveView = () => {
    markPendingRequestsStale();
    setLoading(true);
    setRefreshCounter((currentCount) => currentCount + 1);
  };

  const handleSearch = (keyword) => {
    const query = keyword.trim();

    showView(
      query
        ? { type: "search", value: query }
        : ALL_TODOS_VIEW
    );
  };

  const handleCategoryFilter = (category) => {
    showView({ type: "category", value: category });
  };

  const handlePriorityFilter = (priority) => {
    showView({ type: "priority", value: priority });
  };

  useEffect(() => {
    let isActive = true;
    const requestId = latestRequestId.current + 1;
    latestRequestId.current = requestId;

    const fetchTodos = async () => {
      const { request, errorMessage } = getRequestForView(activeView);

      try {
        const data = await request();

        if (isActive && requestId === latestRequestId.current) {
          setError("");
          setTodos(data);
        }
      } catch (err) {
        console.error(err);

        if (isActive && requestId === latestRequestId.current) {
          setError(errorMessage);
        }
      } finally {
        if (isActive && requestId === latestRequestId.current) {
          setLoading(false);
        }
      }
    };

    fetchTodos();

    return () => {
      isActive = false;
    };
  }, [activeView, refreshCounter]);

  return (
    <div>
      <Navbar
        onSearch={handleSearch}
        onCategoryFilter={handleCategoryFilter}
        onPriorityFilter={handlePriorityFilter}
        onShowAll={() => showView(ALL_TODOS_VIEW)}
      />

      <div className="todo-layout">
        <aside className="todo-sidebar">
          <TodoForm onTodoCreated={refreshActiveView} />
        </aside>

        <main className="todo-main" aria-busy={loading}>
          {error && (
            <div className="error-state" role="alert">
              <strong>Error:</strong> {error}
            </div>
          )}

          {loading ? (
            <div className="loading-state" role="status">
              Loading todos...
            </div>
          ) : todos.length === 0 ? (
            <div className="empty-state">
              <p>No todos found. Create one to get started!</p>
            </div>
          ) : (
            <div className="todos-grid">
              {todos.map((todo) => (
                <TodoCard
                  key={todo.id}
                  todo={todo}
                  onTodoUpdated={refreshActiveView}
                />
              ))}
            </div>
          )}
        </main>
      </div>
    </div>
  );
}

export default TodoPage;
