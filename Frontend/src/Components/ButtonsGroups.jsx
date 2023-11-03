import { useState } from "react";
import { DangerButton, UpdatesButton } from "./CustomizedButtons";
import {
  ChangeEmailModal,
  ChangeNamesModal,
  ChangePasswordModal,
} from "./Modals";

const ButtonsGroups = () => {
  const [openEmailChange, setOpenEmailChange] = useState(false);
  const [openPasswordChange, setOpenPasswordChange] = useState(false);
  const [openNamesChange, setOpenNamesChange] = useState(false);

  const handleEmailChange = () => setOpenEmailChange(true);
  const handlePasswordChange = () => setOpenPasswordChange(true);
  const handleNamesChange = () => setOpenNamesChange(true);

  const handleCloseEmailChange = () => setOpenEmailChange(false);
  const handleClosePasswordChange = () => setOpenPasswordChange(false);
  const handleCloseNamesChange = () => setOpenNamesChange(false);

  return (
    <div className="z-0 bg-gradient-to-br from-[#5A189A] to-[#E0AAFF] p-6 max-w-[600px] mx-auto rounded-lg shadow-lg hover:shadow-2xl transition-transform duration-300 text-white">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 md:flex justify-around">
        <div className="flex flex-col space-y-4 items-center md:items-start text-center md:text-left">
          <UpdatesButton label="Update email" onClick={handleEmailChange} />
          <UpdatesButton label="Update name" onClick={handleNamesChange} />

          <ChangeEmailModal
            key="emailChangeModal" // Add a unique key for the email change modal
            open={openEmailChange}
            onClose={handleCloseEmailChange}
          />
          <ChangeNamesModal
            key="namesChangeModal"
            open={openNamesChange}
            onClose={handleCloseNamesChange}
          />
        </div>
        <div className="flex flex-col space-y-4 items-center md:items-start text-center md:text-left">
          <UpdatesButton
            label="Update password"
            onClick={handlePasswordChange}
          />
          <DangerButton label="Delete account😭" />

          <ChangePasswordModal
            key="passwordChangeModal" // Add a unique key for the password change modal
            open={openPasswordChange}
            onClose={handleClosePasswordChange}
          />
        </div>
      </div>
    </div>
  );
};

export default ButtonsGroups;
