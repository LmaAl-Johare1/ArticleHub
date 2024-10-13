import React from 'react';
import './Pagination.css';

const Pagination = ({ currentPage, totalPages, onPageChange }) => {
  const handleNextPage = () => {
    if (currentPage < totalPages) {
      onPageChange(currentPage + 1);
    }
  };

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
    }
  };

  const renderPageButtons = () => {
    const buttons = [];
    
    // Show first page button
    if (totalPages > 0) {
      buttons.push(
        <button
          key={1}
          className={`pagination-btn ${1 === currentPage ? 'active' : ''}`}
          onClick={() => onPageChange(1)}
          aria-current={1 === currentPage ? 'page' : undefined}
        >
          1
        </button>
      );
    }

    // Add ellipsis if there are pages in between
    if (totalPages > 2 && currentPage > 3) {
      buttons.push(<span key="ellipsis-start">...</span>);
    }

    // Add page numbers around the current page
    for (let i = Math.max(2, currentPage - 1); i <= Math.min(totalPages - 1, currentPage + 1); i++) {
      buttons.push(
        <button
          key={i}
          className={`pagination-btn ${i === currentPage ? 'active' : ''}`}
          onClick={() => onPageChange(i)}
          aria-current={i === currentPage ? 'page' : undefined}
        >
          {i}
        </button>
      );
    }

    // Show last page button
    if (totalPages > 2 && currentPage < totalPages - 2) {
      buttons.push(<span key="ellipsis-end">...</span>);
      buttons.push(
        <button
          key={totalPages}
          className={`pagination-btn ${totalPages === currentPage ? 'active' : ''}`}
          onClick={() => onPageChange(totalPages)}
          aria-current={totalPages === currentPage ? 'page' : undefined}
        >
          {totalPages}
        </button>
      );
    }

    return buttons;
  };

  return (
    <div className="pagination-container">
      <button
        className="pagination-btn"
        onClick={handlePreviousPage}
        disabled={currentPage === 1}
        aria-label="Previous Page"
      >
        &laquo;
      </button>

      {renderPageButtons()}

      <button
        className="pagination-btn"
        onClick={handleNextPage}
        disabled={currentPage === totalPages}
        aria-label="Next Page"
      >
        &raquo;
      </button>
    </div>
  );
};

export default Pagination;
