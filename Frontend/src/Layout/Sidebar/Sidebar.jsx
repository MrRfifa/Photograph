import { useEffect, useState, useRef, useContext } from "react";
import { motion } from "framer-motion";
import icon from "../../assets/cam.png";
import manLogo from "../../assets/Genders/man.png";
import womanLogo from "../../assets/Genders/woman.png";
import {
  AiOutlineHome,
  AiOutlineAppstore,
  AiOutlineSetting,
  AiOutlineArrowLeft,
  AiOutlineMenu,
  AiOutlineLogout,
} from "react-icons/ai";
import { useMediaQuery } from "react-responsive";
import { NavLink, useLocation } from "react-router-dom";
import AuthContext from "../../Context/AuthContext";
import AuthService from "../../Services/Auth/AuthService";

const Sidebar = () => {
  const isTabletMid = useMediaQuery({ query: "(max-width: 1250px)" });
  const [open, setOpen] = useState(!isTabletMid); // Initialize the state based on the screen size
  const sidebarRef = useRef();
  const { pathname } = useLocation();
  const { userInfo } = useContext(AuthContext);
  var userFemale = false;
  const [userInfoSpecific, setUserInfoSpecific] = useState({
    firstname: "",
    lastname: "",
    profileImage: null,
  });
  // Check if infos.userInfo exists and has at least 3 elements
  useEffect(() => {
    if (userInfo && userInfo[3]) {
      AuthService.getUserSpecificInfo(userInfo[3].value)
        .then((res) => {
          const { message } = res.userInfoSpec || {};
          const { firstName, lastName, fileContentBase64 } = message || {};

          setUserInfoSpecific({
            firstname: firstName || "",
            lastname: lastName || "",
            profileImage: fileContentBase64 || "",
          });
        })
        .catch((error) => {
          console.error("Error fetching user information:", error);
        });
    }
  }, [userInfo]);
  useEffect(() => {
    if (isTabletMid) {
      setOpen(false);
    } else {
      setOpen(true);
    }
  }, [isTabletMid]);

  useEffect(() => {
    if (isTabletMid) {
      setOpen(false);
    }
  }, [pathname, isTabletMid]);

  const Nav_animation = isTabletMid
    ? {
        open: {
          x: 0,
          width: "16rem",
          transition: {
            damping: 40,
          },
        },
        closed: {
          x: -250,
          width: 0,
          transition: {
            damping: 40,
            delay: 0.15,
          },
        },
      }
    : {
        open: {
          width: "16rem",
          transition: {
            damping: 40,
          },
        },
        closed: {
          width: "3.65rem",
          transition: {
            damping: 40,
          },
        },
      };

  return (
    <div>
      <div
        onClick={() => setOpen(false)}
        className={`lg:hidden fixed inset-0 max-h-screen z-[998] bg-black/50 ${
          open ? "block" : "hidden"
        } `}
      ></div>
      <motion.div
        ref={sidebarRef}
        variants={Nav_animation}
        initial={{ x: isTabletMid ? -250 : 0 }}
        animate={open ? "open" : "closed"}
        className="bg-gray-700 text-gray shadow-xl z-[999] max-w-[16rem] w-[16rem] overflow-hidden lg:relative fixed h-screen"
      >
        <div className="flex items-center gap-2.5 font-medium border-b py-3 border-slate-300 mx-3">
          <img src={icon} width={45} alt="Photograph icon" />
          <span className="text-xl whitespace-pre text-white">Photograph</span>
        </div>
        <div className="flex items-center gap-2.5 font-medium border-b py-3 border-slate-300 mx-3">
          <img
            src={
              userInfoSpecific.profileImage
                ? `data:image/*;base64,${userInfoSpecific.profileImage}`
                : userFemale
                ? womanLogo
                : manLogo
            }
            width={45}
            alt=""
            className="rounded-full"
          />
          <span className="text-xl whitespace-pre text-white uppercase">
            {userInfoSpecific.lastname} {userInfoSpecific.firstname}
          </span>
        </div>

        <div className="flex flex-col h-full">
          <ul className="whitespace-pre px-2.5 text-[0.9rem] py-5 flex flex-col gap-1 font-medium overflow-x-hidden scrollbar-thin scrollbar-track-white scrollbar-thumb-slate-100 lg:h-[68%] h-[70%]">
            <li>
              <NavLink to="/home" className="link text-white">
                <AiOutlineHome size={25} className="min-w-max" />
                Home
              </NavLink>
            </li>
            <li>
              <NavLink to="/my-photos" className="link text-white">
                <AiOutlineAppstore size={25} className="min-w-max" />
                My Photos
              </NavLink>
            </li>
            <li>
              <NavLink to="/settings" className="link text-white">
                <AiOutlineSetting size={25} className="min-w-max" />
                Settings
              </NavLink>
            </li>
            <li>
              <NavLink
                to="/login"
                onClick={() => {
                  localStorage.clear();
                  window.location.reload();
                }}
                className="link text-white"
              >
                <AiOutlineLogout size={25} className="min-w-max" />
                Logout
              </NavLink>
            </li>
          </ul>
        </div>
        <motion.div
          onClick={() => {
            setOpen(!open);
          }}
          animate={
            open
              ? {
                  x: 0,
                  y: 0,
                  rotate: 0,
                }
              : {
                  x: -10,
                  y: -200,
                  rotate: 180,
                }
          }
          transition={{ duration: 0 }}
          className="absolute w-fit h-fit lg:block z-50 hidden right-2 bottom-3 cursor-pointer"
        >
          <AiOutlineArrowLeft className="text-white" size={25} />
        </motion.div>
      </motion.div>
      <div
        className="m-3 block lg:hidden md:block"
        onClick={() => setOpen(true)}
      >
        <AiOutlineMenu size={25} />
      </div>
    </div>
  );
};

export default Sidebar;
