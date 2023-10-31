import { useContext } from "react";
import AuthContext from "../../Context/AuthContext";

const Home = () => {
  const infos = useContext(AuthContext);
  console.log(infos.userInfo);
  return <div className="w-40 h-64 bg-black text-white">hello</div>;
};

export default Home;
