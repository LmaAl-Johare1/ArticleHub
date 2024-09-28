import React, { useState } from 'react';
import './Pagination.css'; // Import custom CSS for styling

const Pagination = () => {
  // Static data for current page and total pages
  const [currentPage, setCurrentPage] = useState(1);
  const totalPages = 5; // Set total pages statically for now

  const pageNumbers = [];
  for (let i = 1; i <= totalPages; i++) {
    pageNumbers.push(i);
  }

  // Static page change function
  const handlePageChange = (number) => {
    setCurrentPage(number);
  };

  return (
    <div className="pagination-container">
      {pageNumbers.map((number) => (
        <button
          key={number}
          className={`pagination-btn ${number === currentPage ? 'active' : ''}`}
          onClick={() => handlePageChange(number)}
        >
          {number}
        </button>
      ))}
      <button
        className="pagination-btn"
        onClick={() => handlePageChange(currentPage + 1 <= totalPages ? currentPage + 1 : totalPages)}
      >
        &raquo;
      </button>
    </div>
  );
};

export default Pagination;
