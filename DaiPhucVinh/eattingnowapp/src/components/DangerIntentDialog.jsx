import React, { useState } from "react";
import { Pane, Dialog, Button } from "evergreen-ui";

function CustomDialog(props) {
  const { title, content, intent, confirmLabel, showButton = true } = props;
  const [isShown, setIsShown] = useState(false);

  const handleCloseDialog = () => {
    setIsShown(false);
  };

  const handleShowDialog = () => {
    setIsShown(true);
  };

  return (
    <Pane>
      <Dialog
        width={400}
        isShown={isShown}
        title={title}
        intent={intent}
        onCloseComplete={handleCloseDialog}
        confirmLabel={confirmLabel}
      >
        {content}
      </Dialog>

      {showButton ? (
        <Button onClick={handleShowDialog}>Đăng Xuất</Button>
      ) : null}
    </Pane>
  );
}

export default CustomDialog;
