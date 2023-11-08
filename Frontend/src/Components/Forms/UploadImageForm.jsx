import { useState } from "react";
import toast from "react-hot-toast";
import { UpdatesButton } from "../CustomizedButtons";
import ImageService from "../../Services/User/ImageService";

const UploadImageForm = () => {
  const [progress, setProgress] = useState({ started: false, pc: 0 });
  const [msg, setMsg] = useState(null);
  const [file, setFile] = useState(null);
  const [imageTitle, setImageTitle] = useState("");
  const [imageDescription, setImageDescription] = useState("");
  const [titleError, setTitleError] = useState("");
  const [descriptionError, setDescriptionError] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();

    // Custom validation for required fields
    if (!imageTitle) {
      setTitleError("Title is required");
    } else {
      setTitleError("");
    }

    if (!imageDescription) {
      setDescriptionError("Description is required");
    } else {
      setDescriptionError("");
    }

    try {
      if (!file) {
        setMsg("No file selected");
        toast.error("No file selected", {
          duration: 2000,
          position: "top-right",
          icon: "🤌🏻",
          className: "bg-yellow-500 text-white",
        });
        return;
      }
      const formData = new FormData();
      formData.append("imageTitle", imageTitle);
      formData.append("imageDescription", imageDescription);
      formData.append("file", file);

      setMsg("Uploading...");
      setProgress((prevState) => ({
        ...prevState,
        started: true,
      }));

      const response = await ImageService.uploadImage(
        formData,
        setProgress,
        setMsg
      );

      if (response.status) {
        toast.success(msg || "Image uploaded successfully", {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
        //window.location.reload("/settings");
      } else {
        toast.error("Image change failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }

      // Continue with the rest of your form submission logic
    } catch (error) {
      toast.error(msg || "Error occurred", {
        duration: 2000,
        position: "top-right",
        icon: "🤌🏻",
        className: "bg-red-500 text-white",
      });
    }
  };

  return (
    <div className="p-4 mx-auto">
      <form
        encType="multipart/form-data"
        className="lg:grid lg:grid-cols-2 lg:gap-4 md:flex md:flex-col "
        onSubmit={handleSubmit}
      >
        <div className="lg:col-span-1">
          <div className="mb-4">
            <label htmlFor="imageTitle" className="block">
              Title
            </label>
            <input
              type="text"
              id="imageTitle"
              name="imageTitle"
              value={imageTitle}
              onChange={(e) => setImageTitle(e.target.value)}
              className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2"
            />
            {titleError && <div className="text-red-500">{titleError}</div>}
          </div>
          <div className="mb-4">
            <label htmlFor="imageDescription" className="block">
              Description
            </label>
            <textarea
              id="imageDescription"
              name="imageDescription"
              value={imageDescription}
              onChange={(e) => setImageDescription(e.target.value)}
              className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2"
            />
            {descriptionError && (
              <div className="text-red-500">{descriptionError}</div>
            )}
          </div>
        </div>
        <div className="lg:col-span-1">
          <div className="mb-4">
            <label htmlFor="file" className="block">
              Image
            </label>
            <input
              onChange={(event) => {
                setFile(event.target.files[0]);
              }}
              accept="image/*"
              type="file"
              id="file"
              name="file"
              className="p-3 flex flex-col h-full w-full rounded-md border border-input bg-white px-3 py-2 text-sm text-gray-900 file:border-0 file:bg-transparent file:text-gray-900 file:text-sm file:font-medium"
            />
            {progress.started && (
              <div className="flex items-center space-x-2">
                <progress
                  max="100"
                  value={progress.pc}
                  className="w-52 h-4 [&::-webkit-progress-bar]:rounded-lg [&::-webkit-progress-value]:rounded-lg   [&::-webkit-progress-bar]:bg-slate-300 [&::-webkit-progress-value]:bg-blue-600 [&::-moz-progress-bar]:bg-blue-600"
                ></progress>
                <span className="text-blue-600">{progress.pc.toFixed(2)}%</span>
              </div>
            )}
            {msg && <span>{msg}</span>}
          </div>
          <div className="mt-0  mx-auto p-3">
            <UpdatesButton type="submit" label="Upload" />
          </div>
        </div>
      </form>
    </div>
  );
};

export default UploadImageForm;
