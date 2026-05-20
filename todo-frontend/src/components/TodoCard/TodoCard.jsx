import { useEffect, useState } from "react";
import {
  deleteTodo,
  updateTodo,
} from "../../services/todoService";
import {
  getCategoryLabel,
  getPriorityClass,
  getPriorityLabel,
} from "../../constants/todoOptions";
import "./TodoCard.css";

function TodoCard({
  todo,
  onTodoUpdated,
}) {
  const [pendingAction, setPendingAction] = useState(null);
  const [mutationError, setMutationError] = useState("");
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const isMutating = pendingAction !== null;

  useEffect(() => {
    if (!deleteDialogOpen) {
      return undefined;
    }

    const handleKeyDown = (event) => {
      if (event.key === "Escape" && !isMutating) {
        setDeleteDialogOpen(false);
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [deleteDialogOpen, isMutating]);

  const handleDelete = async () => {
    try {
      setMutationError("");
      setPendingAction("delete");
      await deleteTodo(todo.id);
      setDeleteDialogOpen(false);
      onTodoUpdated();
    } catch (err) {
      console.error(err);
      setMutationError("Failed to delete todo");
    } finally {
      setPendingAction(null);
    }
  };

  const handleToggleComplete = async () => {
    try {
      setMutationError("");
      setPendingAction("toggle");
      await updateTodo(todo.id, {
        ...todo,
        isCompleted: !todo.isCompleted,
      });
      onTodoUpdated();
    } catch (err) {
      console.error(err);
      setMutationError("Failed to update todo");
    } finally {
      setPendingAction(null);
    }
  };

  const priorityLabel = getPriorityLabel(todo.priority);
  const categoryLabel = getCategoryLabel(todo.category);
  const priorityClass = getPriorityClass(todo.priority);

  return (
    <>
      <div className={`todo-card ${todo.isCompleted ? "completed" : ""} fade-in`}>
        <div className="todo-header">
          <h3 className="todo-title">
            {todo.title}
          </h3>
        </div>

        <div className="todo-badges">
          <span className={`badge badge-priority ${priorityClass}`}>
            {priorityLabel}
          </span>
          <span className="badge badge-category">
            {categoryLabel}
          </span>
          {todo.isCompleted && (
            <span className="badge badge-status">
              Completed
            </span>
          )}
        </div>

        {todo.description && (
          <p className="todo-description">
            {todo.description}
          </p>
        )}

        {mutationError && (
          <div className="todo-error" role="alert">
            {mutationError}
          </div>
        )}

        <div className="todo-actions">
          <button
            type="button"
            className="btn-action btn-complete"
            onClick={handleToggleComplete}
            title={todo.isCompleted ? "Mark as pending" : "Mark as complete"}
            disabled={isMutating}
          >
            {pendingAction === "toggle"
              ? "Saving..."
              : todo.isCompleted
                ? "Pending"
                : "Complete"}
          </button>

          <button
            type="button"
            className="btn-action btn-delete"
            onClick={() => {
              setMutationError("");
              setDeleteDialogOpen(true);
            }}
            title="Delete this todo"
            disabled={isMutating}
          >
            Delete
          </button>
        </div>
      </div>

      {deleteDialogOpen && (
        <div
          className="delete-dialog-backdrop"
          onMouseDown={(event) => {
            if (event.target === event.currentTarget && !isMutating) {
              setDeleteDialogOpen(false);
            }
          }}
        >
          <div
            className="delete-dialog"
            role="dialog"
            aria-modal="true"
            aria-labelledby={`delete-title-${todo.id}`}
          >
            <h2 id={`delete-title-${todo.id}`}>Delete todo?</h2>
            <p>
              This will permanently remove <strong>{todo.title}</strong>.
            </p>

            {mutationError && (
              <div className="todo-error" role="alert">
                {mutationError}
              </div>
            )}

            <div className="delete-dialog-actions">
              <button
                type="button"
                className="dialog-btn dialog-btn-secondary"
                onClick={() => setDeleteDialogOpen(false)}
                disabled={isMutating}
                autoFocus
              >
                Cancel
              </button>
              <button
                type="button"
                className="dialog-btn dialog-btn-danger"
                onClick={handleDelete}
                disabled={isMutating}
              >
                {pendingAction === "delete" ? "Deleting..." : "Delete"}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default TodoCard;
