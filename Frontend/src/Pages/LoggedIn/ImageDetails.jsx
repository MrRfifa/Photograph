import { useParams } from "react-router-dom";
import { useCallback, useMemo, useState } from "react";
import ImageService from "../../Services/User/ImageService";
import { FaSpinner } from "react-icons/fa";
import { ImageModal } from "../../Components/Modals";
import LikeService from "../../Services/User/LikeService";

const ImageDetails = () => {
  const imageId = useParams();
  const [imageDetails, setImageDetails] = useState({});
  const [liked, setLiked] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  LikeService.LikedImage(imageId.id).then((res) => {
    if (res.message.status === "failed") {
      setLiked(false);
    } else {
      setLiked(true);
    }
  });

  const handleLike = useCallback(async () => {
    try {
      if (liked) {
        await LikeService.UnlikeImage(imageId.id);
        setLiked(false);
      } else {
        await LikeService.LikeImage(imageId.id);
        setLiked(true);
      }
    } catch (error) {
      console.error("Error in like/unlike operation:", error);
    }
  }, [imageId, liked]);

  const openModal = () => {
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
  };

  useMemo(async () => {
    try {
      const result = await ImageService.GetImageByImageId(imageId.id);
      if (result.success) {
        setImageDetails(result.message);
      } else {
        console.error(result.error);
      }
    } catch (error) {
      console.error(error);
    }
  }, [imageId]);

  if (!imageId) {
    return (
      <div className="w-full h-full flex items-center justify-center">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="md:ml-[10rem] w-full">
      <div className="grid grid-cols-1 lg:grid-cols-2 h-full gap-5">
        <div className="flex flex-col h-full">
          <div className="rounded-xl flex flex-col space-y-5">
            <h1 className="font-monospace text-23 leading-30 tracking-wider text-[#9D4EDD] font-bold no-underline italic small-caps capitalize">
              {imageDetails.description}
            </h1>
            <p className="font-monospace text-23 leading-30 tracking-wider text-[#E0AAFF] font-bold no-underline italic small-caps capitalize">
              {imageDetails.title}
            </p>
            <img
              src={`data:image/png;base64,${imageDetails.fileContentBase64}`}
              alt={imageDetails.id}
              className="border border-solid cursor-pointer"
              onClick={openModal}
            />
            <button
              className={`bg-${liked ? "red" : "blue"}-500 hover:bg-${
                liked ? "red" : "blue"
              }-600 text-white w-full p-2 rounded-full`}
              onClick={handleLike}
            >
              {liked ? "🥺 Unlike" : "❤️ Like"}
            </button>
            {modalOpen && (
              <ImageModal imageDetails={imageDetails} closeModal={closeModal} />
            )}
          </div>
          <div className="">
            <h1 className="font-sans text-30 leading-30 text-white font-normal no-underline normal-case small-caps">
              © {imageDetails.username}
            </h1>
          </div>
        </div>
        <div className="w-full h-full p-2 md:mt-20">
          <div className="w-full flex flex-col space-y-7">
            <div className="flex flex-col space-y-5">
              <textarea className="rounded-xl bg-gray-700 p-2 text-white" />
              <button className="text-white w-full p-2 rounded-full bg-[#144552] hover:bg-[#006466]">
                Comment
              </button>
            </div>
            <div className="bg-yellow rounded-lg border-2 border-solid h-full">
              Comments lisn
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ImageDetails;
