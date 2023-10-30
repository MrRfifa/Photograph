import { Navigate, Route, Routes } from "react-router-dom";
import HomePage from "../Pages/General/HomePage";
import LoginPage from "../Pages/General/LoginPage";
import RegisterPage from "../Pages/General/RegisterPage";

function AuthRoute() {
  return (
    <Routes>
      <Route path="/" exact element={<HomePage />} />
      <Route path="/login" exact element={<LoginPage />} />
      <Route path="/register" exact element={<RegisterPage />} />
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default AuthRoute;
