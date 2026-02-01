import { Feather } from "@expo/vector-icons";

export const icons = {
  Index: (props: any) => <Feather name="home" size={24} {...props} />,
  Search: (props: any) => <Feather name="search" size={24} {...props} />,
  Insights: (props: any) => <Feather name="bar-chart-2" size={24} {...props} />,
  Profile: (props: any) => <Feather name="user" size={24} {...props} />,
  Scan: (props: any) => <Feather name="camera" size={26} {...props} />,
};
