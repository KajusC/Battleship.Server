import { useTheme } from "@/contexts/ThemeContext";
import React from "react";
import { StyleSheet, Text, View } from "react-native";
import Svg, { Path } from "react-native-svg";

interface GraphCardProps {
  title?: string;
  currentAmount?: number;
  lastMonthAmount?: number;
  percentageChange?: number;
}

export default function GraphCard({
  title = "This Month's Spend",
  currentAmount = 428.5,
  lastMonthAmount = 382.0,
  percentageChange = 12,
}: GraphCardProps) {
  const isPositive = percentageChange >= 0;
  const { colors } = useTheme();

  return (
    <View
      className="rounded-[40px] p-6 w-full max-w-sm shadow-lg"
      style={{ backgroundColor: colors.statisticsCardBackground }}
    >
      {/* Header */}
      <View className="flex-row justify-between items-center mb-2">
        <Text className="text-white text-base font-medium">{title}</Text>
        <View className="bg-white/20 rounded-full px-3 py-1 flex-row items-center">
          <Text className="text-white text-xs mr-1">
            {isPositive ? "↗" : "↘"}
          </Text>
          <Text className="text-white text-xs font-semibold">
            {isPositive ? "+" : ""}
            {percentageChange}%
          </Text>
        </View>
      </View>

      {/* Current Amount */}
      <Text className="text-white text-5xl font-bold mb-1">
        ${currentAmount.toFixed(2)}
      </Text>

      {/* Last Month Amount */}
      <Text className="text-white/70 text-sm mb-6">
        Last month: ${lastMonthAmount.toFixed(2)}
      </Text>

      {/* Graph */}
      <Svg width="100%" height="80" viewBox="0 0 300 80">
        <Path
          d="M 0 60 Q 50 55, 75 50 T 150 40 Q 200 35, 225 30 T 300 10"
          stroke="white"
          strokeWidth="3"
          fill="none"
          strokeLinecap="round"
          opacity={0.8}
        />
      </Svg>
    </View>
  );
}

const styles = StyleSheet.create({});
