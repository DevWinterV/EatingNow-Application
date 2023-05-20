import * as React from "react";
import ReactPaginate from "react-paginate";
export default function Paginate({
  onPageChange,
  pageTotal,
  pageRangeDisplayed,
}) {
  return (
    <ReactPaginate
      className="pagination pagination-rounded justify-content-center mt-4"
      disabledClassName="disabled"
      previousClassName="page-item"
      previousLinkClassName="page-link"
      nextClassName="page-item"
      nextLinkClassName="page-link"
      pageLinkClassName="page-link"
      pageClassName="page-item"
      activeClassName="active"
      breakLabel="..."
      nextLabel=">"
      onPageChange={onPageChange}
      pageRangeDisplayed={pageRangeDisplayed || 5}
      pageCount={pageTotal}
      previousLabel="<"
      renderOnZeroPageCount={null}
    />
  );
}
