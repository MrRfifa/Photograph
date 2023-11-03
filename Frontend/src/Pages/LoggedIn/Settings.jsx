import { useContext, useEffect, useState } from "react";
import AuthContext from "../../Context/AuthContext";
import manLogo from "../../assets/Genders/man.png";
import womanLogo from "../../assets/Genders/woman.png";
// import { ChangeEmailForm } from "../../Components/ChangeEmailForm";
import ProfileCard from "../../Components/ProfileCard";
import ButtonsGroups from "../../Components/ButtonsGroups";
import { UpdatesButton } from "../../Components/CustomizedButtons";
import { FaSpinner } from 'react-icons/fa';

const Settings = () => {
  const infos = useContext(AuthContext);
  const [userInfos, setUserInfos] = useState(null);
  const [profileImage, setProfileImage] = useState(null);

  useEffect(() => {
    if (infos.userInfo && infos.userInfo.length >= 3) {
      const userInfo = infos.userInfo;
      const dateOfBirthValue = userInfo[3].value;
      const dateOnly = dateOfBirthValue.split(" ")[0];
      const gender = userInfo[2].value;

      setUserInfos({
        lastname: userInfo[4].value,
        firstname: userInfo[5].value,
        email: userInfo[0].value,
        gender: gender,
        dateOfBirth: dateOnly,
      });

      if (!profileImage && gender === "female") {
        setProfileImage(womanLogo);
      } else if (!profileImage && gender === "male") {
        setProfileImage(manLogo);
      }
    }
  }, [infos.userInfo, profileImage]);

  if (!userInfos) {
    return (
      <div className="w-full h-full flex items-center justify-center">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="w-full text-white  h-[95%]  lg:max-w-[1240px] md:max-w-[850px] max-w-[550px] md:text-xl sm:text-lg text-base">
      <div className="grid grid-cols-1 lg:grid-cols-3 lg:max-w-[1240px] mx-auto gap-2 h-full md:max-w-[850px] max-w-[550px] ">
        <div className="flex flex-col items-center ">
          <div className="w-[200px] h-[200px] mx-auto rounded-2xl mt-[10%] lg:mt-[30%] lg:w-[300px] lg:h-[300px] md:w-[250px] md:h-[250px] ">
            <img
              className="w-[500px] mx-auto my-4"
              src={profileImage}
              alt="camera"
            />
          </div>
          <div className="mt-[10%] mb-[5%] lg:mt-[30%]">
            <UpdatesButton label="Update" />
          </div>

          {/* <button
            className="mt-[10%] mb-[5%] lg:mt-[30%] relative px-8 py-2 rounded-md bg-white isolation-auto z-10 border-2 border-lime-500
            before:absolute before:w-full before:transition-all before:duration-700 before:hover:w-full before:-right-full before:hover:right-0 before:rounded-full  before:bg-lime-500 before:-z-10  before:aspect-square before:hover:scale-150 overflow-hidden before:hover:duration-700"
          >
            Update
          </button> */}
        </div>
        <div className="col-span-2 flex justify-center items-center">
          <div className="grid grid-rows-2 flex-col items-center justify-evenly pt-5">
            <div className="w-full">
              <ProfileCard
                dateOfBirth={userInfos.dateOfBirth}
                lastname={userInfos.lastname}
                firstname={userInfos.firstname}
                email={userInfos.email}
                gender={userInfos.gender}
              />
            </div>
            <div className="w-full">
              <ButtonsGroups />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Settings;
