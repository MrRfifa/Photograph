import PropTypes from "prop-types";
import { ImageModal } from "../Modals";
import { useCallback, useEffect, useState } from "react";
import LikeService from "../../Services/User/LikeService";
import ImageService from "../../Services/User/ImageService";

const ImageContent = ({ imageId }) => {
  const [liked, setLiked] = useState(false);
  const [imageDetails, setImageDetails] = useState({});
  const [modalOpen, setModalOpen] = useState(false);
  const [numberLikes, setNumberLikes] = useState(0);

  const openModal = () => setModalOpen(true);
  const closeModal = () => setModalOpen(false);
  const handleLike = useCallback(async () => {
    try {
      const likeServiceFunc = liked
        ? LikeService.UnlikeImage
        : LikeService.LikeImage;
      await likeServiceFunc(imageId);
      setLiked(!liked);
      setNumberLikes((prevLikes) => (liked ? prevLikes - 1 : prevLikes + 1));
    } catch (error) {
      console.error("Error in like/unlike operation:", error);
    }
  }, [imageId, liked]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [likedResponse, numberLikesResponse] = await Promise.all([
          LikeService.LikedImage(imageId),
          LikeService.GetNumberLikesPerImage(imageId),
        ]);

        setLiked(likedResponse.message.status !== "failed");
        setNumberLikes(numberLikesResponse.message.message);
      } catch (error) {
        console.error("Error fetching image details:", error);
      }
    };

    const fetchImageDetails = async () => {
      try {
        const result = await ImageService.GetImageByImageId(imageId);
        if (result.success) {
          setImageDetails(result.message);
        } else {
          console.error(result.error);
        }
      } catch (error) {
        console.error("Error fetching image details:", error);
      }
    };

    fetchData();
    fetchImageDetails();
  }, [imageId]);
  return (
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
        <div className="flex flex-row justify-between">
          <button
            className={`bg-${liked ? "red" : "blue"}-500 hover:bg-${
              liked ? "red" : "blue"
            }-600 text-white p-2 rounded-full w-[80%]`}
            onClick={handleLike}
          >
            {liked ? "🥺 Unlike" : "❤️ Like"}
          </button>
          <p className="w-[15%] bg-[#22577a] rounded-full text-2xl text-center py-1 text-white">
            {numberLikes}🤍
          </p>
        </div>
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
  );
};

export default ImageContent;

ImageContent.propTypes = {
  imageId: PropTypes.string.isRequired,
};
