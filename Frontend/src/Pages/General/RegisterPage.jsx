import RegisterForm from "../../Components/Forms/RegisterForm";
import registerImage from "../../assets/register.jpg";

const RegisterPage = () => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 h-screen w-full">
      <div className="hidden sm:block">
        <img
          className="w-full h-full object-cover"
          src={registerImage}
          alt="regiter cam image"
        />
      </div>
      <RegisterForm />
    </div>
  );
};

export default RegisterPage;
