import { ForgetPasswordForm } from "../../Components/Forms/ForgetPasswordForm";
import forgetImage from "../../assets/forget-password.jpg";

const ForgetPasswordPage = () => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 h-screen w-full">
      <div className="hidden sm:block">
        <img
          className="w-full h-full object-cover"
          src={forgetImage}
          alt="regiter cam image"
        />
      </div>
      <ForgetPasswordForm />
    </div>
  );
};

export default ForgetPasswordPage;
