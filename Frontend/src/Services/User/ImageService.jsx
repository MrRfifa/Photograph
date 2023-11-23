import axios from "axios";
import AuthVerifyService from "../Auth/AuthVerifyService";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const GetImagesByUser = async () => {
  const userId = AuthVerifyService.getUserId();
  try {
    const response = await axios.get(`${API_URL}Image/get-images/${userId}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: localStorage.getItem("token"),
      },
    });

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

const uploadImage = (fd, setProgress, setMsg) => {
  const userId = AuthVerifyService.getUserId();
  return axios
    .post(`${API_URL}Image/upload/${userId}`, fd, {
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

const GetAllImages = async () => {
  try {
    const response = await axios.get(`${API_URL}Image/get-images`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: localStorage.getItem("token"),
      },
    });

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

const GetImageByImageId = async (imageId) => {
  try {
    const response = await axios.get(`${API_URL}Image/get/${imageId}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: localStorage.getItem("token"),
      },
    });

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

const ImageService = {
  GetImagesByUser,
  uploadImage,
  GetAllImages,
  GetImageByImageId,
};

export default ImageService;
