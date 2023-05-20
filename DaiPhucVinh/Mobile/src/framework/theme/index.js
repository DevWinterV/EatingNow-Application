import React, { useContext } from "react";
import { StatusBar } from "react-native";
import { Toggle, ThemeContext } from "react-native-magnus";

import { PepoTheme } from "../../pepoTheme";

export default function ThemeSwitcher() {
  const { theme, setTheme } = useContext(ThemeContext);
  const onToggle = () => {
    if (theme.name === "dark") {
      setTheme(PepoTheme.light);
      StatusBar.setBarStyle("dark-content");
    } else {
      setTheme(PepoTheme.dark);
      StatusBar.setBarStyle("light-content");
    }
  };
  return <Toggle on={theme.name === "dark"} onPress={onToggle} />;
}
