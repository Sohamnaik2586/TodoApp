import { useState } from "react";
import "./FilterBar.css";

function FilterBar({
  onCategoryFilter,
  onPriorityFilter,
  onShowAll,
  categoryOptions,
  priorityOptions,
}) {
  const [openMenu, setOpenMenu] = useState(null);
  const isCategoryOpen = openMenu === "category";
  const isPriorityOpen = openMenu === "priority";

  const toggleMenu = (menu) => {
    setOpenMenu((currentMenu) => (currentMenu === menu ? null : menu));
  };

  const handleCategorySelect = (category) => {
    onCategoryFilter(category);
    setOpenMenu(null);
  };

  const handlePrioritySelect = (priority) => {
    onPriorityFilter(priority);
    setOpenMenu(null);
  };

  const handleShowAll = () => {
    onShowAll();
    setOpenMenu(null);
  };

  return (
    <div className="navbar-right">
      <button type="button" className="filter-btn" onClick={handleShowAll}>
        All
      </button>

      <div className="dropdown-container">
        <button
          type="button"
          className="filter-btn dropdown-btn"
          onClick={() => toggleMenu("category")}
          aria-controls="category-filter-menu"
          aria-expanded={isCategoryOpen}
          aria-haspopup="menu"
        >
          Category
        </button>
        {isCategoryOpen && (
          <div className="dropdown-menu" id="category-filter-menu" role="menu">
            {categoryOptions.map((cat) => (
              <button
                key={cat}
                type="button"
                className="dropdown-item"
                onClick={() => handleCategorySelect(cat)}
                role="menuitem"
              >
                {cat}
              </button>
            ))}
          </div>
        )}
      </div>

      <div className="dropdown-container">
        <button
          type="button"
          className="filter-btn dropdown-btn"
          onClick={() => toggleMenu("priority")}
          aria-controls="priority-filter-menu"
          aria-expanded={isPriorityOpen}
          aria-haspopup="menu"
        >
          Priority
        </button>
        {isPriorityOpen && (
          <div className="dropdown-menu" id="priority-filter-menu" role="menu">
            {priorityOptions.map((pri) => (
              <button
                key={pri}
                type="button"
                className="dropdown-item"
                onClick={() => handlePrioritySelect(pri)}
                role="menuitem"
              >
                {pri}
              </button>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default FilterBar;
