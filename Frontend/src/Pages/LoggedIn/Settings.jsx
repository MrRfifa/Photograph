import { useContext, useEffect, useState } from "react";
import AuthContext from "../../Context/AuthContext";
import manLogo from "../../assets/Genders/man.png";
import womanLogo from "../../assets/Genders/woman.png";
import ProfileCard from "../../Components/Cards/ProfileCard";
import ButtonsGroups from "../../Components/Buttons/ButtonsGroups";
import { UpdatesButton } from "../../Components/Buttons/CustomizedButtons";
import { FaSpinner } from "react-icons/fa";
import { ChangeProfileImageModal } from "../../Components/Modals";
import AuthService from "../../Services/Auth/AuthService";

const Settings = () => {
  const { userInfo } = useContext(AuthContext);
  const [userInfos, setUserInfos] = useState({});
  const [userInfoSpecific, setUserInfoSpecific] = useState({
    firstname: "",
    lastname: "",
    email: "",
  });
  const [profileImage, setProfileImage] = useState(null);
  const [openImageChange, setOpenImageChange] = useState(false);
  const [gender, setGender] = useState(null);

  const handleImageChange = () => setOpenImageChange(true);
  const handleCloseImageChange = () => setOpenImageChange(false);

  useEffect(() => {
    if (userInfo && userInfo[3]) {
      AuthService.getUserSpecificInfo(userInfo[3].value)
        .then((res) => {
          const { message } = res.userInfoSpec || {};
          const { fileName, fileContentBase64, firstName, lastName, email } =
            message || {};

          setUserInfoSpecific({
            firstname: firstName || "",
            lastname: lastName || "",
            email: email || "",
          });

          if (fileContentBase64 === null) {
            setProfileImage(null);
          } else {
            if (fileName) {
              setProfileImage(`data:image/png;base64,${fileContentBase64}`);
            } else {
              setProfileImage(gender === "female" ? womanLogo : manLogo);
            }
          }
        })
        .catch((error) => {
          console.error("Error fetching user information:", error);
        });
    }
  }, [userInfo, gender]);

  useEffect(() => {
    if (userInfo) {
      const dateOfBirthValue = userInfo[2]?.value;
      const dateOnly = dateOfBirthValue ? dateOfBirthValue.split(" ")[0] : null;
      const userGender = userInfo[1]?.value;

      setUserInfos((prevState) => ({
        ...prevState,
        gender: userGender,
        dateOfBirth: dateOnly,
      }));

      setGender(userGender);
    }
  }, [userInfo]);

  if (
    !userInfos ||
    !userInfoSpecific.firstname ||
    !userInfoSpecific.lastname ||
    !userInfoSpecific.email
  ) {
    return (
      <div className="w-full h-full flex items-center justify-center">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="w-full text-white h-[95%] lg:max-w-[1240px] md:max-w-[850px] max-w-[550px] md:text-xl sm:text-lg text-base ml-0 lg:ml-20">
      <div className="grid grid-cols-1 lg:grid-cols-3 lg:max-w-[1240px] mx-auto gap-2 h-full md:max-w-[850px] max-w-[550px] ">
        <div className="flex flex-col items-center">
          <div className="w-[200px] h-[200px] mx-auto rounded-2xl mt-[10%] lg:mt-[30%] lg:w-[300px] lg:h-[300px] md:w-[250px] md:h-[250px] ">
            <img
              className="w-[500px] mx-auto my-4 rounded-full"
              src={profileImage}
              alt="camera"
            />
          </div>
          <div className="mt-[10%] mb-[5%] lg:mt-[30%]">
            <UpdatesButton label="Update" onClick={handleImageChange} />
            <ChangeProfileImageModal
              key="imageChangeModal"
              open={openImageChange}
              onClose={handleCloseImageChange}
            />
          </div>
        </div>
        <div className="col-span-2 flex justify-center items-center">
          <div className="grid grid-rows-2 flex-col items-center justify-evenly pt-5">
            <div className="w-full">
              <ProfileCard
                dateOfBirth={userInfos.dateOfBirth}
                lastname={userInfoSpecific.lastname}
                firstname={userInfoSpecific.firstname}
                email={userInfoSpecific.email}
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
