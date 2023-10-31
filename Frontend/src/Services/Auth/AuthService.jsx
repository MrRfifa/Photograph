import axios from "axios";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const login = (email, password) =>
  axios
    .post(`${API_URL}Auth/login`, {
      email,
      password,
    })
    .then((response) => {
      if (response.data && response.data.token) {
        const token = "bearer " + response.data.token;
        localStorage.setItem("token", token);
        return { success: true, token };
      } else if (response == "Not verified.") {
        return { success: false, error: "Not verified" };
      } else {
        return { success: false, error: "Email or password is incorrect" };
      }
    })
    .catch((error) => {
      console.error("Login error:", error);
      return { success: false, error: error.response.data };
    });

const register = (
  firstName,
  lastName,
  gender,
  dateOfBirth,
  email,
  password,
  confirmPassword
) =>
  axios
    .post(`${API_URL}Auth/register`, {
      firstName,
      lastName,
      gender,
      dateOfBirth,
      email,
      password,
      confirmPassword,
      role: "owner",
    })
    .then((response) => {
      if (response.data) {
        return { success: true, message: "Signed up successfully!" };
      } else {
        return { success: false, error: "Registration failed" };
      }
    })
    .catch((error) => {
      console.error("Registration error:", error);
      return { success: false, error: error.response.data };
    });

const forgetPassword = (email) =>
  axios
    .post(`${API_URL}Auth/forgot-password`, `"${email}"`, {
      headers: {
        "Content-Type": "application/json",
      },
    })
    .then((response) => {
      if (response.data) {
        return {
          success: true,
          message: "An email is sent to the provided email address.",
        };
      } else {
        // console.log("Password reset failed. Response:", response);
        return { success: false, error: "Password reset is failed" };
      }
    })
    .catch((error) => {
      // console.error("Forget password error:", error);
      return { success: false, error: error.response.data };
    });

const resetPassword = (token, password, confirmPassword) =>
  axios
    .post(
      `${API_URL}Auth/reset-password`,
      {
        token,
        password,
        confirmPassword,
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      if (response.data) {
        return {
          success: true,
          message: "You have successfully resetted your password",
        };
      } else {
        return { success: false, error: "Password reset is failed" };
      }
    })
    .catch((error) => {
      console.error("Login error:", error);
      return { success: false, error: error.response.data };
    });

const getUserInfo = (authToken) => {
  const headers = {
    "Content-Type": "application/json",
    Authorization: authToken,
  };

  return axios
    .get(`${API_URL}User/info`, {
      headers,
    })
    .then((response) => {
      if (response.data) {
        return {
          userInfo: response.data,
        };
      } else {
        return { error: "User information not available" };
      }
    })
    .catch((error) => {
      console.error("Error fetching user information:", error);
      return { success: false, error: error.response.data };
    });
};

const AuthService = {
  login,
  register,
  forgetPassword,
  resetPassword,
  getUserInfo,
};

export default AuthService;
