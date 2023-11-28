import LoginForm from "../../Components/Forms/LoginForm";
import loginImage from "../../assets/Generals/login.jpg";

const LoginPage = () => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 h-screen w-full">
      <LoginForm />
      <div className="hidden sm:block">
        <img
          className="w-full h-full object-cover"
          src={loginImage}
          alt="regiter cam image"
        />
      </div>
    </div>
  );
};

export default LoginPage;
