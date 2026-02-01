import { useTheme } from "@/hooks/useTheme";
import { useLinkBuilder } from "@react-navigation/native";
import { View } from "react-native";
import CustomTabBarButtonScan from "./CustomTabBarButtonScan";
import RegularTabButton from "./RegularTabButton";

export default function CustomTabBar({
  state,
  descriptors,
  navigation,
}: {
  state: any;
  descriptors: any;
  navigation: any;
}) {
  const { colors } = useTheme();
  const { buildHref } = useLinkBuilder();

  return (
    <View className="absolute bottom-10 left-5 right-5 items-center">
      <View
        className="flex-row justify-between items-center px-[15px] py-[10px] rounded-[40px] w-full"
        style={{
          backgroundColor: colors.tabBarBackground,
          borderWidth: 1,
          borderColor: colors.tabBarBorder,
          shadowColor: colors.shadowColor,
          shadowOffset: { width: 0, height: 5 },
          shadowRadius: 10,
          shadowOpacity: 0.1,
          elevation: 5,
        }}
      >
        {state.routes.map((route: any, index: number) => {
          const { options } = descriptors[route.key];
          const label =
            options.tabBarLabel !== undefined
              ? options.tabBarLabel
              : options.title !== undefined
                ? options.title
                : route.name;

          const isFocused = state.index === index;
          const isScan = route.name === "Scan";

          const onPress = () => {
            const event = navigation.emit({
              type: "tabPress",
              target: route.key,
              canPreventDefault: true,
            });

            if (!isFocused && !event.defaultPrevented) {
              navigation.navigate(route.name, route.params);
            }
          };

          const onLongPress = () => {
            navigation.emit({
              type: "tabLongPress",
              target: route.key,
            });
          };

          if (isScan) {
            return (
              <CustomTabBarButtonScan
                key={route.name}
                href={buildHref(route.name, route.params)}
                onPress={onPress}
                onLongPress={onLongPress}
                routeName={route}
                label={label}
              />
            );
          }

          return (
            <RegularTabButton
              key={route.name}
              route={route}
              href={buildHref(route.name, route.params)}
              isFocused={isFocused}
              label={label}
              onPress={onPress}
              onLongPress={onLongPress}
              accessibilityState={isFocused ? { selected: true } : {}}
              accessibilityLabel={options.tabBarAccessibilityLabel}
              testID={options.tabBarButtonTestID}
            />
          );
        })}
      </View>
    </View>
  );
}
