import * as React from "react";
export default function Breadcrumb({ title, sources }) {
  return (
    <div className="row">
      <div className="col-12">
        <div
          className="page-title-box d-sm-flex align-items-center justify-content-between"
          style={{
            paddingBottom: "13px",
          }}
        >
          <span
            className="mb-sm-0 "
            style={{ fontSize: "12px", fontWeight: "bold" }}
          >
            {title}
          </span>
          {sources && sources.length > 0 && (
            <div className="page-title-right" style={{ fontSize: "12px" }}>
              <ol className="breadcrumb m-0">
                {sources.map((brc, idx) => (
                  <li
                    className={
                      "breadcrumb-item" + (brc?.active ? " active" : "")
                    }
                    key={idx}
                  >
                    {brc?.active ? (
                      brc.name
                    ) : (
                      <a href={brc.href || "#"}>{brc.name}</a>
                    )}
                  </li>
                ))}
              </ol>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
