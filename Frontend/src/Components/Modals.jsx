import PropTypes from "prop-types";
import { useContext } from "react";
import AuthContext from "../Context/AuthContext";
import ChangePasswordForm from "./Forms/ChangePasswordForm";
import ChangeEmailForm from "./Forms/ChangeEmailForm";
import ModalComponent from "./ModalComponent";
import ChangeNamesForm from "./Forms/ChangeNamesForm";
import ChangeProfileImageForm from "./Forms/ChangeProfileImageForm";

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
