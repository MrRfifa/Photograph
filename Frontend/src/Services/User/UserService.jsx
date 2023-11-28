import axios from "axios";
import AuthVerifyService from "../Auth/AuthVerifyService";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const changeEmailAddress = (newEmail, currentPassword) => {
  const userId = AuthVerifyService.getUserId();
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
};

const changePassword = (currentPassword, password, confirmPassword) => {
  const userId = AuthVerifyService.getUserId();
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
};

const changeNames = async (currentPassword, newFirstname, newLastname) => {
  const userId = AuthVerifyService.getUserId();

  try {
    const response = await axios.put(
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
    );

    if (response.data) {
      return {
        success: true,
        message: response.data.message, // Assuming the success message is under the "message" key
      };
    } else {
      console.error("Unexpected response format:", response);
      return { success: false, error: "Names change request failed" };
    }
  } catch (error) {
    console.error("Error changing names:", error);
    return { success: false, error: error.response?.data || "Unknown error" };
  }
};

const changeProfileImage = (fd, setProgress, setMsg) => {
  const userId = AuthVerifyService.getUserId();
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

const confirmAccountDeletion = (token) =>
  axios
    .delete(`${API_URL}User/account/verify-delete`, {
      params: {
        token: token,
      },
    })
    .then((response) => {
      if (response.status == "204") {
        return { success: true };
      } else {
        return { success: false };
      }
    })
    .catch((error) => {
      console.error("Deletion error:", error);
      return { success: false, error: error.response.data };
    });

const accountDeletionRequest = async (password) => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.put(
      `${API_URL}User/${userId}/account/delete`,
      null,
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: localStorage.getItem("token"),
        },
        params: {
          currentPassword: password,
        },
      }
    );

    if (response.data && response.data.status === "success") {
      return { success: true, message: response.data.message };
    } else {
      return { success: false, error: response.data };
    }
  } catch (error) {
    console.error("Deletion error:", error);

    if (error.response && error.response.data) {
      return { success: false, error: error.response.data };
    } else {
      return { success: false, error: "Unexpected error format" };
    }
  }
};

const UserService = {
  changeEmailAddress,
  changePassword,
  changeNames,
  changeProfileImage,
  confirmAccountDeletion,
  accountDeletionRequest,
};

export default UserService;
