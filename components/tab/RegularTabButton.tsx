import { icons } from "@/assets/icons";
import { useTheme } from "@/hooks/useTheme";
import { Feather } from "@expo/vector-icons";
import { PlatformPressable } from "@react-navigation/elements";
import { useEffect } from "react";
import Animated, {
    Easing,
    useAnimatedStyle,
    useSharedValue,
    withTiming,
} from "react-native-reanimated";

interface RegularTabButtonProps {
  route: any;
  href: string;
  isFocused: boolean;
  label: string;
  onPress: () => void;
  onLongPress: () => void;
  accessibilityState: any;
  accessibilityLabel?: string;
  testID?: string;
}

export default function RegularTabButton({
  route,
  href,
  isFocused,
  label,
  onPress,
  onLongPress,
  accessibilityState,
  accessibilityLabel,
  testID,
}: RegularTabButtonProps) {
  const { colors } = useTheme();
  const scale = useSharedValue(1);
  const iconScale = useSharedValue(isFocused ? 1 : 0.9);

  useEffect(() => {
    iconScale.value = withTiming(isFocused ? 1 : 0.9, {
      duration: 200,
      easing: Easing.out(Easing.ease),
    });
  }, [isFocused]);

  const animatedContainerStyle = useAnimatedStyle(() => ({
    transform: [{ scale: scale.value }],
  }));

  const animatedIconStyle = useAnimatedStyle(() => ({
    transform: [{ scale: iconScale.value }],
  }));

  const handlePressIn = () => {
    scale.value = withTiming(0.85, {
      duration: 100,
      easing: Easing.out(Easing.ease),
    });
  };

  const handlePressOut = () => {
    scale.value = withTiming(1, {
      duration: 150,
      easing: Easing.out(Easing.ease),
    });
  };

  return (
    <PlatformPressable
      href={href}
      accessibilityState={accessibilityState}
      accessibilityLabel={accessibilityLabel}
      testID={testID}
      onPress={onPress}
      onLongPress={onLongPress}
      onPressIn={handlePressIn}
      onPressOut={handlePressOut}
      className="flex-1 items-center justify-center h-[50px]"
    >
      <Animated.View
        style={[
          animatedContainerStyle,
          { alignItems: "center", justifyContent: "center" },
        ]}
      >
        <Animated.View
          style={animatedIconStyle}
          className="items-center justify-center"
        >
          {icons[route.name as keyof typeof icons] ? (
            icons[route.name as keyof typeof icons]({
              color: isFocused ? colors.primary : colors.inactive,
              style: { opacity: isFocused ? 1 : 0.6 },
            })
          ) : (
            <Feather name="help-circle" size={24} color={colors.primary} />
          )}
        </Animated.View>
        <Animated.Text
          className="text-[10px] mt-0.5 font-semibold text-center"
          style={{
            color: isFocused ? colors.primary : colors.inactive,
            opacity: isFocused ? 1 : 0,
          }}
        >
          {label}
        </Animated.Text>
      </Animated.View>
    </PlatformPressable>
  );
}
