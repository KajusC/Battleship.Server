import { useTheme } from "@/contexts/ThemeContext";
import React from "react";
import { Switch, Text, View } from "react-native";

export default function Profile() {
  const { theme, colors, toggleTheme, setThemeMode } = useTheme();

  return (
    <View
      className="flex-1 items-center justify-center"
      style={{ backgroundColor: colors.background }}
    >
      <Text className="text-foreground text-xl font-bold mb-4">Profile</Text>
      <View className="flex-row items-center">
        <Text className="text-foreground mr-2">Dark Mode</Text>
        <Switch value={theme === "dark"} onValueChange={toggleTheme} />
      </View>
    </View>
  );
}
