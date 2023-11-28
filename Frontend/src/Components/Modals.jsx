import PropTypes from "prop-types";
import { useEffect, useState } from "react";
import ChangePasswordForm from "./Forms/ChangePasswordForm";
import ChangeEmailForm from "./Forms/ChangeEmailForm";
import ModalComponent from "./ModalComponent";
import ChangeNamesForm from "./Forms/ChangeNamesForm";
import ChangeProfileImageForm from "./Forms/ChangeProfileImageForm";
import UploadImageForm from "./Forms/UploadImageForm";
import AuthVerifyService from "../Services/Auth/AuthVerifyService";
import AuthService from "../Services/Auth/AuthService";
import DeleteAccountRequestForm from "./Forms/DeleteAccountRequestForm";

export const ChangeEmailModal = ({ open, onClose }) => {
  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeEmailForm />
    </ModalComponent>
  );
};

ChangeEmailModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ChangePasswordModal = ({ open, onClose }) => {
  const userId = AuthVerifyService.getUserId();

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
  const [username, setUsername] = useState({ lastname: "", firstname: "" });
  const userId = AuthVerifyService.getUserId();

  useEffect(() => {
    AuthService.getUserSpecificInfo(userId)
      .then((res) => {
        setUsername({
          lastname: res.userInfoSpec.message.lastName,
          firstname: res.userInfoSpec.message.firstName,
        });
      })
      .catch((error) => {
        console.error("Error fetching user info:", error);
      });
  }, [userId]);

  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeNamesForm
        initialFirstname={username.firstname}
        initialLastname={username.lastname}
      />
    </ModalComponent>
  );
};

ChangeNamesModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};

export const ChangeProfileImageModal = ({ open, onClose }) => {
  return (
    <ModalComponent open={open} onClose={onClose}>
      <ChangeProfileImageForm />
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

export const DeleteAccountModal = ({ open, onClose }) => {
  return (
    <ModalComponent open={open} onClose={onClose}>
      <DeleteAccountRequestForm />
    </ModalComponent>
  );
};

DeleteAccountModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
};
