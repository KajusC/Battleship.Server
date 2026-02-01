import React, { createContext, useContext, useMemo, useState } from "react";
import { useColorScheme as useNativeColorScheme } from "react-native";

export type ThemeMode = "light" | "dark" | "auto";
export type ActiveTheme = "light" | "dark";

interface NavigationTheme {
  dark: boolean;
  colors: {
    primary: string;
    background: string;
    card: string;
    text: string;
    border: string;
    notification: string;
  };
  fonts: {
    regular: { fontFamily: string; fontWeight: string };
    medium: { fontFamily: string; fontWeight: string };
    bold: { fontFamily: string; fontWeight: string };
    heavy: { fontFamily: string; fontWeight: string };
  };
}

interface ThemeContextType {
  theme: ActiveTheme;
  themeMode: ThemeMode;
  setThemeMode: (mode: ThemeMode) => void;
  toggleTheme: () => void;
  colors: typeof import("../constants/Colors").Colors.light;
  navigationTheme: NavigationTheme;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const systemColorScheme = useNativeColorScheme();
  const [themeMode, setThemeModeState] = useState<ThemeMode>("auto");

  const activeTheme: ActiveTheme =
    themeMode === "auto"
      ? systemColorScheme === "dark"
        ? "dark"
        : "light"
      : themeMode;

  const setThemeMode = (mode: ThemeMode) => {
    setThemeModeState(mode);
  };

  const toggleTheme = () => {
    const nextTheme: ThemeMode = activeTheme === "light" ? "dark" : "light";
    setThemeMode(nextTheme);
  };

  const Colors = require("../constants/Colors").Colors;
  const colors = Colors[activeTheme];

  const navigationTheme = useMemo(
    () => ({
      dark: activeTheme === "dark",
      colors: {
        primary: colors.primary,
        background: colors.background,
        card: colors.card,
        text: colors.text,
        border: colors.border,
        notification: colors.primary,
        ...colors,
      },
      fonts: {
        regular: { fontFamily: "System", fontWeight: "400" },
        medium: { fontFamily: "System", fontWeight: "500" },
        bold: { fontFamily: "System", fontWeight: "600" },
        heavy: { fontFamily: "System", fontWeight: "700" },
      },
    }),
    [activeTheme, colors],
  );

  return (
    <ThemeContext.Provider
      value={{
        theme: activeTheme,
        themeMode,
        setThemeMode,
        toggleTheme,
        colors,
        navigationTheme,
      }}
    >
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  const context = useContext(ThemeContext);
  if (context === undefined) {
    throw new Error("useTheme must be used within a ThemeProvider");
  }
  return context;
}
