import { createContext, useEffect, useState } from "react";
import AuthService from "../Services/Auth/AuthService";
import PropTypes from "prop-types";

const AuthContext = createContext();

function AuthContextProvider(props) {
  const [userInfo, setUserInfo] = useState(null);
  const token = localStorage.getItem("token");

  useEffect(() => {
    AuthService.getUserInfo(token)
      .then((result) => {
        if (result) {
          setUserInfo(result.userInfo);
        } else {
          console.error("Error:", result);
        }
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  }, [token]);

  return (
    <AuthContext.Provider
      value={{
        userInfo,
      }}
    >
      {props.children}
    </AuthContext.Provider>
  );
}

export default AuthContext;
export { AuthContextProvider };

AuthContextProvider.propTypes = {
  children: PropTypes.node.isRequired,
};
