import { createContext, useContext, useState } from "react";

const TopbarContext = createContext(null);

export function TopbarProvider({ children }) {
  const [actions, setActions] = useState(null);

  return (
    <TopbarContext.Provider value={{ actions, setActions }}>
      {children}
    </TopbarContext.Provider>
  );
}

export function useTopbar() {
  const ctx = useContext(TopbarContext);
  if (!ctx) {
    throw new Error("useTopbar must be used inside TopbarProvider");
  }
  return ctx;
}
