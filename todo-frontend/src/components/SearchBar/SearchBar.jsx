import "./SearchBar.css";

function SearchBar({ keyword, onKeywordChange, onSearch }) {
  const handleSubmit = (event) => {
    event.preventDefault();
    onSearch();
  };

  return (
    <form className="navbar-center" onSubmit={handleSubmit}>
      <input
        type="text"
        aria-label="Search todos"
        placeholder="Search todos..."
        value={keyword}
        onChange={(event) => onKeywordChange(event.target.value)}
      />
      <button type="submit">Search</button>
    </form>
  );
}

export default SearchBar;
