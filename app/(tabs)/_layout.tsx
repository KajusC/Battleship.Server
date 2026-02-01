import CustomTabBar from "@/components/tab/CustomTabBar";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { Tabs } from "expo-router";

const Tab = createBottomTabNavigator();

export default function _layout() {
  return (
    <Tabs
      tabBar={(props) => <CustomTabBar {...props} />}
      screenOptions={{
        headerShown: false,
        animation: "fade",
      }}
    >
      <Tabs.Screen name="Index" options={{ title: "Home" }} />
      <Tabs.Screen name="Insights" options={{ title: "Insights" }} />
      <Tabs.Screen name="Scan" options={{ title: "Scan" }} />
      <Tabs.Screen name="Search" options={{ title: "Search" }} />
      <Tabs.Screen name="Profile" options={{ title: "Profile" }} />
    </Tabs>
  );
}
