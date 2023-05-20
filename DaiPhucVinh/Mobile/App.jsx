// import "react-native-gesture-handler";
import * as React from "react";
import { ThemeProvider } from "react-native-magnus";
import { PepoTheme } from "./src/pepoTheme";
import Skeleton from "./src/Skeleton";
//import { Appearance, Platform } from "react-native";
//import { Audio } from "expo-av";
import * as Font from "expo-font";

let customFonts = {
  NunitoSansBold: require("./assets/fonts/NunitoSans-Bold.ttf"),
  NunitoSansBoldItalic: require("./assets/fonts/NunitoSans-BoldItalic.ttf"),
  NunitoSansExtraBold: require("./assets/fonts/NunitoSans-ExtraBold.ttf"),
  NunitoSansItalic: require("./assets/fonts/NunitoSans-Italic.ttf"),
  NunitoSansLight: require("./assets/fonts/NunitoSans-Light.ttf"),
  NunitoSansRegular: require("./assets/fonts/NunitoSans-Regular.ttf"),
  NunitoSansSemiBold: require("./assets/fonts/NunitoSans-SemiBold.ttf"),
  DMSerifText: require("./assets/fonts/DMSerifText-Regular.ttf"),
  GreatVibes: require("./assets/fonts/GreatVibes-Regular.ttf"),
};

export default function App() {
  const [appLoaded, setAppLoaded] = React.useState(false);
  const [isDarkMode, setDarkMode] = React.useState(false);

  // const [sound, setSound] = React.useState();
  // React.useEffect(() => {
  //   return sound
  //     ? () => {
  //         sound.unloadAsync();
  //       }
  //     : undefined;
  // }, [sound]);
  // async function playSound() {
  //   const { sound } = await Audio.Sound.createAsync(require("./assets/sounds/audi.mp3"));
  //   setSound(sound);
  //   await sound.playAsync();
  //   setTimeout(async function () {
  //     await sound.stopAsync();
  //   }, 3000);
  // }

  React.useEffect(() => {
    async function ApplicationInit() {
      await Font.loadAsync(customFonts);
      //const colorScheme = Appearance.getColorScheme();
      setDarkMode("light");
    }
    ApplicationInit().then(() => {
      setAppLoaded(true);
      //playSound();
    });
  }, []);

  const PepoApp = () => (
    <ThemeProvider theme={isDarkMode === "dark" ? PepoTheme.dark : PepoTheme.light}>
      <Skeleton />
    </ThemeProvider>
  );
  return appLoaded && <PepoApp />;
}
