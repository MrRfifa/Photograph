import { useState } from "react";
import toast from "react-hot-toast";
import axios from "axios";

const ChangeProfileImageForm = () => {
  const [loading, setLoading] = useState(false);
  const [file, setFile] = useState(null);
  const [progress, setProgress] = useState({ started: false, pc: 0 });
  const [msg, setMsg] = useState(null);

  const handleSubmit = async (event) => {
    event.preventDefault();

    console.log("Form submitted with image:", file);

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
        return;
      }
      const fd = new FormData();
      fd.append("file", file);
      console.log(fd);
      setMsg("Uploading...");
      setProgress((prevState) => {
        return { ...prevState, started: true };
      });
      axios
        .post(
          "http://httpbin.org/post",
          fd,
          {
            onUploadProgress: (progressEvent) => {
              setProgress((prevState) => {
                return { ...prevState, pc: progressEvent.progress * 100 };
              });
            },
          },
          {
            headers: {
              "Custom-header": "value",
            },
          }
        )
        .then((res) => {
          setMsg("Uploaded succeeded");
          console.log(res);
        })
        .catch((err) => {
          setMsg("Uploaded failed");
          console.log(err);
        });

      // Handle the form submission here, e.g., upload the image to a server.
      // You can use the FormData object 'fd' to send the image data.
    } catch (error) {
      toast.error(
        "An error occurred during image upload. Please try again later.",
        {
          duration: 2000,
          position: "top-right",
          icon: "🤌🏻",
          className: "bg-red-500 text-white",
        }
      );
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
    console.log(file);
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
            <progress max={100} value={progress.pc}></progress>
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
