import { useState } from "react";
import { CATEGORY_OPTIONS, PRIORITY_OPTIONS } from "../../constants/todoOptions";
import { createTodo } from "../../services/todoService";
import "./TodoForm.css";

const createEmptyTodo = () => ({
  title: "",
  description: "",
  priority: 0,
  category: 0,
});

function TodoForm({ onTodoCreated }) {
  const [formData, setFormData] = useState(createEmptyTodo);
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  const handleChange = (event) => {
    const { name, value } = event.target;
    const nextValue =
      name === "priority" || name === "category" ? Number(value) : value;

    setFormData((currentData) => ({
      ...currentData,
      [name]: nextValue,
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!formData.title.trim()) {
      setError("Title is required");
      return;
    }

    try {
      setSubmitting(true);
      await createTodo(formData);
      setFormData(createEmptyTodo());
      setError("");
      onTodoCreated();
    } catch (err) {
      console.error(err);
      setError("Failed to create todo");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="form-container">
      <h2>Create New Todo</h2>

      {error && (
        <div className="form-error" role="alert">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} aria-busy={submitting}>
        <div className="form-group">
          <label htmlFor="title">Title</label>
          <input
            id="title"
            type="text"
            name="title"
            placeholder="What needs to be done?"
            value={formData.title}
            onChange={handleChange}
            disabled={submitting}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            name="description"
            placeholder="Add details (optional)"
            value={formData.description}
            onChange={handleChange}
            disabled={submitting}
          />
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="priority">Priority</label>
            <select
              id="priority"
              name="priority"
              value={formData.priority}
              onChange={handleChange}
              disabled={submitting}
            >
              {PRIORITY_OPTIONS.map((priority) => (
                <option key={priority.value} value={priority.value}>
                  {priority.label}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label htmlFor="category">Category</label>
            <select
              id="category"
              name="category"
              value={formData.category}
              onChange={handleChange}
              disabled={submitting}
            >
              {CATEGORY_OPTIONS.map((category) => (
                <option key={category.value} value={category.value}>
                  {category.label}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="form-actions">
          <button type="submit" className="btn-submit" disabled={submitting}>
            {submitting ? "Adding..." : "Add Todo"}
          </button>
        </div>
      </form>
    </div>
  );
}

export default TodoForm;
