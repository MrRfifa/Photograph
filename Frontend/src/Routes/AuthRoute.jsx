import { Navigate, Route, Routes } from "react-router-dom";
import HomePage from "../Pages/General/HomePage";
import LoginPage from "../Pages/General/LoginPage";
import RegisterPage from "../Pages/General/RegisterPage";
import ForgetPasswordPage from "../Pages/General/ForgetPasswordPage";
import ResetPasswordPage from "../Pages/General/ResetPasswordPage";
import DeleteAccountPage from "../Pages/General/DeleteAccountPage";

function AuthRoute() {
  return (
    <Routes>
      <Route path="/" exact element={<HomePage />} />
      <Route path="/login" exact element={<LoginPage />} />
      <Route path="/register" exact element={<RegisterPage />} />
      <Route path="/forget-password" exact element={<ForgetPasswordPage />} />
      <Route path="/reset-password" exact element={<ResetPasswordPage />} />
      <Route path="/delete-account" exact element={<DeleteAccountPage />} />
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default AuthRoute;
