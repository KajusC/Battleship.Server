import { icons } from "@/assets/icons";
import { useTheme } from "@/hooks/useTheme";
import { PlatformPressable } from "@react-navigation/elements";
import React, { useEffect } from "react";
import Animated, {
  useAnimatedStyle,
  useSharedValue,
  withRepeat,
  withSequence,
  withSpring,
  withTiming,
} from "react-native-reanimated";

interface CustomTabBarButtonProps {
  href: string;
  onPress: () => void;
  onLongPress: () => void;
  routeName: any;
  label: string;
}

export default function CustomTabBarButtonScan({
  href,
  onPress,
  onLongPress,
  routeName,
  label,
}: CustomTabBarButtonProps) {
  const { colors } = useTheme();
  const scale = useSharedValue(1);
  const pulse = useSharedValue(1);

  // Subtle pulse animation
  useEffect(() => {
    pulse.value = withRepeat(
      withSequence(
        withTiming(1.05, { duration: 1000 }),
        withTiming(1, { duration: 1000 }),
      ),
      -1,
      true,
    );
  }, []);

  const animatedStyle = useAnimatedStyle(() => ({
    transform: [{ scale: scale.value * pulse.value }],
  }));

  const handlePressIn = () => {
    scale.value = withSpring(0.9, { damping: 15, stiffness: 300 });
  };

  const handlePressOut = () => {
    scale.value = withSpring(1, { damping: 15, stiffness: 300 });
  };

  return (
    <PlatformPressable
      href={href}
      onPress={onPress}
      onLongPress={onLongPress}
      onPressIn={handlePressIn}
      onPressOut={handlePressOut}
      className="w-[60px] h-[60px] -mt-10 items-center justify-center"
    >
      <Animated.View
        className="w-14 h-14 rounded-full items-center justify-center"
        style={[
          {
            backgroundColor: colors.fabInner,
            shadowColor: colors.fabInner,
            shadowOffset: { width: 0, height: 4 },
            shadowOpacity: 0.3,
            shadowRadius: 5,
            elevation: 6,
          },
          animatedStyle,
        ]}
      >
        {icons.Scan({ color: colors.buttonText })}
      </Animated.View>
    </PlatformPressable>
  );
}
