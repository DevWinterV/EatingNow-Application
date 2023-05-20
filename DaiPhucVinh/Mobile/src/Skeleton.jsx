import * as React from "react";
import SplashScreen from "./SplashScreen";
import AnimatedNavigation from "./animatedNavigation";
import { AuthContext } from "./framework/context";
import { Layout } from "./screens/layout";
import NetInfo from "@react-native-community/netinfo";
import { AppInit } from "./api/auth";

export default function Skeleton() {
  const [loading, setLoading] = React.useState(true);
  const [role, setRole] = React.useState();
  const [user, setUser] = React.useState();
  const value = React.useMemo(
    () => ({
      role,
      setRole,
      user,
      setUser,
    }),
    [role]
  );
  async function onViewAppearing() {
    const unsubscribe = NetInfo.addEventListener((internet) => {
      AppInit(false);
      // AppInit(internet?.isConnected);
    });
    unsubscribe();
  }
  React.useEffect(() => {
    onViewAppearing();
    setTimeout(() => {
      setLoading(false);
    }, 2500);
  }, []);
  return loading ? (
    <SplashScreen />
  ) : (
    <AuthContext.Provider value={value}>
      <Layout>
        <AnimatedNavigation />
      </Layout>
    </AuthContext.Provider>
  );
}
