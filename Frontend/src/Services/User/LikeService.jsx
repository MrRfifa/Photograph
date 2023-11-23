import axios from "axios";
import AuthVerifyService from "../Auth/AuthVerifyService";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const LikeImage = async (imageId) => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.post(
      `${API_URL}Like/like/${userId}/${imageId}`,
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
    console.error("Error getting images:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const UnlikeImage = async (imageId) => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.post(
      `${API_URL}Like/unlike/${userId}/${imageId}`,
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
    console.error("Error getting images:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const LikedImage = async (imageId) => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(
      `${API_URL}Like/liked/${userId}/${imageId}`,
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
    console.error("Error getting images:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const LikeService = {
  LikeImage,
  UnlikeImage,
  LikedImage,
};

export default LikeService;
