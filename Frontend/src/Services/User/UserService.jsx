import axios from "axios";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const changeEmailAddress = (userId, newEmail, currentPassword) =>
  axios
    .put(
      `${API_URL}User/${userId}/account/email`,
      {
        newEmail,
        currentPassword,
      },
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: localStorage.getItem("token"),
        },
      }
    )
    .then((response) => {
      if (response.data) {
        return {
          success: true,
          message: response.data,
        };
      } else {
        return { success: false, error: "Email change request failed" };
      }
    })
    .catch((error) => {
      console.error("Error changing email address:", error);
      return { success: false, error: error.response.data };
    });

const changePassword = (userId, currentPassword, password, confirmPassword) =>
  axios
    .put(
      `${API_URL}User/${userId}/account/password`,
      {
        currentPassword,
        password,
        confirmPassword,
      },
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: localStorage.getItem("token"),
        },
      }
    )
    .then((response) => {
      if (response.data) {
        return {
          success: true,
          message: response.data,
        };
      } else {
        return { success: false, error: "Password change request failed" };
      }
    })
    .catch((error) => {
      console.error("Error changing password:", error);
      return { success: false, error: error.response.data };
    });

const changeNames = (userId, currentPassword, newFirstname, newLastname) =>
  axios
    .put(
      `${API_URL}User/${userId}/account/names`,
      {
        currentPassword,
        newFirstname,
        newLastname,
      },
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: localStorage.getItem("token"),
        },
      }
    )
    .then((response) => {
      if (response.data) {
        return {
          success: true,
          message: response.data,
        };
      } else {
        return { success: false, error: "Names change request failed" };
      }
    })
    .catch((error) => {
      console.error("Error changing names:", error);
      return { success: false, error: error.response.data };
    });

const UserService = {
  changeEmailAddress,
  changePassword,
  changeNames,
};

export default UserService;
