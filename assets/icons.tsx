import { Feather } from "@expo/vector-icons";

export const icons = {
  index: (props: any) => <Feather name="home" size={24} {...props} />,
  search: (props: any) => <Feather name="search" size={24} {...props} />,
  insights: (props: any) => <Feather name="bar-chart-2" size={24} {...props} />,
  profile: (props: any) => <Feather name="user" size={24} {...props} />,
  scan: (props: any) => <Feather name="camera" size={26} {...props} />,
};
