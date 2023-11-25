import axios from "axios";
import AuthVerifyService from "../Auth/AuthVerifyService";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const CommentImage = async (imageId, commentText) => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.post(
      `${API_URL}Comment/comment/${userId}/${imageId}`,
      commentText,
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
      return { success: false, error: "Image commenting failed" };
    }
  } catch (error) {
    console.error("Error commenting image:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const GetCommentsPerImageId = async (imageId) => {
  try {
    const response = await axios.get(
      `${API_URL}Comment/comments-per-image/${imageId}`,
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
      return { success: false, error: "Image comments getting failed" };
    }
  } catch (error) {
    console.error("Error getting image comments:", error);
    return { success: false, error: error.message || "An error occurred" };
  }
};

const GetNumberCommentsPerImage = async (imageId) => {
  try {
    const response = await axios.get(
      `${API_URL}Comment/number-comments/${imageId}`,
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

const CommentService = {
  CommentImage,
  GetCommentsPerImageId,
  GetNumberCommentsPerImage,
};

export default CommentService;
