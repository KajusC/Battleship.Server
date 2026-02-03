import { useTheme } from "@/contexts/ThemeContext";
import React from "react";
import { StyleSheet, Text, View } from "react-native";
import { LineChart } from "react-native-gifted-charts";

interface GraphCardProps {
  title?: string;
  currentAmount?: number;
  lastMonthAmount?: number;
  percentageChange?: number;
  data?: { value: number }[];
}

export default function GraphCard({
  title = "This Month's Spend",
  currentAmount = 428.5,
  lastMonthAmount = 382.0,
  percentageChange = 12,
  data = [
    { value: 10 },
    { value: 30 },
    { value: 20 },
    { value: 50 },
    { value: 40 },
  ],
}: GraphCardProps) {
  const isPositive = percentageChange >= 0;
  const { colors } = useTheme();

  return (
    <View
      className="rounded-[40px] p-6 w-full max-w-sm shadow-lg overflow-hidden"
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
      <View style={{ height: 80, marginLeft: -20, marginBottom: -20 }}>
        <LineChart
          curved
          data={data}
          height={80}
          width={300}
          hideDataPoints
          hideRules
          hideAxesAndRules
          color="white"
          thickness={3}
          initialSpacing={0}
          endSpacing={0}
          spacing={40}
        />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({});
