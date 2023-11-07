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

const changeProfileImage = (userId, fd, setProgress, setMsg) => {
  return axios
    .post(`${API_URL}Image/upload-profile-image/${userId}`, fd, {
      onUploadProgress: (progressEvent) => {
        setProgress((prevState) => ({
          ...prevState,
          pc: (progressEvent.loaded / progressEvent.total) * 100,
        }));
      },
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: localStorage.getItem("token"),
      },
    })
    .then((res) => {
      if (res.data && res.data.status) {
        setMsg(res.data.message || "Uploaded successfully");
        console.log(res.data);
        return res.data; // Return the response data
      } else {
        setMsg("Upload failed");
        console.error(res.data);
        return res.data; // Return the response data even in case of failure
      }
    })
    .catch((err) => {
      setMsg("Upload failed");
      console.error(err);
      throw err; // Rethrow the error to propagate it
    });
};

const UserService = {
  changeEmailAddress,
  changePassword,
  changeNames,
  changeProfileImage,
};

export default UserService;
