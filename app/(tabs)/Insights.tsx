import { useTheme } from "@/contexts/ThemeContext";
import React from "react";
import { Dimensions, ScrollView, Text, View } from "react-native";
import { LineChart, PieChart } from "react-native-gifted-charts";

export default function Insights() {
  const { colors } = useTheme();

  const spendingData = [
    { value: 50, label: "Mon" },
    { value: 80, label: "Tue" },
    { value: 90, label: "Wed" },
    { value: 70, label: "Thu" },
    { value: 87, label: "Fri" },
    { value: 65, label: "Sat" },
    { value: 45, label: "Sun" },
  ];

  const pieData = [
    {
      value: 47,
      color: "#009FFF",
      gradientCenterColor: "#006DFF",
      focused: true,
    },
    { value: 40, color: "#93FCF8", gradientCenterColor: "#3BE9DE" },
    { value: 16, color: "#BDB2FA", gradientCenterColor: "#8F80F3" },
    { value: 3, color: "#FFA5BA", gradientCenterColor: "#FF7F97" },
  ];

  return (
    <View className="flex" style={{ backgroundColor: colors.background }}>
      <ScrollView
        contentContainerStyle={{
          flexGrow: 1,
          alignItems: "center",
          paddingBottom: 140,
        }}
      >
        <View className="flex justify-start items-start px-6 mt-20 w-full">
          <View className="flex">
            <Text
              className="text-[32px] font-bold mb-2"
              style={{
                color: colors.text,
              }}
            >
              Insights
            </Text>
          </View>
        </View>
        <View
          className="mb-6 mx-6 rounded-[40px] p-6 shadow-lg overflow-hidden"
          style={{ backgroundColor: colors.statisticsCardBackground }}
        >
          {/* Header */}
          <View className="flex-row justify-between items-center mb-2">
            <Text className="text-white text-base font-medium">
              This Month's Spend
            </Text>
            <View className="bg-white/20 rounded-full px-3 py-1 flex-row items-center">
              <Text className="text-white text-xs mr-1">↗</Text>
              <Text className="text-white text-xs font-semibold">+12%</Text>
            </View>
          </View>

          {/* Current Amount */}
          <Text className="text-white text-5xl font-bold mb-1">$428.50</Text>

          {/* Last Month Amount */}
          <Text className="text-white/70 text-sm mb-6">
            Last month: $382.00
          </Text>

          {/* Graph */}
          <View style={{ alignItems: "center" }}>
            <LineChart
              curved
              data={spendingData}
              height={180}
              width={Dimensions.get("window").width - 100}
              color="white"
              thickness={3}
              initialSpacing={20}
              spacing={40}
              hideRules
              yAxisThickness={0}
              xAxisThickness={0}
              yAxisTextStyle={{ color: "rgba(255,255,255,0.7)" }}
              xAxisLabelTextStyle={{ color: "rgba(255,255,255,0.7)" }}
              hideDataPoints={false}
              dataPointsColor="white"
            />
          </View>
        </View>
        <View style={{ padding: 20, alignItems: "center" }}>
          <PieChart
            data={pieData}
            donut
            showGradient
            sectionAutoFocus
            radius={90}
            innerRadius={60}
            innerCircleColor={"#232B5D"}
            centerLabelComponent={() => {
              return (
                <View
                  style={{ justifyContent: "center", alignItems: "center" }}
                >
                  <Text
                    style={{ fontSize: 22, color: "white", fontWeight: "bold" }}
                  >
                    47%
                  </Text>
                  <Text style={{ fontSize: 14, color: "white" }}>
                    Excellent
                  </Text>
                </View>
              );
            }}
          />
        </View>
      </ScrollView>
    </View>
  );
}
