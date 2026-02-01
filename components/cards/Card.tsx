import { useTheme } from "@/hooks/useTheme";
import React from "react";
import {
    Image,
    ImageSourcePropType,
    StyleSheet,
    Text,
    View,
} from "react-native";

export default function Card({
  imageUrl,
  icon,
  title,
  description,
}: {
  imageUrl?: ImageSourcePropType;
  icon?: React.ReactNode;
  title?: string;
  description?: string;
}) {
  const { colors } = useTheme();
  return (
    <View
      className="rounded-lg p-4 m-2 shadow-md"
      style={{ backgroundColor: colors.cardBorder }}
    >
      <View className="flex-col items-start mb-4">
        {icon ? (
          <View className="mr-4">{icon}</View>
        ) : imageUrl ? (
          <View className="bg-orange-300 rounded-[20px] p-2 mb-2 size-10">
            <Image source={imageUrl} className="w-6 h-6" />
          </View>
        ) : null}
        <Text
          className="text-xs font-semibold opacity-60"
          style={{
            color: colors.text,
          }}
        >
          {title || "Card Title"}
        </Text>
      </View>
      <Text
        className="text-gray-600 dark:text-gray-300 font-bold text-lg"
        style={{
          color: colors.text,
        }}
      >
        {description || "This is a description of the card."}
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({});
