import axios from "axios";
import AuthVerifyService from "../Auth/AuthVerifyService";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const GetNumberLikesReceived = async () => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(
      `${API_URL}Statistic/likes-received/${userId}`,
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
        message: response.data,
      };
    } else {
      return { success: false, error: "Images request failed" };
    }
  } catch (error) {
    console.error("Error getting image likes number:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};
const GetNumberCommentsReceived = async () => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(
      `${API_URL}Statistic/comments-received/${userId}`,
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
        message: response.data,
      };
    } else {
      return { success: false, error: "Images request failed" };
    }
  } catch (error) {
    console.error("Error getting image likes number:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};
const GetNumberLikesDone = async () => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(
      `${API_URL}Statistic/likes-done/${userId}`,
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
        message: response.data,
      };
    } else {
      return { success: false, error: "Images request failed" };
    }
  } catch (error) {
    console.error("Error getting image likes number:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};
const GetNumberCommentsDone = async () => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(
      `${API_URL}Statistic/comments-done/${userId}`,
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
        message: response.data,
      };
    } else {
      return { success: false, error: "Images request failed" };
    }
  } catch (error) {
    console.error("Error getting image likes number:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const StaticticService = {
  GetNumberLikesReceived,
  GetNumberCommentsReceived,
  GetNumberLikesDone,
  GetNumberCommentsDone,
};

export default StaticticService;
