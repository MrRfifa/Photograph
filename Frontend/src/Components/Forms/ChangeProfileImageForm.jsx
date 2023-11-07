import { useState } from "react";
import toast from "react-hot-toast";
import UserService from "../../Services/User/UserService";
import PropTypes from "prop-types";

const ChangeProfileImageForm = ({ userId }) => {
  const [loading, setLoading] = useState(false);
  const [file, setFile] = useState(null);
  const [progress, setProgress] = useState({ started: false, pc: 0 });
  const [msg, setMsg] = useState(null);

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      setLoading(true);

      if (!file) {
        setMsg("No file selected");
        toast.error("No file selected", {
          duration: 2000,
          position: "top-right",
          icon: "🤌🏻",
          className: "bg-yellow-500 text-white",
        });
      }

      const fd = new FormData();
      fd.append("file", file);

      setMsg("Uploading...");
      setProgress((prevState) => ({
        ...prevState,
        started: true,
      }));

      const response = await UserService.changeProfileImage(
        userId,
        fd,
        setProgress,
        setMsg
      );
      console.log(response);

      if (response.status) {
        toast.success(msg || "Image uploaded successfully", {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
        window.location.reload("/settings");
      } else {
        toast.error("Image change failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error(msg || "Error occured", {
        duration: 2000,
        position: "top-right",
        icon: "🤌🏻",
        className: "bg-red-500 text-white",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (event) => {
    if (event.target.files && event.target.files[0]) {
      setFile(event.target.files[0]);
    }
  };

  return (
    <div>
      <h2 className="text-2xl pl-[25%] font-bold text-white mb-4">
        Change Image
      </h2>
      <form
        onSubmit={handleSubmit}
        className="px-7 grid place-items-center"
        encType="multipart/form-data"
      >
        <div className="grid gap-6" id="form">
          <div className="grid gap-6 w-full">
            <label className="text-white" htmlFor="image">
              Update image
            </label>
            <input
              type="file"
              className="file:bg-gradient-to-b file:from-blue-500 file:to-blue-600
                  file:py-3 file:px-6 file:m-2 file:border-none file:rounded-full file:text-white file:cursor-pointer   
                  bg-gradient-to-br from-gray-600 to-gray-700 text-white/80 rounded-full cursor-pointer"
              onChange={handleFileChange}
              accept="image/*"
            />
          </div>
          {progress.started && (
            <progress
              className="w-full [&::-webkit-progress-bar]:rounded-lg [&::-webkit-progress-value]:rounded-lg   [&::-webkit-progress-bar]:bg-slate-300 [&::-webkit-progress-value]:bg-blue-600 [&::-moz-progress-bar]:bg-blue-600"
              max={100}
              value={progress.pc}
            ></progress>
          )}
          {msg && <span>{msg}</span>}
          <button
            className="outline-none shadow-2xl w-full p-3 bg-blue-400 hover:border-blue-500 hover:border-solid hover:border-2 hover:text-blue-500 font-bold"
            type="submit"
          >
            {loading ? "Updating..." : "Update"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default ChangeProfileImageForm;

ChangeProfileImageForm.propTypes = {
  userId: PropTypes.string.isRequired,
};
