import PropTypes from "prop-types";
import { useContext } from "react";
import AuthContext from "../Context/AuthContext";
import ChangePasswordForm from "./Forms/ChangePasswordForm";
import ChangeEmailForm from "./Forms/ChangeEmailForm";
import ModalComponent from "./ModalComponent";
import ChangeNamesForm from "./Forms/ChangeNamesForm";
import ChangeProfileImageForm from "./Forms/ChangeProfileImageForm";
import UploadImageForm from "./Forms/UploadImageForm";
import AuthVerifyService from "../Services/Auth/AuthVerifyService";

export const ChangeEmailModal = ({ open, onClose }) => {
  const { userInfo } = useContext(AuthContext);
  const userId = userInfo[3].value;

  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeEmailForm userId={userId} key={userId} />
    </ModalComponent>
  );
};

ChangeEmailModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ChangePasswordModal = ({ open, onClose }) => {
  const { userInfo } = useContext(AuthContext);
  const userId = userInfo[3].value;

  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangePasswordForm userId={userId} key={userId} />
    </ModalComponent>
  );
};

ChangePasswordModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ChangeNamesModal = ({ open, onClose }) => {
  const { userInfo } = useContext(AuthContext);
  const userId = userInfo[3].value;
  const userName = {
    lastname: "userInfoSpecific.message.lastName",
    firstname: "userInfoSpecific.message.firstName",
  };

  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeNamesForm
        userId={userId}
        key={userId}
        initialFirstname={userName.firstname}
        initialLastname={userName.lastname}
      />
    </ModalComponent>
  );
};

ChangeNamesModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ChangeProfileImageModal = ({ open, onClose }) => {
  const { userInfo } = useContext(AuthContext);
  const userId = userInfo[3].value;

  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeProfileImageForm key={userId} userId={userId} />
    </ModalComponent>
  );
};

ChangeProfileImageModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const UploadImageModal = ({ open, onClose }) => {
  // const { userInfo } = useContext(AuthContext);
  // const userId = userInfo[3].value;
  const userId = AuthVerifyService.getUserId();

  return (
    <ModalComponent open={open} onClose={onClose}>
      <UploadImageForm key={userId} userId={userId} />
    </ModalComponent>
  );
};

UploadImageModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ImageModal = ({ imageDetails, closeModal }) => {
  return (
    <div
      className="fixed top-3 left-96 mx-auto my-auto w-[80%] h-[80%] bg-opacity-100 hidden lg:flex"
      onClick={closeModal}
    >
      <div className="bg-white w-[80%] h-[100%] p-2 rounded-lg">
        <img
          src={`data:image/png;base64,${imageDetails.fileContentBase64}`}
          alt={imageDetails.id}
          className="w-[100%] h-[100%]"
        />
      </div>
    </div>
  );
};

ImageModal.propTypes = {
  imageDetails: PropTypes.object.isRequired,
  closeModal: PropTypes.func,
};
