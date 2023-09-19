import React from "react";
import { AnimatePresence } from "framer-motion";
import { StateProvider } from "./context/StateProvider";
import { initialState } from "./context/initialState";
import reducer from "./context/reducer";
import { Routers } from "./Pages";

const App = () => {
  return (
    <StateProvider initialState={initialState} reducer={reducer}>
      <AnimatePresence>
        <Routers />
       
      </AnimatePresence>
    </StateProvider>
    
  );
};

export default App;
