import * as React from "react";
import { Modal } from "react-bootstrap";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";

export default function DialogControl({ content, title, isOpen, onClose }) {
  return (
    <div className="table-rep-plugin">
      <div className="table-responsive mb-0" data-pattern="priority-columns">
        <Modal
          size="xl"
          show={isOpen}
          onEscapeKeyDown={() => onClose(false)}
          backdrop="static"
          keyboard={false}
          className="fade"
          tabIndex="-1"
          role="dialog"
          aria-labelledby="staticBackdropLabel"
          aria-hidden="true"
          centered
          style={{ fontSize: "12px" }}
        >
          <Modal.Header>
            <p
              style={{ fontSize: "12px" }}
              className="modal-title fw-bold"
              id="staticBackdropLabel"
            >
              {title}
            </p>
            <button
              type="button"
              className="btn-close"
              onClick={() => {
                onClose(false);
              }}
            ></button>
          </Modal.Header>
          <Modal.Body>
            <div className="row">
              <div className="col-lg-12">
                <div className="mb-3">
                  <label className="form-label fw-bold">{title}</label>
                  {content}
                </div>
              </div>
            </div>
          </Modal.Body>
          <Modal.Footer>
            <button
              type="button"
              className="btn btn-success"
              //onClick={onSaveCustomer}
              style={{ fontSize: "12px" }}
            >
              Đồng ý
            </button>
            <button
              type="button"
              className="btn btn-warning"
              onClick={() => {
                onClose(false);
              }}
              style={{ fontSize: "12px" }}
            >
              Hủy
            </button>
          </Modal.Footer>
        </Modal>
      </div>
    </div>
  );
}
