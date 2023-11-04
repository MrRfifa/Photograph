import PropTypes from "prop-types";
import { useContext } from "react";
import AuthContext from "../Context/AuthContext";
import ChangePasswordForm from "./Forms/ChangePasswordForm";
import ChangeEmailForm from "./Forms/ChangeEmailForm";
import ModalComponent from "./ModalComponent";
import ChangeNamesForm from "./Forms/ChangeNamesForm";

export const ChangeEmailModal = ({ open, onClose }) => {
  const infos = useContext(AuthContext);
  const userId = infos.userInfo[6].value;

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
  const infos = useContext(AuthContext);
  const userId = infos.userInfo[6].value;

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
  const infos = useContext(AuthContext);
  const userId = infos.userInfo[6].value;
  const userName = {
    lastname: infos.userInfo[4].value,
    firstname: infos.userInfo[5].value,
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