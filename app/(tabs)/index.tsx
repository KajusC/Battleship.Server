import Card from "@/components/cards/Card";
import GraphCard from "@/components/cards/GraphCard";
import { useTheme } from "@/hooks/useTheme";
import { Image, ScrollView, Text, View } from "react-native";

export default function Index() {
  const { colors, toggleTheme, theme } = useTheme();

  return (
    <View className="flex-1" style={{ backgroundColor: colors.background }}>
      <ScrollView
        contentContainerStyle={{
          flexGrow: 1,
          alignItems: "center",
          paddingBottom: 140,
        }}
      >
        <View className="flex flex-row justify-between items-start px-6 py-10 mt-20">
          <View className="flex">
            <Text
              className="font-normal opacity-80 text-base"
              style={{
                color: colors.text,
              }}
            >
              Welcome back,
            </Text>
            <Text
              className="text-[32px] font-bold mb-2"
              style={{
                color: colors.text,
              }}
            >
              Clanker
            </Text>
          </View>

          <View className="flex-1 pt-3 mr-10 items-end align-bottom">
            {/* <MaterialIcons
            name="local-grocery-store"
            size={34}
            color={colors.text}
          /> */}
            <Image
              source={require("../../assets/images/userImage.png")}
              className="size-[5rem]"
            />
          </View>
        </View>
        {/* Cards Section for grocery */}
        <GraphCard />
        <View className="flex-row flex-wrap justify-center gap-4 mt-6 px-4 mb-10 w-full max-w-lg">
          <View className="w-[45%]">
            <Card
              title="Fruits"
              imageUrl={require("../../assets/icons/apple.png")}
              description="$ 150"
            />
          </View>
          <View className="w-[45%]">
            <Card
              title="Vegetables"
              imageUrl={require("../../assets/icons/broccoli.png")}
              description="$ 90"
            />
          </View>
          <View className="w-[45%]">
            <Card
              title="Dairy"
              imageUrl={require("../../assets/icons/milk.png")}
              description="$ 60"
            />
          </View>
          <View className="w-[45%]">
            <Card
              title="Bakery"
              imageUrl={require("../../assets/icons/bread.png")}
              description="$ 40"
            />
          </View>
        </View>
      </ScrollView>
    </View>
  );
}
