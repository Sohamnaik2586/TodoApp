import { useState } from "react";
import FilterBar from "../FilterBar/FilterBar";
import SearchBar from "../SearchBar/SearchBar";
import { CATEGORY_LABELS, PRIORITY_LABELS } from "../../constants/todoOptions";
import "./Navbar.css";

function Navbar({ onSearch, onCategoryFilter, onPriorityFilter, onShowAll }) {
  const [keyword, setKeyword] = useState("");

  const handleSearch = () => {
    onSearch(keyword);
  };

  return (
    <div className="navbar">
      <div className="navbar-left">
        <h1>Todo App</h1>
      </div>

      <SearchBar
        keyword={keyword}
        onKeywordChange={setKeyword}
        onSearch={handleSearch}
      />

      <FilterBar
        onShowAll={onShowAll}
        onCategoryFilter={onCategoryFilter}
        onPriorityFilter={onPriorityFilter}
        categoryOptions={CATEGORY_LABELS}
        priorityOptions={PRIORITY_LABELS}
      />
    </div>
  );
}

export default Navbar;
