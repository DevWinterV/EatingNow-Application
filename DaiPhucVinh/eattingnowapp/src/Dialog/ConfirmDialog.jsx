import { Dialog, HelpIcon, Pane, Strong, Text } from "evergreen-ui";
import * as React from "react";

export default function ConfirmDialog({
  isShown,
  onConfirm,
  onCancel,
  confirmLabel = "Đồng ý",
  cancelLabel = "Đóng",
  intent,
  width = 400,
  title,
  message,
}) {
  return (
    <Dialog
      isShown={isShown}
      title="Dialog xác nhận hủy phiếu"
      onConfirm={onConfirm}
      onCancel={onCancel}
      onCloseComplete={onCancel}
      hasHeader={false}
      confirmLabel={confirmLabel}
      intent={intent}
      cancelLabel={cancelLabel}
      width={width}
    >
      <Pane textAlign="center">
        <Pane>
          <HelpIcon size={40} color="warning" />
        </Pane>
        <Pane>
          <Strong size={500}>{title}</Strong>
        </Pane>
        <Pane>
          <Text size={400}>{message}</Text>
        </Pane>
      </Pane>
    </Dialog>
  );
}
