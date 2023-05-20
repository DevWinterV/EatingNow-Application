import * as React from "react";
import { Div } from "react-native-magnus";
export function Layout({ children }) {
  return (
    <Div bg="body" flex={1}>
      {children}
    </Div>
  );
}
