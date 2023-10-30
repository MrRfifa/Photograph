import { jwtDecode } from "jwt-decode";

const AuthVerify = () => {
  const token = localStorage.getItem("token");
  if (token) {
    const decodedToken = jwtDecode(token);
    if (decodedToken.exp * 1000 < Date.now()) {
      localStorage.clear();
      return 0; // Token is expired, return 0
    }
    if (
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] === "owner"
    ) {
      return 1;
    }
  }
  return 0;
};

const AuthVerifyService = {
  AuthVerify,
};

export default AuthVerifyService;
