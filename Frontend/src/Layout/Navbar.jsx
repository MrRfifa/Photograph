import { useState } from "react";
import { AiOutlineClose, AiOutlineMenu } from "react-icons/ai";
import camLogo from "../assets/cam.png";
import { Link } from "react-router-dom";
import { LabelDestinationLinkButton } from "../Components/CustomizedButtons";

function Navbar() {
  const [nav, setNav] = useState(true);

  const handleNav = () => {
    setNav(!nav);
  };

  return (
    <div className="w-full flex justify-between items-center h-24 mx-auto px-4 text-slate-950 bg-[#5E548E] ">
        <img className="w-24" src={camLogo} alt="cam" />
      <ul className="hidden md:flex font-bold">
        <li className="p-4">
          <LabelDestinationLinkButton
            nav={true}
            label="Login"
            destination="/login"
          />
        </li>
        <li className="p-4">
          <LabelDestinationLinkButton
            nav={true}
            label="Register"
            destination="/register"
          />
        </li>
      </ul>
      <div onClick={handleNav} className="block md:hidden">
        {!nav ? <AiOutlineClose size={20} /> : <AiOutlineMenu size={20} />}
      </div>
      <div
        className={
          !nav
            ? "fixed left-0 top-0 w-[60%] border-r h-full border-r-black bg-[#5E548E] ease-in-out duration-500 z-10"
            : "fixed left-[-100%] ease-in-out duration-0"
        }
      >
        <img className="w-24" src={camLogo} alt="cam" />
        <ul className="uppercase p-4 font-bold">
          <li className="p-4 border-b border-black">
            <Link to="/login">Login</Link>
          </li>
          <li className="p-4 border-b border-black">
            <Link to="/register">Register</Link>
          </li>
        </ul>
      </div>
    </div>
  );
}

export default Navbar;
