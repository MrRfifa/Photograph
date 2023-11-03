import { ResetPasswordForm } from "../../Components/Forms/ResetPasswordForm";
import resetImage from "../../assets/reset-password.jpg";

const ResetPassword = () => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 h-screen w-full">
      <ResetPasswordForm />
      <div className="hidden sm:block">
        <img
          className="w-full h-full object-cover"
          src={resetImage}
          alt="regiter cam image"
        />
      </div>
    </div>
  );
};

export default ResetPassword;
